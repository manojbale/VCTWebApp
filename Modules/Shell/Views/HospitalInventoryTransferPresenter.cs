using System;

using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;
using System.Web;
using System.Collections.Generic;

namespace VCTWebApp.Shell.Views
{

    public class HospitalInventoryTransferPresenter : Presenter<IHospitalInventoryTransfer>
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

        public HospitalInventoryTransferPresenter()
            : this(new InventoryStockRepository(HttpContext.Current.User.Identity.Name))
        {
        }

        public HospitalInventoryTransferPresenter(InventoryStockRepository userRepository)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "HospitalInventoryTransferPresenter", "Constructor is invoked.");

            this.InventoryStockRepositoryService = new InventoryStockRepository();
            this.DictionaryRepositoryService = new DictionaryRepository();
        }

        #endregion

        #region Public Overrides
        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed every time the view loads
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "HospitalInventoryTransferPresenter", "OnViewInitialized() is invoked.");
            try
            {
                PopulateHospital();
                PopulateEmptyTable();                
            }
            catch
            {
                throw;
            }

            helper.LogInformation(HttpContext.Current.User.Identity.Name, "HospitalInventoryTransferPresenter", "OnViewInitialized() is completed");
        }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "HospitalInventoryTransferPresenter", "OnViewLoaded() is invoked.");

            try
            {
             
                helper.LogInformation(HttpContext.Current.User.Identity.Name, "HospitalInventoryTransferPresenter", "OnViewLoaded() is completed");

            }
            catch
            {
                throw;
            }

        }     
        #endregion

        #region Public Methods

        public void PopulateEmptyTable()
        {
            List<PartyAvailableCatalog> emptyList = new List<PartyAvailableCatalog>();
            emptyList.Add(new PartyAvailableCatalog());
            View.PartyAvailableCatalogList = emptyList;
        }

        public void PopulateHospital()
        {
            View.PartyList = new PartyRepository().FetchParties(Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"]));
        }

        public bool Save(out string result)
        {
            bool status = false;
            try
            {
                status = InventoryStockRepositoryService.SaveHospitalInventoryTransfer(Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"]), View.InventoryType, View.FromParty, View.ToParty, View.TableXml, HttpContext.Current.User.Identity.Name, out result);               
            }
            catch
            {
                throw;
            }

            return status;
        }
                     
        #endregion

        #region Private Methods
       
        #endregion

    }
}
