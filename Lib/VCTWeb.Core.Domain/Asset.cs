/****************************************************************************  
 * Asset.cs                                                          
 *                                                                            
 * Description:     Describes business logic for VirtualBuildKit.  
 *  
 * Author:          Sanjeev Kumar
 * Date:            23/Apr/2014
 *                                                                            
 ****************************************************************************/

using System;

namespace VCTWeb.Core.Domain
{
    [Serializable]
    public class Asset
    {
        public int AssetId { get; set; }
        public string PartNum { get; set; }
        public string LotNum { get; set; }
        public DateTime ExpiryDt { get; set; }
        public int AssetTypeId { get; set; }
        public string AssetDesc { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDt { get; set; }
        public int SeqNum { get; set; }
        public int SeqCount { get; set; }
    }

    [Serializable]
    public class VirtualBuilKit
    {
        public Int64 KitFamilyId { get; set; }
        public int KitItemId { get; set; }
        public string KitNum { get; set; }
        public string PartNum { get; set; }
        public string Description { get; set; }
        public Int64 LocationId { get; set; }
        public string LotNum { get; set; }
        public Int64 LocationPartDetailId { get; set; }
        public bool IsNearExpiry { get; set; }
    }

    [Serializable]
    public class PendingBuildKit
    {
        public string KitNumber { get; set; }
        public string KitDescription { get; set; }
        public DateTime LastBuildOn { get; set; }
        public string KitFamily { get; set; }
    }

}
