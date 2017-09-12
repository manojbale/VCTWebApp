using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;
using System.Web;


namespace VCTWebApp.Shell.Views
{
    public class InventoryAssignmentPresenter : Presenter<IInventoryAssignmentView>
    {
        private Helper helper = new Helper();
        private InventoryStockRepository inventoryStockRepositoryService;

        #region Constructors

        public InventoryAssignmentPresenter()
            : this(new InventoryStockRepository(HttpContext.Current.User.Identity.Name))
        {
        }

        public InventoryAssignmentPresenter(InventoryStockRepository inventoryStockRepository)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "InventoryAssignmentPresenter", "Constructor is invoked.");
            inventoryStockRepositoryService = inventoryStockRepository;
        }

        #endregion

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads

            helper.LogInformation(HttpContext.Current.User.Identity.Name, "InventoryAssignmentPresenter", "OnViewLoaded is invoked.");

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
                    //View.Quantity = SelectedCase.Quantity;
                    View.CaseStatus = SelectedCase.CaseStatus;    
                    
                    if (SelectedCase.CaseType == VCTWeb.Core.Domain.Constants.CaseType.RoutineCase.ToString())
                    {
                        View.ShippingDate = SelectedCase.ShippingDate;
                        View.RetrievalDate = SelectedCase.RetrievalDate;
                        View.ProcedureName = SelectedCase.ProcedureName;
                    }
                    if (SelectedCase.InventoryType == Constants.InventoryType.Kit.ToString())
                    {
                        GetKitNumbersToBeAssigned();
                        PopulateKitDetail();
                    }
                    else
                    {
                        GetLotNumbersToBeAssigned();
                        //View.Quantity = 0;
                        PopulateItemDetail();
                    }
                }
            }
            catch
            {
                throw;
            }

        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "InventoryAssignmentPresenter", "OnViewInitialized() is invoked.");
            try
            {
                PopulateCaseType();
                PopulatePendingCasesList();
            }
            catch
            {
                throw;
            }
        }

        public void PopulatePendingCasesList()
        {
            View.PendingCasesList = new CaseRepository().FetchAllPendingCasesByCaseType(View.SelectedCaseType, Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"]));
        }

        private void GetKitNumbersToBeAssigned()
        {
            View.KitstobeAssigned = inventoryStockRepositoryService.GetKitNumbersToBeAssigned(View.SelectedCaseId, Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"]));
        }

        private void GetLotNumbersToBeAssigned()
        {
            View.LotstobeAssigned = inventoryStockRepositoryService.GetLotNumbersToBeAssignedByCaseId(View.SelectedCaseId, Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"]));
        }

        //public List<InventoryStockPart> GetLotNumbersToBeAssigned(string partNumber)
        //{
        //    return (inventoryStockRepositoryService.GetLotNumbersToBeAssigned(partNumber, Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"])));
        //}

        public bool AssignKitInventory(string buildKitIds, string CaseStatus)
        {
            if (inventoryStockRepositoryService.AssignKitInventory(View.SelectedCaseId, buildKitIds, CaseStatus))
            {
                PopulatePendingCasesList();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AssignPartInventory(string casePartDetailXmlString, string CaseStatus)
        {
            if (inventoryStockRepositoryService.AssignPartInventory(View.SelectedCaseId, casePartDetailXmlString, CaseStatus))
            {
                PopulatePendingCasesList();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void PopulateItemDetail()
        {
            View.ItemDetailList = new CaseRepository().GetCasePartDetailByCaseId(View.SelectedCaseId);
        }

        public void PopulateKitDetail()
        {
            View.KitDetailList = new CaseRepository().GetKitFamilyByCaseId(View.SelectedCaseId);
        }

        private void PopulateCaseType()
        {
            View.CaseTypeList = new CaseRepository().FetchAllCaseType();
        }
    }
}
