using System;
using Microsoft.Practices.ObjectBuilder;
using System.Data;
using VCTWebApp.Resources;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
using VCTWebApp.Web;

namespace VCTWebApp.Shell.Views
{
    public partial class ReplenishmentTransfer : Microsoft.Practices.CompositeWeb.Web.UI.Page, IReplenishmentTransferView
    {
        #region Instance Variables
        private ReplenishmentTransferPresenter _presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private Security security = null;
        #endregion

        #region Protected Methods

        protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ClearFields();
                
                if (ddlLocation.SelectedIndex > 0)
                {
                    if (rblstLocationType.SelectedIndex == rblstLocationType.Items.Count - 1)
                    {
                        Presenter.PopulatePartyReplenishmentTransfer();
                    }
                    else
                    {
                        Presenter.PopulateLocationReplenishmentTransfer();
                    }
                }
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                ClearFields();
                ddlLocation.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string result;
            try
            {
                lblError.Text = string.Empty;
                List<VCTWeb.Core.Domain.ReplenishmentTransfer> lstSelectedReplenishmentTransfer = new List<VCTWeb.Core.Domain.ReplenishmentTransfer>();
                if (gdvPartDetails.Rows.Count == 0)
                {
                    string message = "There is no data to save against " + rblstLocationType.SelectedItem.Text + ".";
                    lblError.Text = "<font color='red'>" + message + "</font>";
                    ddlLocation.SelectedIndex = 0;
                    return;
                }

                if (GetAndValidateGridData(lstSelectedReplenishmentTransfer))
                {
                    if (rblstLocationType.SelectedIndex == rblstLocationType.Items.Count - 1)
                    {
                        if (Presenter.SavePartyReplenishmentTransfer(lstSelectedReplenishmentTransfer, out result))
                        {
                            ClearFields();
                            string message = string.Format(vctResource.GetString("msgReplenishmentTransferSaved"), result);
                            lblError.Text = "<font color='blue'>" + message + "</font>";
                            //lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgReplenishmentTransferSaved"), this.lblHeader.Text) + "</font>";
                        }
                    }
                    else
                    {
                        if (Presenter.SaveLocationReplenishmentTransfer(lstSelectedReplenishmentTransfer, out result))
                        {
                            ClearFields();
                            string message = string.Format(vctResource.GetString("msgReplenishmentTransferSaved"), result);
                            lblError.Text = "<font color='blue'>" + message + "</font>";
                            //lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgReplenishmentTransferSaved"), this.lblHeader.Text) + "</font>";
                        }
                    }
                }
               
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        private bool GetAndValidateGridData(List<VCTWeb.Core.Domain.ReplenishmentTransfer> lstSelectedReplenishmentTransfer)
        {
            try
            {
                DateTime dt = Convert.ToDateTime(txtRequiredOn.Text.Trim());
                if (dt.Date < DateTime.Now.Date)
                {
                    lblError.Text = vctResource.GetString("valRequiredOn");
                    return false;
                }
            }
            catch
            {
                lblError.Text = vctResource.GetString("valRequiredOn");
                return false;
            }
            if (gdvPartDetails.Rows == null && gdvPartDetails.Rows.Count == 0)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valItemSelect"), this.lblHeader.Text);
                return false;
            }
            if (gdvPartDetails.Rows.Count>0)
            {
            foreach (GridViewRow gvr in gdvPartDetails.Rows)
            {
                CheckBox chkSelect = gvr.FindControl("chkSelect") as CheckBox;
                if (chkSelect.Checked)
                {
                    Label lblPartNum = gvr.FindControl("lblPartNum") as Label;
                    TextBox txtReplenishQty = gvr.FindControl("txtReplenishQty") as TextBox;
                    if (string.IsNullOrEmpty(txtReplenishQty.Text))
                    {
                        lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valQuantityForItem") + lblPartNum.Text, this.lblHeader.Text);
                        return false;
                    }
                    if (Convert.ToInt32(txtReplenishQty.Text) < 1)
                    {
                        lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valQuantityValueForItem") + lblPartNum.Text, this.lblHeader.Text);
                        return false;
                    }
                    lstSelectedReplenishmentTransfer.Add(new VCTWeb.Core.Domain.ReplenishmentTransfer
                    {
                        PartNum = lblPartNum.Text,
                        ReplenishQty = Convert.ToInt32(txtReplenishQty.Text)
                    });
                }
            }
            if (lstSelectedReplenishmentTransfer.Count == 0)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valItemSelect"), this.lblHeader.Text);
                return false;
            }
                }
            return true;
        }

