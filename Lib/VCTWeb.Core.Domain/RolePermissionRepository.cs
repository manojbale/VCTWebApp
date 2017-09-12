using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Data;


namespace VCTWeb.Core.Domain
{
    /// <summary>
    /// Role Permission Repository Classs
    /// </summary>
    public class RolePermissionRepository
    {
        private string _user;

        public RolePermissionRepository(string user)
        {
            _user = user;
        }
        #region Public Methods
        /// <summary>
        /// Gets the role permission list.
        /// </summary>
        /// <returns></returns>
        public List<RolePermission> GetRolePermissionList()
        {
            List<RolePermission> rolePermissionList = new List<RolePermission>();
            Database db = DbHelper.CreateDatabase();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GET_ROLE_PERMISSIONS))
            {
                using (SafeDataReader reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        RolePermission rolePermission = new RolePermission();
                        rolePermission = LoadRolePermission(reader);

                        rolePermissionList.Add(rolePermission);
                    }
                }
            }
            return rolePermissionList;
        }

        /// <summary>
        /// Gets the role permissions by role id.
        /// </summary>
        /// <param name="roleId">The role id.</param>
        /// <returns></returns>
        public List<RolePermission> GetRolePermissionsByRoleId(Int64 roleId)
        {
            List<RolePermission> rolePermissionList = new List<RolePermission>();
            Database db = DbHelper.CreateDatabase();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GET_ROLE_PERMISSIONS_BY_ROLEID))
            {
                db.AddInParameter(cmd, "@RoleId", DbType.Int64, roleId);
                using (SafeDataReader reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        RolePermission rolePermission = new RolePermission();
                        rolePermission = LoadRolePermission(reader);

                        rolePermissionList.Add(rolePermission);
                    }
                }
            }
            return rolePermissionList;
        }

        /// <summary>
        /// Saves the role permissions.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="rolePermissionList">The role permission list.</param>
        public void SaveRolePermissions(Role role, List<RolePermission> rolePermissionList)
        {
            Database db = DbHelper.CreateDatabase();
            Int64 roleId = 0;
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_SAVEROLE))
            {
                db.AddInParameter(cmd, "@RoleId", DbType.Int64, role.RoleId);
                db.AddInParameter(cmd, "@RoleName", DbType.String, role.RoleName.Trim());
                db.AddInParameter(cmd, "@Description", DbType.String, role.Description.Trim());
                db.AddInParameter(cmd, "@IsActive", DbType.Boolean, role.IsActive);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);

                roleId = Convert.ToInt64(db.ExecuteScalar(cmd), CultureInfo.InvariantCulture);
            }

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_DELETE_PERMISSIONS_BY_ROLEID))
            {
                db.AddInParameter(cmd, "@RoleId", DbType.Int64, roleId);
                
                db.ExecuteNonQuery(cmd);
            }

            foreach (RolePermission rolePermission in rolePermissionList)
            {
                if (rolePermission.GrantPermission == true)
                {
                    using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_SAVE_ROLE_PERMISSIONS))
                    {
                        db.AddInParameter(cmd, "@RoleId", DbType.Int64, roleId);
                        db.AddInParameter(cmd, "@PermissionCode", DbType.String, rolePermission.PermissionCode);
                        db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);
                        db.ExecuteNonQuery(cmd);
                    }
                }
            }
            
        }

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="roleId">The role id.</param>
        public static void DeleteRole(Int64 roleId)
        {
            Database db = DbHelper.CreateDatabase();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_DELETE_ROLE_BY_ROLEID))
            {
                db.AddInParameter(cmd, "@RoleId", DbType.Int64, roleId);

                db.ExecuteNonQuery(cmd);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Loads the role permission.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        private static RolePermission LoadRolePermission(SafeDataReader reader)
        {
            RolePermission rolePermission = new RolePermission();

            rolePermission.Entity = reader.GetString("Entity").Trim();
            rolePermission.PermissionCode = reader.GetString("PermissionCode").Trim();
            rolePermission.ImpliedPermissionCode = reader.GetString("ImpliedPermissionCode").Trim();
            rolePermission.GrantPermission = reader.GetBoolean("GrantPermission");
            rolePermission.IsModified = false;
            rolePermission.IsNew = false;           

            return rolePermission;

        }
        #endregion
    }
}
