
/****************************************************************************  
 * PartyLocationRole.cs                                                          
 *                                                                            
 * Description:     Describes business logic for PartyLocationRole.  
 *  
 * Author:          Sanjay Kumar
 * Date:            May/20/2013
 *                                                                            
 ****************************************************************************/

using System;

namespace VCTWeb.Core.Domain
{
    public class PartyLocationRole
    {

        public Int64 PartyId { get; set; }
        public int LocationId { get; set; }
        public bool IsActive { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

    }
}

