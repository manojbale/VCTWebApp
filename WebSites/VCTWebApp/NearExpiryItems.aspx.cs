using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VCTWeb.Core.Domain;
using System.Data;
using System.ComponentModel;
using System.IO;
using Microsoft.Practices.ObjectBuilder;
using VCTWebApp.Resources;
using VCTWebApp.Web;
using System.Data.SqlClient;
using System.Globalization;
using System.Reflection;

namespace VCTWebApp.Shell.Views
{
    public partial class NearExpiryItems : Microsoft.Practices.CompositeWeb.Web.UI.Page//, IRegionalOfficeRequestView
    {
        #region Instance Variables

        //private RegionalOfficeRequestPresenter _presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        //private VCTWeb.Core.Domain.Helper helper = new VCTWeb.Core.Domain.Helper();
        private Security security = null;

        #endregion

        #region Private Properties

        //private bool CanView
        //{
        //    get
        //    {
        //        return ViewState[Common.CAN_VIEW] != null ? (bool)ViewState[Common.CAN_VIEW] : false;
        //    }
        //    set
        //    {
        //        ViewState[Common.CAN_VIEW] = value;
        //    }
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
                    PopulateLocation();
                    ////this.GetPendingRequestsData();
                    //this.LocalizePage();
                    //this.Presenter.OnViewInitialized();
                    ////this.Form.DefaultButton = this.btnSave.UniqueID; //Set the default button to save.

                    //this.DisplayMessageForMissingMasters();
                }
            }
            catch (SqlException ex)
            {
                //this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region Create New Presenter

        //[CreateNew]
        //public RegionalOfficeRequestPresenter Presenter
        //{
        //    get
        //    {
        //        return this._presenter;
        //    }
        //    set
        //    {
        //        if (value == null)
        //            throw new ArgumentNullException("value");

        //        this._presenter = value;
        //        this._presenter.View = this;
        //    }
        //}

        #endregion

        #region IRegionalOfficeRequestView Members

        #endregion

        #region Event Handlers

        protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = string.Empty;
                PopulateNearExpiryItems();
            }
            catch (Exception ex)
            {
                //this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        protected void gdvStockLevel_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gdvNearExpiryItems.PageIndex = e.NewPageIndex;
                PopulateNearExpiryItems();
            }
            catch (SqlException ex)
            {
                //this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void gdvNearExpiryItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkDelete = (LinkButton)e.Row.FindControl("lnkDelete");
                LocationPartDetail locationPartDetail = (LocationPartDetail)e.Row.DataItem;
                if (locationPartDetail.ExpiryDate < DateTime.Now)
                {
                    e.Row.ForeColor = System.Drawing.Color.Red;
                    if (locationPartDetail.PartStatus == Constants.PartStatus.Available.ToString())
                    {
                        lnkDelete.Attributes.Add("onclick", "javascript:return " + "confirm('" + vctResource.GetString("msgTrashSelectedItemConfirm") + "')");
                        lnkDelete.Visible = true;
                    }
                }
            }
        }

        protected void gdvNearExpiryItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("DeleteRec"))
                {
                    LocationRepository repository = new LocationRepository(HttpContext.Current.User.Identity.Name);
                    repository.DeleteDeleteItemFromLocationPartDetail(Convert.ToInt64(e.CommandArgument));
                    lblError.Text = "<font color='blue'>Item Trashed Successfully</font>";
                    PopulateNearExpiryItems();
                    //lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgPARLevelUpdate"), this.lblHeader.Text) + "</font>";
                }
            }
            catch (Exception ex)
            {
                //this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
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
            else if (security.HasAccess("NearExpiryItems"))
            {
                //CanCancel = security.HasPermission("NearExpiryItems");
            }
            else
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }

        private void PopulateNearExpiryItems()
        {
            gdvNearExpiryItems.DataSource = null;
            if (ddlLocation.SelectedIndex != 0)
            {
                if (ddlLocation.SelectedIndex == 1)
                {
                    gdvNearExpiryItems.DataSource = new LocationRepository().GetNearExpiryItemsByLocationId(0, Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"]));
                }
                else
                {
                    gdvNearExpiryItems.DataSource = new LocationRepository().GetNearExpiryItemsByLocationId(Convert.ToInt32(ddlLocation.SelectedValue), 0);
                }
            }
            gdvNearExpiryItems.DataBind();
        }

        private void PopulateLocation()
        {
            ddlLocation.DataSource = new LocationRepository().GetLocationByParentLocationId(Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"]));
            ddlLocation.DataTextField = "LocationName";
            ddlLocation.DataValueField = "LocationId";
            ddlLocation.DataBind();

            ddlLocation.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll")));
            ddlLocation.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect")));
        }

        #endregion

    }
}

