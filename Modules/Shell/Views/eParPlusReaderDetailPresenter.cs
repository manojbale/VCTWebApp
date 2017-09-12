using System;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public class eParPlusReaderDetailPresenter : Presenter<IeParPlusReaderDetailView>
    {
        #region Instance Variables
        private readonly CustomerShelfRepository _customerShelfRepository;
        private readonly Helper _helper = new Helper();
        #endregion

        #region Constructors

        public eParPlusReaderDetailPresenter()
            : this(new CustomerShelfRepository())
        {
        }

        public eParPlusReaderDetailPresenter(CustomerShelfRepository customerShelfRepository)
        {
            _helper.LogInformation(HttpContext.Current.User.Identity.Name, "ReaderDetailPresenter", "Constructor is invoked.");
            _customerShelfRepository = customerShelfRepository;
        }

        #endregion

        #region Public Overrides

        public override void OnViewInitialized()
        {
            _helper.LogInformation(HttpContext.Current.User.Identity.Name, "eParPlusReaderDetailPresenter ", "OnViewInitialized() is invoked.");
            SetDefaultValues();
        }

        #endregion

        private void SetDefaultValues()
        {
            View.ListOfCustomerShelfProperty = _customerShelfRepository.FetchCustomerShelfPropertyByCustomerShelfId(View.CustomerShelfId);
            var listDistinctAntenna = _customerShelfRepository.FetchDistinctAntennaByCustomerShelfId(View.CustomerShelfId);
            var totalAntenna = listDistinctAntenna.Count;
            var firstItem = new CustomerShelfAntenna
            {
                CustomerShelfAntennaId = -1,
                AntennaName = "--Select--"
            };
            listDistinctAntenna.Insert(0, firstItem);
            View.ListOfDistinctAntenna = listDistinctAntenna;
            if (listDistinctAntenna.Any())
            {
                View.SelectedCustomerShelfAntennaId = listDistinctAntenna[0].CustomerShelfAntennaId;
                GetAntennaProperties();
            }
            var customerShelf = _customerShelfRepository.FetchCustomerShelfByCustomerShelfId(View.CustomerShelfId);
            if (customerShelf != null)
            {
                View.AccountNumber = customerShelf.AccountNumber;
                View.ShelfName = customerShelf.ShelfName;
                View.ShelfCode = customerShelf.ShelfCode;
                View.ReaderHealthLastUpdatedOn = Convert.ToString(customerShelf.ReaderHealthLastUpdatedOn);
                View.ReaderIP = customerShelf.ReaderIP;
                View.TotalAntenna = Convert.ToString(totalAntenna);
            }
        }

        public void GetAntennaProperties()
        {
            View.ListOfCustomerShelfAntennaProperty = _customerShelfRepository.FetchCustomerShelfAntennaPropertyByCustomerShelfAntennaId(View.SelectedCustomerShelfAntennaId);
        }

        public bool SaveModifiedReaderAntennaValues()
        {
            var readerPropertyXml = new StringBuilder();
            var antennaPropertyXml = new StringBuilder();

            #region Create XML File of Modified Reader Properties
            if (View.ListOfCustomerShelfProperty.Any())
            {
                readerPropertyXml.Append("<root>");
                foreach (var customerShelfProperty in View.ListOfCustomerShelfProperty)
                {
                    if (customerShelfProperty.IsEditable)
                    {
                        if (customerShelfProperty.PropertyValue.Trim().ToUpper() != customerShelfProperty.ModifiedPropertyValue.Trim().ToUpper())
                        {
                            readerPropertyXml.Append("<ReaderPropertyDetail>");
                            readerPropertyXml.Append("<PropertyName>" + customerShelfProperty.PropertyName + "</PropertyName>");
                            readerPropertyXml.Append("<PropertyValue>" + customerShelfProperty.ModifiedPropertyValue + "</PropertyValue>");
                            readerPropertyXml.Append("</ReaderPropertyDetail>");
                        }
                    }
                }
                readerPropertyXml.Append("</root>");
            }
            #endregion


            //#region Create XML File of Modified Reader Antenna Properties
            //if (View.ListOfCustomerShelfAntennaProperty.Any())
            //{
            //    antennaPropertyXml.Append("<root>");
            //    foreach (var customerShelfAntennaProperty in View.ListOfCustomerShelfAntennaProperty)
            //    {
            //        if (customerShelfAntennaProperty.IsEditableAntenna)
            //        {
            //            if (customerShelfAntennaProperty.PropertyValueAntenna.Trim().ToUpper() != customerShelfAntennaProperty.ModifiedPropertyValueAntenna.Trim().ToUpper())
            //            {
            //                antennaPropertyXml.Append("<AntennaPropertyDetail>");
            //                //antennaPropertyXml.Append("<AntennaName>" + customerShelfAntennaProperty.AntennaName + "</AntennaName>");
            //                antennaPropertyXml.Append("<PropertyName>" + customerShelfAntennaProperty.PropertyNameAntenna + "</PropertyName>");
            //                antennaPropertyXml.Append("<PropertyValue>" + customerShelfAntennaProperty.ModifiedPropertyValueAntenna + "</PropertyValue>");
            //                antennaPropertyXml.Append("</AntennaPropertyDetail>");
            //            }
            //        }
            //    }
            //    antennaPropertyXml.Append("</root>");
            //}
            //#endregion

            return _customerShelfRepository.SaveModifiedReaderAntennaValues(View.AccountNumber, View.ShelfCode, Convert.ToString(readerPropertyXml), Convert.ToString(antennaPropertyXml));
        }
    }
}
