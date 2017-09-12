using System;
using System.Collections.Generic;
using System.Text;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public interface IPartDetailView
    {
        List<PartDetailStockLevel> PartDetailStockLevelList { set; }
    }
}




