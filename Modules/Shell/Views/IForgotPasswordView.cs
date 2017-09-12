using System;
using System.Collections.Generic;
using System.Text;

namespace VCTWebApp.Shell.Views
{
    public interface IForgotPasswordView
    {
        string UserName { get; set; }
        string SecurityQuestion { get; set; }
        string SecurityAnswer { get; set; }
        string NewPassword { get; set; }
        string ConfirmPassword { get; set; }

        void SetControlState(bool isActiveDirectory);
    }
}




