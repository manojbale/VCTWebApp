using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCTWeb.Core.Domain
{
    [Serializable]
    public class KitFamilyLocations
    {

        public Int64 KitFamilyLocationId { get; set; }

        public Int64 KitFamilyId { get; set; }

        public Int32 LocationId { get; set; }

        public string LocationName { get; set; }

        public string LocationType { get; set; }

        public Boolean LocationExists { get; set; }
    }
}
