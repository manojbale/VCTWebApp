
/****************************************************************************  
 * Kit.cs                                                          
 *                                                                            
 * Description:     Describes business logic for Kit.  
 *  
 * Author:          Sanjay Kumar
 * Date:            May/20/2013
 *                                                                            
 ****************************************************************************/

using System;

namespace VCTWeb.Core.Domain
{
    [Serializable]
    public class KitListing
    {
        public string KitNumber { get; set; }
        public string KitName { get; set; }
        public string KitDescription { get; set; }
        public int? NumberOfSets { get; set; }
        public string Aisle { get; set; }
        public string Row { get; set; }
        public string Tier { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? PMSchedule { get; set; }
        public string Lubricate { get; set; }
        // public string Value { get; set; }
        public string Procedure { get; set; }

        public bool IsManuallyAdded { get; set; }
        public bool IsAvailable { get; set; }
        public Int64? KitFamilyId { get; set; }
        public int? LocationId { get; set; }
        public bool IsActive { get; set; }
        public Decimal RentalFee { get; set; }
        public string UpdatedBy { get; set; }
        public string KitFamily { get; set; }
    }
}


