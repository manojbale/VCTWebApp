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
    public partial class InventoryCountExpectedKits : Microsoft.Practices.CompositeWeb.Web.UI.Page//, IRegionalOfficeRequestView
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
                    PopulateHospital();
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

        public List<VCTWeb.Core.Domain.VirtualCheckOut> VirtualCheckOutList
        {
            get
            {
                return (List<VCTWeb.Core.Domain.VirtualCheckOut>)ViewState["VirtualCheckOutList"];
            }
            set
            {
                ViewState["VirtualCheckOutList"] = value;
            }
        }

        #endregion

        #region Event Handlers

        protected void ddlHospital_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                PopulateExpectedKits();
            }
            catch (Exception ex)
            {
                //this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        protected void gdvInventoryCount_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                KitDetailStockLevel kitDetailStockLevel = (KitDetailStockLevel)e.Row.DataItem;
                if (kitDetailStockLevel.IsNearExpiry)
                    e.Row.ForeColor = System.Drawing.Color.Red;

                GridView grdChild = (GridView)e.Row.FindControl("grdChild");
                List<VCTWeb.Core.Domain.VirtualCheckOut> lst = this.VirtualCheckOutList.FindAll(v => v.BuildKitId == kitDetailStockLevel.BuildKitId);
                grdChild.DataSource = this.VirtualCheckOutList.FindAll(v => v.BuildKitId == kitDetailStockLevel.BuildKitId);
                //grdChild.DataSource = PopulateBuildKitById(kitDetailStockLevel.BuildKitId);
                grdChild.DataBind();
            }
        }

        protected void grdChild_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                VCTWeb.Core.Domain.VirtualCheckOut virtualCheckOut = (VCTWeb.Core.Domain.VirtualCheckOut)e.Row.DataItem;
                if (virtualCheckOut.IsNearExpiry)
                    e.Row.ForeColor = System.Drawing.Color.Red;
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
            else if (security.HasAccess("InventoryCountExpected"))
            {
                //CanCancel = security.HasPermission("InventoryCountExpected");
            }
            else
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }

        private void PopulateExpectedKits()
        {
            gdvInventoryCount.DataSource = null;
            if (ddlHospital.SelectedIndex != 0)
            {
                this.VirtualCheckOutList = new InventoryStockRepository().PopulateBuildKitItemsByLocationAndParty(Convert.ToInt32(Session["LoggedInLocationId"]), Convert.ToInt64(ddlHospital.SelectedValue));
                gdvInventoryCount.DataSource = new KitFamilyRepository().GetKitDetailStockLevelByLocationAndParty(Convert.ToInt32(Session["LoggedInLocationId"]), Convert.ToInt64(ddlHospital.SelectedValue));
            }
            gdvInventoryCount.DataBind();
        }

        private void PopulateHospital()
        {
            ddlHospital.DataSource = new PartyRepository().FetchParties(Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"]));
            ddlHospital.DataTextField = "Name";
            ddlHospital.DataValueField = "PartyId";
            ddlHospital.DataBind();

            ddlHospital.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect")));
        }

        #endregion

    }
}

