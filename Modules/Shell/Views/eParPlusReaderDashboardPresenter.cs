using System.Collections.Generic;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;
using System.Web;
using System;

namespace VCTWebApp.Shell.Views
{
    public class eParPlusReaderDashboardPresenter : Presenter<IeParPlusReaderDashboard>
    {
        #region Instance Variables
        private readonly CustomerShelfRepository _customerShelfRepository;
        private readonly DictionaryRepository _dictionaryRepository;
        private readonly Helper _helper = new Helper();
        #endregion

        #region Constructors

        public eParPlusReaderDashboardPresenter()
            : this(new CustomerShelfRepository())
        {
        }

        private eParPlusReaderDashboardPresenter(CustomerShelfRepository customerShelfRepository)
        {
            _helper.LogInformation(HttpContext.Current.User.Identity.Name, "eParPlusReaderDashboardPresenter", "Constructor is invoked.");
            _customerShelfRepository = customerShelfRepository;
            _dictionaryRepository = new DictionaryRepository();
        }

        #endregion

        #region Public Overrides

        public override void OnViewInitialized()
        {
            _helper.LogInformation(HttpContext.Current.User.Identity.Name, "eParPlusLowInventoryReportPresenter", "OnViewInitialized() is invoked.");
            SetDefaultValues();
        }

        #endregion

        #region Private Methods

        private void SetDefaultValues()
        {
            View.ListCustomerShelf = new List<CustomerShelf>();
            FetchCustomerShelfForDashBoard();
        }

        #endregion Private Methods

        #region Public Methods

        private void FetchCustomerShelfForDashBoard()
        {
            View.ListCustomerShelf = _customerShelfRepository.FetchCustomerShelfForDashBoard();
        }
        
        public void FetchColumnPerRowInDashboard()
        {
            try
            {
                Dictionary oDictionary = _dictionaryRepository.GetDictionaryRule("ColumnPerRowInDashboard");
                if (oDictionary != null)
                {
                    View.ColumnPerRowInDashboard = Convert.ToInt32(oDictionary.KeyValue);
                }
            }
            catch 
            {
                View.ColumnPerRowInDashboard = 10;
            }
        }
        
        #endregion Public  Methods

    }
}
