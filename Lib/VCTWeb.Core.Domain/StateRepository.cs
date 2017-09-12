using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;

namespace VCTWeb.Core.Domain
{
    /// <summary>
    /// State Repository Class
    /// </summary>
    public class StateRepository
    {
        /// <summary>
        /// Gets the state list.
        /// </summary>
        /// <param name="countryCode">The country code.</param>
        /// <returns></returns>
        public List<State> GetStateList(string countryCode)
        {
            List<State> _stateList = new List<State>();
            Database db = DbHelper.CreateDatabase();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GET_STATE_LIST))
            {
                db.AddInParameter(cmd, "@CountryCode", DbType.String, countryCode);
                using (SafeDataReader reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        State _state = new State();
                        _state.StateName = reader.GetString("StateName");

                        _stateList.Add(_state);
                    }
                    reader.Close();
                }
            }
            return _stateList;
        }
    }
}
