
/****************************************************************************  
 * LocationContacts.cs                                                          
 *                                                                            
 * Description:     Describes business logic for LocationContacts.  
 *  
 * Author:          Sanjay Kumar
 * Date:            May/20/2013
 *                                                                            
 ****************************************************************************/

using System;

namespace VCTWeb.Core.Domain
{
    public class LocationContacts
    {

        public int LocationId { get; set; }
        public string UserName { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsActive { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

    }
}

