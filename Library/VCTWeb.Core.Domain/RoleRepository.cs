using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;

namespace VCTWeb.Core.Domain
{

    /// <summary>
    /// This class provides the methods to fetch and update roles.
    /// </summary>
    public class RoleRepository
    {
        private string _user;

        public RoleRepository()
        {            
        }

        public RoleRepository(string user)
        {
            _user = user;
        }

        public virtual void Save(Role theRole)
        {

            if (theRole.IsModified || theRole.IsNew)
            {

                SaveRole(theRole);
            }
        }

        /// <summary>
        /// Saves the role.
        /// </summary>
        /// <param name="theRole">The role.</param>
        private  void SaveRole(Role theRole)
        {
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_SAVEROLE))
            {
                db.AddInParameter(cmd, "@RoleId", DbType.Int64, theRole.RoleId);
                db.AddInParameter(cmd, "@Description", DbType.String, theRole.Description);
                db.AddInParameter(cmd, "@RoleName", DbType.String, theRole.RoleName);
                db.AddInParameter(cmd, "@IsActive", DbType.Boolean, theRole.IsActive);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);
                db.ExecuteNonQuery(cmd);
                theRole.IsModified = false;
                theRole.IsNew = false;
            }
        }

        /// <summary>
        /// Fetches the Role of the by user name.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public  List<Role> FetchByUserName(string username)
        {
            SafeDataReader reader = null;
            Role newRole = null;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETLISTOFROLEBYUSERNAME))
            {
                db.AddInParameter(cmd, "@UserName", DbType.String, username);
                List<Role> listOfRole = new List<Role>();
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newRole = Load(reader);
                        listOfRole.Add(newRole);
                    }
                }
                return listOfRole;
            }
        }

        /// <summary>
        /// Fetches the Role of the by user name.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public List<Role> FetchRolesByUserName(string username)
        {
            SafeDataReader reader = null;
            Role newRole = null;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETLISTOFROLEBYUSERNAME))
            {
                db.AddInParameter(cmd, "@UserName", DbType.String, username);
                List<Role> listOfRole = new List<Role>();
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newRole = LoadRole(reader);
                        listOfRole.Add(newRole);
                    }
                }
                return listOfRole;
            }
        }

        /// <summary>
        /// Fetches the roles by active directory groups.
        /// </summary>
        /// <param name="adGroupList">The ad group list.</param>
        /// <returns>List of Role</returns>
        public  List<Role> FetchByRoles(string adGroupList)
        {
            List<Role> listOfRole = new List<Role>();

            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETLISTOFROLEBYADGROUPS))
            {
                db.AddInParameter(cmd, "@ADGroups", DbType.String, adGroupList);
                using (SafeDataReader reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {                    
                    while (reader.Read())
                    {
                        listOfRole.Add(Load(reader));
                    }                    
                }                
            }

            return listOfRole;
        }

        /// <summary>
        /// Fetches all Roles.
        /// </summary>
        /// <returns></returns>
        public List<Role> FetchAll()
        {
            SafeDataReader reader = null;
            Role newRole = null;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GET_ROLES))
            {
                List<Role> listOfRole = new List<Role>();
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newRole = Load(reader);
                        listOfRole.Add(newRole);
                    }
                }
                return listOfRole;
            }
        }

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="roleId">The role id.</param>
        public void DeleteRole(Int64 roleId)
        {
            Database db = DbHelper.CreateDatabase();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_DELETE_ROLE_BY_ROLEID))
            {
                db.AddInParameter(cmd, "@RoleId", DbType.Int64, roleId);

                db.ExecuteNonQuery(cmd);
            }
        } 

        /// <summary>
        /// Loads the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        private  Role Load(SafeDataReader reader)
        {
            Role newRole = new Role();
            newRole.RoleId = reader.GetInt64("RoleId");
            newRole.Description = reader.GetString("Description");
            newRole.RoleName = reader.GetString("RoleName");
            newRole.IsActive = reader.GetBoolean("IsActive");
            newRole.IsModified = false;
            newRole.IsNew = false;
            return newRole;
        }

        private Role LoadRole(SafeDataReader reader)
        {
            Role newRole = new Role();
            newRole.RoleId = reader.GetInt64("RoleId");
            newRole.Description = reader.GetString("Description");
            newRole.RoleName = reader.GetString("RoleName");
            newRole.IsActive = reader.GetBoolean("IsActive");
            newRole.IsModified = false;
            newRole.IsNew = false;
            return newRole;
        }
        
    }
}
