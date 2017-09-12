using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace VCTWeb.Core.Domain
{
    [Serializable]
    public class BOM
    {
        public long BOMId { get; set; }
        public string Description { get; set; }
        public string ProcedureName { get; set; }
        public string KitNumber { get; set; }
        public string KitName { get; set; }
        public string TrayTypeName { get; set; }
        public DateTime? PublishedOn { get; set; }
        public DateTime? ValidTill { get; set; }
    }

    [Serializable]
    public class TrayType
    {
        public string TrayTypeName { get; set; }
    }
}
