using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;
using System.Web;

namespace VCTWebApp.Shell.Views
{
    public class SelectKitPresenter : Presenter<ISelectKitView>
    {
        #region Instance Variables

        private KitListingRepository kitListingRepositoryService;
        private DictionaryRepository dictionaryRepository;

        private Helper helper = new Helper();

        #endregion
        
         #region Constructors

        public SelectKitPresenter()
            : this(new UserRepository(HttpContext.Current.User.Identity.Name))
        {
        }

        public SelectKitPresenter(UserRepository userRepository)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "SelectKitPresenter", "Constructor is invoked.");

            this.kitListingRepositoryService = new KitListingRepository();
            this.dictionaryRepository = new DictionaryRepository();
        }

        #endregion

        #region Private Methods

        public void PopulateKitListingList()
        {
            View.KitListingList = this.kitListingRepositoryService.GetKitsByProcedureAndCatalog(View.ProcedureName, View.CatalogNumber);
        }

        #endregion

        #region Public Overrides

        public override void OnViewLoaded()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "SelectKitPresenter", "OnViewLoaded is invoked.");

            try
            {
              //  this.PopulateKitListingList();
            }
            catch
            {
                throw;
            }
        }

        public override void OnViewInitialized()
        {

        }

        #endregion
    }
}




