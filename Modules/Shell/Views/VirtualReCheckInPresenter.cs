using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCTWeb.Core.Domain;
using System.Web;
using Microsoft.Practices.CompositeWeb;

namespace VCTWebApp.Shell.Views
{
    public class VirtualReCheckInPresenter: Presenter<IVirtualReCheckInView>
    {
        private InventoryStockRepository InventoryStockRepositoryService;        
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

        public VirtualReCheckInPresenter()
            : this(new InventoryStockRepository(HttpContext.Current.User.Identity.Name))
        {
        }

        public VirtualReCheckInPresenter(InventoryStockRepository userRepository)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "VirtualReCheckOutPresenter", "Constructor is invoked.");

            this.InventoryStockRepositoryService = new InventoryStockRepository();            
        }

        #endregion

        #region Public Overrides
        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed every time the view loads
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "VirtualReCheckInPresenter", "OnViewInitialized() is invoked.");
            try
            {                
                PopulateCaseType();
                PopulatePendingCasesList();
            }
            catch
            {
                throw;
            }

            helper.LogInformation(HttpContext.Current.User.Identity.Name, "VirtualReCheckInPresenter", "OnViewInitialized() is completed");
        }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "VirtualReCheckInPresenter", "OnViewLoaded() is invoked.");

            try
            {
                Cases SelectedCase = new CaseRepository().FetchCaseById(View.SelectedCaseId);
                if (SelectedCase != null)
                {
                    View.CaseNumber = SelectedCase.CaseNumber;
                    View.SurgeryDate = SelectedCase.SurgeryDate;

                    //View.ShipFromLocation = SelectedCase.ShipFromLocation;
                    //View.ShipToLocation = SelectedCase.PartyName;
                    //View.ShipToLocationType = SelectedCase.PartyType;                    
                    View.ShipFromLocation = SelectedCase.PartyName;
                    View.ShipToLocation = SelectedCase.ShipFromLocation;
                    View.ShipToLocationType = Convert.ToString(HttpContext.Current.Session["LoggedInLocationType"]);
                    
                    
                    //View.CaseType = SelectedCase.CaseType;
                    View.PartyId = SelectedCase.PartyId;
                    View.InventoryType = SelectedCase.InventoryType;
                    
                    //View.KitFamily = SelectedCase.KitFamilyName;
                    //View.KitFamilyId = (SelectedCase.KitFamilyId == null ? 0 : Convert.ToInt64(SelectedCase.KitFamilyId));
                    //View.KitFamilyDesc = SelectedCase.KitFamilyDesc;
                    View.Quantity = SelectedCase.Quantity;
                    
                    if (SelectedCase.CaseType == VCTWeb.Core.Domain.Constants.CaseType.RoutineCase.ToString())
                    {
                        View.ShippingDate = SelectedCase.ShippingDate;
                        View.RetrievalDate = SelectedCase.RetrievalDate;
                        View.ProcedureName = SelectedCase.ProcedureName;
                    }
                    if (SelectedCase.InventoryType == Constants.InventoryType.Kit.ToString())
                    {
                        View.CheckedInKitList = InventoryStockRepositoryService.GetShippingKitsByCaseId(View.SelectedCaseId);                       
                    }
                    //else
                    //{
                    //    View.CheckedInPartList = InventoryStockRepositoryService.GetShippingPartsByCaseId(View.SelectedCaseId);
                    //    View.Quantity = 0;                        
                    //}
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
        public bool Save()
        {             
            bool result;
            try
            {
                result = InventoryStockRepositoryService.SaveReCheckedInDetails(View.SelectedCaseId, View.InventoryType, View.TableXml, HttpContext.Current.User.Identity.Name);
                if (result)
                {
                    PopulatePendingCasesList();
                }
            }
            catch
            {
                throw;
            }

            return result;
        }

        public void PopulatePendingCasesList()
        {
            Int32 LocationId = Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"]);
            View.PendingCasesList = InventoryStockRepositoryService.ReCheckInPendingCasesList(LocationId, View.CaseType);
        }

        public List<VCTWeb.Core.Domain.VirtualCheckOut> PopulateCheckOutBuildKitById(Int64 CaseKitId)
        {               
            List<VCTWeb.Core.Domain.VirtualCheckOut> lstBuildKit = InventoryStockRepositoryService.GetCheckOutKitByCaseKitId(CaseKitId);
            return lstBuildKit;
        }
        #endregion

        #region Private Methods
        private void PopulateCaseType()
        {
            View.CaseTypeList = new CaseRepository().FetchAllCaseType();
        }                
        #endregion

    }
}
