using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;

namespace VCTWeb.Core.Domain
{
    /// <summary>
    /// Country Repository Class
    /// </summary>
    public class CountryRepository
    {
        /// <summary>
        /// Gets the country list.
        /// </summary>
        /// <returns></returns>
        public List<Country> GetCountryList()
        {
            List<Country> _countryList = new List<Country>();
            Database db = DbHelper.CreateDatabase();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GET_COUNTRY_LIST))
            {
                using (SafeDataReader reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        Country _country = new Country();
                        _country.CountryCode = reader.GetString("CountryCode").Trim();
                        _country.CountryName = reader.GetString("CountryName").Trim();


                        _countryList.Add(_country);
                    }
                }
            }
            return _countryList;
        }
    }
}
