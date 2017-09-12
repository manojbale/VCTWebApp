using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public class ForgotPasswordPresenter : Presenter<IForgotPasswordView>
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

        public ForgotPasswordPresenter()
        {
            helper.LogInformation(string.Empty, "ForgotPasswordPresenter", "Constructor is invoked.");
            this.userRepositoryService = new UserRepository();
        }

        #endregion

        #region Private Methods

        private void SetFieldsBlank()
        {
            helper.LogInformation(string.Empty, "ForgotPasswordPresenter", "SetFieldsBlank() is invoked.");

            View.UserName = string.Empty;
            View.SecurityQuestion = string.Empty;
            View.SecurityAnswer = string.Empty;
            View.NewPassword = string.Empty;
            View.ConfirmPassword = string.Empty;
        }

        private void RaiseStatusEvent(string message)
        {
            helper.LogInformation(string.Empty, "ForgotPasswordPresenter", "RaiseStatusEvent is invoked for message: " + message);

            if (this.NotityEventStatus != null)
                this.NotityEventStatus(this, message);
        }

        #endregion

        #region Public Overrides

        public override void OnViewInitialized()
        {
            helper.LogInformation(string.Empty, "ForgotPasswordPresenter", "OnViewInitialized() is invoked.");
            try
            {
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
            helper.LogInformation(string.Empty, "ForgotPasswordPresenter", "Save() is invoked.");

            Constants.ResultStatus resultStatus = Constants.ResultStatus.Error;
            try
            {
               // SaltedHash saltedHash = new SaltedHash();
                
                //Validate the passwords
                if (!userRepositoryService.VerifySecurityQuestionAnswer(View.UserName, View.SecurityQuestion, View.SecurityAnswer))
                    this.RaiseStatusEvent("valVerifySecurityQuestionAnswer");
                else
                {
                    userRepositoryService.UpdateUserPassword(View.UserName, View.NewPassword);

                    resultStatus = Constants.ResultStatus.Updated;

                    helper.LogInformation(string.Empty, "ForgotPasswordPresenter", "Password is changed successfully.");
                }
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




