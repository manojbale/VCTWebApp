using System.Collections.Generic;
using System.Data;

namespace VCTWeb.Core.Domain
{
    /// <summary>
    /// Summary description for OrderAdjustmentRepository
    /// </summary>
    
    public class OrderAdjustmentRepository
    {
        readonly string _user = string.Empty; 

        public OrderAdjustmentRepository()
        { 
        
        }

        public OrderAdjustmentRepository(string user)
        {
            _user = user;
        }

        public bool SaveOrderAdjustment(OrderAdjustment theOrderAdjustment)
        {
            bool isSaved;
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.usp_EppSaveOrderAdjustment))
            {
                db.AddInParameter(cmd, "@OrderId", DbType.Int32, theOrderAdjustment.OrderId);
                db.AddInParameter(cmd, "@DispositionTypeId", DbType.Int32, theOrderAdjustment.DispositionTypeId);
                db.AddInParameter(cmd, "@Remarks", DbType.String, theOrderAdjustment.Remarks);
                db.AddInParameter(cmd, "@Qty", DbType.Int16, theOrderAdjustment.Qty);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);
                isSaved = (db.ExecuteNonQuery(cmd) > 0);
            }
            return isSaved;
        }

        public List<OrderAdjustment> FetchAllOrderAdjustmentByOrderId(int orderId)
        {
            var listOfOrderAdjustment = new List<OrderAdjustment>();
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.UspEppFetchAllOrderAdjustmentByOrderId))
            {
                db.AddInParameter(cmd, "@OrderId", DbType.Int32, orderId);
                using (var  reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                        listOfOrderAdjustment .Add(Load(reader));
                }                
            }
            return listOfOrderAdjustment;
        }

        private OrderAdjustment Load(SafeDataReader reader)
        {
            var newOrderAdjustment = new OrderAdjustment
            {
                OrderAdjustmentId = reader.GetInt64("OrderAdjustmentId"),
                OrderId = reader.GetInt32("OrderId"),
                Qty = reader.GetInt16("Qty"),
                DispositionTypeId = reader.GetInt32("DispositionTypeId"),
                DispositionType = reader.GetString("DispositionType"),
                Remarks = reader.GetString("Remarks"),
                UpdatedBy = reader.GetString("UpdatedBy"),
                UpdatedOn = reader.GetDateTime("UpdatedOn")
            };
            return newOrderAdjustment;
        }
        
        public List<DispositionType> DispositionTypeList()
        {
            var db = DbHelper.CreateDatabase();
            var lstDispositionType = new List<DispositionType>();

            using (var cmd = db.GetStoredProcCommand(Constants.USP_GetDispositionTypesByCategory))
            {
                db.AddInParameter(cmd, "@DispositionCategory", DbType.String, "OrderAdjustment");
                using (var reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstDispositionType.Add(new DispositionType
                        {
                            DispositionTypeId = reader.GetInt32("DispositionTypeId"),
                            Disposition = reader.GetString("DispositionType"),
                        });
                    }

                }
                return lstDispositionType;
            }
        }

    }

}

