using System;
using Microsoft.Practices.ObjectBuilder;
using VCTWebApp.Resources;
using VCTWeb.Core.Domain;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace VCTWebApp.Shell.Views
{
    public partial class ChangePassword : Microsoft.Practices.CompositeWeb.Web.UI.Page, IChangePasswordView
    {
        #region Instance Variables

        private ChangePasswordPresenter presenter;
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
                heading = vctResource.GetString("mnuChangePassword");
                lblHeader.Text = heading;
                Page.Title = heading;

                this.lblUserName.Text = vctResource.GetString("labelUserName");
                this.lblOldPassword.Text = vctResource.GetString("labelOldPassword");
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

        //private bool ValidPassword(string password)
        //{

        //    bool result = false;
        //    //Regex rx = new Regex("^(?=.*[0-9])(?=.*[a-zA-Z]).{6,8}$", RegexOptions.IgnoreCase);
        //    //Match m = rx.Match(password);
        //    //string res = m.Groups[0].Value;
        //    //if (res == "")
        //    //{
        //    //    result = false;
        //    //}
        //    //else
        //    //{
        //    //    result = true;
        //    //}
        //    if (password.Trim().Length < 6)
        //        result = false;
        //    else
        //        result = true;
        //    return result;

        //}

        private bool IsStrongPassword(string password)
        {
            if (password.Length >= 8 && password.Any(char.IsLower) && password.Any(char.IsUpper) && password.Any(char.IsDigit))
                return true;
            else
                return false;
        }


        private void UserManagement()
        {
            //if (Session["license"] != null)
            //{
            //    IntPtr license = (IntPtr)Session["license"];
            //    RLM.rlm_checkin(license);
            //}
            Session.Abandon();
            FormsAuthentication.SignOut();
            Response.Redirect("~/Login.aspx");
        }

        #endregion

        #region Init/Page Load

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            presenter.NotityEventStatus += new ChangePasswordPresenter.NotifyEventStatusHandler(presenter_NotityEventStatus);
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
                        this.txtOldPassword.Focus();
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
        public ChangePasswordPresenter Presenter
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

        #region IChangePasswordView Members

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

        public string OldPassword
        {
            get
            {
                return this.txtOldPassword.Text.Trim();
            }
            set
            {
                this.txtOldPassword.Text = value;
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
                    bool flag = presenter.IsNewUser();
                    if (presenter.IsNewUser())
                    { 
                        UserManagement();
                    }

                    if (string.IsNullOrEmpty(txtOldPassword.Text.Trim()))
                    {
                        this.lblError.Text = "<font color='red'>" + "Please enter Password " + "</font>";
                        return;
                    }
                    else if (string.IsNullOrEmpty(txtNewPassword.Text.Trim()))
                    {
                          this.lblError.Text = "<font color='red'>" + "Please enter new  Password " + "</font>";
                           return;
                    }
                    else if (!string.IsNullOrEmpty(txtNewPassword.Text.Trim()))
                    {
                         if (!IsStrongPassword(txtNewPassword.Text.Trim()))
                         {
                             lblError.Text = vctResource.GetString("valStrongPassword");
                             return;
                         }
                    }

                    if (string.IsNullOrEmpty(txtConfirmPassword.Text.Trim()))
                    {
                         this.lblError.Text = "<font color='red'>" + "Please Enter Confirm Password  " + "</font>";
                         return;
                    }
                    if (txtNewPassword.Text.Trim() != txtConfirmPassword.Text.Trim())
                    {
                        this.lblError.Text = "<font color='red'>" + "New password and confirm new password must be same " + "</font>";
                        return;
                    }
                    
                    if (!presenter.IsPasswordSame())
                            {
                                Constants.ResultStatus resultStatus = presenter.Save();
                                if (resultStatus == Constants.ResultStatus.Updated)
                                {
                                    presenter.OnViewInitialized();

                                    this.lblError.Text = "<font color='blue'>" + vctResource.GetString("msgPasswordUpdated") + "   "+"<a href='Login.aspx'>Click Here to login </a> " + "</font>";

                                    Session.Abandon();
                                    Session.RemoveAll();
                                }
                            }
                            else
                            {
                                lblError.Text = vctResource.GetString("Login_msgcharforsame");
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

