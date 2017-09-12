using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;
using System.Web;

namespace VCTWebApp.Shell.Views
{
    public class NewProductTransferPresenter : Presenter<INewProductTransfer>
    {
        
        private Helper helper = new Helper();

        private CaseRepository caseRepositoryService;
        private KitFamilyRepository kitFamilyRepositoryService;
        //private PartyRepository partyRepositoryService;
        //private CaseRepository caseRepositoryService;
        //private KitListingRepository kitListingRepositoryService;        
        

        #region Constructors

        public NewProductTransferPresenter()
            : this(new CaseRepository(HttpContext.Current.User.Identity.Name))
        {
        }

        public NewProductTransferPresenter(CaseRepository userRepository)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "AugmentTransferPresenter", "Constructor is invoked.");
            kitFamilyRepositoryService = new KitFamilyRepository();
            //partyRepositoryService = new PartyRepository();
            caseRepositoryService = new CaseRepository(HttpContext.Current.User.Identity.Name);
            //kitListingRepositoryService = new KitListingRepository();
        }

        #endregion

        #region Public Overrides
        public override void OnViewInitialized()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "KitFamilyPresenter", "OnViewInitialized() is invoked.");
            try
            {
                SetFieldsBlanks();
                //PopulateKitFamilyList();
                //PopulateEmptyKitFamilyLocations();
                //PopulateEmptyKitPartsTable();                
            }
            catch
            {
                throw;
            }
        }
        
        public override void OnViewLoaded()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "KitFamilyPresenter", "OnViewLoaded is invoked.");

            try
            {                
                PopulateKitFamilyParts();
                PopulateKitFamilyLocations();
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Private Methods
        private void PopulateKitFamilyList()
        {
            View.KitFamilyList = new VCTWeb.Core.Domain.KitFamilyRepository().FetchAllKitFamilyByLocationId(View.LocationId);
        }             

        private void PopulateKitFamilyParts()
        {
            //List<KitFamilyParts> oKitFamilyParts = kitFamilyRepositoryService.GetKitFamilyPartsByName(View.KitFamilyName);
            List<KitFamilyParts> oKitFamilyParts = kitFamilyRepositoryService.GetKitFamilyPartsById(View.KitFamilyId);
            
            if (oKitFamilyParts.Count > 0)
                View.KitPartsList = oKitFamilyParts;
            else
                PopulateEmptyKitPartsTable();
        }        
        #endregion

        #region Public Methods
        public void SetFieldsBlanks()
        {            
            PopulateEmptyKitPartsTable();
            PopulateKitFamilyLocations();
        }

        public void PopulateKitFamilyLocations()
        {
            if (View.KitFamilyId > 0)
            {
                List<NewProductTransfer> KitFamilyLocationList = this.kitFamilyRepositoryService.GetKitFamilyLocationsByParentLocationId(View.KitFamilyId);
                KitFamilyLocationList.Remove(KitFamilyLocationList.Find(n => n.LocationId == View.LocationId));
                View.KitFamilyLocationList = KitFamilyLocationList;
            }
            else
            {
                List<NewProductTransfer> lstLocation = new List<NewProductTransfer>();
                lstLocation.Add(new NewProductTransfer());
                View.KitFamilyLocationList = lstLocation;
            }
        }

        public void PopulateEmptyKitFamilyLocations()
        {
            List<NewProductTransfer> lstProductTransfer = new List<NewProductTransfer>();
            lstProductTransfer.Add(new NewProductTransfer());
            View.KitFamilyLocationList = lstProductTransfer;
        }

        public void PopulateEmptyKitPartsTable()
        {
            List<KitFamilyParts> emptyKitTableList = new List<KitFamilyParts>();
            emptyKitTableList.Add(new KitFamilyParts());
            View.KitPartsList = emptyKitTableList;
        }

        public bool SaveNewProductTransfer(out string result)
        {            
            bool status = caseRepositoryService.SaveNewProductTransfer(View.KitFamilyId, View.LocationId, View.TransDate, View.KitFamilyLocationsTableXML, View.KitFamilyPartsTableXML, out result);
            return status;
        }
        #endregion
    }
}
