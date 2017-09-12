using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;
using System.Web.UI.WebControls;
using System.Web;
using System.Linq;
using System.Transactions;

namespace VCTWebApp.Shell.Views
{
    public class PartyLocationPresenter : Presenter<IPartyLocationView>
    {
        #region Instance Variables

        private Helper helper = new Helper();

        private readonly PartyRepository partyRepositoryService;
        private readonly PartyLocationRepository partyLocationRepositoryService;
        private readonly LocationRepository locationRepositoryService;
        private readonly CountryRepository countryRepositoryService;
        private readonly AddressRepository addressRepositoryService;
        private readonly StateRepository stateRepositoryService;
        
        #endregion

        #region Public Properties

        public string SelectedPartyLocationValue { get; set; }
        public string SelectedParty { get; set; }

        #endregion

        #region Constructors

        public PartyLocationPresenter()
            : this(new PartyRepository(),
            new PartyLocationRepository(HttpContext.Current.User.Identity.Name),
            new CountryRepository(),
            new LocationRepository(HttpContext.Current.User.Identity.Name),
            new StateRepository(),
            new AddressRepository())
        {
        }

        public PartyLocationPresenter(PartyRepository partyRepository,
            PartyLocationRepository partyLocationRepository,
            CountryRepository countryRepository,
            LocationRepository locationRepository,
            StateRepository stateRepository,
            AddressRepository addressRepository)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyLocationPresenter", "Constructor is invoked.");

            this.partyRepositoryService = partyRepository;
            this.partyLocationRepositoryService = partyLocationRepository;
            this.countryRepositoryService = countryRepository;
            this.locationRepositoryService = locationRepository;
            this.stateRepositoryService = stateRepository;
            this.addressRepositoryService = addressRepository;
        }

        #endregion

        #region Private Methods

        private void PopulatePartyLocations()
        {
            View.PartyLocationList = this.partyRepositoryService.FetchParties();
        }

        //private void BindTreeNodeCollection()
        //{
        //    helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyLocationPresenter", "BindTreeNodeCollection() is invoked.");
        //    try
        //    {
        //        lstParty = this.partyRepositoryService.FetchAllParties();
        //        helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyLocationPresenter", "No. of Records fetched with call to FetchAllParties() is " + Convert.ToString(lstParty.Count));

        //        lstPartyLocation = this.partyLocationRepositoryService.FetchAllPartyLocations();
        //        helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyLocationPresenter", "No. of Records fetched with call to FetchAllPartyLocations() is " + Convert.ToString(lstPartyLocation.Count));

        //        partyLocationsTreeNodes = new TreeNodeCollection();

        //        foreach (Party party in this.lstParty)
        //        {
        //            TreeNode node = GetNode(party.Name, party.PartyId.ToString());
        //            node.SelectAction = TreeNodeSelectAction.Expand;
        //            this.partyLocationsTreeNodes.Add(node);
        //            AddLocations(node, party.PartyId);
        //            node.CollapseAll();
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        //private TreeNode GetNode(string text, string value)
        //{
        //    TreeNode tNode = new TreeNode();
        //    tNode.Text = text;
        //    tNode.Value = value;

        //    return tNode;
        //}

        //private void AddLocations(TreeNode node, long partyId)
        //{
        //    helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyLocationPresenter", "AddLocations() is invoked.");

        //    var result = this.lstPartyLocation.Where(partyloc => partyloc.PartyId == partyId).ToList();
        //    if (result != null && result.Count > 0)
        //    {
        //        foreach (PartyLocation partyLocation in result)
        //        {
        //            TreeNode tmpTNode = GetNode(partyLocation.LocationName, partyLocation.LocationId.ToString());
        //            node.ChildNodes.Add(tmpTNode);
        //        }
        //    }
        //    else
        //    {
        //        TreeNode tmpTNode = GetNode("Add " + View.PartyLocationType.Name, string.Empty);
        //        node.ChildNodes.Add(tmpTNode);
        //    }
        //}

        private void SetFieldsBlank()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyLocationPresenter", "SetFieldsBlank() is invoked.");

            View.LocationId = 0;
            View.AddressId = 0;

