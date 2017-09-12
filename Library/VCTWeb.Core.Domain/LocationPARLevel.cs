﻿
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
    public class LocationPARLevel
    {
        public long PARLevelId { get; set; }
        public int LocationId { get; set; }
        public string PartNum { get; set; }
        public string Description { get; set; }
        public int PARLevelQty { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}


