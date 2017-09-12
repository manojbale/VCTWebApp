
/****************************************************************************  
 * LocationType.cs                                                          
 *                                                                            
 * Description:     Describes business logic for LocationType.  
 *  
 * Author:          Sanjay Kumar
 * Date:            May/20/2013
 *                                                                            
 ****************************************************************************/

using System;

namespace VCTWeb.Core.Domain
{
    [Serializable]
    public class LocationType
    {
        public Int64 LocationTypeId { get; set; }
        public Int64? ParentId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool RequiresAddress { get; set; }
        public bool IsActive { get; set; }
        public bool IsEditable { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}

