using System;

namespace VCTWeb.Core.Domain
{
    [Serializable]
    public class LocationPartDetail
    {
        public long LocationPartDetailId { get; set; }
        public int LocationId { get; set; }
        public string PartNum { get; set; }
        public string Description { get; set; }
        public string LotNum { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string PartStatus { get; set; }
        public string LocationName { get; set; }
        public string LocationType { get; set; }
    }
}


