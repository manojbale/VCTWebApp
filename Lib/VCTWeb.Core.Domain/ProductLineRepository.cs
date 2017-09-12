using System.Collections.Generic;

namespace VCTWeb.Core.Domain
{
    /// <summary>
    /// Summary description for ProductLineRepository
    /// </summary>
    
    public class ProductLineRepository
    {
        public List<ProductLine> FetchAllProductLine()
        {
            var listOfProductLine = new List<ProductLine>();
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.UspEppFetchAllProductLine))
            {
                using (var reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        listOfProductLine.Add(Load(reader));
                    }
                }
                
            }
            return listOfProductLine;
        }


        private ProductLine Load(SafeDataReader reader)
        {
            var newProductLine = new ProductLine
            {
                ProductLineName = reader.GetString("ProductLineName"),
                ProductLineDesc = reader.GetString("ProductLineDesc"),
                UpdatedBy = reader.GetString("UpdatedBy"),
                UpdatedOn = reader.GetDateTime("UpdatedOn")
            };
            return newProductLine;
        }

    }

}

