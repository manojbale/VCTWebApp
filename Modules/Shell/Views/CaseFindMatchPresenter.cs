using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;
using System.Web;

namespace VCTWebApp.Shell.Views
{
    public class CaseFindMatchPresenter : Presenter<ICaseFindMatchView>
    {
        #region Instance Variables

        private SearchResultRepository searchResultRepositoryService;
        private CaseRepository caseRepositoryService;
        private DictionaryRepository dictionaryRepository;

        private Helper helper = new Helper();

        #endregion

        #region Constructors

        public CaseFindMatchPresenter()
            : this(new CaseRepository(HttpContext.Current.User.Identity.Name))
        {
        }

        public CaseFindMatchPresenter(CaseRepository caseRepository)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "CaseFindMatchPresenter", "Constructor is invoked.");

            this.searchResultRepositoryService = new SearchResultRepository();
            this.caseRepositoryService = caseRepository;
            this.dictionaryRepository = new DictionaryRepository();
        }

        #endregion

        #region Private Methods

        private void PopulateKitSearchResultList()
        {
            View.KitSearchResultList = this.searchResultRepositoryService.GetSearchResultByCaseId(View.SelectedCaseId, View.SearchQuantity, HttpContext.Current.User.Identity.Name);
        }

        private void PopulateKitSearchResultForRequestedLocation()
        {
            List<KitSearchResult> lstKitSearchResult = this.searchResultRepositoryService.GetSearchResultForRequestedLocationByCaseId(View.SelectedCaseId);
            if (lstKitSearchResult != null && lstKitSearchResult.Count > 0)
            {
                View.kitSearchResultForRequestedLocation = lstKitSearchResult[0];
            }
        }

        private void PopulateKitSearchResultForShipToLocation()
        {
            List<KitSearchResult> lstKitSearchResult = this.searchResultRepositoryService.GetSearchResultForShipToLocationByCaseId(View.SelectedCaseId);
            if (lstKitSearchResult != null && lstKitSearchResult.Count > 0)
            {
                View.kitSearchResultForShipToLocation = lstKitSearchResult[0];
            }
        }

        #endregion

        #region Public Overrides

        public override void OnViewLoaded()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "CaseFindMatchPresenter", "OnViewLoaded is invoked.");

            try
            {

            }
            catch
            {
                throw;
            }
        }

        public override void OnViewInitialized()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "CaseFindMatchPresenter", "OnViewInitialized() is invoked.");
            try
            {
                this.PopulateKitSearchResultForRequestedLocation();
                this.PopulateKitSearchResultList();
                this.PopulateKitSearchResultForShipToLocation();
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Public Methods

        public Dictionary GetDictionaryRule(string key)
        {
            return this.dictionaryRepository.GetDictionaryRule(key);
        }

        public Constants.ResultStatus SendRequest(out string caseNumberCreated)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "CaseFindMatchPresenter", "SendRequest() is invoked.");

            Constants.ResultStatus resultStatus = Constants.ResultStatus.Error;
            try
            {
                caseNumberCreated = this.caseRepositoryService.SendRequestForCase(View.SelectedCaseId, View.RequestedQuantity, View.SelectedLocationId);
                resultStatus = Constants.ResultStatus.RequestSent;
                helper.LogInformation(HttpContext.Current.User.Identity.Name, "CaseFindMatchPresenter", "Request '" + View.SelectedCaseId + "' has been sent for confirmation to Location '" + View.SelectedLocationId + "' successfully.");
            }
            catch
            {
                throw;
            }

            return resultStatus;
        }

        #endregion
    }
}




