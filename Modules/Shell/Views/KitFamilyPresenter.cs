using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;
using System.Web;

namespace VCTWebApp.Shell.Views
{
    public class KitFamilyPresenter : Presenter<IKitFamilyView>
    {
        #region Instance Variables

        private KitFamilyRepository kitFamilyRepositoryService;
        private DictionaryRepository dictionaryRepository;

        private Helper helper = new Helper();

        #endregion

        #region Constructors

        public KitFamilyPresenter()
            : this(new KitFamilyRepository(HttpContext.Current.User.Identity.Name))
        {
        }

        public KitFamilyPresenter(KitFamilyRepository kitFamilyRepository)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "KitFamilyPresenter", "Constructor is invoked.");

            this.kitFamilyRepositoryService = kitFamilyRepository;
            this.dictionaryRepository = new DictionaryRepository();
        }

        #endregion

        #region Private Methods

        private void SetFieldsBlank()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "KitFamilyPresenter", "SetFieldsBlank() is invoked.");

            View.Name = string.Empty;
            View.NumberOfTubs = 0;
            View.KitType = "0";
            View.Description = string.Empty;
            View.Active = true;
        }

        private void PopulateKitTypes()
        {
            View.KitTypeList = this.kitFamilyRepositoryService.FetchAllKitTypes();
        }

        private KitFamily GetSelectedKitFamily()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "KitFamilyPresenter", "GetSelectedKitFamily() is invoked for SelectedKitFamilyId: " + View.SelectedKitFamilyId);

            try
            {
                //return View.KitFamilyList.Find(delegate(KitFamily kitFamilyInList) { return kitFamilyInList.KitFamilyId == View.SelectedKitFamilyId; });
                return kitFamilyRepositoryService.GetKitFamilyByKitFamilyId(View.SelectedKitFamilyId);
            }
            catch
            {
                return null;
            }
        }

        private Constants.ResultStatus CheckStatusKitFamily(string kitFamily, long kitFamilyId)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "KitFamilyPresenter", "CheckDuplicateKitFamily() is invoked with parameters values - Kit Family: " + kitFamily + " and Kit Family Id: " + kitFamilyId.ToString());
            if (kitFamilyRepositoryService.CheckDuplicateKitFamily(kitFamily, kitFamilyId))
            {
                return Constants.ResultStatus.DuplicateKitFamily;
            }
            if (!View.Active && kitFamilyRepositoryService.CheckInUse(kitFamilyId))
            {
                View.Active = true;
                return Constants.ResultStatus.InUse;
            }
            return Constants.ResultStatus.Ok;
        }

        public void PopulateKitFamilyPartsList()
        {
            View.KitFamilyPartsList = this.kitFamilyRepositoryService.GetKitFamilyPartsById(View.SelectedKitFamilyId);
            //return this.kitFamilyRepositoryService.GetKitFamilyPartsById(View.SelectedKitFamilyId);            
        }

        public void PopulateKitFamilyLocationsList()
        {
            View.KitFamilyLocationList = this.kitFamilyRepositoryService.GetKitFamilyLocationsById(View.SelectedKitFamilyId);
        }
      
        #endregion

        #region Public Overrides

        public override void OnViewLoaded()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "KitFamilyPresenter", "OnViewLoaded is invoked.");

            try
            {
                //View.KitFamilyList = this.kitFamilyRepositoryService.FetchAllKitFamily();
                KitFamily kitFamily = this.GetSelectedKitFamily();
                if (kitFamily != null)
                {
                    View.Name = kitFamily.KitFamilyName.Trim();
                    View.Description = kitFamily.KitFamilyDescription.Trim();
                    View.NumberOfTubs = kitFamily.NumberOfTubs;
                    View.KitType = kitFamily.KitTypeName.Trim();
                    View.Active = kitFamily.IsActive;
                    PopulateKitFamilyPartsList();
                    PopulateKitFamilyLocationsList();
                }
                                
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
                View.KitFamilyList = this.kitFamilyRepositoryService.FetchAllKitFamily();
                this.PopulateKitTypes();
                PopulateKitFamilyLocationsList();
                PopulateEmptyKitTable();
                this.SetFieldsBlank();
            }
            catch
            {
                throw;
            }
        }

        public void PopulateEmptyKitTable()
        {
            List<KitFamilyParts> emptyKitTableList = new List<KitFamilyParts>();
            emptyKitTableList.Add(new KitFamilyParts());
            View.KitFamilyPartsList = emptyKitTableList;            
        }
        #endregion

        #region Public Methods

        public Constants.ResultStatus Save()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "KitFamilyPresenter", "Save() is invoked.");

            Constants.ResultStatus resultStatus = Constants.ResultStatus.Error;
            try
            {
                resultStatus = this.CheckStatusKitFamily(View.Name.Trim(), View.SelectedKitFamilyId);
                if (resultStatus == Constants.ResultStatus.Ok)
                {
                    KitFamily kitFamily = new KitFamily();
                    kitFamily.KitFamilyId = View.SelectedKitFamilyId;
                    kitFamily.KitFamilyName = View.Name;
                    kitFamily.KitFamilyDescription = View.Description;
                    kitFamily.NumberOfTubs = View.NumberOfTubs;
                    kitFamily.KitTypeName = View.KitType;
                    kitFamily.IsActive = View.Active;

                    //this.kitFamilyRepositoryService.SaveKitFamily(kitFamily);
                    this.kitFamilyRepositoryService.SaveKitFamily(kitFamily, View.KitFamilyPartTableXml, View.KitFamilyLocationTableXml);
                    helper.LogInformation(HttpContext.Current.User.Identity.Name, "KitFamilyPresenter", "Kit Family '" + kitFamily.KitFamilyName + "' is saved successfully.");

                    if (kitFamily.KitFamilyId == 0)
                        resultStatus = Constants.ResultStatus.Created;
                    else
                        resultStatus = Constants.ResultStatus.Updated;
                }
            }
            catch
            {
                throw;
            }

            return resultStatus;
        }

        public Dictionary GetDictionaryRule(string key)
        {
            return this.dictionaryRepository.GetDictionaryRule(key);
        }

        public bool AddPartyPARLevelQuantity(string partNum, int qty)
        {
            KitFamilyParts oModel = new KitFamilyParts();
            oModel.KitFamilyId = View.SelectedKitFamilyId;
            oModel.CatalogNumber = partNum;
            oModel.Quantity = qty;

            return kitFamilyRepositoryService.SaveKitFamilyPartDetail(oModel);            
        }

        public bool UpdateKitFamilyItemQuantity(Int64 KitFamilyItemId, int qty)
        {
            KitFamilyParts oModel = new KitFamilyParts();
            oModel.KitFamilyItemId = KitFamilyItemId;            
            oModel.Quantity = qty;

            return kitFamilyRepositoryService.UpdateKitFamilyPartQtyById(oModel);
        }


        public bool DeleteKitFamilyPartById(Int64 KitFamilyItemId)
        {
            return kitFamilyRepositoryService.DeleteKitFamilyPartById(KitFamilyItemId);
        }

        #endregion
    }
}




