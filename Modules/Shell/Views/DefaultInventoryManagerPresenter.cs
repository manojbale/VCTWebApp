using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using System.Web;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public class DefaultInventoryManagerPresenter : Presenter<IDefaultInventoryManagerView>
    {

        #region Instance Variables
        private CaseRepository caseRepositoryService;        
        private Helper helper = new Helper();
        #endregion

        #region Constructors
        public DefaultInventoryManagerPresenter()
            : this(new CaseRepository(HttpContext.Current.User.Identity.Name))
        {
        }

        public DefaultInventoryManagerPresenter(CaseRepository caseRepository)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "ContactPresenter", "Constructor is invoked.");

            this.caseRepositoryService = caseRepository;            
        }
        #endregion              

        #region Public Overrides     
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            PopulatePartsHighOrderList();
            PopulateActiveKitFamilyList();
            PopulatePendingBuildKitList();
            PopulateMap();
        }
        #endregion


        #region Private Methods

        private void PopulatePartsHighOrderList()
        {
            List<Default> lstPartOrder = caseRepositoryService.GetPartsHighOrderList(View.LocationId);
            View.PartOrderList = lstPartOrder;
            if (lstPartOrder.Count > 0)
            {
                View.InventoryUtilizationDay = lstPartOrder[0].InventoryUtilizationDay;
            }
        }

        private void PopulateActiveKitFamilyList()
        {
            View.ActiveKitFamilyList = new KitFamilyRepository().GetActiveKitFamiliesByLocationId(Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"]));
        }

        private void PopulatePendingBuildKitList()
        {
            View.PendingBuildKitList = new AssetRepository().GetListOfPendingBuildKit(Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"]));
        }

        private void PopulateMap()
        {
            Default BranchDetail = caseRepositoryService.GetHomeMapBranchDetail(View.LocationId);
            List<Default> CasesList = caseRepositoryService.GetHomeMapCasesList(View.LocationId, View.BusinessDate);

            View.BranchDetail = BranchDetail;
            View.CasesList = CasesList;                        
        }
        #endregion

        #region Public Methods
      
        #endregion
            
    }
}




