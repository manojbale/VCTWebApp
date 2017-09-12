using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;
using System.Web;

namespace VCTWebApp.Shell.Views
{
    public class PartyPresenter : Presenter<IPartyView>
    {
        #region Instance Variables

        private PartyRepository partyRepositoryService;
        private DictionaryRepository dictionaryRepository;

        private Helper helper = new Helper();

        #endregion

        #region Constructors

        public PartyPresenter()
            : this(new PartyRepository(HttpContext.Current.User.Identity.Name))
        {
        }

        public PartyPresenter(PartyRepository partyRepository)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyPresenter", "Constructor is invoked.");

            this.partyRepositoryService = partyRepository;
            this.dictionaryRepository = new DictionaryRepository();
        }

        #endregion

        #region Private Methods
        private Constants.ResultStatus CheckStatusForParty(long PartyId)
        {
            if (!View.IsActive && partyRepositoryService.CheckInUse(PartyId))
            {
                View.IsActive = true;
                return Constants.ResultStatus.InUse;
            }
            return Constants.ResultStatus.Ok;
        }

       
        private void SetFieldsBlank()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyPresenter", "SetFieldsBlank() is invoked.");

            View.Name = string.Empty;
            View.Code = string.Empty;
            View.Description = string.Empty;
            View.PartyTypeId = 0;
            //View.LinkedLocationId = null;
            View.CompanyPrefix = string.Empty;
            View.ShippingDaysGap = 0;
            View.RetrievalDaysGap = 0;
            View.IsActive = true;
            View.Owner = false;

            View.Latitude = string.Empty;
            View.Longitude = string.Empty;
            View.Address1 = string.Empty;
            View.Address2 = string.Empty;
            View.ZipCode = string.Empty;
            View.Country = null;
            View.State = string.Empty;
            View.City = string.Empty;
            View.AddressId = 0;

            View.PartyLinkedLocationList = this.partyRepositoryService.GetPartyLocationByPartyId(0);
        }

        private Party GetSelectedParty()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyPresenter", "GetSelectedParty() is invoked for SelectedPartyd: " + View.SelectedPartyId);

            try
            {
                return this.partyRepositoryService.GetPartyById(View.SelectedPartyId);
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region Public Overrides

        /// <summary>
        /// Loads the states.
        /// </summary>
        public void LoadStates()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyPresenter", "LoadStates() is invoked.");
            try
            {
                List<State> stateList = new StateRepository().GetStateList(View.Country);
                View.StateList = stateList;
                helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyPresenter", "# rows returned by GetStateList() method: " + Convert.ToString(stateList.Count) + " for country code: " + View.Country);
            }
            catch
            {
                throw;
            }
        }

        public override void OnViewLoaded()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyPresenter", "OnViewLoaded is invoked.");

            try
            {
                Party party = this.GetSelectedParty();
                if (party != null)
                {
                    View.Name = party.Name.Trim();
                    View.Code = party.Code.Trim();
                    View.Description = party.Description.Trim();
                    View.PartyTypeId = party.PartyTypeId;
                    //View.LinkedLocationId = party.LinkedLocationId;
                    View.CompanyPrefix = party.CompanyPrefix.Trim();
                    View.ShippingDaysGap = party.ShippingDaysGap;
                    View.RetrievalDaysGap = party.RetrievalDaysGap;
                    View.Owner = party.Owner;
                    View.IsActive = party.IsActive;

                    party.Address = this.partyRepositoryService.GetPartyAddress(View.SelectedPartyId);
                    View.AddressId = party.Address.AddressId;
                    View.Latitude = Convert.ToString(party.Address.Latitude);
                    View.Longitude = Convert.ToString(party.Address.Longitude);
                    View.Address1 = party.Address.Line1;
                    View.Address2 = party.Address.Line2;
                    View.ZipCode = party.Address.Zip;
                    View.Country = party.Address.Country;
                    View.StateList = new StateRepository().GetStateList(View.Country);
                    View.State = party.Address.State;
                    View.City = party.Address.City;

                    View.PartyLinkedLocationList = this.partyRepositoryService.GetPartyLocationByPartyId(View.SelectedPartyId);
                }
            }
            catch
            {
                throw;
            }
        }

        public override void OnViewInitialized()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyPresenter", "OnViewInitialized() is invoked.");
            try
            {
                View.PartyList = this.partyRepositoryService.FetchParties();
                View.PartyTypeList = this.partyRepositoryService.FetchAllPartyTypes();
                View.CountryList = new CountryRepository().GetCountryList();
                //View.LinkedLocationList = new LocationRepository().FetchAllLocations();
                this.SetFieldsBlank();
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Public Methods

        public bool IsPartyExists(string partyName)
        {
            return this.partyRepositoryService.IsPartyExists(partyName);
        }

        public Constants.ResultStatus Save(string partyName)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyPresenter", "Save() is invoked.");

            //Constants.ResultStatus resultStatus = Constants.ResultStatus.Error;
            Constants.ResultStatus resultStatus = CheckStatusForParty(View.SelectedPartyId);
            Boolean IsPartyExists = false;
            if (View.SelectedPartyId == 0)
            {
                IsPartyExists = this.IsPartyExists(View.Name);
                if (IsPartyExists)
                {
                    resultStatus = Constants.ResultStatus.Duplicate;
                    return resultStatus;
                }
            }
            else
            {
                if (View.Name != partyName)
                {
                    IsPartyExists = this.IsPartyExists(View.Name);
                    if (IsPartyExists)
                    {
                        resultStatus = Constants.ResultStatus.Duplicate;
                        return resultStatus;
                    }
                }

            }
            try
            {
                //string mode = "Edit";

                Party party = party = new Party(); //this.GetSelectedParty();
                //if (party == null)
                //{
                //    party = new Party();
                //    mode = "Add";
                //}
                party.PartyId = View.SelectedPartyId;
                party.Name = View.Name;
                party.Code = View.Code;
                party.Description = View.Description;
                party.PartyTypeId = View.PartyTypeId;
                //party.LinkedLocationId = View.LinkedLocationId;
                party.CompanyPrefix = View.CompanyPrefix;
                party.ShippingDaysGap = View.ShippingDaysGap;
                party.RetrievalDaysGap = View.RetrievalDaysGap;
                party.IsActive = View.IsActive;
                party.Owner = View.Owner;

                if (resultStatus == Constants.ResultStatus.Ok)
                {
                    Address address = new Address();
                    address.AddressId = View.AddressId;
                    if (!string.IsNullOrEmpty(View.Latitude))
                        address.Latitude = Convert.ToDecimal(View.Latitude);
                    if (!string.IsNullOrEmpty(View.Longitude))
                        address.Longitude = Convert.ToDecimal(View.Longitude);
                    address.City = View.City.Trim();
                    address.Country = View.Country.Trim();
                    address.Line1 = View.Address1.Trim();
                    address.Line2 = View.Address2.Trim();
                    address.State = View.State.Trim();
                    address.Zip = View.ZipCode.Trim();

                    this.partyRepositoryService.SaveParty(party, address, View.PartyLocationIds);
                    helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyPresenter", "Party '" + party.Name + "' is saved successfully.");

                    //if (mode == "Add")
                    //    resultStatus = Constants.ResultStatus.Created;
                    //else
                    //    resultStatus = Constants.ResultStatus.Updated;
                }
            }
            catch
            {
                throw;
            }

            return resultStatus;
        }



        public Dictionary GetDictionaryRule(string key)
        {
            return this.dictionaryRepository.GetDictionaryRule(key);
        }

        #endregion
    }
}




