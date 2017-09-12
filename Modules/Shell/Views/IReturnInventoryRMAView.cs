using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public interface IReturnInventoryRMAView
    {
        List<ReturnInventoryRMA> ReturnInventoryKitList { set; get; }
        List<ReturnInventoryRMA> ReturnInventoryPartList { set; get; }        
        List<Party> PartyList { set; }
        List<Region> RegionList { set; }
        List<Corp> CorpList { set; }
        string CaseNum { get; }
        string TableXml { get; }
        string ReturnFrom { get; }
        string InventoryType { get; }
        Int64 PartyId { get; }
        Int32 ToLocationId { get; }
        List<DispositionType> DispositionList { set; }
    }
}
