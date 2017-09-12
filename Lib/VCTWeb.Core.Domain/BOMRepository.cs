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
    public class BOMRepository
    {
        private string _user;

        public BOMRepository()
        {
        }

        public BOMRepository(string user)
        {
            _user = user;
        }

        public void PublishBOM(long bomId)
        {
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_PUBLISH_BOM))
            {
                db.AddInParameter(cmd, "@BOMId", DbType.Int64, bomId);
                db.ExecuteNonQuery(cmd);
            }

        }

        public bool CheckDuplicateBOM(string kitNumber, long bomId)
        {
            Database db = DbHelper.CreateDatabase();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GET_DUPLICATE_BOM))
            {
                db.AddInParameter(cmd, "KitNumber", DbType.String, kitNumber.Trim());
                db.AddInParameter(cmd, "BOMId", DbType.Int64, bomId);
                int count = Convert.ToInt32(db.ExecuteScalar(cmd), CultureInfo.InvariantCulture);

                if (count > 0)
                    return true;
            }

            return false;
        }

        public List<Catalog> FetchAllCatalogNumbers()
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<Catalog> lstCatalog = new List<Catalog>();
            Catalog newCatalog = new Catalog();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETLISTOFCATALOGNUMBERS))
            {
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newCatalog = LoadCatalog(reader);
                        lstCatalog.Add(newCatalog);
                    }

                }
                return lstCatalog;
            }
        }

        public List<Catalog> FetchCatalogNumbersByBOMId(long bomId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<Catalog> lstCatalog = new List<Catalog>();
            Catalog newCatalog = new Catalog();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETLISTOFCATALOGNUMBERSBYBOMID))
            {
                db.AddInParameter(cmd, "@BOMId", DbType.Int64, bomId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newCatalog = LoadCatalog(reader);
                        lstCatalog.Add(newCatalog);
                    }

                }
                return lstCatalog;
            }
        }

        private Catalog LoadCatalog(SafeDataReader reader)
        {
            Catalog newCatalog = new Catalog();

            newCatalog.CatalogNumber = reader.GetString("CatalogNumber");
            newCatalog.Description = reader.GetString("Description");

            return newCatalog;
        }

        public List<TrayType> FetchAllTrayTypes()
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<TrayType> lstTrayType = new List<TrayType>();
            TrayType newTrayType = new TrayType();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETLISTOFTRAYTYPES))
            {
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newTrayType = LoadTrayType(reader);
                        lstTrayType.Add(newTrayType);
                    }

                }
                return lstTrayType;
            }
        }

        private TrayType LoadTrayType(SafeDataReader reader)
        {
            TrayType newTrayType = new TrayType();

            newTrayType.TrayTypeName = reader.GetString("TrayTypeName");

            return newTrayType;
        }

        public List<BOM> FetchAllPublishedBOMs()
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<BOM> lstBOM = new List<BOM>();
            BOM newBOM = new BOM();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETLISTOFPUBLISHEDBOMS))
            {
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newBOM = LoadBOM(reader);
                        lstBOM.Add(newBOM);
                    }

                }
                return lstBOM;
            }
        }

        public List<BOM> FetchAllUnPublishedBOMs()
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<BOM> lstBOM = new List<BOM>();
            BOM newBOM = new BOM();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETLISTOFUNPUBLISHEDBOMS))
            {
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newBOM = LoadBOM(reader);
                        lstBOM.Add(newBOM);
                    }

                }
                return lstBOM;
            }
        }

        public BOM FetchBOMById(long BOMId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();            
            BOM newBOM = null;
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetBOMByBOMId))
            {
                db.AddInParameter(cmd, "@BOMId", DbType.Int64, BOMId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newBOM = LoadBOM(reader);                        
                    }
                }
                return newBOM;
            }
        }

        private BOM LoadBOM(SafeDataReader reader)
        {
            BOM newBOM = new BOM();

            newBOM.BOMId = reader.GetInt64("BOMId");
            newBOM.Description = reader.GetString("Description");
            newBOM.ProcedureName = reader.GetString("ProcedureName");
            newBOM.KitNumber = reader.GetString("KitNumber");
            newBOM.KitName = reader.GetString("KitName");
            newBOM.TrayTypeName = reader.GetString("TrayTypeName");
            newBOM.PublishedOn = reader.GetLocalDateTime("PublishedOn");
            newBOM.ValidTill = reader.GetNullableDateTime("ValidTill");

            return newBOM;
        }

        public long SaveBOM(BOM bom, string catalogXmlString)
        {
            long bomId = 0;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_SAVE_BOM))
            {
                db.AddInParameter(cmd, "@BOMId", DbType.Int64, bom.BOMId);
                db.AddInParameter(cmd, "@Description", DbType.String, bom.Description);
                db.AddInParameter(cmd, "@ProcedureName", DbType.String, bom.ProcedureName);
                db.AddInParameter(cmd, "@KitNumber", DbType.String, bom.KitNumber);
                db.AddInParameter(cmd, "@KitName", DbType.String, bom.KitName);
                db.AddInParameter(cmd, "@TrayTypeName", DbType.String, bom.TrayTypeName);
                db.AddInParameter(cmd, "@ValidTill", DbType.Date, bom.ValidTill);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);
                db.AddInParameter(cmd, "@CatalogXmlString", DbType.String, catalogXmlString);
                bomId = Convert.ToInt64(db.ExecuteScalar(cmd), CultureInfo.InvariantCulture);
            }
            return bomId;
        }
    }
}
