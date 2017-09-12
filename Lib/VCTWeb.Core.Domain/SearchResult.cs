
/****************************************************************************  
 * SearchResult.cs                                                          
 *                                                                            
 * Description:     Describes business logic for SearchResult.  
 *  
 * Author:          Sanjay Kumar
 * Date:            May/20/2013
 *                                                                            
 ****************************************************************************/

using System;

namespace VCTWeb.Core.Domain
{
    public class SearchResult
    {

        public Int64 RequestId { get; set; }
        public int LocationId { get; set; }
        public int? TotalKitBuilt { get; set; }
        public int? TotalKitShipped { get; set; }
        public int? TotalKitPurged { get; set; }
        public int? BuiltItemCount { get; set; }
        public int? BOMItemCount { get; set; }
        public int? MatchingPtage { get; set; }
        public DateTime? PrevCheckInOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }

    [Serializable]
    public class KitSearchResult
    {
        public string KitNumber { get; set; }
        public string KitName { get; set; }
        public string BranchName { get; set; }
        public int BranchId { get; set; }
        public string BranchAddress { get; set; }
        public string Address2 { get; set; }
        public int Quantity { get; set; }
        public int? Excess { get; set; }
        public int? PerfectExcess { get; set; }
        public int? PartialExcess { get; set; }
        public int? TotalKitBuilt { get; set; }
        public int? TotalKitShipped { get; set; }
        public int? TotalKitPurged { get; set; }
        public int? TotalKitHold { get; set; }
        public int? ReservedQty { get; set; }
        public int? BuiltItemCount { get; set; }
        public int? BOMItemCount { get; set; }
        public int? MatchingPtage { get; set; }
        public DateTime? PreviousCheckInDate { get; set; }
        public string RequestedBy { get; set; }
        public string RequestedLocation { get; set; }
        public int RequestedLocationId { get; set; }
        public string CatalogNumber { get; set; }
        public string ShipToCustomer { get; set; }
        public string ShipToCustomerAdd1 { get; set; }
        public string ShipToCustomerAdd2 { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonEmail { get; set; }
    }
}

