using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using System.Web.Configuration;

namespace VCTWeb.Core.Domain
{
    public class RequestTransactionRepository
    {
        public void SaveRequestTransaction(RequestTransaction newRequestTransaction)
        {
            System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(Encryption.Decrypt(WebConfigurationManager.AppSettings["LinkedServerConnectionString"].ToString()));
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_SAVEREQUESTTRANSACTION))
            {
                db.AddInParameter(cmd, "@ServerName", DbType.String, builder.DataSource);
                db.AddInParameter(cmd, "@UserName", DbType.String, builder.UserID);
                db.AddInParameter(cmd, "@Password", DbType.String, builder.Password);
                db.AddInParameter(cmd, "@InitialCatalog", DbType.String, builder.InitialCatalog);
                db.AddInParameter(cmd, "@Comments", DbType.String, newRequestTransaction.Comments);
                db.AddInParameter(cmd, "@LocationId", DbType.Int64, newRequestTransaction.LocationId);
                db.AddInParameter(cmd, "@RequestId", DbType.Int64, newRequestTransaction.RequestId);
                db.AddInParameter(cmd, "@RequestTransactionId", DbType.Int64, newRequestTransaction.RequestTransactionId);
                db.AddInParameter(cmd, "@RequestStatus", DbType.String, newRequestTransaction.RequestStatus);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, newRequestTransaction.UpdatedBy);
                db.ExecuteNonQuery(cmd);
            }
        }
    }
}
