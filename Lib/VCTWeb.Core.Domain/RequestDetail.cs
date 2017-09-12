
/****************************************************************************  
 * RequestDetail.cs                                                          
 *                                                                            
 * Description:     Describes business logic for RequestDetail.  
 *  
 * Author:          Sanjay Kumar
 * Date:            May/20/2013
 *                                                                            
 ****************************************************************************/

using System;

namespace VCTWeb.Core.Domain
{
    public class RequestDetail
    {

        public Int64 RequestDetailId { get; set; }
        public Int64 RequestId { get; set; }
        public string KitNumber { get; set; }
        public string ProcedureName { get; set; }
        public string CatalogNumber { get; set; }

    }
}

