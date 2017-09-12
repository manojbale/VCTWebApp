using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace VCTWeb.Core.Domain
{
    [Serializable]
    public class PartyLocation
    {
        public Address Address { get; set; }
        public Int64 PartyId { get; set; }
        public string PartyName { get; set; }
        public bool RequiresAddress { get; set; }
        public string LocationType { get; set; }
        
        public int LocationId { get; set; }
        [StringLengthValidator(1, 100, Ruleset = "PartyLocation", MessageTemplate = "valLocationCode")]
        public string Code { get; set; }
        public int? AddressId { get; set; }
        public Int64 LocationTypeId { get; set; }
        public int? ParentLocationId { get; set; }
        [StringLengthValidator(1, 100, Ruleset = "PartyLocation", MessageTemplate = "valLocationName")]
        public string LocationName { get; set; }
        public string Description { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public string GLN { get; set; }
        public bool IsActive { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
