
/****************************************************************************  
 * KitItem.cs                                                          
 *                                                                            
 * Description:     Describes business logic for KitItem.  
 *  
 * Author:          Sanjeev Kumar
 * Date:            23/April/2014
 *                                                                            
 ****************************************************************************/

using System;

namespace VCTWeb.Core.Domain
{
    [Serializable]
    public class KitItem
    {
        public int KitItemId { get; set; }
        public int ParentId { get; set; }
        public int ChildId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTill { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDt { get; set; }
    }
}
