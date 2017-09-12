using System;
using Microsoft.Practices.ObjectBuilder;
using VCTWebApp.Resources;
using VCTWeb.Core.Domain;
using VCTWebApp.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Globalization;

namespace VCTWebApp.Shell.Views
{
    public partial class ConfigurationSetting : Microsoft.Practices.CompositeWeb.Web.UI.Page, IConfigurationSettingView
    {
        #region Instance Variables

        private ConfigurationSettingPresenter _presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private Helper helper = new Helper();
        public bool isEDI = false;
        private Security security = null;


        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {

            lblError.Text = string.Empty;
            if (!this.IsPostBack)
            {
                helper.LogInformation(User.Identity.Name, "ConfigurationSetting", "Starting initialization");

                this.AuthorizedPage();
                LocalizePage();
                this._presenter.OnViewInitialized();
                this.EnableDisableControls();

                helper.LogInformation(User.Identity.Name, "ConfigurationSetting", "Ending initialization");

            }
        }

        protected void gdvConfigSetting_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            helper.LogInformation(User.Identity.Name, "ConfigurationSetting", "Start Editing");

            if (ViewState["IsEdi"] != null)
            {
                isEDI = (bool)ViewState["IsEdi"];
            }

            gdvConfigSetting.EditIndex = e.NewEditIndex;
            PopulateGrid();
            TextBox txtValue = (TextBox)gdvConfigSetting.Rows[e.NewEditIndex].FindControl("txtValue");
            if (txtValue != null)
                txtValue.Focus();

            helper.LogInformation(User.Identity.Name, "ConfigurationSetting", "End Editing");
        }
        protected void gdvConfigSetting_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            helper.LogInformation(User.Identity.Name, "ConfigurationSetting", "Start Updating");
            try
            {
                string key = gdvConfigSetting.DataKeys[e.RowIndex].Values[0].ToString().Trim();
                string dataType = gdvConfigSetting.DataKeys[e.RowIndex].Values[1].ToString().Trim();
                GridViewRow gridRow = gdvConfigSetting.Rows[e.RowIndex];
                //if (ViewState["IsEdi"] != null)
                //{
                //    isEDI = (bool)ViewState["IsEdi"];
                //}
                //object objValue = null;
                //if (!isEDI)
                //{ objValue = (object)gridRow.FindControl("txtValue"); }
                //else
                //{ objValue = (object)gridRow.FindControl("ddlEdi"); }
                TextBox txtValue = (TextBox)gridRow.FindControl("txtValue");
                TextBox txtIntValue = (TextBox)gridRow.FindControl("txtIntValue");
                TextBox txtPropertyValueDate = (TextBox)gridRow.FindControl("txtPropertyValueDate");
                Image imgCalender = (Image)gridRow.FindControl("imgCalender");
                DropDownList ddlConfig = (DropDownList)gridRow.FindControl("ddlConfig");
                if ((txtValue != null) && (txtPropertyValueDate != null) && (imgCalender != null) && (ddlConfig != null) && (!string.IsNullOrEmpty(key)))
                {
                    _presenter.KeyName = key;
                    //if (!isEDI)
                    //{

                    switch (dataType.ToLower())
                    {
                        case "date":
                            _presenter.KeyValue = txtPropertyValueDate.Text.Trim();
                            break;
                        case "int":
                            _presenter.KeyValue = txtIntValue.Text.Trim();
                            break;
                        case "string":
                            _presenter.KeyValue = txtValue.Text.Trim();
                            break;
                        case "list":
                            _presenter.KeyValue = ddlConfig.SelectedValue.ToString();
                            break;
                        default:
                            break;
                    }

                    //}
                    //else
                    //{
                    //    DropDownList ddlValue = (DropDownList)objValue;
                    //    _presenter.Value = ddlValue.SelectedValue;
                    //}
                    _presenter.DataType = dataType;
                    _presenter.KeyGroup = this.SelectedGroup;
                    Constants.ResultStatus resultStatus = _presenter.Save();
                    if (resultStatus == Constants.ResultStatus.Updated)
                    {
                        gdvConfigSetting.EditIndex = -1;
                        _presenter.KeyGroup = this.SelectedGroup;
                        _presenter.OnViewLoaded();
                        this.lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgUpdated"), this.lblHeader.Text) + "</font>";
                    }
                    else if (resultStatus == Constants.ResultStatus.ConfigurationTypeMismatch)
                    {
                        this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgConfigurationTypeMismatch"), key);
                        //txtValue.Focus();
                    }
                }
            }
            catch
            {
                throw;
            }

