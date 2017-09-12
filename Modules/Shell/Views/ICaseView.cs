using System;
using System.Collections.Generic;
using System.Text;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public interface ICaseView
    {
        List<CaseKitFamilyDetailGroup> KitFamilyList { set; }
        List<Users> SalesRep { set; }
        List<CasePartDetailGroup> PartDetailList { set; }
        //List<CasePartDetailGroup> IndicativePartDetailList { set; }
        string CaseKitFamilyDetailXml { get; }
        string CasePartDetailXml { get; }
        //String CaseKitDetailXml { get; set; }

        long CaseId { get; set; }
        string CaseNumber { get; set; }

        DateTime SurgeryDate { get; set; }
        DateTime? ShippingDate { set; }
        DateTime? RetrievalDate { set; }
        string PatientName { get; set; }
        string SpecialInstructions { get; set; }
        string SelectedSalesRep { get; set; }
        string SelectedProcedureName { get; set; }
        string Physician { get; set; }
        string InventoryType { get; set; }
        double? TotalPrice { get; set; }

        string CaseStatus { get; set; }

        //int Quantity { get; set; }

        //Int64? SelectedKitFamilyId { get; set; }
        Int64? SelectedParty { get; set; }
        string SelectedPartyName { get; set; }
        //string KitFamilyName { set; }
    }
}




