
/****************************************************************************  
 * LotMaster.cs                                                          
 *                                                                            
 * Description:     Describes business logic for LotMaster.  
 *  
 * Author:          Sanjay Kumar
 * Date:            May/20/2013
 *                                                                            
 ****************************************************************************/

using System;

namespace VCTWeb.Core.Domain
{
    public class LotMaster
    {

        public int ID { get; set; }
        public string ProductNum { get; set; }
        public string LotNum { get; set; }
        public DateTime? LotCommDate { get; set; }
        public DateTime? LotExpirydate { get; set; }
        public string Description { get; set; }

    }
}

