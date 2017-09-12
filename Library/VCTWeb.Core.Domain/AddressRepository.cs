using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace VCTWeb.Core.Domain
{

    public class AddressRepository
    {

        public AddressRepository()
        {

        }

        #region Public Methods

        /// <summary>
        /// Fetches all Regions.
        /// </summary>
        /// <returns></returns>
        public List<Region> FetchRegions(int parentLocationId = 0)
        {
            SafeDataReader reader = null;
            Region newRegion = null;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETLISTOFREGIONS))
            {
                db.AddInParameter(cmd, "@ParentLocationId", DbType.Int32, parentLocationId);
                List<Region> listOfRegion = new List<Region>();
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newRegion = this.LoadRegion(reader);
                        listOfRegion.Add(newRegion);
                    }

                }
                return listOfRegion;
            }
        }

        /// <summary>
        /// Fetches all Regions.
        /// </summary>
        /// <returns></returns>
        public List<SalesOffice> FetchSalesOffices(int parentLocationId = 0)
        {
            SafeDataReader reader = null;
            SalesOffice newSalesOffice = null;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETLISTOFSALESOFFICES))
            {
                db.AddInParameter(cmd, "@ParentLocationId", DbType.Int32, parentLocationId);
                List<SalesOffice> listOfSalesOffice = new List<SalesOffice>();
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newSalesOffice = this.LoadSalesOffice(reader);
                        listOfSalesOffice.Add(newSalesOffice);
                    }

                }
                return listOfSalesOffice;
            }
        }

        #endregion

        #region Private Methods


        private Region LoadRegion(SafeDataReader reader)
        {
            Region newRegion = new Region();
            newRegion.RegionName = reader.GetString("RegionName").Trim();
            newRegion.RegionId = reader.GetInt32("RegionId");
            return newRegion;
        }


        private SalesOffice LoadSalesOffice(SafeDataReader reader)
        {
            SalesOffice newSalesOffice = new SalesOffice();
            newSalesOffice.LocationId = reader.GetInt32("LocationId");
            newSalesOffice.LocationName = reader.GetString("LocationName").Trim();
            return newSalesOffice;
        }

        #endregion


    }
}
