using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public interface IInventoryTransferView
    {
        List<InventoryTransfer> InventoryTransKitList { set; }
        List<InventoryTransfer> InventoryTransPartList { set; }
        List<InventoryTransfer> RegionBranchList { set; }
        List<InventoryTransfer> ToLocationList { get; set; }
        List<KitFamilyParts> KitFamilyPartsList { get; set; }

        Int32 FromLocationId { get; }
        string TableXml { get; }
        int InventoryDays { get; set; }
        VCTWeb.Core.Domain.Constants.InventoryType InventoryType { get; }
    }
}
