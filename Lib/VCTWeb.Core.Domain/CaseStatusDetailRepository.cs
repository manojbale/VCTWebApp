using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace VCTWeb.Core.Domain
{
    public class CaseStatusDetailRepository
    {
        public List<CaseStatusDetail> FetchCaseStatusDetailById(long CaseId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<CaseStatusDetail> lstCaseStatusDetail = new List<CaseStatusDetail>();
            CaseStatusDetail newCaseStatusDetail = new CaseStatusDetail();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetCaseStatusDetailByCaseId))
            {
                db.AddInParameter(cmd, "@CaseId", DbType.Int64, CaseId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newCaseStatusDetail = LoadCaseStatusDetail(reader);
                        lstCaseStatusDetail.Add(newCaseStatusDetail);
                    }
                }
                return lstCaseStatusDetail;
            }
        }

        private CaseStatusDetail LoadCaseStatusDetail(SafeDataReader reader)
        {
            CaseStatusDetail newCaseStatusDetail = new CaseStatusDetail();
            newCaseStatusDetail.CaseStatusId = reader.GetInt64("CaseStatusId");
            newCaseStatusDetail.CaseId = reader.GetInt64("CaseId");
            newCaseStatusDetail.CaseStatus = reader.GetString("CaseStatus");
            newCaseStatusDetail.Description = reader.GetString("Description");
            newCaseStatusDetail.UpdatedBy = reader.GetString("UpdatedBy");
            newCaseStatusDetail.UpdatedOn = reader.GetLocalDateTime("UpdatedOn");

            return newCaseStatusDetail;
        }
    }
}
