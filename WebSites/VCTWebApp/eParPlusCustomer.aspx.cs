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
    public partial class Customer : Microsoft.Practices.CompositeWeb.Web.UI.Page, ICustomerView
    {
        #region Instance Variables
        private CustomerPresenter presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private Helper helper = new Helper();
        private Security security = null;
        #endregion Instance Variables

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
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Events




        protected void lnkFilterCustomerListData_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtCustomerName.Enabled = true;
                this.txtAccountNumber.Enabled = true;
                this.presenter.FilterCustomer();

                if (lstExistingCustomers != null && lstExistingCustomers.Items.Count == 1)
                {
                    lstExistingCustomers.SelectedIndex = 0;
                    FillSelectedCustomerDetails();
                }


                txtCustomerName.Focus();
            }
            catch { }
        }

        protected void lnkAddOwnershipStructure_Click(object sender, EventArgs e)
        {
            try
            {
                EnableDisableLinkButton("OWNERSHIPSTRUCTURE", (imgAddOwnershipStructure.ImageUrl.Trim().ToUpper().Contains("ADD.GIF")));
            }
            catch
            {

            }
        }

        protected void lnkAddManagementStructure_Click(object sender, EventArgs e)
        {
            try
            {
                EnableDisableLinkButton("MANAGEMENTSTRUCTURE", (imgAddManagementStructure.ImageUrl.Trim().ToUpper().Contains("ADD.GIF")));
            }
            catch
            {

            }
        }

        protected void lnkAddSpineOnlyMultiSpecialty_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    EnableDisableLinkButton("SPINEONLYMULTISPECIALTY", (imgAddSpineOnlyMultiSpecialty.ImageUrl.Trim().ToUpper().Contains("ADD.GIF")));
                }
                catch { }
            }
            catch
            {

            }
        }

        protected void lnkAddBranchAgency_Click(object sender, EventArgs e)
        {
            try
            {
                EnableDisableLinkButton("BRANCHAGENCY", (imgAddBranchAgency.ImageUrl.Trim().ToUpper().Contains("ADD.GIF")));
            }
            catch { }
        }

        protected void lnkAddManager_Click(object sender, EventArgs e)
        {
            try
            {
                //EnableDisableLinkButton("MANAGER", (imgAddManager.ImageUrl.Trim().ToUpper().Contains("ADD.GIF")));
            }
            catch
            {

            }
        }

        protected void lstExistingCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FillSelectedCustomerDetails();
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

        private void FillSelectedCustomerDetails()
        {
            this.lblError.Text = string.Empty;
            EnableDisableLinkButton("OWNERSHIPSTRUCTURE", false);
            EnableDisableLinkButton("MANAGEMENTSTRUCTURE", false);
            EnableDisableLinkButton("SPINEONLYMULTISPECIALTY", false);
            EnableDisableLinkButton("BRANCHAGENCY", false);
            EnableDisableLinkButton("MANAGER", false);
            ddlSalesRepresentative.SelectedIndex = 0;
            ddlSpecialistRep.SelectedIndex = 0;
            presenter.OnViewLoaded();
            this.txtCustomerName.Enabled = false;
            this.txtAccountNumber.Enabled = false;
            this.txtStreetAddress.Focus();
        }

        protected void lnkResetFilterCriteria_Click(object sender, EventArgs e)
        {
            try
            {
                txtCustomerNameFilter.Text = string.Empty;
                txtAccountNumberFilter.Text = string.Empty;
                ddlBranchAgencyFilter.SelectedIndex = 0;
                ddlManagementStructureFilter.SelectedIndex = 0;
                ddlManagerFilter.SelectedIndex = 0;
                ddlOwnershipStructureFilter.SelectedIndex = 0;
                ddlSalesRepresentativeFilter.SelectedIndex = 0;
                ddlSpecialistRep.SelectedIndex = 0;
                ddlSpineonlyMultiSpecialtyFilter.SelectedIndex = 0;
                chkActiveInactiveFilter.Checked = true;
            }
            catch { }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                this.lblError.Text = string.Empty;
                this.presenter.OnViewInitialized();
                ResetControlAfterSaveUpdate();
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

                if (ValidateBeforeSave())
                {
                    Constants.ResultStatus resultStatus = presenter.Save();
                    if (resultStatus == Constants.ResultStatus.Created)
                    {
                        presenter.OnViewInitialized();
                        ResetControlAfterSaveUpdate();
                        this.txtCustomerName.Focus();
                        this.lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgCreated"), this.lblHeader.Text) + "</font>";
                    }
                    else if (resultStatus == Constants.ResultStatus.Updated)
                    {
                        presenter.OnViewInitialized();
                        ResetControlAfterSaveUpdate();
                        this.txtCustomerName.Focus();
                        this.lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgUpdated"), this.lblHeader.Text) + "</font>";
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

        #endregion Events

        #region Grid View Events

        protected void gdvCustomerProductLine_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    VCTWeb.Core.Domain.CustomerProductLine objCustomerProductLine = (VCTWeb.Core.Domain.CustomerProductLine)e.Row.DataItem;
                    DropDownList ddlExistingCustomer = e.Row.FindControl("ddlExistingCustomer") as DropDownList;
                    ddlExistingCustomer.DataSource = presenter.FilterCustomerForProductLine(objCustomerProductLine.ProductLineName);
                    ddlExistingCustomer.DataTextField = "NameAccount";
                    ddlExistingCustomer.DataValueField = "AccountNumber";
                    ddlExistingCustomer.DataBind();
                    ddlExistingCustomer.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect"), "0"));
                }
            }
            catch
            {

            }
        }

        #endregion Grid View Events

        #region Member Methods

        private bool ValidateBeforeSave()
        {
            if (string.IsNullOrEmpty(this.CustomerName))
            {
                lblError.Text = "Please enter Customer Name.";
                return false;
            }
            else
            {
                if (txtCustomerName.Enabled)
                {
                    List<VCTWeb.Core.Domain.Customer> CustomerList = Session["CustomerList"] as List<VCTWeb.Core.Domain.Customer>;
                    if (CustomerList.FindIndex(i => i.CustomerName.Trim().ToUpper() == this.CustomerName.Trim().ToUpper()) >= 0)
                    {
                        lblError.Text = "Customer Name already exists.";
                        return false;
                    }
                }
            }

            if (string.IsNullOrEmpty(this.AccountNumber.Trim()))
            {
                lblError.Text = "Please enter Account Number.";
                return false;
            }
            else
            {
                if (txtAccountNumber.Enabled)
                {
                    List<VCTWeb.Core.Domain.Customer> CustomerList = Session["CustomerList"] as List<VCTWeb.Core.Domain.Customer>;
                    if (CustomerList.FindIndex(i => i.AccountNumber.Trim().ToUpper() == this.AccountNumber.Trim().ToUpper()) >= 0)
                    {
                        lblError.Text = "Account Number already exists.";
                        return false;
                    }
                }
            }


            if (string.IsNullOrEmpty(this.StreetAddress.Trim()))
            {
                lblError.Text = "Please enter Street Name.";
                return false;
            }
            if (string.IsNullOrEmpty(this.City.Trim()))
            {
                lblError.Text = "Please enter City.";
                return false;
            }
            if (string.IsNullOrEmpty(this.State.Trim()))
            {
                lblError.Text = "Please select State.";
                return false;
            }
            if (string.IsNullOrEmpty(this.Zip.Trim()))
            {
                lblError.Text = "Please enter Zip code.";
                return false;
            }
            if (string.IsNullOrEmpty(this.OwnershipStructure))
            {
                lblError.Text = "Please select/enter Ownership Structure.";
                return false;
            }
            if (string.IsNullOrEmpty(this.ManagementStructure))
            {
                lblError.Text = "Please select/enter Management Structure.";
                return false;
            }
            if (string.IsNullOrEmpty(this.SpineOnlyMultiSpecialty))
            {
                lblError.Text = "Please select SpineOnly/MultiSpecialty.";
                return false;
            }
            if (string.IsNullOrEmpty(this.BranchAgency))
            {
                lblError.Text = "Please select/enter Branch/Agency.";
                return false;
            }
            if (string.IsNullOrEmpty(this.Manager))
            {
                lblError.Text = "Please select Regional Rep.";
                return false;
            }
            if (string.IsNullOrEmpty(this.SalesRepresentative))
            {
                lblError.Text = "Please select Local Rep.";
                return false;
            }
            if (string.IsNullOrEmpty(this.SpecialistRep))
            {
                lblError.Text = "Please select Specialist Rep.";
                return false;
            }
            if (string.IsNullOrEmpty(this.txtConsumptionInterval.Text.Trim()))
            {
                lblError.Text = "Please enter Consumption Interval.";
                return false;
            }
            else if (this.ConsumptionInterval <= 0)
            {
                lblError.Text = "Consumption Interval should be greater than Zero.";
                return false;
            }

            if (string.IsNullOrEmpty(this.txtAssetNearExpiryDays.Text.Trim()))
            {
                lblError.Text = "Please enter Asset Near Expiry Days.";
                return false;
            }
            else if (this.AssetNearExpiryDays <= 0)
            {
                lblError.Text = "Asset Near Expiry Days should be greater than Zero.";
                return false;
            }


            //Check Duplicate Ownership Structure
            if (txtOwnershipStructure.Visible)
            {
                foreach (ListItem item in ddlOwnershipStructure.Items)
                {
                    if (item.Text.Trim().ToUpper() == OwnershipStructure.ToUpper().Trim())
                    {
                        lblError.Text = "Entered Ownership Structure already exists. Please click on undo and select from dropdown.";
                        return false;
                    }
                }
            }


            //Check Duplicate Management Structure
            if (txtManagementStructure.Visible)
            {
                foreach (ListItem item in ddlManagementStructure.Items)
                {
                    if (item.Text.Trim().ToUpper() == ManagementStructure.ToUpper().Trim())
                    {
                        lblError.Text = "Entered Management Structure already exists. Please click on undo and select from dropdown.";
                        return false;
                    }
                }
            }

            //Check Duplicate Branch Agency
            if (txtBranchAgency.Visible)
            {
                foreach (ListItem item in ddlBranchAgency.Items)
                {
                    if (item.Text.Trim().ToUpper() == BranchAgency.ToUpper().Trim())
                    {
                        lblError.Text = "Entered Branch/Agency already exists. Please click on undo and select from dropdown.";
                        return false;
                    }
                }
            }

            return true;
        }

        private void AuthorizedPage()
        {
            security = new Security();
            if (Session["LoggedInUser"] == null)
            {
                Response.Redirect("~/ErrorPage.aspx?ErrorKey=Common_msgSessionExpired");
            }
            else if (security.HasAccess("Customer"))
            {
                //CanCancel = security.HasPermission("PARLevel");
            }
            else
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }

        private void LocalizePage()
        {
            try
            {
                string heading = string.Empty;
                heading = vctResource.GetString("mnuCustomer");
                lblHeader.Text = heading;
                Page.Title = heading;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void EnableDisableLinkButton(string Control, bool IsAdd)
        {
            switch (Control.ToUpper().Trim())
            {
                case "OWNERSHIPSTRUCTURE":
                    if (IsAdd)
                    {
                        txtOwnershipStructure.Visible = true;
                        ddlOwnershipStructure.Visible = false;
                        imgAddOwnershipStructure.ImageUrl = "~/Images/icon_undo.png";
                        imgAddOwnershipStructure.ToolTip = "Undo";
                        txtOwnershipStructure.Focus();
                        txtOwnershipStructure.Text = string.Empty;
                    }
                    else
                    {
                        txtOwnershipStructure.Visible = false;
                        ddlOwnershipStructure.Visible = true;
                        imgAddOwnershipStructure.ImageUrl = "~/Images/Add.gif";
                        imgAddOwnershipStructure.ToolTip = "Add";
                    }
                    break;

                case "MANAGEMENTSTRUCTURE":
                    if (IsAdd)
                    {
                        txtManagementStructure.Visible = true;
                        ddlManagementStructure.Visible = false;
                        imgAddManagementStructure.ImageUrl = "~/Images/icon_undo.png";
                        imgAddManagementStructure.ToolTip = "Undo";
                        txtManagementStructure.Text = string.Empty;
                        txtManagementStructure.Focus();
                    }
                    else
                    {
                        txtManagementStructure.Visible = false;
                        ddlManagementStructure.Visible = true;
                        imgAddManagementStructure.ImageUrl = "~/Images/Add.gif";
                        imgAddManagementStructure.ToolTip = "Add";
                    }
                    break;

                case "SPINEONLYMULTISPECIALTY":
                    if (IsAdd)
                    {
                        txtSpineOnlyMultiSpecialty.Visible = true;
                        ddlSpineOnlyMultiSpecialty.Visible = false;
                        imgAddSpineOnlyMultiSpecialty.ImageUrl = "~/Images/icon_undo.png";
                        imgAddSpineOnlyMultiSpecialty.ToolTip = "Undo";
                        txtSpineOnlyMultiSpecialty.Text = string.Empty;
                        txtSpineOnlyMultiSpecialty.Focus();
                    }
                    else
                    {
                        txtSpineOnlyMultiSpecialty.Visible = false;
                        ddlSpineOnlyMultiSpecialty.Visible = true;
                        imgAddSpineOnlyMultiSpecialty.ImageUrl = "~/Images/Add.gif";
                        imgAddSpineOnlyMultiSpecialty.ToolTip = "Add";
                    }
                    break;

                case "BRANCHAGENCY":
                    if (IsAdd)
                    {
                        txtBranchAgency.Visible = true;
                        ddlBranchAgency.Visible = false;
                        imgAddBranchAgency.ImageUrl = "~/Images/icon_undo.png";
                        imgAddBranchAgency.ToolTip = "Undo";
                        txtBranchAgency.Text = string.Empty;
                        txtBranchAgency.Focus();
                    }
                    else
                    {
                        txtBranchAgency.Visible = false;
                        ddlBranchAgency.Visible = true;
                        imgAddBranchAgency.ImageUrl = "~/Images/Add.gif";
                        imgAddBranchAgency.ToolTip = "Add";
                    }
                    break;


                case "MANAGER":
                    if (IsAdd)
                    {
                        txtManager.Visible = true;
                        ddlManager.Visible = false;
                        //imgAddManager.ImageUrl = "~/Images/icon_undo.png";
                        //imgAddManager.ToolTip = "Undo";
                        txtManager.Text = string.Empty;
                        txtManager.Focus();
                    }
                    else
                    {
                        txtManager.Visible = false;
                        ddlManager.Visible = true;
                        //imgAddManager.ImageUrl = "~/Images/Add.gif";
                        //imgAddManager.ToolTip = "Add";
                    }
                    break;
            }
        }

        private void ResetControlAfterSaveUpdate()
        {
            try
            {
                this.txtCustomerName.Enabled = true;
                this.txtAccountNumber.Enabled = true;
                this.txtOwnershipStructure.Visible = false;
                this.txtManagementStructure.Visible = false;
                this.txtSpineOnlyMultiSpecialty.Visible = false;
                this.txtBranchAgency.Visible = false;
                this.txtManager.Visible = false;

                this.ddlOwnershipStructure.Visible = true;
                this.ddlManagementStructure.Visible = true;
                this.ddlSpineOnlyMultiSpecialty.Visible = true;
                this.ddlBranchAgency.Visible = true;
                this.ddlManager.Visible = true;

                this.txtCustomerNameFilter.Text = string.Empty;
                this.txtAccountNumberFilter.Text = string.Empty;
                this.txtCustomerName.Text = string.Empty;
                this.txtAccountNumber.Text = string.Empty;
                this.txtOwnershipStructure.Text = string.Empty;
                this.txtManagementStructure.Text = string.Empty;
                this.txtSpineOnlyMultiSpecialty.Text = string.Empty;
                this.txtBranchAgency.Text = string.Empty;
                this.txtManager.Text = string.Empty;

                this.ddlOwnershipStructureFilter.SelectedIndex = 0;
                this.ddlManagementStructureFilter.SelectedIndex = 0;
                this.ddlSpineonlyMultiSpecialtyFilter.SelectedIndex = 0;
                this.ddlBranchAgencyFilter.SelectedIndex = 0;
                this.ddlManagerFilter.SelectedIndex = 0;
                this.ddlSalesRepresentativeFilter.SelectedIndex = 0;
                this.ddlSpecialistRep.SelectedIndex = 0;
                this.chkActiveInactiveFilter.Checked = true;
                this.txtCustomerName.Focus();

                EnableDisableLinkButton("OWNERSHIPSTRUCTURE", false);
                EnableDisableLinkButton("MANAGEMENTSTRUCTURE", false);
                EnableDisableLinkButton("SPINEONLYMULTISPECIALTY", false);
                EnableDisableLinkButton("BRANCHAGENCY", false);
                EnableDisableLinkButton("MANAGER", false);
            }
            catch { }
        }

        #endregion Member Methods

        #region Create New Presenter

        [CreateNew]
        public CustomerPresenter Presenter
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

        #region ICustomerView Members

        public string CustomerName
        {
            get
            {
                return txtCustomerName.Text.Trim();
            }
            set
            {
                txtCustomerName.Text = value.Trim();
            }
        }

        public string AccountNumber
        {
            get
            {
                return txtAccountNumber.Text.Trim();
            }
            set
            {
                txtAccountNumber.Text = value.Trim();
            }
        }

        public string StreetAddress
        {
            get
            {
                return txtStreetAddress.Text.Trim();
            }
            set
            {
                txtStreetAddress.Text = value.Trim();
            }
        }

        public string City
        {
            get
            {
                return txtCity.Text.Trim();
            }
            set
            {
                txtCity.Text = value.Trim();
            }
        }

        public string State
        {
            get
            {
                if (this.ddlState.SelectedIndex > 0)
                    return Convert.ToString(this.ddlState.SelectedValue);
                else
                    return "";
            }
            set
            {
                if (value == null || value == "")
                    this.ddlState.SelectedIndex = 0;
                else
                    this.ddlState.SelectedValue = value.ToString();
            }

        }

        public string Zip
        {
            get
            {
                return txtZipCode.Text.Trim();
            }
            set
            {
                txtZipCode.Text = value.Trim();
            }
        }

        public string OwnershipStructure
        {
            get
            {
                if (txtOwnershipStructure.Visible)
                {
                    if (!string.IsNullOrEmpty(txtOwnershipStructure.Text.Trim()))
                        return Convert.ToString(this.txtOwnershipStructure.Text.Trim());
                    else
                        return "";
                }
                else
                {
                    if (this.ddlOwnershipStructure.SelectedIndex > 0)
                        return Convert.ToString(this.ddlOwnershipStructure.SelectedValue);
                    else
                        return "";
                }
            }
            set
            {
                if (value == null || value == "")
                    this.ddlOwnershipStructure.SelectedIndex = 0;
                else
                    this.ddlOwnershipStructure.SelectedValue = value.ToString();
            }
        }

        public string ManagementStructure
        {
            get
            {
                if (txtManagementStructure.Visible)
                {
                    if (!string.IsNullOrEmpty(txtManagementStructure.Text.Trim()))
                        return Convert.ToString(this.txtManagementStructure.Text.Trim());
                    else
                        return "";
                }
                else
                {
                    if (this.ddlManagementStructure.SelectedIndex > 0)
                        return Convert.ToString(this.ddlManagementStructure.SelectedValue);
                    else
                        return "";
                }
            }
            set
            {
                if (value == null || value == "")
                    this.ddlManagementStructure.SelectedIndex = 0;
                else
                    this.ddlManagementStructure.SelectedValue = value.ToString();
            }
        }

        public string SpineOnlyMultiSpecialty
        {
            get
            {
                if (txtSpineOnlyMultiSpecialty.Visible)
                {
                    if (!string.IsNullOrEmpty(txtSpineOnlyMultiSpecialty.Text.Trim()))
                        return Convert.ToString(this.txtSpineOnlyMultiSpecialty.Text.Trim());
                    else
                        return "";
                }
                else
                {
                    if (this.ddlSpineOnlyMultiSpecialty.SelectedIndex > 0)
                        return Convert.ToString(this.ddlSpineOnlyMultiSpecialty.SelectedValue);
                    else
                        return "";
                }
            }
            set
            {
                if (value == null || value == "")
                    this.ddlSpineOnlyMultiSpecialty.SelectedIndex = 0;
                else
                    this.ddlSpineOnlyMultiSpecialty.SelectedValue = value.ToString();
            }
        }

        public string BranchAgency
        {
            get
            {
                if (txtBranchAgency.Visible)
                {
                    if (!string.IsNullOrEmpty(txtBranchAgency.Text.Trim()))
                        return Convert.ToString(this.txtBranchAgency.Text.Trim());
                    else
                        return "";
                }
                else
                {
                    if (this.ddlBranchAgency.SelectedIndex > 0)
                        return Convert.ToString(this.ddlBranchAgency.SelectedValue);
                    else
                        return "";
                }
            }
            set
            {
                if (value == null || value == "")
                    this.ddlBranchAgency.SelectedIndex = 0;
                else
                    this.ddlBranchAgency.SelectedValue = value.ToString();
            }
        }

        public string Manager
        {
            get
            {
                if (txtManager.Visible)
                {
                    if (!string.IsNullOrEmpty(txtManager.Text.Trim()))
                        return Convert.ToString(this.txtManager.Text.Trim());
                    else
                        return "";
                }
                else
                {
                    if (this.ddlManager.SelectedIndex > 0)
                        return Convert.ToString(this.ddlManager.SelectedValue);
                    else
                        return "";
                }
            }
            set
            {
                if (value == null || value == "")
                    this.ddlManager.SelectedIndex = 0;
                else
                    this.ddlManager.SelectedValue = value.ToString();
            }
        }

        int? ICustomerView.QtyOfORs
        {
            get
            {
                int ORQty = 0;
                try
                {
                    ORQty = Convert.ToInt16(txtQtyofOR.Text.Trim());
                }
                catch { }
                return ORQty;
            }
            set
            {
                int ORQty = 0;
                try
                {
                    ORQty = Convert.ToInt16(value);
                }
                catch { }
                if (ORQty > 0)
                    txtQtyofOR.Text = Convert.ToString(value);
                else
                    txtQtyofOR.Text = string.Empty;
            }
        }

        public string SalesRepresentative
        {
            get
            {
                if (this.ddlSalesRepresentative.SelectedIndex > 0)
                    return Convert.ToString(this.ddlSalesRepresentative.SelectedValue);
                else
                    return string.Empty;
            }
            set
            {
                if (value == null || value == "")
                    this.ddlSalesRepresentative.SelectedIndex = 0;
                else
                    this.ddlSalesRepresentative.SelectedValue = value.ToString();
            }
        }

        public string SpecialistRep
        {
            get
            {
                if (this.ddlSpecialistRep.SelectedIndex > 0)
                    return Convert.ToString(this.ddlSpecialistRep.SelectedValue);
                else
                    return string.Empty;
            }
            set
            {
                if (value == null || value == "")
                    this.ddlSpecialistRep.SelectedIndex = 0;
                else
                    this.ddlSpecialistRep.SelectedValue = value.ToString();
            }
        }

        public bool Active
        {
            get
            {
                return this.chkIsActive.Checked;
            }
            set
            {
                this.chkIsActive.Checked = value;
            }
        }

        public int ConsumptionInterval
        {
            get
            {
                int val = 0;
                if (!string.IsNullOrEmpty(txtConsumptionInterval.Text.Trim()))
                    val = Convert.ToInt16(txtConsumptionInterval.Text.Trim());
                return val;
            }
            set
            {
                txtConsumptionInterval.Text = Convert.ToString(value);
            }
        }

        public int AssetNearExpiryDays
        {
            get
            {
                int val = 0;
                if (!string.IsNullOrEmpty(txtAssetNearExpiryDays.Text.Trim()))
                    val = Convert.ToInt16(txtAssetNearExpiryDays.Text.Trim());
                return val;
            }
            set
            {
                txtAssetNearExpiryDays.Text = Convert.ToString(value);
            }
        }

        public List<VCTWeb.Core.Domain.Customer> CustomerList
        {
            set
            {
                Session["CustomerList"] = value;
                this.lstExistingCustomers.DataSource = value;
                this.lstExistingCustomers.DataTextField = "NameAccount";
                this.lstExistingCustomers.DataValueField = "AccountNumber";
                this.lstExistingCustomers.DataBind();
            }
        }


        public List<State> StateList
        {
            set
            {
                this.ddlState.DataSource = null;
                this.ddlState.DataSource = value;
                this.ddlState.DataTextField = "StateName";
                this.ddlState.DataValueField = "StateName";
                this.ddlState.DataBind();
                this.ddlState.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect"), "0"));
                ddlState.SelectedIndex = 0;
            }
        }


        public List<string> OwnershipStructureList
        {
            set
            {
                this.ddlOwnershipStructure.DataSource = null;
                this.ddlOwnershipStructure.DataSource = value;
                this.ddlOwnershipStructure.DataBind();
                this.ddlOwnershipStructure.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect"), "0"));
                ddlOwnershipStructure.SelectedIndex = 0;


                this.ddlOwnershipStructureFilter.DataSource = null;
                this.ddlOwnershipStructureFilter.DataSource = value;
                this.ddlOwnershipStructureFilter.DataBind();
                this.ddlOwnershipStructureFilter.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect"), "0"));
                ddlOwnershipStructureFilter.SelectedIndex = 0;
            }

        }

        public List<string> ManagementStructureList
        {
            set
            {
                this.ddlManagementStructure.DataSource = null;
                this.ddlManagementStructure.DataSource = value;
                this.ddlManagementStructure.DataBind();
                this.ddlManagementStructure.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect"), "0"));
                ddlManagementStructure.SelectedIndex = 0;

                this.ddlManagementStructureFilter.DataSource = null;
                this.ddlManagementStructureFilter.DataSource = value;
                this.ddlManagementStructureFilter.DataBind();
                this.ddlManagementStructureFilter.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect"), "0"));
                ddlManagementStructureFilter.SelectedIndex = 0;
            }
        }

        public List<string> SpineOnlyMultiSpecialtyList
        {
            set
            {
                this.ddlSpineOnlyMultiSpecialty.DataSource = null;
                this.ddlSpineOnlyMultiSpecialty.DataSource = value;
                this.ddlSpineOnlyMultiSpecialty.DataBind();
                this.ddlSpineOnlyMultiSpecialty.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect"), "0"));
                ddlSpineOnlyMultiSpecialty.SelectedIndex = 0;

                this.ddlSpineonlyMultiSpecialtyFilter.DataSource = null;
                this.ddlSpineonlyMultiSpecialtyFilter.DataSource = value;
                this.ddlSpineonlyMultiSpecialtyFilter.DataBind();
                this.ddlSpineonlyMultiSpecialtyFilter.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect"), "0"));
                ddlSpineonlyMultiSpecialtyFilter.SelectedIndex = 0;
            }
        }

        public List<string> BranchAgencyList
        {
            set
            {
                this.ddlBranchAgency.DataSource = null;
                this.ddlBranchAgency.DataSource = value;
                this.ddlBranchAgency.DataBind();
                this.ddlBranchAgency.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect"), "0"));
                ddlBranchAgency.SelectedIndex = 0;

                this.ddlBranchAgencyFilter.DataSource = null;
                this.ddlBranchAgencyFilter.DataSource = value;
                this.ddlBranchAgencyFilter.DataBind();
                this.ddlBranchAgencyFilter.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect"), "0"));
                ddlBranchAgencyFilter.SelectedIndex = 0;
            }
        }

        List<Users> ICustomerView.ManagerList
        {
            set
            {
                this.ddlManager.DataSource = null;
                this.ddlManager.DataSource = value;
                this.ddlManager.DataTextField = "FullName";
                this.ddlManager.DataValueField = "UserName";
                this.ddlManager.DataBind();
                this.ddlManager.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect"), "0"));
                ddlManager.SelectedIndex = 0;


                this.ddlManagerFilter.DataSource = null;
                this.ddlManagerFilter.DataSource = value;
                this.ddlManagerFilter.DataTextField = "FullName";
                this.ddlManagerFilter.DataValueField = "UserName";
                this.ddlManagerFilter.DataBind();
                this.ddlManagerFilter.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect"), "0"));
                ddlManagerFilter.SelectedIndex = 0;
            }
        }

        List<Users> ICustomerView.SalesRepresentativeList
        {
            set
            {
                ddlSalesRepresentative.SelectedValue = null;
                this.ddlSalesRepresentative.DataSource = null;
                this.ddlSalesRepresentative.DataSource = value;
                this.ddlSalesRepresentative.DataTextField = "FullName";
                this.ddlSalesRepresentative.DataValueField = "UserName";
                this.ddlSalesRepresentative.DataBind();
                this.ddlSalesRepresentative.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect"), "0"));
                ddlSalesRepresentative.SelectedIndex = 0;


                ddlSalesRepresentativeFilter.SelectedValue = null;
                this.ddlSalesRepresentativeFilter.DataSource = null;
                this.ddlSalesRepresentativeFilter.DataSource = value;
                this.ddlSalesRepresentativeFilter.DataTextField = "FullName";
                this.ddlSalesRepresentativeFilter.DataValueField = "UserName";
                this.ddlSalesRepresentativeFilter.DataBind();
                this.ddlSalesRepresentativeFilter.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect"), "0"));
                ddlSalesRepresentativeFilter.SelectedIndex = 0;
            }
        }

        List<Users> ICustomerView.SpecialistRepList
        {
            set
            {
                ddlSpecialistRep.SelectedValue = null;
                this.ddlSpecialistRep.DataSource = null;
                this.ddlSpecialistRep.DataSource = value;
                this.ddlSpecialistRep.DataTextField = "FullName";
                this.ddlSpecialistRep.DataValueField = "UserName";
                this.ddlSpecialistRep.DataBind();
                this.ddlSpecialistRep.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect"), "0"));
                ddlSpecialistRep.SelectedIndex = 0;
            }
        }

        public string SelectedCustomerAccountNumber
        {
            get
            {
                if (this.lstExistingCustomers.SelectedIndex >= 0)
                    return Convert.ToString(this.lstExistingCustomers.SelectedValue);
                else
                    return String.Empty;
            }
        }

        public string SelectedCustomerNameFilter
        {
            get
            {
                return txtCustomerNameFilter.Text.Trim();
            }
        }

        public string SelectedAccountNumberFilter
        {
            get
            {
                return txtAccountNumberFilter.Text.Trim();
            }
        }

        public string SelectedOwnershipStructureFilter
        {
            get
            {
                if (this.ddlOwnershipStructureFilter.SelectedIndex > 0)
                    return Convert.ToString(this.ddlOwnershipStructureFilter.SelectedValue);
                else
                    return string.Empty;
            }
        }

        public string SelectedManagementStructureFilter
        {
            get
            {
                if (this.ddlManagementStructureFilter.SelectedIndex > 0)
                    return Convert.ToString(this.ddlManagementStructureFilter.SelectedValue);
                else
                    return string.Empty;
            }
        }

        public string SelectedSpineonlyMultiSpecialtyFilter
        {
            get
            {
                if (this.ddlSpineonlyMultiSpecialtyFilter.SelectedIndex > 0)
                    return Convert.ToString(this.ddlSpineonlyMultiSpecialtyFilter.SelectedValue);
                else
                    return string.Empty;
            }
        }

        public string SelectedBranchAgencyFilter
        {
            get
            {
                if (this.ddlBranchAgencyFilter.SelectedIndex > 0)
                    return Convert.ToString(this.ddlBranchAgencyFilter.SelectedValue);
                else
                    return string.Empty;
            }
        }

        public string SelectedManagerFilter
        {
            get
            {
                if (this.ddlManagerFilter.SelectedIndex > 0)
                    return Convert.ToString(this.ddlManagerFilter.SelectedValue);
                else
                    return string.Empty;
            }
        }

        public string SelectedSalesRepresentativeFilter
        {
            get
            {
                if (this.ddlSalesRepresentativeFilter.SelectedIndex > 0)
                    return Convert.ToString(this.ddlSalesRepresentativeFilter.SelectedValue);
                else
                    return string.Empty;
            }
        }

        public bool SelectedActiveInactiveFilter
        {
            get
            {
                return chkActiveInactiveFilter.Checked;
            }
        }

        public List<CustomerProductLine> CustomerProductLineList
        {
            set
            {
                this.gdvCustomerProductLine.DataSource = value;
                this.gdvCustomerProductLine.DataBind();
                foreach (GridViewRow row in gdvCustomerProductLine.Rows)
                {
                    CheckBox chk = row.FindControl("chkSelect") as CheckBox;
                    DropDownList ddlExistingCustomer = row.FindControl("ddlExistingCustomer") as DropDownList;
                    if (chk != null && chk.Checked)
                    {
                        chk.Enabled = false;
                        if (ddlExistingCustomer != null) ddlExistingCustomer.Enabled = false;
                    }
                }
            }
            get
            {
                List<CustomerProductLine> list = new List<CustomerProductLine>();
                foreach (GridViewRow row in gdvCustomerProductLine.Rows)
                {
                    Label lblProductLineNameGrid = (Label)row.FindControl("lblProductLineName");
                    CheckBox chkSelectGrid = (CheckBox)row.FindControl("chkSelect");
                    DropDownList ddlExistingCustomerGrid = (DropDownList)row.FindControl("ddlExistingCustomer");

                    if (lblProductLineNameGrid != null && chkSelectGrid != null && ddlExistingCustomerGrid != null)
                    {
                        if (chkSelectGrid.Enabled && chkSelectGrid.Checked && ddlExistingCustomerGrid.SelectedIndex > 0)
                        {
                            CustomerProductLine customerProductLine = new CustomerProductLine();
                            customerProductLine.ProductLineName = lblProductLineNameGrid.Text.Trim();
                            customerProductLine.AccountNumber = Convert.ToString(ddlExistingCustomerGrid.SelectedValue);
                            list.Add(customerProductLine);
                        }
                    }
                }
                return list;
            }
        }

        public string SelectedProductLines
        {
            get
            {
                string productLineNames = string.Empty;
                foreach (GridViewRow row in gdvCustomerProductLine.Rows)
                {
                    CheckBox chk = row.FindControl("chkSelect") as CheckBox;
                    if (chk != null && chk.Checked)
                    {
                        Label lblProductLineName = row.FindControl("lblProductLineName") as Label;
                        if (lblProductLineName != null)
                            productLineNames += (productLineNames == string.Empty ? string.Empty : ",") + lblProductLineName.Text.Trim();
                    }
                }
                return productLineNames;
            }
        }

        #endregion


    }

}

