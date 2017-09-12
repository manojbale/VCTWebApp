using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCTWeb.Core.Domain
{
    [Serializable]
    public class Cases
    {
        public Int64 CaseId { get; set; }
        public string CaseNumber { get; set; }
        public string InventoryType { get; set; }
        public DateTime SurgeryDate { get; set; }
        public DateTime ShippingDate { get; set; }
        public DateTime RetrievalDate { get; set; }
        public Int64? PartyId { get; set; }
        public string ProcedureName { get; set; }
        //public Int64? KitFamilyId { get; set; }
        public string PatientName { get; set; }
        public string Physician { get; set; }
        public string SpecialInstructions { get; set; }
        public string SalesRep { get; set; }
        public string CaseStatus { get; set; }
        public string CaseType { get; set; }
        public string Technicians { get; set; }
        public double? TotalPrice { get; set; }
        public int LocationId { get; set; }
        public int Quantity { get; set; }

        // properties from other table
        public string PartyName { get; set; }
        public string PartyType { get; set; }
        public string ShipFromLocation { get; set; }
        //public string KitFamilyName { get; set; }
        //public string KitFamilyDesc { get; set; }        
    }

    //[Serializable]
    //public class CasePartDetail
    //{
    //    public Int64 CasePartId { get; set; }
    //    public Int64 CaseId { get; set; }
    //    public string PartNum { get; set; }
    //    public Int64 LocationPartDetailId { get; set; }        
    //}

    public class CaseSmall
    {
        public Int64 CaseId { get; set; }
        public string CaseNumber { get; set; }
    }

    public class CaseInvoiceAdvisory
    {
        public string InventoryType { get; set; }
        public string Particular { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
    }

    public class CaseStatusCls
    {
        public string CaseStatus { get; set; }
        public string Description { get; set; }
    }

    public class CaseType
    {
        public string CaseTypeName { get; set; }
    }

    public class CaseMerge
    {
        public string CaseValues { get; set; }
        public DateTime SurgeryDate { get; set; }
    }

    public class Physician
    {
        public string PhysicianName { get; set; }
       // public int PhysicianId { get; set; }
    }

    [Serializable]
    public class ItemDetail
    {
        public long CasePartId { get; set; }
        public string PartNum { get; set; }
        public string LotNum { get; set; }
        public Int64 LocationPartDetailId { get; set; }
    }


    [Serializable]
    public class CasePartDetailGroup
    {
        public string PartNum { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public bool Selected { get; set; }
    }


    [Serializable]
    public class CaseKitFamilyDetailGroup
    {
        public string KitFamilyName { get; set; }
        public long KitFamilyId { get; set; }
        public string KitFamilyDescription { get; set; }
        public int Quantity { get; set; }
        public bool Selected { get; set; }
    }

    [Serializable]
    public class NewProductTransfer
    {        
        public Int64 KitFamilyId { get; set; }
        public DateTime TransferDate { get; set; }
        public Int32 LocationId { get; set; }
        public String LocationName { get; set; }
        public string LocationTypeName { get; set; }
        public int Quantity { get; set; }
        public Int32 LocationStatus { get; set; }
        
    }

    [Serializable]
    public class ViewCancelTransaction
    {
        public Int64 CaseId { get; set; }
        public string CaseType { get; set; }
        public string InventoryType { get; set; }
        public string CaseNumber { get; set; }
        public string RequestNumber { get; set; }
        public string CaseStatus { get; set; }
        public DateTime SurgeryDate { get; set; }                
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }               
        public int Count { get; set; }
        public string PartNum { get; set; }
        public string Description { get; set; }
        public string LotNum { get; set; }
        public string KitFamilyName { get; set; }
        public int Quantity { get; set; }
        public string KitNumber { get; set; }
        public string TransferNumber { get; set; }
        public string LocationType { get; set; }
        public Int64 BuildKitId { get; set; }
        public string ExpiryDate { get; set; }
        public bool IsNearExpiry { get; set; }
               
        
        public string DispositionType { get; set; }
        public string Remarks { get; set; }
        public string RowType { get; set; }

        public Int64 PartyId { get; set; }
        public string PartyName { get; set; }
        public Int32 LocationId { get; set; }
        public string FromLocationName { get; set; }
        public Int32 ToLocationId { get; set; }
        public string ToLocationName { get; set; }
        public string LTParty { get; set; }
        public string FromLTName { get; set; }
        public string ToLTName { get; set; }
        public int TotalRecordCount { get; set; }


    }

    [Serializable]
    public class Default
    {
        public string PartNum { get; set; }
        public string Description { get; set; }
        public int Count { get; set; }
        public int InventoryUtilizationDay { get; set; }

        public string Title { get; set; }
        public Int64 PartyId { get; set; }
        public string BranchName { get; set; }
        public string HospitalName { get; set; }
        public Decimal Longitude { get; set; }
        public Decimal Latitude { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string ShippingDate { get; set; }
        public string CaseNumber { get; set; }
        public string CaseType { get; set; }
        public string InventoryType { get; set; }
        public string CaseStatus { get; set; }

    }

    [Serializable]
    public class ReturnInventoryRMA
    {
        public Int32 Index { get; set; }
        public Int64 CaseId { get; set; }
        public Int64 LocationPartDetailId { get; set; }
        public string KitNumber { get; set; }
        public string PartNum { get; set; }
        public string LotNum { get; set; }
        public string Description { get; set; }
        public string PartStatus { get; set; }
        public bool Return { get; set; }
        public bool SeekReturn { get; set; }
        public string DispositionType { get; set; }
        public int DispositionTypeId { get; set; }
        public int Qty { get; set; }
        public Int64 CasePartId { get; set; }
    }

    [Serializable]
    public class KitHistoryCaseDetail
    {
        public DateTime SurgeryDate { get; set; }
        public string CaseNumber { get; set; }
        public string PartyName { get; set; }
        public string ProcedureName { get; set; }
        public string SalesRep { get; set; }
        public string CaseStatus { get; set; }
        public string PartNum { get; set; }
        public string LotNum { get; set; }
        public string Description { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
