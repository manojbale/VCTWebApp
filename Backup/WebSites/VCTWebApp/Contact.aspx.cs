using System;
using Microsoft.Practices.ObjectBuilder;
using VCTWebApp.Resources;
using VCTWeb.Core.Domain;
using VCTWebApp.Web;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Linq;

namespace VCTWebApp.Shell.Views
{
    public partial class Contact : Microsoft.Practices.CompositeWeb.Web.UI.Page, IContactView
    {
        #region Instance Variables

        private ContactPresenter presenter;
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
            else if (security.HasAccess("Contact"))
            {
                CanView = security.HasPermission("Contact.Manage");
            }
            else
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }

        private void LocalizePage()
        {
            try
            {
                string heading = string.Empty;
                heading = vctResource.GetString("mnuContact");
                lblHeader.Text = heading;
                Page.Title = heading;

                this.lblExistingContatcts.Text = vctResource.GetString("labelExistingContacts");
                this.lblFirstName.Text = vctResource.GetString("labelFirstName");
                this.lblLastName.Text = vctResource.GetString("labelLastName");
                this.lblEmailId.Text = vctResource.GetString("labelPrimaryEmailId");
                this.lblPhone.Text = vctResource.GetString("labelPhone");
                this.lblCell.Text = vctResource.GetString("labelCell");
                this.lblFax.Text = vctResource.GetString("labelFax");
                //this.lblSalesOffice.Text = vctResource.GetString("lblSalesOffice");
                this.chkActive.Text = vctResource.GetString("labelActive");

                //this.btnNew.Text = vctResource.GetString("btnReset");
                //this.btnSave.Text = vctResource.GetString("btnSave");
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                    //this.ShowBadgeNumberControls();
                    this.Form.DefaultButton = this.btnSave.UniqueID; //Set the default button to save.

                    this.DisplayMessageForMissingMasters();

                    this.txtFirstName.Focus();
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
        public ContactPresenter Presenter
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

        #region IContactView Members

        //public List<SalesOffice> SalesOfficeList
        //{
        //    set
        //    {
        //        this.ddlSalesOfficeLocation.DataSource = value;
        //        this.ddlSalesOfficeLocation.DataTextField = "LocationName";
        //        this.ddlSalesOfficeLocation.DataValueField = "LocationId";
        //        this.ddlSalesOfficeLocation.DataBind();

        //        this.ddlSalesOfficeLocation.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect")));
        //    }
        //}

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

        //public int? LocationId
        //{
        //    get
        //    {
        //        if (this.ddlSalesOfficeLocation.SelectedIndex == 0)
        //            return null;
        //        else
        //            return Convert.ToInt32(this.ddlSalesOfficeLocation.SelectedValue);
        //    }
        //    set
        //    {
        //        if (value == 0 || value == null)
        //            this.ddlSalesOfficeLocation.SelectedIndex = 0;
        //        else
        //            this.ddlSalesOfficeLocation.SelectedValue = value.ToString();
        //    }
        //}

        public string SelectedLocationIds
        {
            get
            {
                string locationIds = string.Empty;
                foreach (GridViewRow row in gdvLocationContact.Rows)
                {
                    CheckBox chk = row.FindControl("chkSelect") as CheckBox;
                    if (chk != null && chk.Checked)
                    {
                        HiddenField hdn = row.FindControl("hdnLocationId") as HiddenField;
                        if (hdn != null)
                            locationIds += (locationIds == string.Empty ? string.Empty : ",") + hdn.Value;
                    }
                }
                return locationIds;
            }
        }

        public int SelectedContactId
        {
            get
            {
                if (this.lstExistingContacts.SelectedIndex >= 0)
                    return Convert.ToInt32(this.lstExistingContacts.SelectedValue);
                else
                    return 0;
            }
        }

        public List<VCTWeb.Core.Domain.LocationContact> LocationContactList
        {
            set
            {
                this.gdvLocationContact.DataSource = value;
                this.gdvLocationContact.DataBind();
            }
        }

        public List<VCTWeb.Core.Domain.Contact> ContactList
        {
            set
            {
                this.lstExistingContacts.DataSource = value;
                this.lstExistingContacts.DataTextField = "FullName";
                this.lstExistingContacts.DataValueField = "ContactId";
                this.lstExistingContacts.DataBind();
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

        public string Email
        {
            get
            {
                return this.txtEmailId.Text.Trim();
            }
            set
            {
                hdnEmailId.Value = value;
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

        #endregion

        #region Event Handlers

        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                this.lblError.Text = string.Empty;
                presenter.OnViewInitialized();
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.lblError.Text = string.Empty;

                if (Page.IsValid && ValidatedPageInput())
                {
                    Constants.ResultStatus resultStatus = presenter.Save(hdnEmailId.Value);
                        if (resultStatus == Constants.ResultStatus.Created)
                        {
                            presenter.OnViewInitialized();
                            this.txtFirstName.Focus();

                            this.lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgCreated"), this.lblHeader.Text) + "</font>";
                        }
                        else if (resultStatus == Constants.ResultStatus.Updated)
                        {
                            presenter.OnViewInitialized();
                            this.txtFirstName.Focus();

                            this.lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgUpdated"), this.lblHeader.Text) + "</font>";
                        }
                        else if (resultStatus == Constants.ResultStatus.DuplicateContact)
                        {
                            presenter.OnViewInitialized();
                            this.txtFirstName.Focus();

                            this.lblError.Text = "<font color='red'>" + "Contact with the email already exists" + "</font>";
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

        protected void lstExistingContacts_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //this.SelectedContactId = Convert.ToInt32(lstExistingContacts.SelectedValue);
                this.lblError.Text = string.Empty;
                presenter.OnViewLoaded();
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
                this.lblError.Text = "<font color='red'>" + "Please enter valid Cell number in digits " + "</font>";
                return false;
            }
            if (txtFax.Text.Any(char.IsLetter))
            {
                this.lblError.Text = "<font color='red'>" + "Please enter valid Fax number in digits" + "</font>";
                return false;
            }


            return true;
        }

        #endregion
    }
}

