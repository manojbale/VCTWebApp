
/****************************************************************************  
 * Location.cs                                                          
 *                                                                            
 * Description:     Describes business logic for Location.  
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
    public class Location
    {
        public Address Address { get; set; }

        public int LocationId { get; set; }
        [StringLengthValidator(1, 100, Ruleset = "Location", MessageTemplate = "valLocationCode")]
        public string Code { get; set; }
        public int? AddressId { get; set; }
        public Int64 LocationTypeId { get; set; }
        public int? ParentLocationId { get; set; }
        [StringLengthValidator(1, 100, Ruleset = "Location", MessageTemplate = "valLocationName")]
        public string LocationName { get; set; }
        public string LocationTypeLocationName { get; set; }
        public string Description { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public string GLN { get; set; }
        public bool IsActive { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

        // fields of LocationType tbl.
        public string LocationType { get; set; }
        public bool RequiresAddress { get; set; }
    }
}

