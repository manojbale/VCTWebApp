
/****************************************************************************  
 * RequestTransaction.cs                                                          
 *                                                                            
 * Description:     Describes business logic for RequestTransaction.  
 *  
 * Author:          Sanjay Kumar
 * Date:            May/20/2013
 *                                                                            
 ****************************************************************************/

using System;

namespace VCTWeb.Core.Domain
{
    public class RequestTransaction
    {

        public Int64 RequestTransactionId { get; set; }
        public Int64 RequestId { get; set; }
        public string Comments { get; set; }
        public string RequestStatus { get; set; }
        public int LocationId { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

    }
}

