using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using System.Web;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public class CasePresenter : Presenter<ICaseView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IShellController _controller;
        // public CasePresenter([CreateNew] IShellController controller)
        // {
        // 		_controller = controller;
        // }

        private Helper helper = new Helper();

        private CaseRepository caseRepositoryService;
        
        #region Constructors

        public CasePresenter()
            : this(new CaseRepository(HttpContext.Current.User.Identity.Name))
        {
        }

        public CasePresenter(CaseRepository caseRepository)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "CasePresenter", "Constructor is invoked.");
            caseRepositoryService = caseRepository;
        }

        #endregion

        public override void OnViewLoaded()
        {
            //helper.LogInformation(HttpContext.Current.User.Identity.Name, "CasePresenter", "OnViewLoaded() is invoked.");
            //try
            //{
            //    if (View.CaseId != 0)
            //    {
            //        Case CurrentCase = caseRepositoryService.BindCaseById(View.CaseId);
            //        BindCaseToView(CurrentCase);
            //    }
            //    helper.LogInformation(HttpContext.Current.User.Identity.Name, "CasePresenter", "OnViewLoaded() is completed.");
            //}
            //catch (Exception ex)
            //{                
            //    throw;
            //}
        }

        public void PopulateEmptyKitTable()
        {
            List<CaseKitFamilyDetailGroup> emptyList = new List<CaseKitFamilyDetailGroup>();
            emptyList.Add(new CaseKitFamilyDetailGroup());
            View.KitFamilyList = emptyList;
        }

        public void BindCaseToView(Cases CurrentCase)
        {
            View.SurgeryDate = CurrentCase.SurgeryDate;
            View.ShippingDate = CurrentCase.ShippingDate;
            View.RetrievalDate = CurrentCase.RetrievalDate;
            View.PatientName = CurrentCase.PatientName;
            View.SpecialInstructions = CurrentCase.SpecialInstructions;
            View.SelectedSalesRep = CurrentCase.SalesRep;
            View.CaseStatus=CurrentCase.CaseStatus;
            //View.SelectedKitFamilyId = CurrentCase.KitFamilyId;
            //View.KitFamilyName = CurrentCase.KitFamilyName + " (" + CurrentCase.KitFamilyDesc + ")";
            View.SelectedProcedureName = CurrentCase.ProcedureName;
            View.Physician = CurrentCase.Physician;
            View.SelectedParty = CurrentCase.PartyId;
            //View.Technicians = CurrentCase.Technicians;
            View.InventoryType = CurrentCase.InventoryType;
            View.TotalPrice = CurrentCase.TotalPrice;
            View.SelectedPartyName = CurrentCase.PartyName;
            View.CaseNumber = CurrentCase.CaseNumber;
            //View.Quantity = CurrentCase.Quantity;
            //View.KitFamilyName = CurrentCase.KitFamilyName;
        }

        public override void OnViewInitialized()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "CasePresenter", "OnViewInitialized() is invoked.");
            try
            {
                //this.PopulateKitFamily();
                this.PopulateSalesRep();
            }
            catch
            {
                throw;
            }
        }

        private void PopulateSalesRep()
        {
            //View.SalesRep = new UserRepository().FetchAllUsers();
            View.SalesRep = new UserRepository().GetListOfSalesRepByLocationId(Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"].ToString()));
        }

        //private void PopulateKitFamily()
        //{
        //    View.KitFamily = new KitFamilyRepository().FetchAllKitFamilyByLocationId(Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"].ToString()));
        //}

        public bool Save()
        {
            Cases CaseToBeSaved = GetCaseToBeSaved();
            
            return caseRepositoryService.SaveCase(CaseToBeSaved, View.CaseKitFamilyDetailXml, View.CasePartDetailXml);
        }

        private Cases GetCaseToBeSaved()
        {
            Cases newcase = new Cases();
            newcase.CaseId = View.CaseId;
            newcase.SurgeryDate = View.SurgeryDate;
            newcase.PatientName = View.PatientName;
            newcase.SpecialInstructions = View.SpecialInstructions;
            newcase.SalesRep = View.SelectedSalesRep;
            newcase.CaseStatus = View.CaseStatus;
            //newcase.KitFamilyId = View.SelectedKitFamilyId;            
            newcase.ProcedureName = View.SelectedProcedureName;
            newcase.Physician = View.Physician;
            newcase.PartyId = View.SelectedParty;
            newcase.InventoryType = View.InventoryType;
            newcase.CaseType = Constants.CaseType.RoutineCase.ToString();
            newcase.TotalPrice = View.TotalPrice;
            newcase.LocationId = Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"].ToString());
            //newcase.Quantity = View.Quantity;

            return newcase;
        }

        public void SetFieldsBlank()
        {
            //View.SurgeryDate = DateTime.Now;
            View.ShippingDate = null;
            View.RetrievalDate = null;
            View.SelectedProcedureName = string.Empty;
            View.SelectedSalesRep = string.Empty;
            //View.SelectedKitFamilyId = 0;
            //View.KitFamilyName = string.Empty;
            View.SelectedParty = null;
            View.Physician = string.Empty;
            View.PatientName = string.Empty;
            View.TotalPrice = null;
            View.SpecialInstructions = string.Empty;
            View.CaseStatus = null;
            View.SelectedPartyName = string.Empty;
            View.CaseNumber = null;
            //View.Quantity = 0;
            View.InventoryType = Constants.InventoryType.Kit.ToString();
            View.KitFamilyList = new List<CaseKitFamilyDetailGroup>();
            View.PartDetailList = new List<CasePartDetailGroup>();
            //View.IndicativePartDetailList = null;
        }

        public void BindCaseById()
        {
            if (View.CaseId != 0) // Bind popup with the selected case
            {
                Cases newCase = caseRepositoryService.FetchCaseById(View.CaseId);
                if (newCase != null)
                {
                    BindCaseToView(newCase);
                }          
            }
            else // Bind Blank Popup
            {
                SetFieldsBlank();
            }


            if (View.InventoryType == Constants.InventoryType.Kit.ToString())
            {
                //PopulateKitFamilyItems();
                View.KitFamilyList = caseRepositoryService.GetGroupedCaseKitFamilyDetailByCaseId(View.CaseId);
            }
            else
            {
                View.PartDetailList = caseRepositoryService.GetGroupedCasePartDetailByCaseId(View.CaseId);
            }
        }

        //public void PopulateKitFamilyItems()
        //{
        //    View.IndicativePartDetailList = caseRepositoryService.GetKitFamilyItemsByCaseId(Convert.ToInt64(View.SelectedKitFamilyId), View.CaseId);
        //}

        public bool CancelCase()
        {
            return caseRepositoryService.CancelCase(View.CaseId);
        }


        public void PopulateKitFamilyByProcedureName(string sProcedureName,string  sPhysicianName)
        {
            View.KitFamilyList = new KitFamilyRepository().FetchAllKitFamilyByProcedureName(sProcedureName, sPhysicianName);
        }
    }
}




