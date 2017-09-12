using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Globalization;


namespace VCTWeb.Core.Domain
{
    public class TestKLRepository
    {

        private string _user;

        public TestKLRepository()
        {

        }

        public TestKLRepository(string user)
        {
            _user = user;
        }

        public bool SaveTestKL(VCTWeb.Core.Domain.TestKL TestKLToBeSaved, string TestKLXml)
        {
            bool returnvalue = false;
            Database db = VCTWeb.Core.Domain.DbHelper.CreateDatabase();
            //using (DbCommand cmd = db.GetStoredProcCommand("test"))
            //{
            //    db.AddInParameter(cmd, "@Name", DbType.String, TestKLToBeSaved.Name);
            //    db.AddInParameter(cmd, "@Age", DbType.Int32, TestKLToBeSaved.Age);
            //    db.AddInParameter(cmd, "@Address", DbType.String, TestKLToBeSaved.Address);
            //    db.ExecuteScalar(cmd);
            //    returnvalue = true; 
            //}

            return returnvalue;
        }
    }
}
