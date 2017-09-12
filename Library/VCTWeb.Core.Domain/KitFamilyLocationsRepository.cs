using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace VCTWeb.Core.Domain
{
    public class KitFamilyLocationsRepository
    {

         private string _user;

        public KitFamilyLocationsRepository()
        {
        }

        public KitFamilyLocationsRepository(string user)
        {
            _user = user;
        }

        private KitFamily LoadKitFamily(SafeDataReader reader)
        {
            KitFamily newKitFamily = new KitFamily();
            newKitFamily.KitFamilyId = reader.GetInt64("KitFamilyId");
            newKitFamily.NumberOfTubs = reader.GetInt16("NumberOfTubs");
            newKitFamily.KitFamilyName = reader.GetString("KitFamilyName").Trim();
            newKitFamily.KitFamilyDescription = reader.GetString("KitFamilyDescription").Trim();
            newKitFamily.KitTypeName = reader.GetString("KitTypeName").Trim();
            newKitFamily.IsActive = reader.GetBoolean("IsActive");
            return newKitFamily;
        }

        public List<KitType> FetchAllKitTypes()
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<KitType> lstKitType = new List<KitType>();
            KitType newKitType = new KitType();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETLISTOFKITTYPES))
            {
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newKitType = LoadKitType(reader);
                        lstKitType.Add(newKitType);
                    }

                }
                return lstKitType;
            }
        }

        private KitType LoadKitType(SafeDataReader reader)
        {
            KitType newKitType = new KitType();

            newKitType.KitTypeName = reader.GetString("KitTypeName");

            return newKitType;
        }

        public KitFamily GetKitFamilyByKitFamilyId(long kitFamilyId)
        {
            SafeDataReader reader = null;
            KitFamily newKitFamily = null;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetKitFamilyById))
            {
                db.AddInParameter(cmd, "@KitFamilyId", DbType.Int64, kitFamilyId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newKitFamily = this.LoadKitFamily(reader);
                    }
                }
                return newKitFamily;
            }
        }
              

    }
}