            View.Party = string.Empty;
            //View.LocationType = string.Empty;
            View.LocationName = string.Empty;
            View.LocationCode = string.Empty;
            View.GLN = string.Empty;
            View.Description = string.Empty;
            View.Latitude = string.Empty;
            View.Longitude = string.Empty;
            View.Address1 = string.Empty;
            View.Address2 = string.Empty;
            View.ZipCode = string.Empty;
            View.Country = string.Empty;
            View.State = string.Empty;
            View.City = string.Empty;
            //View.Region = string.Empty;
            View.RequiresAddress = true;
        }

        private Constants.ResultStatus CheckDuplicateLocationName(string locationName, int locationId)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyLocationPresenter", "CheckDuplicateLocationName() is invoked with parameters values - Location Name: " + locationName + " and Location Id: " + locationId.ToString());
            if (locationRepositoryService.CheckDuplicateLocationName(locationName, locationId))
            {
                return Constants.ResultStatus.DuplicateLocationName;
            }
            return Constants.ResultStatus.Ok;
        }

        private void FetchLocationTypeForParty()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyLocationPresenter", "FetchLocationTypeForParty() is invoked");
            View.PartyLocationType = locationRepositoryService.FetchLocationTypeForParty();
        }

        #endregion

        #region Public Overrides

        public override void OnViewLoaded()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyLocationPresenter", "OnViewLoaded() is invoked.");
            try
            {                
                LoadPartyLocation();

                //LoadLocation();
            }
            catch
            {
                throw;
            }
        }

        private void LoadPartyLocation()
        {            
            PartyLocation partyLocation = this.partyLocationRepositoryService.FetchPartyLocationByPartyId(View.SelectedPartyId);
            if (partyLocation != null)
            {
                View.LocationId = partyLocation.LocationId;
                //PartyLocation partyLocation = this.partyLocationRepositoryService.GetPartyLocationByLocationId(View.LocationId);
                //View.SelectedPartyId = partyLocation.PartyId;

                View.Party = partyLocation.PartyName;
                View.LocationType = partyLocation.LocationType;
                View.LocationName = partyLocation.LocationName;
                View.LocationCode = partyLocation.Code;
                View.GLN = partyLocation.GLN;
                View.Description = partyLocation.Description;
                View.Latitude = Convert.ToString(partyLocation.Latitude);
                View.Longitude = Convert.ToString(partyLocation.Longitude);

                View.IsActive = partyLocation.IsActive;
                View.RequiresAddress = partyLocation.RequiresAddress;
                if (partyLocation.RequiresAddress)
                {
                    partyLocation.Address = this.locationRepositoryService.GetLocationAddress(View.LocationId);
                    View.AddressId = partyLocation.Address.AddressId;
                    View.Address1 = partyLocation.Address.Line1;
                    View.Address2 = partyLocation.Address.Line2;
                    View.ZipCode = partyLocation.Address.Zip;
                    View.Country = partyLocation.Address.Country;
                    View.StateList = this.stateRepositoryService.GetStateList(View.Country);
                    View.State = partyLocation.Address.State;
                    //View.Region = partyLocation.Address.Region;
                    View.City = partyLocation.Address.City;
                }
                else
                {
                    View.Country = string.Empty;
                    View.State = string.Empty;
                }
            }
            else
            {
                SetFieldsBlank();
                View.Party = View.SelectedPartyName;
            }
            
        }

        //private void LoadLocation()
        //{
        //    long partyId;
        //    int locationId;
        //    string[] splitItems = this.SelectedPartyLocationValue.Split('/');

        //    helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyLocationPresenter", "SelectedPartyLocation.Split('/') has length: " + splitItems.Length.ToString());

        //    if (splitItems.Length == 2)
        //    {
        //        if (int.TryParse(splitItems[1], out locationId))
        //        {
        //            View.LocationId = locationId;

        //            PartyLocation partyLocation = this.partyLocationRepositoryService.GetPartyLocationByLocationId(View.LocationId);
        //            View.PartyId = partyLocation.PartyId;

        //            View.Party = partyLocation.PartyName;
        //            View.LocationType = partyLocation.LocationType;
        //            View.LocationName = partyLocation.LocationName;
        //            View.LocationCode = partyLocation.Code;
        //            View.GLN = partyLocation.GLN;
        //            View.Description = partyLocation.Description;
        //            View.Latitude = Convert.ToString(partyLocation.Latitude);
        //            View.Longitude = Convert.ToString(partyLocation.Longitude);

        //            View.IsActive = partyLocation.IsActive;
        //            View.RequiresAddress = partyLocation.RequiresAddress;
        //            if (partyLocation.RequiresAddress)
        //            {
        //                partyLocation.Address = this.locationRepositoryService.GetLocationAddress(View.LocationId);
        //                View.AddressId = partyLocation.Address.AddressId;
        //                View.Address1 = partyLocation.Address.Line1;
        //                View.Address2 = partyLocation.Address.Line2;
        //                View.ZipCode = partyLocation.Address.Zip;
        //                View.Country = partyLocation.Address.Country;
        //                View.StateList = this.stateRepositoryService.GetStateList(View.Country);
        //                View.State = partyLocation.Address.State;
        //                //View.Region = partyLocation.Address.Region;
        //                View.City = partyLocation.Address.City;
        //            }
        //            else
        //            {
        //                View.Country = string.Empty;
        //                View.State = string.Empty;
        //            }
        //        }
        //        else
        //        {
        //            if (long.TryParse(splitItems[0], out partyId))
        //            {
        //                View.PartyId = partyId;
        //                //party = this.partyRepositoryService.GetPartyByPartyId(View.PartyId);

        //                View.Party = this.SelectedParty.Split('/')[0].Trim();
        //                View.LocationType = View.PartyLocationType.Name;
        //                View.RequiresAddress = View.PartyLocationType.RequiresAddress;
        //                View.State = string.Empty;
        //            }
        //        }
        //    }
        //    else //If length is 1 then the root node is selected
        //    {
        //        View.CountryList = this.countryRepositoryService.GetCountryList();
        //        View.StateList = new List<State>();
        //    }

        //    helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyLocationPresenter", "Selected party location has PartyId: " + Convert.ToString(View.PartyId));
        //}

        public override void OnViewInitialized()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyLocationPresenter", "OnViewInitialized() is invoked.");
            try
            {
                this.FetchLocationTypeForParty();
                this.PopulatePartyLocations();
                //this.BindTreeNodeCollection();
                //View.PartyLocationNodeList = partyLocationsTreeNodes;
                View.CountryList = this.countryRepositoryService.GetCountryList();
                this.SetFieldsBlank();
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <returns>Result Status - Created, Updated</returns>
        public Constants.ResultStatus Save()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyLocationPresenter", "Save() is invoked.");

            Constants.ResultStatus resultStatus;

            try
            {
                resultStatus = this.CheckDuplicateLocationName(View.LocationName.Trim(), View.LocationId);
                if (resultStatus == Constants.ResultStatus.Ok)
                {
                    //Set the values in the partyLocation
                    PartyLocation partyLocation = new PartyLocation();
                    partyLocation.LocationTypeId = View.PartyLocationType.LocationTypeId;
                    partyLocation.PartyId = View.SelectedPartyId;
                    partyLocation.LocationId = View.LocationId;
                    partyLocation.LocationName = View.LocationName;
                    partyLocation.Description = View.Description;
                    partyLocation.IsActive = View.IsActive;

                    if (!string.IsNullOrEmpty(View.Latitude))
                        partyLocation.Latitude = Convert.ToDecimal(View.Latitude);
                    if (!string.IsNullOrEmpty(View.Longitude))
                        partyLocation.Longitude = Convert.ToDecimal(View.Longitude);
                    partyLocation.Code = View.LocationCode;
                    partyLocation.GLN = View.GLN;


                    Address address = new Address();
                    address.AddressId = View.AddressId;
                    address.City = View.City;
                    address.Country = View.Country;
                    address.Line1 = View.Address1;
                    address.Line2 = View.Address2;
                    address.State = View.State;
                    //address.Region = View.Region;
                    address.Zip = View.ZipCode;
                    int locationId = locationRepositoryService.SavePartyLocationAndAddress(partyLocation, View.RequiresAddress, address);

                    helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyLocationPresenter", "Party location saved with locationId: " + locationId.ToString());
                    if (partyLocation.LocationId == 0)
                        resultStatus = Constants.ResultStatus.Created;
                    else
                        resultStatus = Constants.ResultStatus.Updated;
                }
            }
            catch
            {
                throw;
            }

            return resultStatus;
        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        /// <returns>Result Status - Deleted</returns>
        public Constants.ResultStatus Delete()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyLocationPresenter", "Delete() is invoked.");
            Constants.ResultStatus resultStatus;
            try
            {
                this.locationRepositoryService.DeleteLocation(View.LocationId);
                resultStatus = Constants.ResultStatus.Deleted;

                helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyLocationPresenter", "Deleted party location for locationId: " + Convert.ToString(View.LocationId));
            }
            catch
            {
                throw;
            }
            return resultStatus;
        }

        /// <summary>
        /// Loads the states.
        /// </summary>
        public void LoadStates()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyLocationPresenter", "LoadStates() is invoked.");
            try
            {
                List<State> stateList = this.stateRepositoryService.GetStateList(View.Country);
                View.StateList = stateList;
                helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyLocationPresenter", "# rows returned by GetStateList() method: " + Convert.ToString(stateList.Count) + " for country code: " + View.Country);
            }
            catch
            {
                throw;
            }
        }

        #endregion
    }
}




