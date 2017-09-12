using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCTWebApp.Shell.Views
{
    public interface IVirtualReCheckInView
    {
        string CaseType { get; }
        long SelectedCaseId { get; }
        string InventoryType { get; set; }

        string CaseNumber { set; }
        DateTime SurgeryDate { set; }
        string ShipFromLocation { set; }
        string ShipToLocation { set; }
        string ShipToLocationType { set; }

        //long KitFamilyId { get; set; }
        //string KitFamily { set; }
        //string KitFamilyDesc { set; }
        int Quantity { set; }
        DateTime? ShippingDate { set; }
        DateTime? RetrievalDate { set; }
        string ProcedureName { set; }
        Int64? PartyId { get; set; }

        List<VCTWeb.Core.Domain.CaseSmall> PendingCasesList { set; }
        List<VCTWeb.Core.Domain.VirtualCheckOut> CheckedInKitList { set; }
        List<VCTWeb.Core.Domain.VirtualCheckOut> CheckedInPartList { set; }
        List<VCTWeb.Core.Domain.CaseType> CaseTypeList { set; }
        string TableXml { get; }
    }
}

