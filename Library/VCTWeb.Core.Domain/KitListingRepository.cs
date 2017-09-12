using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Globalization;

namespace VCTWeb.Core.Domain
{
    public class KitListingRepository
    {
        private string _user;

        public KitListingRepository()
        {
        }

        public KitListingRepository(string user)
        {
            _user = user;
        }

        public List<KitListing> GetKitsByProcedureAndCatalog(string procedureName, string catalogNumber)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<KitListing> lstKitListing = new List<KitListing>();
            KitListing newKitListing = new KitListing();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETKITSBYPROCEDUREANDCATALOG))
            {
                db.AddInParameter(cmd, "@ProcedureName", DbType.String, procedureName);
                db.AddInParameter(cmd, "@CatalogNumber", DbType.String, catalogNumber);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newKitListing = Load(reader);
                        lstKitListing.Add(newKitListing);
                    }

                }
                return lstKitListing;
            }
        }

        public List<KitListing> GetKitsByKitNumber(string sKitNumber)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<KitListing> lstKitListing = new List<KitListing>();
            KitListing newKitListing = new KitListing();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETKITSBYKITNUMBER))
            {
                db.AddInParameter(cmd, "@KitNumber", DbType.String, sKitNumber);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newKitListing = Load(reader);
                        lstKitListing.Add(newKitListing);
                    }

                }
                return lstKitListing;
            }
        }

        public List<KitListing> GetKitsByKitNumberOrDesc(string sKitNumber, int locationId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<KitListing> lstKitListing = new List<KitListing>();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetKitsByKitNumberOrDesc))
            {
                db.AddInParameter(cmd, "@KitNumber", DbType.String, sKitNumber);
                db.AddInParameter(cmd, "@LocationId", DbType.String, locationId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstKitListing.Add(new KitListing()
                        {
                            KitNumber = reader.GetString("KitNumber"),
                            KitName = reader.GetString("KitName")
                        });
                    }

                }
                return lstKitListing;
            }
        }

        public List<KitListing> GetMappedKitsByKitNumber(string sKitNumber, int LocationId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<KitListing> lstKitListing = new List<KitListing>();
            KitListing newKitListing = new KitListing();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetMappedKitsByKitNumber))
            {
                db.AddInParameter(cmd, "@KitNumber", DbType.String, sKitNumber);
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);

                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newKitListing = Load(reader);
                        lstKitListing.Add(newKitListing);
                    }

                }
                return lstKitListing;
            }
        }

        public KitListing GetKitByKitNumber(string sKitNumber)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            KitListing newKitListing = null;
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetKitByKitNumber))
            {
                db.AddInParameter(cmd, "@KitNumber", DbType.String, sKitNumber);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    if (reader.Read())
                    {
                        newKitListing = Load(reader);
                    }
                }
                return newKitListing;
            }
        }

        public List<KitListing> GetKitsByKitName(string sKitName)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<KitListing> lstKitListing = new List<KitListing>();
            KitListing newKitListing = new KitListing();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETKITSBYKITNAME))
            {
                db.AddInParameter(cmd, "@KitName", DbType.String, sKitName);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newKitListing = Load(reader);
                        lstKitListing.Add(newKitListing);
                    }

                }
                return lstKitListing;
            }
        }

        public List<KitListing> GetManuallyAddedKits(int? LocationId = 0)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<KitListing> lstKitListing = new List<KitListing>();
            KitListing newKitListing = new KitListing();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetManuallyAddedKits))
            {
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);

                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newKitListing = Load(reader);
                        lstKitListing.Add(newKitListing);
                    }
                }
                return lstKitListing;
            }
        }

        public List<KitListing> GetKitListingByLocationIdAndKitFamilyId(Int64 KitFamilyId, int LocationId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<KitListing> lstKitListing = new List<KitListing>();
            KitListing newKitListing = new KitListing();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetKitListingByLocationIdAndKitFamilyId))
            {
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);
                db.AddInParameter(cmd, "@KitFamilyId", DbType.Int64, KitFamilyId);

                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newKitListing = Load(reader);
                        lstKitListing.Add(newKitListing);
                    }
                }
                return lstKitListing;
            }
        }

        private KitListing Load(SafeDataReader reader)
        {
            KitListing newKitListing = new KitListing();

            newKitListing.KitNumber = reader.GetString("KitNumber");
            newKitListing.KitName = reader.GetString("KitName");
            newKitListing.KitDescription = reader.GetString("KitDescription");
            newKitListing.NumberOfSets = reader.GetNullableInt32("NumberOfSets");
            newKitListing.Aisle = reader.GetString("Aisle");
            newKitListing.Row = reader.GetString("Row");
            newKitListing.Tier = reader.GetString("Tier");
            newKitListing.DateCreated = reader.GetNullableDateTime("DateCreated");
            newKitListing.PMSchedule = reader.GetNullableDateTime("PMSchedule");
            newKitListing.Lubricate = reader.GetString("Lubricate");
            newKitListing.KitFamilyId = reader.GetNullableInt64("KitFamilyId");
            newKitListing.IsManuallyAdded = reader.GetBoolean("IsManuallyAdded");
            newKitListing.IsAvailable = reader.GetBoolean("IsAvailable");
            newKitListing.LocationId = reader.GetNullableInt32("LocationId");
            newKitListing.IsActive = reader.GetBoolean("IsActive");
            newKitListing.UpdatedBy = reader.GetString("UpdatedBy");
            newKitListing.Procedure = reader.GetString("Procedure");
            newKitListing.RentalFee = reader.GetDecimal("RentalFee");

            newKitListing.KitFamily = reader.GetString("KitFamily");


            return newKitListing;
        }

        public List<KitTable> usp_GetKitTableListByKitFamily(string KitFamily)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<KitTable> lstKitTable = new List<KitTable>();
            KitTable newKitTable = new KitTable();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetKitTableByKitFamily))
            {
                db.AddInParameter(cmd, "@KitFamily", DbType.String, KitFamily);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newKitTable = LoadKitTable(reader);
                        lstKitTable.Add(newKitTable);
                    }

                }
                return lstKitTable;
            }
        }


        public List<KitTable> usp_GetKitTableListByKitFamilyId(Int64? kitFamilyId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<KitTable> lstKitTable = new List<KitTable>();
            KitTable newKitTable = new KitTable();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetKitTableListByKitFamilyId))
            {
                db.AddInParameter(cmd, "@KitFamilyId", DbType.String, kitFamilyId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newKitTable = LoadKitTable(reader);
                        lstKitTable.Add(newKitTable);
                    }

                }
                return lstKitTable;
            }
        }

        private KitTable LoadKitTable(SafeDataReader reader)
        {
            KitTable newkit = new KitTable();
            //newkit.KitNumber = reader.GetString("KitNumber");
            newkit.Catalognumber = reader.GetString("Catalognumber");
            newkit.Description = reader.GetString("Description");
            newkit.Quantity = reader.GetInt32("Quantity");
            //newkit.CaseLotCode = reader.GetString("CaseLotCode");            
            return newkit;
        }

        public List<KitTable> GetKitTableListByKitNumber(string kitNumber)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<KitTable> lstKitTable = new List<KitTable>();
            KitTable newKitTable = new KitTable();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetKitTableListByKitNumber))
            {
                db.AddInParameter(cmd, "@KitNumber", DbType.String, kitNumber);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newKitTable = LoadKitTableDetail(reader);
                        lstKitTable.Add(newKitTable);
                    }

                }
                return lstKitTable;
            }
        }

        private KitTable LoadKitTableDetail(SafeDataReader reader)
        {
            KitTable newkit = new KitTable();
            newkit.KitNumber = reader.GetString("KitNumber");
            newkit.Catalognumber = reader.GetString("Catalognumber");
            return newkit;
        }

        public bool IsKitNumberDuplicate(string kitNumber)
        {
            Database db = DbHelper.CreateDatabase();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetDuplicateKitNumber))
            {
                db.AddInParameter(cmd, "@KitNumber", DbType.String, kitNumber);

                int count = Convert.ToInt32(db.ExecuteScalar(cmd), CultureInfo.InvariantCulture);

                if (count > 0)
                    return true;
            }

            return false;
        }

        public bool SaveKitListing(KitListing kitListingToBeSaved, string kitTableXml)
        {
            bool returnvalue = false;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_SaveKitListing))
            {
                db.AddInParameter(cmd, "@KitNumber", DbType.String, kitListingToBeSaved.KitNumber);
                db.AddInParameter(cmd, "@KitName", DbType.String, kitListingToBeSaved.KitName);
                db.AddInParameter(cmd, "@KitDescription", DbType.String, kitListingToBeSaved.KitDescription);
                db.AddInParameter(cmd, "@KitFamilyId", DbType.Int32, kitListingToBeSaved.KitFamilyId);
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, kitListingToBeSaved.LocationId);
                db.AddInParameter(cmd, "@RentalFee", DbType.Decimal, kitListingToBeSaved.RentalFee);
                db.AddInParameter(cmd, "@IsActive", DbType.Boolean, kitListingToBeSaved.IsActive);
                db.AddInParameter(cmd, "@Procedure", DbType.String, kitListingToBeSaved.Procedure);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, kitListingToBeSaved.UpdatedBy);
                db.AddInParameter(cmd, "@KitTableXmlString", DbType.String, kitTableXml);
                db.ExecuteScalar(cmd);
                returnvalue = true;
            }
            return returnvalue;
        }


        public bool DeleteKitListing(string SelectedKitNumber)
        {
            bool returnvalue = false;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_DeleteKitListing))
            {
                db.AddInParameter(cmd, "@KitNumber", DbType.String, SelectedKitNumber);
                db.ExecuteScalar(cmd);
                returnvalue = true;
            }
            return returnvalue;
        }
    }
}
