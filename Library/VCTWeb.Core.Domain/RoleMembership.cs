
/****************************************************************************  
 * RoleMembership.cs                                                          
 *                                                                            
 * Description:     Describes business logic for RoleMembership.  
 *  
 * Author:          Sanjay Kumar
 * Date:            May/20/2013
 *                                                                            
 ****************************************************************************/

using System;

namespace VCTWeb.Core.Domain
{
    public class RoleMembership
    {

        public string UserName { get; set; }
        public Int64 RoleId { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

    }
}

