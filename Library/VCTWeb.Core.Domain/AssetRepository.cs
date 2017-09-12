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
    public class AssetRepository
    {
        private string _user;

        public AssetRepository()
        {
        }

        public AssetRepository(string user)
        {
            _user = user;
        }

        #region Public Methods

        /// <summary>
        /// Gets the Lot Number List.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>list of Lot Number object</returns>
        public List<VirtualBuilKit> GetLotNumbersToAssigned(string KitNumber, string PartNumber, int LocationId)
        {
            SafeDataReader reader = null;
            List<VirtualBuilKit> lstLotNumber = null;
            VirtualBuilKit oModel = null;

            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETLOTNUMBERSTOASSIGNED))
            {

                db.AddInParameter(cmd, "@KitNumber", DbType.String, KitNumber);
                db.AddInParameter(cmd, "@PartNumber", DbType.String, PartNumber);
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);

                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    lstLotNumber = new List<VirtualBuilKit>();
                    while (reader.Read())
                    {
                        oModel = new VirtualBuilKit();
                        oModel.LocationPartDetailId = reader.GetInt64("LocationPartDetailId");
                        oModel.LotNum = reader.GetString("LotNum");
                        lstLotNumber.Add(oModel);
                    }
                }
            }

            return lstLotNumber;
        }

        public string GetKitNumberDescByKitNumber(string KitNumber)
        {
            SafeDataReader reader = null;
            string KitNumberDesc = null;

            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETKITNUMBERDESCBYKITNUMBER))
            {
                db.AddInParameter(cmd, "@KitNumber", DbType.String, KitNumber);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    if (reader.Read())
                    {
                        KitNumberDesc = reader.GetString("KitDescription");
                    }
                }
            }

            return KitNumberDesc;
        }

        public List<VirtualBuilKit> GetSelectBuildKitByKitNumber(string KitNumber, Int32 LocationId)
        {
            SafeDataReader reader = null;
            List<VirtualBuilKit> lstSelectedBuildKit = null;
            VirtualBuilKit oModel = null;

            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetSelectBuildKitByKitNumber))
            {
                db.AddInParameter(cmd, "@KitNumber", DbType.String, KitNumber);
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    lstSelectedBuildKit = new List<VirtualBuilKit>();
                    while (reader.Read())
                    {
                        oModel = new VirtualBuilKit();
                        oModel.LocationPartDetailId = reader.GetInt64("LocationPartDetailId");
                        oModel.LotNum = reader.GetString("LotNum");
                        oModel.PartNum = reader.GetString("PartNum");
                        lstSelectedBuildKit.Add(oModel);
                    }
                }
            }

            return lstSelectedBuildKit;
        }

        public List<PendingBuildKit> GetListOfPendingBuildKit(Int32 LocationId)
        {
            SafeDataReader reader = null;
            List<PendingBuildKit> lstPendingBuildKit = new List<PendingBuildKit>();

            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetListOfPendingBuildKit))
            {
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstPendingBuildKit.Add(new PendingBuildKit()
                        {
                            KitNumber = reader.GetString("KitNumber"),
                            KitDescription = reader.GetString("KitDescription"),
                            KitFamily = reader.GetString("KitFamily"),
                            LastBuildOn = reader.GetDateTime("LastBuildOn")
                        });
                    }
                }
            }

            return lstPendingBuildKit;
        }

        /// <summary>
        /// Gets the Virtual build Kit List.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>list of VirtualBuildKit object</returns>
        public List<VirtualBuilKit> GetVirtualBuilKitList(string KitNumber, Int32 LocationId)
        {
            SafeDataReader reader = null;
            VirtualBuilKit oModel = null;
            List<VirtualBuilKit> lstVirtualBuildKit = null;

            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetVirtualBuildKitByKitNumber))
            {
                db.AddInParameter(cmd, "@KitNumber", DbType.String, KitNumber);
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);

                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    lstVirtualBuildKit = new List<VirtualBuilKit>();
                    while (reader.Read())
                    {
                        oModel = this.LoadVirtualBuildKit(reader);
                        lstVirtualBuildKit.Add(oModel);
                    }
                }
            }

            return lstVirtualBuildKit;
        }

        /// <summary>
        /// Save Virtual Build Kit
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>void</returns>
        public bool SaveVirtualBuildKit(VirtualBuilKit oModel, string VirtualBuildKitTableXML)
        {
            bool result = false;    
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_SAVEVIRTUALBUILDKIT))
            {
                db.AddInParameter(cmd, "@KitFamilyId", DbType.String, oModel.KitFamilyId);
                db.AddInParameter(cmd, "@KitNumber", DbType.String, oModel.KitNum);
                //db.AddInParameter(cmd, "@KitDesc", DbType.String, oModel.Description);
                db.AddInParameter(cmd, "@LocationId", DbType.String, oModel.LocationId);
                db.AddInParameter(cmd, "@VirtualBuildKitTableXML", DbType.String, VirtualBuildKitTableXML);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);
                result = Convert.ToBoolean(db.ExecuteScalar(cmd));
            }

            return result;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads the application configuration.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Dictionary object</returns>
        private VirtualBuilKit LoadVirtualBuildKit(SafeDataReader reader)
        {
            VirtualBuilKit oModel = new VirtualBuilKit();

            oModel.KitFamilyId = reader.GetInt64("KitFamilyId");
            oModel.PartNum = reader.GetString("CatalogNumber");
            oModel.Description = reader.GetString("Description");

            return oModel;
        }
        #endregion

    }
}
