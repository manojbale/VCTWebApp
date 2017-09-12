using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCTWeb.Core.Domain;
using System.Web;
using Microsoft.Practices.CompositeWeb;

namespace VCTWebApp.Shell.Views
{
    public class VirtualCheckOutPresenter: Presenter<IVirtualCheckOutView>
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

        public VirtualCheckOutPresenter()
            : this(new InventoryStockRepository(HttpContext.Current.User.Identity.Name))
        {
        }

        public VirtualCheckOutPresenter(InventoryStockRepository userRepository)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "VirtualCheckOutPresenter", "Constructor is invoked.");

            this.InventoryStockRepositoryService = new InventoryStockRepository();            
        }

        #endregion

        #region Public Overrides
        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed every time the view loads
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "VirtualCheckOutPresenter", "OnViewInitialized() is invoked.");
            try
            {                
                PopulateCaseType();
            }
            catch
            {
                throw;
            }

            helper.LogInformation(HttpContext.Current.User.Identity.Name, "VirtualCheckOutPresenter", "OnViewInitialized() is completed");
        }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "VirtualCheckOutPresenter", "OnViewLoaded() is invoked.");

            try
            {
                Cases SelectedCase = new CaseRepository().FetchCaseById(View.SelectedCaseId);
                if (SelectedCase != null)
                {
                    View.CaseNumber = SelectedCase.CaseNumber;
                    View.SurgeryDate = SelectedCase.SurgeryDate;
                    View.ShipFromLocation = SelectedCase.ShipFromLocation;
                    View.ShipToLocation = SelectedCase.PartyName;
                    View.ShipToLocationType = SelectedCase.PartyType;
                    //View.CaseType = SelectedCase.CaseType;
                    View.InventoryType = SelectedCase.InventoryType;

                    //View.KitFamily = SelectedCase.KitFamilyName;
                    //View.KitFamilyId = (SelectedCase.KitFamilyId == null ? 0 : Convert.ToInt64(SelectedCase.KitFamilyId));
                    //View.KitFamilyDesc = SelectedCase.KitFamilyDesc;
                    View.Quantity = SelectedCase.Quantity;
                    View.CaseStatus = SelectedCase.CaseStatus;

                    if (SelectedCase.CaseType == VCTWeb.Core.Domain.Constants.CaseType.RoutineCase.ToString())
                    {
                        View.ShippingDate = SelectedCase.ShippingDate;
                        View.RetrievalDate = SelectedCase.RetrievalDate;
                        View.ProcedureName = SelectedCase.ProcedureName;
                    }

                    if (SelectedCase.InventoryType == Constants.InventoryType.Kit.ToString())
                    {
                        List<VirtualCheckOut> lstShippingKits = InventoryStockRepositoryService.GetShippingKitsByCaseId(View.SelectedCaseId);
                        View.ShippingKitList = lstShippingKits;
                    }
                    else
                    {
                        List<VirtualCheckOut> lstShippingParts = InventoryStockRepositoryService.GetShippingPartsByCaseId(View.SelectedCaseId);
                        View.Quantity = lstShippingParts.Count;
                        View.ShippingPartList = lstShippingParts.FindAll(t => (!string.IsNullOrEmpty(t.PartStatus)));
                    }

                }

                helper.LogInformation(HttpContext.Current.User.Identity.Name, "VirtualCheckOutPresenter", "OnViewLoaded() is completed");

            }
            catch
            {
                throw;
            }

        }     
        #endregion

        #region Public Methods
        public bool Save(string FinalCaseStatus)
        {             
            bool result;
            try
            {
                result = InventoryStockRepositoryService.SaveShippingDetails(View.SelectedCaseId, View.InventoryType, View.TableXml, HttpContext.Current.User.Identity.Name, FinalCaseStatus);
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
            View.PendingCasesList = InventoryStockRepositoryService.ShippingPendingCasesList(LocationId, View.CaseType);
        }

        public List<VCTWeb.Core.Domain.VirtualCheckOut> PopulateBuildKitById(Int64 BuildKitId)
        {
            List<VCTWeb.Core.Domain.VirtualCheckOut> lstBuildKit = InventoryStockRepositoryService.PopulateBuildKitById(BuildKitId);

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
