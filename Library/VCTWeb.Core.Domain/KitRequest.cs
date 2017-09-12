using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCTWeb.Core.Domain
{
    [Serializable]
    public class KitRequest
    {
        public int RequestId { get; set; }
        public string KitNumber { get; set; }
        public string KitName { get; set; }
        public int KitQuantity { get; set; }
        public DateTime RequiredOn { get; set; }
        public string KitComments { get; set; }
        public string KitStatus { get; set; }
        public DateTime KitStatusUpdatedOn { get; set; }
        public DateTime RequestedOn { get; set; }
        public string RequestedBy { get; set; }
        public string RequestedLocationId { get; set; }
        public string CatalogNumber { get; set; }
        public string ShipToCustomer { get; set; }
        public string ProcedureName { get; set; }
    }

    [Serializable]
    public class MapLocation
    {
        public string LocationName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string ZIP { get; set; }
        public string MatchingPtage { get; set; }
        public string KitItemsCount { get; set; }
        public string Others1 { get; set; }
        public string Others2 { get; set; }
    }


    //[Serializable]
    //public class ConfigSettings
    //{
    //    public string LoggedInUser { get; set; }
    //    public string LoggedInSalesOffice { get; set; }
    //    public string LoggedInRegion { get; set; }
    //}

    [Serializable]
    public class RIMSummary
    {
        public string RequestedLocationId { get; set; }
        public string KitStatus { get; set; }
        public Int32 KitCount { get; set; }
    }

    //[Serializable]
    //public class KitSerachResult
    //{
    //    public string KitNumber { get; set; }
    //    public string KitName { get; set; }
    //    public string BranchName { get; set; }
    //    public string BranchAddress { get; set; }
    //    public string Address2 { get; set; }
    //    public string MatchingPtage { get; set; }
    //    public string CatalogNumber { get; set; }
    //    public string Excess { get; set; }
    //    public string KitItemsCount { get; set; }
    //    public string PreviousCheckInDate { get; set; }
    //    public string ShipToCustomer { get; set; }
    //    public string RequestedLocationId { get; set; }
    //    public string RequestedBy { get; set; }
    //    public string Lattitude { get; set; }
    //    public string Longitude { get; set; }
    //    public string ContactPersonName { get; set; }
    //    public string ContactPersonEmail { get; set; }
    //}
}
