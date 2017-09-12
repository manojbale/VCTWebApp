
/****************************************************************************  
 * Address.cs                                                          
 *                                                                            
 * Description:     Describes business logic for Address.  
 *  
 * Author:          Sanjay Kumar
 * Date:            May/20/2013
 *                                                                            
 ****************************************************************************/

using System;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using VCTWeb.Core.Domain.CustomValidators;

namespace VCTWeb.Core.Domain
{

    public class LowInventory
    {
        public string CustomerName { get; set; }
        public string AccountNumber { get; set; }
        public string RefNum { get; set; }
        public string ProductLine { get; set; }
        public string ProductLineDesc { get; set; }
        public string Size { get; set; }
        public string Category { get; set; }
        public string SubCategory1 { get; set; }
        public string SubCategory2 { get; set; }
        public string SubCategory3 { get; set; }
        public string PartDesc { get; set; }
        public int PARLevelQty { get; set; }
        public int InvLevelQty { get; set; }
        public int OrderedProductQty { get; set; }
        public int LowInvQty { get; set; }
        public DateTime? LastScanned { get; set; }
        public int BackOrderQty { get; set; }
    }


    public class InventoryAmount
    {
        public string CustomerName { get; set; }
        public string AccountNumber { get; set; }
        public string RefNum { get; set; }
        public string ProductLine { get; set; }
        public string ProductLineDesc { get; set; }
        public string Category { get; set; }
        public string SubCategory1 { get; set; }
        public string SubCategory2 { get; set; }
        public string SubCategory3 { get; set; }
        public string PartDesc { get; set; }
        public string LotNum { get; set; }
        public string TagId { get; set; }
        public string ItemStatus { get; set; }
        public string ItemStatusDescription { get; set; }
        public DateTime ExpiryDt { get; set; }
        public int Qty { get; set; }
        public int OffCartQty { get; set; }
        public bool IsNearExpiry { get; set; }
        public DateTime LastScanned { get; set; }
        public int AssetNearExpiryDays { get; set; }
        public bool IsManuallyConsumed { get; set; }
        public int PARLevelQty { get; set; }
    }


    public class InventoryOffCartRate
    {
        public string CustomerName { get; set; }
        public string AccountNumber { get; set; }
        public string RefNum { get; set; }
        public string ProductLine { get; set; }
        public string ProductLineDesc { get; set; }
        public string Category { get; set; }
        public string SubCategory1 { get; set; }
        public string SubCategory2 { get; set; }
        public string SubCategory3 { get; set; }
        public string PartDesc { get; set; }
        public string LotNum { get; set; }
        public string TagId { get; set; }
        public DateTime ExpiryDt { get; set; }
        public int Qty { get; set; }
        public DateTime LastScanned { get; set; }
        public Int32 OffCartCount { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime ConsumptionDate { get; set; }

    }


    public class EPPTransaction
    {
        public string CustomerName { get; set; }
        public string AccountNumber { get; set; }
        public string ItemStatus { get; set; }
        public string StatusDescription { get; set; }
        public string RefNum { get; set; }
        public string LotNum { get; set; }
        public string PartDesc { get; set; }
        public string Category { get; set; }
        public string SubCategory1 { get; set; }
        public string SubCategory2 { get; set; }
        public string SubCategory3 { get; set; }
        public string TagId { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int Count { get; set; }
    }


    public class ItemStatus
    {
        public string ItemStatusCode { get; set; }
        public string StatusDescription { get; set; }
        public bool IsExceptionalStatus { get; set; }
    }


    public class ConsumptionRate
    {
        public string CustomerName { get; set; }
        public string AccountNumber { get; set; }
        public string RefNum { get; set; }
        public string PartDesc { get; set; }
        public string Category { get; set; }
        public string SubCategory1 { get; set; }
        public string SubCategory2 { get; set; }
        public string SubCategory3 { get; set; }
        public int ParLevelQty { get; set; }
        public int ConsumedQty { get; set; }
        public int NoOfDays { get; set; }
        public decimal ConsumptionRatePercent { get; set; }

        public ConsumptionRate() { }

        public ConsumptionRate(string _CustomerName, string _AccountNumber, int _ConsumedQty, int _NoOfDays)
        {
            this.CustomerName = _CustomerName;
            this.AccountNumber = _AccountNumber;
            this.ConsumedQty = _ConsumedQty;
            this.ConsumptionRatePercent = _NoOfDays > 0 ? (Convert.ToDecimal(_ConsumedQty) / Convert.ToDecimal(_NoOfDays)) : Convert.ToDecimal(0.0);
            string str = String.Format("{0:0.##}", this.ConsumptionRatePercent);
            this.ConsumptionRatePercent = Convert.ToDecimal(str);
            this.NoOfDays = _NoOfDays;
            this.RefNum = string.Empty;
            this.PartDesc = string.Empty;
        }

    }


    public class ManualConsumptionReport
    {
        public string CustomerName { get; set; }
        public string AccountNumber { get; set; }
        public string RefNum { get; set; }
        public string ProductLine { get; set; }
        public string ProductLineDesc { get; set; }
        public string Category { get; set; }
        public string SubCategory1 { get; set; }
        public string SubCategory2 { get; set; }
        public string SubCategory3 { get; set; }
        public string PartDesc { get; set; }
        public string LotNum { get; set; }
        public string TagId { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsConsumed { get; set; }
        public DateTime? ConsumedDate { get; set; }
    }
}

