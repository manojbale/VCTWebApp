using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace VCTWeb.Core.Domain
{
    /// <summary>
    /// Summary description for ImpliedPermissionRepository
    /// </summary>
    public class ImpliedPermissionRepository
    {

        /// <summary>
        /// Fetches the implied permissions by permissions.
        /// </summary>
        /// <param name="permissions">The permissions.</param>
        /// <returns>List of Implied Permissions for a list of permissions</returns>
        public static List<string> FetchImpliedPermissionsByPermissions(List<Permission> permissions)
        {
            List<ImpliedPermission> lstAllLips = FetchAll();
            Dictionary<string, string> dctAllLips = new Dictionary<string, string>();
            List<string> lstLips = new List<string>();

            //Load all permissions in dictionary
            foreach (ImpliedPermission ip in lstAllLips)
                dctAllLips.Add(ip.Action, ip.ImpliedPermissionCode);

            foreach (Permission permission in permissions)
            {
                LoopupImpliedPermissions(dctAllLips, permission.Action, lstLips);
            }
            return lstLips;
        }

        /// <summary>
        /// Loopups the implied permissions. (Recursive)
        /// </summary>
        /// <param name="dctAllLips">The Dictionary of all List of Implied Permissions.</param>
        /// <param name="permission">The permission.</param>
        /// <param name="lstLips">The List of Implied Permissions for a singe permission.</param>
        private static void LoopupImpliedPermissions(Dictionary<string, string> dctAllLips, string permission, List<string> lstLips)
        {
            if (dctAllLips.ContainsKey(permission))
            {
                string ip = dctAllLips[permission];
                if (!lstLips.Contains(ip))
                    lstLips.Add(ip);
                LoopupImpliedPermissions(dctAllLips, ip,lstLips);
            }
            else
            {
                return;
            }
        }

        

        public static List<ImpliedPermission> FetchAll()
        {
            SafeDataReader reader = null;
            
            Database db = DbHelper.CreateDatabase();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETIMPLIEDPERMISSIONS))
            {
                ImpliedPermission newImpliedPermission = null;          
                List<ImpliedPermission> listOfImpliedPermission = new List<ImpliedPermission>();


                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {

                    while (reader.Read())
                    {
                        newImpliedPermission = Load(reader);
                        listOfImpliedPermission.Add(newImpliedPermission);
                    }

                }
                return listOfImpliedPermission;
            }
        }

        private static ImpliedPermission Load(SafeDataReader reader)
        {
            ImpliedPermission newImpliedPermission = new ImpliedPermission();

            newImpliedPermission.Action = reader.GetString("PermissionCode").Trim();
            newImpliedPermission.ImpliedPermissionCode = reader.GetString("ImpliedPermissionCode").Trim();

            newImpliedPermission.IsModified = false;
            newImpliedPermission.IsNew = false;

            return newImpliedPermission;
        }

    }

}
