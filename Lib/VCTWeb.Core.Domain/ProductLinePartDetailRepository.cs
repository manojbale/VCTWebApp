using System.Collections.Generic;
using System.Data;

namespace VCTWeb.Core.Domain
{
    public class ProductLinePartDetailRepository
    {
        public bool UpdateParLevelQuantityForRefNum(string refNum, int parLevelQty)
        {
            bool returnvalue;
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.Usp_EppUpdateParLevelQuantityForRefNum))
            {
                db.AddInParameter(cmd, "@RefNum", DbType.String, refNum);                
                db.AddInParameter(cmd, "@PARLevelQty", DbType.Int32, parLevelQty);
                returnvalue = (db.ExecuteNonQuery(cmd) > 0);                
            }
            return returnvalue;
           
        }

        public List<ProductLinePartDetail> FetchAllProductLinePartDetail()
        {
            var listOfProductLinePartDetail = new List<ProductLinePartDetail>();
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.UspEppFetchAllProductLinePartDetail))
            {
                using (var reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        listOfProductLinePartDetail.Add(Load(reader));
                    }
                }
                return listOfProductLinePartDetail;
            }
        }

        public List<PartCatalog> FetchPartCatalogByPartNum(string refNum)
        {
            var db = DbHelper.CreateDatabase();
            var lstCatalog = new List<PartCatalog>();
            using (var cmd = db.GetStoredProcCommand(Constants.Usp_EppFetchPartCatalogByPartNum))
            {
                db.AddInParameter(cmd, "@RefNum", DbType.String, refNum);
                using (var reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstCatalog.Add(LoadPartCatalog(reader));
                    }
                }
                return lstCatalog;
            }
        }

        private PartCatalog LoadPartCatalog(SafeDataReader reader)
        {
            var newCatalog = new PartCatalog
            {
                RefNum = reader.GetString("RefNum"),
                Description = reader.GetString("Description"),
                CatalogFull = reader.GetString("CatalogFull")
            };
            return newCatalog;
        }

        private ProductLinePartDetail Load(SafeDataReader reader)
        {
            var newProductLinePartDetail = new ProductLinePartDetail
            {
                ProductLineName = reader.GetString("ProductLineName"),
                ProductLineDesc = reader.GetString("ProductLineDesc"),
                RefNum = reader.GetString("RefNum"),
                Category = reader.GetString("Category"),
                SubCategory1 = reader.GetString("SubCategory1"),
                SubCategory2 = reader.GetString("SubCategory2"),
                SubCategory3 = reader.GetString("SubCategory3"),
                Size = reader.GetString("Size"),
                DefaultPARLevel = reader.GetInt16("DefaultPARLevel")
            };

            var partType = string.Empty;
            if (!string.IsNullOrEmpty(newProductLinePartDetail.Category))
                partType = newProductLinePartDetail.Category;

            if (!string.IsNullOrEmpty(newProductLinePartDetail.SubCategory1))
                partType += ", " + newProductLinePartDetail.SubCategory1;

            if (!string.IsNullOrEmpty(newProductLinePartDetail.SubCategory2))
                partType += ", " + newProductLinePartDetail.SubCategory2;

            if (!string.IsNullOrEmpty(newProductLinePartDetail.SubCategory3))
                partType += ", " + newProductLinePartDetail.SubCategory3;

            newProductLinePartDetail.PartType = partType;

            return newProductLinePartDetail;
        }
    }
}
