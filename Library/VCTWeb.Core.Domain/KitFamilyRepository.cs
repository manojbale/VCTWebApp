using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using System.Globalization;

namespace VCTWeb.Core.Domain
{
    public class KitFamilyRepository
    {
        private string _user;

        public KitFamilyRepository()
        {
        }

        public KitFamilyRepository(string user)
        {
            _user = user;
        }

        public void SaveKitFamily(KitFamily kitFamily, string KitFamilyPartTableXml, string KitFamilyLocationTableXml)
        {
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_SAVEKITFAMILY))
            {
                db.AddInParameter(cmd, "@KitFamilyId", DbType.Int64, kitFamily.KitFamilyId);
                db.AddInParameter(cmd, "@KitFamilyName", DbType.String, kitFamily.KitFamilyName);
                db.AddInParameter(cmd, "@KitTypeName", DbType.String, kitFamily.KitTypeName);
                db.AddInParameter(cmd, "@NumberOfTubs", DbType.Int16, kitFamily.NumberOfTubs);
                db.AddInParameter(cmd, "@IsActive", DbType.Boolean, kitFamily.IsActive);
                db.AddInParameter(cmd, "@KitFamilyDescription", DbType.String, kitFamily.KitFamilyDescription);
                db.AddInParameter(cmd, "@KitTablePartXmlString", DbType.String, KitFamilyPartTableXml);
                db.AddInParameter(cmd, "@KitTableLocationXmlString", DbType.String, KitFamilyLocationTableXml);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);
                db.ExecuteNonQuery(cmd);
            }
        }
        
        public List<ActiveKitFamily> GetActiveKitFamiliesByLocationId(int locationId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetActiveKitFamiliesByLocationId))
            {
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                List<ActiveKitFamily> lstActiveKitFamily = new List<ActiveKitFamily>();
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstActiveKitFamily.Add(new ActiveKitFamily()
                        {
                            KitFamilyName = reader.GetString("KitFamilyName"),
                            KitFamilyDescription = reader.GetString("KitFamilyDescription"),
                            OrderCount = reader.GetInt32("OrderCount")
                        });
                    }

                }
                return lstActiveKitFamily;
            }
        }

        /// <summary>
        /// Fetches all Contacts.
        /// </summary>
        /// <returns></returns>
        public List<KitFamily> FetchAllKitFamily()
        {
            SafeDataReader reader = null;
            KitFamily newKitFamily = null;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETLISTOFKITFAMILY))
            {

                List<KitFamily> listOfKitFamily = new List<KitFamily>();
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newKitFamily = this.LoadKitFamily(reader);
                        listOfKitFamily.Add(newKitFamily);
                    }

                }
                return listOfKitFamily;
            }
        }

        /// <summary>
        /// Fetches all Contacts.
        /// </summary>
        /// <returns></returns>
        public List<KitFamily> FetchAllKitFamilyByLocationId(int? LocationId)
        {
            SafeDataReader reader = null;
            KitFamily newKitFamily = null;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetKitFamilyByLocationId))
            {
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);
                List<KitFamily> listOfKitFamily = new List<KitFamily>();
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newKitFamily = this.LoadKitFamily(reader);
                        listOfKitFamily.Add(newKitFamily);
                    }

                }
                return listOfKitFamily;
            }
        }

        public KitFamily GetKitFamilyByKitFamilyId(long kitFamilyId)
        {
            SafeDataReader reader = null;
            KitFamily newKitFamily = null;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetKitFamilyById))
            {
                db.AddInParameter(cmd, "@KitFamilyId", DbType.Int64, kitFamilyId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newKitFamily = this.LoadKitFamily(reader);
                    }
                }
                return newKitFamily;
            }
        }

        public List<KitStockLevel> GetKitStockLevelByLocationId(int locationId, int parentLocationId,long kitFamilyId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<KitStockLevel> lstStockLevel = new List<KitStockLevel>();
            KitStockLevel newStockLevel = new KitStockLevel();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetKitStockLevelByLocationId))
            {
                db.AddInParameter(cmd, "LocationId", DbType.Int32, locationId);
                db.AddInParameter(cmd, "ParentLocationId", DbType.Int32, parentLocationId);
                db.AddInParameter(cmd, "@KitFamilyId", DbType.Int64, kitFamilyId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newStockLevel = LoadKitStockLevel(reader);
                        lstStockLevel.Add(newStockLevel);
                    }

                }
                return lstStockLevel;
            }
        }

        private KitStockLevel LoadKitStockLevel(SafeDataReader reader)
        {
            KitStockLevel newKitStockLevel = new KitStockLevel();

            newKitStockLevel.LocationId = reader.GetInt32("LocationId");
            newKitStockLevel.LocationName = reader.GetString("LocationName");
            newKitStockLevel.LocationType = reader.GetString("LocationType");
            newKitStockLevel.KitFamilyId = reader.GetInt64("KitFamilyId");
            newKitStockLevel.KitFamilyName = reader.GetString("KitFamilyName");
            newKitStockLevel.KitFamilyDescription = reader.GetString("KitFamilyDescription");
            newKitStockLevel.LeastExpiryDate = reader.GetDateTime("LeastExpiryDate");
            newKitStockLevel.AvailableQuantity = reader.GetInt32("AvailableQuantity");
            newKitStockLevel.AssignedToCaseQuantity = reader.GetInt32("AssignedToCaseQuantity");
            newKitStockLevel.ShippedQuantity = reader.GetInt32("ShippedQuantity");
            newKitStockLevel.ReceivedQuantity = reader.GetInt32("ReceivedQuantity");
            newKitStockLevel.IsNearExpiry = reader.GetBoolean("IsNearExpiry");
            newKitStockLevel.Longitude = reader.GetDecimal("Longitude");
            newKitStockLevel.Latitude = reader.GetDecimal("Latitude");

            return newKitStockLevel;
        }

        public List<KitDetailStockLevel> GetKitDetailStockLevelByLocationAndKitFamily(int locationId, long kitFamilyId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<KitDetailStockLevel> lstKitDetailStockLevel = new List<KitDetailStockLevel>();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetKitDetailStockLevelByLocationAndKitFamily))
            {
                db.AddInParameter(cmd, "LocationId", DbType.Int32, locationId);
                db.AddInParameter(cmd, "KitFamilyId", DbType.Int64, kitFamilyId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstKitDetailStockLevel.Add(LoadKitDetailStockLevel(reader));
                    }

                }
                return lstKitDetailStockLevel;
            }
        }

        public List<KitDetailStockLevel> GetKitDetailStockLevelByLocationAndParty(int locationId, long partyId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<KitDetailStockLevel> lstKitDetailStockLevel = new List<KitDetailStockLevel>();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetKitDetailStockLevelByLocationAndParty))
            {
                db.AddInParameter(cmd, "LocationId", DbType.Int32, locationId);
                db.AddInParameter(cmd, "PartyId", DbType.Int64, partyId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstKitDetailStockLevel.Add(LoadKitDetailStockLevel(reader));
                    }

                }
                return lstKitDetailStockLevel;
            }
        }

        private KitDetailStockLevel LoadKitDetailStockLevel(SafeDataReader reader)
        {
            KitDetailStockLevel newKitDetailStockLevel = new KitDetailStockLevel();

            newKitDetailStockLevel.BuildKitId = reader.GetInt64("BuildKitId");
            newKitDetailStockLevel.KitNumber = reader.GetString("KitNumber");
            newKitDetailStockLevel.Description = reader.GetString("Description");
            newKitDetailStockLevel.LeastExpiryDate = reader.GetDateTime("LeastExpiryDate");
            newKitDetailStockLevel.Status = reader.GetString("Status");
            newKitDetailStockLevel.LinkedCaseNumber = reader.GetString("LinkedCaseNumber");
            newKitDetailStockLevel.Hospital = reader.GetString("Hospital");
            newKitDetailStockLevel.IsNearExpiry = reader.GetBoolean("IsNearExpiry");

            return newKitDetailStockLevel;
        }

        public bool CheckDuplicateKitFamily(string kitFamilyName, long kitFamilyId)
        {
            Database db = DbHelper.CreateDatabase();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GET_DUPLICATE_KIT_FAMILY))
            {
                db.AddInParameter(cmd, "KitFamilyName", DbType.String, kitFamilyName.Trim());
                db.AddInParameter(cmd, "KitFamilyId", DbType.Int64, kitFamilyId);
                int count = Convert.ToInt32(db.ExecuteScalar(cmd), CultureInfo.InvariantCulture);

                if (count > 0)
                    return true;
            }

            return false;
        }

        private KitFamily LoadKitFamily(SafeDataReader reader)
        {
            KitFamily newKitFamily = new KitFamily();
            newKitFamily.KitFamilyId = reader.GetInt64("KitFamilyId");
            newKitFamily.NumberOfTubs = reader.GetInt16("NumberOfTubs");
            newKitFamily.KitFamilyName = reader.GetString("KitFamilyName").Trim();
            newKitFamily.KitFamilyDescription = reader.GetString("KitFamilyDescription").Trim();
            newKitFamily.KitTypeName = reader.GetString("KitTypeName").Trim();
            newKitFamily.IsActive = reader.GetBoolean("IsActive");
            return newKitFamily;
        }

        public List<KitType> FetchAllKitTypes()
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<KitType> lstKitType = new List<KitType>();
            KitType newKitType = new KitType();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETLISTOFKITTYPES))
            {
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newKitType = LoadKitType(reader);
                        lstKitType.Add(newKitType);
                    }

                }
                return lstKitType;
            }
        }

        private KitType LoadKitType(SafeDataReader reader)
        {
            KitType newKitType = new KitType();

            newKitType.KitTypeName = reader.GetString("KitTypeName");

            return newKitType;
        }

        public bool CheckInUse(long kitFamilyId)
        {
            Database db = DbHelper.CreateDatabase();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_CheckInUseKitFamily))
            {
                db.AddInParameter(cmd, "@KitFamilyId", DbType.Int64, kitFamilyId);
                int count = Convert.ToInt32(db.ExecuteScalar(cmd), CultureInfo.InvariantCulture);

                if (count > 0)
                    return true;
            }

            return false;
        }

        public List<KitFamilyParts> GetKitFamilyPartsByName(string KitFamilyName)
        {
            SafeDataReader reader = null;
            List<KitFamilyParts> lstKitFamilyParts = null;
            KitFamilyParts newKitFamily = null;

            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetKitFamilyPartsByKitFamilyName))
            {
                db.AddInParameter(cmd, "@KitFamilyName", DbType.String, KitFamilyName);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    lstKitFamilyParts = new List<KitFamilyParts>();
                    while (reader.Read())
                    {
                        newKitFamily = this.LoadKitFamilyParts(reader);
                        lstKitFamilyParts.Add(newKitFamily);
                    }
                }
                return lstKitFamilyParts;
            }
        }

        public List<KitFamilyParts> GetKitFamilyPartsById(long kitFamilyId)
        {
            SafeDataReader reader = null;
            List<KitFamilyParts> lstKitFamilyParts = null;
            KitFamilyParts newKitFamily = null;

            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetKitFamilyPartsById))
            {
                db.AddInParameter(cmd, "@KitFamilyId", DbType.Int64, kitFamilyId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    lstKitFamilyParts = new List<KitFamilyParts>();
                    while (reader.Read())
                    {
                        newKitFamily = this.LoadKitFamilyParts(reader);
                        lstKitFamilyParts.Add(newKitFamily);
                    }
                }
                return lstKitFamilyParts;
            }
        }

        private KitFamilyParts LoadKitFamilyParts(SafeDataReader reader)
        {
            KitFamilyParts newKitFamily = new KitFamilyParts();
            newKitFamily.KitFamilyItemId = reader.GetInt64("KitFamilyItemId");
            newKitFamily.KitFamilyId = reader.GetInt64("KitFamilyId");
            newKitFamily.Description = reader.GetString("PartDescription").Trim();
            newKitFamily.CatalogNumber = reader.GetString("CatalogNumber");
            newKitFamily.Quantity = reader.GetInt32("Quantity");

            return newKitFamily;
        }

        public List<KitFamilyLocations> GetKitFamilyLocationsById(long kitFamilyId)
        {
            SafeDataReader reader = null;
            List<KitFamilyLocations> lstKitFamilyLocations = null;
            KitFamilyLocations newKitFamilyLocation = null;

            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetKitFamilyLocationsById))
            {
                db.AddInParameter(cmd, "@KitFamilyId", DbType.Int64, kitFamilyId);

                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    lstKitFamilyLocations = new List<KitFamilyLocations>();
                    while (reader.Read())
                    {
                        newKitFamilyLocation = this.LoadKitFamilyLocations(reader);
                        lstKitFamilyLocations.Add(newKitFamilyLocation);
                    }
                }
                return lstKitFamilyLocations;
            }
        }

        private KitFamilyLocations LoadKitFamilyLocations(SafeDataReader reader)
        {
            KitFamilyLocations oModel = new KitFamilyLocations();
            oModel.LocationId = reader.GetInt32("LocationId");
            oModel.LocationName = reader.GetString("LocationName");
            oModel.LocationType = reader.GetString("LocationType");
            oModel.LocationExists = reader.GetBoolean("LocationExists");

            return oModel;
        }


        public List<NewProductTransfer> GetKitFamilyLocationsByLocationId(long kitFamilyId)
        {
            SafeDataReader reader = null;
            List<NewProductTransfer> lstKitFamilyLocations = null;
            NewProductTransfer newKitFamilyLocation = null;

            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetKitFamilyLocationsById))
            {
                db.AddInParameter(cmd, "@KitFamilyId", DbType.Int64, kitFamilyId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    lstKitFamilyLocations = new List<NewProductTransfer>();
                    while (reader.Read())
                    {
                        newKitFamilyLocation = this.LoadKitFamilyLocationByLocationId(reader);
                        lstKitFamilyLocations.Add(newKitFamilyLocation);
                    }
                }
                return lstKitFamilyLocations;
            }
        }

        public List<NewProductTransfer> GetKitFamilyLocationsByParentLocationId(Int64 KitFamilyId)
        {
            SafeDataReader reader = null;
            List<NewProductTransfer> lstKitFamilyLocations = null;
            NewProductTransfer newKitFamilyLocation = null;

            Database db = DbHelper.CreateDatabase();
            //using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetLocationByParentLocationId))
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetLocationsByKitFamilyId))
            {
                db.AddInParameter(cmd, "@KitFamilyId", DbType.Int64, KitFamilyId);

                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    lstKitFamilyLocations = new List<NewProductTransfer>();
                    while (reader.Read())
                    {
                        newKitFamilyLocation = this.LoadKitFamilyLocationByLocationId(reader);
                        lstKitFamilyLocations.Add(newKitFamilyLocation);
                    }
                }
                return lstKitFamilyLocations;
            }
        }


        private NewProductTransfer LoadKitFamilyLocationByLocationId(SafeDataReader reader)
        {
            NewProductTransfer oModel = new NewProductTransfer();
            oModel.LocationId = reader.GetInt32("LocationId");
            oModel.LocationName = reader.GetString("LocationName");
            oModel.LocationTypeName = reader.GetString("LocationTypeName");
            oModel.LocationStatus = reader.GetInt32("LocationStatus");
            return oModel;
        }

        public bool SaveKitFamilyPartDetail(KitFamilyParts oModel)
        {
            bool returnvalue = false;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_SaveKitFamilyPartDetail))
            {
                db.AddInParameter(cmd, "@KitFamilyId", DbType.Int64, oModel.KitFamilyId);
                db.AddInParameter(cmd, "@CatalogNumber", DbType.String, oModel.CatalogNumber);
                db.AddInParameter(cmd, "@Quantity", DbType.Int32, oModel.Quantity);
                db.ExecuteScalar(cmd);
                returnvalue = true;
            }
            return returnvalue;
        }

        public bool UpdateKitFamilyPartQtyById(KitFamilyParts oModel)
        {
            bool returnvalue = false;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_UpdateKitFamilyPartQty))
            {
                db.AddInParameter(cmd, "@KitFamilyItemId", DbType.Int64, oModel.KitFamilyItemId);
                db.AddInParameter(cmd, "@Quantity", DbType.Int32, oModel.Quantity);
                db.ExecuteScalar(cmd);

                returnvalue = true;
            }
            return returnvalue;
        }

        public bool DeleteKitFamilyPartById(Int64 KitFamilyItemId)
        {
            bool returnvalue = false;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_DeleteKitFamilyPartById))
            {
                db.AddInParameter(cmd, "@KitFamilyItemId", DbType.Int64, KitFamilyItemId);
                db.ExecuteScalar(cmd);

                returnvalue = true;
            }
            return returnvalue;
        }

        public List<PartStockLevel> GetPartStockLevelByLocationId(int locationId, int parentLocationId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<PartStockLevel> lstPartStockLevel = new List<PartStockLevel>();
            PartStockLevel newPartStockLevel = new PartStockLevel();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetPartStockLevelByLocationId))
            {
                db.AddInParameter(cmd, "LocationId", DbType.Int32, locationId);
                db.AddInParameter(cmd, "ParentLocationId", DbType.Int32, parentLocationId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newPartStockLevel = LoadPartStockLevel(reader);
                        lstPartStockLevel.Add(newPartStockLevel);
                    }

                }
                return lstPartStockLevel;
            }
        }

        private PartStockLevel LoadPartStockLevel(SafeDataReader reader)
        {
            PartStockLevel newStockLevel = new PartStockLevel();

            newStockLevel.LocationId = reader.GetInt32("LocationId");
            newStockLevel.LocationName = reader.GetString("LocationName");
            newStockLevel.LocationType = reader.GetString("LocationType");
            newStockLevel.PartNumber = reader.GetString("PartNumber");
            newStockLevel.Description = reader.GetString("Description");
            newStockLevel.LeastExpiryDate = reader.GetDateTime("LeastExpiryDate");
            newStockLevel.AvailableQuantity = reader.GetInt32("AvailableQuantity");
            newStockLevel.AssignedToCaseQuantity = reader.GetInt32("AssignedToCaseQuantity");
            newStockLevel.IsNearExpiry = reader.GetBoolean("IsNearExpiry");

            return newStockLevel;
        }

        public List<PartDetailStockLevel> GetPartDetailStockLevelByLocationAndPart(int locationId, string partNum)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<PartDetailStockLevel> lstPartDetailStockLevel = new List<PartDetailStockLevel>();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetPartDetailStockLevelByLocationAndPart))
            {
                db.AddInParameter(cmd, "LocationId", DbType.Int32, locationId);
                db.AddInParameter(cmd, "PartNum", DbType.String, partNum);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstPartDetailStockLevel.Add(LoadPartDetailStockLevel(reader));
                    }

                }
                return lstPartDetailStockLevel;
            }
        }

        private PartDetailStockLevel LoadPartDetailStockLevel(SafeDataReader reader)
        {
            PartDetailStockLevel newPartDetailStockLevel = new PartDetailStockLevel();

            newPartDetailStockLevel.LotNum = reader.GetString("LotNum");
            newPartDetailStockLevel.Description = reader.GetString("Description");
            newPartDetailStockLevel.ExpiryDate = reader.GetDateTime("ExpiryDate");
            newPartDetailStockLevel.Status = reader.GetString("Status");
            newPartDetailStockLevel.LinkedCaseNumber = reader.GetString("LinkedCaseNumber");
            newPartDetailStockLevel.IsNearExpiry = reader.GetBoolean("IsNearExpiry");

            return newPartDetailStockLevel;
        }

        public List<CaseKitFamilyDetailGroup> FetchAllKitFamilyByProcedureName(string sProcedureName, string sPhysicianName)
        {
            SafeDataReader reader = null;
            CaseKitFamilyDetailGroup newKitFamilyDetail = null;
            Database db = DbHelper.CreateDatabase();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetKitsFamilyByProcedureName))
            {
                db.AddInParameter(cmd, "@ProcedureName", DbType.String, sProcedureName);
                db.AddInParameter(cmd, "@PhysicianName", DbType.String, sPhysicianName);
                List<CaseKitFamilyDetailGroup> listOfKitFamily = new List<CaseKitFamilyDetailGroup>();
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newKitFamilyDetail = this.LoadKitFamilyDetailGroup(reader);
                        listOfKitFamily.Add(newKitFamilyDetail);
                    }

                }
                return listOfKitFamily;
            }
        }


        private CaseKitFamilyDetailGroup LoadKitFamilyDetailGroup(SafeDataReader reader)
        {
            CaseKitFamilyDetailGroup newCaseKitFamilyDetailGroup = new CaseKitFamilyDetailGroup();

            newCaseKitFamilyDetailGroup.KitFamilyName = reader.GetString("KITFAMILYNAME");
            newCaseKitFamilyDetailGroup.KitFamilyDescription = reader.GetString("DESCRIPTION");
            newCaseKitFamilyDetailGroup.Quantity = Convert.ToInt32(reader["QUANTITY"]);
            newCaseKitFamilyDetailGroup.KitFamilyId = Convert.ToInt64(reader["KITFAMILYID"]);          
            return newCaseKitFamilyDetailGroup;
        }


        public List<VirtualCheckOut> GetInventoryReport(Int32 LocationId,long KitFamilyId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<VirtualCheckOut> lstCases;

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetInventoryReportData))
            {
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);
                db.AddInParameter(cmd, "@KitFamilyId", DbType.Int64, KitFamilyId);

                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    lstCases = new List<VirtualCheckOut>();
                    while (reader.Read())
                    {
                        lstCases.Add(new VirtualCheckOut()
                        {

                            KitNumber = reader.GetString("KitNumber"),
                            Description = reader.GetString("KitDescription"),
                            PartNum = reader.GetString("PartNum"),
                            PartDescription = reader.GetString("PartDescription"),
                            LotNum = reader.GetString("LotNum"),
                            ExpiryDate = reader.GetDateTime("ExpiryDate"),
                            BuildDate = reader.GetDateTime("BuildDate")



                        });
                    }
                }            
                return lstCases;
            }
        }

       

    }
}
