using System;
using Microsoft.Practices.ObjectBuilder;
using VCTWebApp.Resources;
using VCTWeb.Core.Domain;
using VCTWebApp.Web;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Globalization;

namespace VCTWebApp.Shell.Views
{
    public partial class Role : Microsoft.Practices.CompositeWeb.Web.UI.Page, IRoleView
    {
        #region Instance Variables

        private RolePresenter presenter;
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

        private List<RolePermission> TempRolePermissionList
        {
            get
            {
                return (List<RolePermission>)ViewState["TempRolePermissionList"];
            }
            set
            {
                ViewState["TempRolePermissionList"] = value;
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

        private List<string> TempEntityList
        {
            get
            {
                return (List<string>)ViewState["EntityList"];
            }
            set
            {
                ViewState["EntityList"] = value;
            }
        }

        private string TempSelectedEntity
        {
            get
            {
                return Convert.ToString(ViewState["SelectedEntity"]);
            }
            set
            {
                ViewState["SelectedEntity"] = value;
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
            else if (security.HasAccess("Role"))
            {
                CanView = security.HasPermission("Role.Manage");
            }
            else
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }

        private void LocalizePage()
        {
            string heading = string.Empty;
            heading = vctResource.GetString("mnuRole");
            lblHeader.Text = heading;
            Page.Title = heading;

            this.lblExistingRoles.Text = vctResource.GetString("labelexistingroles");
            this.lblRoleName.Text = vctResource.GetString("labelRoleName");
            this.lblDescription.Text = vctResource.GetString("labelDescription");
            this.lblPermissions.Text = vctResource.GetString("labelPermissions");

            //this.btnNew.Text = vctResource.GetString("btnReset");
            //this.btnSave.Text = vctResource.GetString("btnSave");
        }

        private void UpdatePermissionDetails()
        {
            foreach (RolePermission rolePermission in this.TempRolePermissionList)
            {
                foreach (GridViewRow row in this.gdvPermissionList.Rows)
                {
                    string permissionCode = Convert.ToString(this.gdvPermissionList.DataKeys[row.RowIndex].Value);
                    if (!string.IsNullOrEmpty(permissionCode) && string.Compare(permissionCode.Trim(), rolePermission.PermissionCode.Trim()) == 0)
                    {
                        CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                        if (chkSelect != null)
                        {
                            rolePermission.GrantPermission = chkSelect.Checked;
                        }
                    }
                }
            }
        }

        private void UpdateTempCriterion()
        {
            this.TempSelectedEntity = this.SelectedEntity;
        }

        private void SetLastCriterion()
        {
            this.EntityList = this.TempEntityList;
            this.SelectedEntity = this.TempSelectedEntity;
        }

        private void BindGrid()
        {
            List<RolePermission> rolePermissionList = this.TempRolePermissionList as List<RolePermission>;
            if (rolePermissionList != null && rolePermissionList.Count == 0)
            {
                rolePermissionList.Add(new RolePermission());
            }

            this.gdvPermissionList.DataSource = rolePermissionList;
            this.gdvPermissionList.DataKeyNames = new string[] { "PermissionCode" };
            this.gdvPermissionList.DataBind();
        }

        private void BindEntityFilterList()
        {
            DropDownList ddlSelectEntity = (DropDownList)this.gdvPermissionList.HeaderRow.FindControl("ddlSelectEntity");
            if (ddlSelectEntity != null)
            {
                ddlSelectEntity.DataSource = this.EntityList;
                ddlSelectEntity.DataBind();

                ddlSelectEntity.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll")));
            }
        }

        private void UpdateRolePermissionListForSave()
        {
            foreach (RolePermission lastRolePermission in this.RolePermissionList)
            {
                foreach (RolePermission currentRolePermision in this.TempRolePermissionList)
                {
                    if (string.Compare(currentRolePermision.PermissionCode.Trim(), lastRolePermission.PermissionCode.Trim()) == 0)
                    {
                        lastRolePermission.GrantPermission = currentRolePermision.GrantPermission;
                    }
                }
            }
        }

        private bool PermissionIsSelected()
        {
            foreach (RolePermission lastRolePermission in this.RolePermissionList)
            {
                if (lastRolePermission.GrantPermission)
                    return true;
            }
            return false;
        }


        private void UpdateRolePermissionListForFilter(List<RolePermission> filteredRolePermissionList)
        {
            foreach (RolePermission filteredRolePermission in filteredRolePermissionList)
            {
                foreach (RolePermission currentRolePermision in this.TempRolePermissionList)
                {
                    if (string.Compare(currentRolePermision.PermissionCode.Trim(), filteredRolePermission.PermissionCode.Trim()) == 0)
                    {
                        filteredRolePermission.GrantPermission = currentRolePermision.GrantPermission;
                    }
                }
            }
        }

        #endregion

        #region Init/Page Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                security = new Security();
                this.AuthorizedPage();
                this.LocalizePage();
                this.presenter.OnViewInitialized();
                this.TempRolePermissionList = this.RolePermissionList;
                this.BindGrid();
                this.BindEntityFilterList();

                this.btnDelete.Enabled = false;

                this.btnDelete.Attributes.Add("onclick", "javascript:return " + "confirm('" + vctResource.GetString("msgDeleteConfirm") + "')");

                this.Form.DefaultButton = this.btnSave.UniqueID; //Set the default button to save.

                this.txtRoleName.Focus();
            }
        }

        #endregion

        [CreateNew]
        public RolePresenter Presenter
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

        #region IRoleView Members

        public List<string> EntityList
        {
            get
            {
                return this.TempEntityList;
            }
            set
            {
                this.TempEntityList = value;
            }
        }

        public string SelectedEntity
        {
            get
            {
                DropDownList ddlSelectEntity = (DropDownList)this.gdvPermissionList.HeaderRow.FindControl("ddlSelectEntity");
                if (ddlSelectEntity != null)
                {
                    return ddlSelectEntity.SelectedValue.Trim();
                }
                return string.Empty;
            }
            set
            {
                DropDownList ddlSelectEntity = (DropDownList)this.gdvPermissionList.HeaderRow.FindControl("ddlSelectEntity");
                if (ddlSelectEntity != null)
                {
                    ddlSelectEntity.SelectedIndex = ddlSelectEntity.Items.IndexOf(ddlSelectEntity.Items.FindByValue(value));
                }
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
                this.lstExistingRoles.DataSource = value;
                this.lstExistingRoles.DataTextField = "RoleName";
                this.lstExistingRoles.DataValueField = "RoleId";
                this.lstExistingRoles.DataBind();
            }
        }

        public List<RolePermission> RolePermissionList
        {
            get
            {
                return (List<RolePermission>)ViewState["RolePermissionList"];
            }
            set
            {
                ViewState["RolePermissionList"] = value;
            }
        }

        public string RoleName
        {
            get
            {
                return this.txtRoleName.Text.Trim();
            }
            set
            {
                this.txtRoleName.Text = value;
            }
        }

        public string Description
        {
            get
            {
                return this.txtDescription.Text.Trim();
            }
            set
            {
                this.txtDescription.Text = value;
            }
        }

        public long SelectedRoleId
        {
            get
            {
                if (string.IsNullOrEmpty(this.lstExistingRoles.SelectedValue))
                    return 0;
                return Convert.ToInt64(this.lstExistingRoles.SelectedValue);
            }
        }

        #endregion

        #region Event Handlers

        protected void gdvPermissionList_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            this.UpdatePermissionDetails();
            this.gdvPermissionList.PageIndex = e.NewPageIndex;
            this.BindGrid();
            this.BindEntityFilterList();
        }

