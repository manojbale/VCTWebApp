using System;
using System.Collections.Generic;
using System.Text;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public interface IContactView
    {
        //List<SalesOffice> SalesOfficeList { set; }
        List<Contact> ContactList { set; }
        List<LocationContact> LocationContactList { set; }
        int SelectedContactId { get; }

        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        string Phone { get; set; }
        string Cell { get; set; }
        string Fax { get; set; }
        string SelectedLocationIds { get; }
        //int? LocationId { get; set; }
        bool Active { get; set; }

    }
}




