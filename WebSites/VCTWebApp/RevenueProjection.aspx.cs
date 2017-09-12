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
    public partial class RevenueProjection : Microsoft.Practices.CompositeWeb.Web.UI.Page//, IRegionalOfficeRequestView
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

                    DateTime dttmToday = DateTime.Now.AddDays(1);
                    while (dttmToday.DayOfWeek != DayOfWeek.Monday)
                        dttmToday = dttmToday.AddDays(1);
                    txtStartDate.Text = dttmToday.ToString("d");
                    dttmToday = dttmToday.AddDays(6);
                    txtEndDate.Text = dttmToday.ToString("d");
                    imgCalenderFrom.Visible = false;
                    Image1.Visible = false;
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
                //SetFieldsBlank();
                //if (ddlLocation.SelectedIndex > 0)
                //{
                //    PopulateRevenueProjection();
                //}
            }
            catch (Exception ex)
            {
                //this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlLocation.SelectedIndex == 0)
                {
                    lblError.Text = vctResource.GetString("mtTicketLocationRequired");
                    gdvRevenueProjection.DataSource = null;
                    gdvRevenueProjection.DataBind();
                    return;
                }

                DateTime startDate = Convert.ToDateTime(txtStartDate.Text);
                DateTime endDate = Convert.ToDateTime(txtEndDate.Text);

                if (startDate > endDate)
                {
                    lblError.Text = vctResource.GetString("StartDateGreaterEndDate");
                    gdvRevenueProjection.DataSource = null;
                    gdvRevenueProjection.DataBind();
                    return;
                }

                SetFieldsBlank();
                PopulateRevenueProjection();
            }
            catch
            {
                
            }
        }

        protected void rblPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gdvRevenueProjection.DataSource = null;
                gdvRevenueProjection.DataBind();

                if (rblPeriod.SelectedIndex == 0)
                {
                    DateTime dttmToday = DateTime.Now.AddDays(1);
                    while (dttmToday.DayOfWeek != DayOfWeek.Monday)
                        dttmToday = dttmToday.AddDays(1);
                    txtStartDate.Text = dttmToday.ToString("d");
                    dttmToday = dttmToday.AddDays(6);
                    txtEndDate.Text = dttmToday.ToString("d");
                    imgCalenderFrom.Visible = false;
                    Image1.Visible = false;
                }

                else if (rblPeriod.SelectedIndex == 1)
                {
                    DateTime dttmToday = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    txtStartDate.Text = dttmToday.ToString("d");
                    dttmToday = dttmToday.AddMonths(1).AddDays(-1);
                    txtEndDate.Text = dttmToday.ToString("d");
                    imgCalenderFrom.Visible = false;
                    Image1.Visible = false;
                }
                else
                {
                    imgCalenderFrom.Visible = true;
                    Image1.Visible = true;
                }
            }
            catch (Exception ex)
            {
                //this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        protected void gdvRevenueProjection_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView grdChild=e.Row.FindControl("grdChild") as GridView;
                VCTWeb.Core.Domain.RevenueProjection revenueProjection = (VCTWeb.Core.Domain.RevenueProjection)e.Row.DataItem;
                //grdChild.DataSource = ((List<VCTWeb.Core.Domain.RevenueProjection>)ViewState["RevenueProjectionList"]).Where(w => w.LocationId == revenueProjection.LocationId).ToList();
                grdChild.DataSource = lstRevenueProjection.Where(w => w.LocationId == revenueProjection.LocationId).ToList();
                grdChild.DataBind();
                //PartStockLevel partStockLevel = (PartStockLevel)e.Row.DataItem;
                //if (partStockLevel.IsNearExpiry)
                //    e.Row.ForeColor = System.Drawing.Color.Red;
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
            else if (security.HasAccess("RevenueProjection"))
            {
                //CanCancel = security.HasPermission("InventoryStockParts");
            }
            else
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }

        private void SetFieldsBlank()
        {
            tblTotalBar.Visible = false;
            lblKitRentalTotal.Text = "$ 0.00";
            lblKitPartTotal.Text = "$ 0.00";
            lblPartTotal.Text = "$ 0.00";
            lblGrandTotal.Text = "$ 0.00";
            gdvRevenueProjection.DataSource = null;
            gdvRevenueProjection.DataBind();
            lblError.Text = string.Empty;
        }

        List<VCTWeb.Core.Domain.RevenueProjection> lstRevenueProjection = new List<VCTWeb.Core.Domain.RevenueProjection>();

        private void PopulateRevenueProjection()
        {
           
            if (ddlLocation.SelectedIndex != 0)
            {
                List<VCTWeb.Core.Domain.RevenueProjection> lstRevenueProjectionSummary = new List<VCTWeb.Core.Domain.RevenueProjection>();
                lstRevenueProjection = new PartyRepository().GetRevenueProjectionByParentLocationId(Convert.ToInt32(ddlLocation.SelectedValue), Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text));
                //ViewState["RevenueProjectionList"] = lstRevenueProjection;
                List<int> distinctLocationIds = lstRevenueProjection.Select(s => s.LocationId).Distinct().ToList();
                foreach (int locationId in distinctLocationIds)
                {
                    string locationName = lstRevenueProjection.Where(w => w.LocationId == locationId).Select(s => s.LocationName).ToList()[0];
                    string locationType = lstRevenueProjection.Where(w => w.LocationId == locationId).Select(s => s.LocationType).ToList()[0];
                    lstRevenueProjectionSummary.Add(new VCTWeb.Core.Domain.RevenueProjection() {
                        LocationId=locationId,
                        LocationName = locationName,
                        LocationType=locationType,
                        KitRentalAmount = lstRevenueProjection.Where(w => w.LocationId == locationId).Sum(s => s.KitRentalAmount),
                        KitPartAmount = lstRevenueProjection.Where(w => w.LocationId == locationId).Sum(s => s.KitPartAmount),
                        PartAmount = lstRevenueProjection.Where(w => w.LocationId == locationId).Sum(s => s.PartAmount),
                        TotalAmount = lstRevenueProjection.Where(w => w.LocationId == locationId).Sum(s => s.TotalAmount)
                    });
                }

                if (lstRevenueProjectionSummary.Count <= 0)
                {
                   this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("noRecordFound"), this.lblHeader.Text);
                   
                }
                else
                {
                    lblError.Text = string.Empty;

                    gdvRevenueProjection.DataSource = lstRevenueProjectionSummary;
                    gdvRevenueProjection.DataBind();

                    decimal kitRentalAmount = lstRevenueProjection.Sum(s => s.KitRentalAmount);
                    decimal kitPartAmount = lstRevenueProjection.Sum(s => s.KitPartAmount);
                    decimal partAmount = lstRevenueProjection.Sum(s => s.PartAmount);
                    decimal totalAmount = lstRevenueProjection.Sum(s => s.TotalAmount);
                    lblKitRentalTotal.Text = "$ " + kitRentalAmount.ToString();
                    lblKitPartTotal.Text = "$ " + kitPartAmount.ToString();
                    lblPartTotal.Text = "$ " + partAmount.ToString();
                    lblGrandTotal.Text = "$ " + totalAmount.ToString();
                    tblTotalBar.Visible = true;
                }
            }
        }

        private void PopulateLocation()
        {
            ddlLocation.DataSource = new LocationRepository().GetLocationByParentLocationId(Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"]));
            ddlLocation.DataTextField = "LocationName";
            ddlLocation.DataValueField = "LocationId";
            ddlLocation.DataBind();

            //ddlLocation.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll")));
            ddlLocation.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect")));
        }

        #endregion

    }
}

