using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;
using System.Web;

namespace VCTWebApp.Shell.Views
{
    public class ChangePasswordPresenter : Presenter<IChangePasswordView>
    {
        #region Delegates & Events

        public delegate void NotifyEventStatusHandler(object sender, string message);
        public event NotifyEventStatusHandler NotityEventStatus;

        #endregion

        #region Instance Variables

        private UserRepository userRepositoryService;

        private Helper helper = new Helper();

        #endregion

        #region Constructors

        public ChangePasswordPresenter()
            : this(new UserRepository(HttpContext.Current.User.Identity.Name))
        {
        }

        public ChangePasswordPresenter(UserRepository userRepository)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "ChangePasswordPresenter", "Constructor is invoked.");
            this.userRepositoryService = userRepository;
        }

        #endregion

        #region Private Methods

        private void SetFieldsBlank()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "ChangePasswordPresenter", "SetFieldsBlank() is invoked.");

            View.OldPassword = string.Empty;
            View.NewPassword = string.Empty;
            View.ConfirmPassword = string.Empty;
        }

        private void RaiseStatusEvent(string message)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "ChangePasswordPresenter", "RaiseStatusEvent is invoked for message: " + message);

            if (this.NotityEventStatus != null)
                this.NotityEventStatus(this, message);
        }

        #endregion

        #region Public Overrides

        public override void OnViewInitialized()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "ChangePasswordPresenter", "OnViewInitialized() is invoked.");
            try
            {
                View.UserName = HttpContext.Current.User.Identity.Name;
                this.SetFieldsBlank();

                //Disable the page if the active directory is 'true'
                bool isActiveDirectory;
                bool.TryParse(new ConfigurationRepository().GetConfigurationKeyValue(ConfigSetting.IS_ACTIVE_DIRECTORY), out isActiveDirectory);
                View.SetControlState(isActiveDirectory);
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
        /// <returns>ResultStatus - Error, Updated.</returns>
        public Constants.ResultStatus Save()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "ChangePasswordPresenter", "Save() is invoked.");

            Constants.ResultStatus resultStatus = Constants.ResultStatus.Error;
            try
            {
               // SaltedHash saltedHash = new SaltedHash();
                Users currentUser = userRepositoryService.FetchUserByName(View.UserName);

                //Validate the passwords
                if (string.IsNullOrEmpty(View.OldPassword))
                    this.RaiseStatusEvent("valOldPassword");
               // else if (!saltedHash.VerifyPassword(View.OldPassword, currentUser.Password))
                else if (!MD5CryptorEngine.VerifyPassword(View.OldPassword, currentUser.Password))
                    this.RaiseStatusEvent("valVerifyOldPassword");
                else if (string.IsNullOrEmpty(View.NewPassword))
                    this.RaiseStatusEvent("valNewPassword");
                else if (string.Compare(View.NewPassword, View.ConfirmPassword) != 0)
                    this.RaiseStatusEvent("valNewPasswordAndVerifyPassword");
                else
                {
                    userRepositoryService.UpdateUserPassword(View.NewPassword);

                    resultStatus = Constants.ResultStatus.Updated;

                    helper.LogInformation(HttpContext.Current.User.Identity.Name, "ChangePasswordPresenter", "Password is changed successfully.");
                }
            }
            catch
            {
                throw;
            }

            return resultStatus;
        }

        public bool IsNewUser()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "IsNewUser", "IsNewUser() is invoked.");

            bool flag = false;
            try
            {
                Users currentUser = userRepositoryService.FetchUserByName(View.UserName);

                //Validate the passwords
                //if (currentUser.LastPasswordDate.HasValue)
                //{
                DateTime expdatetime = Convert.ToDateTime(currentUser.LastPasswordDate);

                //Validate the passwords
                if (currentUser.LastPasswordDate == null || DateTime.UtcNow >= expdatetime.AddDays(90))
                {
                    flag = true;
                }
                //}


                helper.LogInformation(HttpContext.Current.User.Identity.Name, "IsNewUser", "IsNewUser");
            }
            catch
            {
                throw;
            }

            return flag;
        }

        public bool IsPasswordSame()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "IsNewUser", "IsNewUser() is invoked.");

            bool flag = false;
            try
            {
                Users currentUser = userRepositoryService.FetchUserByName(View.UserName);

                if (View.NewPassword == View.OldPassword)
                {
                    flag = true;
                }


                helper.LogInformation(HttpContext.Current.User.Identity.Name, "IsNewUser", "IsNewUser");
            }
            catch
            {
                throw;
            }

            return flag;
        }
        #endregion
    }
}




