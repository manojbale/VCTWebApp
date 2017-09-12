using System;
using Microsoft.Practices.ObjectBuilder;
using VCTWebApp.Resources;
using VCTWebApp.Web;
using System.Collections.Generic;
using VCTWeb.Core.Domain;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Linq;

namespace VCTWebApp.Shell.Views
{
    public partial class User : Microsoft.Practices.CompositeWeb.Web.UI.Page, IUserView
    {
        #region Instance Variables

        private UserPresenter presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private Helper helper = new Helper();
        private Security security = null;

        #endregion

        #region Private Properties

        private bool CanView
        {
            get
            {
                return ViewState[Common.CAN_VIEW] != null ? (bool)ViewState[Common.CAN_VIEW] : false;
            }
            set
            {
                ViewState[Common.CAN_VIEW] = value;
            }
        }

        private List<VCTWeb.Core.Domain.Users> TempUserList
        {
            get
            {
                return (List<VCTWeb.Core.Domain.Users>)ViewState["UserList"];
            }
            set
            {
                ViewState["UserList"] = value;
            }
        }

        private List<VCTWeb.Core.Domain.Role> TempRoleList
        {
            get
            {
                return (List<VCTWeb.Core.Domain.Role>)ViewState["RoleList"];
            }
            set
            {
                ViewState["RoleList"] = value;
            }
        }

        #endregion

        #region Private Methods

        private void AuthorizedPage()
        {
            security = new Security();
            if (Session["LoggedInUser"] == null)
            {
                Response.Redirect("~/ErrorPage.aspx?ErrorKey=Common_msgSessionExpired");
            }
            else if (security.HasAccess("User"))
            {
                CanView = security.HasPermission("User.Manage");
            }
            else
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }

