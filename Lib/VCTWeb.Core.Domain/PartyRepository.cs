using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using System.Globalization;
using System.Web;

namespace VCTWeb.Core.Domain
{
    public class PartyRepository
    {
        private string _user;

        public PartyRepository()
        {
        }

        public PartyRepository(string user)
        {
            _user = user;
        }

        public List<Party> FetchParties(int locationId = 0)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<Party> lstParty = new List<Party>();
            Party newParty = new Party();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETLISTOFPARTIES))
            {
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newParty = LoadParty(reader);
                        lstParty.Add(newParty);
                    }

                }
                return lstParty;
            }
        }

        public List<PartyCycleCount> GetExptectedInventoryCountByPartyId(long partyId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<PartyCycleCount> lstPartyCycleCount = new List<PartyCycleCount>();
            PartyCycleCount newPartyCycleCount = new PartyCycleCount();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetExptectedInventoryCountByPartyId))
            {
                db.AddInParameter(cmd, "@PartyId", DbType.Int64, partyId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newPartyCycleCount = LoadPartyCycleCount(reader);
                        lstPartyCycleCount.Add(newPartyCycleCount);
                    }

                }
                return lstPartyCycleCount;
            }
        }

        public List<PartyCycleCount> GetPartyCycleCountByPartyId(long partyId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<PartyCycleCount> lstPartyCycleCount = new List<PartyCycleCount>();
            PartyCycleCount newPartyCycleCount = new PartyCycleCount();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetPartyCycleCountByPartyId))
            {
                db.AddInParameter(cmd, "@PartyId", DbType.Int64, partyId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newPartyCycleCount = LoadPartyCycleCount(reader);
                        lstPartyCycleCount.Add(newPartyCycleCount);
                    }

                }
                return lstPartyCycleCount;
            }
        }

        public List<PartyCycleCount> GetPartyCycleCountMatchByPartyId(long partyId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<PartyCycleCount> lstPartyCycleCount = new List<PartyCycleCount>();
            PartyCycleCount newPartyCycleCount = new PartyCycleCount();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetPartyCycleCountMatchByPartyId))
            {
                db.AddInParameter(cmd, "@PartyId", DbType.Int64, partyId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newPartyCycleCount = LoadPartyCycleCount(reader);
                        lstPartyCycleCount.Add(newPartyCycleCount);
                    }

                }
                return lstPartyCycleCount;
            }
        }

        private PartyCycleCount LoadPartyCycleCount(SafeDataReader reader)
        {
            PartyCycleCount newPartyCycleCount = new PartyCycleCount();

            newPartyCycleCount.PartyCycleCountId = reader.GetInt64("PartyCycleCountId");
            newPartyCycleCount.PartNum = reader.GetString("PartNum");
            newPartyCycleCount.LotNum = reader.GetString("LotNum");
            newPartyCycleCount.PartDescription = reader.GetString("PartDescription");
            newPartyCycleCount.CycleCountQty = reader.GetInt32("CycleCountQty");
            newPartyCycleCount.ExpectedQty = reader.GetInt32("ExpectedQty");
            newPartyCycleCount.CycleCountDate = string.Format(CultureInfo.CurrentCulture, reader.GetLocalDateTime("CycleCountDate").ToString("d"));
            newPartyCycleCount.Status = reader.GetString("Status");
            newPartyCycleCount.DispositionType = reader.GetString("DispositionType");

            return newPartyCycleCount;
        }

        public List<DispositionType> GetDispositionTypesByCategory(string dispositionCategory)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<DispositionType> lstDispositionType = new List<DispositionType>();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetDispositionTypesByCategory))
            {
                db.AddInParameter(cmd, "@DispositionCategory", DbType.String, dispositionCategory);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstDispositionType.Add(new DispositionType()
                            {
                                DispositionTypeId = reader.GetInt32("DispositionTypeId"),
                                Disposition = reader.GetString("DispositionType")
                            });
                    }
                }
                return lstDispositionType;
            }
        }

        public Party GetPartyById(long partyId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            Party newParty = null;
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetPartyById))
            {
                db.AddInParameter(cmd, "@PartyId", DbType.Int64, partyId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    if (reader.Read())
                    {
                        newParty = LoadParty(reader);
                    }

                }
                return newParty;
            }
        }

        public List<PartyType> FetchAllPartyTypes()
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<PartyType> lstPartyType = new List<PartyType>();
            PartyType newPartyType = new PartyType();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETLISTOFPARTYTYPES))
            {
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newPartyType = LoadPartyType(reader);
                        lstPartyType.Add(newPartyType);
                    }

                }
                return lstPartyType;
            }
        }

        public Address GetPartyAddress(long partyId)
        {
            SafeDataReader reader = null;
            Address address = new Address();
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GET_PARTY_ADDRESS))
            {
                db.AddInParameter(cmd, "partyId", DbType.Int64, partyId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        address.AddressId = reader.GetInt32("AddressId");
                        address.Longitude = reader.GetDecimal("Longitude");
                        address.Latitude = reader.GetDecimal("Latitude");
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

        public void SaveParty(Party party, Address address, string partyLocationIds)
        {
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_SAVEPARTY))
            {
                db.AddInParameter(cmd, "@PartyId", DbType.Int64, party.PartyId);
                db.AddInParameter(cmd, "@Name", DbType.String, party.Name);
                db.AddInParameter(cmd, "@Code", DbType.String, party.Code);
                db.AddInParameter(cmd, "@Description", DbType.String, party.Description);
                db.AddInParameter(cmd, "@PartyTypeId", DbType.Int64, party.PartyTypeId);
                //db.AddInParameter(cmd, "@LinkedLocationId", DbType.Int32, party.LinkedLocationId);
                db.AddInParameter(cmd, "@CompanyPrefix", DbType.String, party.CompanyPrefix);
                db.AddInParameter(cmd, "@ShippingDaysGap", DbType.Int32, party.ShippingDaysGap);
                db.AddInParameter(cmd, "@RetrievalDaysGap", DbType.Int32, party.RetrievalDaysGap);
                db.AddInParameter(cmd, "@IsActive", DbType.Boolean, party.IsActive);
                db.AddInParameter(cmd, "@Owner", DbType.Boolean, party.Owner);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);

                db.AddInParameter(cmd, "@Latitude", DbType.String, address.Latitude);
                db.AddInParameter(cmd, "@Longitude", DbType.String, address.Longitude);
                db.AddInParameter(cmd, "@AddressId", DbType.Int32, address.AddressId);
                db.AddInParameter(cmd, "@Line1", DbType.String, address.Line1);
                db.AddInParameter(cmd, "@Line2", DbType.String, address.Line2);
                db.AddInParameter(cmd, "@City", DbType.String, address.City);
                db.AddInParameter(cmd, "@Country", DbType.String, address.Country);
                db.AddInParameter(cmd, "@State", DbType.String, address.State);
                db.AddInParameter(cmd, "@Zip", DbType.String, address.Zip);

                db.AddInParameter(cmd, "@PartyLocationIds", DbType.String, partyLocationIds);
                db.ExecuteNonQuery(cmd);
            }
        }

        public void SaveInventoryCountReconcile(long partyId, string consumptionDetailXML, string reconcileDetailXML)
        {
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_SaveInventoryCountReconcile))
            {
                db.AddInParameter(cmd, "@PartyId", DbType.Int64, partyId);
                db.AddInParameter(cmd, "@ReconcileDetailXML", DbType.String, reconcileDetailXML);
                db.AddInParameter(cmd, "@ConsumptionDetailXML", DbType.String, consumptionDetailXML);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);
                db.ExecuteNonQuery(cmd);
            }
        }

        public List<Party> GetPartyByPartyName(string sPartyName)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<Party> lstParty = new List<Party>();
            Party newParty = new Party();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETPARTYBYPARTYNAME))
            {
                db.AddInParameter(cmd, "@Name", DbType.String, sPartyName);
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"]));
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newParty = LoadParty(reader);
                        lstParty.Add(newParty);
                    }

                }
                return lstParty;
            }
        }

        private Party LoadParty(SafeDataReader reader)
        {
            Party newParty = new Party();

            newParty.PartyId = reader.GetInt64("PartyId");
            newParty.Name = reader.GetString("Name");
            newParty.Code = reader.GetString("Code");
            newParty.Description = reader.GetString("Description");
            newParty.PartyTypeId = reader.GetInt64("PartyTypeId");
            //newParty.LinkedLocationId = reader.GetNullableInt32("LinkedLocationId");
            newParty.CompanyPrefix = reader.GetString("CompanyPrefix");
            newParty.ShippingDaysGap = reader.GetInt32("ShippingDaysGap");
            newParty.RetrievalDaysGap = reader.GetInt32("RetrievalDaysGap");
            newParty.IsActive = reader.GetBoolean("IsActive");
            newParty.UpdatedBy = reader.GetString("UpdatedBy");
            newParty.Owner = reader.GetBoolean("Owner");
            newParty.UpdatedOn = reader.GetLocalDateTime("UpdatedOn");

            return newParty;
        }

        private PartyType LoadPartyType(SafeDataReader reader)
        {
            PartyType newPartyType = new PartyType();

            newPartyType.PartyTypeId = reader.GetInt64("PartyTypeId");
            newPartyType.Name = reader.GetString("Name");
            newPartyType.IsActive = reader.GetBoolean("IsActive");
            newPartyType.UpdatedBy = reader.GetString("UpdatedBy");
            newPartyType.UpdatedOn = reader.GetLocalDateTime("UpdatedOn");

            return newPartyType;
        }

        public bool CheckInUse(long PartyId)
        {
            Database db = DbHelper.CreateDatabase();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_CheckInUseParty))
            {
                db.AddInParameter(cmd, "@PartyId", DbType.Int64, PartyId);
                int count = Convert.ToInt32(db.ExecuteScalar(cmd), CultureInfo.InvariantCulture);

                if (count > 0)
                    return true;
            }

            return false;
        }


        public bool IsPartyExists(string partyName)
        {
            Database db = DbHelper.CreateDatabase();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_CheckIsPartyExists))
            {
                db.AddInParameter(cmd, "@PartyName", DbType.String, partyName);
                int count = Convert.ToInt32(db.ExecuteScalar(cmd), CultureInfo.InvariantCulture);

                if (count > 0)
                    return true;
            }

            return false;
        }

        public List<PartyLinkedLocation> GetPartyLocationByPartyId(long partyId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<PartyLinkedLocation> lstPartyLinkedLocation = new List<PartyLinkedLocation>();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetPartyLocationByPartyId))
            {
                db.AddInParameter(cmd, "@PartyId", DbType.Int64, partyId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstPartyLinkedLocation.Add(new PartyLinkedLocation()
                        {
                            LocationId = reader.GetInt32("LocationId"),
                            LocationName = reader.GetString("LocationName"),
                            LocationType = reader.GetString("LocationType"),
                            Selected = reader.GetBoolean("Selected")
                        });
                    }

                }
                return lstPartyLinkedLocation;
            }
        }

        public List<RevenueProjection> GetRevenueProjectionByLocationId(int locationId, DateTime startDate, DateTime endDate)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<RevenueProjection> lstRevenueProjection = new List<RevenueProjection>();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetRevenueProjectionByLocationId))
            {
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                db.AddInParameter(cmd, "@StartDate", DbType.Date, startDate);
                db.AddInParameter(cmd, "@EndDate", DbType.Date, endDate);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstRevenueProjection.Add(new RevenueProjection()
                        {
                            ParentLocationId = reader.GetInt32("ParentLocationId"),
                            ParentLocationName = reader.GetString("ParentLocationName"),
                            LocationId = reader.GetInt32("LocationId"),
                            LocationName = reader.GetString("LocationName"),
                            LocationType = reader.GetString("LocationType"),
                            Party = reader.GetString("Party"),
                            KitRentalAmount = reader.GetDecimal("KitRentalAmount"),
                            KitPartAmount = reader.GetDecimal("KitPartAmount"),
                            PartAmount = reader.GetDecimal("PartAmount")
                        });
                    }

                }
                return lstRevenueProjection;
            }
        }

        public List<RevenueProjection> GetRevenueProjectionByParentLocationId(int parentLocationId, DateTime startDate, DateTime endDate)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<RevenueProjection> lstRevenueProjection = new List<RevenueProjection>();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetRevenueProjectionByParentLocationId))
            {
                db.AddInParameter(cmd, "@ParentLocationId", DbType.Int32, parentLocationId);
                db.AddInParameter(cmd, "@StartDate", DbType.Date, startDate);
                db.AddInParameter(cmd, "@EndDate", DbType.Date, endDate);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstRevenueProjection.Add(new RevenueProjection()
                        {
                            ParentLocationId = reader.GetInt32("ParentLocationId"),
                            ParentLocationName = reader.GetString("ParentLocationName"),
                            LocationId = reader.GetInt32("LocationId"),
                            LocationName = reader.GetString("LocationName"),
                            LocationType = reader.GetString("LocationType"),
                            Party = reader.GetString("Party"),
                            KitRentalAmount = reader.GetDecimal("KitRentalAmount"),
                            KitPartAmount = reader.GetDecimal("KitPartAmount"),
                            PartAmount = reader.GetDecimal("PartAmount"),
                            TotalAmount = reader.GetDecimal("TotalAmount")
                        });
                    }

                }
                return lstRevenueProjection;
            }
        }
    }
}
