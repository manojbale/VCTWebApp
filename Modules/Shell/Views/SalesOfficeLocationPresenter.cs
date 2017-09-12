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
    public class SalesOfficeLocationPresenter : Presenter<ISalesOfficeLocationView>
    {
        #region Instance Variables

        private Helper helper = new Helper();

        private readonly LocationRepository locationRepositoryService;
        private readonly CountryRepository countryRepositoryService;
        private readonly ContactRepository contactRepositoryService;
        private readonly AddressRepository addressRepositoryService;
        private readonly StateRepository stateRepositoryService;


        #region Variables for generating the hirarchy

        private List<Location> lstBranch;
        private List<Location> lstSatellite;

        private TreeNodeCollection locationsTreeNodes;

        #endregion

        #endregion

        #region Public Properties

        public string SelectedParentLocationValue { get; set; }
        public string SelectedParentLocation { get; set; }

        #endregion

        #region Constructors

        public SalesOfficeLocationPresenter()
            : this(
            new CountryRepository(),
            new LocationRepository(HttpContext.Current.User.Identity.Name),
            new ContactRepository(HttpContext.Current.User.Identity.Name),
            new StateRepository(),
            new AddressRepository())
        {
        }

        public SalesOfficeLocationPresenter(
            CountryRepository countryRepository,
            LocationRepository locationRepository,
            ContactRepository contactRepository,
            StateRepository stateRepository,
            AddressRepository addressRepository)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "SalesOfficeLocationPresenter", "Constructor is invoked.");

            this.countryRepositoryService = countryRepository;
            this.locationRepositoryService = locationRepository;
            this.contactRepositoryService = contactRepository;
            this.stateRepositoryService = stateRepository;
            this.addressRepositoryService = addressRepository;
        }

        #endregion

        #region Private Methods

        private void BindTreeNodeCollection()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "SalesOfficeLocationPresenter", "BindTreeNodeCollection() is invoked.");
            try
            {
                //lstBranch = this.locationRepositoryService.FetchAllLocationsByLocationType(View.BranchLocationType.LocationTypeId);
                //helper.LogInformation(HttpContext.Current.User.Identity.Name, "SalesOfficeLocationPresenter", "No. of Records fetched with call to FetchAllBranchLocations() is " + Convert.ToString(lstBranch.Count));

                //lstSatellite = this.locationRepositoryService.FetchAllLocationsByLocationType(View.SatelliteLocationType.LocationTypeId);
                //helper.LogInformation(HttpContext.Current.User.Identity.Name, "SalesOfficeLocationPresenter", "No. of Records fetched with call to FetchAllSatelliteLocations() is " + Convert.ToString(lstSatellite.Count));

                locationsTreeNodes = new TreeNodeCollection();

                //TreeNode tmpTNode = GetNode("Add " + View.BranchLocationType.Name, string.Empty);
                //this.locationsTreeNodes.Add(tmpTNode);

                //foreach (Location location in this.lstBranch)
                //{
                //    TreeNode node = GetNode(location.LocationName, location.LocationId.ToString());
                //    this.locationsTreeNodes.Add(node);
                //    AddLocations(node, location.LocationId);
                //    node.CollapseAll();
                //}

                List<Location> lstLocations = this.locationRepositoryService.FetchAllLocations();
                var Areas = lstLocations.Where(t => t.LocationType == "Area").ToList();
                //var Regions = lstLocations.Where(t => t.LocationTypeId == 5).ToList();
                //var SOL = lstLocations.Where(t => t.LocationTypeId == 6).ToList();

                TreeNode AddAreaNode = new TreeNode("Add Area");
                locationsTreeNodes.Add(AddAreaNode);

                foreach (Location area in Areas)
                {                    
                    TreeNode areaNode = new TreeNode(area.LocationName, area.LocationId.ToString());
                    locationsTreeNodes.Add(areaNode);
                    var Regions = lstLocations.Where(t => t.LocationType == "Region" && t.ParentLocationId == area.LocationId).ToList();
                    TreeNode AddRegionNode = new TreeNode("Add Region");
                    areaNode.ChildNodes.Add(AddRegionNode);
                    foreach (Location region in Regions)
                    {                        
                        TreeNode regionNode = new TreeNode(region.LocationName, region.LocationId.ToString());
                        areaNode.ChildNodes.Add(regionNode);
                        //areaNode.CollapseAll();
                        var SOL = lstLocations.Where(t => t.LocationType == "Sales Office Location" && t.ParentLocationId == region.LocationId).ToList();
                        TreeNode AddSOLNode = new TreeNode("Add Sales Office Location");
                        regionNode.ChildNodes.Add(AddSOLNode);
                        foreach (Location sol in SOL)
                        {                            
                            regionNode.ChildNodes.Add(new TreeNode(sol.LocationName, sol.LocationId.ToString()));
                            //regionNode.CollapseAll();
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private TreeNode GetNode(string text, string value)
        {
            TreeNode tNode = new TreeNode();
            tNode.Text = text;
            tNode.Value = value;

            return tNode;
        }

        private void AddLocations(TreeNode node, int parentLocationId)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "SalesOfficeLocationPresenter", "AddLocations() is invoked.");
            
            TreeNode tmpTNode = GetNode("Add " + View.SatelliteLocationType.Name, string.Empty);
            node.ChildNodes.Add(tmpTNode);
            
            var result = this.lstSatellite.Where(satelliteloc => satelliteloc.ParentLocationId == parentLocationId).ToList();
            if (result != null && result.Count > 0)
            {
                foreach (Location location in result)
                {
                    TreeNode tmpNode = GetNode(location.LocationName, location.LocationId.ToString());
                    node.ChildNodes.Add(tmpNode);
                }
            }
        }

        private void SetFieldsBlank()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "SalesOfficeLocationPresenter", "SetFieldsBlank() is invoked.");

            View.ParentLocationId = 0;
            View.LocationTypeId = 0;
            View.LocationId = 0;
            View.AddressId = 0;

            View.ParentLocation = string.Empty;
            View.LocationType = string.Empty;
            View.LocationName = string.Empty;
            View.LocationCode = string.Empty;
            View.GLN = string.Empty;
            View.Description = string.Empty;
            View.Latitude = string.Empty;
            View.Longitude = string.Empty;
            View.Address1 = string.Empty;
            View.Address2 = string.Empty;
            View.ZipCode = string.Empty;
            View.Country = null;
            View.State = string.Empty;
            View.City = string.Empty;
            //View.Region = string.Empty;
            View.RequiresAddress = false;
        }

        //private Constants.ResultStatus CheckIfOnlyOnePrimaryIsSelected()
        //{
        //    helper.LogInformation(HttpContext.Current.User.Identity.Name, "UserPresenter", "CheckIfOnlyOnePrimaryIsSelected() is invoked.");

        //    var result = View.LocationContactList.Where(locationContact => locationContact.IsPrimary == true).ToList();
        //    if (result != null && result.Count > 1)
        //    {
        //        return Constants.ResultStatus.SelectOnlyOneItem;
        //    }
        //    else
        //    {
        //        return Constants.ResultStatus.Ok; 
        //    }            
        //}

        private Constants.ResultStatus CheckLocationStatus(string locationName, int locationId)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "SalesOfficeLocationPresenter", "CheckDuplicateLocationName() is invoked with parameters values - Location Name: " + locationName + " and Location Id: " + locationId.ToString());
            if (locationRepositoryService.CheckDuplicateLocationName(locationName, locationId))
            {
                return Constants.ResultStatus.DuplicateLocationName;
            }            
            if (!View.IsActive && locationRepositoryService.CheckLocationInUse(locationId))
            {
                View.IsActive = true;
                return Constants.ResultStatus.InUse;
            }
            return Constants.ResultStatus.Ok;
        }

        private void FetchLocationTypeForBranchAndSatellite()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "SalesOfficeLocationPresenter", "FetchLocationTypeForBranchAndSatellite() is invoked");
            View.BranchLocationType = locationRepositoryService.FetchLocationTypeForBranch();
            View.SatelliteLocationType = locationRepositoryService.FetchLocationTypeForSatellite();
        }

        #endregion

        #region Public Overrides

        public override void OnViewLoaded()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "SalesOfficeLocationPresenter", "OnViewLoaded() is invoked.");
            try
            {
                this.SetFieldsBlank();

                //int parentLocationId;
                int locationId;
                string[] splitItems = this.SelectedParentLocationValue.Split('/');

                helper.LogInformation(HttpContext.Current.User.Identity.Name, "SalesOfficeLocationPresenter", "SelectedParentLocationValue.Split('/') has length: " + splitItems.Length.ToString());

                var lastitem = splitItems.Last();

                if (int.TryParse(lastitem, out locationId))
                {
                    View.LocationId = locationId;
                    Location location = this.locationRepositoryService.GetLocationByLocationId(View.LocationId);
                    View.ParentLocationId = location.ParentLocationId;
                    View.LocationTypeId = location.LocationTypeId;
                    View.ParentLocation = this.SelectedParentLocation;
                    View.LocationType = location.LocationType;
                    View.LocationName = location.LocationName;
                    View.LocationCode = location.Code;
                    View.GLN = location.GLN;
                    View.IsActive = location.IsActive;
                    View.Description = location.Description;
                    View.Latitude = Convert.ToString(location.Latitude);
                    View.Longitude = Convert.ToString(location.Longitude);
                    View.RequiresAddress = location.RequiresAddress;
                    if (View.RequiresAddress)
                    {
                        location.Address = this.locationRepositoryService.GetLocationAddress(View.LocationId);
                        View.AddressId = location.Address.AddressId;
                        View.Address1 = location.Address.Line1;
                        View.Address2 = location.Address.Line2;
                        View.ZipCode = location.Address.Zip;
                        View.Country = location.Address.Country;
                        View.StateList = this.stateRepositoryService.GetStateList(View.Country);
                        View.State = location.Address.State;
                        View.City = location.Address.City;
                    }
                }
                else
                {
                    switch (lastitem)
                    {
                        case "Add Area":
                            View.ParentLocationId = null;
                            View.LocationType = "Area";
                            View.RequiresAddress = false;                            
                            break;
                        case "Add Region":
                            View.ParentLocationId = Convert.ToInt32(splitItems[0]);
                            View.LocationType = "Region";
                            View.RequiresAddress = false;
                            View.ParentLocation = this.SelectedParentLocation;
                            break;
                        case "Add Sales Office Location":
                            View.ParentLocationId = Convert.ToInt32(splitItems[1]);
                            View.LocationType = "Sales Office Location";
                            View.RequiresAddress = true;
                            View.ParentLocation = this.SelectedParentLocation;
                            break;
                        default:
                            break;
                    }
                }

                //if (splitItems.Length == 2)
                //{
                //    if (int.TryParse(splitItems[1], out locationId))
                //    {
                //        View.LocationId = locationId;

                //        Location location = this.locationRepositoryService.GetLocationByLocationId(View.LocationId);
                //        View.ParentLocationId = location.ParentLocationId;
                //        View.LocationTypeId = View.SatelliteLocationType.LocationTypeId;
                //        View.ParentLocation = this.SelectedParentLocation;
                //        View.LocationType = View.SatelliteLocationType.Name;
                //        View.LocationName = location.LocationName;
                //        View.LocationCode = location.Code;
                //        View.GLN = location.GLN;
                //        View.Description = location.Description;
                //        View.Latitude = Convert.ToString(location.Latitude);
                //        View.Longitude = Convert.ToString(location.Longitude);
                        
                //        View.RequiresAddress = View.SatelliteLocationType.RequiresAddress;
                //        if (View.RequiresAddress)
                //        {
                //            location.Address = this.locationRepositoryService.GetLocationAddress(View.LocationId);
                //            View.AddressId = location.Address.AddressId;
                //            View.Address1 = location.Address.Line1;
                //            View.Address2 = location.Address.Line2;
                //            View.ZipCode = location.Address.Zip;
                //            View.Country = location.Address.Country;
                //            View.StateList = this.stateRepositoryService.GetStateList(View.Country);
                //            View.State = location.Address.State;
                //            View.Region = location.Address.Region;
                //            View.City = location.Address.City;
                //        }
                //        else
                //        {
                //            View.Country = string.Empty;
                //            View.State = string.Empty;
                //        }
                //    }
                //    else
                //    {
                //        if (int.TryParse(splitItems[0], out parentLocationId))
                //        {
                //            View.ParentLocationId = parentLocationId;
                //            View.LocationTypeId = View.SatelliteLocationType.LocationTypeId;
                //            View.ParentLocation = this.SelectedParentLocation;
                //            View.LocationType = View.SatelliteLocationType.Name;
                //            View.RequiresAddress = View.SatelliteLocationType.RequiresAddress;
                //            View.State = string.Empty;
                //        }
                //    }
                //}
                //else //If length is 1 then the root node is selected
                //{
                //    if (int.TryParse(splitItems[0], out locationId))
                //    {
                //        View.LocationId = locationId;

                //        Location location = this.locationRepositoryService.GetLocationByLocationId(View.LocationId);
                //        View.ParentLocationId = location.ParentLocationId;
                //        View.LocationTypeId = View.BranchLocationType.LocationTypeId;
                //        View.ParentLocation = this.SelectedParentLocation;
                //        View.LocationType = View.BranchLocationType.Name;
                //        View.LocationName = location.LocationName;
                //        View.LocationCode = location.Code;
                //        View.GLN = location.GLN;
                //        View.Description = location.Description;
                //        View.Latitude = Convert.ToString(location.Latitude);
                //        View.Longitude = Convert.ToString(location.Longitude);

                //        View.RequiresAddress = View.BranchLocationType.RequiresAddress;
                //        if (View.RequiresAddress)
                //        {
                //            location.Address = this.locationRepositoryService.GetLocationAddress(View.LocationId);
                //            View.AddressId = location.Address.AddressId;
                //            View.Address1 = location.Address.Line1;
                //            View.Address2 = location.Address.Line2;
                //            View.ZipCode = location.Address.Zip;
                //            View.Country = location.Address.Country;
                //            View.StateList = this.stateRepositoryService.GetStateList(View.Country);
                //            View.State = location.Address.State;
                //            View.Region = location.Address.Region;
                //            View.City = location.Address.City;
                //        }
                //        else
                //        {
                //            View.Country = string.Empty;
                //            View.State = string.Empty;
                //        }
                //    }
                //    else
                //    {
                //        View.ParentLocationId = null;
                //        View.LocationTypeId = View.BranchLocationType.LocationTypeId;
                //        View.ParentLocation = this.SelectedParentLocation;
                //        View.LocationType = View.BranchLocationType.Name;
                //        View.RequiresAddress = View.BranchLocationType.RequiresAddress;
                //        View.State = string.Empty;
                //    }
                //}

                //View.LocationContactList = this.contactRepositoryService.FetchAllContactsByLocationId(View.LocationId);


                ////Set the grant status for the current user roles
                //foreach (Role userRole in allContactList)
                //{
                //    foreach (Role role in allContactList)
                //    {
                //        if (userRole.RoleId == role.RoleId)
                //        {
                //            role.GrantRole = true;
                //        }
                //    }
                //}

                //View.RoleList = allContactList;

                helper.LogInformation(HttpContext.Current.User.Identity.Name, "SalesOfficeLocationPresenter", "Selected location has LocationId: " + Convert.ToString(View.LocationId));
            }
            catch
            {                
                throw;
            }
        }

        public override void OnViewInitialized()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "SalesOfficeLocationPresenter", "OnViewInitialized() is invoked.");
            try
            {
                this.FetchLocationTypeForBranchAndSatellite();
                this.BindTreeNodeCollection();
                //View.LocationContactList = this.contactRepositoryService.FetchAllContactsByLocationId(0);
                View.SalesOfficeLocationNodeList = locationsTreeNodes;
                View.CountryList = this.countryRepositoryService.GetCountryList();
                //View.RegionList = this.addressRepositoryService.FetchAllRegions();
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
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "SalesOfficeLocationPresenter", "Save() is invoked.");

            Constants.ResultStatus resultStatus;

            try
            {
                resultStatus = this.CheckLocationStatus(View.LocationName.Trim(), View.LocationId);
                if (resultStatus == Constants.ResultStatus.Ok)
                {
                    //resultStatus = CheckIfOnlyOnePrimaryIsSelected();
                    if (resultStatus == Constants.ResultStatus.Ok)
                    {
                        //Set the values in the Location
                        Location location = new Location();
                        location.LocationTypeId = View.LocationTypeId;
                        location.ParentLocationId = View.ParentLocationId;
                        location.LocationId = View.LocationId;
                        location.LocationName = View.LocationName;
                        location.Description = View.Description;
                        location.IsActive = View.IsActive;

                        if (!string.IsNullOrEmpty(View.Latitude))
                            location.Latitude = Convert.ToDecimal(View.Latitude);
                        if (!string.IsNullOrEmpty(View.Longitude))
                            location.Longitude = Convert.ToDecimal(View.Longitude);
                        location.Code = View.LocationCode;
                        location.GLN = View.GLN;

                        location.LocationType = View.LocationType;

                        //var result = View.LocationContactList.Where(locationContact => locationContact.ContactSelected == true).ToList();

                        Address address = new Address();
                        if (View.LocationType == "Sales Office Location")
                        {
                            address.AddressId = View.AddressId;
                            address.City = View.City.Trim();
                            address.Country = View.Country.Trim();
                            address.Line1 = View.Address1.Trim();
                            address.Line2 = View.Address2.Trim();
                            address.State = View.State.Trim();
                            //address.Region = View.Region;
                            address.Zip = View.ZipCode.Trim(); 
                        }

                        int locationId = locationRepositoryService.SaveLocationAndAddress(location, View.RequiresAddress, address);


                        helper.LogInformation(HttpContext.Current.User.Identity.Name, "SalesOfficeLocationPresenter", "Location saved with locationId: " + locationId.ToString());

                        if (location.LocationId == 0)
                            resultStatus = Constants.ResultStatus.Created;
                        else
                            resultStatus = Constants.ResultStatus.Updated;
                    }
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
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "SalesOfficeLocationPresenter", "Delete() is invoked.");
            Constants.ResultStatus resultStatus;
            try
            {
                this.locationRepositoryService.DeleteLocation(View.LocationId);
                resultStatus = Constants.ResultStatus.Deleted;

                helper.LogInformation(HttpContext.Current.User.Identity.Name, "SalesOfficeLocationPresenter", "Deleted location for locationId: " + Convert.ToString(View.LocationId));
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
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "SalesOfficeLocationPresenter", "LoadStates() is invoked.");
            try
            {
                List<State> stateList = this.stateRepositoryService.GetStateList(View.Country);
                View.StateList = stateList;
                helper.LogInformation(HttpContext.Current.User.Identity.Name, "SalesOfficeLocationPresenter", "# rows returned by GetStateList() method: " + Convert.ToString(stateList.Count) + " for country code: " + View.Country);
            }
            catch
            {
                throw;
            }
        }

        #endregion
    }
}




