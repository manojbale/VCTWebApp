
/****************************************************************************  
 * Address.cs                                                          
 *                                                                            
 * Description:     Describes business logic for Address.  
 *  
 * Author:          Sanjay Kumar
 * Date:            May/20/2013
 *                                                                            
 ****************************************************************************/

using System;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using VCTWeb.Core.Domain.CustomValidators;

namespace VCTWeb.Core.Domain
{
    public class Address
    {

        public int AddressId { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string WebSite { get; set; }
        public string PrimaryEmailId { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

        public decimal Latitude { set; get; }
        public decimal Longitude { set; get; }

    }

    public class Region
    {
        public string RegionName { get; set; }
        public int RegionId { get; set; }
    }

    public class SalesOffice
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }
    }

    public class Corp
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }
    }
}

