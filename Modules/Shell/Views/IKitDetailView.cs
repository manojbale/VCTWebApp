using System;
using System.Collections.Generic;
using System.Text;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public interface IKitDetailView
    {
        List<KitDetailStockLevel> KitDetailStockLevelList { set; }
        List<VirtualCheckOut> VirtualCheckOutList { set; }
    }
}