        protected void rblstLocationType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {                
                ClearFields();
                
                if (rblstLocationType.SelectedIndex == 0)
                {
                    lblLocation.Text = "Region: ";
                    Presenter.PopulateRegion();
                }
                else if (rblstLocationType.SelectedIndex == 1)
                {
                    lblLocation.Text = "Branch: ";
                    Presenter.PopulateBranch();
                }
                else if (rblstLocationType.SelectedIndex == 2)
                {
                    lblLocation.Text = "Hospital: ";
                    Presenter.PopulateHospital();
                }
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        private void ClearFields()
        {
            txtRequiredOn.Text = string.Empty;
            lblError.Text = string.Empty;
            gdvPartDetails.DataSource = null;
            gdvPartDetails.DataBind();          
            SetRequiredOn();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Text = string.Empty;
            if (!this.IsPostBack)
            {
                this.AuthorizedPage();
                this._presenter.OnViewInitialized();
                this.LocalizePage();
                SetRequiredOn();
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
            else if (security.HasAccess("ReplenishmentTransfer"))
            {
                //CanCancel = security.HasPermission("ReplenishmentTransfer");
            }
            else
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }

        private void LocalizePage()
        {
            try
            {
                string heading = string.Empty;
                heading = vctResource.GetString("mnuReplenishmentTransfer");
                lblHeader.Text = heading;
                Page.Title = heading;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool ValidateInput()
        {
            //if (string.IsNullOrEmpty(newParNum))
            //{
            //    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valPartNumberEmpty"), this.lblHeader.Text);
            //    return false;
            //}
            //if (rblstLocationType.SelectedIndex == rblstLocationType.Items.Count - 1)
            //{
            //    var item = this.PartyPARLevelList.Find(t => t.PartNum == newParNum);
            //    if (item != null)
            //    {
            //        lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valPartNumberExists"), this.lblHeader.Text);
            //        return false;
            //    }
            //}
            //else
            //{
            //    var item = this.LocationPARLevelList.Find(t => t.PartNum == newParNum);
            //    if (item != null)
            //    {
            //        lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valPartNumberExists"), this.lblHeader.Text);
            //        return false;
            //    }
            //}

            //if (string.IsNullOrEmpty(newQuantity))
            //{
            //    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valQuantity"), this.lblHeader.Text);
            //    return false;
            //}
            //if (Convert.ToInt32(newQuantity) < 1)
            //{
            //    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valQuantityValue"), this.lblHeader.Text);
            //    return false;
            //}
            return true;
        }

        private void SetRequiredOn()
        {
            txtRequiredOn.Text = DateTime.Now.AddDays(10).ToShortDateString();
        }
        #endregion

        #region Create New Presenter
        [CreateNew]
        public ReplenishmentTransferPresenter Presenter
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

        #region IPARLevelView Implementations

        public List<VCTWeb.Core.Domain.ReplenishmentTransfer> ReplenishmentTransferList
        {
            get
            {
                return (List<VCTWeb.Core.Domain.ReplenishmentTransfer>)ViewState["ReplenishmentTransferList"];
            }
            set
            {
                ViewState["ReplenishmentTransferList"] = value;
                this.gdvPartDetails.DataSource = value;
                this.gdvPartDetails.DataBind();
            }
        }

        public List<VCTWeb.Core.Domain.Region> RegionList
        {
            set
            {
                ddlLocation.DataSource = value;
                ddlLocation.DataTextField = "RegionName";
                ddlLocation.DataValueField = "RegionId";
                ddlLocation.DataBind();

                ddlLocation.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect")));
            }
        }

        public List<VCTWeb.Core.Domain.SalesOffice> SalesOfficeList
        {
            set
            {
                ddlLocation.DataSource = value;
                ddlLocation.DataTextField = "LocationName";
                ddlLocation.DataValueField = "LocationId";
                ddlLocation.DataBind();

                ddlLocation.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect")));
            }
        }

        public List<VCTWeb.Core.Domain.Party> PartyList
        {
            set
            {
                ddlLocation.DataSource = value;
                ddlLocation.DataTextField = "Name";
                ddlLocation.DataValueField = "PartyId";
                ddlLocation.DataBind();

                ddlLocation.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect")));
            }
        }

        public int SelectedLocationId
        {
            get
            {
                if (ddlLocation.SelectedIndex > 0)
                    return Convert.ToInt32(ddlLocation.SelectedValue);
                else
                    return -1;
            }
        }

        public long SelectedPartyId
        {
            get
            {
                if (ddlLocation.SelectedIndex > 0)
                    return Convert.ToInt64(ddlLocation.SelectedValue);
                else
                    return -1;
            }
        }

        public DateTime RequiredOn
        {
            get
            {
                return Convert.ToDateTime(txtRequiredOn.Text.Trim());
            }
        }

        #endregion


    }
}

