using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public interface INewProductTransfer
    {
        List<KitFamily> KitFamilyList { set; }
        List<KitFamilyParts> KitPartsList { set; }
        List<NewProductTransfer> KitFamilyLocationList { set; }

        string KitFamilyLocationsTableXML { get; }
        string KitFamilyPartsTableXML { get; }

        string KitFamilyName { get; }
        Int64 KitFamilyId { get; }
        DateTime TransDate { get; set;  }
        Int32 LocationId { get; }

    }
}
