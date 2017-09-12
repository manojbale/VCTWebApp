using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;
using System.Web;

namespace VCTWebApp.Shell.Views
{
    public class ReplenishmentTransferPresenter : Presenter<IReplenishmentTransferView>
    {
        private PARLevelRepository parLevelRepositoryService;
        private Helper helper = new Helper();

        #region Constructors

        public ReplenishmentTransferPresenter()
            : this(new PARLevelRepository(HttpContext.Current.User.Identity.Name))
        {
        }

        public ReplenishmentTransferPresenter(PARLevelRepository parLevelRepository)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "ReplenishmentTransferPresenter", "Constructor is invoked.");

            this.parLevelRepositoryService = parLevelRepository;
        }

        #endregion

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "ReplenishmentTransferPresenter", "OnViewLoaded() is invoked.");
            try
            {

                helper.LogInformation(HttpContext.Current.User.Identity.Name, "ReplenishmentTransferPresenter", "OnViewLoaded() is completed");
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

        public void PopulateLocationReplenishmentTransfer()
        {
            View.ReplenishmentTransferList = this.parLevelRepositoryService.GetReplenishmentTransferByLocationId(View.SelectedLocationId);
        }

        public void PopulatePartyReplenishmentTransfer()
        {
            View.ReplenishmentTransferList = this.parLevelRepositoryService.GetReplenishmentTransferByPartyId(View.SelectedPartyId);
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

        public bool SavePartyReplenishmentTransfer(List<ReplenishmentTransfer> lstSelectedReplenishmentTransfer, out string result)
        {
            result = "";
            string itemDetailXmlString = "<root>";
            if (lstSelectedReplenishmentTransfer.Count > 0)
            {
                foreach (ReplenishmentTransfer rt in lstSelectedReplenishmentTransfer)
                {
                    itemDetailXmlString += "<ItemDetail>";
                    itemDetailXmlString += "<PartNum>" + rt.PartNum + "</PartNum>";
                    itemDetailXmlString += "<Quantity>" + rt.ReplenishQty.ToString() + "</Quantity>";
                    itemDetailXmlString += "</ItemDetail>";
                }
            }
            else
            {
                return false;
            }
            itemDetailXmlString += "</root>";
            return parLevelRepositoryService.SavePartyReplenishmentTransfer(View.SelectedPartyId, View.RequiredOn, itemDetailXmlString, Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"]), out result);
        }

        public bool SaveLocationReplenishmentTransfer(List<ReplenishmentTransfer> lstSelectedReplenishmentTransfer, out string result)
        {
            result = "";
            string itemDetailXmlString = "<root>";
            if (lstSelectedReplenishmentTransfer.Count > 0)
            {
                foreach (ReplenishmentTransfer rt in lstSelectedReplenishmentTransfer)
                {
                    itemDetailXmlString += "<ItemDetail>";
                    itemDetailXmlString += "<PartNum>" + rt.PartNum + "</PartNum>";
                    itemDetailXmlString += "<Quantity>" + rt.ReplenishQty.ToString() + "</Quantity>";
                    itemDetailXmlString += "</ItemDetail>";
                }
            }
            else
            {
                return false;
            }
            itemDetailXmlString += "</root>";
            return parLevelRepositoryService.SaveLocationReplenishmentTransfer(View.SelectedLocationId, View.RequiredOn, itemDetailXmlString, Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"]), out result);
        }
    }
}




