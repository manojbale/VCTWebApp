using System;
using Microsoft.Practices.ObjectBuilder;
using VCTWebApp.Resources;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Collections.Generic;
using VCTWebApp.Web;

namespace VCTWebApp.Shell.Views
{
    public partial class CustomerPARLevel : Microsoft.Practices.CompositeWeb.Web.UI.Page, ICustomerPARLevel
    {
        #region Instance Variables
        private CustomerPARLevelPresenter _presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private Security security = null;
        #endregion

        #region Init/Page Load

        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Text = string.Empty;
            if (!this.IsPostBack)
            {
                this.AuthorizedPage();
                this._presenter.OnViewInitialized();
            }
            this.LocalizePage();
        }

        #endregion Init/Page Load

        #region Create New Presenter
        [CreateNew]
        public CustomerPARLevelPresenter Presenter
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

        #region Events

        protected void gdvCustomerPartDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gdvCustomerPartDetails.EditIndex = e.NewEditIndex;
                this.CustomerPARLevelList = this.CustomerPARLevelList;


                TextBox txtPARLevelQty = gdvCustomerPartDetails.Rows[e.NewEditIndex].FindControl("txtPARLevelQty") as TextBox;
                if (txtPARLevelQty != null)
                    txtPARLevelQty.Focus();

            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        protected void gdvCustomerPartDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gdvCustomerPartDetails.EditIndex = -1;
                this.CustomerPARLevelList = this.CustomerPARLevelList;
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        protected void gdvCustomerPartDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("UpdateRec"))
                {
                    Label lblPartNum = gdvCustomerPartDetails.Rows[gdvCustomerPartDetails.EditIndex].FindControl("lblPartNum") as Label;
                    TextBox txtPARLevelQty = gdvCustomerPartDetails.Rows[gdvCustomerPartDetails.EditIndex].FindControl("txtPARLevelQty") as TextBox;
                    if (lblPartNum != null && txtPARLevelQty != null)
                    {
                        if (ValidateItemsOnEdit(txtPARLevelQty.Text))
                        {
                            if (Presenter.Save(this.SelectedAccountNumber, lblPartNum.Text.Trim(), Convert.ToInt16(txtPARLevelQty.Text)))
                            {
                                lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgPARLevelUpdate"), this.lblHeader.Text) + "</font>";
                                this.CustomerPARLevelList[gdvCustomerPartDetails.EditIndex].PARLevelQty = Convert.ToInt16(txtPARLevelQty.Text);
                                gdvCustomerPartDetails.EditIndex = -1;
                                Presenter.PopulateCustomerParLevel();                                
                            }
                        }
                    }
                }
                else if (e.CommandName.Equals("AddNewRow"))
                {
                    if (hdnPartNumNew.Value == string.Empty)
                    {
                        lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valRefNumberEmpty"), this.lblHeader.Text);
                    }
                    else
                    {
                        TextBox txtNewPartNum = gdvCustomerPartDetails.HeaderRow.FindControl("txtNewPartNum") as TextBox;
                        TextBox txtNewPARLevelQty = gdvCustomerPartDetails.HeaderRow.FindControl("txtNewPARLevelQty") as TextBox;

                        if (txtNewPartNum != null && txtNewPARLevelQty != null)
                        {
                            if (ValidateItemsOnAdd(hdnPartNumNew.Value, txtNewPARLevelQty.Text))
                            {
                                if (ddlCustomer.SelectedIndex > 0)
                                {
                                    if (Presenter.Save(this.SelectedAccountNumber, txtNewPartNum.Text.Trim(), Convert.ToInt16(txtNewPARLevelQty.Text)))
                                    {
                                        lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgPARLevelAdded"), this.lblHeader.Text) + "</font>";
                                        Presenter.PopulateCustomerParLevel();
                                        hdnPartNumNew.Value = string.Empty;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (e.CommandName.Equals("DeleteRec"))
                {
                    string RefNum = Convert.ToString(e.CommandArgument);
                    if (!string.IsNullOrEmpty(RefNum))
                    {
                        if (Presenter.DeleteCustomerPARLevel(this.SelectedAccountNumber, RefNum.Trim()))
                        {
                            lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgPARLevelDelete"), this.lblHeader.Text) + "</font>";
                            this.CustomerPARLevelList.RemoveAll(i => i.AccountNumber == this.SelectedAccountNumber && i.RefNum == RefNum.Trim());
                            this.CustomerPARLevelList = this.CustomerPARLevelList;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        protected void gdvCustomerPartDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gdvCustomerPartDetails_OnPaging(object sender, GridViewPageEventArgs e)
        {
            gdvCustomerPartDetails.EditIndex = -1;            
            gdvCustomerPartDetails.PageIndex = e.NewPageIndex;
            this.CustomerPARLevelList = this.CustomerPARLevelList;
        }

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gdvCustomerPartDetails.EditIndex = -1;
                this.CustomerPARLevelList = this.CustomerPARLevelList;

                if (ddlCustomer.SelectedIndex > 0)
                {
                    gdvCustomerPartDetails.ShowHeaderWhenEmpty = true;
                    Presenter.PopulateCustomerParLevel();
                }
                else
                {
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        #endregion

        #region Private Methods

        private void ClearFields()
        {
            lblError.Text = string.Empty;
            gdvCustomerPartDetails.ShowHeaderWhenEmpty = false;
            gdvCustomerPartDetails.DataSource = null;
            gdvCustomerPartDetails.DataBind();
        }

        private void AuthorizedPage()
        {
            security = new Security();
            if (Session["LoggedInUser"] == null)
            {
                Response.Redirect("~/ErrorPage.aspx?ErrorKey=Common_msgSessionExpired");
            }
            else if (security.HasAccess("CustomerPARLevel"))
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
                heading = vctResource.GetString("mnuCustomerPARLevel");
                lblHeader.Text = heading;
                Page.Title = heading;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool ValidateItemsOnAdd(string newParNum, string newQuantity)
        {
            if (string.IsNullOrEmpty(newParNum))
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valRefNumberEmpty"), this.lblHeader.Text);
                return false;
            }

            var item = this.CustomerPARLevelList.Find(t => t.RefNum == newParNum);
            if (item != null)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valRefNumberExists"), this.lblHeader.Text);
                return false;
            }

            if (string.IsNullOrEmpty(newQuantity))
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valQuantity"), this.lblHeader.Text);
                return false;
            }
            if (Convert.ToInt32(newQuantity) < 1)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valQuantityValue"), this.lblHeader.Text);
                return false;
            }
            return true;
        }

        private bool ValidateItemsOnEdit(string newQuantity)
        {
            if (string.IsNullOrEmpty(newQuantity))
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valQuantity"), this.lblHeader.Text);
                return false;
            }
            if (Convert.ToInt32(newQuantity) < 1)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valQuantityValue"), this.lblHeader.Text);
                return false;
            }
            return true;
        }

        #endregion

        #region ICustomerPARLevel Members

        public List<VCTWeb.Core.Domain.CustomerPARLevel> CustomerPARLevelList
        {
            get
            {
                return (List<VCTWeb.Core.Domain.CustomerPARLevel>)ViewState["CustomerPARLevelList"];
            }
            set
            {
                gdvCustomerPartDetails.PageSize = this.PageSize;
                ViewState["CustomerPARLevelList"] = value;
                gdvCustomerPartDetails.DataSource = value;
                gdvCustomerPartDetails.DataBind();
            }
        }

        public List<VCTWeb.Core.Domain.Customer> CustomerList
        {
            set
            {
                ddlCustomer.DataSource = null;
                ddlCustomer.DataBind();
                ddlCustomer.DataSource = value;
                ddlCustomer.DataTextField = "NameAccount";
                ddlCustomer.DataValueField = "AccountNumber";
                ddlCustomer.DataBind();
                this.ddlCustomer.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect"), "0"));
                ddlCustomer.SelectedIndex = 0;
                ClearFields();
            }
        }

        public string SelectedAccountNumber
        {
            get
            {
                if (ddlCustomer.SelectedIndex > 0)
                    return Convert.ToString(ddlCustomer.SelectedValue);
                else
                    return string.Empty;
            }
        }

        public int PageSize
        {
            set
            {
                ViewState["PageSize"] = value;
            }
            get
            {
                return (int)ViewState["PageSize"];
            }
        }

        #endregion
    }
}

