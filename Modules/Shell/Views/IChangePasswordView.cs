using System;
using System.Collections.Generic;
using System.Text;

namespace VCTWebApp.Shell.Views
{
    public interface IChangePasswordView
    {
        string UserName { get; set; }
        string OldPassword { get; set; }
        string NewPassword { get; set; }
        string ConfirmPassword { get; set; }

        void SetControlState(bool isActiveDirectory);
    }
}




