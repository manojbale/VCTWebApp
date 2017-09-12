using System;
using System.Collections.Generic;
using System.Text;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public interface ICaseFindMatchView
    {
        List<KitSearchResult> KitSearchResultList { set; }
        KitSearchResult kitSearchResultForRequestedLocation { get; set; }
        KitSearchResult kitSearchResultForShipToLocation { get; set; }

        long SelectedCaseId { get; }
        int SelectedLocationId { get; }
        int RequestedQuantity { get; }

        int SearchQuantity { get; }
    }
}




