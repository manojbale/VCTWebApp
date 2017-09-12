using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace VCTWeb.Core.Domain
{
    /// <summary>
    /// Name		:	CustomerPARLevel  	
    /// Purpose		:	Repository class for table CustomerPARLevel.
    /// Created By	:	Suraj Namdeo
    /// Created On	:	May 11 2015  8:55PM  		
    /// </summary>
    
    public class CustomerPARLevelRepository
    {
        private string _user;

        public CustomerPARLevelRepository()
        {
        }

        public CustomerPARLevelRepository(string user)
        {
            _user = user;
        }

        public bool SaveCustomerPARLevel(CustomerPARLevel theCustomerPARLevel)
        {
            bool bRetVal = false;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand("usp_SaveCustomerPARLevel"))
            {
                db.AddInParameter(cmd, "@AccountNumber", DbType.String, theCustomerPARLevel.AccountNumber);
                db.AddInParameter(cmd, "@RefNum", DbType.String, theCustomerPARLevel.RefNum);
                db.AddInParameter(cmd, "@PARLevelQty", DbType.Int16, theCustomerPARLevel.PARLevelQty);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);                
                bRetVal = (db.ExecuteNonQuery(cmd) > 0 ? true : false);
            }
            return bRetVal;
        }
        
        public List<CustomerPARLevel> FetchCustomerPARLevelByAccountNumber(string AccountNumber)
        {
            List<CustomerPARLevel> listOfCustomerPARLevel = new List<CustomerPARLevel>();
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand("Usp_FetchCustomerPARLevelByAccountNumber"))
            {
                db.AddInParameter(cmd, "@AccountNumber", DbType.String, AccountNumber);
                using (SafeDataReader reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        listOfCustomerPARLevel.Add(Load(reader));
                    }
                }                
            }
            return listOfCustomerPARLevel;
        }
        
        public bool DeleteCustomerPARLevel(string AccountNumber, string RefNum)
        {
            bool returnvalue = false;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand("USP_DeleteCustomerPARLevel"))
            {
                db.AddInParameter(cmd, "@AccountNumber", DbType.String, AccountNumber);
                db.AddInParameter(cmd, "@RefNum", DbType.String, RefNum);
                db.ExecuteScalar(cmd);
                returnvalue = true;
            }
            return returnvalue;
        }

        private CustomerPARLevel Load(SafeDataReader reader)
        {
            CustomerPARLevel newCustomerPARLevel = new CustomerPARLevel();
            newCustomerPARLevel.AccountNumber = reader.GetString("AccountNumber");
            newCustomerPARLevel.RefNum = reader.GetString("RefNum");
            newCustomerPARLevel.Description = reader.GetString("Description");
            newCustomerPARLevel.PARLevelQty = reader.GetInt16("PARLevelQty");
            newCustomerPARLevel.UpdatedBy = reader.GetString("UpdatedBy");
            newCustomerPARLevel.UpdatedOn = reader.GetDateTime("UpdatedOn");
            return newCustomerPARLevel;
        }
    }

}

