
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
    public class ReplenishmentTransfer
    {
        public string PartNum { get; set; }
        public string Description { get; set; }
        public int PARLevelQty { get; set; }
        public int AvailableQty { get; set; }
        public int ReplenishQty { get; set; }
    }
}


