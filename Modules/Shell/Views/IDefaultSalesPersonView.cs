using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public interface IDefaultSalesPersonView
    {
        List<ViewCancelTransaction> lstPendingCases { set; }
        List<ViewCancelTransaction> ChildList { get; set; }
        Int32 LocationId { get; }
    }
}