            helper.LogInformation(User.Identity.Name, "ConfigurationSetting", "End Updating");
        }


        protected void gdvConfigSetting_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            gdvConfigSetting.EditIndex = -1;
            PopulateGrid();
        }

        protected void gdvConfigSetting_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            gdvConfigSetting.PageIndex = e.NewPageIndex;
            PopulateGrid();
        }

        protected void gdvConfigSetting_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                this.LocalizeGrid(e);
            }
            if (e.Row.RowState == DataControlRowState.Edit)
            {
                //DropDownList ddlEdit = (DropDownList)e.Row.FindControl("ddlEdi");
                //if (ddlEdit != null && isEDI)
                //    ddlEdit.SelectedValue = ViewState["FtpValue"].ToString();
                TextBox txtValue = (TextBox)e.Row.FindControl("txtValue");
                TextBox txtIntValue = (TextBox)e.Row.FindControl("txtIntValue");
                TextBox txtPropertyValueDate = (TextBox)e.Row.FindControl("txtPropertyValueDate");
                Image imgCalender = (Image)e.Row.FindControl("imgCalender");
                DropDownList ddlConfig = (DropDownList)e.Row.FindControl("ddlConfig");

                if (txtPropertyValueDate != null && imgCalender != null && txtValue != null && txtIntValue != null && ddlConfig != null)
                {
                    switch (((Configuration)e.Row.DataItem).DataType.Trim().ToLower())
                    {
                        case "date":
                            txtValue.Visible = false;
                            txtIntValue.Visible = false;
                            ddlConfig.Visible = false;
                            break;
                        case "int":
                            txtValue.Visible = false;
                            txtPropertyValueDate.Visible = false;
                            imgCalender.Visible = false;
                            ddlConfig.Visible = false;
                            break;
                        case "string":
                            txtPropertyValueDate.Visible = false;
                            txtIntValue.Visible = false;
                            imgCalender.Visible = false;
                            ddlConfig.Visible = false;
                            break;
                        case "list":
                            txtValue.Visible = false;
                            txtIntValue.Visible = false;
                            txtPropertyValueDate.Visible = false;
                            imgCalender.Visible = false;
                            string[] configurationListValues;
                            string configurationListValue = (((Configuration)e.Row.DataItem).ListValues);
                            configurationListValues = configurationListValue.Split(',');
                            ddlConfig.DataSource = configurationListValues;
                            ddlConfig.DataBind();
                            ddlConfig.SelectedValue = (((Configuration)e.Row.DataItem).KeyValue);
                            break;
                        default:
                            break;
                    }
                }
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //System.Web.UI.WebControls.Label lblValue = (System.Web.UI.WebControls.Label)e.Row.FindControl("lblValue");
                //System.Web.UI.WebControls.Label lblKey = (System.Web.UI.WebControls.Label)e.Row.FindControl("lblKey");
                //if (lblKey.Text.ToLower() == "ftpchoice")
                //{
                //    if (lblValue != null)
                //    {
                //        ViewState["FtpValue"] = lblValue.Text.Trim();
                //        if (lblValue.Text.Trim().ToLower() == "true")
                //            lblValue.Text = "FTP";
                //        else if (lblValue.Text.Trim().ToLower() == "false")
                //            lblValue.Text = "Local";
                //    }
                //}

                TextBox txtValue = (TextBox)e.Row.FindControl("txtValue");
                TextBox txtIntValue = (TextBox)e.Row.FindControl("txtIntValue");
                TextBox txtPropertyValueDate = (TextBox)e.Row.FindControl("txtPropertyValueDate");
                Image imgCalender = (Image)e.Row.FindControl("imgCalender");
                DropDownList ddlConfig = (DropDownList)e.Row.FindControl("ddlConfig");

                if (txtPropertyValueDate != null && imgCalender != null && txtValue != null && txtIntValue != null && ddlConfig != null)
                {
                    switch (((Configuration)e.Row.DataItem).DataType.Trim().ToLower())
                    {
                        case "date":
                            txtValue.Visible = false;
                            txtIntValue.Visible = false;
                            ddlConfig.Visible = false;
                            break;
                        case "int":
                            txtValue.Visible = false;
                            txtPropertyValueDate.Visible = false;
                            imgCalender.Visible = false;
                            ddlConfig.Visible = false;
                            break;
                        case "string":
                            txtPropertyValueDate.Visible = false;
                            txtIntValue.Visible = false;
                            imgCalender.Visible = false;
                            ddlConfig.Visible = false;
                            break;
                        case "list":
                            txtValue.Visible = false;
                            txtIntValue.Visible = false;
                            txtPropertyValueDate.Visible = false;
                            imgCalender.Visible = false;
                            string[] configurationListValues;
                            string configurationListValue = (((Configuration)e.Row.DataItem).ListValues);
                            configurationListValues = configurationListValue.Split(',');
                            ddlConfig.DataSource = configurationListValues;
                            ddlConfig.DataBind();
                            ddlConfig.SelectedValue = (((Configuration)e.Row.DataItem).KeyValue);
                            break;
                        default:
                            break;
                    }
                }

                LinkButton lnkUpdate = (LinkButton)e.Row.FindControl("lnkUpdate");
                if (lnkUpdate != null)
                    lnkUpdate.Text = vctResource.GetString("lnkUpdate");

                LinkButton lnkCancel = (LinkButton)e.Row.FindControl("lnkCancel");
                if (lnkCancel != null)
                    lnkCancel.Text = vctResource.GetString("lnkCancel");

                LinkButton lnkEdit = (LinkButton)e.Row.FindControl("lnkEdit");
                if (lnkEdit != null)
                {
                    Configuration configuration = e.Row.DataItem as Configuration;
                    if (configuration != null)
                        lnkEdit.Enabled = configuration.Editable;
                }
            }
        }
        protected void lstExistingConfigSetting_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedGroup = lstExistingConfigSetting.SelectedValue;
            _presenter.KeyGroup = this.SelectedGroup;
            //if (SelectedGroup.ToLower().Contains("edi"))
            //    isEDI = true;
            //else
            //    isEDI = false;
            //ViewState["IsEdi"] = isEDI;
            gdvConfigSetting.EditIndex = -1;
            _presenter.OnViewLoaded();
            this.EnableDisableControls();
        }

        protected void gdvConfigSetting_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName.ToLower() == "edit")
            //{
            //    if (e.CommandArgument.ToString().ToLower() == "ftpchoice")
            //    {
            //        isEDI = true;
            //    }
            //    else
            //    {
            //        isEDI = false;

            //    }
            //    ViewState["IsEdi"] = isEDI;
            //}
        }

        #endregion

        #region Public Proprties

        [CreateNew]
        public ConfigurationSettingPresenter Presenter
        {
            get
            {
                return this._presenter;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this._presenter = value;
                this._presenter.View = this;
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
            else if (security.HasAccess("Configuration"))
            {
                CanEdit = security.HasPermission("Configuration.Manage");
            }
            else
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }

        private void EnableDisableControls()
        {

            EnableAllPanels();
            if (!CanEdit)
            {
                pnlConfigSettings.Enabled = false;
            }
        }

        private void EnableAllPanels()
        {
            pnlConfigSettings.Enabled = true;

        }

        private void DisableAllPanels()
        {
            pnlConfigSettings.Enabled = false;
        }

        private void LocalizePage()
        {
            try
            {
                string heading = string.Empty;
                heading = vctResource.GetString("mnuConfigurationSettings");
                lblHeader.Text = heading;
                Page.Title = heading;

                lblListConfigSetting.Text = vctResource.GetString("labelExistingConfigSetting");
            }
            catch
            {
                throw;
            }

        }
        private void LocalizeGrid(GridViewRowEventArgs e)
        {
            try
            {
                e.Row.Cells[0].Text = vctResource.GetString("labelConfigDes");
                e.Row.Cells[2].Text = vctResource.GetString("labelConfigValue");
                e.Row.Cells[3].Text = vctResource.GetString("labelAction");
            }
            catch
            {
                throw;
            }
        }
        private void PopulateGrid()
        {

            gdvConfigSetting.DataSource = this.ConfigurationGroupWiseList;
            gdvConfigSetting.DataBind();
        }
        private void BindEmptyGrid()
        {
            List<Configuration> configurationList = new List<Configuration>();
            Configuration configuration = new Configuration();
            configurationList.Add(configuration);
            gdvConfigSetting.DataSource = configurationList;
            gdvConfigSetting.DataBind();
            gdvConfigSetting.Rows[0].Visible = false;
        }

        #endregion

        #region Private Proprties

        private string SelectedGroup
        {
            get { return ((this.ViewState["SelectedGroup"] == null) ? string.Empty : Convert.ToString(this.ViewState["SelectedGroup"])); }

            set { this.ViewState["SelectedGroup"] = value; }
        }

        private List<Configuration> ConfigurationGroupWiseList
        {
            get { return ((this.ViewState["ConfigurationGroupWiseList"] == null) ? new List<Configuration>() : (List<Configuration>)(this.ViewState["ConfigurationGroupWiseList"])); }

            set { this.ViewState["ConfigurationGroupWiseList"] = value; }
        }

        private bool CanEdit
        {
            get
            {
                return ViewState[Common.CAN_EDIT] != null ? (bool)ViewState[Common.CAN_EDIT] : false;
            }
            set
            {
                ViewState[Common.CAN_EDIT] = value;
            }
        }
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

        #endregion

        #region IConfigurationSettingView Members

        public List<Configuration> ConfigurationGroupList
        {
            set
            {
                lstExistingConfigSetting.DataSource = value;
                lstExistingConfigSetting.DataTextField = "KeyGroup";
                lstExistingConfigSetting.DataValueField = "KeyGroup";
                lstExistingConfigSetting.DataBind();
                BindEmptyGrid();
            }
        }

        public List<Configuration> ConfigurationsByGroupList
        {
            set
            {
                if (value != null && value.Count > 0)
                {
                    this.ConfigurationGroupWiseList = value;
                    PopulateGrid();
                }
                else
                {
                    BindEmptyGrid();
                }
            }
        }

        #endregion

    }
}

