
/****************************************************************************  
 * Dictionary.cs                                                          
 *                                                                            
 * Description:     Describes business logic for Dictionary.  
 *  
 * Author:          Sanjay Kumar
 * Date:            May/20/2013
 *                                                                            
 ****************************************************************************/

using System;

namespace VCTWeb.Core.Domain
{
    public class Dictionary
    {

        public string KeyName { get; set; }
        public string KeyValue { get; set; }
        public string DataType { get; set; }
        public bool Editable { get; set; }
        public string KeyGroup { get; set; }
        public string Description { get; set; }
        public string ListValues { get; set; }

    }
}

