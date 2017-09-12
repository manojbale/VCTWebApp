
/****************************************************************************  
 * PermissionEntity.cs                                                          
 *                                                                            
 * Description:     Describes business logic for PermissionEntity.  
 *  
 * Author:          Sanjay Kumar
 * Date:            May/20/2013
 *                                                                            
 ****************************************************************************/

using System;

namespace VCTWeb.Core.Domain
{
    public class PermissionEntity
    {

        public string Entity { get; set; }
        public bool IsAvailable { get; set; }

    }
}

