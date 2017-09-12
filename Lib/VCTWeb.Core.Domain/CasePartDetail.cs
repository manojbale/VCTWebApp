using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCTWeb.Core.Domain
{
    [Serializable]
    public class CasePartDetail
    {
        public Int64 CasePartId { get; set; }
        public int CaseId { get; set; }
        public string PartNum { get; set; }
        public Int64 LocationPartDetailId { get; set; }        
    }
}
