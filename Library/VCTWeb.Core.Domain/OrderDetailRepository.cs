using System;
using System.Collections.Generic;
using System.Data;

namespace VCTWeb.Core.Domain
{
    /// <summary>
    /// Summary description for OrderDetailRepository
    /// </summary>
    public class OrderDetailRepository
    {
        public List<OrderDetail> FetchShipAndBillReport(string accountNumber, string state, string ownershipStructure,
            string managementStructure, string branchAgency, string manager, string salesRepresentative,
            string productLine, string category, string subCategory1, string subCategory2, string subCategory3, DateTime? orderStartDate, DateTime? orderEndDate,
            DateTime? shippedStartDate, DateTime? shippedEndDate, string loginUserName)
        {
            var listOfOrderDetail = new List<OrderDetail>();
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.usp_EppFetchShipAndBillReport))
            {
                if (!string.IsNullOrEmpty(accountNumber))
                    db.AddInParameter(cmd, "@AccountNumber", DbType.String, accountNumber);

                if (!string.IsNullOrEmpty(state))
                    db.AddInParameter(cmd, "@State", DbType.String, state);

                if (!string.IsNullOrEmpty(ownershipStructure))
                    db.AddInParameter(cmd, "@OwnershipStructure", DbType.String, ownershipStructure);

                if (!string.IsNullOrEmpty(managementStructure))
                    db.AddInParameter(cmd, "@ManagementStructure", DbType.String, managementStructure);

                if (!string.IsNullOrEmpty(branchAgency))
                    db.AddInParameter(cmd, "@BranchAgency", DbType.String, branchAgency);

                if (!string.IsNullOrEmpty(manager))
                    db.AddInParameter(cmd, "@Manager", DbType.String, manager);

                if (!string.IsNullOrEmpty(salesRepresentative))
                    db.AddInParameter(cmd, "@SalesRepresentative", DbType.String, salesRepresentative);

                if (!string.IsNullOrEmpty(productLine))
                    db.AddInParameter(cmd, "@ProductLine", DbType.String, productLine);

                if (!string.IsNullOrEmpty(category))
                    db.AddInParameter(cmd, "@Category", DbType.String, category);

                if (!string.IsNullOrEmpty(subCategory1))
                    db.AddInParameter(cmd, "@SubCategory1", DbType.String, subCategory1);

                if (!string.IsNullOrEmpty(subCategory2))
                    db.AddInParameter(cmd, "@SubCategory2", DbType.String, subCategory2);

                if (!string.IsNullOrEmpty(subCategory3))
                    db.AddInParameter(cmd, "@SubCategory3", DbType.String, subCategory3);

                if (orderStartDate != null)
                    db.AddInParameter(cmd, "@OrderStartDate", DbType.Date, orderStartDate);

                if (orderEndDate != null)
                    db.AddInParameter(cmd, "@OrderEndDate", DbType.Date, orderEndDate);

                if (shippedStartDate != null)
                    db.AddInParameter(cmd, "@ShippedStartDate", DbType.Date, shippedStartDate);

                if (shippedEndDate != null)
                    db.AddInParameter(cmd, "@ShippedEndDate", DbType.Date, shippedEndDate);

                db.AddInParameter(cmd, "@LoginUserName", DbType.String, loginUserName);

                using (var reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                        listOfOrderDetail.Add(Load(reader));
                }
            }
            return listOfOrderDetail;
        }

        private OrderDetail Load(SafeDataReader reader)
        {
            var newOrderDetail = new OrderDetail
            {
                OrderId = reader.GetInt32("OrderId"),
                AccountNumber = reader.GetString("AccountNumber"),
                CustomerName = reader.GetString("CustomerName"),
                OrderNumber = reader.GetString("OrderNumber"),
                LineNumber = reader.GetString("LineNumber"),
                RefNum = reader.GetString("RefNum"),
                OrderedQty = reader.GetInt16("OrderedQty"),
                ShippedQty = reader.GetNullableInt16("ShippedQty"),
                CancelledQty = reader.GetNullableInt16("CancelledQty"),
                OrderStatus = reader.GetString("OrderStatus"),
                OrderDate = reader.GetDateTime("OrderDate"),
                ShippedDate = reader.GetNewNullableDateTime("ShippedDate"),
                CreatedBy = reader.GetString("CreatedBy"),
                ReceivedQty = reader.GetNullableInt16("ReceivedQty"),
                AdjustQty = reader.GetNullableInt16("AdjustQty"),
                DispositionType = reader.GetString("DispositionType"),
                DispositionTypeId = reader.GetNullableInt32("DispositionTypeId"),
                OrderAdjustmentId = reader.GetNullableInt64("OrderAdjustmentId"),
                Remarks = reader.GetString("Remarks"),
                UpdatedBy = reader.GetString("UpdatedBy"),
                UpdatedOn = reader.GetDateTime("UpdatedOn")
            };
            return newOrderDetail;
        }
    }
}