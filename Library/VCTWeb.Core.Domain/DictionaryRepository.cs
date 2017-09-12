 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace VCTWeb.Core.Domain
{
    public class DictionaryRepository
    {
        #region Public Static Methods

        public static Dictionary<string, string> GetDictionaryListByPrefix(string keyPrefix)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            Database db = DbHelper.CreateDatabase();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GET_DICTIONARY_LIST_BY_PREFIX))
            {
                db.AddInParameter(cmd, "@KeyPrefix", DbType.String, keyPrefix);
                using (SafeDataReader reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {

                    while (reader.Read())
                    {
                        dictionary.Add(reader.GetString("KeyName").Trim(), reader.GetString("KeyValue").Trim());
                    }
                }
            }
            return dictionary;
        }

        /// <summary>
        /// Gets the dictionary rule.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>dictionary object</returns>
        public  Dictionary GetDictionaryRule(string key)
        {
            Database db = DbHelper.CreateDatabase();
            Dictionary obj = null;
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GET_DICTIONARY_RULE))
            {
                db.AddInParameter(cmd, "@KeyName", DbType.String, key);
                using (SafeDataReader reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    if (reader.Read())
                    {
                        obj = LoadApplicationConfiguration(reader);
                    }
                }
            }

            return obj;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Loads the application configuration.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Dictionary object</returns>
        private Dictionary LoadApplicationConfiguration(SafeDataReader reader)
        {
            Dictionary newApplicationConfiguration = new Dictionary();
            newApplicationConfiguration.KeyValue = reader.GetString("KeyValue");
            return newApplicationConfiguration;
        }

        /// <summary>
        /// Loads the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Dictionary Object</returns>
        private Dictionary Load(SafeDataReader reader)
        {
            Dictionary newConfiguration = new Dictionary();

            newConfiguration.KeyName = reader.GetString("KeyName");
            newConfiguration.KeyValue = reader.GetString("ValueValue");
            return newConfiguration;
        }
        #endregion
    }
}
