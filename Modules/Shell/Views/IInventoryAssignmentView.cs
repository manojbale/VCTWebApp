using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public interface IInventoryAssignmentView
    {
        List<CaseType> CaseTypeList { set; }
        List<CaseSmall> PendingCasesList { set; }
        long SelectedCaseId { get; }

        string CaseNumber { set; }
        DateTime SurgeryDate { set; }
        string ShipFromLocation { set; }
        string ShipToLocation { set; }
        string ShipToLocationType { set; }
        
        //string KitFamily { set; }
        //string KitFamilyDesc { set; }
        //long KitFamilyId { get; set; }
        //int Quantity { set; }
        DateTime? ShippingDate { set; }
        DateTime? RetrievalDate { set; }
        string ProcedureName { set; }

        string InventoryType { get; set; }
        
        string SelectedCaseType { get; }

        List<KitFamilSmall> KitDetailList { set; }
        List<ItemDetail> ItemDetailList { set; }
        List<InventoryStockKit> KitstobeAssigned { set; }
        List<InventoryStockPart> LotstobeAssigned { set; }
        string CaseStatus { get; set; }

    }
}
