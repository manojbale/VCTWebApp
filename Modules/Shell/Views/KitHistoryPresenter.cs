using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;
using System.Web;

namespace VCTWebApp.Shell.Views
{

    public class KitHistoryPresenter : Presenter<IKitHistoryView>
    {

        #region Instance Variables


        private Helper helper = new Helper();

        #endregion

        #region Constructors

        public KitHistoryPresenter()
        {
        }

        #endregion

        #region Private Methods

        private void SetFieldsBlank()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "KitHistoryPresenter", "SetFieldsBlank() is invoked.");
            View.KitNumber = string.Empty;
            //View.KitNumberDesc = string.Empty;
        }

        private void PopulateEmptyKitHistoryCaseDetail()
        {
            List<KitHistoryCaseDetail> lstKitHistoryCaseDetail = new List<KitHistoryCaseDetail>();
            lstKitHistoryCaseDetail.Add(new KitHistoryCaseDetail());
            //View.KitHistoryCaseDetailList = lstKitHistoryCaseDetail;
            View.KitHistoryCaseDetailList = null;
        }

        #endregion

        #region Public Overrides

        public override void OnViewLoaded()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "KitFamilyPresenter", "OnViewLoaded is invoked.");

            try
            {
                ////View.KitFamilyList = this.kitFamilyRepositoryService.FetchAllKitFamily();
                //KitFamily kitFamily = this.GetSelectedKitFamily();
                //if (kitFamily != null)
                //{
                //    View.Name = kitFamily.KitFamilyName.Trim();
                //    View.Description = kitFamily.KitFamilyDescription.Trim();
                //    View.NumberOfTubs = kitFamily.NumberOfTubs;
                //    View.KitType = kitFamily.KitTypeName.Trim();
                //    View.Active = kitFamily.IsActive;

                //    PopulateKitFamilyPartsList();
                //    PopulateKitFamilyLocationsList();
                //}

            }
            catch
            {
                throw;
            }
        }

        public override void OnViewInitialized()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "KitFamilyPresenter", "OnViewInitialized() is invoked.");
            try
            {
                PopulateEmptyKitHistoryCaseDetail();
                SetFieldsBlank();
                //View.KitFamilyList = this.kitFamilyRepositoryService.FetchAllKitFamily();
                //this.PopulateKitTypes();
                //PopulateKitFamilyLocationsList();
                //PopulateEmptyKitTable();
                //this.SetFieldsBlank();
            }
            catch
            {
                throw;
            }
        }

        //public void PopulateEmptyKitTable()
        //{
        //    List<KitFamilyParts> emptyKitTableList = new List<KitFamilyParts>();
        //    emptyKitTableList.Add(new KitFamilyParts());
        //    View.KitFamilyPartsList = emptyKitTableList;
        //}

        #endregion

        #region Public Methods

        public void PopulateKitHistoryCaseDetailList()
        {            
            View.KitHistoryCaseDetailList = new CaseRepository().GetKitHistoryByKitNumberAndLocationId(View.KitNumber, Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"]));
        }

        #endregion

    }

}
