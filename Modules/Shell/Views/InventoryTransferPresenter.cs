using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;
using System.Web;

namespace VCTWebApp.Shell.Views
{
    public class InventoryTransferPresenter : Presenter<IInventoryTransferView>
    {
        private InventoryStockRepository InventoryStockRepositoryService;        
        private Helper helper = new Helper();
        private DictionaryRepository DictionaryRepositoryService;
        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IShellController _controller;
        // public KitListingPresenter([CreateNew] IShellController controller)
        // {
        // 		_controller = controller;
        // }

        #region Constructors

        public InventoryTransferPresenter()
            : this(new InventoryStockRepository(HttpContext.Current.User.Identity.Name))
        {
        }

        public InventoryTransferPresenter(InventoryStockRepository userRepository)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "Inventory Transfer Presenter", "Constructor is invoked.");

            this.InventoryStockRepositoryService = new InventoryStockRepository();
            this.DictionaryRepositoryService = new DictionaryRepository();
        }

        #endregion

        #region Public Overrides
        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed every time the view loads
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "VirtualCheckInPresenter", "OnViewInitialized() is invoked.");
            try
            {
                PopulateFromLocation();
                PopulateInventoryNotInUseDays();                
            }
            catch
            {
                throw;
            }

            helper.LogInformation(HttpContext.Current.User.Identity.Name, "VirtualCheckInPresenter", "OnViewInitialized() is completed");
        }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "VirtualCheckInPresenter", "OnViewLoaded() is invoked.");

            try
            {
                if (View.InventoryType == VCTWeb.Core.Domain.Constants.InventoryType.Part)
                {
                    List<InventoryTransfer> lstInventoryTransfer =InventoryStockRepositoryService.PopulateUnUtilizeInventoryParts(View.FromLocationId, View.InventoryDays);
                    
                    if (lstInventoryTransfer.Count > 0)
                        View.InventoryTransPartList = lstInventoryTransfer;
                    else
                    {
                        lstInventoryTransfer = new List<InventoryTransfer>();
                        lstInventoryTransfer.Add(new InventoryTransfer());
                        View.InventoryTransPartList = lstInventoryTransfer;
                    }
                }
                else if (View.InventoryType == VCTWeb.Core.Domain.Constants.InventoryType.Kit)
                {
                    //View.InventoryTransKitList = InventoryStockRepositoryService.PopulateUnUtilizeInventoryKits(View.FromLocationId, View.InventoryDays);
                    List<InventoryTransfer> lstInventoryTransfer = InventoryStockRepositoryService.PopulateUnUtilizeInventoryKits(View.FromLocationId, View.InventoryDays);
                    
                    if (lstInventoryTransfer != null && lstInventoryTransfer.Count == 0)                    
                    {
                        lstInventoryTransfer = new List<InventoryTransfer>();
                        lstInventoryTransfer.Add(new InventoryTransfer());                        
                    }

                    View.InventoryTransKitList = lstInventoryTransfer;
                }

                helper.LogInformation(HttpContext.Current.User.Identity.Name, "VirtualCheckInPresenter", "OnViewLoaded() is completed");

            }
            catch
            {
                throw;
            }

        }     
        #endregion

        #region Public Methods
        
        public void PopulateEmptyKitTable()
        {
            List<KitFamilyParts> emptyKitTableList = new List<KitFamilyParts>();
            emptyKitTableList.Add(new KitFamilyParts());
            View.KitFamilyPartsList = emptyKitTableList;
        }

        public bool Save(out string result)
        {             
            bool status = false;
            try
            {
                status = InventoryStockRepositoryService.SaveInventoryTransfer(View.FromLocationId, View.InventoryType.ToString(), View.TableXml, HttpContext.Current.User.Identity.Name, out result);                
                if (status)
                {
                    OnViewInitialized();
                    OnViewLoaded();
                }
            }
            catch
            {
                throw;
            }

            return status;
        }

        public void PopulateFromLocation()
        {
            if (Convert.ToString(HttpContext.Current.Session["LoggedInLocationType"]).ToUpper() == "AREA")
            {
                View.RegionBranchList = PopulateRegion();
            }
            else if (Convert.ToString(HttpContext.Current.Session["LoggedInLocationType"]).ToUpper() == "REGION")
            {
                View.RegionBranchList = PopulateBranch();
            }
        }
             
        #endregion

        #region Private Methods
        private List<InventoryTransfer> PopulateRegion()
        {
            List<InventoryTransfer> lstInventoryTransfer = null;
            List<Region> lstRegion = new AddressRepository().FetchRegions(Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"]));

            if (lstRegion != null)
            {
                lstInventoryTransfer = new List<InventoryTransfer>(); ;
                foreach (Region item in lstRegion)
                {
                    lstInventoryTransfer.Add(new InventoryTransfer()
                    {
                        LocationId = item.RegionId,
                        LocationName = item.RegionName
                    });
                }
            }
            return lstInventoryTransfer;
        }

        private List<InventoryTransfer> PopulateBranch()
        {
            List<InventoryTransfer> lstInventoryTransfer = null;
            List<SalesOffice> lstBranch = new AddressRepository().FetchSalesOffices(Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"]));

            if (lstBranch != null)
            {
                lstInventoryTransfer = new List<InventoryTransfer>(); ;
                foreach (SalesOffice item in lstBranch)
                {
                    lstInventoryTransfer.Add(new InventoryTransfer()
                    {
                        LocationId = item.LocationId,
                        LocationName = item.LocationName
                    });
                }
            }
            return lstInventoryTransfer;
        }

        private void PopulateInventoryNotInUseDays()
        { 
            Dictionary oDictionary = DictionaryRepositoryService.GetDictionaryRule("InventoryUtilizationDays");
            View.InventoryDays = Convert.ToInt32(oDictionary.KeyValue);
        }
        #endregion

    }
}
