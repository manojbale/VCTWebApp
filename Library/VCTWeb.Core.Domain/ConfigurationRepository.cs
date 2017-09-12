using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace VCTWeb.Core.Domain
{
    /// <summary>
    /// This class provides the methods to retrieve and update data related to Configurations
    /// </summary>
    public class ConfigurationRepository
    {
        private string _user;

        public ConfigurationRepository()
        {
        }
        public ConfigurationRepository(string user)
        {
            _user = user;
        }

        #region Public Methods

        /// <summary>
        /// Fetches all configurations.
        /// </summary>
        /// <returns>List of Configuration objects</returns>
        public List<Configuration> FetchAllConfigurations()
        {
            List<Configuration> configurations = null;

            Database db = DbHelper.CreateDatabase();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GET_ALL_CONFIGURATIONS))
            {
                using (SafeDataReader reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    configurations = new List<Configuration>();
                    while (reader.Read())
                    {
                        configurations.Add(LoadConfiguration(reader));
                    }
                }
            }

            return configurations;
        }

        /// <summary>
        /// Fetches all configuration groups.
        /// </summary>
        /// <returns>List of Configuration objects</returns>
        public List<Configuration> FetchAllConfigurationGroups()
        {
            List<Configuration> configurations = null;

            Database db = DbHelper.CreateDatabase();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GET_ALL_CONFIGURATION_GROUPS))
            {
                using (SafeDataReader reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    configurations = new List<Configuration>();
                    while (reader.Read())
                    {
                        configurations.Add(LoadConfigurationGroups(reader));
                    }
                }
            }

            return configurations;
        }

        /// <summary>
        /// Fetches the configurations by group.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <returns>List of Configuration objects</returns>
        public List<Configuration> FetchConfigurationsByGroup(string group)
        {
            List<Configuration> configurations = null;

            Database db = DbHelper.CreateDatabase();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GET_CONFIGURATIONS_BY_GROUP))
            {
                db.AddInParameter(cmd, "KeyGroup", DbType.String, group.Trim());

                using (SafeDataReader reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    configurations = new List<Configuration>();
                    while (reader.Read())
                    {
                        configurations.Add(LoadConfiguration(reader));
                    }
                }
            }
            return configurations;
        }


        /// <summary>
        /// Fetches the editable configurations.
        /// </summary>
        /// <returns>List of Configuration objects</returns>
        public List<Configuration> FetchEditableConfigurations()
        {
            List<Configuration> configurations = null;

            Database db = DbHelper.CreateDatabase();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GET_EDITABLE_CONFIGURATIONS))
            {
                using (SafeDataReader reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    configurations = new List<Configuration>();
                    while (reader.Read())
                    {
                        configurations.Add(LoadConfiguration(reader));
                    }
                }
            }

            return configurations;
        }

        /// <summary>
        /// Updates the configurations.
        /// </summary>
        /// <param name="configurations">The configurations.</param>
        public void UpdateConfigurations(List<Configuration> configurations)
        {
            if (configurations != null && configurations.Count > 0)
            {
                foreach (Configuration configuration in configurations)
                {
                    if (configuration.IsModified)
                    {
                        SaveConfigurations(configuration);
                    }
                }
            }

        }

        /// <summary>
        /// Saves the configurations.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void SaveConfigurations(Configuration configuration)
        {
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_UPDATE_CONFIGURATION))
            {
                db.AddInParameter(cmd, "@KeyName", DbType.String, configuration.KeyName);
                db.AddInParameter(cmd, "@KeyValue", DbType.String, configuration.KeyValue);
                db.ExecuteNonQuery(cmd);
            }
        }

        public string GetConfigurationKeyValue(string key)
        {
            Database db = DbHelper.CreateDatabase();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GET_CONFIGURATION_KEY_VALUE))
            {
                db.AddInParameter(cmd, "KeyName", DbType.String, key.Trim());
                return Convert.ToString(db.ExecuteScalar(cmd));

            }

        }

        public bool GetByPassInventory()
        {
            string bypassInventory = string.Empty;

            SafeDataReader reader = null;

            Database db = DbHelper.CreateDatabase();
            string sql = "SELECT Value FROM Dictionary WHERE [KeyName] = 'ByPassInventory'";

            using (reader = new SafeDataReader(db.ExecuteReader(CommandType.Text, sql)))
            {
                while (reader.Read())
                {
                    bypassInventory = reader.GetString("KeyValue");
                }
            }

            if (bypassInventory == "true")
                return true;
            else
                return false;
        }


        public void UpdateByPassInventory(bool bypassInventory)
        {
            string sqlQuery = string.Empty;
            Database db = DbHelper.CreateDatabase();

            if (bypassInventory)
                sqlQuery = "UPDATE Dictionary SET KeyValue = 'true' WHERE [KeyName] = 'ByPassInventory'";
            else
                sqlQuery = "UPDATE Dictionary SET KeyValue = 'false' WHERE [KeyName] = 'ByPassInventory'";

            using (DbCommand cmd = db.GetSqlStringCommand(sqlQuery))
            {
                try
                {
                    db.ExecuteNonQuery(cmd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public int GetGridPageSize()
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            int PageSize = 0;

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GET_DICTIONARY_RULE))
            {
                db.AddInParameter(cmd, "@KeyName", DbType.String, "GridPageSize");

                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        PageSize = int.Parse(reader.GetString("KeyValue"));
                    }
                }
            }

            return PageSize;
        }

        #endregion

        #region Private Methods

        private Configuration LoadConfiguration(SafeDataReader reader)
        {
            Configuration configuration = new Configuration();
            configuration.KeyName = reader.GetString("KeyName").Trim();
            configuration.KeyValue = reader.GetString("KeyValue").Trim();
            configuration.DataType = reader.GetString("DataType");
            configuration.Editable = reader.GetBoolean("Editable");
            configuration.KeyGroup = reader.GetString("KeyGroup");
            configuration.Description = reader.GetString("Description");
            configuration.ListValues = reader.GetString("ListValues");  
            configuration.IsModified = false;
            configuration.IsNew = false;

            return configuration;
        }

        private Configuration LoadConfigurationGroups(SafeDataReader reader)
        {
            Configuration configuration = new Configuration();
            configuration.KeyGroup = reader.GetString("KeyGroup");
            return configuration;
        }
        #endregion
    }
}
