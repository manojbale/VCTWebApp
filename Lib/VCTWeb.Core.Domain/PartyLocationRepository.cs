using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace VCTWeb.Core.Domain
{
    public class PartyLocationRepository
    {
        private string _user;

        public PartyLocationRepository()
        {
        }

        public PartyLocationRepository(string user)
        {
            _user = user;
        }

        //public List<PartyLocation> FetchAllPartyLocations()
        //{
        //    SafeDataReader reader = null;
        //    Database db = DbHelper.CreateDatabase();
        //    List<PartyLocation> lstPartyLocation = new List<PartyLocation>();
        //    PartyLocation newPartyLocation = new PartyLocation();
        //    using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETLISTOFPARTYLOCATIONS))
        //    {
        //        using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
        //        {
        //            while (reader.Read())
        //            {
        //                newPartyLocation = LoadPartyLocation(reader);
        //                lstPartyLocation.Add(newPartyLocation);
        //            }

        //        }
        //        return lstPartyLocation;
        //    }
        //}

        public PartyLocation FetchPartyLocationByPartyId(long partyId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            PartyLocation newPartyLocation = null;
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETPARTYLOCATIONBYPARTYID))
            {
                db.AddInParameter(cmd, "PartyId", DbType.Int64, partyId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    if (reader.Read())
                    {
                        newPartyLocation = LoadPartyLocation(reader);
                    }

                }
                return newPartyLocation;
            }
        }

        private PartyLocation LoadPartyLocation(SafeDataReader reader)
        {
            PartyLocation newPartyLocation = new PartyLocation();

            newPartyLocation.PartyId = reader.GetInt64("PartyId");
            newPartyLocation.PartyName = reader.GetString("PartyName");
            newPartyLocation.RequiresAddress = reader.GetBoolean("RequiresAddress");
            newPartyLocation.LocationType = reader.GetString("LocationType");

            newPartyLocation.LocationId = reader.GetInt32("LocationId");
            newPartyLocation.Code = reader.GetString("Code");
            newPartyLocation.AddressId = reader.GetNullableInt32("AddressId");
            newPartyLocation.LocationTypeId = reader.GetInt64("LocationTypeId");
            newPartyLocation.ParentLocationId = reader.GetNullableInt32("ParentLocationId");
            newPartyLocation.LocationName = reader.GetString("LocationName");
            newPartyLocation.Description = reader.GetString("Description");
            newPartyLocation.Longitude = reader.GetDecimal("Longitude");
            newPartyLocation.Latitude = reader.GetDecimal("Latitude");
            newPartyLocation.GLN = reader.GetString("GLN");
            newPartyLocation.IsActive = reader.GetBoolean("IsActive");
            newPartyLocation.UpdatedBy = reader.GetString("UpdatedBy");
            newPartyLocation.UpdatedOn = reader.GetLocalDateTime("UpdatedOn");

            return newPartyLocation;
        }

        //public PartyLocation GetPartyLocationByLocationId(int locationId)
        //{
        //    SafeDataReader reader = null;
        //    Database db = DbHelper.CreateDatabase();
        //    PartyLocation newPartyLocation = new PartyLocation();
        //    using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETPARTYLOCATIONBYLOCATIONID))
        //    {
        //        db.AddInParameter(cmd, "LocationId", DbType.Int32, locationId);
        //        using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
        //        {
        //            if (reader.Read())
        //            {
        //                newPartyLocation = LoadPartyLocation(reader);
        //            }

        //        }
        //        return newPartyLocation;
        //    }
        //}
    }
}
