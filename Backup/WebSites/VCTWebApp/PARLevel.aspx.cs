using System;
using Microsoft.Practices.ObjectBuilder;
using System.Data;
using VCTWebApp.Resources;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Collections.Generic;
using VCTWebApp.Web;

namespace VCTWebApp.Shell.Views
{
    public partial class PARLevel : Microsoft.Practices.CompositeWeb.Web.UI.Page, IPARLevelView
    {
        #region Instance Variables
        private PARLevelPresenter _presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private Security security = null;
        #endregion

        #region Protected Methods

        protected void gdvPartDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gdvPartDetails.EditIndex = e.NewEditIndex;
                if (rblstLocationType.SelectedIndex == rblstLocationType.Items.Count - 1)
                {
                    this.PartyPARLevelList = this.PartyPARLevelList;
                }
                else
                {
                    this.LocationPARLevelList = this.LocationPARLevelList;
                }
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        protected void gdvPartDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gdvPartDetails.EditIndex = -1;
                if (rblstLocationType.SelectedIndex == rblstLocationType.Items.Count - 1)
                {
                    this.PartyPARLevelList = this.PartyPARLevelList;
                }
                else
                {
                    this.LocationPARLevelList = this.LocationPARLevelList;
                }
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ClearFields();
                if (ddlLocation.SelectedIndex > 0)
                {
                    if (rblstLocationType.SelectedIndex == rblstLocationType.Items.Count - 1)
                    {
                        Presenter.PopulatePartyPARLevel();
                    }
                    else
                    {
                        Presenter.PopulateLocationPARLevel();
                    }
                }
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        protected void gdvPartDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("UpdateRec"))
                {
                    HiddenField hdnPARLevelId = gdvPartDetails.Rows[gdvPartDetails.EditIndex].FindControl("hdnPARLevelId") as HiddenField;
                    TextBox txtPARLevelQty = gdvPartDetails.Rows[gdvPartDetails.EditIndex].FindControl("txtPARLevelQty") as TextBox;
                    if (hdnPARLevelId != null && txtPARLevelQty != null)
                    {
                        if (ValidateItemsOnEdit(txtPARLevelQty.Text))
                        {
                            if (rblstLocationType.SelectedIndex == rblstLocationType.Items.Count - 1)
                            {
                                if (Presenter.UpdatePartyPARLevelQuantity(Convert.ToInt64(hdnPARLevelId.Value), Convert.ToInt32(txtPARLevelQty.Text)))
                                {
                                    lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgPARLevelUpdate"), this.lblHeader.Text) + "</font>";
                                    this.PartyPARLevelList[gdvPartDetails.EditIndex].PARLevelQty = Convert.ToInt32(txtPARLevelQty.Text);
                                    gdvPartDetails.EditIndex = -1;
                                    this.PartyPARLevelList = this.PartyPARLevelList;

                                }
                            }
                            else
                            {
                                if (Presenter.UpdateLocationPARLevelQuantity(Convert.ToInt64(hdnPARLevelId.Value), Convert.ToInt32(txtPARLevelQty.Text)))
                                {
                                    lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgPARLevelUpdate"), this.lblHeader.Text) + "</font>";
                                    this.LocationPARLevelList[gdvPartDetails.EditIndex].PARLevelQty = Convert.ToInt32(txtPARLevelQty.Text);
                                    gdvPartDetails.EditIndex = -1;
                                    this.LocationPARLevelList = this.LocationPARLevelList;

                                }
                            }
                        }
                    }
                }
                else if (e.CommandName.Equals("AddNewRow"))
                {
                    if (hdnPartNumNew.Value == string.Empty)
                    {
                        //lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valPartNumberEmpty"), this.lblHeader.Text);
                        lblError.Text = "Please enter Ref Number ";
                    }
                    else
                    {
                        TextBox txtNewPARLevelQty = gdvPartDetails.HeaderRow.FindControl("txtNewPARLevelQty") as TextBox;
                        if (txtNewPARLevelQty != null)
                        {
                            if (ValidateItemsOnAdd(hdnPartNumNew.Value, txtNewPARLevelQty.Text))
                            {
                                if (ddlLocation.SelectedIndex > 0)
                                {
                                    if (rblstLocationType.SelectedIndex == rblstLocationType.Items.Count - 1)
                                    {
                                        if (Presenter.AddPartyPARLevelQuantity(hdnPartNumNew.Value, Convert.ToInt32(txtNewPARLevelQty.Text)))
                                        {
                                            lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgPARLevelAdded"), this.lblHeader.Text) + "</font>";
                                            Presenter.PopulatePartyPARLevel();
                                            hdnPartNumNew.Value = string.Empty;
                                        }
                                    }
                                    else
                                    {
                                        if (Presenter.AddLocationPARLevelQuantity(hdnPartNumNew.Value, Convert.ToInt32(txtNewPARLevelQty.Text)))
                                        {
                                            lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgPARLevelAdded"), this.lblHeader.Text) + "</font>";
                                            Presenter.PopulateLocationPARLevel();
                                            hdnPartNumNew.Value = string.Empty;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (e.CommandName.Equals("DeleteRec"))
                {
                   
                    
                    long parLevelId = Convert.ToInt64(e.CommandArgument);              
                    if (parLevelId > -1)
                    {
                        if (rblstLocationType.SelectedIndex == rblstLocationType.Items.Count - 1)
                        {
                            if (Presenter.DeletePartyPARLevelQuantity(parLevelId))
                            {
                                lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgPARLevelDelete"), this.lblHeader.Text) + "</font>";
                                this.PartyPARLevelList.RemoveAll(i => i.PARLevelId == parLevelId);
                                this.PartyPARLevelList = this.PartyPARLevelList;

                            }
                        }
                        else
                        {
                            if (Presenter.DeleteLocationPARLevelQuantity(Convert.ToInt64(parLevelId)))
                            {
                                lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgPARLevelUpdate"), this.lblHeader.Text) + "</font>";
                                this.LocationPARLevelList.RemoveAll(i => i.PARLevelId == parLevelId);
                                this.LocationPARLevelList = this.LocationPARLevelList;

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }
        protected void gdvPartDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    LinkButton l = (LinkButton)e.Row.FindControl("lnkDelete");
            //    l.Attributes.Add("onclick", "javascript:return " +
            //    "confirm('Are you sure you want to delete this record ");
            //}
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
            lblError.Text = string.Empty;
            gdvPartDetails.DataSource = null;
            gdvPartDetails.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Text = string.Empty;
            if (!this.IsPostBack)
            {
                this.AuthorizedPage();
                this._presenter.OnViewInitialized();
                this.LocalizePage();
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
            else if (security.HasAccess("PARLevel"))
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
                heading = vctResource.GetString("mnuPARLevel");
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
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valPartNumberEmpty"), this.lblHeader.Text);
                return false;
            }
            if (rblstLocationType.SelectedIndex == rblstLocationType.Items.Count - 1)
            {
                var item = this.PartyPARLevelList.Find(t => t.PartNum == newParNum);
                if (item != null)
                {
                    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valPartNumberExists"), this.lblHeader.Text);
                    return false;
                }
            }
            else
            {
                var item = this.LocationPARLevelList.Find(t => t.PartNum == newParNum);
                if (item != null)
                {
                    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valPartNumberExists"), this.lblHeader.Text);
                    return false;
                }
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

        #region Create New Presenter
        [CreateNew]
        public PARLevelPresenter Presenter
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

        public List<VCTWeb.Core.Domain.LocationPARLevel> LocationPARLevelList
        {
            get
            {
                return (List<VCTWeb.Core.Domain.LocationPARLevel>)ViewState["LocationPARLevelList"];
            }
            set
            {
                ViewState["LocationPARLevelList"] = value;
                this.gdvPartDetails.DataSource = value;
                this.gdvPartDetails.DataBind();
            }
        }

        public List<VCTWeb.Core.Domain.PartyPARLevel> PartyPARLevelList
        {
            get
            {
                return (List<VCTWeb.Core.Domain.PartyPARLevel>)ViewState["PartyPARLevelList"];
            }
            set
            {
                ViewState["PartyPARLevelList"] = value;
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

        #endregion


    }
}

