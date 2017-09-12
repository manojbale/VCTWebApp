using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;
using System.Web;

namespace VCTWebApp.Shell.Views
{
    public class DefaultSalesPersonPresenter: Presenter<IDefaultSalesPersonView>
    {

        #region Instance Variables
        private CaseRepository caseRepositoryService;
        private InventoryStockRepository InventoryStockRepositoryService;  

        private Helper helper = new Helper();
        #endregion

        #region Constructors
        public DefaultSalesPersonPresenter()
            : this(new CaseRepository(HttpContext.Current.User.Identity.Name))
        {
        }

        public DefaultSalesPersonPresenter(CaseRepository caseRepository)
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
                PopulateCasesList();
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

        #region Public Methods

        public void PopulateCasesList()
        {
            List<ViewCancelTransaction> lstPendingCases = this.caseRepositoryService.GetPendingCasesBySalesPerson(View.LocationId, HttpContext.Current.User.Identity.Name.ToString());

            //if (lstPendingCases != null && lstPendingCases.Count > 0)
            //{
                View.lstPendingCases = lstPendingCases;
            //}
            //else
            //{
            //    View.lstPendingCases = PopulateEmptyPendingCases();   
            //}
        }

        public void PopulateCaseItems(string InventoryType, Int64 CaseId)
        {
            if (InventoryType.ToUpper() == "PART")
            {
                View.ChildList = this.caseRepositoryService.GetCaseItemsListByCaseId(CaseId);
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

                View.ChildList = lstKit;
            }
        }

        public List<VCTWeb.Core.Domain.VirtualCheckOut> PopulateBuildKitById(Int64 BuildKitId)
        {
            List<VCTWeb.Core.Domain.VirtualCheckOut> lstBuildKit = InventoryStockRepositoryService.PopulateBuildKitById(BuildKitId);

            return lstBuildKit;
        }
        #endregion

        #region Private Methods
        private List<ViewCancelTransaction> PopulateEmptyPendingCases()
        {
            List<ViewCancelTransaction> lstPendingCases = new List<ViewCancelTransaction>();
            lstPendingCases.Add(new ViewCancelTransaction());

            return lstPendingCases;
        }
        #endregion

    }
}
