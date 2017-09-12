using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;
using System.Web;

namespace VCTWebApp.Shell.Views
{
    
    public class VirtualBuildKitPresenter : Presenter<IVirtualBuildKitView>
    {

        #region Instance Variables

        private AssetRepository AssetRepositoryService;
        private DictionaryRepository dictionaryRepository;

        private Helper helper = new Helper();

        #endregion

        #region Constructors

        public VirtualBuildKitPresenter()
            : this(new AssetRepository(HttpContext.Current.User.Identity.Name))
        {
        }

        public VirtualBuildKitPresenter(AssetRepository virtualBuildKitRepository)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "KitFamilyPresenter", "Constructor is invoked.");

            this.AssetRepositoryService = virtualBuildKitRepository;
            this.dictionaryRepository = new DictionaryRepository();
        }

        #endregion

        #region Private Methods

        private void SetFieldsBlank()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "KitFamilyPresenter", "SetFieldsBlank() is invoked.");
            View.KitNumber = string.Empty;
            //View.KitNumberDesc = string.Empty;
        }

        private void PopulateEmptyVirtualKitList()
        {
            List<VirtualBuilKit> lstVirtualBuildKit = new List<VirtualBuilKit>();
            lstVirtualBuildKit.Add(new VirtualBuilKit());
            View.VirtualBuildKitList = null;
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
                PopulateEmptyVirtualKitList();
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

        public bool SaveVirtualBuildKit()
        //public Constants.ResultStatus SaveVirtualBuildKit()
        {
            bool result;
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "VirtualBuildKitPresenter", "Save() is invoked.");

            //Constants.ResultStatus resultStatus = Constants.ResultStatus.Error;
            try
            {                
                VirtualBuilKit oVirtualBuildKit = new VirtualBuilKit();
                oVirtualBuildKit.KitFamilyId = View.KitFamilyId;
                oVirtualBuildKit.KitNum = View.KitNumber;
                //oVirtualBuildKit.Description = View.KitNumberDesc;
                oVirtualBuildKit.LocationId = View.LocationId;

                result = this.AssetRepositoryService.SaveVirtualBuildKit(oVirtualBuildKit, View.VirtualBuildKitTableXml);
                
                helper.LogInformation(HttpContext.Current.User.Identity.Name, "VirtualBuildKitPresenter", "Virtual Build Kit '" + View.KitNumber + "' is saved successfully.");
                                    
                //resultStatus = Constants.ResultStatus.Updated;
                    
            }
            catch
            {
                throw;
            }

            //return resultStatus;
            return result;
        }

        public void PopulateLotNumbers()
        {
            View.LotNumberList = AssetRepositoryService.GetLotNumbersToAssigned(View.KitNumber, View.PartNum, View.LocationId);
        }

        public void PopulateVirtualBuildKitList()
        {            
            //View.KitNumberDesc = AssetRepositoryService.GetKitNumberDescByKitNumber(View.KitNumber);
            View.VirtualBuildKitList = AssetRepositoryService.GetVirtualBuilKitList(View.KitNumber, View.LocationId);
            
            if (View.VirtualBuildKitList.Count > 0)
            {
                View.KitFamilyId = View.VirtualBuildKitList[0].KitFamilyId;
            }

            View.SelectedBuildKitList = AssetRepositoryService.GetSelectBuildKitByKitNumber(View.KitNumber, View.LocationId);            
        }

        #endregion

    }

}
