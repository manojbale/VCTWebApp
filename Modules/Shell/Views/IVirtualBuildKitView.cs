using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public interface IVirtualBuildKitView
    {
        List<VirtualBuilKit> VirtualBuildKitList { get; set; }

        List<VirtualBuilKit> LotNumberList { get; set; }

        List<VirtualBuilKit> SelectedBuildKitList { get; set; }

        string VirtualBuildKitTableXml { get; }
                
        string KitNumber { get; set; }

        //string KitNumberDesc { get; set; }
        
        int LocationId { get; }

        string PartNum { get; set; }

        Int64 KitFamilyId { get; set; }

      //  string KitFamily { get; set; }
                
    }
}
