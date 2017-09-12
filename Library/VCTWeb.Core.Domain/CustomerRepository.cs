using System;
using System.Collections.Generic;
using System.Data;

namespace VCTWeb.Core.Domain
{
    [CLSCompliant(true)]
    public class CustomerRepository
    {
        #region Member Variables

        private readonly string _user;

        #endregion Member Variables

        #region Constructor

        public CustomerRepository()
        {
        }

        public CustomerRepository(string user)
        {
            _user = user;
        }

        #endregion

        #region Public methods

        public Customer FetchCustomerByAccountNumber(string accountNumber)
        {
            Customer newCustomer = null;
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.USP_EppGetCustomerByAccountNumber))
            {
                db.AddInParameter(cmd, "@AccountNumber", DbType.String, accountNumber);
                using (var reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newCustomer = LoadCustomer(reader);
                        break;
                    }
                }
                return newCustomer;
            }
        }

        public List<Customer> FetchAllCustomer(bool isAllCustomers, string loginUserName = null)
        {
            var lstCustomer = new List<Customer>();
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.usp_EppFetchListOfFilteredCustomers))
            {
                if (!isAllCustomers)
                    db.AddInParameter(cmd, "@IsActive", DbType.Int16, 1);
                if (!string.IsNullOrEmpty(loginUserName))
                    db.AddInParameter(cmd, "@LoginUserName", DbType.String, loginUserName);
                using (var reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstCustomer.Add(LoadCustomer(reader));
                    }
                }
                return lstCustomer;
            }
        }

        public List<Customer> FetchAllFilteredCustomer(string customerName, string accountNumber, string ownershipStructure, string managementStructure, string spineOnlyMultiSpecialty, string branchAgency, string manager, string salesRepresentative, bool isActive)
        {

            var lstCustomer = new List<Customer>();
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.usp_EppFetchListOfFilteredCustomers))
            {
                db.AddInParameter(cmd, "@CustomerName", DbType.String, customerName);
                db.AddInParameter(cmd, "@AccountNumber", DbType.String, accountNumber);
                db.AddInParameter(cmd, "@OwnershipStructure", DbType.String, ownershipStructure);
                db.AddInParameter(cmd, "@ManagementStructure", DbType.String, managementStructure);
                db.AddInParameter(cmd, "@SpineOnlyMultiSpecialty", DbType.String, spineOnlyMultiSpecialty);
                db.AddInParameter(cmd, "@BranchAgency", DbType.String, branchAgency);
                db.AddInParameter(cmd, "@Manager", DbType.String, manager);
                db.AddInParameter(cmd, "@SalesRepresentative", DbType.String, salesRepresentative);
                db.AddInParameter(cmd, "@IsActive", DbType.Int16, isActive);

                using (var reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstCustomer.Add(LoadCustomer(reader));
                    }
                }
                return lstCustomer;
            }
        }

        public List<Customer> FetchCustomerForReportFilterDropdown()
        {
            var db = DbHelper.CreateDatabase();
            var lstDropdownData = new List<Customer>();
            using (var cmd = db.GetStoredProcCommand(Constants.Usp_EppFetchCustomerForReportFilterDropdown))
            {
                using (var reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstDropdownData.Add(LoadCustomer(reader));
                    }
                }
                return lstDropdownData;
            }
        }

        public List<CustomerProductLine> FetchCustomerProductLineByProductLine(string productLineName)
        {
            var listCustomerProductLine = new List<CustomerProductLine>();
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.usp_EppFetchCustomerProductLineByProductLine))
            {
                db.AddInParameter(cmd, "@ProductLineName", DbType.String, productLineName);
                using (var reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        listCustomerProductLine.Add(LoadCustomerProductLine(reader));
                    }
                }
            }
            return listCustomerProductLine;
        }

        public List<CustomerProductLine> FetchAllCustomerProductLine()
        {
            var listCustomerProductLine = new List<CustomerProductLine>();
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.Usp_EppFetchAllCustomerProductLine))
            {
                using (var reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                        listCustomerProductLine.Add(LoadProductLineCustomer(reader));
                }
            }
            return listCustomerProductLine;
        }

        public bool SaveCustomer(Customer customer, string xmlString)
        {
            bool isSaved;
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.usp_EppSaveCustomer))
            {
                db.AddInParameter(cmd, "@CustomerName", DbType.String, customer.CustomerName);
                db.AddInParameter(cmd, "@AccountNumber", DbType.String, customer.AccountNumber);
                db.AddInParameter(cmd, "@StreetAddress", DbType.String, customer.StreetAddress);
                db.AddInParameter(cmd, "@City", DbType.String, customer.City);
                db.AddInParameter(cmd, "@State", DbType.String, customer.State);
                db.AddInParameter(cmd, "@Zip", DbType.String, customer.Zip);
                db.AddInParameter(cmd, "@OwnershipStructure", DbType.String, customer.OwnershipStructure);
                db.AddInParameter(cmd, "@ManagementStructure", DbType.String, customer.ManagementStructure);
                db.AddInParameter(cmd, "@SpineOnlyMultiSpecialty", DbType.String, customer.SpineOnlyMultiSpecialty);
                if (customer.QtyOfORs == null)
                    db.AddInParameter(cmd, "@QtyOfORs", DbType.Int32, DBNull.Value);
                else
                    db.AddInParameter(cmd, "@QtyOfORs", DbType.Int32, customer.QtyOfORs);

                db.AddInParameter(cmd, "@BranchAgency", DbType.String, customer.BranchAgency);
                db.AddInParameter(cmd, "@Manager", DbType.String, customer.Manager);
                db.AddInParameter(cmd, "@SalesRepresentative", DbType.String, customer.SalesRepresentative);
                db.AddInParameter(cmd, "@SpecialistRep", DbType.String, customer.SpecialistRep);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);
                db.AddInParameter(cmd, "@IsActive", DbType.Boolean, customer.IsActive);

                db.AddInParameter(cmd, "@ConsumptionInterval", DbType.Int32, customer.ConsumptionInterval);
                db.AddInParameter(cmd, "@AssetNearExpiryDays", DbType.Int32, customer.AssetNearExpiryDays);

                db.AddInParameter(cmd, "@ProductLines", DbType.String, customer.ProductLines);

                if (!string.IsNullOrEmpty(xmlString))
                    db.AddInParameter(cmd, "@XMLString", DbType.String, xmlString);

                isSaved = (db.ExecuteNonQuery(cmd) > 0);
            }
            return isSaved;
        }

        public CustomerUser FetchUserForCustomer(string userType, string accountNumber)
        {
            CustomerUser customerUser;
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand(Constants.Usp_EppFetchUserDetailForAccountNumber))
            {
                db.AddInParameter(cmd, "@UserType", DbType.String, userType);
                db.AddInParameter(cmd, "@AccountNumber", DbType.String, accountNumber);
                using (var reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    customerUser = reader.Read() ? LoadCustomerUser(reader) : new CustomerUser();
                }
            }
            return customerUser;
        }

        #endregion Public methods

        #region Private methods

        private CustomerUser LoadCustomerUser(SafeDataReader reader)
        {
            var customerUser = new CustomerUser
            {
                EmailID = reader.GetString("EmailID"),
                FullName = reader.GetString("FullName"),
                UserName = reader.GetString("UserName")
            };
            return customerUser;
        }

        private CustomerProductLine LoadProductLineCustomer(SafeDataReader reader)
        {
            var newCustomerProductLine = new CustomerProductLine
            {
                AccountNumber = reader.GetString("AccountNumber"),
                ProductLineName = reader.GetString("ProductLineName").Trim(),
                CustomerName = reader.GetString("CustomerName"),
                NameAccount = reader.GetString("NameAccount")
            };
            return newCustomerProductLine;
        }

        private CustomerProductLine LoadCustomerProductLine(SafeDataReader reader)
        {
            var newCustomerProductLine = new CustomerProductLine
            {
                ProductLineName = reader.GetString("ProductLineName").Trim(),
                ProductLineDesc = reader.GetString("ProductLineDesc").Trim(),
                Selected = reader.GetBoolean("Selected")
            };
            return newCustomerProductLine;
        }

        private Customer LoadCustomer(SafeDataReader reader)
        {
            var newCustomer = new Customer
            {
                CustomerName = reader.GetString("CustomerName").Trim(),
                AccountNumber = reader.GetString("AccountNumber").Trim(),
                BranchAgency = reader.GetString("BranchAgency").Trim(),
                IsActive = reader.GetBoolean("IsActive"),
                City = reader.GetString("City").Trim(),
                SalesRepresentative = reader.GetString("SalesRepresentative").Trim(),
                SpecialistRep = reader.GetString("SpecialistRep").Trim(),
                ManagementStructure = reader.GetString("ManagementStructure").Trim(),
                Manager = reader.GetString("Manager").Trim(),
                OwnershipStructure = reader.GetString("OwnershipStructure").Trim(),
                QtyOfORs = reader.GetInt32("QtyOfORs"),
                SpineOnlyMultiSpecialty = reader.GetString("SpineOnlyMultiSpecialty").Trim(),
                State = reader.GetString("State").Trim(),
                StreetAddress = reader.GetString("StreetAddress").Trim(),
                UpdatedBy = reader.GetString("UpdatedBy").Trim(),
                UpdatedOn = reader.GetDateTime("UpdatedOn"),
                Zip = reader.GetString("Zip").Trim(),
                ConsumptionInterval = reader.GetInt16("ConsumptionInterval_Mins"),
                AssetNearExpiryDays = reader.GetInt16("AssetNearExpiryDays"),
                NameAccount = reader.GetString("NameAccount").Trim()
            };
            return newCustomer;
        }

        #endregion Private methods
    }
}
