using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Globalization;

namespace VCTWeb.Core.Domain
{

    public class UserRepository
    {
        private string _user;

        public UserRepository()
        {
        }

        public UserRepository(string user)
        {
            _user = user;
        }

        #region Public Methods

        /// <summary>
        /// Saves the user.
        /// </summary>
        /// <param name="theUser">The user.</param>
        /// <param name="mode">The mode.</param>
        public void SaveUserAndUserRole(Users theUser, string mode, bool defaultPass, string roleMembershipXmlString)
        {
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_SAVE_USER_AND_ROLE_MEMEBERSHIP))
            {
                //SaltedHash saltedHash = new SaltedHash();
                string hash = string.Empty;
                if (defaultPass == true)
                {
                    string defaultPassword = new ConfigurationRepository().GetConfigurationKeyValue("DefaultPassword");
                    //hash = saltedHash.EncryptPassword(defaultPassword.Trim());
                    hash = MD5CryptorEngine.Encrypt(defaultPassword.Trim(),true);
                }
                else
                    hash = null;
                db.AddInParameter(cmd, "@UserName", DbType.String, theUser.UserName);
                db.AddInParameter(cmd, "@FirstName", DbType.String, theUser.FirstName);
                db.AddInParameter(cmd, "@LastName", DbType.String, theUser.LastName);
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, theUser.LocationId);
                db.AddInParameter(cmd, "@SecurityQuestion", DbType.String, theUser.SecurityQuestion);
                db.AddInParameter(cmd, "@SecurityAnswer", DbType.String, theUser.SecurityAnswer);
                db.AddInParameter(cmd, "@IsSystemUser", DbType.Boolean, theUser.IsSystemUser);
                db.AddInParameter(cmd, "@IsDomainUser", DbType.Boolean, theUser.IsDomainUser);
                db.AddInParameter(cmd, "@Domain", DbType.String, theUser.Domain);
                db.AddInParameter(cmd, "@IsActive", DbType.Boolean, theUser.IsActive);
                db.AddInParameter(cmd, "@Password", DbType.String, hash);
                db.AddInParameter(cmd, "@EmailID", DbType.String, theUser.EmailID);
                db.AddInParameter(cmd, "@Phone", DbType.String, theUser.Phone);
                db.AddInParameter(cmd, "@Cell", DbType.String, theUser.Cell);
                db.AddInParameter(cmd, "@Fax", DbType.String, theUser.Fax);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);
                db.AddInParameter(cmd, "@Mode", DbType.String, mode);
                db.AddInParameter(cmd, "@RoleMembershipXmlString", DbType.String, roleMembershipXmlString);
                db.ExecuteNonQuery(cmd);
                theUser.IsModified = false;
                theUser.IsNew = false;
            }
        }

        /// <summary>
        /// Deletes the user role.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        public void DeleteUserRole(string userName)
        {

            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_DELETEROLEBYUSERNAME))
            {
                db.AddInParameter(cmd, "@UserName", DbType.String, userName);
                db.ExecuteNonQuery(cmd);
            }
        }

        /// <summary>
        /// Saves the user role.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="roleList">The role list.</param>
        public void SaveUserRole(string userName, List<Role> roleList)
        {
            Database db = DbHelper.CreateDatabase();
            foreach (Role role in roleList)
            {
                if (role.RoleId != 0)
                {
                    using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_SAVEROLEMEMBERSHIP))
                    {
                        db.AddInParameter(cmd, "@UserName", DbType.String, userName);
                        db.AddInParameter(cmd, "@RoleId", DbType.Int64, role.RoleId);
                        db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);
                        db.ExecuteNonQuery(cmd);
                    }
                }

            }
        }

        /// <summary>
        /// Saves the user role.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="roleList">The role list.</param>
        public void UpdateUserPassword(string userName,string password)
        {
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_UPDATE_USER_PASSWORD))
            {
                //SaltedHash saltedHash = new SaltedHash();
                string hash = string.Empty;

                if (!string.IsNullOrEmpty(password))
                {
                   // hash = saltedHash.EncryptPassword(password.Trim());
                    hash = MD5CryptorEngine.Encrypt(password.Trim(),true);
                }

                db.AddInParameter(cmd, "@UserName", DbType.String, userName);
                db.AddInParameter(cmd, "@Password", DbType.String, hash);

                db.ExecuteNonQuery(cmd);
            }
        }

        /// <summary>
        /// Saves the user role.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="roleList">The role list.</param>
        public void UpdateUserPassword(string password)
        {
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_UPDATE_USER_PASSWORD))
            {
                //SaltedHash saltedHash = new SaltedHash();
                string hash = string.Empty;

                if (!string.IsNullOrEmpty(password))
                {
                    //hash = saltedHash.EncryptPassword(password.Trim());
                    hash = MD5CryptorEngine.Encrypt(password.Trim(),true);
                }

                db.AddInParameter(cmd, "@UserName", DbType.String, _user);
                db.AddInParameter(cmd, "@Password", DbType.String, hash);

                db.ExecuteNonQuery(cmd);
            }
        }

        public Users FetchUserByName(string username)
        {
            SafeDataReader reader = null;
            Users newUser = null;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETUSERINFORMATION))
            {
                db.AddInParameter(cmd, "@UserName", DbType.String, username);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    if (reader.Read())
                    {
                        newUser = LoadUser(reader);
                    }
                    else
                    {
                        newUser = new Users();
                    }

                }
                return newUser;
            }
        }

        /// <summary>
        /// Fetches all users.
        /// </summary>
        /// <returns></returns>
        public List<Users> FetchAllUsers()
        {
            SafeDataReader reader = null;
            Users newUser = null;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETLISTOFUSERS))
            {
                List<Users> listOfUser = new List<Users>();
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newUser = this.LoadUser(reader);
                        listOfUser.Add(newUser);
                    }

                }
                return listOfUser;
            }
        }

        
        public List<Users> GetListOfSalesRepByLocationId(int locationId)
        {
            SafeDataReader reader = null;
            Users newUser = null;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetListOfSalesRepByLocationId))
            {
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                List<Users> listOfUser = new List<Users>();
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newUser = this.LoadUser(reader);
                        listOfUser.Add(newUser);
                    }

                }
                return listOfUser;
            }
        }


        public List<Users> FetchListOfSystemUsers()
        {
            SafeDataReader reader = null;
            Users newUser = null;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.Usp_EppGetListOfSystemUsers))
            {
                List<Users> listOfUser = new List<Users>();
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newUser = this.LoadUser(reader);
                        listOfUser.Add(newUser);
                    }

                }
                return listOfUser;
            }
        }


        public List<Users> FetchListOfSystemUsersByRoleName(string roleName)
        {
            SafeDataReader reader = null;
            Users newUser = null;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.Usp_EppGetListOfSystemUsersByRoleName))
            {
                db.AddInParameter(cmd, "@RoleName", DbType.String, roleName);
                List<Users> listOfUser = new List<Users>();
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newUser = this.LoadUser(reader);
                        listOfUser.Add(newUser);
                    }

                }
                return listOfUser;
            }
        }

        /// <summary>
        /// Deletes the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        public void Delete(string username)
        {
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_DELETEUSER))
            {
                db.AddInParameter(cmd, "@UserName", DbType.String, username);
                db.ExecuteNonQuery(cmd);
            }
        }


        public void InsertLogggingDetails(string ipAddress, string userName, bool isSuccess)
        {
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_INSERT_LOGGGING_DETAILS))
            {
                db.AddInParameter(cmd, "@IPAddress", DbType.String, ipAddress);
                db.AddInParameter(cmd, "@UserName", DbType.String, userName);
                db.AddInParameter(cmd, "@IsSuccess", DbType.Boolean, isSuccess);
                db.ExecuteNonQuery(cmd);
            }
        }

        public string GetUserRoleByUserName(string userName)
        {
            object obj = null;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETUSERROLEBYUSERNAME))
            {
                db.AddInParameter(cmd, "@UserName", DbType.String, userName);
                obj = db.ExecuteScalar(cmd);
            }
            if (obj != null)
                return obj.ToString();
            else
                return string.Empty;
        }

        public bool VerifySecurityQuestionAnswer(string userName, string securityQuestion, string securityAnswer)
        {
            object obj = null;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_VERIFYSECURITYQUESTIONANSWER))
            {
                db.AddInParameter(cmd, "@UserName", DbType.String, userName);
                db.AddInParameter(cmd, "@SecurityQuestion", DbType.String, securityQuestion);
                db.AddInParameter(cmd, "@SecurityAnswer", DbType.String, securityAnswer);
                obj = db.ExecuteScalar(cmd);
            }
            if (obj != null && System.Convert.ToInt16(obj) > 0)
                return true;
            else
                return false;
        }

        #endregion

        #region Private Methods


        private Users LoadUser(SafeDataReader reader)
        {
            Users newUser = new Users();
            newUser.UserName = reader.GetString("UserName").Trim();
            newUser.FullName = reader.GetString("FullName").Trim();
            newUser.FirstName = reader.GetString("FirstName").Trim();
            newUser.LastName = reader.GetString("LastName").Trim();
            newUser.SecurityQuestion = reader.GetString("SecurityQuestion").Trim();
            newUser.SecurityAnswer = reader.GetString("SecurityAnswer").Trim();
            newUser.LocationId = reader.GetInt32("LocationId");
            newUser.Location = reader.GetString("Location").Trim();
            newUser.LocationType = reader.GetString("LocationType").Trim();
            newUser.IsSystemUser = reader.GetBoolean("IsSystemUser");
            newUser.IsDomainUser = reader.GetBoolean("IsDomainUser");
            newUser.Domain = reader.GetString("Domain").Trim();
            newUser.IsActive = reader.GetBoolean("IsActive");
            newUser.Password = reader.GetString("Password").Trim();
            newUser.EmailID = reader.GetString("EmailID").Trim();
            newUser.Phone = reader.GetString("Phone").Trim();
            newUser.Cell = reader.GetString("Cell").Trim();
            newUser.Fax = reader.GetString("Fax").Trim();
            newUser.LastPasswordDate = reader.GetNewNullableDateTime("LastPasswordDate");
            newUser.IsModified = false;
            newUser.IsNew = false;
            return newUser;
        }

        #endregion






        public bool CheckInUseUser(string userId)
        {
            Database db = DbHelper.CreateDatabase();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_CheckInUseUser))
            {
                db.AddInParameter(cmd, "@userId", DbType.String, userId);
                int count = Convert.ToInt32(db.ExecuteScalar(cmd), CultureInfo.InvariantCulture);

                if (count > 0)
                    return true;
            }

            return false;
        }
    }
}
