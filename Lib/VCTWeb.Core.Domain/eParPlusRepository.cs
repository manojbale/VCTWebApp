using System.Collections.Generic;
using System.Data;
using System;

namespace VCTWeb.Core.Domain
{

    public class EParPlusRepository
    {
        public List<String> FetchDistinctColumnValueForTable(string tableName, string columnName)
        {
            var db = DbHelper.CreateDatabase();
            var lstDropdownData = new List<string>();
            using (var cmd = db.GetStoredProcCommand(Constants.Usp_EppFetchDistinctColumnValueForTable))
            {
                db.AddInParameter(cmd, "@TableName", DbType.String, tableName);
                db.AddInParameter(cmd, "@ColumnName", DbType.String, columnName);
                using (var reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstDropdownData.Add(reader.GetString("ColumnValue"));
                    }
                }
                return lstDropdownData;
            }
        }

        public List<String> FetchProductLinePartCategoryHierarchy(string productLine, string category, string subCategory1, string subCategory2, int filterLevel)
        {
            var db = DbHelper.CreateDatabase();
            var lstDropdownData = new List<string>();
            using (var cmd = db.GetStoredProcCommand(Constants.Usp_EppFetchProductLinePartCategoryHierarchy))
            {
                if (!string.IsNullOrEmpty(productLine))
                    db.AddInParameter(cmd, "@ProductLine", DbType.String, productLine);

                if (!string.IsNullOrEmpty(category))
                    db.AddInParameter(cmd, "@Category", DbType.String, category);

                if (!string.IsNullOrEmpty(subCategory1))
                    db.AddInParameter(cmd, "@SubCategory1", DbType.String, subCategory1);

                if (!string.IsNullOrEmpty(subCategory2))
                    db.AddInParameter(cmd, "@SubCategory2", DbType.String, subCategory2);

                db.AddInParameter(cmd, "@FilterLevel", DbType.Int32, filterLevel);

                using (var reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        var colVal = reader.GetString("ColumnValue");
                        if (!string.IsNullOrEmpty(colVal.Trim()))
                            lstDropdownData.Add(colVal);
                    }
                }
                return lstDropdownData;
            }
        }

        public int InitiateEppManualScan(string accountNumber)
        {
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.UspEppInitiateManualScan))
            {
                //if (!string.IsNullOrEmpty(customerName.Trim()))
                //    db.AddInParameter(cmd, "@CustomerName", DbType.String, customerName);
                if (!string.IsNullOrEmpty(accountNumber.Trim()))
                    db.AddInParameter(cmd, "@AccountNumber", DbType.String, accountNumber);
                object numberOfShelves = db.ExecuteScalar(cmd);
                if (numberOfShelves != null)
                    return Convert.ToInt32(numberOfShelves);
                else
                    return 0;
            }
        }

        public List<ItemStatus> FetchAllEppItemStatus(string itemStatus)
        {
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.UspEppFetchAllItemStatus))
            {
                if (!string.IsNullOrEmpty(itemStatus))
                    db.AddInParameter(cmd, "@ItemStatus", DbType.String, itemStatus);

                var lstItemStatus = new List<ItemStatus>();
                using (var reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstItemStatus.Add(new ItemStatus
                        {
                            ItemStatusCode = reader.GetString("ItemStatus"),
                            StatusDescription = reader.GetString("StatusDescription"),
                            IsExceptionalStatus = reader.GetBoolean("IsExceptionalStatus")
                        });
                    }

                }
                return lstItemStatus;
            }
        }

        public List<ItemStatus> FetchAllEppItemStatus()
        {
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.UspEppFetchAllItemStatus))
            {
                var lstItemStatus = new List<ItemStatus>();
                using (var reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstItemStatus.Add(new ItemStatus
                        {
                            ItemStatusCode = reader.GetString("ItemStatus"),
                            StatusDescription = reader.GetString("StatusDescription"),
                            IsExceptionalStatus = reader.GetBoolean("IsExceptionalStatus")
                        });
                    }

                }
                return lstItemStatus;
            }
        }


        public List<EPPTransaction> FetchTransactionReport(string accountNumber, string state, string ownershipStructure,
            string managementStructure, string branchAgency, string manager, string salesRepresentative,
            string productLine, string category, string subCategory1, string subCategory2, string subCategory3, DateTime startDate, DateTime endDate, string itemStatus, string loginUserName)
        {
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.Usp_EppFetchTransactionReport))
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

                db.AddInParameter(cmd, "@StartDate", DbType.Date, startDate);

                db.AddInParameter(cmd, "@EndDate", DbType.Date, endDate);

                db.AddInParameter(cmd, "@LoginUserName", DbType.String, loginUserName);

                if (!string.IsNullOrEmpty(itemStatus))
                    db.AddInParameter(cmd, "@ItemStatus", DbType.String, itemStatus);

                var lstEppTransaction = new List<EPPTransaction>();
                using (var reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstEppTransaction.Add(LoadTransactionReportData(reader));
                    }
                }
                return lstEppTransaction;
            }
        }


        public List<ConsumptionRate> FetchConsumptionRateReport(string accountNumber, string state, string ownershipStructure,
            string managementStructure, string branchAgency, string manager, string salesRepresentative,
            string productLine, string category, string subCategory1, string subCategory2, string subCategory3, DateTime startDate, DateTime endDate, string loginUserName)
        {
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.usp_EppFetchConsumptionRate))
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

                db.AddInParameter(cmd, "@StartDate", DbType.Date, startDate);

                db.AddInParameter(cmd, "@EndDate", DbType.Date, endDate);

                db.AddInParameter(cmd, "@LoginUserName", DbType.String, loginUserName);

                var lstConsumptionRate = new List<ConsumptionRate>();
                using (var reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstConsumptionRate.Add(LoadConsumptionRate(reader));
                    }
                }
                return lstConsumptionRate;
            }
        }


        public List<LowInventory> FetchLowInventoryReport(string accountNumber, string state, string ownershipStructure,
            string managementStructure, string branchAgency, string manager, string salesRepresentative,
           string productLine, string category, string subCategory1, string subCategory2, string subCategory3, string loginUserName)
        {
            var lstLowInventory = new List<LowInventory>();
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.usp_EppFetchLowInventoryReport))
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

                db.AddInParameter(cmd, "@LoginUserName", DbType.String, loginUserName);

                using (var reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstLowInventory.Add(LoadLowInventory(reader));
                    }
                }
                return lstLowInventory;
            }
        }

        public List<InventoryAmount> FetchInventoryAmountReport(string accountNumber, string state, string ownershipStructure,
            string managementStructure, string branchAgency, string manager, string salesRepresentative,
           string productLine, string category, string subCategory1, string subCategory2, string subCategory3, int expirationDays, string itemStatus, string loginUserName)
        {
            var lstLowInventory = new List<InventoryAmount>();
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.usp_EppFetchInventoryAmountReport))
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

                if (expirationDays > 0)
                    db.AddInParameter(cmd, "@ExpirationDays", DbType.Int32, expirationDays);

                if (!string.IsNullOrEmpty(itemStatus))
                    db.AddInParameter(cmd, "@ItemStatus", DbType.String, itemStatus);

                db.AddInParameter(cmd, "@LoginUserName", DbType.String, loginUserName);

                using (var reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstLowInventory.Add(LoadInventoryAmount(reader));
                    }
                }
                return lstLowInventory;
            }
        }

        public List<InventoryOffCartRate> FetchInventoryOffCartRateReport(string accountNumber, string state, string ownershipStructure,
           string managementStructure, string branchAgency, string manager, string salesRepresentative,
          string productLine, string category, string subCategory1, string subCategory2, string subCategory3, DateTime startDate, DateTime endDate, string loginUserName)
        {
            var lstLowInventory = new List<InventoryOffCartRate>();
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.Usp_EppFetchOffCartRateForConsumedItems))
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

                db.AddInParameter(cmd, "@StartDate", DbType.Date, startDate);

                db.AddInParameter(cmd, "@EndDate", DbType.Date, endDate);

                db.AddInParameter(cmd, "@LoginUserName", DbType.String, loginUserName);

                using (var reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstLowInventory.Add(LoadInventoryOffCartRate(reader));
                    }
                }
                return lstLowInventory;
            }
        }


        public List<ManualConsumptionReport> FetchManualConsumptionReport(string accountNumber, string state, string ownershipStructure,
          string managementStructure, string branchAgency, string manager, string salesRepresentative,
         string productLine, string category, string subCategory1, string subCategory2, string subCategory3, DateTime startDate, DateTime endDate, string loginUserName)
        {
            var lstManualConsumptionReport = new List<ManualConsumptionReport>();
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.Usp_EppFetchManualConsumptionReport))
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

                db.AddInParameter(cmd, "@StartDate", DbType.Date, startDate);

                db.AddInParameter(cmd, "@EndDate", DbType.Date, endDate);

                db.AddInParameter(cmd, "@LoginUserName", DbType.String, loginUserName);

                using (var reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstManualConsumptionReport.Add(LoadManualConsumptionReport(reader));
                    }
                }
                return lstManualConsumptionReport;
            }
        }


        public List<EPPTransaction> FetchTagHistoryByTagId(string accountNumber, string tagId)
        {
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.Usp_EppFetchTagHistoryByTagId))
            {
                db.AddInParameter(cmd, "@AccountNumber", DbType.String, accountNumber);
                db.AddInParameter(cmd, "@TagId", DbType.String, tagId);

                var lstEppTransaction = new List<EPPTransaction>();
                using (var reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstEppTransaction.Add(LoadTagHistoryData(reader));
                    }
                }
                return lstEppTransaction;
            }
        }


        private EPPTransaction LoadTransactionReportData(SafeDataReader reader)
        {
            var item = new EPPTransaction
            {
                CustomerName = reader.GetString("CustomerName"),
                AccountNumber = reader.GetString("AccountNumber"),
                ItemStatus = reader.GetString("ItemStatus"),
                StatusDescription = reader.GetString("StatusDescription"),
                RefNum = reader.GetString("RefNum"),
                LotNum = reader.GetString("LotNum"),
                PartDesc = reader.GetString("PartDesc"),
                TagId = reader.GetString("TagId"),
                UpdatedOn = reader.GetDateTime("UpdatedOn"),
                Category = reader.GetString("Category"),
                SubCategory1 = reader.GetString("SubCategory1"),
                SubCategory2 = reader.GetString("SubCategory2"),
                SubCategory3 = reader.GetString("SubCategory3")
            };
            return item;
        }

        private EPPTransaction LoadTagHistoryData(SafeDataReader reader)
        {
            var item = new EPPTransaction
            {
                CustomerName = reader.GetString("CustomerName"),
                AccountNumber = reader.GetString("AccountNumber"),
                ItemStatus = reader.GetString("ItemStatus"),
                StatusDescription = reader.GetString("StatusDescription"),
                RefNum = reader.GetString("RefNum"),
                LotNum = reader.GetString("LotNum"),
                //PartDesc = reader.GetString("PartDesc"),
                TagId = reader.GetString("TagId"),
                UpdatedOn = reader.GetDateTime("UpdatedOn")
                //Category = reader.GetString("Category"),
                //SubCategory1 = reader.GetString("SubCategory1"),
                //SubCategory2 = reader.GetString("SubCategory2"),
                //SubCategory3 = reader.GetString("SubCategory3")
            };
            return item;
        }

        private ConsumptionRate LoadConsumptionRate(SafeDataReader reader)
        {
            var item = new ConsumptionRate
            {
                CustomerName = reader.GetString("CustomerName"),
                AccountNumber = reader.GetString("AccountNumber"),
                RefNum = reader.GetString("RefNum"),
                PartDesc = reader.GetString("PartDesc"),
                ParLevelQty = reader.GetInt16("ParLevelQty"),
                ConsumedQty = reader.GetInt32("ConsumedQty"),
                NoOfDays = reader.GetInt32("NoOfDays"),
                ConsumptionRatePercent = reader.GetDecimal("ConsumptionRatePercent"),
                Category = reader.GetString("Category"),
                SubCategory1 = reader.GetString("SubCategory1"),
                SubCategory2 = reader.GetString("SubCategory2"),
                SubCategory3 = reader.GetString("SubCategory3")
            };
            var str = String.Format("{0:0.##}", item.ConsumptionRatePercent);
            item.ConsumptionRatePercent = Convert.ToDecimal(str);
            return item;
        }

        private LowInventory LoadLowInventory(SafeDataReader reader)
        {
            var item = new LowInventory
            {
                CustomerName = reader.GetString("CustomerName"),
                AccountNumber = reader.GetString("AccountNumber"),
                RefNum = reader.GetString("RefNum"),
                ProductLine = reader.GetString("ProductLine"),
                ProductLineDesc = reader.GetString("ProductLineDesc"),
                Size = reader.GetString("Size"),
                PartDesc = reader.GetString("PartDesc"),
                PARLevelQty = reader.GetInt16("PARLevelQty"),
                InvLevelQty = reader.GetInt32("InvLevelQty"),
                OrderedProductQty = reader.GetInt32("OrderedProductQty"),
                BackOrderQty = reader.GetInt32("BackOrderQty"),
                LowInvQty = reader.GetInt32("LowInvQty"),
                LastScanned = reader.GetNewNullableDateTime("LastScanned"),
                Category = reader.GetString("Category"),
                SubCategory1 = reader.GetString("SubCategory1"),
                SubCategory2 = reader.GetString("SubCategory2"),
                SubCategory3 = reader.GetString("SubCategory3")
            };
            return item;
        }

        private InventoryAmount LoadInventoryAmount(SafeDataReader reader)
        {
            var item = new InventoryAmount
            {
                CustomerName = reader.GetString("CustomerName"),
                AccountNumber = reader.GetString("AccountNumber"),
                ProductLine = reader.GetString("ProductLine"),
                ProductLineDesc = reader.GetString("ProductLineDesc"),
                RefNum = reader.GetString("RefNum"),
                PartDesc = reader.GetString("PartDesc"),
                LotNum = reader.GetString("LotNum"),
                TagId = reader.GetString("TagId"),
                ItemStatusDescription = reader.GetString("ItemStatusDescription"),
                ItemStatus = reader.GetString("ItemStatus"),
                IsNearExpiry = reader.GetBoolean("IsNearExpiry"),
                ExpiryDt = reader.GetDateTime("ExpiryDt"),
                LastScanned = reader.GetDateTime("LastScanned"),
                Category = reader.GetString("Category"),
                SubCategory1 = reader.GetString("SubCategory1"),
                SubCategory2 = reader.GetString("SubCategory2"),
                SubCategory3 = reader.GetString("SubCategory3"),
                AssetNearExpiryDays = reader.GetInt16("AssetNearExpiryDays"),
                PARLevelQty = reader.GetInt16("PARLevelQty"),
                IsManuallyConsumed = (reader.GetInt32("IsManuallyConsumed") == 1)
            };
            return item;
        }

        private InventoryOffCartRate LoadInventoryOffCartRate(SafeDataReader reader)
        {
            var item = new InventoryOffCartRate
            {
                CustomerName = reader.GetString("CustomerName"),
                AccountNumber = reader.GetString("AccountNumber"),
                ProductLine = reader.GetString("ProductLine"),
                ProductLineDesc = reader.GetString("ProductLineDesc"),
                RefNum = reader.GetString("RefNum"),
                PartDesc = reader.GetString("PartDesc"),
                LotNum = reader.GetString("LotNum"),
                TagId = reader.GetString("TagId"),
                ExpiryDt = reader.GetDateTime("ExpiryDt"),
                LastScanned = reader.GetDateTime("LastScanned"),
                Category = reader.GetString("Category"),
                SubCategory1 = reader.GetString("SubCategory1"),
                SubCategory2 = reader.GetString("SubCategory2"),
                SubCategory3 = reader.GetString("SubCategory3"),
                OffCartCount = reader.GetInt32("OffCartCount"),
                CheckInDate = reader.GetDateTime("CheckInDate"),
                ConsumptionDate = reader.GetDateTime("ConsumptionDate")
            };
            return item;
        }

        private ManualConsumptionReport LoadManualConsumptionReport(SafeDataReader reader)
        {
            var item = new ManualConsumptionReport
            {
                CustomerName = reader.GetString("CustomerName"),
                AccountNumber = reader.GetString("AccountNumber"),
                ProductLine = reader.GetString("ProductLine"),
                ProductLineDesc = reader.GetString("ProductLineDesc"),
                RefNum = reader.GetString("RefNum"),
                PartDesc = reader.GetString("PartDesc"),
                LotNum = reader.GetString("LotNum"),
                TagId = reader.GetString("TagId"),
                Category = reader.GetString("Category"),
                SubCategory1 = reader.GetString("SubCategory1"),
                SubCategory2 = reader.GetString("SubCategory2"),
                SubCategory3 = reader.GetString("SubCategory3"),
                UpdatedOn = reader.GetDateTime("UpdatedOn"),
                UpdatedBy = reader.GetString("UpdatedBy"),
                IsConsumed = reader.GetBoolean("IsConsumed"),
                ConsumedDate = reader.GetNullableDateTime("ConsumedDate")
            };
            return item;
        }
    }
}
