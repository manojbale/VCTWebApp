
/****************************************************************************  
 * Party.cs                                                          
 *                                                                            
 * Description:     Describes business logic for Party.  
 *  
 * Author:          Sanjay Kumar
 * Date:            May/20/2013
 *                                                                            
 ****************************************************************************/

using System;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace VCTWeb.Core.Domain
{
    [Serializable]
    public class Party
    {

        public Int64 PartyId { get; set; }

        [StringLengthValidator(1, 100, Ruleset = "Party", MessageTemplate = "valPartyName")]
        public string Name { get; set; }

        public string Code { get; set; }
        public string Description { get; set; }
        public Int64 PartyTypeId { get; set; }
        //public int? LinkedLocationId { get; set; }
        public string CompanyPrefix { get; set; }
        public int ShippingDaysGap { get; set; }
        public int RetrievalDaysGap { get; set; }
        public bool IsActive { get; set; }
        public string UpdatedBy { get; set; }
        public bool Owner { get; set; }
        public DateTime UpdatedOn { get; set; }

        public Address Address { get; set; }

    }

    [Serializable]
    public class PartyLinkedLocation
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string LocationType { get; set; }
        public bool Selected { get; set; }
    }

    [Serializable]
    public class RevenueProjection
    {
        public int ParentLocationId { get; set; }
        public string ParentLocationName { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string LocationType { get; set; }
        public string Party { get; set; }
        public decimal KitRentalAmount { get; set; }
        public decimal KitPartAmount { get; set; }
        public decimal PartAmount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}

