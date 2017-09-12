using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public class CaseSchedulePresenter : Presenter<ICaseScheduleView>
    {
        #region Private Fields
        private Helper helper = new Helper(); 
        #endregion

        #region Constructors

        public CaseSchedulePresenter()         
        {
        }

        #endregion

        #region Init/Load
        public override void OnViewLoaded()
        {
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "CasePresenter", "OnViewInitialized() is invoked.");
            try
            {
                this.PopulateCaseStatus(); 
                this.PopulateSalesRepList();
                this.PopulatePhysicianList();
            }
            catch
            {
                throw;
            }
        }
        #endregion

        private void PopulateCaseStatus()
        {
            View.CaseStatusList = new CaseRepository().FetchAllCaseStatus();
        }

        private void PopulatePhysicianList()
        {
            View.PhysicianList = new CaseRepository().FetchAllPhysician();
        }

        private void PopulateSalesRepList()
        {
            View.SalesRepList = new UserRepository().FetchAllUsers();
        }
        
    }
}
