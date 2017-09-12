using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public interface IAgingOrdersView
    {
        List<Default> AgingOrdersList { get; set; }
        Default BranchDetail { get; set; }        
        int LocationId { get; }
        System.DateTime BusinessDate { get; }
    }
}
