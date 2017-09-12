using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCTWeb.Core.Domain
{

    [Serializable]
    public class KitFamilyParts
    {

        public Int64 KitFamilyItemId { get; set; }

        //public KitFamily KitFamilyId { get; set; }
        public Int64 KitFamilyId { get; set; }

        public string Description { get; set; }

        public string CatalogNumber { get; set; }

        public int Quantity { get; set; }

    }

}
