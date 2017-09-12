using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCTWeb.Core.Domain;
using System.Web;
using Microsoft.Practices.CompositeWeb;

namespace VCTWebApp.Shell.Views
{
    public class VirtualCheckInPresenter: Presenter<IVirtualCheckInView>
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

        public VirtualCheckInPresenter()
            : this(new InventoryStockRepository(HttpContext.Current.User.Identity.Name))
        {
        }

        public VirtualCheckInPresenter(InventoryStockRepository userRepository)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "VirtualCheckOutPresenter", "Constructor is invoked.");

            this.InventoryStockRepositoryService = new InventoryStockRepository();            
        }

        #endregion

        #region Public Overrides
        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed every time the view loads
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "VirtualCheckInPresenter", "OnViewInitialized() is invoked.");
            try
            {                
                PopulateCaseType();
                PopulatePendingCasesList();
            }
            catch
            {
                throw;
            }

            helper.LogInformation(HttpContext.Current.User.Identity.Name, "VirtualCheckInPresenter", "OnViewInitialized() is completed");
        }

        public List<DispositionType> PopulateDispositionType()
        {
            return (new CaseRepository().GetDespositionTypeByCategory("ReturnInventoryRMA"));
        }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "VirtualCheckInPresenter", "OnViewLoaded() is invoked.");

            try
            {
                Cases SelectedCase = new CaseRepository().FetchCaseById(View.SelectedCaseId);
                if (SelectedCase != null)
                {
                    View.CaseNumber = SelectedCase.CaseNumber;
                    View.SurgeryDate = SelectedCase.SurgeryDate;

                    if ((SelectedCase.CaseType == "ReturnInventoryRMATransfer" || SelectedCase.CaseType == "RoutineCase") && SelectedCase.PartyId > 0)
                    {
                        View.ShipFromLocation = SelectedCase.PartyName;
                        View.ShipToLocation = SelectedCase.ShipFromLocation; 
                    }
                    else
                    {
                        View.ShipFromLocation = SelectedCase.ShipFromLocation;
                        View.ShipToLocation = SelectedCase.PartyName;
                    }
                    View.ShipToLocationType = Convert.ToString(HttpContext.Current.Session["LoggedInLocationType"]);
                    //View.ShipToLocationType = SelectedCase.PartyType;
                    //View.CaseType = SelectedCase.CaseType;
                    View.PartyId = SelectedCase.PartyId;
                    View.InventoryType = SelectedCase.InventoryType;
                    
                    //View.KitFamily = SelectedCase.KitFamilyName;
                    //View.KitFamilyId = (SelectedCase.KitFamilyId == null ? 0 : Convert.ToInt64(SelectedCase.KitFamilyId));
                    //View.KitFamilyDesc = SelectedCase.KitFamilyDesc;
                    View.Quantity = SelectedCase.Quantity;
                    View.CaseTypeByCaseId = SelectedCase.CaseType;
                    View.CaseStatus = SelectedCase.CaseStatus;

                    if (SelectedCase.CaseType == VCTWeb.Core.Domain.Constants.CaseType.RoutineCase.ToString())
                    {
                        View.ShippingDate = SelectedCase.ShippingDate;
                        View.RetrievalDate = SelectedCase.RetrievalDate;
                        View.ProcedureName = SelectedCase.ProcedureName;
                    }
                    if (SelectedCase.InventoryType == Constants.InventoryType.Kit.ToString())
                    {                                             
                        List<VirtualCheckOut> lstCheckedInKits = InventoryStockRepositoryService.GetShippingKitsByCaseId(View.SelectedCaseId);
                        View.CheckedInKitList = lstCheckedInKits.FindAll(t => t.KitStatus != "AssignedToCase");
                    }
                    else
                    {
                        List<VirtualCheckOut> lstCheckedInParts = InventoryStockRepositoryService.GetShippingPartsByCaseId(View.SelectedCaseId);
                        View.Quantity = lstCheckedInParts.Count;

                        if (SelectedCase.CaseType == "ReturnInventoryRMATransfer")
                        {
                            View.CheckedInPartList = lstCheckedInParts;
                        }
                        else
                        {
                            View.CheckedInPartList = lstCheckedInParts.FindAll(t => (t.PartStatus != "AssignedToCase" && (!string.IsNullOrEmpty(t.PartStatus))));
                        }
                    }
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
        public bool SaveDetails(out string Result, string FinalStatus)
        {             
            bool result;
            try
            {
                Int32 LocationId = Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"]);
                result = InventoryStockRepositoryService.SaveCheckedInDetails(View.SelectedCaseId, View.InventoryType, View.TableXml, HttpContext.Current.User.Identity.Name, LocationId, out Result, FinalStatus);
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
            View.PendingCasesList = InventoryStockRepositoryService.CheckInPendingCasesList(LocationId, View.CaseType);
        }

        public List<VCTWeb.Core.Domain.VirtualCheckOut> PopulateCheckOutKitByCaseKitId(Int64 CaseKitId)
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
