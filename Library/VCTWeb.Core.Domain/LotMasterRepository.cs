using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace VCTWeb.Core.Domain
{
    public class LotMasterRepository
    {
        public List<LotMaster> GetLotsByLotNumber(string sLotNumber)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<LotMaster> lstLotMaster = new List<LotMaster>();
            LotMaster newLotMaster = new LotMaster();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETLOTSBYLOTNUMBER))
            {
                db.AddInParameter(cmd, "@LotNum", DbType.String, sLotNumber);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newLotMaster = Load(reader);
                        lstLotMaster.Add(newLotMaster);
                    }

                }
                return lstLotMaster;
            }
        }

        private LotMaster Load(SafeDataReader reader)
        {
            LotMaster newLotMaster = new LotMaster();

            newLotMaster.ID = reader.GetInt32("ID");
            newLotMaster.ProductNum = reader.GetString("ProductNum");
            newLotMaster.LotNum = reader.GetString("LotNum");
            newLotMaster.LotCommDate = reader.GetNullableDateTime("LotCommDate");
            newLotMaster.LotExpirydate = reader.GetNullableDateTime("LotExpirydate");
            newLotMaster.Description = reader.GetString("Description");

            return newLotMaster;
        }
    }
}
