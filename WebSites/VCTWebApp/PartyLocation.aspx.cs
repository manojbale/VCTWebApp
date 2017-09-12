using System;
using Microsoft.Practices.ObjectBuilder;
using VCTWebApp.Resources;
using VCTWeb.Core.Domain;
using VCTWebApp.Web;
using System.Web;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace VCTWebApp.Shell.Views
{
    public partial class PartyLocation : Microsoft.Practices.CompositeWeb.Web.UI.Page, IPartyLocationView
    {
        #region Instance Variables

        private PartyLocationPresenter presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private Helper helper = new Helper();
        private Security security = null;

        #endregion

        #region Private Properties

        private bool CanAdd
        {
            get
            {
                return ViewState[Common.CAN_ADD] != null ? (bool)ViewState[Common.CAN_ADD] : false;
            }
            set
            {
                ViewState[Common.CAN_ADD] = value;
            }
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
        private bool CanDelete
        {
            get
            {
                return ViewState[Common.CAN_DELETE] != null ? (bool)ViewState[Common.CAN_DELETE] : false;
            }
            set
            {
                ViewState[Common.CAN_DELETE] = value;
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

        #region Private Methods

        private void AuthorizedPage()
        {
            security = new Security();
            if (Session["LoggedInUser"] == null)
            {
                Response.Redirect("~/ErrorPage.aspx?ErrorKey=Common_msgSessionExpired");
            }
            else if (security.HasAccess("PartyLocation"))
            {
                CanView = security.HasPermission("PartyLocation.Manage");
                CanAdd = security.HasPermission("PartyLocation.Manage");
                CanEdit = security.HasPermission("PartyLocation.Manage");
                CanDelete = security.HasPermission("PartyLocation.Manage");
            }
            else
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }

        private void EnableDisableControls(Constants.CurrentMode currentMode)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyLocation page", "EnabledDisableControls() is invoked.");
            if (currentMode == Constants.CurrentMode.Add)
            {
                EnableAllPanels();
                if (!CanAdd)
                {
                    DisableAllPanels();
                }
                //else
                //{
                //    btnDelete.Enabled = false;
                //}
            }
            else if (currentMode == Constants.CurrentMode.Edit)
            {
                EnableAllPanels();
                if (!CanEdit)
                {
                    pnlPartyLocationDetail.Enabled = false;
                    pnlLocationAttributesDetail.Enabled = false;
                    pnlAddress.Enabled = false;
                    btnSave.Enabled = false;
                }
                if (!CanAdd)
                {
                    btnSave.Enabled = false;
                }
                //{
                //    if (!CanDelete)
                //        btnDelete.Enabled = false;
                //}
            }
            else
            {
                DisableAllPanels();
            }
        }

        private void EnableAllPanels()
        {
            pnlPartyLocationDetail.Enabled = true;
            pnlLocationAttributesDetail.Enabled = true;
            pnlAddress.Enabled = true;
            pnlButtonOnly.Enabled = true;

            //if (this.PartyId > 0 && String.IsNullOrEmpty(this.LocationName))
            //    btnDelete.Enabled = false;
            //else
            //    btnDelete.Enabled = true;
            btnSave.Enabled = true;
        }

        private void DisableAllPanels()
        {
            pnlPartyLocationDetail.Enabled = false;
            pnlLocationAttributesDetail.Enabled = false;
            pnlAddress.Enabled = false;
            pnlButtonOnly.Enabled = false;
        }

        private void LocalizePage()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyLocation page", "LocalizePage() is invoked.");
            try
            {
                string heading = string.Empty;
                heading = vctResource.GetString("mnuPartyLocation");
                lblHeader.Text = heading;
                Page.Title = heading;
                this.lblExistingPartyLocations.Text = vctResource.GetString("labelExistingLocation");
                this.lblParty.Text = vctResource.GetString("labelParty");
                this.lblLocationType.Text = vctResource.GetString("labelLocationType");
                this.lblLocationAttributes.Text = vctResource.GetString("labelLocationAttributes");
                this.lblLocationName.Text = vctResource.GetString("labelLocationName");
                this.lblLocationCode.Text = vctResource.GetString("labelLocationCode");
                this.lblGLN.Text = vctResource.GetString("labelGLN");
                this.lblDescription.Text = vctResource.GetString("labelDescription");
                this.lblLatitude.Text = vctResource.GetString("labelLatitude");
                this.lblLongitude.Text = vctResource.GetString("labelLongitude");


                this.lblLocationAddress.Text = vctResource.GetString("labelLocationAddress");
                this.lblAddress1.Text = vctResource.GetString("labelAddress1");
                this.lblAddress2.Text = vctResource.GetString("labelAddress2");
                this.lblCity.Text = vctResource.GetString("labelCity");
                this.lblCountry.Text = vctResource.GetString("labelCountry");
                this.lblZipCode.Text = vctResource.GetString("labelZip");
                this.lblState.Text = vctResource.GetString("labelState");
                //this.lblRegion.Text = vctResource.GetString("lblRegion");

                //this.btnDelete.Text = vctResource.GetString("btnDelete");
                //this.btnReset.Text = vctResource.GetString("btnReset");
                //this.btnSave.Text = vctResource.GetString("btnSave");

            }
            catch
            {
                throw;
            }
        }

        private void DisableAddress(bool disabled)
        {
            txtAddress1.Enabled = !disabled;
            txtAddress2.Enabled = !disabled;
            txtCity.Enabled = !disabled;
            txtZipCode.Enabled = !disabled;
            ddlCountry.Enabled = !disabled;
            //ddlRegion.Enabled = !disabled;
            //txtRegion.Enabled = !disabled;
            txtState.Enabled = !disabled;
            ddlState.Enabled = !disabled;
            lblCountry.Font.Bold = !disabled;
        }

        private bool IsValidInput()
        {
            try
            {
                if (!String.IsNullOrEmpty(txtLatitude.Text.Trim()))
                {
                    decimal d = Convert.ToDecimal(txtLatitude.Text.Trim());
                }
            }
            catch
            {
                lblError.Text = vctResource.GetString("valLatitude");
                txtLatitude.Focus();
                return false;
            }
            try
            {
                if (!String.IsNullOrEmpty(txtLongitude.Text.Trim()))
                {
                    decimal d = Convert.ToDecimal(txtLongitude.Text.Trim());
                }
            }
            catch
            {
                lblError.Text = vctResource.GetString("valLongitude");
                txtLongitude.Focus();
                return false;
            }
            if (ddlCountry.SelectedIndex <= 0)
            {
                lblError.Text = vctResource.GetString("valCountry");
                ddlCountry.Focus();
                return false;
            }
            //if ((ddlRegion.Visible && ddlRegion.SelectedIndex <= 0) || (txtRegion.Visible && txtRegion.Text.Trim() == string.Empty))
            //{
            //    lblError.Text = vctResource.GetString("valRegion");
            //    if (ddlRegion.Visible)
            //        ddlRegion.Focus();
            //    else
            //        txtRegion.Focus();
            //    return false;
            //}
            return true;
        }

        //private void DisplayMessageForMissingMasters()
        //{
        //    //helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyLocation page", "DisplayMessageForMissingMasters() is invoked.");
        //    //if (this.tvwPartyLocation.Nodes.Count <= 0)
        //    //{
        //    //    this.lblError.Text = vctResource.GetString("msgDefineMaster");
        //    //    this.lblError.Text += "<br />" + vctResource.GetString("mnuParty");
        //    //}
        //}

        #endregion

        #region Init/Page Load

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    this.AuthorizedPage();

                    this.presenter.OnViewInitialized();
                    if (lstPartyLocation.Items.Count > 0)
                    {
                        lstPartyLocation.SelectedIndex = 0;
                    }
                    presenter.OnViewLoaded();
                    this.lblError.Text = string.Empty;
                    this.LocalizePage();

                    this.EnableDisableControls(Constants.CurrentMode.Add);

                    //btnDelete.Attributes.Add("onclick", "javascript:return " + "confirm('" + vctResource.GetString("msgDeleteConfirm") + "')");

                    //this.txtDescription.Attributes.Add("onKeyDown", "javascript:LimitText(this, 255)");
                    //this.txtDescription.Attributes.Add("onKeyUp", "javascript:LimitText(this, 255)");
                    //this.txtDescription.Attributes.Add("onpaste", "javascript:PreventPaste(this, 255)");

                    this.Form.DefaultButton = this.btnSave.UniqueID; //Set the default button to save.

                    //this.DisplayMessageForMissingMasters();
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
        public PartyLocationPresenter Presenter
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

        #region IPartyLocationView Members

        //public TreeNodeCollection PartyLocationNodeList
        //{
        //    get
        //    {
        //        //return this.tvwPartyLocation.Nodes;
        //        return null;
        //    }
        //    set
        //    {
        //        //this.tvwPartyLocation.Nodes.Clear();
        //        //foreach (TreeNode node in value)
        //        //{
        //        //    this.tvwPartyLocation.Nodes.Add(node);
        //        //}
        //        //txtLocationName.Focus();
        //    }
        //}

        public List<VCTWeb.Core.Domain.Party> PartyLocationList
        {
            set
            {
                this.lstPartyLocation.DataSource = value;
                this.lstPartyLocation.DataTextField = "Name";
                this.lstPartyLocation.DataValueField = "PartyId";
                this.lstPartyLocation.DataBind();
            }
        }

        public List<Country> CountryList
        {
            set
            {
                this.ddlCountry.DataSource = value;
                this.ddlCountry.DataTextField = "CountryName";
                this.ddlCountry.DataValueField = "CountryCode";

                this.ddlCountry.DataBind();

                this.ddlCountry.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect")));

                //Set the initial display state and region field text box
                //this.txtRegion.Visible = true;
                //this.ddlRegion.Visible = false;
                this.txtState.Visible = true;
                this.ddlState.Visible = false;
            }
        }

        public List<State> StateList
        {
            set
            {
                this.ddlState.DataSource = value;
                this.ddlState.DataTextField = "StateName";
                this.ddlState.DataValueField = "StateName";

                this.ddlState.DataBind();

                this.ddlState.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect"), string.Empty));
            }
        }

        public string Party
        {
            set
            {
                this.txtParty.Text = value;
            }
        }

        public string LocationType
        {
            get
            {
                return this.txtLocationType.Text.Trim();
            }
            set
            {
                this.txtLocationType.Text = value;
            }
        }

        public string LocationName
        {
            get
            {
                return this.txtLocationName.Text.Trim();
            }
            set
            {
                this.txtLocationName.Text = value;
            }
        }

        public string LocationCode
        {
            get
            {
                return this.txtLocationCode.Text.Trim();
            }
            set
            {
                this.txtLocationCode.Text = value;
            }
        }

        public string GLN
        {
            get
            {
                return this.txtGLN.Text.Trim();
            }
            set
            {
                this.txtGLN.Text = value;
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

        public string Latitude
        {
            get
            {
                return this.txtLatitude.Text.Trim();
            }
            set
            {
                this.txtLatitude.Text = value;
            }
        }

        public string Longitude
        {
            get
            {
                return this.txtLongitude.Text.Trim();
            }
            set
            {
                this.txtLongitude.Text = value;
            }
        }

        public string Address1
        {
            get
            {
                return this.txtAddress1.Text.Trim();
            }
            set
            {
                this.txtAddress1.Text = value;
            }
        }

        public string Address2
        {
            get
            {
                return this.txtAddress2.Text.Trim();
            }
            set
            {
                this.txtAddress2.Text = value;
            }
        }

        public string City
        {
            get
            {
                return this.txtCity.Text.Trim();
            }
            set
            {
                this.txtCity.Text = value;
            }
        }

        public string Country
        {
            get
            {
                return this.ddlCountry.SelectedItem.Text;
            }
            set
            {
                this.ddlCountry.SelectedIndex = this.ddlCountry.Items.IndexOf(this.ddlCountry.Items.FindByText(value));

                //if (string.Compare(this.Country.Trim(), "USA") == 0)
                //{
                //    lblState.Font.Bold = true;
                //    lblZipCode.Font.Bold = true;

                //}
                //else
                //{
                //    lblState.Font.Bold = false;
                //    lblZipCode.Font.Bold = false;
                //}

            }
        }

        public string ZipCode
        {
            get
            {
                return this.txtZipCode.Text.Trim();
            }
            set
            {
                this.txtZipCode.Text = value;
            }
        }

        //public string Region
        //{
        //    get
        //    {
        //        //if (string.Compare(this.Country.Trim(), "USA") == 0)
        //        //{
        //        //    return this.ddlRegion.SelectedValue;
        //        //}
        //        //else
        //        //{
        //        //    return this.txtRegion.Text.Trim();
        //        //}
        //        return null;
        //    }
        //    set
        //    {
        //        //if (string.Compare(this.Country.Trim(), "USA") == 0)
        //        //{
        //        //    this.txtRegion.Visible = false;
        //        //    this.ddlRegion.Visible = true;
        //        //    this.ddlRegion.SelectedIndex = this.ddlRegion.Items.IndexOf(this.ddlRegion.Items.FindByValue(value));
        //        //}
        //        //else
        //        //{
        //        //    this.txtRegion.Visible = true;
        //        //    this.ddlRegion.Visible = false;
        //        //    this.txtRegion.Text = value;
        //        //}
        //    }
        //}

        public string State
        {
            get
            {
                if (string.Compare(this.Country.Trim(), "USA") == 0)
                {
                    return this.ddlState.SelectedValue;
                }
                else
                {
                    return this.txtState.Text.Trim();
                }
            }
            set
            {
                if (string.Compare(this.Country.Trim(), "USA") == 0)
                {
                    this.txtState.Visible = false;
                    this.ddlState.Visible = true;
                    this.ddlState.SelectedIndex = this.ddlState.Items.IndexOf(this.ddlState.Items.FindByValue(value));
                }
                else
                {
                    this.txtState.Visible = true;
                    this.ddlState.Visible = false;
                    this.txtState.Text = value;
                }
            }
        }

        public bool RequiresAddress
        {
            get
            {
                return (bool)ViewState["RequiresAddress"];
            }
            set
            {
                ViewState["RequiresAddress"] = value;
                this.DisableAddress(!value);
            }
        }

        public bool IsActive
        {
            get
            {
                return chkIsActive.Checked;
            }
            set
            {
                chkIsActive.Checked = value;
            }
        }

        public long SelectedPartyId
        {
            get
            {
                return (lstPartyLocation.SelectedIndex >= 0 ? Convert.ToInt64(lstPartyLocation.SelectedValue) : 0);
            }
        }

        public string SelectedPartyName
        {
            get
            {
                return (lstPartyLocation.SelectedIndex >= 0 ? lstPartyLocation.SelectedItem.Text : string.Empty);
            }
        }

        //public long ParentLocationId
        //{
        //    get
        //    {
        //        return Convert.ToInt64(ViewState["ParentLocationId"]);
        //    }
        //    set
        //    {
        //        ViewState["ParentLocationId"] = value;
        //    }
        //}

        public int LocationId
        {
            get
            {
                return Convert.ToInt32(ViewState["LocationId"]);
            }
            set
            {
                ViewState["LocationId"] = value;
            }
        }

        public int AddressId
        {
            get
            {
                return Convert.ToInt32(ViewState["AddressId"]);
            }
            set
            {
                ViewState["AddressId"] = value;
            }
        }

        public LocationType PartyLocationType
        {
            get
            {
                return (LocationType)(ViewState["PartyLocationType"]);
            }
            set
            {
                ViewState["PartyLocationType"] = value;
            }
        }

        #endregion

        #region Event Handlers

        protected void lstPartyLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblError.Text = string.Empty;
            Presenter.OnViewLoaded();
        }

        //protected void tvwPartyLocation_SelectedNodeChanged(object sender, EventArgs e)
        //{
        //    //helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyLocation page", "tvwPartyLocation_SelectedNodeChanged() is invoked.");

        //    //presenter.SelectedPartyLocationValue = tvwPartyLocation.SelectedNode.ValuePath;
        //    //if (tvwPartyLocation.SelectedNode.Parent != null)
        //    //{
        //    //    presenter.SelectedParty = tvwPartyLocation.SelectedNode.Parent.Text;
        //    //}
        //    //presenter.OnViewLoaded();
        //    //this.lblError.Text = string.Empty;
        //    //if (this.PartyId > 0)
        //    //{
        //    //    if (String.IsNullOrEmpty(this.LocationName))
        //    //        this.EnableDisableControls(Constants.CurrentMode.Add);
        //    //    else
        //    //        this.EnableDisableControls(Constants.CurrentMode.Edit);
        //    //}
        //    //else
        //    //{
        //    //    this.EnableDisableControls(Constants.CurrentMode.View);
        //    //}
        //}

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyLocation page", "btnDelete_Click() is invoked.");
            try
            {
                Constants.ResultStatus resultStatus = presenter.Delete();
                if (resultStatus == Constants.ResultStatus.Deleted)
                {
                    presenter.OnViewInitialized();
                    //this.EnableDisableControls(Constants.CurrentMode.View);
                    this.lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgDeleted"), this.lblHeader.Text) + "</font>";
                }
                //this.tvwPartyLocation.ExpandAll();
            }
            catch (SqlException ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
            catch
            {
                throw;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyLocation page", "btnSave_Click() is invoked.");
            try
            {
                this.lblError.Text = string.Empty;
                if (Page.IsValid && IsValidInput())
                {
                    Constants.ResultStatus resultStatus = presenter.Save();
                    if (resultStatus == Constants.ResultStatus.Created)
                    {
                        presenter.OnViewInitialized();
                        if (lstPartyLocation.Items.Count > 0)
                        {
                            lstPartyLocation.SelectedIndex = 0;
                        }
                        presenter.OnViewLoaded();
                        //this.EnableDisableControls(Constants.CurrentMode.View);
                        this.lblError.Text += "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgCreated"), this.lblHeader.Text) + "</font>";

                    }
                    else if (resultStatus == Constants.ResultStatus.Updated)
                    {
                        presenter.OnViewInitialized();
                        if (lstPartyLocation.Items.Count > 0)
                        {
                            lstPartyLocation.SelectedIndex = 0;
                        }
                        presenter.OnViewLoaded();
                        //this.EnableDisableControls(Constants.CurrentMode.View);
                        this.lblError.Text += "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgUpdated"), this.lblHeader.Text) + "</font>";
                    }
                    else if (resultStatus == Constants.ResultStatus.DuplicateLocationName)
                    {
                        this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgLocationCode"), this.lblHeader.Text);
                    }
                    else if (resultStatus == Constants.ResultStatus.MissingPropertyValue)
                    {
                        this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgAttributeValue"), this.lblHeader.Text);
                    }
                    else if (resultStatus == Constants.ResultStatus.InvalidPropertyValue)
                    {
                        this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgInvalidAttributeValue"), this.lblHeader.Text);
                    }
                    //this.tvwPartyLocation.ExpandAll();
                }
            }
            catch (SqlException ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
            catch
            {
                throw;
            }
        }

        //protected void btnReset_Click(object sender, EventArgs e)
        //{
        //    helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyLocation page", "btnReset_Click() is invoked.");

        //    presenter.OnViewInitialized();
        //    //this.EnableDisableControls(Constants.CurrentMode.View);
        //}

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "PartyLocation page", "ddlCountry_SelectedIndexChanged() is invoked.");
            if (string.Compare(this.Country.Trim(), "USA") == 0)
            {
                this.txtState.Visible = false;
                this.ddlState.Visible = true;
                presenter.LoadStates();
                //this.txtRegion.Visible = false;
                //this.ddlRegion.Visible = true;
                //presenter.LoadRegions();

            }
            else
            {
                this.txtState.Visible = true;
                this.ddlState.Visible = false;
                //this.txtRegion.Visible = true;
                //this.ddlRegion.Visible = false;
            }
        }

        #endregion


    }
}

