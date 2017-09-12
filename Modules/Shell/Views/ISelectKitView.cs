using System;
using System.Collections.Generic;
using System.Text;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public interface ISelectKitView
    {
        List<KitListing> KitListingList { set; }

        string ProcedureName { get; set; }
        string CatalogNumber { get; set; }
    }
}




