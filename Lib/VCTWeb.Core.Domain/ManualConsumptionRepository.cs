using System.Data;


namespace VCTWeb.Core.Domain
{
    /// <summary>
    /// Summary description for ManualConsumptionRepository
    /// </summary>
    public class ManualConsumptionRepository
    {
        public bool SaveManualConsumption(ManualConsumption theManualConsumption)
        {
            bool isSaved;
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.UspEppSaveManualConsumption))
            {
                db.AddInParameter(cmd, "@AccountNumber", DbType.String, theManualConsumption.AccountNumber);
                db.AddInParameter(cmd, "@TagId", DbType.String, theManualConsumption.TagId);
                db.AddInParameter(cmd, "@IsActive", DbType.Boolean, theManualConsumption.IsActive);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, theManualConsumption.UpdatedBy);
                isSaved = (db.ExecuteNonQuery(cmd) > 0);
            }
            return isSaved;
        }

        public bool IsManaulScanCompleted(string accountNumber, System.DateTime manualScanInitiatedAt)
        {
            bool isCompleted=false;
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.UspEppIsManuallyCompleted))
            {
                db.AddInParameter(cmd, "@AccountNumber", DbType.String, accountNumber);
                db.AddInParameter(cmd, "@ManualScanInitiatedAt", DbType.DateTime, manualScanInitiatedAt);
                object obj = db.ExecuteScalar(cmd);
                if (obj != null)
                    isCompleted = System.Convert.ToBoolean(obj);
            }
            return isCompleted;
        }

        public bool RevertManualConsumption(string accountNumber, string tagId, string updatedBy)
        {
            bool isSaved;
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.UspEppRevertManualConsumption))
            {
                db.AddInParameter(cmd, "@AccountNumber", DbType.String, accountNumber);
                db.AddInParameter(cmd, "@TagId", DbType.String, tagId);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, updatedBy);
                isSaved = (db.ExecuteNonQuery(cmd) > 0);
            }
            return isSaved;
        }
    }

}

