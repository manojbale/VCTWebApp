using System;
using System.Collections.Generic;
using System.Text;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public interface IKitListingView
    {
        List<KitListing> KitListingList { set; }
        List<KitTable> KitTableList { get; set; }
        List<KitFamily> KitFamilyList { set; }
        //List<Procedures> ProcedureList { set; }
        
        KitTable IndiviualKitItem { get; set; }

        Int64 SelectedKitFamilyId { get; }

        string KitTableXml { get; }

        string Procedure { get; set; }

        string SelectedKitNumber { get; }

        string KitNumber { get; set; }

        string KitName { get; set; }

        string KitDescription { get; set; }

        int? NumberOfSets { get; }

        string Aisle { get; }

        string Row { get; }

        string Tier { get; }

        DateTime? DateCreated { get; }

        DateTime? PMSchedule { get; }

        string Lubricate { get; }

        bool IsManuallyAdded { get; }

        Int64? KitFamilyId { get; set; }

        String KitFamily { get; set; }

        String KitFamilyHead { get; }
        
        int LocationId { get; }

        bool IsActive { get; set; }

        Decimal RentalFee { get; set; }

    }

    
}




