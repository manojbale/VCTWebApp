using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;
using System.Web;

namespace VCTWebApp.Shell.Views
{
    public class KitListingPresenter : Presenter<IKitListingView>
    {
        private KitListingRepository kitListingRepositoryService;
        private AddressRepository addressRepositoryService;
        private Helper helper = new Helper();

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IShellController _controller;
        // public KitListingPresenter([CreateNew] IShellController controller)
        // {
        // 		_controller = controller;
        // }

        #region Constructors

        public KitListingPresenter()
            : this(new KitListingRepository(HttpContext.Current.User.Identity.Name))
        {
        }

        public KitListingPresenter(KitListingRepository userRepository)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "KitListingPresenter", "Constructor is invoked.");

            this.kitListingRepositoryService = new KitListingRepository();
            this.addressRepositoryService = new AddressRepository();
        }

        #endregion

        #region Public Method
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "KitListingPresenter", "OnViewLoaded() is invoked.");
            try
            {                
                KitListing KitListing = this.GetSelectedKitListing();
                if (KitListing != null)
                {
                    View.KitDescription = KitListing.KitDescription;                    
                    View.KitNumber = KitListing.KitNumber;
                    View.KitName = KitListing.KitName;
                    View.KitFamilyId = KitListing.KitFamilyId;
                    View.IsActive = KitListing.IsActive;
                    //View.LocationId = KitListing.LocationId;
                    View.Procedure = KitListing.Procedure;
                    View.RentalFee = KitListing.RentalFee;
                    View.KitFamily = KitListing.KitFamily;
                    //View.KitListingList = this.bomRepositoryService.FetchCatalogNumbersByBOMId(KitListing.BOMId);
                }

                helper.LogInformation(HttpContext.Current.User.Identity.Name, "KitListingPresenter", "OnViewLoaded() is completed");
            }
            catch
            {
                throw;
            }
        }               

        public void PopulateEmptyKitTable()
        {
            List<VCTWeb.Core.Domain.KitTable> emptyKitTableList = new List<VCTWeb.Core.Domain.KitTable>();
            emptyKitTableList.Add(new VCTWeb.Core.Domain.KitTable());
            View.KitTableList = emptyKitTableList;
        }

        public override void OnViewInitialized()
        {
            PopulateKitFamilyList();
            PopulateKitListingList();            
            PopulateEmptyKitTable();
            //this.PopulateProcedures();
        }
        
        public void PopulateKitListingList()
        {

            View.KitListingList = this.kitListingRepositoryService.GetKitListingByLocationIdAndKitFamilyId(View.SelectedKitFamilyId, View.LocationId);
            //View.KitListingList = this.kitListingRepositoryService.GetKitListingByLocationIdAndKitFamilyId(View.KitFamilyHead, View.LocationId);
        }

        public bool IsKitNumberDuplicate(string kitNumber)
        {
            return kitListingRepositoryService.IsKitNumberDuplicate(kitNumber);
        }

        public bool SaveKitListing()
        {
            KitListing kitListingToBeSaved = GetKitListingToBeSaved();
            return kitListingRepositoryService.SaveKitListing(kitListingToBeSaved, View.KitTableXml);
        }

        public void PopulateKitTableList()
        {
            //View.KitTableList = new KitTableRepository().GetKitTableByKitNumber(View.SelectedKitNumber);
            List<KitTable> TmpKitTableList = kitListingRepositoryService.usp_GetKitTableListByKitFamilyId(View.KitFamilyId);            
            List<KitTable> KitTableList = PopulateKitTable(TmpKitTableList);

            List<KitTable> lstKitTableSelected = kitListingRepositoryService.GetKitTableListByKitNumber(View.KitNumber);

            if (lstKitTableSelected.Count > 0)
            {
                PopulateSelectedKitTable(KitTableList, lstKitTableSelected);
            }

            View.KitTableList = KitTableList;

            if (View.KitTableList.Count == 0)
            {
                PopulateEmptyKitTable();
            }
        }

        public void PopulateKitFamilyList()
        {
            View.KitFamilyList = new VCTWeb.Core.Domain.KitFamilyRepository().FetchAllKitFamilyByLocationId(View.LocationId);
        }

        public bool UpdateIndiviualKitItems()
        {
            KitTable kitTable = View.IndiviualKitItem;
            return new KitTableRepository().ModifyKitTable(kitTable, "Update");
        }

        public bool AddIndiviualKitItems()
        {
            KitTable kitTable = View.IndiviualKitItem;
            return new KitTableRepository().ModifyKitTable(kitTable, "Add");
        }

        public bool DeleteIndiviualKitItems()
        {
            KitTable kitTable = View.IndiviualKitItem;
            return new KitTableRepository().ModifyKitTable(kitTable, "Delete");
        }

        public bool Delete()
        {
            return kitListingRepositoryService.DeleteKitListing(View.SelectedKitNumber);
        }

        public void SetFieldsBlanks()
        {
            View.KitNumber = string.Empty;
            View.KitName = string.Empty;
            View.KitFamilyId = null;
            View.KitDescription = string.Empty;
            View.IsActive = true;
            View.Procedure = string.Empty;

            PopulateEmptyKitTable();
        }
        #endregion

        #region Private Method
        private KitListing GetSelectedKitListing()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "KitListingPresenter", "GetSelectedKitListing() is invoked for SelectedKitListing: " + View.KitNumber.ToString());

            try
            {
                return kitListingRepositoryService.GetKitByKitNumber(View.SelectedKitNumber);
            }
            catch
            {
                return null;
            }
        }

        //private void PopulateProcedures()
        //{
        //    View.ProcedureList = new ProceduresRepository().FetchAllProcedures();
        //}

        // TODO: Handle other view events and set state in the view
        
        private KitListing GetKitListingToBeSaved()
        {
            KitListing newKitListing = new KitListing();
            newKitListing.KitNumber = View.KitNumber;
            newKitListing.KitName = View.KitName;
            newKitListing.KitDescription = View.KitDescription;
            newKitListing.NumberOfSets = View.NumberOfSets;
            newKitListing.Aisle = View.Aisle;
            newKitListing.Row = View.Row;
            newKitListing.Tier = View.Tier;
            newKitListing.DateCreated = View.DateCreated;
            newKitListing.PMSchedule = View.PMSchedule;
            newKitListing.Lubricate = View.Lubricate;
            newKitListing.IsManuallyAdded = View.IsManuallyAdded;
            newKitListing.KitFamilyId = View.KitFamilyId;            
            newKitListing.LocationId = View.LocationId;
            newKitListing.UpdatedBy = Convert.ToString(HttpContext.Current.Session["LoggedInUser"]);
            // if ALL option is selected then null is assigned to locationId.
            if (View.LocationId == -1)
            {
                newKitListing.LocationId = null;
            }
            newKitListing.RentalFee = View.RentalFee;
            newKitListing.IsActive = View.IsActive;
            newKitListing.Procedure = View.Procedure;

            return newKitListing;
        }
                       
        private List<KitTable> PopulateKitTable(List<KitTable> lst)
        {
            List<KitTable> KitTableList = new List<KitTable>();
            
            foreach (KitTable item in lst)
            {                
                for (int i = 0; i < item.Quantity; i++)
                {
                    //ctr += 1;
                    KitTableList.Add(new KitTable() { Catalognumber = item.Catalognumber, Description = item.Description });                    
                }
            }

            return KitTableList;
        }

        private void PopulateSelectedKitTable(List<KitTable> lst, List<KitTable> lstSelected)
        {
            int ctr = 0;
            foreach (KitTable selItem in lstSelected)
            {
                int index = lst.FindLastIndex(x => (x.Catalognumber == selItem.Catalognumber && x.ItemNumber == null) );
                if (index != -1)
                {
                    lst[index].KitNumber = selItem.KitNumber;
                    lst[index].ItemNumber = 1;
                }
            }

        }
        #endregion

    }
}




