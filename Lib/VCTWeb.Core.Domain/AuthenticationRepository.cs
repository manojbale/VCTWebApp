
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Security.Cryptography;

using Microsoft.Practices.EnterpriseLibrary.Data;


namespace VCTWeb.Core.Domain
{

    /// <summary>
    /// the data layer class abstracts all data-base and resource file interactions from the
    /// rest of the code
    /// </summary>
    public class AuthenticationRepository
    {

        #region Public Methods

        /// <summary>
        /// checks whether this combination of username exists in the User table; we are
        /// case sensitive; 
        /// </summary>
        /// <param name="UserName">the user name</param>
        /// <param name="Password">the password</param>
        /// <returns>returns true if valid username & pwd</returns>
        public bool CheckUserName(string userName)
        {
            bool validUser = false;
            Object ReturnValue;
            Database db = DbHelper.CreateDatabase();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_CHECKUSERNAME))
            {
                db.AddInParameter(cmd, "@UserName", DbType.String, userName);
                ReturnValue = db.ExecuteScalar(cmd);
            }
            // if the return value from ExecuteScalar is null then we did not find the username & password
            validUser = (ReturnValue == null ? false : true);

            return validUser;
        }

        /// <summary>
        /// checks whether this combination of username & pwd exists in the User table; we are
        /// case sensitive; 
        /// </summary>
        /// <param name="UserName">the user name</param>
        /// <param name="Password">the password</param>
        /// <returns>returns true if valid username & pwd</returns>
        public bool CheckUserNameAndPassword(string userName, string password)
        {
            bool validUser = false;

            string hash = string.Empty;
            hash = RetrievePasswordHashForUserFromDatabase(userName);

            if (!string.IsNullOrEmpty(hash))
            {
                //Verify the password for the hash and emtpy salt.
                //SaltedHash saltedHash = new SaltedHash();
                //validUser = saltedHash.VerifyPassword(password, hash.Trim());
                validUser = MD5CryptorEngine.VerifyPassword(password, hash.Trim());

            }

            return validUser;
        }

        /// <summary>
        /// checks whether this combination of username & password hash exists in the User table
        /// </summary>
        /// <param name="UserName">the user name</param>
        /// <param name="Password">the password</param>
        /// <returns>returns true if valid username & password hash</returns>
        public bool CheckUserNameAndPasswordHash(string userName, string password_hash)
        {
            bool validUser = false;
            Object ReturnValue;
            Database db = DbHelper.CreateDatabase();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_CHECK_USERNAME_AND_PASSWORD))
            {
                db.AddInParameter(cmd, "@UserName", DbType.String, userName);
                db.AddInParameter(cmd, "@Password", DbType.String, password_hash);
                ReturnValue = db.ExecuteScalar(cmd);
            }

            // if the return value from ExecuteScalar is null then we did not find the username & password
            validUser = (ReturnValue == null ? false : true);

            return validUser;
        }

        /// <summary>
        /// Retrieves password hash of user from database.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>returns the hash value of password.</returns>
        public string RetrievePasswordHashForUserFromDatabase(string userName)
        {
            Object returnValue;
            Database db = DbHelper.CreateDatabase();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GET_PASSWORDVALUE_FOR_USERNAME))
            {
                db.AddInParameter(cmd, "@UserName", DbType.String, userName);
                returnValue = db.ExecuteScalar(cmd);
            }

            return Convert.ToString(returnValue);
        }

        /// <summary>
        /// reads all the security information for this user from the database; we separately return the
        /// unique list of role the user is belonging to plus a unique list of permission the
        /// user has; this information is used for the IPrincipal object
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="Roles">The roles.</param>
        /// <param name="Permissions">The permissions.</param>
        public void RetrieveSecurityInformation(string username, out List<Role> Roles, out List<Permission> Permissions)
        {
            Roles = (new RoleRepository()).FetchByUserName(username);
            Permissions = (new PermissionRepository()).FetchByUserName(username);
        }

        /// <summary>
        /// Retrieves the security information for active directory.
        /// </summary>
        /// <param name="adGroupList">The ad group list.</param>
        /// <param name="Roles">The roles.</param>
        /// <param name="Permissions">The permissions.</param>
        public void RetrieveSecurityInformationForActiveDirectory(string adGroupList, out List<Role> Roles, out List<Permission> Permissions)
        {
            Roles = (new RoleRepository()).FetchByRoles(adGroupList);
            Permissions = (new PermissionRepository()).FetchByRoles(adGroupList);
        }

        /// <summary>
        /// read the information associated with the user; this information is then used for the
        /// IIdentity object
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="UserInfo">the list of user information</param>
        public void RetrieveUserInformation(string username, out Users UserInfo)
        {
            UserInfo = (new UserRepository()).FetchUserByName(username);
        }

        #endregion

        #region Other

        /// <summary>
        /// delegate which can be used to pass along to ReadValuesIntoHashtable; if a delegate is provided
        /// then it is called otherwise ReadValuesIntoHashtable implements a default behavior how to add
        /// information to the NameValues hash-table
        /// </summary>
        private delegate void AddRecordInfo(Hashtable NameValues, SafeDataReader DataReader);

        #endregion

    }
}