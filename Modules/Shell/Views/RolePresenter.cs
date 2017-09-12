using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;
using System.Web;
using System.Transactions;


namespace VCTWebApp.Shell.Views
{
    public class RolePresenter : Presenter<IRoleView>
    {

        #region Instance Variables

        
        private RoleRepository roleRepositoryService;
        private RolePermissionRepository rolePermissionRepositoryService;

        private Helper helper = new Helper();

        #endregion

        #region Constructors

        public RolePresenter()
            : this(new RolePermissionRepository(HttpContext.Current.User.Identity.Name),
                    new RoleRepository(HttpContext.Current.User.Identity.Name))
        {
        }

        public RolePresenter(RolePermissionRepository rolePermissionRepository, RoleRepository roleRepository)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "RolePresenter", "Constructor is invoked.");

            this.rolePermissionRepositoryService = rolePermissionRepository;
            this.roleRepositoryService = roleRepository;
        }

        #endregion

        #region Private Methods

        private void SetFieldsBlank()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "RolePresenter", "SetFieldsBlank() is invoked.");

            View.RoleName = string.Empty;
            View.Description = string.Empty;
        }

        private Role GetSelectedRole()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "RolePresenter", "GetSelectedRole is invoked for selectedRoleId: " + Convert.ToString(View.SelectedRoleId));

            try
            {
                return View.RoleList.Find(delegate(Role roleInList) { return roleInList.RoleId == View.SelectedRoleId; });
            }
            catch
            {
                return null;
            }
        }

        private List<string> GetEntityList()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "RolePresenter", "GetEntityList() is invoked.");

            List<string> entityList = new List<string>();

            foreach (RolePermission rolePermission in View.RolePermissionList)
            {
                if (!entityList.Contains(rolePermission.Entity.Trim()))
                {
                    entityList.Add(rolePermission.Entity.Trim());
                }
            }

            return entityList;
        }

        #endregion

        #region Public Overrides

        public override void OnViewLoaded()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "RolePresenter", "OnViewLoaded() is invoked.");

            try
            {
                Role role = this.GetSelectedRole();
                if (role != null)
                {
                    View.RoleName = role.RoleName.Trim();
                    View.Description = role.Description.Trim();
                    View.RolePermissionList = this.rolePermissionRepositoryService.GetRolePermissionsByRoleId(role.RoleId);
                }
            }
            catch
            {
                throw;
            }
        }

        public override void OnViewInitialized()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "RolePresenter", "OnViewInitialized() is invoked.");
            try
            {
                View.RoleList = this.roleRepositoryService.FetchAll();
                View.RolePermissionList = this.rolePermissionRepositoryService.GetRolePermissionList();
                View.EntityList = this.GetEntityList();
                this.SetFieldsBlank();
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <returns>ResultStatus - Error, Created, Updated.</returns>
        public Constants.ResultStatus Save()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "RolePresenter", "Save() is invoked.");

            Constants.ResultStatus resultStatus = Constants.ResultStatus.Error;
            try
            {
                Role role = this.GetSelectedRole();
                if (role == null)
                {
                    role = new Role();
                }

                role.RoleName = View.RoleName;
                role.Description = View.Description;

                this.rolePermissionRepositoryService.SaveRolePermissions(role, View.RolePermissionList);

                helper.LogInformation(HttpContext.Current.User.Identity.Name, "RolePresenter", "Role saved for roleName: " + role.RoleName);

                if (role.RoleId == 0)
                    resultStatus = Constants.ResultStatus.Created;
                else
                    resultStatus = Constants.ResultStatus.Updated;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resultStatus;
        }

        public Constants.ResultStatus Delete()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "RolePresenter", "Delete() is invoked.");

            Constants.ResultStatus resultStatus = Constants.ResultStatus.Error;

            try
            {
                this.roleRepositoryService.DeleteRole(View.SelectedRoleId);
                resultStatus = Constants.ResultStatus.Deleted;
            }
            catch
            {
                throw;
            }

            return resultStatus;
        }

        #endregion
    }
}




