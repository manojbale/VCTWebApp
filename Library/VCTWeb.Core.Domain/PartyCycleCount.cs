using System;

namespace VCTWeb.Core.Domain
{
    [Serializable]
    public class PartyCycleCount
    {
        public long PartyCycleCountId { get; set; }
        public string PartNum { get; set; }
        public string LotNum { get; set; }
        public string PartDescription { get; set; }
        public int CycleCountQty { get; set; }
        public int ExpectedQty { get; set; }
        public string CycleCountDate { get; set; }
        public string Status { get; set; }
        public string DispositionType { get; set; }
    }

    [Serializable]
    public class PartyCycleCountChild
    {
        public long PartyCycleCountId { get; set; }
        public string PartNum { get; set; }
        public string LotNum { get; set; }
        public int Quantity { get; set; }
        public bool IsNegativeVariance { get; set; }
        public string DispositionType { get; set; }
    }

    [Serializable]
    public class DispositionType
    {
        public int DispositionTypeId { get; set; }
        public string Disposition { get; set; }
    }

}