        protected void gdvPermissionList_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                System.Web.UI.WebControls.Label lblEntity = (System.Web.UI.WebControls.Label)e.Row.FindControl("lblEntity");
                if (lblEntity != null)
                    lblEntity.Text = vctResource.GetString("labelEntity");

                e.Row.Cells[1].Text = vctResource.GetString("labelPermissionCode");

                System.Web.UI.WebControls.Label lblAction = (System.Web.UI.WebControls.Label)e.Row.FindControl("lblAction");
                if (lblAction != null)
                    lblAction.Text = vctResource.GetString("colAction");

                System.Web.UI.WebControls.Label lblSelect = (System.Web.UI.WebControls.Label)e.Row.FindControl("lblSelect");
                LinkButton lnkSelectAll = (LinkButton)e.Row.FindControl("lnkSelectAll");
                LinkButton lnkSelectNone = (LinkButton)e.Row.FindControl("lnkSelectNone");
                if (lblSelect != null)
                    lblSelect.Text = vctResource.GetString("labelSelectPermission");
                if (lnkSelectAll != null)
                    lnkSelectAll.Text = vctResource.GetString("labelAllPermission");
                if (lnkSelectNone != null)
                    lnkSelectNone.Text = vctResource.GetString("labelNone");
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string permissionCode = ((VCTWeb.Core.Domain.RolePermission)e.Row.DataItem).PermissionCode;
                if (permissionCode.Trim() == "Manage.User"
                    || permissionCode.Trim() == "Manage.Role")
                //|| permissionCode.Trim() == "Manage.Password")
                {
                    CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");
                    if (chkSelect != null)
                    {
                        chkSelect.Enabled = false;
                    }
                }
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            this.lblError.Text = string.Empty;
            this.TempRoleList = null;
            presenter.OnViewInitialized();
            this.TempRolePermissionList = this.RolePermissionList;
            this.BindGrid();
            this.BindEntityFilterList();
            this.btnDelete.Enabled = false;
            this.txtRoleName.Focus();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.lblError.Text = string.Empty;
                this.UpdatePermissionDetails();
                this.UpdateRolePermissionListForSave();

