using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace VCTWeb.Core.Domain
{
    public class KitTableRepository
    {
        private string _user;

        public KitTableRepository()
        {
        }

        public KitTableRepository(string user)
        {
            _user = user;
        }
        public List<Catalog> GetCatalogByCatalogNumber(string sCatalogNumber)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<Catalog> lstCatalog = new List<Catalog>();
            Catalog newCatalog = new Catalog();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETCATALOGBYCATALOGNUMBER))
            {
                db.AddInParameter(cmd, "@CatalogNumber", DbType.String, sCatalogNumber);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newCatalog = Load(reader);
                        lstCatalog.Add(newCatalog);
                    }

                }
                return lstCatalog;
            }
        }

        public List<PartyAvailableCatalog> GetCatalogCountByCatalogNumber(Int32 LocationId, Int64 PartyId, string sCatalogNumber)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<PartyAvailableCatalog> lstPartyAvailableCatalog = new List<PartyAvailableCatalog>();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetPartyAvailableQtyById))
            {
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);
                db.AddInParameter(cmd, "@PartyId", DbType.Int64, PartyId);
                db.AddInParameter(cmd, "@CatalogNumber", DbType.String, sCatalogNumber);

                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstPartyAvailableCatalog.Add(new PartyAvailableCatalog()
                        {
                            CatalogNumber = reader.GetString("CatalogNumber"),
                            LotNum = reader.GetString("LotNum"),
                            Description = reader.GetString("Description"),
                            CatalogFull = reader.GetString("CatalogFull"),
                            AvailableQty = reader.GetInt32("AvailableQty")
                        });
                    }
                }
                return lstPartyAvailableCatalog;
            }
        }

        public List<Catalog> GetRMAPartsByPartNum(string sCatalogNumber, Int64 PartyId, Int32 LocationId, Int64 CaseShipFromLocationId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<Catalog> lstCatalog = new List<Catalog>();
            Catalog newCatalog = new Catalog();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetRMAPartsByCatalogNumber))
            {
                db.AddInParameter(cmd, "@CatalogNumber", DbType.String, sCatalogNumber);
                db.AddInParameter(cmd, "@PartyId", DbType.Int64, PartyId);
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);
                db.AddInParameter(cmd, "@CaseShipFromLocationId", DbType.Int64, CaseShipFromLocationId);

                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstCatalog.Add(new Catalog()
                        {
                            //LocationPartDetailId = reader.GetInt64("LocationPartDetailId"),
                            CatalogNumber = reader.GetString("CatalogNumber"),
                            Description = reader.GetString("Description"),
                            CatalogFull = reader.GetString("CatalogFull")
                        });
                        
                    }

                }
                return lstCatalog;
            }
        }

        public List<Cases> GetRMACasesByCaseNum(string sCaseNumber, Int64 PartyId, Int32 LocationId, Int64 CaseShipFromLocationId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<Cases> lstCases = new List<Cases>();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetRMACasesByCaseNum))
            {
                db.AddInParameter(cmd, "@CaseNumber", DbType.String, sCaseNumber);
                db.AddInParameter(cmd, "@PartyId", DbType.Int64, PartyId);
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);
                db.AddInParameter(cmd, "@CaseShipFromLocationId", DbType.Int32, CaseShipFromLocationId);                

                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstCases.Add(new Cases()
                        {
                            CaseId = reader.GetInt64("CaseId"),                            
                            CaseNumber = reader.GetString("CaseNumber"),
                            InventoryType = reader.GetString("InventoryType")
                        });
                    }

                }
                return lstCases;
            }
        }

        public List<Catalog> GetCatalogByKitNumber(string KitNumber)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<Catalog> lstCatalog = new List<Catalog>();
            Catalog newCatalog = new Catalog();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetCatalogByKitNumber))
            {
                db.AddInParameter(cmd, "@KitNumber", DbType.String, KitNumber);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newCatalog = Load(reader);
                        lstCatalog.Add(newCatalog);
                    }
                }
                return lstCatalog;
            }
        }

        public List<KitTable> GetKitTableByKitNumber(string kitNumber)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<KitTable> lstKitTable = new List<KitTable>();
            KitTable newKitTable = new KitTable();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetKitTableByKitNumber))
            {
                db.AddInParameter(cmd, "@KitNumber", DbType.String, kitNumber);
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

        public bool ModifyKitTable(KitTable kitTable, string ModificationType)
        {
            bool returnvalue = false;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_ModifyKitTable))
            {
                db.AddInParameter(cmd, "@KitNumber", DbType.String, kitTable.KitNumber);
                db.AddInParameter(cmd, "@ItemNumber", DbType.String, kitTable.ItemNumber);
                db.AddInParameter(cmd, "@CatalogNumber", DbType.String, kitTable.Catalognumber);
                db.AddInParameter(cmd, "@Description", DbType.String, kitTable.Description);
                db.AddInParameter(cmd, "@Qty", DbType.String, kitTable.Quantity);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);
                db.AddInParameter(cmd, "@ModificationType", DbType.String, ModificationType);
                db.ExecuteScalar(cmd);
                returnvalue = true;
            }
            return returnvalue;
        }
        

        private KitTable LoadKitTable(SafeDataReader reader)
        {
            KitTable newkit = new KitTable();
            newkit.KitNumber = reader.GetString("KitNumber");
            newkit.ItemNumber = reader.GetNullableInt32("ItemNumber");
            newkit.Catalognumber = reader.GetString("Catalognumber");
            newkit.Description = reader.GetString("Description");
            newkit.CaseLotCode = reader.GetString("CaseLotCode");
            newkit.Sent = reader.GetNullableInt32("Sent");
            newkit.Creturn = reader.GetNullableInt32("Creturn");
            newkit.Group = reader.GetNullableInt32("Group");
            newkit.BOQty = reader.GetNullableInt32("BOQty");
            newkit.IsManuallyAdded = reader.GetBoolean("IsManuallyAdded");
            newkit.Quantity = reader.GetNullableInt32("Qty");

            return newkit;
        }

        private Catalog Load(SafeDataReader reader)
        {
            Catalog newCatalog = new Catalog();

            newCatalog.CatalogNumber = reader.GetString("CatalogNumber");
            newCatalog.Description = reader.GetString("Description");
            newCatalog.CatalogFull = reader.GetString("CatalogFull");

            return newCatalog;
        }
    }
}
