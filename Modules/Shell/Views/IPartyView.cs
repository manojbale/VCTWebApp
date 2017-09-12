using System;
using System.Collections.Generic;
using System.Text;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public interface IPartyView
    {
        string Name { get; set; }
        string Code { get; set; }
        string Description { get; set; }
        Int64 PartyTypeId { get; set; }
        //int? LinkedLocationId { get; set; }
        string CompanyPrefix { get; set; }
        int ShippingDaysGap { get; set; }
        int RetrievalDaysGap { get; set; }
        bool IsActive { get; set; }
        bool Owner { get; set; }

        string Latitude { set; get; }
        string Longitude { set; get; }
        string Address1 { set; get; }
        string Address2 { set; get; }
        string City { set; get; }
        string Country { set; get; }
        string ZipCode { set; get; }
        string State { set; get; }
        int AddressId { set; get; }

        string PartyLocationIds { get; }

        Int64 SelectedPartyId { get; }
        List<Party> PartyList { set; }
        List<PartyType> PartyTypeList { set; }
        List<PartyLinkedLocation> PartyLinkedLocationList { set; }
        List<Country> CountryList { set; }
        List<State> StateList { set; }
    }
}




