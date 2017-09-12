
/****************************************************************************  
 * PartyType.cs                                                          
 *                                                                            
 * Description:     Describes business logic for PartyType.  
 *  
 * Author:          Sanjay Kumar
 * Date:            May/20/2013
 *                                                                            
 ****************************************************************************/

using System;

namespace VCTWeb.Core.Domain
{
    public class PartyType
    {

        public Int64 PartyTypeId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

    }
}

