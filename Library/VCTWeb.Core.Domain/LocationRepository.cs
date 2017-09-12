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
    public class LocationRepository
    {
        private string _user;

        public LocationRepository()
        {
        }

        public LocationRepository(string user)
        {
            _user = user;
        }

        public Location GetLocationByLocationId(int locationId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            Location newLocation = new Location();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETLOCATIONBYLOCATIONID))
            {
                db.AddInParameter(cmd, "LocationId", DbType.Int32, locationId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    if (reader.Read())
                    {
                        newLocation = LoadLocation(reader);
                    }

                }
                return newLocation;
            }
        }

        public List<Location> GetLocationByParentLocationId(int parentLocationId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<Location> lstLocation = new List<Location>();
            Location newLocation = new Location();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetLocationByParentLocationId))
            {
                db.AddInParameter(cmd, "ParentLocationId", DbType.Int32, parentLocationId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newLocation = LoadLocation(reader);
                        lstLocation.Add(newLocation);
                    }

                }
                return lstLocation.OrderBy(x=>x.LocationName).ToList();
            }
        }

        private Location LoadLocation(SafeDataReader reader)
        {
            Location newLocation = new Location();

            newLocation.LocationId = reader.GetInt32("LocationId");
            newLocation.Code = reader.GetString("Code");
            newLocation.AddressId = reader.GetNullableInt32("AddressId");
            newLocation.LocationTypeId = reader.GetInt64("LocationTypeId");
            newLocation.ParentLocationId = reader.GetNullableInt32("ParentLocationId");
            newLocation.LocationName = reader.GetString("LocationName");
            newLocation.LocationTypeLocationName = reader.GetString("LocationTypeLocationName");
            newLocation.Description = reader.GetString("Description");
            newLocation.Longitude = reader.GetDecimal("Longitude");
            newLocation.Latitude = reader.GetDecimal("Latitude");
            newLocation.GLN = reader.GetString("GLN");
            newLocation.IsActive = reader.GetBoolean("IsActive");
            newLocation.LocationType = reader.GetString("LocationType");
            newLocation.RequiresAddress = reader.GetBoolean("RequiresAddress");
            newLocation.UpdatedBy = reader.GetString("UpdatedBy");
            newLocation.UpdatedOn = reader.GetLocalDateTime("UpdatedOn");

            return newLocation;
        }


        public List<LocationPartDetail> GetNearExpiryItemsByLocationId(int locationId, int parentLocationId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<LocationPartDetail> lstLocationPartDetail = new List<LocationPartDetail>();
            LocationPartDetail newLocationPartDetail = new LocationPartDetail();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetNearExpiryItemsByLocationId))
            {
                db.AddInParameter(cmd, "LocationId", DbType.Int32, locationId);
                db.AddInParameter(cmd, "ParentLocationId", DbType.Int32, parentLocationId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newLocationPartDetail = LoadLocationPartDetail(reader);
                        lstLocationPartDetail.Add(newLocationPartDetail);
                    }

                }
                return lstLocationPartDetail;
            }
        }

        private LocationPartDetail LoadLocationPartDetail(SafeDataReader reader)
        {
            LocationPartDetail newLocationPartDetail = new LocationPartDetail();

            newLocationPartDetail.LocationPartDetailId = reader.GetInt64("LocationPartDetailId");
            newLocationPartDetail.LocationId = reader.GetInt32("LocationId");
            newLocationPartDetail.PartNum = reader.GetString("PartNum");
            newLocationPartDetail.Description = reader.GetString("Description");
            newLocationPartDetail.LotNum = reader.GetString("LotNum");
            newLocationPartDetail.ExpiryDate = reader.GetDateTime("ExpiryDate");
            newLocationPartDetail.PartStatus = reader.GetString("PartStatus");
            newLocationPartDetail.LocationName = reader.GetString("LocationName");
            newLocationPartDetail.LocationType = reader.GetString("LocationType");

            return newLocationPartDetail;
        }

        public LocationType FetchLocationTypeForParty()
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            LocationType newLocationType = new LocationType();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETLOCATIONTYPEFORPARTY))
            {
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    if (reader.Read())
                    {
                        newLocationType = Load(reader);
                    }

                }
                return newLocationType;
            }
        }

        public LocationType FetchLocationTypeForBranch()
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            LocationType newLocationType = new LocationType();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETLOCATIONTYPEFORBRANCH))
            {
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    if (reader.Read())
                    {
                        newLocationType = Load(reader);
                    }

                }
                return newLocationType;
            }
        }

        public LocationType FetchLocationTypeForSatellite()
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            LocationType newLocationType = new LocationType();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETLOCATIONTYPEFORSATELLITE))
            {
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    if (reader.Read())
                    {
                        newLocationType = Load(reader);
                    }

                }
                return newLocationType;
            }
        }

        public Address GetLocationAddress(int locationId)
        {
            SafeDataReader reader = null;
            Address address = new Address();
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GET_LOCATION_ADDRESS))
            {
                db.AddInParameter(cmd, "LocationId", DbType.Int32, locationId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        address.AddressId = reader.GetInt32("AddressId");
                        address.Line1 = reader.GetString("Line1").ToString().Trim();
                        address.Line2 = reader.GetString("Line2").ToString().Trim();
                        address.City = reader.GetString("City").ToString().Trim();
                        address.Country = reader.GetString("Country").ToString().Trim();
                        address.Region = reader.GetString("Region").ToString().Trim();
                        address.State = reader.GetString("State").ToString().Trim();
                        address.Zip = reader.GetString("Zip").ToString().Trim();
                    }
                }
                return address;
            }
        }

        public int SavePartyLocationAndAddress(PartyLocation partyLocation, bool requiresAddress, Address address)
        {
            int locationId = 0;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_SAVE_LOCATION_AND_ADDRESS))
            {
                db.AddInParameter(cmd, "@ParentLocationId", DbType.Int64, partyLocation.ParentLocationId);
                db.AddInParameter(cmd, "@LocationTypeId", DbType.Int64, partyLocation.LocationTypeId);
                db.AddInParameter(cmd, "@PartyId", DbType.Int64, partyLocation.PartyId);
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, partyLocation.LocationId);
                db.AddInParameter(cmd, "@LocationName", DbType.String, partyLocation.LocationName.Trim());
                db.AddInParameter(cmd, "@Description", DbType.String, partyLocation.Description.Trim());
                db.AddInParameter(cmd, "@Latitude", DbType.String, partyLocation.Latitude);
                db.AddInParameter(cmd, "@Longitude", DbType.String, partyLocation.Longitude);
                db.AddInParameter(cmd, "@Code", DbType.String, partyLocation.Code.Trim());
                db.AddInParameter(cmd, "@GLN", DbType.String, partyLocation.GLN.Trim());
                db.AddInParameter(cmd, "@RequiresAddress", DbType.Boolean, requiresAddress);
                db.AddInParameter(cmd, "@IsActive", DbType.Boolean, partyLocation.IsActive);
                db.AddInParameter(cmd, "@AddressId", DbType.Int32, address.AddressId);
                db.AddInParameter(cmd, "@Line1", DbType.String, address.Line1);
                db.AddInParameter(cmd, "@Line2", DbType.String, address.Line2);
                db.AddInParameter(cmd, "@City", DbType.String, address.City);
                db.AddInParameter(cmd, "@Country", DbType.String, address.Country);
                db.AddInParameter(cmd, "@State", DbType.String, address.State);
                //db.AddInParameter(cmd, "@Region", DbType.String, address.Region);
                db.AddInParameter(cmd, "@Zip", DbType.String, address.Zip);

                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);
                db.AddInParameter(cmd, "@LocationType", DbType.String, partyLocation.LocationType);
                locationId = Convert.ToInt32(db.ExecuteScalar(cmd), CultureInfo.InvariantCulture);
            }
            return locationId;
        }

        public int SaveLocationAndAddress(Location Location, bool requiresAddress, Address address)
        {
            int locationId = 0;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_SAVE_LOCATION_AND_ADDRESS))
            {
                db.AddInParameter(cmd, "@ParentLocationId", DbType.Int64, Location.ParentLocationId);
                db.AddInParameter(cmd, "@LocationTypeId", DbType.Int64, Location.LocationTypeId);
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, Location.LocationId);
                db.AddInParameter(cmd, "@LocationName", DbType.String, Location.LocationName.Trim());
                db.AddInParameter(cmd, "@Description", DbType.String, Location.Description.Trim());
                db.AddInParameter(cmd, "@Latitude", DbType.String, Location.Latitude);
                db.AddInParameter(cmd, "@Longitude", DbType.String, Location.Longitude);
                db.AddInParameter(cmd, "@Code", DbType.String, Location.Code.Trim());
                db.AddInParameter(cmd, "@GLN", DbType.String, Location.GLN.Trim());
                db.AddInParameter(cmd, "@RequiresAddress", DbType.Boolean, requiresAddress);
                db.AddInParameter(cmd, "@IsActive", DbType.Boolean, Location.IsActive);
                db.AddInParameter(cmd, "@AddressId", DbType.Int32, address.AddressId);
                db.AddInParameter(cmd, "@Line1", DbType.String, address.Line1);
                db.AddInParameter(cmd, "@Line2", DbType.String, address.Line2);
                db.AddInParameter(cmd, "@City", DbType.String, address.City);
                db.AddInParameter(cmd, "@Country", DbType.String, address.Country);
                db.AddInParameter(cmd, "@State", DbType.String, address.State);
                //db.AddInParameter(cmd, "@Region", DbType.String, address.Region);
                db.AddInParameter(cmd, "@Zip", DbType.String, address.Zip);

                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);
                db.AddInParameter(cmd, "@LocationType", DbType.String, Location.LocationType);
                locationId = Convert.ToInt32(db.ExecuteScalar(cmd), CultureInfo.InvariantCulture);
            }
            return locationId;
        }

        public void SaveLocationAddress(Address address, Int32 locationId)
        {
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_SAVE_LOCATION_ADDRESS))
            {

                db.AddInParameter(cmd, "@AddressId", DbType.Int32, address.AddressId);
                db.AddInParameter(cmd, "@Line1", DbType.String, address.Line1.Trim());
                db.AddInParameter(cmd, "@Line2", DbType.String, address.Line2.Trim());
                db.AddInParameter(cmd, "@City", DbType.String, address.City.Trim());
                db.AddInParameter(cmd, "@Country", DbType.String, address.Country.Trim());
                db.AddInParameter(cmd, "@State", DbType.String, address.State.Trim());
                db.AddInParameter(cmd, "@Region", DbType.String, address.Region.Trim());
                db.AddInParameter(cmd, "@Zip", DbType.String, address.Zip.Trim());
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);
                db.ExecuteNonQuery(cmd);
            }
        }

        public void DeleteLocation(int locationid)
        {
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_DELETE_LOCATION))
            {
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, locationid);
                db.ExecuteNonQuery(cmd);
            }

        }

        public void DeleteDeleteItemFromLocationPartDetail(long locationPartDetailId)
        {
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_DeleteItemFromLocationPartDetail))
            {
                db.AddInParameter(cmd, "@LocationPartDetailId", DbType.Int64, locationPartDetailId);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);
                db.ExecuteNonQuery(cmd);
            }

        }

        private LocationType Load(SafeDataReader reader)
        {
            LocationType newLocationType = new LocationType();

            newLocationType.LocationTypeId = reader.GetInt64("LocationTypeId");
            newLocationType.ParentId = reader.GetNullableInt64("ParentId");
            newLocationType.Name = reader.GetString("Name");
            newLocationType.Code = reader.GetString("Code");
            newLocationType.Description = reader.GetString("Description");
            newLocationType.RequiresAddress = reader.GetBoolean("RequiresAddress");
            newLocationType.IsActive = reader.GetBoolean("IsActive");
            newLocationType.IsEditable = reader.GetBoolean("IsEditable");
            newLocationType.UpdatedBy = reader.GetString("UpdatedBy");
            newLocationType.UpdatedOn = reader.GetLocalDateTime("UpdatedOn");
            
            return newLocationType;
        }

        public bool CheckDuplicateLocationName(string locationName, int locationId)
        {
            Database db = DbHelper.CreateDatabase();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GET_DUPLICATE_LOCATIONCODE))
            {
                db.AddInParameter(cmd, "LocationName", DbType.String, locationName.Trim());
                db.AddInParameter(cmd, "LocationID", DbType.Int32, locationId);
                int count = Convert.ToInt32(db.ExecuteScalar(cmd), CultureInfo.InvariantCulture);

                if (count > 0)
                    return true;
            }

            return false;
        }

        public List<Location> FetchAllLocations()
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<Location> lstLocation = new List<Location>();
            Location newLocation = new Location();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetListOfAllLocations))
            {
                //db.AddInParameter(cmd, "LocationTypeId", DbType.Int64, locationTypeId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newLocation = LoadLocation(reader);
                        lstLocation.Add(newLocation);
                    }

                }
                return lstLocation;
            }
        }

        public bool CheckLocationInUse(int locationId)
        {
            Database db = DbHelper.CreateDatabase();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_CheckInUseLocation))
            {
                db.AddInParameter(cmd, "@LocationId", DbType.Int64, locationId);
                int count = Convert.ToInt32(db.ExecuteScalar(cmd), CultureInfo.InvariantCulture);

                if (count > 0)
                    return true;
            }

            return false;
        }
    }
}
