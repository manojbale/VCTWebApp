using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using System.Globalization;
using System.Web.Configuration;

namespace VCTWeb.Core.Domain
{
    public class RequestRepository
    {
        private string _user;

        public RequestRepository()
        {
        }

        public RequestRepository(string user)
        {
            _user = user;
        }

        public bool SendRequest(long requestId, int requestedQuantity, int locationId)
        {
            System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(Encryption.Decrypt(WebConfigurationManager.AppSettings["LinkedServerConnectionString"].ToString()));
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_SENDREQUEST))
            {
                db.AddInParameter(cmd, "@ServerName", DbType.String, builder.DataSource);
                db.AddInParameter(cmd, "@UserName", DbType.String, builder.UserID);
                db.AddInParameter(cmd, "@Password", DbType.String, builder.Password);
                db.AddInParameter(cmd, "@InitialCatalog", DbType.String, builder.InitialCatalog);
                db.AddInParameter(cmd, "@RequestId", DbType.Int64, requestId);
                db.AddInParameter(cmd, "@RequestedQuantity", DbType.Int32, requestedQuantity);
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);
                int quantityInserted = Convert.ToInt32(db.ExecuteScalar(cmd), CultureInfo.InvariantCulture);
                return (quantityInserted == requestedQuantity);
            }
        }

        public List<UserPendingRequest> GetRequestsByRegionLocation(int regionId, int? locationId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<UserPendingRequest> lstUserPendingRequest = new List<UserPendingRequest>();
            UserPendingRequest newUserPendingRequest = new UserPendingRequest();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETREQUESTSBYREGIONLOCATION))
            {
                db.AddInParameter(cmd, "@RegionId", DbType.Int32, regionId);
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newUserPendingRequest = LoadUserPendingRequests(reader);
                        lstUserPendingRequest.Add(newUserPendingRequest);
                    }

                }
                return lstUserPendingRequest;
            }
        }
        public List<UserPendingRequest> GetRequestsByUser(string userName)
        {
            System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(Encryption.Decrypt(WebConfigurationManager.AppSettings["LinkedServerConnectionString"].ToString()));
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<UserPendingRequest> lstUserPendingRequest = new List<UserPendingRequest>();
            UserPendingRequest newUserPendingRequest = new UserPendingRequest();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETREQUESTSBYUSER))
            {
                db.AddInParameter(cmd, "@ServerName", DbType.String, builder.DataSource);
                db.AddInParameter(cmd, "@UserName", DbType.String, builder.UserID);
                db.AddInParameter(cmd, "@Password", DbType.String, builder.Password);
                db.AddInParameter(cmd, "@InitialCatalog", DbType.String, builder.InitialCatalog);
                db.AddInParameter(cmd, "@CreatedBy", DbType.String, userName);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newUserPendingRequest = LoadUserPendingRequests(reader);
                        lstUserPendingRequest.Add(newUserPendingRequest);
                    }

                }
                return lstUserPendingRequest;
            }
        }

        public List<UserPendingRequest> GetFilteredRequestsByUser(string userName, DateTime StartDate, DateTime EndDate)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<UserPendingRequest> lstUserPendingRequest = new List<UserPendingRequest>();
            UserPendingRequest newUserPendingRequest = new UserPendingRequest();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETFILTEREDREQUESTSBYUSER))
            {
                db.AddInParameter(cmd, "@CreatedBy", DbType.String, userName);
                db.AddInParameter(cmd, "@StartDate", DbType.DateTime, StartDate);
                db.AddInParameter(cmd, "@EndDate", DbType.DateTime, EndDate);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newUserPendingRequest = LoadUserPendingRequests(reader);
                        lstUserPendingRequest.Add(newUserPendingRequest);
                    }

                }
                return lstUserPendingRequest;
            }
        }

        public List<SummaryPendingRequest> GetRequestSummaryByRegion(int regionId)
        {
            System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(Encryption.Decrypt(WebConfigurationManager.AppSettings["LinkedServerConnectionString"].ToString()));
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<SummaryPendingRequest> lstSummaryPendingRequest = new List<SummaryPendingRequest>();
            SummaryPendingRequest newSummaryPendingRequest = new SummaryPendingRequest();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETREQUESTSUMMARYBYREGION))
            {
                db.AddInParameter(cmd, "@ServerName", DbType.String, builder.DataSource);
                db.AddInParameter(cmd, "@UserName", DbType.String, builder.UserID);
                db.AddInParameter(cmd, "@Password", DbType.String, builder.Password);
                db.AddInParameter(cmd, "@InitialCatalog", DbType.String, builder.InitialCatalog);
                db.AddInParameter(cmd, "@RegionId", DbType.Int32, regionId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newSummaryPendingRequest = LoadSummaryPendingRequest(reader);
                        lstSummaryPendingRequest.Add(newSummaryPendingRequest);
                    }

                }
                return lstSummaryPendingRequest;
            }
        }

        public void SaveRequest(Request newRequest, string requestDetailXmlString, string requestTransactionXmlString)
        {
            System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(Encryption.Decrypt(WebConfigurationManager.AppSettings["LinkedServerConnectionString"].ToString()));
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_SAVENEWREQUEST))
            {
                db.AddInParameter(cmd, "@ServerName", DbType.String, builder.DataSource);
                db.AddInParameter(cmd, "@UserName", DbType.String, builder.UserID);
                db.AddInParameter(cmd, "@Password", DbType.String, builder.Password);
                db.AddInParameter(cmd, "@InitialCatalog", DbType.String, builder.InitialCatalog);
                db.AddInParameter(cmd, "@Comments", DbType.String, newRequest.Comments);
                db.AddInParameter(cmd, "@CreatedBy", DbType.String, newRequest.CreatedBy);
                db.AddInParameter(cmd, "@CreatedOn", DbType.Date, newRequest.CreatedOn);
                //db.AddInParameter(cmd, "@KitNumber", DbType.String, newRequest.KitNumber);
                db.AddInParameter(cmd, "@ShipToParty", DbType.Int64, newRequest.ShipToParty);
                db.AddInParameter(cmd, "@LocationId", DbType.Int64, newRequest.LocationId);
                //db.AddInParameter(cmd, "@ProcedureId", DbType.Int64, newRequest.ProcedureId);
                db.AddInParameter(cmd, "@Quantity", DbType.Int32, newRequest.Quantity);
                db.AddInParameter(cmd, "@RequestId", DbType.Int64, newRequest.RequestId);
                db.AddInParameter(cmd, "@RequiredOn", DbType.DateTime, newRequest.RequiredOn.ToUniversalTime());
                db.AddInParameter(cmd, "@RequestDetailXmlString", DbType.String, requestDetailXmlString);
                db.AddInParameter(cmd, "@RequestTransactionXmlString", DbType.String, requestTransactionXmlString);

                newRequest.RequestId = Convert.ToInt64(db.ExecuteScalar(cmd), CultureInfo.InvariantCulture);

                //db.ExecuteNonQuery(cmd);
            }
        }

        private Request Load(SafeDataReader reader)
        {
            Request newRequest = new Request();

            newRequest.RequestId = reader.GetInt64("RequestId");
            newRequest.CreatedBy = reader.GetString("CreatedBy");
            newRequest.CreatedOn = reader.GetDateTime("CreatedOn");
            newRequest.Quantity = reader.GetInt32("Quantity");
            newRequest.Comments = reader.GetString("Comments");
            newRequest.RequiredOn = reader.GetDateTime("RequiredOn");
            newRequest.ShipToParty = reader.GetNullableInt64("ShipToParty");
            newRequest.LocationId = reader.GetInt32("LocationId");

            return newRequest;
        }

        private UserPendingRequest LoadUserPendingRequests(SafeDataReader reader)
        {
            UserPendingRequest newUserPendingRequest = new UserPendingRequest();

            newUserPendingRequest.RequestId = reader.GetInt64("RequestId");
            newUserPendingRequest.CreatedBy = reader.GetString("CreatedBy");
            newUserPendingRequest.CreatedOn = reader.GetDateTime("CreatedOn");
            newUserPendingRequest.Quantity = reader.GetInt32("Quantity");
            newUserPendingRequest.Comments = reader.GetString("Comments");
            newUserPendingRequest.RequiredOn = reader.GetDateTime("RequiredOn");
            //newUserPendingRequest.ProcedureId = reader.GetNullableInt64("ProcedureId");
            newUserPendingRequest.ShipToParty = reader.GetNullableInt64("ShipToParty");
            newUserPendingRequest.LocationId = reader.GetInt32("LocationId");


            newUserPendingRequest.KitNumber = reader.GetString("KitNumber");
            newUserPendingRequest.KitName = reader.GetString("KitName");
            newUserPendingRequest.RequestStatus = reader.GetString("RequestStatus");
            newUserPendingRequest.ShipToCustomer = reader.GetString("ShipToCustomer");
            newUserPendingRequest.ProcedureName = reader.GetString("ProcedureName");
            newUserPendingRequest.CatalogNumberList = reader.GetString("CatalogNumberList");
            newUserPendingRequest.LocationName = reader.GetString("LocationName");
            newUserPendingRequest.RequestNumber = reader.GetString("RequestNumber");

            return newUserPendingRequest;
        }

        private SummaryPendingRequest LoadSummaryPendingRequest(SafeDataReader reader)
        {
            SummaryPendingRequest newSummaryPendingRequest = new SummaryPendingRequest();

            newSummaryPendingRequest.RequestedLocationId = reader.GetInt32("RequestedLocationId");
            newSummaryPendingRequest.RequestedLocationName = reader.GetString("RequestedLocationName");
            newSummaryPendingRequest.KitStatus = reader.GetString("KitStatus");
            newSummaryPendingRequest.KitCount = reader.GetInt32("KitCount");

            return newSummaryPendingRequest;
        }

        public List<RequestStatus> GetRequestStatusListByUser(string userName, DateTime StartDate, DateTime EndDate)
        {
            System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(Encryption.Decrypt(WebConfigurationManager.AppSettings["LinkedServerConnectionString"].ToString()));
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<RequestStatus> RequestStatusList = new List<RequestStatus>();
            RequestStatus Status = new RequestStatus();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetListOfRequestStausByUser))
            {
                db.AddInParameter(cmd, "@ServerName", DbType.String, builder.DataSource);
                db.AddInParameter(cmd, "@UserName", DbType.String, builder.UserID);
                db.AddInParameter(cmd, "@Password", DbType.String, builder.Password);
                db.AddInParameter(cmd, "@InitialCatalog", DbType.String, builder.InitialCatalog);
                db.AddInParameter(cmd, "@CreatedBy", DbType.String, userName);
                db.AddInParameter(cmd, "@StartDate", DbType.DateTime, StartDate);
                db.AddInParameter(cmd, "@EndDate", DbType.DateTime, EndDate);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        Status = LoadRequestStatus(reader);
                        RequestStatusList.Add(Status);
                    }

                }
                return RequestStatusList;
            }
        }

        public List<RequestStatus> GetRequestStatusListByRegion(int regionId, DateTime StartDate, DateTime EndDate)
        {
            System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(Encryption.Decrypt(WebConfigurationManager.AppSettings["LinkedServerConnectionString"].ToString()));
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<RequestStatus> RequestStatusList = new List<RequestStatus>();
            RequestStatus Status = new RequestStatus();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetListOfRequestStausByRegion))
            {
                db.AddInParameter(cmd, "@ServerName", DbType.String, builder.DataSource);
                db.AddInParameter(cmd, "@UserName", DbType.String, builder.UserID);
                db.AddInParameter(cmd, "@Password", DbType.String, builder.Password);
                db.AddInParameter(cmd, "@InitialCatalog", DbType.String, builder.InitialCatalog);
                db.AddInParameter(cmd, "@RegionId", DbType.String, regionId);
                db.AddInParameter(cmd, "@StartDate", DbType.DateTime, StartDate);
                db.AddInParameter(cmd, "@EndDate", DbType.DateTime, EndDate);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        Status = LoadRequestStatus(reader);
                        RequestStatusList.Add(Status);
                    }

                }
                return RequestStatusList;
            }
        }

        private RequestStatus LoadRequestStatus(SafeDataReader reader)
        {
            RequestStatus rs = new RequestStatus();
            rs.Status = reader.GetString("RequestStatus");
            rs.Count = reader.GetInt32("Count");

            return rs;
        }

        //public bool UpdateRequestStatusById(long requestId, string requestStatus, long locationId, string user)
        //{
        //    bool returnvalue = false;
        //    Database db = DbHelper.CreateDatabase();
        //    using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_UpdateRequestStatusById))
        //    {
        //        db.AddInParameter(cmd, "@RequestId", DbType.Int64, requestId);
        //        db.AddInParameter(cmd, "@RequestStatus", DbType.String, requestStatus);
        //        db.AddInParameter(cmd, "@LocationId", DbType.Int64, locationId);
        //        db.AddInParameter(cmd, "@UpdatedBy", DbType.String, user);
        //        db.ExecuteScalar(cmd);
        //        returnvalue = true;
        //    }
        //    return returnvalue;
        //}

        public List<UserPendingRequest> GetFilteredRequestsByRegion(int regionId, DateTime StartDate, DateTime EndDate)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<UserPendingRequest> lstUserPendingRequest = new List<UserPendingRequest>();
            UserPendingRequest newUserPendingRequest = new UserPendingRequest();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETFILTEREDREQUESTSBYREGION))
            {
                db.AddInParameter(cmd, "@RegionId", DbType.Int64, regionId);
                db.AddInParameter(cmd, "@StartDate", DbType.DateTime, StartDate);
                db.AddInParameter(cmd, "@EndDate", DbType.DateTime, EndDate);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newUserPendingRequest = LoadUserPendingRequests(reader);
                        lstUserPendingRequest.Add(newUserPendingRequest);
                    }

                }
                return lstUserPendingRequest;
            }
        }
    }
}
