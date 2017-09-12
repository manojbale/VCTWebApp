using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Globalization;
using System.Web;

namespace VCTWeb.Core.Domain
{
    public class PARLevelRepository
    {
        private string _user;

        public PARLevelRepository()
        {
        }

        public PARLevelRepository(string user)
        {
            _user = user;
        }

        public List<LocationPARLevel> GetLocationPARLevelByLocationId(int locationId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<LocationPARLevel> lstLocationPARLevel = new List<LocationPARLevel>();
            LocationPARLevel newLocationPARLevel = new LocationPARLevel();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetLocationPARLevelByLocationId))
            {
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newLocationPARLevel = LoadLoctionPARLevel(reader);
                        lstLocationPARLevel.Add(newLocationPARLevel);
                    }

                }
                return lstLocationPARLevel;
            }
        }

        private LocationPARLevel LoadLoctionPARLevel(SafeDataReader reader)
        {
            LocationPARLevel newLocationPARLevel = new LocationPARLevel();

            newLocationPARLevel.PARLevelId = reader.GetInt64("PARLevelId");
            newLocationPARLevel.LocationId = reader.GetInt32("LocationId");
            newLocationPARLevel.PartNum = reader.GetString("PartNum");
            newLocationPARLevel.Description = reader.GetString("Description");
            newLocationPARLevel.PARLevelQty = reader.GetInt32("PARLevelQty");
            newLocationPARLevel.UpdatedBy = reader.GetString("UpdatedBy");
            newLocationPARLevel.UpdatedOn = reader.GetDateTime("UpdatedOn");

            return newLocationPARLevel;
        }

        public List<PartyPARLevel> GetPartyPARLevelByPartyId(long partyId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<PartyPARLevel> lstPartyPARLevel = new List<PartyPARLevel>();
            PartyPARLevel newPartyPARLevel = new PartyPARLevel();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetPartyPARLevelByPartyId))
            {
                db.AddInParameter(cmd, "@PartyId", DbType.Int64, partyId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newPartyPARLevel = LoadPartyPARLevel(reader);
                        lstPartyPARLevel.Add(newPartyPARLevel);
                    }

                }
                return lstPartyPARLevel;
            }
        }

        private PartyPARLevel LoadPartyPARLevel(SafeDataReader reader)
        {
            PartyPARLevel newPartyPARLevel = new PartyPARLevel();

            newPartyPARLevel.PARLevelId = reader.GetInt64("PARLevelId");
            newPartyPARLevel.PartyId = reader.GetInt64("PartyId");
            newPartyPARLevel.PartNum = reader.GetString("PartNum");
            newPartyPARLevel.Description = reader.GetString("Description");
            newPartyPARLevel.PARLevelQty = reader.GetInt32("PARLevelQty");
            newPartyPARLevel.UpdatedBy = reader.GetString("UpdatedBy");
            newPartyPARLevel.UpdatedOn = reader.GetDateTime("UpdatedOn");

            return newPartyPARLevel;
        }

        public bool SavePartyPARLevel(PartyPARLevel ppl)
        {
            bool returnvalue = false;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_SavePartyPARLevel))
            {
                db.AddInParameter(cmd, "@PARLevelId", DbType.Int64, ppl.PARLevelId);
                db.AddInParameter(cmd, "@PartyId", DbType.Int64, ppl.PartyId);
                db.AddInParameter(cmd, "@PartNum", DbType.String, ppl.PartNum);
                db.AddInParameter(cmd, "@PARLevelQty", DbType.Int32, ppl.PARLevelQty);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);
                db.ExecuteScalar(cmd);
                returnvalue = true;
            }
            return returnvalue;
        }

        public bool SaveLocationPARLevel(LocationPARLevel lpl)
        {
            bool returnvalue = false;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_SaveLocationPARLevel))
            {
                db.AddInParameter(cmd, "@PARLevelId", DbType.Int64, lpl.PARLevelId);
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, lpl.LocationId);
                db.AddInParameter(cmd, "@PartNum", DbType.String, lpl.PartNum);
                db.AddInParameter(cmd, "@PARLevelQty", DbType.Int32, lpl.PARLevelQty);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);
                db.ExecuteScalar(cmd);
                returnvalue = true;
            }
            return returnvalue;
        }

        public bool DeletePartyPARLevel(long parLevelId)
        {
            bool returnvalue = false;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_DeletePartyPARLevel))
            {
                db.AddInParameter(cmd, "@PARLevelId", DbType.Int64, parLevelId);
                db.ExecuteScalar(cmd);
                returnvalue = true;
            }
            return returnvalue;
        }

        public bool DeleteLocationPARLevel(long parLevelId)
        {
            bool returnvalue = false;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_DeleteLocationPARLevel))
            {
                db.AddInParameter(cmd, "@PARLevelId", DbType.Int64, parLevelId);
                db.ExecuteScalar(cmd);
                returnvalue = true;
            }
            return returnvalue;
        }

        public List<ReplenishmentTransfer> GetReplenishmentTransferByPartyId(long partyId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<ReplenishmentTransfer> lstReplenishmentTransfer = new List<ReplenishmentTransfer>();
            ReplenishmentTransfer newReplenishmentTransfer = new ReplenishmentTransfer();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetReplenishmentTransferByPartyId))
            {
                db.AddInParameter(cmd, "@PartyId", DbType.Int64, partyId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newReplenishmentTransfer = LoadReplenishmentTransfer(reader);
                        lstReplenishmentTransfer.Add(newReplenishmentTransfer);
                    }

                }
                return lstReplenishmentTransfer;
            }
        }

        private ReplenishmentTransfer LoadReplenishmentTransfer(SafeDataReader reader)
        {
            ReplenishmentTransfer newReplenishmentTransfer = new ReplenishmentTransfer();

            newReplenishmentTransfer.PartNum = reader.GetString("PartNum");
            newReplenishmentTransfer.Description = reader.GetString("Description");
            newReplenishmentTransfer.PARLevelQty = reader.GetInt32("PARLevelQty");
            newReplenishmentTransfer.AvailableQty = reader.GetInt32("AvailableQty");
            newReplenishmentTransfer.ReplenishQty = reader.GetInt32("ReplenishQty");

            return newReplenishmentTransfer;
        }

        public List<ReplenishmentTransfer> GetReplenishmentTransferByLocationId(int locationId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<ReplenishmentTransfer> lstReplenishmentTransfer = new List<ReplenishmentTransfer>();
            ReplenishmentTransfer newReplenishmentTransfer = new ReplenishmentTransfer();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetReplenishmentTransferByLocationId))
            {
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newReplenishmentTransfer = LoadReplenishmentTransfer(reader);
                        lstReplenishmentTransfer.Add(newReplenishmentTransfer);
                    }

                }
                return lstReplenishmentTransfer;
            }
        }

        public bool SavePartyReplenishmentTransfer(long partyId, DateTime surgeryDate, string itemDetailXmlString, int locationId, out string result)
        {
            bool returnvalue = false;
            result = "";
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_SavePartyReplenishmentTransfer))
            {
                db.AddInParameter(cmd, "@Partyid", DbType.Int64, partyId);
                db.AddInParameter(cmd, "@SurgeryDate", DbType.Date, surgeryDate);
                db.AddInParameter(cmd, "@ItemDetailXmlString", DbType.String, itemDetailXmlString);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                result = Convert.ToString(db.ExecuteScalar(cmd));
                returnvalue = true;
            }
            return returnvalue;
        }

        public bool SaveLocationReplenishmentTransfer(int toLocationId, DateTime surgeryDate, string itemDetailXmlString, int locationId, out string result)
        {
            bool returnvalue = false;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_SaveLocationReplenishmentTransfer))
            {
                db.AddInParameter(cmd, "@ToLocationId", DbType.Int32, toLocationId);
                db.AddInParameter(cmd, "@SurgeryDate", DbType.Date, surgeryDate);
                db.AddInParameter(cmd, "@ItemDetailXmlString", DbType.String, itemDetailXmlString);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                result = Convert.ToString(db.ExecuteScalar(cmd));
                returnvalue = true;
            }
            return returnvalue;
        }
    }
}
