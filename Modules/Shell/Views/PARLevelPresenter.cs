using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;
using System.Web;

namespace VCTWebApp.Shell.Views
{
    public class PARLevelPresenter : Presenter<IPARLevelView>
    {
        private PARLevelRepository parLevelRepositoryService;
        private Helper helper = new Helper();

        #region Constructors

        public PARLevelPresenter()
            : this(new PARLevelRepository(HttpContext.Current.User.Identity.Name))
        {
        }

        public PARLevelPresenter(PARLevelRepository parLevelRepository)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "PARLevelPresenter", "Constructor is invoked.");

            this.parLevelRepositoryService = parLevelRepository;
        }

        #endregion

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "PARLevelPresenter", "OnViewLoaded() is invoked.");
            try
            {   

                helper.LogInformation(HttpContext.Current.User.Identity.Name, "PARLevelPresenter", "OnViewLoaded() is completed");
            }
            catch
            {
                throw;
            }
        }

        public override void OnViewInitialized()
        {
            PopulateRegion();
        }

        public void PopulateLocationPARLevel()
        {
            View.LocationPARLevelList = this.parLevelRepositoryService.GetLocationPARLevelByLocationId(View.SelectedLocationId);
        }

        public void PopulatePartyPARLevel()
        {
            View.PartyPARLevelList = this.parLevelRepositoryService.GetPartyPARLevelByPartyId(View.SelectedPartyId);
        }

        public void PopulateRegion()
        {
            View.RegionList = new AddressRepository().FetchRegions(Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"]));
        }

        public void PopulateBranch()
        {
            View.SalesOfficeList = new AddressRepository().FetchSalesOffices(Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"]));
        }

        public void PopulateHospital()
        {
            View.PartyList = new PartyRepository().FetchParties(Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"]));
        }

        public bool UpdatePartyPARLevelQuantity(long parLevelId, int qty)
        {
            PartyPARLevel ppl = new PartyPARLevel();
            ppl.PARLevelId = parLevelId;
            ppl.PARLevelQty = qty;
            ppl.PartyId = 0;
            ppl.PartNum = string.Empty;
            return parLevelRepositoryService.SavePartyPARLevel(ppl);
        }

        public bool UpdateLocationPARLevelQuantity(long parLevelId, int qty)
        {
            LocationPARLevel lpl = new LocationPARLevel();
            lpl.PARLevelId = parLevelId;
            lpl.PARLevelQty = qty;
            lpl.LocationId = 0;
            lpl.PartNum = string.Empty;
            return parLevelRepositoryService.SaveLocationPARLevel(lpl);
        }

        public bool DeletePartyPARLevelQuantity(long parLevelId)
        {
            return parLevelRepositoryService.DeletePartyPARLevel(parLevelId);
        }

        public bool DeleteLocationPARLevelQuantity(long parLevelId)
        {
            return parLevelRepositoryService.DeleteLocationPARLevel(parLevelId);
        }

        public bool AddPartyPARLevelQuantity(string partNum, int qty)
        {
            PartyPARLevel ppl = new PartyPARLevel();
            ppl.PARLevelId = 0;
            ppl.PARLevelQty = qty;
            ppl.PartyId = View.SelectedPartyId;
            ppl.PartNum = partNum;
            return parLevelRepositoryService.SavePartyPARLevel(ppl);
        }

        public bool AddLocationPARLevelQuantity(string partNum, int qty)
        {
            LocationPARLevel lpl = new LocationPARLevel();
            lpl.PARLevelId = 0;
            lpl.PARLevelQty = qty;
            lpl.LocationId = View.SelectedLocationId;
            lpl.PartNum = partNum;
            return parLevelRepositoryService.SaveLocationPARLevel(lpl);
        }
    }
}




