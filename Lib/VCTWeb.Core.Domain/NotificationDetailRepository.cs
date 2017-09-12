using System;
using System.Collections.Generic;
using System.Data;

namespace VCTWeb.Core.Domain
{
    public class NotificationDetailRepository
    {
        private readonly string _user;

        public NotificationDetailRepository()
        {

        }

        public NotificationDetailRepository(string user)
        {
            _user = user;
        }

        public bool SaveNotificationDetail(NotificationDetail theNotificationDetail)
        {
            bool bRetVal;
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.UspEppSaveNotificationDetail))
            {
                db.AddInParameter(cmd, "@AccountNumber", DbType.String, theNotificationDetail.AccountNumber);
                db.AddInParameter(cmd, "@NotificationType", DbType.String, theNotificationDetail.NotificationType);
                db.AddInParameter(cmd, "@ReceiverType", DbType.String, theNotificationDetail.ReceiverType);
                db.AddInParameter(cmd, "@ReceiverName", DbType.String, theNotificationDetail.ReceiverName);

                if (theNotificationDetail.ReceiverType.Trim().ToUpper() == "OTHER")
                    db.AddInParameter(cmd, "@ReceiverEmailID", DbType.String, theNotificationDetail.ReceiverEmailID);

                db.AddInParameter(cmd, "@TimeToSendEmail", DbType.String, theNotificationDetail.TimeToSendEmail);
                db.AddInParameter(cmd, "@ConfigurationSetting", DbType.String, theNotificationDetail.ConfigurationSetting);
                db.AddInParameter(cmd, "@UTCDifferenceInMins", DbType.Int32, theNotificationDetail.UTCDifferenceInMins);
                db.AddInParameter(cmd, "@LastEmailSent", DbType.DateTime, theNotificationDetail.LastEmailSent);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);
                bRetVal = (db.ExecuteNonQuery(cmd) > 0);
            }
            return bRetVal;
        }

        public bool UpdateNotificationDetailByNotificationDetailId(Int64 notificationDetailId, string configurationSetting, string timeToSendEmail, int uTcDifferenceInMins)
        {
            bool bRetVal;
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.UspEppUpdateNotificationDetail))
            {
                db.AddInParameter(cmd, "@NotificationDetailId", DbType.Int64, notificationDetailId);
                db.AddInParameter(cmd, "@TimeToSendEmail", DbType.String, timeToSendEmail);
                db.AddInParameter(cmd, "@ConfigurationSetting", DbType.String, configurationSetting);
                db.AddInParameter(cmd, "@UTCDifferenceInMins", DbType.Int32, uTcDifferenceInMins);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);
                bRetVal = (db.ExecuteNonQuery(cmd) > 0);
            }
            return bRetVal;
        }

        public NotificationDetail FetchNotificationDetailByKey(Int64 notificationDetailId)
        {
            NotificationDetail newNotificationDetail = null;
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.UspEppFetchNotificationDetailByKey))
            {
                db.AddInParameter(cmd, "@NotificationDetailId", DbType.Int64, notificationDetailId);
                using (var reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    if (reader.Read())
                    {
                        newNotificationDetail = Load(reader);
                    }
                }
                return newNotificationDetail;
            }
        }

        public List<NotificationDetail> FetchAllNotificationDetailByAccountNumber(string accountNumber, string notificationType)
        {
            var listOfNotificationDetail = new List<NotificationDetail>();
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.UspEppFetchAllNotificationDetailByAccountNumber))
            {
                db.AddInParameter(cmd, "@AccountNumber", DbType.String, accountNumber);
                db.AddInParameter(cmd, "@NotificationType", DbType.String, notificationType);
                using (var reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        listOfNotificationDetail.Add(Load(reader));
                    }
                }
            }
            return listOfNotificationDetail;
        }

        public bool Delete(Int64 notificationDetailId)
        {
            bool bRetVal;
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.UspEppDeleteNotificationDetail))
            {
                db.AddInParameter(cmd, "@NotificationDetailId", DbType.Int64, notificationDetailId);
                bRetVal = (db.ExecuteNonQuery(cmd) > 0);
            }
            return bRetVal;
        }

        private NotificationDetail Load(SafeDataReader reader)
        {
            var newNotificationDetail = new NotificationDetail
            {
                NotificationDetailId = reader.GetInt64("NotificationDetailId"),
                AccountNumber = reader.GetString("AccountNumber"),
                NotificationType = reader.GetString("NotificationType"),
                ReceiverType = reader.GetString("ReceiverType"),
                ReceiverName = reader.GetString("ReceiverName"),
                ReceiverEmailID = reader.GetString("ReceiverEmailID"),
                TimeToSendEmail = reader.GetString("TimeToSendEmail"),
                ConfigurationSetting = reader.GetString("ConfigurationSetting"),
                UTCDifferenceInMins = reader.GetInt32("UTCDifferenceInMins"),
                LastEmailSent = reader.GetNullableDateTime("LastEmailSent"),
                UpdatedBy = reader.GetString("UpdatedBy"),
                UpdatedOn = reader.GetDateTime("UpdatedOn")
            };

            if ((System.Web.HttpContext.Current.Session != null) && (System.Web.HttpContext.Current.Session["TimeZoneOffset"] != null))
            {
                DateTime uTCDateTime = new DateTime(2001, 1, 1, Convert.ToInt32(newNotificationDetail.TimeToSendEmail.Substring(0, 2)), Convert.ToInt32(newNotificationDetail.TimeToSendEmail.Substring(3, 2)), 0);
                double timeZoneOffset = double.Parse(System.Web.HttpContext.Current.Session["TimeZoneOffset"].ToString());
                DateTime localDateTime = uTCDateTime.AddMinutes(0 - timeZoneOffset);
                newNotificationDetail.TimeToSendEmail = localDateTime.ToString("HH:mm");
            }
            return newNotificationDetail;
        }

    }
}