                if (Page.IsValid)
                {
                    if (PermissionIsSelected())
                    {
                        Constants.ResultStatus resultStatus = presenter.Save();
                        if (resultStatus == Constants.ResultStatus.Created)
                        {
                            this.TempRoleList = null;
                            this.TempSelectedEntity = "All";
                            presenter.OnViewInitialized();
                            this.TempRolePermissionList = this.RolePermissionList;
                            this.BindGrid();
                            this.BindEntityFilterList();

                            this.lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgCreated"), this.lblHeader.Text) + "</font>";
                        }
                        else if (resultStatus == Constants.ResultStatus.Updated)
                        {
                            this.TempRoleList = null;
                            this.TempSelectedEntity = "All";
                            presenter.OnViewInitialized();
                            this.TempRolePermissionList = this.RolePermissionList;
                            this.BindGrid();
                            this.BindEntityFilterList();
                            this.btnDelete.Enabled = false;
                            this.lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgUpdated"), this.lblHeader.Text) + "</font>";
                        }
                    }
                    else
                        lblError.Text = vctResource.GetString("valPermissionSelection");

                }
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
                if (string.IsNullOrEmpty(this.lblError.Text.Trim()))
                    throw ex;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                this.lblError.Text = string.Empty;

                Constants.ResultStatus resultStatus = presenter.Delete();
                if (resultStatus == Constants.ResultStatus.Deleted)
                {
                    this.TempRoleList = null;
                    this.TempSelectedEntity = "All";
                    presenter.OnViewInitialized();
                    this.TempRolePermissionList = this.RolePermissionList;
                    this.BindGrid();
                    this.BindEntityFilterList();
                    this.btnDelete.Enabled = false;
                    this.lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgDeleted"), this.lblHeader.Text) + "</font>";
                }
            }
            catch (SqlException ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);

                if (this.lblError.Text.Trim() == string.Empty)
                    throw ex;
            }
            catch
            {
                throw;
            }
        }

        protected void lstExistingRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lblError.Text = string.Empty;
            this.TempRolePermissionList = null;
            this.TempSelectedEntity = "All";

            presenter.OnViewLoaded();

            this.TempRolePermissionList = this.RolePermissionList;
            this.BindGrid();
            this.BindEntityFilterList();

            this.SetLastCriterion();

            this.btnDelete.Enabled = true;

            this.txtRoleName.Focus();
        }

        protected void lnkSelectAll_Click(object sender, EventArgs e)
        {
            CheckAllPermissionList(true);
        }

        protected void lnkSelectNone_Click(object sender, EventArgs e)
        {
            CheckAllPermissionList(false);
        }

        private void CheckAllPermissionList(bool isSelect)
        {
            this.UpdateTempCriterion();
            foreach (RolePermission rolePermission in this.TempRolePermissionList)
            {
                rolePermission.GrantPermission = isSelect;
            }
            this.BindGrid();
            this.BindEntityFilterList();

            this.SetLastCriterion();
        }

        protected void ddlSelectEntity_SelectedIndexChanged(object sender, EventArgs e)
        {

            this.UpdateTempCriterion();
            this.UpdatePermissionDetails();

            List<RolePermission> allPermissionList = this.RolePermissionList as List<RolePermission>;
            this.UpdateRolePermissionListForFilter(allPermissionList);

            if (SelectedEntity != "All")
            {
                List<RolePermission> filteredPermissionList =
                     allPermissionList.FindAll(delegate(RolePermission rolePermission) { return rolePermission.Entity.Trim() == SelectedEntity; });

                this.TempRolePermissionList = filteredPermissionList;
                this.BindGrid();
                this.BindEntityFilterList();
            }
            else
            {
                this.TempRolePermissionList = allPermissionList;
                this.BindGrid();
                this.BindEntityFilterList();
            }
            this.SetLastCriterion();
        }

        #endregion	

    }
}

