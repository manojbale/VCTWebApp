using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public interface ICaseScheduleView
    {
        List<Users> SalesRepList { set; }
        List<CaseStatusCls> CaseStatusList { set; }
        List<Physician> PhysicianList { set; }

        string SalesRep { get; set; }
        string Procedure { get; set; }
        Int64? Party { get; set; }
        string CaseStatus { get; set; }
        string Physician { get; set; }
        //List<CaseKitFamilyDetailGroup> KitFamilyList { set; }
    }
}
