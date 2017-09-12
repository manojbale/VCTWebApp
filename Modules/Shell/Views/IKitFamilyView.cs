using System;
using System.Collections.Generic;
using System.Text;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public interface IKitFamilyView
    {
        List<KitType> KitTypeList { set; }
        List<KitFamily> KitFamilyList { set; }        
        List<KitFamilyParts> KitFamilyPartsList { set; }
        List<KitFamilyLocations> KitFamilyLocationList { set; }

        long SelectedKitFamilyId { get; }
        string KitFamilyPartTableXml { get; }
        string KitFamilyLocationTableXml { get; }

        string Name { get; set; }
        string KitType { get; set; }
        string Description { get; set; }
        Int16 NumberOfTubs { get; set; }
        bool Active { get; set; }

    }
}




