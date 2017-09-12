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
    public partial class StockLevelKits : Microsoft.Practices.CompositeWeb.Web.UI.Page//, IRegionalOfficeRequestView
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
                    btnExport.Visible = false;
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



        protected void txtKitFamily_TextChanged(object sender, EventArgs e)
        {
            //if (Convert.ToInt64(hdnKitFamilyId.Value) == 0)
            //{
            //    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valKitFamily"), this.lblHeader.Text);
            //   // return;
            //}
            //else
            //{
            //    Presenter.PopulateKitTableList();
            //}

        }
        protected void btnExport_Click(object sender, EventArgs e)
        {

            int LocationId = 0;
            int kitFamilyId = 0;

            if (ddlLocation.SelectedValue != "All")
            {
                LocationId = Convert.ToInt32(ddlLocation.SelectedValue);
            }
            if (Convert.ToInt64(hdnKitFamilyId.Value) != 0)
            {
                kitFamilyId = Convert.ToInt32(hdnKitFamilyId.Value);
            }

            List<VCTWeb.Core.Domain.VirtualCheckOut> lstInventoryReport = new KitFamilyRepository().GetInventoryReport(LocationId, kitFamilyId);
            if (lstInventoryReport != null && lstInventoryReport.Count > 0)
            {
                ExportInExcel(lstInventoryReport);
            }

        }

        protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {


            }
            catch (Exception ex)
            {
                //this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }


        protected void gdvStockLevel_Sorting(object sender, GridViewSortEventArgs e)
        {
            Image sortImage = new Image();
            string sortingDirection = string.Empty;
            if (direction == SortDirection.Ascending)
            {
                direction = SortDirection.Descending;
                sortingDirection = "Desc";
                sortImage.ImageUrl = "./Images/sort_down.png";

            }
            else
            {
                direction = SortDirection.Ascending;
                sortingDirection = "Asc";
                sortImage.ImageUrl = "../Images/sort_up.png";

            }

            if (ViewState["ListKitStockLevel"] != null)
            {

                //Sort the data.
                List<KitStockLevel> listKitStockLevel = ViewState["ListKitStockLevel"] as List<KitStockLevel>;
                if (sortingDirection == "Desc")
                {
                    switch (e.SortExpression.Trim())
                    {
                        case "AvailableQuantity":
                            listKitStockLevel = listKitStockLevel.OrderByDescending(p => p.AvailableQuantity).ToList();
                            break; 
                        case "AssignedToCaseQuantity":
                            listKitStockLevel = listKitStockLevel.OrderByDescending(p => p.AssignedToCaseQuantity).ToList();
                            break;
                        case "LeastExpiryDate":
                            listKitStockLevel = listKitStockLevel.OrderByDescending(p => p.LeastExpiryDate).ToList();
                            break;
                        case "LocationName":
                            listKitStockLevel = listKitStockLevel.OrderByDescending(p => p.LocationName).ToList();
                            break;
                        case "LocationType":
                            listKitStockLevel = listKitStockLevel.OrderByDescending(p => p.LocationType).ToList();
                            break;
                        case "KitFamilyName":
                            listKitStockLevel = listKitStockLevel.OrderByDescending(p => p.KitFamilyName).ToList();
                            break;
                        case "KitFamilyDescription":
                            listKitStockLevel = listKitStockLevel.OrderByDescending(p => p.KitFamilyDescription).ToList();
                            break;
                        case "ShippedQuantity":
                            listKitStockLevel = listKitStockLevel.OrderByDescending(p => p.ShippedQuantity).ToList();
                            break;
                        case "ReceivedQuantity":
                            listKitStockLevel = listKitStockLevel.OrderByDescending(p => p.ReceivedQuantity).ToList();
                            break; 
                       
                    }
                   
                }
                else
                {
                    switch (e.SortExpression.Trim())
                    {
                        case "AvailableQuantity":
                            listKitStockLevel = listKitStockLevel.OrderBy(p => p.AvailableQuantity).ToList();
                            break;
                        case "AssignedToCaseQuantity":
                            listKitStockLevel = listKitStockLevel.OrderBy(p => p.AssignedToCaseQuantity).ToList();
                            break;
                        case "LeastExpiryDate":
                            listKitStockLevel = listKitStockLevel.OrderBy(p => p.LeastExpiryDate).ToList();
                            break;
                        case "LocationName":
                            listKitStockLevel = listKitStockLevel.OrderBy(p => p.LocationName).ToList();
                            break;
                        case "LocationType":
                            listKitStockLevel = listKitStockLevel.OrderBy(p => p.LocationType).ToList();
                            break;
                        case "KitFamilyName":
                            listKitStockLevel = listKitStockLevel.OrderBy(p => p.KitFamilyName).ToList();
                            break;
                        case "KitFamilyDescription":
                            listKitStockLevel = listKitStockLevel.OrderBy(p => p.KitFamilyDescription).ToList();
                            break;
                        case "ShippedQuantity":
                            listKitStockLevel = listKitStockLevel.OrderBy(p => p.ShippedQuantity).ToList();
                            break;
                        case "ReceivedQuantity":
                            listKitStockLevel = listKitStockLevel.OrderBy(p => p.ReceivedQuantity).ToList();
                            break; 
                    }
                }

                gdvStockLevel.DataSource = listKitStockLevel;
                gdvStockLevel.DataBind();

                int columnIndex = 0;
                foreach (DataControlFieldHeaderCell headerCell in gdvStockLevel.HeaderRow.Cells)
                {
                    if (headerCell.ContainingField.SortExpression == e.SortExpression)
                    {
                        columnIndex = gdvStockLevel.HeaderRow.Cells.GetCellIndex(headerCell);
                    }
                }
                gdvStockLevel.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);

            }
        }



        protected void gdvStockLevel_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                KitStockLevel kitStockLevel = (KitStockLevel)e.Row.DataItem;
                if (kitStockLevel.IsNearExpiry)
                    e.Row.ForeColor = System.Drawing.Color.Red;
            }
        }



        protected void gdvStockLevel_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gdvStockLevel.PageIndex = e.NewPageIndex;
                PopulateKitStockLevel();
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
                if (e.CommandName.Equals("KitFamilyClick"))
                {
                    string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
                    ucKitDetailPopUp.Location = commandArgs[2];
                    ucKitDetailPopUp.KitFamily = commandArgs[3];
                    ucKitDetailPopUp.PopulateData(Convert.ToInt32(commandArgs[0]), Convert.ToInt64(commandArgs[1]));
                    mpeKitDetail.Show();
                }
            }
            catch (Exception ex)
            {
                //this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }


        protected void lnkFilterCustomerListData_Click(object sender, EventArgs e)
        {
            try
            {
                PopulateKitStockLevel();
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                gdvStockLevel.DataSource = null;
                gdvStockLevel.DataBind();
                btnExport.Visible = false;
                ddlLocation.SelectedIndex = 0;
                txtKitFamily.Text = string.Empty;
                hdnKitFamilyId.Value = string.Empty;
                lblError.Text = string.Empty;
            }
            catch (Exception ex)
            {

            }
        }


        #endregion

        #region Private Methods


        private void ExportInExcel(List<VCTWeb.Core.Domain.VirtualCheckOut> lstInventoryReport)
        {
            if (lstInventoryReport != null && lstInventoryReport.Count > 0)
            {
                DataTable dtInventoryReportTable = new DataTable();
                dtInventoryReportTable.Columns.Add("Kit #", typeof(string));
                dtInventoryReportTable.Columns.Add("Kit Description", typeof(string));
                dtInventoryReportTable.Columns.Add("Part #", typeof(string));
                dtInventoryReportTable.Columns.Add("Part Description", typeof(string));
                dtInventoryReportTable.Columns.Add("Lot #", typeof(string));
                dtInventoryReportTable.Columns.Add("Expiry Date", typeof(string));
                dtInventoryReportTable.Columns.Add("Last Build Date", typeof(string));

                foreach (VCTWeb.Core.Domain.VirtualCheckOut virtualCheckOut in lstInventoryReport)
                {
                    DataRow dr;
                    dr = dtInventoryReportTable.NewRow();

                    dr["Kit #"] = virtualCheckOut.KitNumber != null ? virtualCheckOut.KitNumber.ToString() : string.Empty;
                    dr["Kit Description"] = virtualCheckOut.Description != null ? virtualCheckOut.Description.ToString() : string.Empty;
                    dr["Part #"] = virtualCheckOut.PartNum != null ? virtualCheckOut.PartNum.ToString() : string.Empty;
                    dr["Part Description"] = virtualCheckOut.PartDescription != null ? virtualCheckOut.PartDescription.ToString() : string.Empty;
                    dr["Lot #"] = virtualCheckOut.LotNum != null ? virtualCheckOut.LotNum.ToString() : string.Empty;
                    dr["Expiry Date"] = (DateTime.Parse(virtualCheckOut.ExpiryDate.ToString())).ToString("MM/dd/yyyy");
                    dr["Last Build Date"] = (DateTime.Parse(virtualCheckOut.BuildDate.ToString())).ToString("MM/dd/yyyy HH:mm:ss");

                    dtInventoryReportTable.Rows.Add(dr);
                }


                grdViewExport.DataSource = dtInventoryReportTable;
                grdViewExport.DataBind();

                string fileName = "InventoryExport_" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString()
                                    + "_" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".xls";


                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                Response.Charset = "";
                Response.ContentType = "application/vnd.xls";
                StringWriter StringWriter = new System.IO.StringWriter();
                HtmlTextWriter HtmlTextWriter = new HtmlTextWriter(StringWriter);

                grdViewExport.RenderControl(HtmlTextWriter);
                Response.Write(StringWriter.ToString());

                grdViewExport.DataSource = null;
                grdViewExport.DataBind();
                Response.End();

            }
        }

        private void AuthorizedPage()
        {
            security = new Security();
            if (Session["LoggedInUser"] == null)
            {
                Response.Redirect("~/ErrorPage.aspx?ErrorKey=Common_msgSessionExpired");
            }
            else if (security.HasAccess("InventoryStockKits"))
            {
                //CanCancel = security.HasPermission("InventoryStockKits");
            }
            else
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }



        private void PopulateKitStockLevel()
        {
            Int64 kitFamilyId = 0;

            if (ddlLocation.SelectedIndex == 0)
            {
                ClearFields();
                lblError.Text = "Please select location ";
                return;
            }

            if (!string.IsNullOrEmpty(txtKitFamily.Text) && Convert.ToInt64(hdnKitFamilyId.Value) == 0)
            {
                ClearFields();
                lblError.Text = "Please enter valid Kit family ";
                return;
            }

            if (!string.IsNullOrEmpty(txtKitFamily.Text) && Convert.ToInt64(hdnKitFamilyId.Value) != 0)
            {
                kitFamilyId = Convert.ToInt64(hdnKitFamilyId.Value);
            }

            lblError.Text = string.Empty;

            List<KitStockLevel> listKitStockLevel = null;
            if (ddlLocation.SelectedIndex == 1)
            {
                listKitStockLevel = new KitFamilyRepository().GetKitStockLevelByLocationId(0, Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"]), kitFamilyId);
                //gdvStockLevel.DataSource = new KitFamilyRepository().GetKitStockLevelByLocationId(0, Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"]));
            }
            else
            {
                listKitStockLevel = new KitFamilyRepository().GetKitStockLevelByLocationId(Convert.ToInt32(ddlLocation.SelectedValue), 0, kitFamilyId);
                // gdvStockLevel.DataSource = new KitFamilyRepository().GetKitStockLevelByLocationId(Convert.ToInt32(ddlLocation.SelectedValue), 0);
            }

            gdvStockLevel.DataSource = listKitStockLevel;
            gdvStockLevel.DataBind();
            if (listKitStockLevel.Count > 0)
            {
                ViewState["ListKitStockLevel"] = listKitStockLevel;
                btnExport.Visible = true;
            }
            else
            {
                ViewState["ListKitStockLevel"] = null;
                btnExport.Visible = false;
            }


        }

        private void ClearFields()
        {
            txtKitFamily.Text = string.Empty;
            gdvStockLevel.DataSource = null;
            gdvStockLevel.DataBind();
            btnExport.Visible = false;
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

        #region Public Methods


        public SortDirection direction
        {
            get
            {
                if (ViewState["directionState"] == null)
                {
                    ViewState["directionState"] = SortDirection.Ascending;
                }
                return (SortDirection)ViewState["directionState"];
            }
            set
            {
                ViewState["directionState"] = value;
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }



        #endregion

    }
}

