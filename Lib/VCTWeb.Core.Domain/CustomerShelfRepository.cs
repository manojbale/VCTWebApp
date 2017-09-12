using System.Collections.Generic;
using System.Data;

namespace VCTWeb.Core.Domain
{
    public class CustomerShelfRepository
    {
        #region Methods to fetch customer Shelf details.

        public List<CustomerShelf> FetchCustomerShelfForDashBoard()
        {
            var listOfCustomerShelf = new List<CustomerShelf>();
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.Usp_EppFetchCustomerShelfForDashBoard))
            {
                using (var reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        listOfCustomerShelf.Add(LoadCustomerShelf(reader));
                    }
                }
            }
            return listOfCustomerShelf;
        }

        public CustomerShelf FetchCustomerShelfByCustomerShelfId(int customerShelfId)
        {
            CustomerShelf customerShelf = null;
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.Usp_FetchCustomerShelfByCustomerShelfId))
            {
                db.AddInParameter(cmd, "@CustomerShelfId", DbType.Int32, customerShelfId);
                using (var reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    if (reader.Read())
                    {
                        customerShelf = LoadCustomerShelf1(reader);
                    }
                }
            }
            return customerShelf;
        }

        private CustomerShelf LoadCustomerShelf(SafeDataReader reader)
        {
            var newCustomerShelf = new CustomerShelf
            {
                CustomerShelfId = reader.GetInt32("CustomerShelfId"),
                AccountNumber = reader.GetString("AccountNumber"),
                CustomerName = reader.GetString("CustomerName"),
                ShelfCode = reader.GetString("ShelfCode"),
                ShelfName = reader.GetString("ShelfName"),
                ReaderIP = reader.GetString("ReaderIP"),
                ReaderName = reader.GetString("ReaderName"),
                ReaderPort = reader.GetString("ReaderPort"),
                ReaderStatus = reader.GetString("ReaderStatus"),
                ReaderHealthLastUpdatedOn = reader.GetNullableDateTime("ReaderHealthLastUpdatedOn")
            };
            return newCustomerShelf;
        }
                
        private CustomerShelf LoadCustomerShelf1(SafeDataReader reader)
        {
            var newCustomerShelf = new CustomerShelf
            {
                CustomerShelfId = reader.GetInt32("CustomerShelfId"),
                AccountNumber = reader.GetString("AccountNumber"),
                ShelfCode = reader.GetString("ShelfCode"),
                ShelfName = reader.GetString("ShelfName"),
                ReaderIP = reader.GetString("ReaderIP"),
                ReaderName = reader.GetString("ReaderName"),
                ReaderPort = reader.GetString("ReaderPort"),
                ReaderHealthLastUpdatedOn = reader.GetNullableDateTime("ReaderHealthLastUpdatedOn")
            };


            return newCustomerShelf;
        }

        #endregion Methods to fetch customer Shelf details.
        
        #region Methods to fetch Reader Properties

        public List<CustomerShelfProperty> FetchCustomerShelfPropertyByCustomerShelfId(int customerShelfId)
        {
            var listOfCustomerShelfProperty = new List<CustomerShelfProperty>();
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.Usp_EppFetchCustomerShelfPropertyByCustomerShelfId))
            {
                db.AddInParameter(cmd, "@CustomerShelfId", DbType.Int32, customerShelfId);
                using (var reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        listOfCustomerShelfProperty.Add(LoadCustomerShelfProperty(reader));
                    }
                }
            }
            return listOfCustomerShelfProperty;
        }

        private CustomerShelfProperty LoadCustomerShelfProperty(SafeDataReader reader)
        {
            var newCustomerShelfProperty = new CustomerShelfProperty
            {
                CustomerShelfId = reader.GetInt32("CustomerShelfId"),
                ReaderPropertyId = reader.GetInt32("ReaderPropertyId"),
                PropertyName = reader.GetString("PropertyName"),
                PropertyDescription = reader.GetString("PropertyDescription"),
                PropertyValue = reader.GetString("PropertyValue"),
                LastUpdatedOn = reader.GetNullableDateTime("LastUpdatedOn"),
                ModifiedPropertyValue = reader.GetString("ModifiedPropertyValue"),
                HasModified = reader.GetBoolean("HasModified"),
                ModifiedOn = reader.GetNullableDateTime("ModifiedOn"),
                DataType = reader.GetString("DataType"),
                ListValues = reader.GetString("ListValues"),
                IsEditable = reader.GetBoolean("IsEditable")
            };
            return newCustomerShelfProperty;
        }

        #endregion Methods to fetch Reader Properties

        #region Methods to fetch Reader Antenna Properties

        public List<CustomerShelfAntennaProperty> FetchCustomerShelfAntennaPropertyByCustomerShelfAntennaId(int customerShelfAntennaId)
        {
            var listOfCustomerShelfAntennaProperty = new List<CustomerShelfAntennaProperty>();
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.Usp_EppFetchCustomerShelfAntennaPropertyByCustomerShelfAntennaId))
            {
                db.AddInParameter(cmd, "@CustomerShelfAntennaId", DbType.Int32, customerShelfAntennaId);
                using (var reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        listOfCustomerShelfAntennaProperty.Add(LoadCustomerShelfAntennaProperty(reader));
                    }
                }
            }
            return listOfCustomerShelfAntennaProperty;
        }
        
        public List<CustomerShelfAntenna> FetchDistinctAntennaByCustomerShelfId(int customerShelfId)
        {
            var listDistinctAntenna = new List<CustomerShelfAntenna>();
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.Usp_EppFetchDistinctAntennaByCustomerShelfId))
            {
                db.AddInParameter(cmd, "@CustomerShelfId", DbType.Int32, customerShelfId);
                using (var reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        listDistinctAntenna.Add(LoadDistinctCustomerShelfAntenna(reader));
                    }
                }
            }
            return listDistinctAntenna;
        }

        private CustomerShelfAntennaProperty LoadCustomerShelfAntennaProperty(SafeDataReader reader)
        {
            var newCustomerShelfAntennaProperty = new CustomerShelfAntennaProperty
            {
                CustomerShelfAntennaId = reader.GetInt32("CustomerShelfAntennaId"),
                AntennaPropertyId = reader.GetInt32("AntennaPropertyId"),
                PropertyNameAntenna = reader.GetString("PropertyName"),
                PropertyDescriptionAntenna = reader.GetString("PropertyDescription"),
                PropertyValueAntenna = reader.GetString("PropertyValue"),
                LastUpdatedOnAntenna = reader.GetNullableDateTime("LastUpdatedOn"),
                ModifiedPropertyValueAntenna = reader.GetString("ModifiedPropertyValue"),
                HasModifiedAntenna = reader.GetBoolean("HasModified"),
                ModifiedOnAntenna = reader.GetNullableDateTime("ModifiedOn"),
                DataTypeAntenna = reader.GetString("DataType"),
                ListValuesAntenna = reader.GetString("ListValues"),
                IsEditableAntenna = reader.GetBoolean("IsEditable"),
                MaximumLengthAntenna = reader.GetInt32("MaximumLength")
            };

            return newCustomerShelfAntennaProperty;

        }

        private CustomerShelfAntenna LoadDistinctCustomerShelfAntenna(SafeDataReader reader)
        {
            var newCustomerShelfAntennaProperty = new CustomerShelfAntenna
            {
                CustomerShelfAntennaId = reader.GetInt32("CustomerShelfAntennaId"),
                CustomerShelfId = reader.GetInt32("CustomerShelfId"),
                AntennaName = reader.GetString("AntennaName"),
                AntennaDescription = reader.GetString("AntennaDescription")
            };
            return newCustomerShelfAntennaProperty;
        }

        #endregion Methods to fetch Reader Antenna Properties

        #region Methods to Update Reader & Antenna Property Via Dashboard

        public bool SaveModifiedReaderAntennaValues(string locationCode, string shelfCode, string readerPropertyXml, string antennaPropertyXml)
        {
            bool isSaved;
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.Usp_EppSaveModifiedReaderAntennaValues))
            {
                db.AddInParameter(cmd, "@LocationCode", DbType.String, locationCode);
                db.AddInParameter(cmd, "@ShelfCode", DbType.String, shelfCode);
                db.AddInParameter(cmd, "@ReaderPropertyXml", DbType.String, readerPropertyXml);
                db.AddInParameter(cmd, "@AntennaPropertyXml", DbType.String, antennaPropertyXml);
                isSaved = (db.ExecuteNonQuery(cmd) > 0);
            }
            return isSaved;
        }
        
        #endregion Methods to Update Reader Property Via Dashboard

    }
}
