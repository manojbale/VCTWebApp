using System;
using System.Collections.Generic;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public interface IHospitalInventoryTransfer
    {
        List<VCTWeb.Core.Domain.Party> PartyList { set; }
        List<PartyAvailableCatalog> PartyAvailableCatalogList { get; set; }
        List<HospitalInventoryTransfer> PartList { get; set; }
        string InventoryType { get; }
        Int64 FromParty { get; }
        Int64 ToParty { get; }
        string TableXml { get; }
    }
}
