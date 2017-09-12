using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace VCTWeb.Core.Domain
{
    [Serializable]
    public class KitStockLevel
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string LocationType { get; set; }
        public long KitFamilyId { get; set; }
        public string KitFamilyName { get; set; }
        public string KitFamilyDescription { get; set; }
        public DateTime LeastExpiryDate { get; set; }
        public int AvailableQuantity { get; set; }
        public int AssignedToCaseQuantity { get; set; }
        public int ShippedQuantity { get; set; }
        public int ReceivedQuantity { get; set; }
        public bool IsNearExpiry { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
    }

    [Serializable]
    public class KitDetailStockLevel
    {
        public long BuildKitId { get; set; }
        public string KitNumber { get; set; }
        public string Description { get; set; }
        public DateTime LeastExpiryDate { get; set; }
        public string Status { get; set; }
        public string LinkedCaseNumber { get; set; }
        public string Hospital { get; set; }
        public bool IsNearExpiry { get; set; }
    }


    [Serializable]
    public class PartStockLevel
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string LocationType { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public DateTime LeastExpiryDate { get; set; }
        public int AvailableQuantity { get; set; }
        public int AssignedToCaseQuantity { get; set; }
        public bool IsNearExpiry { get; set; }
    }

    [Serializable]
    public class PartDetailStockLevel
    {
        public string LotNum { get; set; }
        public string Description { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Status { get; set; }
        public string LinkedCaseNumber { get; set; }
        public bool IsNearExpiry { get; set; }
    }
}
