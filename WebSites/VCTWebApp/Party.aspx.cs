using System;
using Microsoft.Practices.ObjectBuilder;
using VCTWebApp.Resources;
using VCTWeb.Core.Domain;
using VCTWebApp.Web;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Web;

namespace VCTWebApp.Shell.Views
{
    public partial class Party : Microsoft.Practices.CompositeWeb.Web.UI.Page, IPartyView
    {
        #region Instance Variables

        private PartyPresenter presenter;
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

        #endregion

        #region Private Methods

        private void AuthorizedPage()
        {
            security = new Security();
            if (Session["LoggedInUser"] == null)
            {
                Response.Redirect("~/ErrorPage.aspx?ErrorKey=Common_msgSessionExpired");
            }
            else if (security.HasAccess("Party"))
            {
                CanView = security.HasPermission("Party.Manage");
            }
            else
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }

        private void LocalizePage()
        {
            try
            {
                string heading = string.Empty;
                heading = vctResource.GetString("mnuParty");
                lblHeader.Text = heading;
                Page.Title = heading;

                this.lblExistingParties.Text = vctResource.GetString("labelExistingParties");
                this.lblPartyName.Text = vctResource.GetString("labelPartyName");
                this.lblPartyCode.Text = vctResource.GetString("labelPartyCode");
                this.lblDescription.Text = vctResource.GetString("labelDescription");
                this.lblPartyType.Text = vctResource.GetString("labelPartyType");
                this.lblCompanyPrefix.Text = vctResource.GetString("labelCompanyPrefix");
                this.chkOwner.Text = vctResource.GetString("labelOwner");
                this.chkActive.Text = vctResource.GetString("labelActive");

                //this.btnNew.Text = vctResource.GetString("btnReset");
                //this.btnSave.Text = vctResource.GetString("btnSave");
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            if ((ddlCountry.SelectedIndex <= 0) && (pnlAddress.Enabled == false))
            {
                lblError.Text = vctResource.GetString("valCountry");
                ddlCountry.Focus();
                return false;
            }
            if (this.PartyLocationIds == string.Empty)
            {
                lblError.Text = vctResource.GetString("valSelectOne");
                return false;
            }
            return true;
        }

        private void DisplayMessageForMissingMasters()
        {

        }

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
                    this.LocalizePage();
                    this.Form.DefaultButton = this.btnSave.UniqueID; //Set the default button to save.

                    this.DisplayMessageForMissingMasters();

                    this.txtName.Focus();
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
        public PartyPresenter Presenter
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

        #region IPartyView Members

        public bool IsActive
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

        public bool Owner
        {
            get
            {
                return this.chkOwner.Checked;
            }
            set
            {
                this.chkOwner.Checked = value;
            }
        }

        public Int64 SelectedPartyId
        {
            get
            {
                if (this.lstExistingParties.SelectedIndex >= 0)
                    return Convert.ToInt64(this.lstExistingParties.SelectedValue);
                else
                    return 0;
            }
        }

        public List<VCTWeb.Core.Domain.Party> PartyList
        {
            set
            {
                this.lstExistingParties.DataSource = value;
                this.lstExistingParties.DataTextField = "Name";
                this.lstExistingParties.DataValueField = "PartyId";
                this.lstExistingParties.DataBind();
            }
        }

        public string Name
        {
            get
            {
                return this.txtName.Text.Trim();
            }
            set
            {
                partyName.Value = value;
                this.txtName.Text = value;
            }
        }

        public string Code
        {
            get
            {
                return this.txtCode.Text.Trim();
            }
            set
            {
                this.txtCode.Text = value;
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

        public Int64 PartyTypeId
        {
            get
            {
                return Convert.ToInt64(this.ddlPartyType.SelectedValue);
            }
            set
            {
                if (value == 0)
                    this.ddlPartyType.SelectedIndex = -1;
                else
                    this.ddlPartyType.SelectedValue = value.ToString();
            }
        }

        //public int? LinkedLocationId
        //{
        //    get
        //    {
        //        if (ddlLinkedLocation.SelectedIndex > 0)
        //        {
        //            return Convert.ToInt32(ddlLinkedLocation.SelectedValue);
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    set
        //    {
        //        if (value == null || value == 0)
        //            this.ddlLinkedLocation.SelectedIndex = 0;
        //        else
        //            this.ddlLinkedLocation.SelectedValue = value.ToString();
        //    }
        //}

        public List<PartyType> PartyTypeList
        {
            set
            {
                this.ddlPartyType.DataSource = value;
                this.ddlPartyType.DataTextField = "Name";
                this.ddlPartyType.DataValueField = "PartyTypeId";
                this.ddlPartyType.DataBind();
                this.ddlPartyType.SelectedIndex = -1;
            }
        }

        //public List<Location> LinkedLocationList
        //{
        //    set
        //    {
        //        this.ddlLinkedLocation.DataSource = value;
        //        this.ddlLinkedLocation.DataTextField = "LocationTypeLocationName";
        //        this.ddlLinkedLocation.DataValueField = "LocationId";
        //        this.ddlLinkedLocation.DataBind();
        //        this.ddlLinkedLocation.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect")));

        //        this.gdvPartyLocation.DataSource = value;
        //        this.gdvPartyLocation.DataBind();
        //    }
        //}

        public List<PartyLinkedLocation> PartyLinkedLocationList
        {
            set
            {
                this.gdvPartyLocation.DataSource = value;
                this.gdvPartyLocation.DataBind();
            }
        }

        public string CompanyPrefix
        {
            get
            {
                return this.txtCompanyPrefix.Text.Trim();
            }
            set
            {
                this.txtCompanyPrefix.Text = value;
            }
        }

        public int ShippingDaysGap
        {
            get { return Convert.ToInt32(txtShippingDaysGap.Text); }
            set { txtShippingDaysGap.Text = value.ToString(); }
        }

        public int RetrievalDaysGap
        {
            get { return Convert.ToInt32(txtRetrievalDaysGap.Text); }
            set { txtRetrievalDaysGap.Text = value.ToString(); }
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
                if (value == null)
                    ddlCountry.SelectedIndex = 0;
                else
                    this.ddlCountry.SelectedIndex = this.ddlCountry.Items.IndexOf(this.ddlCountry.Items.FindByText(value));
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

        public string PartyLocationIds
        {
            get
            {
                string locationIds=string.Empty;
                foreach (GridViewRow row in gdvPartyLocation.Rows)
                {
                    CheckBox chkSelect= row.FindControl("chkSelect") as CheckBox;
                    if (chkSelect.Checked)
                    {
                        HiddenField hdnLocationId= row.FindControl("hdnLocationId") as HiddenField;
                        locationIds += (locationIds == string.Empty ? string.Empty : ",") + hdnLocationId.Value;
                    }
                }
                return locationIds;
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

        #endregion

        #region Event Handlers

        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                this.lblError.Text = string.Empty;
                presenter.OnViewInitialized();
                this.txtName.Focus();
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
                if (Page.IsValid && IsValidInput())
                {
                    Constants.ResultStatus resultStatus = presenter.Save(partyName.Value); if (resultStatus == Constants.ResultStatus.Ok)
                    {
                        presenter.OnViewInitialized();
                        this.txtName.Focus();

                        this.lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgSave"), this.lblHeader.Text) + "</font>";
                    }
                    else if (resultStatus == Constants.ResultStatus.InUse)
                    {
                        this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgInUseParty"), this.lblHeader.Text);
                    }
                    else if (resultStatus == Constants.ResultStatus.Duplicate)
                    {
                        //this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgInUseParty"), this.lblHeader.Text);
                        this.lblError.Text = "<font color='red'>" +"Party already exists" + "</font>";
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

        protected void lstExistingParties_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.lblError.Text = string.Empty;
                presenter.OnViewLoaded();
                this.txtName.Focus();
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

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "Party page", "ddlCountry_SelectedIndexChanged() is invoked.");
            if (string.Compare(this.Country.Trim(), "USA") == 0)
            {
                this.txtState.Visible = false;
                this.ddlState.Visible = true;
                presenter.LoadStates();

            }
            else
            {
                this.txtState.Visible = true;
                this.ddlState.Visible = false;
            }
        }

        #endregion
    }
}

