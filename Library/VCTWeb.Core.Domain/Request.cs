
/****************************************************************************  
 * Request.cs                                                          
 *                                                                            
 * Description:     Describes business logic for Request.  
 *  
 * Author:          Sanjay Kumar
 * Date:            May/20/2013
 *                                                                            
 ****************************************************************************/

using System;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace VCTWeb.Core.Domain
{
    public class Request
    {

        public Int64 RequestId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int Quantity { get; set; }
        public string Comments { get; set; }
        public DateTime RequiredOn { get; set; }
        //public string KitNumber { get; set; }
        //public Int64? ProcedureId { get; set; }
        public Int64? ShipToParty { get; set; }
        public int LocationId { get; set; }
    }

    [Serializable]
    public class UserPendingRequest
    {
        public Int64 RequestId { get; set; }
        public string RequestNumber { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int Quantity { get; set; }
        public string Comments { get; set; }
        public DateTime RequiredOn { get; set; }
        //public Int64? ProcedureId { get; set; }
        public Int64? ShipToParty { get; set; }
        public int LocationId { get; set; }

        public string KitNumber { get; set; }
        public string KitName { get; set; }
        public string RequestStatus { get; set; }
        public string ShipToCustomer { get; set; }
        public string ProcedureName { get; set; }
        public string CatalogNumberList { get; set; }
        public string LocationName { get; set; }
    }

    [Serializable]
    public class SummaryPendingRequest
    {
        public int RequestedLocationId { get; set; }
        public string RequestedLocationName { get; set; }
        public string KitStatus { get; set; }
        public int KitCount { get; set; }
    }

    [Serializable]
    public class RequestStatus
    {
        public string Status { get; set; }
        public int Count { get; set; }
    }
}

