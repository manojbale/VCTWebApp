using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public interface IVirtualCheckOutView
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
        
        List<VCTWeb.Core.Domain.CaseSmall> PendingCasesList { set; }
        List<VCTWeb.Core.Domain.VirtualCheckOut> ShippingKitList { set; }
        List<VCTWeb.Core.Domain.VirtualCheckOut> ShippingPartList { set; }
        List<CaseType> CaseTypeList { set; }
        string TableXml { get; }
        string CaseStatus { get; set; }
        //string CaseState { get; set; }
    }
}
