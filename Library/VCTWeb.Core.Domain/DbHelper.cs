using System.Configuration;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Web.Configuration;


namespace VCTWeb.Core.Domain
{
    /// <summary>
    /// DbHelper class contain static methods for handling Date and DateTime 
    /// which can be stored into database.
    /// </summary>
    public static class DbHelper
    {

        /// <summary>
        /// against the passed connection string
        /// create the generic database
        /// </summary>
        /// <returns>returns microsoft practice generic database</returns>
        public static Database CreateDatabase()
        {
            string timeout = string.Empty;
            //string _connectionString = Encryption.Decrypt(WebConfigurationManager.AppSettings["ConnectionString"].ToString());

            string _connectionString = Encryption.Decrypt(WebConfigurationManager.AppSettings["ConnectionString"].ToString());
            

            if (!string.IsNullOrEmpty(_connectionString))
            {

                if (!string.IsNullOrEmpty(WebConfigurationManager.AppSettings["ConnectTimeout"]))
                {
                    _connectionString = _connectionString + ";Connect Timeout=" + WebConfigurationManager.AppSettings["ConnectTimeout"];
                    return new GenericDatabase(_connectionString, DbProviderFactories.GetFactory("System.Data.SqlClient"));
                }
                else
                {
                    return new GenericDatabase(_connectionString, DbProviderFactories.GetFactory("System.Data.SqlClient"));
                }
            }
            else
                throw new Exception("Connection string is Empty");
        }

        public static int GetCommandTimeOut(DbCommand dbCommand)
        {
            if (!string.IsNullOrEmpty(WebConfigurationManager.AppSettings["ConnectTimeout"]))
            {
                return Convert.ToInt32(WebConfigurationManager.AppSettings["ConnectTimeout"]);
            }

            return dbCommand.CommandTimeout;
        }
    }
}