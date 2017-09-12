using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace VCTWeb.Core.Domain
{
    [Serializable]
    public class KitFamily
    {
        [StringLengthValidator(1, 100, Ruleset = "KitFamily", MessageTemplate = "valName")]
        public string KitFamilyName { get; set; }

        public long KitFamilyId { get; set; }
        public string KitTypeName { get; set; }
        public Int16 NumberOfTubs { get; set; }
        public bool IsActive { get; set; }
        public string KitFamilyDescription { get; set; }
    }

    [Serializable]
    public class KitType
    {
        public string KitTypeName { get; set; }
    }

    public class KitFamilSmall
    {
        public string KitFamilyName { get; set; }
        public string KitNumber { get; set; }
        public Int64 BuildKitId { get; set; }
        public Int64 KitFamilyId { get; set; }
    }

    public class ActiveKitFamily
    {
        public string KitFamilyName { get; set; }
        public string KitFamilyDescription { get; set; }
        public int OrderCount { get; set; }        
    }

    [Serializable]
    public class KitFamilyInventoryTransfer
    {
        public long KitFamilyId { get; set; }
        public string KitFamilyName { get; set; }
        public string KitFamilyDescription { get; set; }
        public Int32 AvailableQty { get; set; }
        public string LastUsage { get; set; }        
    }
}
