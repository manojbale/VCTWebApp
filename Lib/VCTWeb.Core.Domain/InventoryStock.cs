using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace VCTWeb.Core.Domain
{
    [Serializable]
    public class InventoryStockKit
    {
        public long BuildKitId { get; set; }
        public string KitNumber { get; set; }
        public long KitFamilyId { get; set; }
    }

    [Serializable]
    public class InventoryStockPart
    {
        public long LocationPartDetailId { get; set; }
        public string LotNum { get; set; }
        public string PartNum { get; set; }
    }

    [Serializable]
    public class VirtualCheckOut
    {
        public Int64 CaseId { get; set; }

        public Int64 CaseKitId { get; set; }
        public Int64 BuildKitId { get; set; }
        public string KitFamilyName { get; set; }
        public string KitNumber { get; set; }

        public Int64 LocationPartDetailId { get; set; }
        public Int64 CasePartId { get; set; }
        public string PartNum { get; set; }
        public string LotNum { get; set; }

        public string Description { get; set; }
        public DateTime ExpiryDate { get; set; }

        public bool ShippedStatus { get; set; }
        public bool IsNearExpiry { get; set; }
        public string PartStatus { get; set; }
        
        public string IsUsageMarked { get; set; }
        public string UsageMarkedBy { get; set; }
        public DateTime UsageMarkedOn { get; set; }

        public string CaseType { get; set; }
        public int DispositionTypeId { get; set; }
        public string DispositionType { get; set; }
        public string Remarks { get; set; }
        public bool SeekReturn { get; set; }
        public int Qty { get; set; }
        public string KitStatus { get; set; }
        public bool SendReplacement { get; set; }
        public string PartDescription { get; set; }
        public string CaseNumber { get; set; }
        public DateTime BuildDate { get; set; }
    }

    [Serializable]
    public class InventoryTransfer
    {
        public Int64 KitId { get; set; }

        public Int64 KitFamilyId { get; set; }
        public string KitFamilyName { get; set; }

        public string PartNum { get; set; }
        
        public string Description { get; set; }
        public DateTime LastUsage { get; set; }
        public int AvailableQty { get; set; }
        public int TransferQty { get; set; }

        public int LocationId { get; set; }
        public string LocationName {get; set;}
    }

    [Serializable]
    public class HospitalInventoryTransfer
    {
        public Int64 KitId { get; set; }

        public Int64 KitFamilyId { get; set; }
        public string KitFamilyName { get; set; }

        public string PartNum { get; set; }

        public string Description { get; set; }
        public DateTime LastUsage { get; set; }
        public int AvailableQty { get; set; }
        public int TransferQty { get; set; }

        public int LocationId { get; set; }
        public string LocationName { get; set; }
    }

    public class InventoryStockRepository
    {
        private string _user;

        public InventoryStockRepository()
        {
        }

        public InventoryStockRepository(string user)
        {
            _user = user;
        }

        public List<InventoryStockKit> GetKitNumbersToBeAssigned(long caseId, int locationId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<InventoryStockKit> InventoryStockKitList = new List<InventoryStockKit>();
            InventoryStockKit InventoryStockKit = new InventoryStockKit();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetKitNumbersToBeAssignedByCaseId))
            {
                db.AddInParameter(cmd, "@CaseId", DbType.Int64, caseId);
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        InventoryStockKit = LoadInventoryStockKit(reader);
                        InventoryStockKitList.Add(InventoryStockKit);
                    }
                }
                return InventoryStockKitList;
            }
        }

        private InventoryStockKit LoadInventoryStockKit(SafeDataReader reader)
        {
            InventoryStockKit inventoryStockKit = new InventoryStockKit();
            inventoryStockKit.BuildKitId = reader.GetInt64("BuildKitId");
            inventoryStockKit.KitNumber = reader.GetString("KitNumber");
            inventoryStockKit.KitFamilyId = reader.GetInt64("KitFamilyId");
            return inventoryStockKit;
        }

        private InventoryStockPart LoadInventoryStockPart(SafeDataReader reader)
        {
            InventoryStockPart inventoryStockPart = new InventoryStockPart();
            inventoryStockPart.LocationPartDetailId = reader.GetInt64("LocationPartDetailId");
            inventoryStockPart.LotNum = reader.GetString("LotNum");
            inventoryStockPart.PartNum = reader.GetString("PartNum");
            return inventoryStockPart;
        }

        public bool AssignKitInventory(long caseId, string BuildKitIds, string CaseStatus)
        {
            bool flagSuccesssfullyAssigned = false;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_AssignKitInventory))
            {
                db.AddInParameter(cmd, "@CaseId", DbType.Int64, caseId);
                db.AddInParameter(cmd, "@BuildKitIds", DbType.String, BuildKitIds);
                db.AddInParameter(cmd, "@CaseStatus", DbType.String, CaseStatus);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);
                flagSuccesssfullyAssigned = Convert.ToBoolean(db.ExecuteScalar(cmd));
            }
            return flagSuccesssfullyAssigned;
        }

        //public List<InventoryStockPart> GetLotNumbersToBeAssigned(string partNumber, int locationId)
        //{
        //    SafeDataReader reader = null;
        //    Database db = DbHelper.CreateDatabase();
        //    List<InventoryStockPart> lstInventoryStockPart = new List<InventoryStockPart>();
        //    InventoryStockPart newInventoryStockPart = new InventoryStockPart();
        //    using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetLotNumbersToBeAssigned))
        //    {
        //        db.AddInParameter(cmd, "@PartNum", DbType.String, partNumber);
        //        db.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
        //        using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
        //        {
        //            while (reader.Read())
        //            {
        //                newInventoryStockPart = LoadInventoryStockPart(reader);
        //                lstInventoryStockPart.Add(newInventoryStockPart);
        //            }
        //        }
        //        return lstInventoryStockPart;
        //    }
        //}

        public List<InventoryStockPart> GetLotNumbersToBeAssignedByCaseId(long caseId, int locationId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<InventoryStockPart> lstInventoryStockPart = new List<InventoryStockPart>();
            InventoryStockPart newInventoryStockPart = new InventoryStockPart();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetLotNumbersToBeAssignedByCaseId))
            {
                db.AddInParameter(cmd, "@CaseId", DbType.Int64, caseId);
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newInventoryStockPart = LoadInventoryStockPart(reader);
                        lstInventoryStockPart.Add(newInventoryStockPart);
                    }
                }
                return lstInventoryStockPart;
            }
        }

        public bool AssignPartInventory(long CaseId, string casePartDetailXmlString, string CaseStatus)
        {
            bool flagSuccessfullyAssigned = false;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_AssignPartInventory))
            {
                db.AddInParameter(cmd, "@CaseId", DbType.Int64, CaseId);
                db.AddInParameter(cmd, "@CasePartDetailXmlString", DbType.String, casePartDetailXmlString);
                db.AddInParameter(cmd, "@CaseStatus", DbType.String, CaseStatus);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);
                flagSuccessfullyAssigned= Convert.ToBoolean(db.ExecuteScalar(cmd));
            }

            return flagSuccessfullyAssigned;
        }

        public List<CaseSmall> ShippingPendingCasesList(Int32 LocationId, string CaseType)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();            
            List<CaseSmall> lstCases;

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetShippingPendingCasesByLocId))
            {                
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);
                db.AddInParameter(cmd, "@CaseType", DbType.String, CaseType);

                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    lstCases = new List<CaseSmall>();
                    while (reader.Read())
                    {
                        lstCases.Add(new CaseSmall() { 
                            CaseId = reader.GetInt64("CaseId"),
                            CaseNumber = reader.GetString("CaseNumber")
                        });
                    }
                }
                return lstCases;
            }
        }

        public List<CaseSmall> CheckInPendingCasesList(Int32 LocationId, string CaseType)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<CaseSmall> lstCases;

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetCheckedInPendingCasesByLocId))
            {
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);
                db.AddInParameter(cmd, "@CaseType", DbType.String, CaseType);
                
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    lstCases = new List<CaseSmall>();
                    while (reader.Read())
                    {
                        lstCases.Add(new CaseSmall()
                        {
                            CaseId = reader.GetInt64("CaseId"),
                            CaseNumber = reader.GetString("CaseNumber")
                        });
                    }
                }
                return lstCases;
            }
        }

        public List<CaseSmall> ReCheckInPendingCasesList(Int32 LocationId, string CaseType)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<CaseSmall> lstCases;

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetReCheckedInPendingCasesByLocId))
            {
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);
                db.AddInParameter(cmd, "@CaseType", DbType.String, CaseType);

                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    lstCases = new List<CaseSmall>();
                    while (reader.Read())
                    {
                        lstCases.Add(new CaseSmall()
                        {
                            CaseId = reader.GetInt64("CaseId"),
                            CaseNumber = reader.GetString("CaseNumber")
                        });
                    }
                }
                return lstCases;
            }
        }

        public List<VirtualCheckOut> GetShippingPartsByCaseId(Int64 CaseId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<VirtualCheckOut> lstCases;

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetShippingPartsByCaseId))
            {
                db.AddInParameter(cmd, "@CaseId", DbType.Int64, CaseId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    lstCases = new List<VirtualCheckOut>();
                    while (reader.Read())
                    {
                        lstCases.Add(new VirtualCheckOut()
                        {
                            CasePartId = reader.GetInt64("CasePartId"),
                            PartNum = reader.GetString("PartNum"),
                            Description = reader.GetString("Description"),
                            LotNum = reader.GetString("LotNum"),
                            ExpiryDate = reader.GetDateTime("ExpiryDate"),
                            IsNearExpiry = reader.GetBoolean("IsNearExpiry"),
                            LocationPartDetailId = reader.GetInt64("LocationPartDetailId"),
                            CaseType = reader.GetString("CaseType"),
                            DispositionType = reader.GetString("DispositionType"),
                            DispositionTypeId = reader.GetInt32("DispositionTypeId"),
                            Remarks = reader.GetString("Remarks"),
                            SeekReturn = reader.GetBoolean("SeekReturn"),
                            SendReplacement = reader.GetBoolean("SendReplacement"),
                            PartStatus = reader.GetString("PartStatus")
                        });
                    }
                }
                return lstCases;
            }
        }

        public List<VirtualCheckOut> GetShippingKitsByCaseId(Int64 CaseId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<VirtualCheckOut> lstCases;

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetShippingKitsByCaseId))
            {
                db.AddInParameter(cmd, "@CaseId", DbType.Int64, CaseId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    lstCases = new List<VirtualCheckOut>();
                    while (reader.Read())
                    {
                        lstCases.Add(new VirtualCheckOut()
                        {
                            CaseKitId = reader.GetInt64("CaseKitId"),
                            BuildKitId = reader.GetInt64("BuildKitId"),
                            KitFamilyName = reader.GetString("KitFamilyName"),
                            KitNumber = reader.GetString("KitNumber"),
                            Description = reader.GetString("KitFamilyDescription"),
                            KitStatus = reader.GetString("KitStatus"),
                            Qty = reader.GetInt32("Quantity")
                        });
                    }
                }
                return lstCases;
            }
        }

        public bool SaveShippingDetails(long CaseId, string InventoryType, string TableXML, string _user, String FinalCaseStatus)
        {
            bool result = false;

            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_SaveShippingDetails))
            {
                db.AddInParameter(cmd, "@CaseId", DbType.Int64, CaseId);
                db.AddInParameter(cmd, "@InventoryType", DbType.String, InventoryType);
                db.AddInParameter(cmd, "@TableXML", DbType.String, TableXML);
                db.AddInParameter(cmd, "@CaseStatus", DbType.String, FinalCaseStatus);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);

                result = Convert.ToBoolean(db.ExecuteScalar(cmd));
            }

            return result;
        }

        public bool SaveCheckedInDetails(long CaseId, string InventoryType, string TableXML, string _user, int LocationId, out string ResultCaseNum, string FinalStatus)
        {
            bool result = false;

            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_SaveCheckedInDetails))
            {
                db.AddInParameter(cmd, "@CaseId", DbType.Int64, CaseId);
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);
                db.AddInParameter(cmd, "@InventoryType", DbType.String, InventoryType);
                db.AddInParameter(cmd, "@TableXML", DbType.String, TableXML);
                db.AddInParameter(cmd, "@CaseStatus", DbType.String, FinalStatus);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);
                db.AddOutParameter(cmd, "@RecordStatus", DbType.Boolean, 1);
                
                ResultCaseNum = Convert.ToString(db.ExecuteScalar(cmd));

                result = Convert.ToBoolean(db.GetParameterValue(cmd, "@RecordStatus"));
            }

            return result;
        }

        public bool SaveReCheckedInDetails(long CaseId, string InventoryType, string TableXML, string _user)
        {
            bool result = false;
             
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_SaveReCheckedInDetails))
            {
                db.AddInParameter(cmd, "@CaseId", DbType.Int64, CaseId);
                db.AddInParameter(cmd, "@InventoryType", DbType.String, InventoryType);
                db.AddInParameter(cmd, "@TableXML", DbType.String, TableXML);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);

                result = Convert.ToBoolean(db.ExecuteScalar(cmd));
            }

            return result;
        }

        public bool SaveHospitalInventoryTransfer(Int32 LocationId, string InventoryType, Int64 FromPartyId, Int64 ToPartyId, string TableXML, string _user, out string ResultCaseNum)
        {
            bool flag = false;
            ResultCaseNum = "";
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_SaveHospitalInventoryTransferDetails))
            {
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);
                db.AddInParameter(cmd, "@InventoryType", DbType.String, InventoryType);
                db.AddInParameter(cmd, "@FromPartyId", DbType.Int64, FromPartyId);
                db.AddInParameter(cmd, "@ToPartyId", DbType.Int64, ToPartyId);                
                db.AddInParameter(cmd, "@TableXML", DbType.String, TableXML);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);
                db.AddOutParameter(cmd, "@RecordStatus", DbType.Boolean, 1);

                ResultCaseNum = Convert.ToString(db.ExecuteScalar(cmd));
                flag = Convert.ToBoolean(db.GetParameterValue(cmd, "@RecordStatus")); ;
            }

            return flag;
        }

        public bool SaveInventoryTransfer(Int32 FromLocationId, string InventoryType, string TableXML, string _user, out string result)
        {
            bool flag = false;
            result = "";
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_SaveInventoryTransferDetails))
            {
                db.AddInParameter(cmd, "@FromLocationId", DbType.Int64, FromLocationId);
                db.AddInParameter(cmd, "@InventoryType", DbType.String, InventoryType);
                db.AddInParameter(cmd, "@TableXML", DbType.String, TableXML);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);

                result = Convert.ToString(db.ExecuteScalar(cmd));
                flag = true;
            }

            return flag;
        }

        public List<VirtualCheckOut> PopulateBuildKitById(long BuildKitId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<VirtualCheckOut> lstBuildKit;

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetBuildKitById))
            {

                db.AddInParameter(cmd, "@BuildKitId", DbType.Int64, BuildKitId);

                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    lstBuildKit = new List<VirtualCheckOut>();
                    while (reader.Read())
                    {
                        lstBuildKit.Add(new VirtualCheckOut()
                        {
                            LocationPartDetailId = reader.GetInt64("LocationpartDetailId"),
                            PartNum = reader.GetString("PartNum"),
                            Description = reader.GetString("Description"),
                            LotNum = reader.GetString("LotNum"),
                            ExpiryDate = reader.GetDateTime("ExpiryDate"),
                            IsNearExpiry = reader.GetBoolean("IsNearExpiry"),
                            PartStatus = reader.GetString("PartStatus")
                        });
                    }
                }
                return lstBuildKit;
            }
        }

        public List<VirtualCheckOut> PopulateCheckOutBuildKitById(long BuildKitId, Int64 CaseId =0) 
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<VirtualCheckOut> lstBuildKit;

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetCheckOutBuildKitById))
            {
                db.AddInParameter(cmd, "@CaseId", DbType.Int64, CaseId);                
                db.AddInParameter(cmd, "@BuildKitId", DbType.Int64, BuildKitId);

                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    lstBuildKit = new List<VirtualCheckOut>();
                    while (reader.Read())
                    {
                        lstBuildKit.Add(new VirtualCheckOut()
                        {
                            LocationPartDetailId = reader.GetInt64("LocationpartDetailId"),
                            PartNum = reader.GetString("PartNum"),
                            Description = reader.GetString("Description"),
                            LotNum = reader.GetString("LotNum"),
                            ExpiryDate = reader.GetDateTime("ExpiryDate"),
                            IsNearExpiry = reader.GetBoolean("IsNearExpiry"),
                            PartStatus = reader.GetString("PartStatus")
                        });
                    }
                }
                return lstBuildKit;
            }
        }

        public List<VirtualCheckOut> GetCheckOutKitByCaseKitId(Int64 CaseKitId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<VirtualCheckOut> lstBuildKit;

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetCheckOutKitByCaseKitId))
            {
                db.AddInParameter(cmd, "@CaseKitId", DbType.Int64, CaseKitId);
                
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    lstBuildKit = new List<VirtualCheckOut>();
                    while (reader.Read())
                    {
                        lstBuildKit.Add(new VirtualCheckOut()
                        {
                            LocationPartDetailId = reader.GetInt64("LocationpartDetailId"),
                            PartNum = reader.GetString("PartNum"),
                            Description = reader.GetString("Description"),
                            LotNum = reader.GetString("LotNum"),
                            ExpiryDate = reader.GetDateTime("ExpiryDate"),
                            IsNearExpiry = reader.GetBoolean("IsNearExpiry"),
                            PartStatus = reader.GetString("PartStatus"),
                            IsUsageMarked = reader.GetString("IsUsageMarked"),
                            UsageMarkedBy= reader.GetString("UsageMarkedBy"),
                            UsageMarkedOn= reader.GetDateTime("UsageMarkedOn")
                          
      
                        });
                    }
                }
                return lstBuildKit;
            }
        }

        //public List<VirtualCheckOut> PopulateCheckOutBuildKitById(long BuildKitId)
        //{
        //    SafeDataReader reader = null;
        //    Database db = DbHelper.CreateDatabase();
        //    List<VirtualCheckOut> lstBuildKit;

        //    using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetCheckedInBuildKitById))
        //    {
        //        db.AddInParameter(cmd, "@BuildKitId", DbType.Int64, BuildKitId);

        //        using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
        //        {
        //            lstBuildKit = new List<VirtualCheckOut>();
        //            while (reader.Read())
        //            {
        //                lstBuildKit.Add(new VirtualCheckOut()
        //                {
        //                    LocationPartDetailId = reader.GetInt64("LocationpartDetailId"),
        //                    PartNum = reader.GetString("PartNum"),
        //                    Description = reader.GetString("Description"),
        //                    LotNum = reader.GetString("LotNum"),
        //                    ExpiryDate = reader.GetDateTime("ExpiryDate"),
        //                    IsNearExpiry = reader.GetBoolean("IsNearExpiry"),
        //                    PartStatus = reader.GetString("PartStatus")
        //                });
        //            }
        //        }
        //        return lstBuildKit;
        //    }
        //}

        public List<VirtualCheckOut> PopulateBuildKitItemsByLocationAndKitFamily(int locationId, long kitFamilyId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<VirtualCheckOut> lstBuildKit;

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetBuildKitItemsByLocationAndKitFamily))
            {
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                db.AddInParameter(cmd, "@KitFamilyId", DbType.Int64, kitFamilyId);

                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    lstBuildKit = new List<VirtualCheckOut>();
                    while (reader.Read())
                    {
                        lstBuildKit.Add(new VirtualCheckOut()
                        {
                            BuildKitId = reader.GetInt64("BuildKitId"),
                            LocationPartDetailId = reader.GetInt64("LocationpartDetailId"),
                            PartNum = reader.GetString("PartNum"),
                            Description = reader.GetString("Description"),
                            LotNum = reader.GetString("LotNum"),
                            ExpiryDate = reader.GetDateTime("ExpiryDate"),
                            IsNearExpiry = reader.GetBoolean("IsNearExpiry")
                        });
                    }
                }
            }

            return lstBuildKit;
        }

        public List<VirtualCheckOut> PopulateBuildKitItemsByLocationAndParty(int locationId, long partyId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<VirtualCheckOut> lstBuildKit;

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetBuildKitItemsByLocationAndParty))
            {
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                db.AddInParameter(cmd, "@PartyId", DbType.Int64, partyId);

                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    lstBuildKit = new List<VirtualCheckOut>();
                    while (reader.Read())
                    {
                        lstBuildKit.Add(new VirtualCheckOut()
                        {
                            BuildKitId = reader.GetInt64("BuildKitId"),
                            LocationPartDetailId = reader.GetInt64("LocationpartDetailId"),
                            PartNum = reader.GetString("PartNum"),
                            Description = reader.GetString("Description"),
                            LotNum = reader.GetString("LotNum"),
                            ExpiryDate = reader.GetDateTime("ExpiryDate"),
                            IsNearExpiry = reader.GetBoolean("IsNearExpiry")
                        });
                    }
                }
            }

            return lstBuildKit;
        }

        public List<InventoryTransfer> PopulateUnUtilizeInventoryParts(Int32 LocationId, Int32 InventoryDays)
        {            
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<InventoryTransfer> lstInventoryTransfer = null;

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetUnutilizePartsListByDays))
            {
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);
                db.AddInParameter(cmd, "@InventoryDays", DbType.Int32, InventoryDays);
                
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    lstInventoryTransfer = new List<InventoryTransfer>();
                    while (reader.Read())
                    {
                        lstInventoryTransfer.Add(new InventoryTransfer()
                        {
                            PartNum = reader.GetString("PartNum"),
                            Description = reader.GetString("Description"),
                            AvailableQty = reader.GetInt32("AvailableQty"),
                            LastUsage = reader.GetDateTime("LastUsage")
                        });
                    }
                }                
            }

            return lstInventoryTransfer;
        }

        public List<InventoryTransfer> PopulateUnUtilizeInventoryKits(Int32 LocationId, Int32 InventoryDays)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<InventoryTransfer> lstInventoryTransfer = null;

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetUnutilizeKitsListByDays))
            {
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);
                db.AddInParameter(cmd, "@InventoryDays", DbType.Int32, InventoryDays);

                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    lstInventoryTransfer = new List<InventoryTransfer>();
                    while (reader.Read())
                    {
                        lstInventoryTransfer.Add(new InventoryTransfer()
                        {
                            KitFamilyId = reader.GetInt64("KitFamilyId"),
                            KitFamilyName = reader.GetString("KitFamilyName"),
                            Description = reader.GetString("Description"),
                            AvailableQty = reader.GetInt32("AvailableQty"),
                            LastUsage = reader.GetDateTime("LastUsage")
                        });
                    }
                }
            }

            return lstInventoryTransfer;
        }




        public List<VirtualCheckOut> GetReplenishmentPlanning(Int32 LocationId, DateTime FromDate, DateTime ToDate)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<VirtualCheckOut> lstCases;

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetReplenishmentPlanning_Data))
            {
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);
                db.AddInParameter(cmd, "@FromDate", DbType.DateTime, FromDate);
                db.AddInParameter(cmd, "@ToDate", DbType.DateTime, ToDate);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    lstCases = new List<VirtualCheckOut>();
                    while (reader.Read())
                    {
                        lstCases.Add(new VirtualCheckOut()
                        {
                            CaseKitId = reader.GetInt64("CaseKitId"),
                            BuildKitId = reader.GetInt64("BuildKitId"),

                            KitNumber = reader.GetString("KitNumber"),
                            KitFamilyName = reader.GetString("KitFamilyName"),                           
                            Description = reader.GetString("KitFamilyDescription"),
                            KitStatus = reader.GetString("KitStatus"),

                            PartNum=reader.GetString("PartNum"),
                            LotNum = reader.GetString("LotNum"),
                            PartDescription = reader.GetString("Description"),
                            ExpiryDate = reader.GetDateTime("ExpiryDate"),
                            CaseNumber = reader.GetString("CaseNumber"),
                            UsageMarkedBy = reader.GetString("UsageMarkedBy")
                          
                        });
                    }
                }
                return lstCases;
            }
        }


    

     

    }
}
