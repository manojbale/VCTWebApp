using System;
using Microsoft.Practices.ObjectBuilder;
using VCTWebApp.Resources;
using VCTWeb.Core.Domain;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace VCTWebApp.Shell.Views
{
    public partial class ForgotPassword : Microsoft.Practices.CompositeWeb.Web.UI.Page, IForgotPasswordView
    {
        #region Instance Variables

        private ForgotPasswordPresenter presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private Helper helper = new Helper();
        //private Security security = null;

        #endregion

        #region Private Properties

        //private bool CanView
        //{
        //    get
        //    {
        //        return ViewState[Common.CAN_VIEW] != null ? (bool)ViewState[Common.CAN_VIEW] : false;
        //    }
        //    set
        //    {
        //        ViewState[Common.CAN_VIEW] = value;
        //    }
        //}

        #endregion

        #region Private Methods

        private void AuthorizedPage()
        {
            //security = new Security();
            //if (security.HasAccess("Password"))
            //{
            //    CanView = security.HasPermission("Password.Manage");
            //}
            //else
            //    Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }

        private void LocalizePage()
        {
            try
            {
                string heading = string.Empty;
                heading = vctResource.GetString("mnuForgotPassword");
                lblHeader.Text = heading;
                Page.Title = heading;

                this.lblUserName.Text = vctResource.GetString("labelUserName");
                this.lblSecurityQuestion.Text = vctResource.GetString("labelSecurityQuestion");
                this.lblSecurityAnswer.Text = vctResource.GetString("labelSecurityAnswer");
                this.lblNewPassword.Text = vctResource.GetString("labelNewPassword");
                this.lblConfirmPassword.Text = vctResource.GetString("labelConfirmNewPassword");
                this.passwordStrengthExtender.PrefixText = vctResource.GetString("labelPasswordPrefix");
                this.passwordStrengthExtender.TextStrengthDescriptions = vctResource.GetString("msgPasswordDescription");

                //this.btnReset.Text = vctResource.GetString("btnReset");
                //this.btnSave.Text = vctResource.GetString("btnSave");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool IsValidInput()
        {
            if (String.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                lblError.Text = vctResource.GetString("valUserName");
                txtUserName.Focus();
                return false;
            }
            if (!IsStrongPassword(txtNewPassword.Text.Trim()))
            {
                lblError.Text = vctResource.GetString("valStrongPassword");
                txtNewPassword.Focus();
                return false;
            }
            if (string.Compare(txtNewPassword.Text.Trim(), txtConfirmPassword.Text.Trim()) != 0)
            {
                lblError.Text = vctResource.GetString("valNewPasswordAndVerifyPassword");
                txtConfirmPassword.Focus();
                return false;
            }
            if (String.IsNullOrEmpty(txtSecurityQuestion.Text.Trim()))
            {
                lblError.Text = vctResource.GetString("valSecurityQuestion");
                txtSecurityQuestion.Focus();
                return false;
            }
            if (String.IsNullOrEmpty(txtSecurityAnswer.Text.Trim()))
            {
                lblError.Text = vctResource.GetString("valSecurityAnswer");
                txtSecurityAnswer.Focus();
                return false;
            }
            return true;
        }

        private bool IsStrongPassword(string password)
        {
            if (password.Length >= 8 && password.Any(char.IsLower) && password.Any(char.IsUpper) && password.Any(char.IsDigit))
                return true;
            else
                return false;
        }

        #endregion

        #region Init/Page Load

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            presenter.NotityEventStatus += new ForgotPasswordPresenter.NotifyEventStatusHandler(presenter_NotityEventStatus);
        }

        void presenter_NotityEventStatus(object sender, string message)
        {
            this.lblError.Text = vctResource.GetString(message);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    //security = new Security();
                    //this.AuthorizedPage();
                    this.presenter.OnViewInitialized();
                    this.LocalizePage();

                    this.Form.DefaultButton = this.btnSave.UniqueID; //Set the default button to save.                    

                    if (this.pnlUserDetail.Enabled)
                        this.txtUserName.Focus();
                }
            }
            catch (SqlException ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Create New Presenter

        [CreateNew]
        public ForgotPasswordPresenter Presenter
        {
            get
            {
                return this.presenter;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this.presenter = value;
                this.presenter.View = this;
            }
        }

        #endregion

        #region IForgotPasswordView Members

        public void SetControlState(bool isActiveDirectory)
        {
            this.pnlUserDetail.Enabled = !isActiveDirectory;
            this.pnlButton.Enabled = !isActiveDirectory;
        }

        public string UserName
        {
            get
            {
                return this.txtUserName.Text.Trim();
            }
            set
            {
                this.txtUserName.Text = value;
            }
        }

        public string SecurityQuestion
        {
            get
            {
                return this.txtSecurityQuestion.Text.Trim();
            }
            set
            {
                this.txtSecurityQuestion.Text = value;
            }
        }

        public string SecurityAnswer
        {
            get
            {
                return this.txtSecurityAnswer.Text.Trim();
            }
            set
            {
                this.txtSecurityAnswer.Text = value;
            }
        }

        public string NewPassword
        {
            get
            {
                return this.txtNewPassword.Text.Trim();
            }
            set
            {
                this.txtNewPassword.Text = value;
            }
        }

        public string ConfirmPassword
        {
            get
            {
                return this.txtConfirmPassword.Text.Trim();
            }
            set
            {
                this.txtConfirmPassword.Text = value;
            }
        }

        #endregion

        #region Event Handlers

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                this.lblError.Text = string.Empty;
                presenter.OnViewInitialized();
            }
            catch (SqlException ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.lblError.Text = string.Empty;
                if (Page.IsValid)
                {
                    if (IsValidInput())
                    {
                        Constants.ResultStatus resultStatus = presenter.Save();
                        if (resultStatus == Constants.ResultStatus.Updated)
                        {
                            presenter.OnViewInitialized();

                            this.lblError.Text = "<font color='blue'>" + vctResource.GetString("msgPasswordUpdated") + "</font>";

                            if (pnlUserDetail.Enabled)
                                this.txtUserName.Focus();
                            //Response.Redirect("~/Login.aspx");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}

