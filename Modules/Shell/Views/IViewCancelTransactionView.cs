using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public interface IViewCancelTransactionView
    {

        //List<CaseType> CaseTypeList { set; }
        List<ViewCancelTransaction> CasesOverAllList { set; }
        List<ViewCancelTransaction> CasesList { set; }
        //List<ViewCancelTransaction> ChildList { get; set;}
        List<DispositionType> DispositionTypeList { set; }

        Int64 CaseId { get; set; }        
        DateTime StartDate { get; }
        DateTime EndDate { get; }
        Int32 LocationId { get; }
        String CaseStatus { get; set; }

        string CaseTypeSearch { get; }
        string CaseNumberSearch { get; }
        string InvTypeSearch { get; }
        string PartyNameSearch { get; }
        string LocationTypeSearch { get; }
        string CaseStatusSearch { get; }
        int DispositionTypeId { get; }
        string Remarks { get; }
        int PageSize { get;  set; }

    }

}
