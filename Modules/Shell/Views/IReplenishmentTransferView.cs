using System;
using System.Collections.Generic;
using System.Text;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public interface IReplenishmentTransferView
    {
        List<ReplenishmentTransfer> ReplenishmentTransferList { set; }
        
        List<Region> RegionList { set; }
        List<SalesOffice> SalesOfficeList { set; }
        List<Party> PartyList { set; }

        int SelectedLocationId { get; }
        long SelectedPartyId { get; }

        DateTime RequiredOn { get; }
    }
}




