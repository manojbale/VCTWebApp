using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCTWeb.Core.Domain
{
    public class CaseStatusDetail
    {
        public Int64 CaseStatusId { get; set; }
        public Int64 CaseId { get; set; }
        public string CaseStatus { get; set; }
        public string Description { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
