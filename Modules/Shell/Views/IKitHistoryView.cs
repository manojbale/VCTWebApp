using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public interface IKitHistoryView
    {
        List<KitHistoryCaseDetail> KitHistoryCaseDetailList { get; set; }
                
        string KitNumber { get; set; }                
    }
}
