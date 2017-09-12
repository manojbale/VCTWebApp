using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace VCTWeb.Core.Domain
{
    /// <summary>
    /// Summary description for PermissionRepository
    /// </summary>
    public class PermissionRepository
    {

        
        public PermissionRepository()
        {
        }

        /// <summary>
        /// Fetches all Permissions.
        /// </summary>
        /// <returns>List of permissions</returns>
        public  List<Permission> FetchAll()
        {
            SafeDataReader reader = null;
            Permission newPermission = null;


            Database db = DbHelper.CreateDatabase();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETPERMISSION))
            {

                List<Permission> listOfPermission = new List<Permission>();


                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {

                    while (reader.Read())
                    {
                        newPermission = Load(reader);
                        listOfPermission.Add(newPermission);
                    }

                }
                return listOfPermission;
            }
        }

        /// <summary>
        /// Fetches list of Permissions & Implied permissions by user name.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public  List<Permission> FetchByUserName(string username)
        {
            SafeDataReader reader = null;
            Permission newPermission = null;


            Database db = DbHelper.CreateDatabase();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETLISTOFPERMISSIONBYUSERNAME))
            {
                db.AddInParameter(cmd, "@UserName", DbType.String, username);

                List<Permission> listOfPermission = new List<Permission>();


                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {

                    while (reader.Read())
                    {
                        newPermission = Load(reader);
                        if (!listOfPermission.Exists(permission => permission.Action == newPermission.Action))
                        {
                            listOfPermission.Add(newPermission);
                        }
                    }

                }
                if (listOfPermission.Count > 0)
                {
                    //Get all implied permissions for list of permissions.
                    List<string> impliedPermissons = ImpliedPermissionRepository.FetchImpliedPermissionsByPermissions(listOfPermission);
                    List<Permission> lstAllPermissions = FetchAll(); //reduce db hits
                    foreach (string ip in impliedPermissons)
                    {
                        if (!listOfPermission.Exists(permission => permission.Action == ip)) //Lambda expression used
                        {
                            //retrieve permission object corrosponding to implied permission string
                            Permission foundPermission = lstAllPermissions.Find(permission => permission.Action == ip); //Lambda expression used
                            if (foundPermission != null)
                                listOfPermission.Add(foundPermission);
                        }

                    }
                }
                return listOfPermission;
            }
        }

        /// <summary>
        /// Fetches list of Permissions & Implied permissions by roles.
        /// </summary>
        /// <param name="adGroupList">The ad group list.</param>
        /// <returns>List of Permission</returns>
        public  List<Permission> FetchByRoles(string adGroupList)
        {
            Permission newPermission = null;

            Database db = DbHelper.CreateDatabase();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETLISTOFPERMISSIONBYROLES))
            {
                db.AddInParameter(cmd, "@Roles", DbType.String, adGroupList);

                List<Permission> listOfPermission = new List<Permission>();

                using (SafeDataReader reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newPermission = Load(reader);
                        if (!listOfPermission.Exists(permission => permission.Action == newPermission.Action))
                        {
                            listOfPermission.Add(newPermission);
                        }
                    }

                }
                if (listOfPermission.Count > 0)
                {
                    //Get all implied permissions for list of permissions.
                    List<string> impliedPermissons = ImpliedPermissionRepository.FetchImpliedPermissionsByPermissions(listOfPermission);
                    List<Permission> lstAllPermissions = FetchAll(); //reduce db hits
                    foreach (string ip in impliedPermissons)
                    {
                        if (!listOfPermission.Exists(permission => permission.Action == ip)) //Lambda expression used
                        {
                            //retrieve permission object corrosponding to implied permission string
                            Permission foundPermission = lstAllPermissions.Find(permission => permission.Action == ip); //Lambda expression used
                            if (foundPermission != null)
                                listOfPermission.Add(foundPermission);
                        }

                    }
                }
                return listOfPermission;
            }
        }


        public List<string> GetEntityList()
        {
            List<string> entityList = new List<string>();
            string entity = string.Empty;
            SafeDataReader reader = null;

            Database db = DbHelper.CreateDatabase();
            string sql = "SELECT * FROM PermissionEntity";

            using (reader = new SafeDataReader(db.ExecuteReader(CommandType.Text, sql)))
            {
                while (reader.Read())
                {
                   entity = reader.GetString("Entity");
                   entityList.Add(entity);
                }
            }
            return entityList;
        }



        /// <summary>
        /// Loads the specified reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        private  Permission Load(SafeDataReader reader)
        {
            Permission newPermission = new Permission();

            newPermission.Action = reader.GetString("PermissionCode").Trim();
            newPermission.Description = reader.GetString("Description");
            newPermission.EntityClass = reader.GetString("Entity").Trim();

            newPermission.IsModified = false;
            newPermission.IsNew = false;

            return newPermission;
        }

    }
}
