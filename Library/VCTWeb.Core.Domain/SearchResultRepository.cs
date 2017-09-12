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
    public class SearchResultRepository
    {
        //public List<KitSearchResult> GetSearchResultByRequestId(long requestId, string updatedBy)
        //{
        //    System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(Encryption.Decrypt(WebConfigurationManager.AppSettings["LinkedServerConnectionString"].ToString()));
        //    SafeDataReader reader = null;
        //    Database db = DbHelper.CreateDatabase();
        //    List<KitSearchResult> lstKitSearchResult = new List<KitSearchResult>();
        //    KitSearchResult newKitSearchResult = new KitSearchResult();
        //    using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETSEARCHRESULTBYREQUESTID))
        //    {
        //        db.AddInParameter(cmd, "@ServerName", DbType.String, builder.DataSource);
        //        db.AddInParameter(cmd, "@UserName", DbType.String, builder.UserID);
        //        db.AddInParameter(cmd, "@Password", DbType.String, builder.Password);
        //        db.AddInParameter(cmd, "@InitialCatalog", DbType.String, builder.InitialCatalog);
        //        db.AddInParameter(cmd, "@RequestId", DbType.Int64, requestId);
        //        db.AddInParameter(cmd, "@UpdatedBy", DbType.String, updatedBy);
        //        using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
        //        {
        //            while (reader.Read())
        //            {
        //                newKitSearchResult = LoadKitSearchResult(reader);
        //                lstKitSearchResult.Add(newKitSearchResult);
        //            }

        //        }
        //        return lstKitSearchResult;
        //    }
        //}

        public List<KitSearchResult> GetSearchResultByCaseId(long caseId, int quantity, string updatedBy)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<KitSearchResult> lstKitSearchResult = new List<KitSearchResult>();
            KitSearchResult newKitSearchResult = new KitSearchResult();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETSEARCHRESULTBYCASEID))
            {
                db.AddInParameter(cmd, "@CaseId", DbType.Int64, caseId);
                db.AddInParameter(cmd, "@Quantity", DbType.Int32, quantity);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, updatedBy);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newKitSearchResult = LoadKitSearchResult(reader);
                        lstKitSearchResult.Add(newKitSearchResult);
                    }

                }
                return lstKitSearchResult;
            }
        }

        //public List<KitSearchResult> GetSearchResultForRequestedLocation(long requestId)
        //{
        //    SafeDataReader reader = null;
        //    Database db = DbHelper.CreateDatabase();
        //    List<KitSearchResult> lstKitSearchResult = new List<KitSearchResult>();
        //    KitSearchResult newKitSearchResult = new KitSearchResult();
        //    using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETSEARCHRESULTFORREQUESTEDLOCATION))
        //    {
        //        db.AddInParameter(cmd, "@RequestId", DbType.Int64, requestId);
        //        using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
        //        {
        //            while (reader.Read())
        //            {
        //                newKitSearchResult = LoadKitSearchResult(reader);
        //                lstKitSearchResult.Add(newKitSearchResult);
        //            }

        //        }
        //        return lstKitSearchResult;
        //    }
        //}

        public List<KitSearchResult> GetSearchResultForRequestedLocationByCaseId(long caseId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<KitSearchResult> lstKitSearchResult = new List<KitSearchResult>();
            KitSearchResult newKitSearchResult = new KitSearchResult();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETSEARCHRESULTFORREQUESTEDLOCATIONBYCASEID))
            {
                db.AddInParameter(cmd, "@CaseId", DbType.Int64, caseId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newKitSearchResult = LoadKitSearchResult(reader);
                        lstKitSearchResult.Add(newKitSearchResult);
                    }

                }
                return lstKitSearchResult;
            }
        }

        //public List<KitSearchResult> GetSearchResultForShipToLocation(long requestId)
        //{
        //    SafeDataReader reader = null;
        //    Database db = DbHelper.CreateDatabase();
        //    List<KitSearchResult> lstKitSearchResult = new List<KitSearchResult>();
        //    KitSearchResult newKitSearchResult = new KitSearchResult();
        //    using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETSEARCHRESULTFORSHIPTOLOCATION))
        //    {
        //        db.AddInParameter(cmd, "@RequestId", DbType.Int64, requestId);
        //        using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
        //        {
        //            while (reader.Read())
        //            {
        //                newKitSearchResult = LoadKitSearchResult(reader);
        //                lstKitSearchResult.Add(newKitSearchResult);
        //            }

        //        }
        //        return lstKitSearchResult;
        //    }
        //}

        public List<KitSearchResult> GetSearchResultForShipToLocationByCaseId(long caseId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<KitSearchResult> lstKitSearchResult = new List<KitSearchResult>();
            KitSearchResult newKitSearchResult = new KitSearchResult();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETSEARCHRESULTFORSHIPTOLOCATIONBYCASEID))
            {
                db.AddInParameter(cmd, "@CaseId", DbType.Int64, caseId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newKitSearchResult = LoadKitSearchResult(reader);
                        lstKitSearchResult.Add(newKitSearchResult);
                    }

                }
                return lstKitSearchResult;
            }
        }

        private SearchResult Load(SafeDataReader reader)
        {
            SearchResult newSearchResult = new SearchResult();

            //newSearchResult.RequestId = reader.GetInt64("RequestId");
            //newSearchResult.CreatedBy = reader.GetString("CreatedBy");
            //newSearchResult.CreatedOn = reader.GetDateTime("CreatedOn");
            //newSearchResult.Quantity = reader.GetInt32("Quantity");
            //newSearchResult.Comments = reader.GetString("Comments");
            //newSearchResult.RequiredOn = reader.GetDateTime("RequiredOn");
            //newSearchResult.ProcedureId = reader.GetNullableInt64("ProcedureId");
            //newSearchResult.ShipToParty = reader.GetNullableInt64("ShipToParty");            
            //newSearchResult.LocationId = reader.GetInt32("LocationId");
            
            return newSearchResult;
        }

        private KitSearchResult LoadKitSearchResult(SafeDataReader reader)
        {
            KitSearchResult newKitSearchResult = new KitSearchResult();

            newKitSearchResult.KitNumber = reader.GetString("KitNumber");
            newKitSearchResult.KitName = reader.GetString("KitName");
            newKitSearchResult.BranchName = reader.GetString("BranchName");
            newKitSearchResult.BranchId = reader.GetInt32("BranchId");
            newKitSearchResult.BranchAddress = reader.GetString("BranchAddress");
            newKitSearchResult.Address2 = reader.GetString("Address2");
            newKitSearchResult.Quantity = reader.GetInt32("Quantity");
            newKitSearchResult.Excess = reader.GetNullableInt32("Excess");
            newKitSearchResult.PerfectExcess = reader.GetNullableInt32("PerfectExcess");
            newKitSearchResult.PartialExcess = reader.GetNullableInt32("PartialExcess");
            newKitSearchResult.TotalKitBuilt = reader.GetNullableInt32("TotalKitBuilt");
            newKitSearchResult.TotalKitShipped = reader.GetNullableInt32("TotalKitShipped");
            newKitSearchResult.TotalKitPurged = reader.GetNullableInt32("TotalKitPurged");
            newKitSearchResult.TotalKitHold = reader.GetNullableInt32("TotalKitHold");
            newKitSearchResult.ReservedQty = reader.GetNullableInt32("ReservedQty");
            newKitSearchResult.BOMItemCount = reader.GetNullableInt32("BOMItemCount");
            newKitSearchResult.BuiltItemCount = reader.GetNullableInt32("BuiltItemCount");
            newKitSearchResult.MatchingPtage = reader.GetNullableInt32("MatchingPtage");
            newKitSearchResult.PreviousCheckInDate = reader.GetNullableDateTime("PreviousCheckInDate");
            newKitSearchResult.RequestedBy = reader.GetString("RequestedBy");
            newKitSearchResult.RequestedLocation = reader.GetString("RequestedLocation");
            newKitSearchResult.RequestedLocationId = reader.GetInt32("RequestedLocationId");
            newKitSearchResult.CatalogNumber = reader.GetString("CatalogNumber");
            newKitSearchResult.ShipToCustomer = reader.GetString("ShipToCustomer");
            newKitSearchResult.ShipToCustomerAdd1 = reader.GetString("ShipToCustomerAdd1");
            newKitSearchResult.ShipToCustomerAdd2 = reader.GetString("ShipToCustomerAdd2");
            newKitSearchResult.Latitude = reader.GetDecimal("Latitude");
            newKitSearchResult.Longitude = reader.GetDecimal("Longitude");
            newKitSearchResult.ContactPersonName = reader.GetString("ContactPersonName");
            newKitSearchResult.ContactPersonEmail = reader.GetString("ContactPersonEmail");

            return newKitSearchResult;
        }
    }
}
