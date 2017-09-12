using System;
using System.Collections.Generic;
using System.Text;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public interface IRoleView
    {
        List<Role> RoleList { get; set; }
        List<RolePermission> RolePermissionList { get; set; }
        List<string> EntityList { get; set; }

        long SelectedRoleId { get; }
        string SelectedEntity { get; set; }
        string RoleName { get; set; }
        string Description { get; set; }
    }
}




