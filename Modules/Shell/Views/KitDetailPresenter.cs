using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using System.Web;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public class KitDetailPresenter : Presenter<IKitDetailView>
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

        
        #region Constructors

        public KitDetailPresenter()           
        {

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

        public void PopulateData(int locationId, long kitFamilyId)
        {
            View.VirtualCheckOutList = new InventoryStockRepository().PopulateBuildKitItemsByLocationAndKitFamily(locationId, kitFamilyId);
            View.KitDetailStockLevelList = new KitFamilyRepository().GetKitDetailStockLevelByLocationAndKitFamily(locationId, kitFamilyId);
        }
    }
}




