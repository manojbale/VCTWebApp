using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace VCTWeb.Core.Domain
{
    [Serializable]
    public class Contact
    {

        [StringLengthValidator(1, 50, Ruleset = "Contact", MessageTemplate = "valFirstName")]
        public string FirstName { get; set; }

        [StringLengthValidator(1, 50, Ruleset = "Contact", MessageTemplate = "valLastName")]
        public string LastName { get; set; }

        public int ContactId { get; set; }
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        [StringLengthValidator(1, 200, Ruleset = "Contact", MessageTemplate = "valEmptyEmailAddress")]
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Cell { get; set; }
        public string Fax { get; set; }
        public List<LocationContact> LocationContactList { get; set; }
        //public int? LocationId { get; set; }
    }

    [Serializable]
    public class LocationContact
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public bool Selected { get; set; }
    }
}
