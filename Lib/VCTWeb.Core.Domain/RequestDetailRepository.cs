using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace VCTWeb.Core.Domain
{
    public class RequestDetailRepository
    {
        public void SaveRequestDetails(RequestDetail newRequestDetail)
        {
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_SAVEREQUESTDETAILS))
            {
                db.AddInParameter(cmd, "@KitNumber", DbType.String, newRequestDetail.KitNumber);
                db.AddInParameter(cmd, "@ProcedureName", DbType.String, newRequestDetail.ProcedureName);
                db.AddInParameter(cmd, "@CatalogNumber", DbType.String, newRequestDetail.CatalogNumber);
                db.AddInParameter(cmd, "@RequestDetailId", DbType.Int64, newRequestDetail.RequestDetailId);
                db.AddInParameter(cmd, "@RequestId", DbType.Int64, newRequestDetail.RequestId);
                db.ExecuteNonQuery(cmd);
            }
        }

        
    }
}
