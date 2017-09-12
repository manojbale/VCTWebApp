using System;
using System.Collections.Generic;
using System.Text;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public interface IUserView
    {
        List<Users> UserList { get; set; }
        List<Role> RoleList { get; set; }
        List<Location> LocationList { set; }

        string UserId { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string SecurityQuestion { get; set; }
        string SecurityAnswer { get; set; }
        int LocationId { get; set; }
        bool IsSystemUser { get; set; }
        bool IsDomainUser { get; set; }
        string Domain { get; set; }
        string Email { get; set; }
        string Phone { get; set; }
        string Cell { get; set; }
        string Fax { get; set; }
        bool Active { get; set; }
        bool ResetPassword { get; set; }
        string SelectedUserId { get; }
    }
}




