using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public interface IPartyLocationView
    {
        //TreeNodeCollection PartyLocationNodeList { set; get; }
        List<Party> PartyLocationList { set; }
        List<Country> CountryList { set; }
        List<State> StateList { set; }

        string Party { set; }
        string LocationType { set; get; }
        string LocationName { set; get; }
        string LocationCode { set; get; }
        string GLN { set; get; }
        string Description { set; get; }
        string Latitude { set; get; }
        string Longitude { set; get; }
        string Address1 { set; get; }
        string Address2 { set; get; }
        string City { set; get; }
        string Country { set; get; }
        string ZipCode { set; get; }
        string State { set; get; }
        //string Region { set; get; }
        bool RequiresAddress { set; get; }
        bool IsActive { get; set; }

        long SelectedPartyId { get; }
        string SelectedPartyName { get; }
        //long LocationTypeId { set; get; }
        //long ParentLocationId { set; get; }
        int LocationId { set; get; }
        int AddressId { set; get; }

        LocationType PartyLocationType { get; set; }
    }
}