        private void LocalizePage()
        {
            try
            {
                string heading = string.Empty;
                heading = vctResource.GetString("mnuUser");
                lblHeader.Text = heading;
                Page.Title = heading;

                this.lblExistingUsers.Text = vctResource.GetString("labelExistingUsers");
                this.lblUserId.Text = vctResource.GetString("labelUserId");
                this.lblFirstName.Text = vctResource.GetString("labelFirstName");
                this.lblLastName.Text = vctResource.GetString("labelLastName");
                this.lblSecurityQuestion.Text = vctResource.GetString("labelSecurityQuestion");
                this.lblSecurityAnswer.Text = vctResource.GetString("labelSecurityAnswer");
                this.lblEmailId.Text = vctResource.GetString("labelPrimaryEmailId");
                this.lblPhone.Text = vctResource.GetString("labelPhone");
                this.lblCell.Text = vctResource.GetString("labelCell");
                this.lblFax.Text = vctResource.GetString("labelFax");
                this.lblDomain.Text = vctResource.GetString("labelDomain");
                this.chkResetPassword.Text = vctResource.GetString("labelResetPassword");
                this.rdoSystemUser.Text = vctResource.GetString("labelSystemUser");
                this.rdoDomainUser.Text = vctResource.GetString("labelDomainUser");
                this.lblRoles.Text = vctResource.GetString("labelRoles");
                this.chkActive.Text = vctResource.GetString("labelActive");

                //this.btnNew.Text = vctResource.GetString("btnReset");
                //this.btnSave.Text = vctResource.GetString("btnSave");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UpdateRoleDetails()
        {
            foreach (VCTWeb.Core.Domain.Role role in this.TempRoleList)
            {
                foreach (GridViewRow row in this.gdvRoleList.Rows)
                {
                    Int64 roleId = Convert.ToInt64(this.gdvRoleList.DataKeys[row.RowIndex].Value);
                    if (role.RoleId == roleId)
                    {
                        CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                        if (chkSelect != null)
                        {
                            role.GrantRole = chkSelect.Checked;
                        }
                    }
                }
            }
        }

        private void DisplayMessageForMissingMasters()
        {
            if (this.TempRoleList.Count <= 0)
            {
                this.lblError.Text = vctResource.GetString("msgDefineMaster");
                this.lblError.Text += "<br />" + vctResource.GetString("mnuRole");
            }
        }

       
        #endregion

        #region Init/Page Load

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    this.EnableDisableEmailValidator();
                    this.AuthorizedPage();
                    this.presenter.OnViewInitialized();
                    this.chkResetPassword.Enabled = false;
                    this.LocalizePage();
                    //this.ShowBadgeNumberControls();
                    this.Form.DefaultButton = this.btnSave.UniqueID; //Set the default button to save.

                    this.DisplayMessageForMissingMasters();

                    this.txtUserId.Focus();
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
        public UserPresenter Presenter
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

        #region IUserView Members

        public bool Active
        {
            get
            {
                return this.chkActive.Checked;
            }
            set
            {
                this.chkActive.Checked = value;
            }
        }

        public bool IsSystemUser
        {
            get
            {
                return this.rdoSystemUser.Checked;
            }
            set
            {
                this.rdoSystemUser.Checked = value;
                this.txtDomain.Enabled = !value;
            }
        }

        public bool IsDomainUser
        {
            get
            {
                return this.rdoDomainUser.Checked;
            }
            set
            {
                this.rdoDomainUser.Checked = value;
                this.txtDomain.Enabled = value;
            }
        }

        public string SelectedUserId
        {
            get
            {
                return this.lstExistingUsers.SelectedValue;
            }
        }

        public List<VCTWeb.Core.Domain.Users> UserList
        {
            get
            {
                return this.TempUserList;
            }
            set
            {
                this.TempUserList = value as List<VCTWeb.Core.Domain.Users>;
                this.lstExistingUsers.DataSource = value;
                this.lstExistingUsers.DataTextField = "FullName";
                this.lstExistingUsers.DataValueField = "UserName";
                this.lstExistingUsers.DataBind();
            }
        }

        public List<VCTWeb.Core.Domain.Role> RoleList
        {
            get
            {
                return this.TempRoleList;
            }
            set
            {
                this.TempRoleList = value as List<VCTWeb.Core.Domain.Role>;
                if (value != null && value.Count == 0)
                {
                    value.Add(new VCTWeb.Core.Domain.Role());
                }
                this.gdvRoleList.DataSource = value;
                this.gdvRoleList.DataKeyNames = new string[] { "RoleId" };
                this.gdvRoleList.DataBind();
            }
        }

        public string UserId
        {
            get
            {
                return this.txtUserId.Text.Trim();
            }
            set
            {
                this.txtUserId.Text = value;
            }
        }

        public string FirstName
        {
            get
            {
                return this.txtFirstName.Text.Trim();
            }
            set
            {
                this.txtFirstName.Text = value;
            }
        }

        public string LastName
        {
            get
            {
                return this.txtLastName.Text.Trim();
            }
            set
            {
                this.txtLastName.Text = value;
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

        public string Domain
        {
            get
            {
                return this.txtDomain.Text.Trim();
            }
            set
            {
                this.txtDomain.Text = value;
            }
        }

        public string Email
        {
            get
            {
                return this.txtEmailId.Text.Trim();
            }
            set
            {
                this.txtEmailId.Text = value;
            }
        }

        public string Phone
        {
            get
            {
                return this.txtPhone.Text.Trim();
            }
            set
            {
                this.txtPhone.Text = value;
            }
        }

        public string Cell
        {
            get
            {
                return this.txtCell.Text.Trim();
            }
            set
            {
                this.txtCell.Text = value;
            }
        }

        public string Fax
        {
            get
            {
                return this.txtFax.Text.Trim();
            }
            set
            {
                this.txtFax.Text = value;
            }
        }

        public bool ResetPassword
        {
            get
            {
                return this.chkResetPassword.Checked;
            }
            set
            {
                this.chkResetPassword.Checked = value;
            }
        }

        public int LocationId
        {
            get
            {
                return Convert.ToInt32(ddlLocation.SelectedValue);
            }
            set
            {
                this.ddlLocation.SelectedValue = value.ToString();
            }
        }

        public List<Location> LocationList
        {
            set
            {
                this.ddlLocation.DataSource = value;
                this.ddlLocation.DataTextField = "LocationTypeLocationName";
                this.ddlLocation.DataValueField = "LocationId";
                this.ddlLocation.DataBind();
                this.ddlLocation.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect"), "0"));
            }
        }


        #endregion

        #region Event Handlers

        protected void gdvRoleList_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            try
            {
                this.UpdateRoleDetails();
                this.gdvRoleList.PageIndex = e.NewPageIndex;
                this.RoleList = this.TempRoleList;
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

        protected void gdvRoleList_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Text = vctResource.GetString("labelRoleName");
                e.Row.Cells[1].Text = vctResource.GetString("colAction");
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the rdoSystemUser control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void rdoSystemUser_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdoSystemUser.Checked)
                {
                    this.txtDomain.Enabled = false;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the rdoDomainUser control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void rdoDomainUser_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdoDomainUser.Checked)
                {
                    this.txtDomain.Enabled = true;
                }
            }
            catch
            {
                throw;
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                this.lblError.Text = string.Empty;
                this.TempUserList = null;
                this.TempRoleList = null;
                presenter.OnViewInitialized();
                this.chkResetPassword.Enabled = false;
                this.txtUserId.Enabled = true;
                this.txtUserId.Focus();
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
                this.UpdateRoleDetails();
                if (Page.IsValid && ValidatedPageInput())
                {
                   
                        Constants.ResultStatus resultStatus = presenter.Save();
                        if (resultStatus == Constants.ResultStatus.SelectAtleastOneItem)
                        {
                            this.lblError.Text = vctResource.GetString("valCheckRole");
                        }
                        else if (resultStatus == Constants.ResultStatus.SelectLocation)
                        {
                            this.lblError.Text = vctResource.GetString("valLocation");
                        }
                        else if (resultStatus == Constants.ResultStatus.Created)
                        {
                            this.TempUserList = null;
                            this.TempRoleList = null;
                            presenter.OnViewInitialized();
                            this.chkResetPassword.Enabled = false;
                            this.txtUserId.Enabled = true;
                            this.txtUserId.Focus();

                            this.lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgCreated"), this.lblHeader.Text) + "</font>";
                        }
                        else if (resultStatus == Constants.ResultStatus.Updated)
                        {
                            this.TempUserList = null;
                            this.TempRoleList = null;
                            presenter.OnViewInitialized();
                            this.chkResetPassword.Enabled = false;
                            this.txtUserId.Enabled = true;
                            this.txtUserId.Focus();

                            this.lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgUpdated"), this.lblHeader.Text) + "</font>";
                        }
                        else if (resultStatus == Constants.ResultStatus.InUse)
                        {
                            this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgInUseUser"), this.lblHeader.Text);
                        }
                        else
                        {
                            //ToDo - Handle the error part
                        }
                   
                }
            }
            catch (Exception ex)
            {
                if (string.Compare(Common.MSG_VAL_EXISTS, ex.Message.Trim(), true, CultureInfo.InvariantCulture) == 0 || string.Compare(Common.MSG_BADGE_EXISTS, ex.Message.Trim(), true, CultureInfo.InvariantCulture) == 0)
                {
                    this.lblError.Text = string.Format(CultureInfo.InvariantCulture, this.vctResource.GetString(ex.Message), this.vctResource.GetString("mnuUser"));
                }
                else
                {
                    throw ex;
                }
            }
        }

        private bool ValidatedPageInput()
        {

            if (!string.IsNullOrEmpty(txtEmailId.Text.Trim()))
            {

                string pattern = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
                System.Text.RegularExpressions.Match match = Regex.Match(txtEmailId.Text.Trim(), pattern, RegexOptions.IgnoreCase);
                if (!match.Success)
                {
                    this.lblError.Text = "<font color='red'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("validEmailId"), this.lblHeader.Text) + "</font>";
                    return false;
                
                }
            }
           
            if (txtPhone.Text.Any(char.IsLetter))
            {
                this.lblError.Text = "<font color='red'>" + "Please enter valid Phone number in digits" + "</font>";
                return false;
            }
            if (txtCell.Text.Any(char.IsLetter) || txtFax.Text.Any(char.IsLetter))
            {
                this.lblError.Text = "<font color='red'>" + "Please enter valid Cell number  in digits" + "</font>";
                return false;
            }
            if (txtFax.Text.Any(char.IsLetter))
            {
                this.lblError.Text = "<font color='red'>" + "Please enter valid  Fax number in digits" + "</font>";
                return false;
            }


            return true;
        }
        private void EnableDisableEmailValidator()
        {
            lblEmailId.CssClass = "label";
            MyPropertyProxyValidator3.Enabled = false;
        }

        protected void lstExistingUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.lblError.Text = string.Empty;
                this.TempRoleList = null;
                presenter.OnViewLoaded();
                if (!string.IsNullOrEmpty(this.lstExistingUsers.SelectedValue))
                {
                    this.chkResetPassword.Enabled = true;
                    this.txtUserId.Enabled = false;
                }
                this.txtFirstName.Focus();
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

