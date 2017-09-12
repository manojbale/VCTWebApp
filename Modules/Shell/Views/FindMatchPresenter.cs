using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;
using System.Web;

namespace VCTWebApp.Shell.Views
{
    public class FindMatchPresenter : Presenter<IFindMatchView>
    {
        #region Instance Variables

        private SearchResultRepository searchResultRepositoryService;
        private RequestRepository requestRepositoryService;
        private DictionaryRepository dictionaryRepository;

        private Helper helper = new Helper();

        #endregion

        #region Constructors

        public FindMatchPresenter()
            : this(new RequestRepository(HttpContext.Current.User.Identity.Name))
        {
        }

        public FindMatchPresenter(RequestRepository requestRepository)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "FindMatchPresenter", "Constructor is invoked.");

            this.searchResultRepositoryService = new SearchResultRepository();
            this.requestRepositoryService = requestRepository;
            this.dictionaryRepository = new DictionaryRepository();
        }
        
        #endregion

        #region Private Methods

        private void PopulateKitSearchResultList()
        {
            //View.KitSearchResultList = this.searchResultRepositoryService.GetSearchResultByRequestId(View.SelectedRequestId, HttpContext.Current.User.Identity.Name);
        }

        private void PopulateKitSearchResultForRequestedLocation()
        {
            //List<KitSearchResult> lstKitSearchResult = this.searchResultRepositoryService.GetSearchResultForRequestedLocation(View.SelectedRequestId);
            //if (lstKitSearchResult != null && lstKitSearchResult.Count > 0)
            //{
            //    View.kitSearchResultForRequestedLocation = lstKitSearchResult[0];
            //}
        }

        private void PopulateKitSearchResultForShipToLocation()
        {
            //List<KitSearchResult> lstKitSearchResult = this.searchResultRepositoryService.GetSearchResultForShipToLocation(View.SelectedRequestId);
            //if (lstKitSearchResult != null && lstKitSearchResult.Count > 0)
            //{
            //    View.kitSearchResultForShipToLocation = lstKitSearchResult[0];
            //}
        }

        #endregion

        #region Public Overrides

        public override void OnViewLoaded()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "FindMatchPresenter", "OnViewLoaded is invoked.");

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
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "FindMatchPresenter", "OnViewInitialized() is invoked.");
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

        public Constants.ResultStatus SendRequest()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "FindMatchPresenter", "SendRequest() is invoked.");

            Constants.ResultStatus resultStatus = Constants.ResultStatus.Error;
            try
            {
                if (this.requestRepositoryService.SendRequest(View.SelectedRequestId, View.RequestedQuantity, View.SelectedLocationId))
                {
                    resultStatus = Constants.ResultStatus.RequestSent;
                }
                helper.LogInformation(HttpContext.Current.User.Identity.Name, "FindMatchPresenter", "Request '" + View.SelectedRequestId + "' has been sent for confirmation to Location '" + View.SelectedLocationId + "' successfully.");
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




