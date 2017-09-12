using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;
using System.Web;

namespace VCTWebApp.Shell.Views
{
    public class ViewCancelTransactionPresenter : Presenter<IViewCancelTransactionView>
    {

        #region Instance Variables
        private CaseRepository caseRepositoryService;
        private InventoryStockRepository InventoryStockRepositoryService;

        private Helper helper = new Helper();
        #endregion

        #region Constructors
        public ViewCancelTransactionPresenter()
            : this(new CaseRepository(HttpContext.Current.User.Identity.Name))
        {
        }

        public ViewCancelTransactionPresenter(CaseRepository caseRepository)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "ContactPresenter", "Constructor is invoked.");

            this.caseRepositoryService = caseRepository;
            this.InventoryStockRepositoryService = new InventoryStockRepository();
        }
        #endregion

        #region Public Overrides
        public override void OnViewInitialized()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "ViewCancelTransactionPresenter", "OnViewInitialized() is invoked.");
            try
            {
                PopulatePageSize();
                PopulateDispositionTypes();
              //  PopulateCasesList(PageIndex, PageSize);
                PopulateCancelDispositionType();


            }
            catch
            {
                throw;
            }
        }

        public override void OnViewLoaded()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "ViewCancelTransactionPresenter", "OnViewLoaded is invoked.");

            try
            {
                //PopulateCasesSummaryList();
                //Contact contact = this.contactRepositoryService.FetchContactsByContactId(View.SelectedContactId);
                //if (contact != null)
                //{
                //    View.FirstName = contact.FirstName.Trim();
                //    View.LastName = contact.LastName.Trim();
                //    View.Email = contact.Email.Trim();
                //    View.Phone = contact.Phone.Trim();
                //    View.Cell = contact.Cell.Trim();
                //    View.Fax = contact.Fax.Trim();
                //    View.LocationId = contact.LocationId;
                //    View.Active = contact.IsActive;
                //}
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Private Methods
        private void SetFieldsBlank()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "ViewCancelTransactionPresenter", "SetFieldsBlank() is invoked.");

            //View.FirstName = string.Empty;
            //View.LastName = string.Empty;
            //View.Email = string.Empty;
            //View.Phone = string.Empty;
            //View.Cell = string.Empty;
            //View.Fax = string.Empty;
            //View.LocationId = null;
            //View.Active = true;
        }

        private void PopulatePageSize()
        {
            View.PageSize = this.caseRepositoryService.GetGridPageSize();
        }


        //private void PopulateEmptyCasesSummaryList()
        //{
        //    List<ViewCancelTransaction> lstViewCancelTransaction = new List<ViewCancelTransaction>();
        //    lstViewCancelTransaction.Add(new ViewCancelTransaction());
        //    View.CasesOverAllList = lstViewCancelTransaction;
        //}

        #endregion

        #region Public Methods

        public List<ViewCancelTransaction> PopulateEmptyCasesList()
        {
            List<ViewCancelTransaction> lstViewCancelTransaction = new List<ViewCancelTransaction>();
            lstViewCancelTransaction.Add(new ViewCancelTransaction());
            return lstViewCancelTransaction;
        }

        public void PopulateCancelDispositionType()
        {
            View.DispositionTypeList = caseRepositoryService.DispositionTypeList();
        }

        public List<CaseType> PopulateDispositionTypes()
        {
            //View.CaseTypeList = this.caseRepositoryService.FetchAllCaseType();
            List<CaseType> lstCastType = this.caseRepositoryService.FetchAllCaseType();
            return lstCastType;
        }

        public List<string> PopulateLocationType()
        {
            List<string> lstLocType = this.caseRepositoryService.GetLocationTypes();
            return lstLocType;
        }

        public List<string> PopulateLocationAndPartyType()
        {
            List<string> lstLocType = this.caseRepositoryService.GetLocationAndPartyTypes();
            return lstLocType;
        }

        public List<VCTWeb.Core.Domain.VirtualCheckOut> PopulateKitItems(Int64 BuildKitId, Int64 CaseId = 0)
        {
            List<VCTWeb.Core.Domain.VirtualCheckOut> lstBuildKit;
            if (CaseId == 0)
                lstBuildKit = InventoryStockRepositoryService.PopulateBuildKitById(BuildKitId);
            else
                lstBuildKit = InventoryStockRepositoryService.PopulateCheckOutBuildKitById(BuildKitId, CaseId);

            return lstBuildKit;
        }

        //public void PopulateCasesSummaryList()
        //{
        //    View.CasesOverAllList = this.caseRepositoryService.GetCasesSummaryByCaseType(View.CaseType, View.StartDate, View.EndDate, View.LocationId);
        //}

        public void PopulateCasesList(int PageIndex,int PageSize)
        {
            List<ViewCancelTransaction> lstCases = this.caseRepositoryService.GetCasesListByCaseType(View.LocationId, View.StartDate, View.EndDate, View.CaseTypeSearch, View.CaseNumberSearch, View.InvTypeSearch, View.PartyNameSearch, View.LocationTypeSearch, View.CaseStatusSearch, PageIndex, PageSize);

            if (lstCases != null && lstCases.Count > 0)
            {
                View.CasesList = lstCases;
            }
            else
            {
                //View.CasesList = PopulateEmptyCasesList();
                View.CasesList = new List<ViewCancelTransaction>();
            }
        }

        public bool CaseTransactionCancel()
        {
            bool result = false;
            result = this.caseRepositoryService.CaseDetailsCancel(View.CaseId, View.DispositionTypeId, View.Remarks);
            return result;
        }

        public List<ViewCancelTransaction> PopulateCaseItems(string InventoryType, Int64 CaseId)
        {
            List<ViewCancelTransaction> lstCaseItems = null;
            if (InventoryType.ToUpper() == "PART")
            {
                lstCaseItems = this.caseRepositoryService.GetCaseItemsListByCaseId(CaseId);
            }
            else if (InventoryType.ToUpper() == "KIT")
            {
                List<ViewCancelTransaction> lstKit = new List<ViewCancelTransaction>();
                lstKit = this.caseRepositoryService.GetKitDetailByCaseId(CaseId);

                if (lstKit != null && lstKit.Count > 0)
                {
                    List<ViewCancelTransaction> lstKitFamily = this.caseRepositoryService.GetKitFamilyDetailByCaseId(CaseId);

                    if (lstKitFamily != null && lstKitFamily.Count > 0)
                    {
                        int index = 0;
                        foreach (var item in lstKitFamily)
                        {
                            lstKit[index].KitNumber = item.KitNumber;
                            index += 1;
                        }
                    }
                }

                lstCaseItems = lstKit;
            }

            return lstCaseItems;
        }

        //public void PopulateCaseItems(string InventoryType,Int64 CaseId)
        //{
        //    if (InventoryType.ToUpper() == "PART")
        //    {
        //        View.ChildList = this.caseRepositoryService.GetCaseItemsListByCaseId(CaseId);
        //    }
        //    else if (InventoryType.ToUpper() == "KIT")
        //    {
        //        List<ViewCancelTransaction> lstKit = new List<ViewCancelTransaction>();
        //        lstKit = this.caseRepositoryService.GetKitDetailByCaseId(CaseId);

        //        if (lstKit != null && lstKit.Count > 0)
        //        {
        //            //for (int i = 0; i < lstKit[0].Quantity -1; i++)
        //            //{
        //            //    lstKit.Add(
        //            //        new ViewCancelTransaction()
        //            //        {
        //            //            CaseId = lstKit[0].CaseId,
        //            //            KitFamilyName = lstKit[0].KitFamilyName,
        //            //            Description = lstKit[0].Description
        //            //        });
        //            //}

        //            List<ViewCancelTransaction> lstKitFamily = this.caseRepositoryService.GetKitFamilyDetailByCaseId(CaseId);

        //            if (lstKitFamily != null && lstKitFamily.Count > 0)
        //            {
        //                int index = 0;
        //                foreach(var item in lstKitFamily)
        //                {
        //                    lstKit[index].KitNumber = item.KitNumber;
        //                    index += 1;
        //                }
        //            }
        //        }

        //        View.ChildList = lstKit;
        //    }
        //}
        #endregion
    }
}
