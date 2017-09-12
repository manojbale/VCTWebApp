using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCTWeb.Core.Domain;
using System.Web;
using Microsoft.Practices.CompositeWeb;

namespace VCTWebApp.Shell.Views
{
    public class AgingOrdersPresenter : Presenter<IAgingOrdersView>
    {

        #region Instance Variables
        private CaseRepository caseRepositoryService;        
        private Helper helper = new Helper();
        #endregion

        #region Constructors
        public AgingOrdersPresenter()
            : this(new CaseRepository(HttpContext.Current.User.Identity.Name))
        {
        }

        public AgingOrdersPresenter(CaseRepository caseRepository)
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
            PopulateAgingOrdersMap();
        }
        #endregion


        #region Private Methods       
        
        private void PopulateAgingOrdersMap()
        {
            View.BranchDetail = caseRepositoryService.GetHomeMapBranchDetail(View.LocationId);            
            View.AgingOrdersList = caseRepositoryService.GetMapAgingOrdersList(View.LocationId, View.BusinessDate);
        }
        #endregion

    
            
    }
}




