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
    public partial class StockLevelParts : Microsoft.Practices.CompositeWeb.Web.UI.Page//, IRegionalOfficeRequestView
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
                PopulatePartStockLevel();
            }
            catch (Exception ex)
            {
                //this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        protected void gdvStockLevel_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                PartStockLevel partStockLevel = (PartStockLevel)e.Row.DataItem;
                if (partStockLevel.IsNearExpiry)
                    e.Row.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void gdvStockLevel_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gdvStockLevel.PageIndex = e.NewPageIndex;
                PopulatePartStockLevel();
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

        protected void gdvStockLevel_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("PartNumberClick"))
                {
                    string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
                    ucPartDetailPopUp.PartNum = commandArgs[1];
                    ucPartDetailPopUp.Location = commandArgs[2];
                    ucPartDetailPopUp.PopulateData(Convert.ToInt32(commandArgs[0]), commandArgs[1]);
                    mpePartDetail.Show();
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
            else if (security.HasAccess("InventoryStockParts"))
            {
                //CanCancel = security.HasPermission("InventoryStockParts");
            }
            else
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }

        private void PopulatePartStockLevel()
        {
            gdvStockLevel.DataSource = null;
            if (ddlLocation.SelectedIndex != 0)
            {
                if (ddlLocation.SelectedIndex == 1)
                {
                    gdvStockLevel.DataSource = new KitFamilyRepository().GetPartStockLevelByLocationId(0, Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"]));
                }
                else
                {
                    gdvStockLevel.DataSource = new KitFamilyRepository().GetPartStockLevelByLocationId(Convert.ToInt32(ddlLocation.SelectedValue), 0);
                }
            }
            gdvStockLevel.DataBind();
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

