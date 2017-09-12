using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using VCTWeb.Core.Domain;
using System.Data;
using System.IO;
using Microsoft.Practices.ObjectBuilder;
using VCTWebApp.Resources;
using VCTWebApp.Web;
using System.Data.SqlClient;

namespace VCTWebApp.Shell.Views
{
    public partial class eParPlusShipAndBillReport : Microsoft.Practices.CompositeWeb.Web.UI.Page, IeParPlusShipAndBillReport
    {
        #region Instance Variables
        List<VCTWeb.Core.Domain.OrderDetail> lstOrderDetail = new List<OrderDetail>();
        private eParPlusShipAndBillReportPresenter presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private Security security = null;
        #endregion

        #region Properties

        private bool CanAdjust
        {
            get
            {
                return ViewState[Common.CAN_ADJUST] != null ? (bool)ViewState[Common.CAN_ADJUST] : false;
            }
            set
            {
                ViewState[Common.CAN_ADJUST] = value;
            }
        }


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

        #endregion

        #region Init/Page Load

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    this.AuthorizedPage();
                    //PopulateLocation();
                    this.presenter.OnViewInitialized();
                    txtOrderEndDate.Text = DateTime.Now.ToString("d");
                    txtOrderStartDate.Text = DateTime.Now.AddDays(-6).ToString("d");

                    //txtShippedEndDate.Text = DateTime.Now.ToString("d");
                    //txtShippedStartDate.Text = DateTime.Now.AddDays(-6).ToString("d");

                    btnExport.Visible = false;
                }
                this.LocalizePage();
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

        [CreateNew]
        public eParPlusShipAndBillReportPresenter Presenter
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

        #region Event Handlers

        protected void gdvShipandBill_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SaveAdjustment")
            {
                Int32 OrderId = Convert.ToInt32(e.CommandArgument);
                if (presenter.UpdateAdjustmentQty(OrderId))
                    lblError.Text = "";
                else
                    lblError.Text = "Problem in Adjusting the Ordered qty.Please try after refreshing the page.";
                presenter.PopulateReport();
            }
        }

        protected void gdvShipandBill_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[14].ToolTip = "* Remaining Qty = Shipped Qty - Received Qty - Adjusted Qty";
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    GridView grdChild = e.Row.FindControl("grdChild") as GridView;
                    List<OrderDetail> ListOrderAdjustment = new List<OrderDetail>();
                    VCTWeb.Core.Domain.OrderDetail objOrderDetail = (VCTWeb.Core.Domain.OrderDetail)e.Row.DataItem;
                    lstOrderDetail = Session["ListOrderDetail"] as List<OrderDetail>;
                    ListOrderAdjustment = lstOrderDetail.FindAll(w => w.AccountNumber == objOrderDetail.AccountNumber && w.OrderId == objOrderDetail.OrderId).ToList();


                    ListOrderAdjustment = ListOrderAdjustment.FindAll(x => x.DispositionType.Trim() != string.Empty);

                    grdChild.DataSource = ListOrderAdjustment;
                    grdChild.DataBind();

                    if (objOrderDetail.RemainingQty == 0)
                    {
                        ImageButton imageButton = e.Row.FindControl("lnkAdjust") as ImageButton;
                        imageButton.Visible = false;
                    }
                }
            }
            catch
            {

            }
        }

        protected void gdvShipandBill_Sorting(object sender, GridViewSortEventArgs e)
        {
            System.Web.UI.WebControls.Image sortImage = new System.Web.UI.WebControls.Image();
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
                sortImage.ImageUrl = "./Images/sort_up.png";
            }

            if (Session["ListOrderDetail"] != null)
            {
                List<OrderDetail> listOrderDetail = Session["ListOrderDetail"] as List<OrderDetail>;

                if (sortingDirection == "Desc")
                {
                    if (e.SortExpression.Trim() == "CustomerName")
                        listOrderDetail = listOrderDetail.OrderByDescending(p => p.CustomerName).ToList();

                    if (e.SortExpression.Trim() == "OrderNumber")
                        listOrderDetail = listOrderDetail.OrderByDescending(p => p.OrderNumber).ToList();

                    if (e.SortExpression.Trim() == "LineNumber")
                        listOrderDetail = listOrderDetail.OrderByDescending(p => p.LineNumber).ToList();

                    if (e.SortExpression.Trim() == "RefNum")
                        listOrderDetail = listOrderDetail.OrderByDescending(p => p.RefNum).ToList();

                    if (e.SortExpression.Trim() == "OrderedQty")
                        listOrderDetail = listOrderDetail.OrderByDescending(p => p.OrderedQty).ToList();

                    if (e.SortExpression.Trim() == "ShippedQty")
                        listOrderDetail = listOrderDetail.OrderByDescending(p => p.ShippedQty).ToList();

                    if (e.SortExpression.Trim() == "CancelledQty")
                        listOrderDetail = listOrderDetail.OrderByDescending(p => p.CancelledQty).ToList();

                    if (e.SortExpression.Trim() == "OrderStatus")
                        listOrderDetail = listOrderDetail.OrderByDescending(p => p.OrderStatus).ToList();

                    if (e.SortExpression.Trim() == "OrderDate")
                        listOrderDetail = listOrderDetail.OrderByDescending(p => p.OrderDate).ToList();

                    if (e.SortExpression.Trim() == "ShippedDate")
                        listOrderDetail = listOrderDetail.OrderByDescending(p => p.ShippedDate).ToList();

                    if (e.SortExpression.Trim() == "ReceivedQty")
                        listOrderDetail = listOrderDetail.OrderByDescending(p => p.ReceivedQty).ToList();

                    if (e.SortExpression.Trim() == "RemainingQty")
                        listOrderDetail = listOrderDetail.OrderByDescending(p => p.RemainingQty).ToList();

                    if (e.SortExpression.Trim() == "AdjustQty")
                        listOrderDetail = listOrderDetail.OrderByDescending(p => p.AdjustQty).ToList();

                }
                else
                {
                    if (e.SortExpression.Trim() == "CustomerName")
                        listOrderDetail = listOrderDetail.OrderBy(p => p.CustomerName).ToList();

                    if (e.SortExpression.Trim() == "OrderNumber")
                        listOrderDetail = listOrderDetail.OrderBy(p => p.OrderNumber).ToList();

                    if (e.SortExpression.Trim() == "LineNumber")
                        listOrderDetail = listOrderDetail.OrderBy(p => p.LineNumber).ToList();

                    if (e.SortExpression.Trim() == "RefNum")
                        listOrderDetail = listOrderDetail.OrderBy(p => p.RefNum).ToList();

                    if (e.SortExpression.Trim() == "OrderedQty")
                        listOrderDetail = listOrderDetail.OrderBy(p => p.OrderedQty).ToList();

                    if (e.SortExpression.Trim() == "ShippedQty")
                        listOrderDetail = listOrderDetail.OrderBy(p => p.ShippedQty).ToList();

                    if (e.SortExpression.Trim() == "CancelledQty")
                        listOrderDetail = listOrderDetail.OrderBy(p => p.CancelledQty).ToList();

                    if (e.SortExpression.Trim() == "OrderStatus")
                        listOrderDetail = listOrderDetail.OrderBy(p => p.OrderStatus).ToList();

                    if (e.SortExpression.Trim() == "OrderDate")
                        listOrderDetail = listOrderDetail.OrderBy(p => p.OrderDate).ToList();

                    if (e.SortExpression.Trim() == "ShippedDate")
                        listOrderDetail = listOrderDetail.OrderBy(p => p.ShippedDate).ToList();

                    if (e.SortExpression.Trim() == "ReceivedQty")
                        listOrderDetail = listOrderDetail.OrderBy(p => p.ReceivedQty).ToList();

                    if (e.SortExpression.Trim() == "RemainingQty")
                        listOrderDetail = listOrderDetail.OrderBy(p => p.RemainingQty).ToList();

                    if (e.SortExpression.Trim() == "AdjustQty")
                        listOrderDetail = listOrderDetail.OrderBy(p => p.AdjustQty).ToList();

                }

                ListOrderDetail = listOrderDetail;

                int columnIndex = 0;
                foreach (DataControlFieldHeaderCell headerCell in gdvShipandBill.HeaderRow.Cells)
                {
                    if (headerCell.ContainingField.SortExpression == e.SortExpression)
                    {
                        columnIndex = gdvShipandBill.HeaderRow.Cells.GetCellIndex(headerCell);
                    }
                }
                gdvShipandBill.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);
            }
        }

        protected void grdChild_Sorting(object sender, GridViewSortEventArgs e)
        {
            //GridView gdvChild = (GridView)sender;

            //System.Web.UI.WebControls.Image sortImage = new System.Web.UI.WebControls.Image();
            //string sortingDirection = string.Empty;

            //if (direction == SortDirection.Ascending)
            //{
            //    direction = SortDirection.Descending;
            //    sortingDirection = "Desc";
            //    sortImage.ImageUrl = "./Images/sort_down.png";
            //}
            //else
            //{
            //    direction = SortDirection.Ascending;
            //    sortingDirection = "Asc";
            //    sortImage.ImageUrl = "./Images/sort_up.png";
            //}

            //string AccountNumber = string.Empty;
            //string CustomerName = string.Empty;

            //foreach (GridViewRow gvr in gdvChild.Rows)
            //{
            //    Label lblAccountNumber = gvr.FindControl("lblAccountNumber") as Label;
            //    Label lblCustomerName = gvr.FindControl("lblCustomerName") as Label;
            //    if (lblAccountNumber != null && lblCustomerName != null)
            //    {
            //        AccountNumber = lblAccountNumber.Text;
            //        CustomerName = lblCustomerName.Text;
            //        break;
            //    }
            //}

            //if (AccountNumber != string.Empty && CustomerName != string.Empty)
            //{
            //    List<ConsumptionRate> listListConsumptionRate = Session["ListConsumptionRate"] as List<ConsumptionRate>;
            //    List<ConsumptionRate> listListConsumptionRateChild = new List<ConsumptionRate>();

            //    listListConsumptionRateChild = listListConsumptionRate.FindAll(i => i.AccountNumber == AccountNumber);
            //    if (sortingDirection == "Desc")
            //    {
            //        if (e.SortExpression.Trim() == "RefNum")
            //            listListConsumptionRateChild = listListConsumptionRateChild.OrderByDescending(p => p.RefNum).ToList();

            //        if (e.SortExpression.Trim() == "PartDesc")
            //            listListConsumptionRateChild = listListConsumptionRateChild.OrderByDescending(p => p.PartDesc).ToList();

            //        if (e.SortExpression.Trim() == "ConsumedQty")
            //            listListConsumptionRateChild = listListConsumptionRateChild.OrderByDescending(p => p.ConsumedQty).ToList();

            //        if (e.SortExpression.Trim() == "NoOfDays")
            //            listListConsumptionRateChild = listListConsumptionRateChild.OrderByDescending(p => p.NoOfDays).ToList();

            //        if (e.SortExpression.Trim() == "ConsumptionRatePercent")
            //            listListConsumptionRateChild = listListConsumptionRateChild.OrderByDescending(p => p.ConsumptionRatePercent).ToList();
            //    }
            //    else
            //    {
            //        if (e.SortExpression.Trim() == "RefNum")
            //            listListConsumptionRateChild = listListConsumptionRateChild.OrderBy(p => p.RefNum).ToList();

            //        if (e.SortExpression.Trim() == "PartDesc")
            //            listListConsumptionRateChild = listListConsumptionRateChild.OrderBy(p => p.PartDesc).ToList();

            //        if (e.SortExpression.Trim() == "ConsumedQty")
            //            listListConsumptionRateChild = listListConsumptionRateChild.OrderBy(p => p.ConsumedQty).ToList();

            //        if (e.SortExpression.Trim() == "NoOfDays")
            //            listListConsumptionRateChild = listListConsumptionRateChild.OrderBy(p => p.NoOfDays).ToList();

            //        if (e.SortExpression.Trim() == "ConsumptionRatePercent")
            //            listListConsumptionRateChild = listListConsumptionRateChild.OrderBy(p => p.ConsumptionRatePercent).ToList();
            //    }

            //    gdvChild.DataSource = listListConsumptionRateChild;
            //    gdvChild.DataBind();

            //    int columnIndex = 0;
            //    foreach (DataControlFieldHeaderCell headerCell in gdvChild.HeaderRow.Cells)
            //    {
            //        if (headerCell.ContainingField.SortExpression == e.SortExpression)
            //        {
            //            columnIndex = gdvChild.HeaderRow.Cells.GetCellIndex(headerCell);
            //            gdvChild.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);
            //        }
            //    }
            //}
        }

        //protected void rblPeriodOrder_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        gdvShipandBill.DataSource = null;
        //        gdvShipandBill.DataBind();
        //        btnExport.Visible = false;
        //        if (rblPeriodOrder.SelectedIndex == 0)
        //        {
        //            txtOrderEndDate.Text = DateTime.Now.ToString("d");
        //            txtOrderStartDate.Text = DateTime.Now.AddDays(-6).ToString("d");
        //            imgOrderCalenderFrom.Visible = false;
        //            imgOrderCalenderTo.Visible = false;
        //        }

        //        else if (rblPeriodOrder.SelectedIndex == 1)
        //        {
        //            txtOrderEndDate.Text = DateTime.Now.ToString("d");
        //            txtOrderStartDate.Text = DateTime.Now.AddMonths(-1).ToString("d");
        //            imgOrderCalenderFrom.Visible = false;
        //            imgOrderCalenderTo.Visible = false;
        //        }
        //        else
        //        {
        //            imgOrderCalenderFrom.Visible = true;
        //            imgOrderCalenderTo.Visible = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
        //    }
        //}

        protected void rbOrderDateLastOneWeek_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                gdvShipandBill.DataSource = null;
                gdvShipandBill.DataBind();
                btnExport.Visible = false;
                txtOrderEndDate.Text = DateTime.Now.ToString("d");
                txtOrderStartDate.Text = DateTime.Now.AddDays(-6).ToString("d");
                imgOrderCalenderFrom.Visible = false;
                imgOrderCalenderTo.Visible = false;

                txtShippedStartDate.Text = string.Empty;
                txtShippedEndDate.Text = string.Empty;
                imgShippedCalenderFrom.Visible = false;
                imgShippedCalenderTo.Visible = false;
            }
            catch (Exception ex)
            {
                //this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        protected void rbOrderDateLastOneMonth_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                gdvShipandBill.DataSource = null;
                gdvShipandBill.DataBind();
                btnExport.Visible = false;
                txtOrderEndDate.Text = DateTime.Now.ToString("d");
                txtOrderStartDate.Text = DateTime.Now.AddMonths(-1).ToString("d");
                imgOrderCalenderFrom.Visible = false;
                imgOrderCalenderTo.Visible = false;

                txtShippedStartDate.Text = string.Empty;
                txtShippedEndDate.Text = string.Empty;
                imgShippedCalenderFrom.Visible = false;
                imgShippedCalenderTo.Visible = false;
            }
            catch (Exception ex)
            {
                //this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        protected void rbOrderDateRange_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                gdvShipandBill.DataSource = null;
                gdvShipandBill.DataBind();
                btnExport.Visible = false;
                imgOrderCalenderFrom.Visible = true;
                imgOrderCalenderTo.Visible = true;

                txtShippedStartDate.Text = string.Empty;
                txtShippedEndDate.Text = string.Empty;
                imgShippedCalenderFrom.Visible = false;
                imgShippedCalenderTo.Visible = false;
            }
            catch (Exception ex)
            {
                //this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        //protected void rblPeriodShipped_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        gdvShipandBill.DataSource = null;
        //        gdvShipandBill.DataBind();
        //        btnExport.Visible = false;
        //        if (rblPeriodShipped.SelectedIndex == 0)
        //        {
        //            txtShippedEndDate.Text = DateTime.Now.ToString("d");
        //            txtShippedStartDate.Text = DateTime.Now.AddDays(-6).ToString("d");
        //            imgShippedCalenderFrom.Visible = false;
        //            imgShippedCalenderTo.Visible = false;
        //        }

        //        else if (rblPeriodShipped.SelectedIndex == 1)
        //        {
        //            txtShippedEndDate.Text = DateTime.Now.ToString("d");
        //            txtShippedStartDate.Text = DateTime.Now.AddMonths(-1).ToString("d");
        //            imgShippedCalenderFrom.Visible = false;
        //            imgShippedCalenderTo.Visible = false;
        //        }
        //        else
        //        {
        //            imgShippedCalenderFrom.Visible = true;
        //            imgShippedCalenderTo.Visible = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
        //    }
        //}

        protected void rbShippedDateLastOneWeek_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                gdvShipandBill.DataSource = null;
                gdvShipandBill.DataBind();
                btnExport.Visible = false;
                txtShippedEndDate.Text = DateTime.Now.ToString("d");
                txtShippedStartDate.Text = DateTime.Now.AddDays(-6).ToString("d");
                imgShippedCalenderFrom.Visible = false;
                imgShippedCalenderTo.Visible = false;

                txtOrderStartDate.Text = string.Empty;
                txtOrderEndDate.Text = string.Empty;
                imgOrderCalenderFrom.Visible = false;
                imgOrderCalenderTo.Visible = false;
            }
            catch (Exception ex)
            {
                //this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        protected void rbShippedDateLastOneMonth_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                gdvShipandBill.DataSource = null;
                gdvShipandBill.DataBind();
                btnExport.Visible = false;
                txtShippedEndDate.Text = DateTime.Now.ToString("d");
                txtShippedStartDate.Text = DateTime.Now.AddMonths(-1).ToString("d");
                imgShippedCalenderFrom.Visible = false;
                imgShippedCalenderTo.Visible = false;

                txtOrderStartDate.Text = string.Empty;
                txtOrderEndDate.Text = string.Empty;
                imgOrderCalenderFrom.Visible = false;
                imgOrderCalenderTo.Visible = false;
            }
            catch (Exception ex)
            {
                //this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        protected void rbShippedDateRange_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                gdvShipandBill.DataSource = null;
                gdvShipandBill.DataBind();
                btnExport.Visible = false;
                imgShippedCalenderFrom.Visible = true;
                imgShippedCalenderTo.Visible = true;

                txtOrderStartDate.Text = string.Empty;
                txtOrderEndDate.Text = string.Empty;
                imgOrderCalenderFrom.Visible = false;
                imgOrderCalenderTo.Visible = false;
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
                lblError.Text = string.Empty;

                if (rbOrderDateRange.Checked) // (rblPeriodOrder.SelectedIndex == 2)
                {
                    if (string.IsNullOrEmpty(txtOrderStartDate.Text) || string.IsNullOrEmpty(txtOrderEndDate.Text))
                    {
                        lblError.Text = "Please provide Order Start & End date.";
                        return;
                    }
                    else
                    {
                        DateTime StartDate = Convert.ToDateTime(txtOrderStartDate.Text);
                        DateTime EndDate = Convert.ToDateTime(txtOrderEndDate.Text);
                        if (StartDate > EndDate)
                        {
                            lblError.Text = "Order Start date should not be greater than End date.";
                            return;
                        }
                    }
                }

                if (rbShippedDateRange.Checked) //(rblPeriodShipped.SelectedIndex == 2)
                {
                    if (string.IsNullOrEmpty(txtShippedStartDate.Text) || string.IsNullOrEmpty(txtOrderEndDate.Text))
                    {
                        lblError.Text = "Please provide Shipped Start & End date.";
                        return;
                    }
                    else
                    {
                        DateTime StartDate = Convert.ToDateTime(txtShippedStartDate.Text);
                        DateTime EndDate = Convert.ToDateTime(txtShippedEndDate.Text);
                        if (StartDate > EndDate)
                        {
                            lblError.Text = "Shipped Start date should not be greater than End date.";
                            return;
                        }
                    }
                }

                presenter.PopulateReport();
            }
            catch (Exception ex)
            {

            }
        }

        protected void lnkRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = string.Empty;
                this.presenter.OnViewInitialized();
                btnExport.Visible = false;

                ddlCustomerNameFilter.Enabled = true;
                ddlBranchAgencyFilter.Enabled = true;
                ddlManagerFilter.Enabled = true;
                ddlSalesRepresentativeFilter.Enabled = true;
                ddlStateFilter.Enabled = true;
                ddlOwnershipStructureFilter.Enabled = true;
                ddlManagementStructureFilter.Enabled = true;

                ddlCustomerNameFilter.SelectedIndex = 0;
                ddlBranchAgencyFilter.SelectedIndex = 0;
                ddlManagerFilter.SelectedIndex = 0;
                ddlSalesRepresentativeFilter.SelectedIndex = 0;
                ddlStateFilter.SelectedIndex = 0;
                ddlOwnershipStructureFilter.SelectedIndex = 0;
                ddlManagementStructureFilter.SelectedIndex = 0;

                //txtOrderEndDate.Text = DateTime.Now.ToString("d");
                //txtOrderStartDate.Text = DateTime.Now.AddDays(-6).ToString("d");
                //rbOrderDateLastOneWeek.Checked = true;
                //rblPeriodOrder.SelectedIndex = 0;

                //txtShippedEndDate.Text = DateTime.Now.ToString("d");
                //txtShippedStartDate.Text = DateTime.Now.AddDays(-6).ToString("d");
                //rblPeriodShipped.SelectedIndex = 0;

            }
            catch (Exception ex)
            {

            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                ExportInExcel();
            }
            catch (Exception ex)
            {

            }
        }

        protected void ddlProductLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if (ddlProductLine.SelectedIndex > 0)
                this.presenter.PopulateCategory();
            }
            catch (Exception ex)
            {

            }
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if (ddlCategory.SelectedIndex > 0)
                this.presenter.PopulateSubCategory1();
            }
            catch (Exception ex)
            {

            }
        }

        protected void ddlSubCategory1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if (ddlSubCategory1.SelectedIndex > 0)
                this.presenter.PopulateSubCategory2();
            }
            catch (Exception ex)
            {

            }
        }

        protected void ddlSubCategory2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if (ddlSubCategory2.SelectedIndex > 0)
                this.presenter.PopulateSubCategory3();
            }
            catch (Exception ex)
            {

            }
        }

        protected void ddlCustomerNameFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //If you select Customer Name, no other filters can be applied.
                if (ddlCustomerNameFilter.SelectedIndex > 0)
                {
                    ddlBranchAgencyFilter.Enabled = false;
                    ddlManagerFilter.Enabled = false;
                    ddlSalesRepresentativeFilter.Enabled = false;
                    ddlStateFilter.Enabled = false;
                    ddlOwnershipStructureFilter.Enabled = false;
                    ddlManagementStructureFilter.Enabled = false;

                    ddlBranchAgencyFilter.SelectedIndex = 0;
                    ddlManagerFilter.SelectedIndex = 0;
                    ddlSalesRepresentativeFilter.SelectedIndex = 0;
                    ddlStateFilter.SelectedIndex = 0;
                    ddlOwnershipStructureFilter.SelectedIndex = 0;
                    ddlManagementStructureFilter.SelectedIndex = 0;
                }
                else
                {
                    ddlBranchAgencyFilter.Enabled = true;
                    ddlManagerFilter.Enabled = true;
                    ddlSalesRepresentativeFilter.Enabled = true;
                    ddlStateFilter.Enabled = true;
                    ddlOwnershipStructureFilter.Enabled = true;
                    ddlManagementStructureFilter.Enabled = true;
                    presenter.FillDropdowns(string.Empty, string.Empty);
                }
            }
            catch { }
        }

        protected void ddlBranchAgencyFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //If you select Branch / Agency , you can add the following filters: 
                //Manager, Sales Representative, State, Ownership Structure, Management Structure.
                if (ddlBranchAgencyFilter.SelectedIndex > 0)
                {
                    presenter.FillDropdowns("BranchAgency", Convert.ToString(ddlBranchAgencyFilter.SelectedValue));
                }
                else
                {
                    presenter.FillDropdowns("BranchAgency", Convert.ToString(0));
                }
            }
            catch { }
        }

        protected void ddlManagerFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //If you select Manager, you can add the following filters:
                //Sales Representative, State, Ownership Structure, Management Structure.
                if (ddlManagerFilter.SelectedIndex > 0)
                {
                    presenter.FillDropdowns("Manager", Convert.ToString(ddlManagerFilter.SelectedValue));
                }
                else
                {
                    presenter.FillDropdowns("Manager", Convert.ToString(0));
                }
            }
            catch { }
        }

        protected void ddlSalesRepresentativeFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //If you select Sales Representative, you can add the following filters:
                //State, Ownership Structure, Management Structure.
                if (ddlSalesRepresentativeFilter.SelectedIndex > 0)
                {
                    presenter.FillDropdowns("SalesRepresentative", Convert.ToString(ddlSalesRepresentativeFilter.SelectedValue));
                }
                else
                {
                    presenter.FillDropdowns("SalesRepresentative", Convert.ToString(0));
                }
            }
            catch { }
        }

        protected void ddlStateFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //If you select State, you can add the following filters:
                //Branch, Manager, Sales Representative, Ownership Structure, Management Structure.
                if (ddlStateFilter.SelectedIndex > 0)
                {
                    presenter.FillDropdowns("State", Convert.ToString(ddlStateFilter.SelectedValue));
                }
                else
                {
                    presenter.FillDropdowns("State", Convert.ToString(0));
                }
            }
            catch { }
        }

        protected void ddlOwnershipStructureFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //If you select Ownership Structure, you can add the following filters:
                //Branch , Manager, Sales Representative, State, Management Structure.
                if (ddlOwnershipStructureFilter.SelectedIndex > 0)
                {
                    presenter.FillDropdowns("OwnershipStructure", Convert.ToString(ddlOwnershipStructureFilter.SelectedValue));
                }
                else
                {
                    presenter.FillDropdowns("OwnershipStructure", Convert.ToString(0));
                }
            }
            catch { }
        }

        protected void ddlManagementStructureFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //If you select Management Structure, you can add the following filters:
                //Branch / Agency, Manager, Sales Representative, State, Ownership Structure.
                if (ddlManagementStructureFilter.SelectedIndex > 0)
                {
                    presenter.FillDropdowns("ManagementStructure", Convert.ToString(ddlManagementStructureFilter.SelectedValue));
                }
                else
                {
                    presenter.FillDropdowns("ManagementStructure", Convert.ToString(0));
                }
            }
            catch { }
        }

        #endregion

        #region Private Methods

        private void LocalizePage()
        {
            try
            {
                string heading = string.Empty;
                heading = vctResource.GetString("mnueParPlusShipAndBillReport");
                lblHeader.Text = heading;
                Page.Title = heading;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void AuthorizedPage()
        {
            security = new Security();
            if (Session["LoggedInUser"] == null)
            {
                Response.Redirect("~/ErrorPage.aspx?ErrorKey=Common_msgSessionExpired");
            }
            else if (security.HasAccess("ePar+.eParPlusShipAndBillReport"))
            {
                CanAdjust = security.HasPermission("ePar+.eParPlusShipAndBillReport.Adjust");
            }
            else
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }

        private void SetFieldsBlank()
        {
            gdvShipandBill.DataSource = null;
            gdvShipandBill.DataBind();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        private void ExportInExcel()
        {
            grdViewExport.DataSource = null;
            grdViewExport.DataBind();
            lstOrderDetail = presenter.PopulateReport();

            if (lstOrderDetail != null && lstOrderDetail.Count > 0)
            {
                DataTable dtReportTable = new DataTable();
                dtReportTable.Columns.Add("Customer Name", typeof(string));
                dtReportTable.Columns.Add("Order #", typeof(string));
                dtReportTable.Columns.Add("Line #", typeof(string));
                dtReportTable.Columns.Add("Ref #", typeof(string));
                dtReportTable.Columns.Add("Ordered Qty", typeof(string));
                dtReportTable.Columns.Add("Shipped Qty", typeof(string));
                dtReportTable.Columns.Add("Cancelled Qty", typeof(string));
                dtReportTable.Columns.Add("Order Status", typeof(string));
                dtReportTable.Columns.Add("Order Date", typeof(string));
                dtReportTable.Columns.Add("Shipped Date", typeof(string));

                dtReportTable.Columns.Add("Received Qty", typeof(string));
                dtReportTable.Columns.Add("Adjusted Qty", typeof(string));
                dtReportTable.Columns.Add("Remaining Qty", typeof(string));

                //dtReportTable.Columns.Add("Disposition Type", typeof(string));
                //dtReportTable.Columns.Add("Remarks", typeof(string));
                //dtReportTable.Columns.Add("Adjusted By", typeof(string));
                //dtReportTable.Columns.Add("Adjusted On", typeof(string));


                var distinctOrderDetails = lstOrderDetail.Select(s => new
                {
                    CustomerName = s.CustomerName,
                    AccountNumber = s.AccountNumber,
                    OrderId = s.OrderId,
                    OrderNumber = s.OrderNumber,
                    LineNumber = s.LineNumber,
                    RefNum = s.RefNum,
                    OrderedQty = s.OrderedQty,
                    ShippedQty = s.ShippedQty,
                    CancelledQty = s.CancelledQty,
                    OrderStatus = s.OrderStatus,
                    OrderDate = s.OrderDate,
                    ShippedDate = s.ShippedDate,
                    ReceivedQty = s.ReceivedQty
                }).Distinct().ToList();


                foreach (var orderDetail in distinctOrderDetails)
                {
                    //int oldRemainingQty = 0;
                    //List<OrderDetail> lstOrderDetailReportToExport = lstOrderDetail.FindAll(x => x.OrderId == OrdDtl.OrderId);
                    //foreach (VCTWeb.Core.Domain.OrderDetail orderDetail in lstOrderDetailReportToExport)
                    //{

                    List<OrderDetail> lstOrderDetailReportToExport = lstOrderDetail.FindAll(x => x.OrderId == orderDetail.OrderId);

                    int adjustedQty = Convert.ToInt16(lstOrderDetailReportToExport.Sum(i => i.AdjustQty));
                    int RemainingQty = Convert.ToInt16(orderDetail.ShippedQty) - Convert.ToInt16(orderDetail.ReceivedQty) - Convert.ToInt16(adjustedQty);

                    DataRow dr;
                    dr = dtReportTable.NewRow();


                    dr["Customer Name"] = orderDetail.CustomerName != null ? orderDetail.CustomerName.ToString() : string.Empty;
                    dr["Order #"] = orderDetail.OrderNumber != null ? orderDetail.OrderNumber.ToString() : string.Empty;
                    dr["Line #"] = orderDetail.LineNumber != null ? orderDetail.LineNumber.ToString() : string.Empty;
                    dr["Ref #"] = orderDetail.RefNum != null ? orderDetail.RefNum.ToString() : string.Empty;
                    dr["Ordered Qty"] = orderDetail.OrderedQty.ToString();
                    dr["Shipped Qty"] = orderDetail.ShippedQty != null ? orderDetail.ShippedQty.ToString() : string.Empty;
                    dr["Cancelled Qty"] = orderDetail.CancelledQty != null ? orderDetail.CancelledQty.ToString() : string.Empty;
                    dr["Order Status"] = orderDetail.OrderStatus != null ? orderDetail.OrderStatus.ToString() : string.Empty;
                    dr["Order Date"] = orderDetail.OrderDate.ToString();
                    dr["Shipped Date"] = orderDetail.ShippedDate.ToString();

                    dr["Received Qty"] = orderDetail.ReceivedQty != null ? orderDetail.ReceivedQty.ToString() : string.Empty;
                    dr["Adjusted Qty"] = adjustedQty != null ? adjustedQty.ToString() : string.Empty;
                    dr["Remaining Qty"] = RemainingQty.ToString();



                    //int RemainingQty;
                    //if (oldRemainingQty == 0)
                    //    RemainingQty = Convert.ToInt16(orderDetail.ShippedQty) - Convert.ToInt16(orderDetail.ReceivedQty) - Convert.ToInt16(orderDetail.AdjustQty) - oldRemainingQty;
                    //else
                    //    RemainingQty = oldRemainingQty - Convert.ToInt16(orderDetail.AdjustQty);
                    //oldRemainingQty = RemainingQty;

                    //dr["Order Created By"] = orderDetail.CreatedBy != null ? orderDetail.CreatedBy.ToString() : string.Empty;
                    //dr["Disposition Type"] = orderDetail.DispositionType != null ? orderDetail.DispositionType.ToString() : string.Empty;
                    //dr["Adjusted Qty"] = orderDetail.AdjustQty != null ? orderDetail.AdjustQty.ToString() : string.Empty;
                    //dr["Remarks"] = orderDetail.Remarks != null ? orderDetail.Remarks.ToString() : string.Empty;
                    //dr["Adjusted By"] = orderDetail.UpdatedBy != null ? orderDetail.UpdatedBy.ToString() : string.Empty;
                    //dr["Adjusted On"] = (orderDetail.UpdatedOn.ToString().Trim() == "1/1/0001 12:00:00 AM" ? "" : orderDetail.UpdatedOn.ToString());

                    dtReportTable.Rows.Add(dr);
                    //}
                }


                grdViewExport.DataSource = dtReportTable;
                grdViewExport.DataBind();

                string filename = "Ship&BillReport_" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString()
                                    + "_" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".xls";

                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
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

        #endregion

        #region IeParPlusShipAndBillReport Members

        public List<OrderDetail> ListOrderDetail
        {
            set
            {
                lblError.Text = "";
                gdvShipandBill.DataSource = null;
                gdvShipandBill.DataBind();

                if (value == null) return;

                lstOrderDetail = value;

                if (lstOrderDetail != null && lstOrderDetail.Count > 0) btnExport.Visible = true;

                if (lstOrderDetail != null && lstOrderDetail.Count > 0)
                    btnExport.Visible = true;


                List<VCTWeb.Core.Domain.OrderDetail> lstOrderDetailSummary = new List<OrderDetail>();

                var distinctOrderDetails = lstOrderDetail.Select(s => new
                {
                    CustomerName = s.CustomerName,
                    AccountNumber = s.AccountNumber,
                    OrderId = s.OrderId,
                    OrderNumber = s.OrderNumber,
                    LineNumber = s.LineNumber,
                    RefNum = s.RefNum,
                    OrderedQty = s.OrderedQty,
                    ShippedQty = s.ShippedQty,
                    CancelledQty = s.CancelledQty,
                    OrderStatus = s.OrderStatus,
                    OrderDate = s.OrderDate,
                    ShippedDate = s.ShippedDate,
                    ReceivedQty = s.ReceivedQty
                }).Distinct().ToList();

                foreach (var item in distinctOrderDetails)
                {
                    int adjustedQty = 0;
                    int remainingQty = 0;
                    List<VCTWeb.Core.Domain.OrderDetail> lstTemp = lstOrderDetail.FindAll(i => i.OrderId == item.OrderId);
                    adjustedQty = Convert.ToInt16(lstTemp.Sum(i => i.AdjustQty));
                    remainingQty = Convert.ToInt16(item.ShippedQty) - Convert.ToInt16(item.ReceivedQty) - Convert.ToInt16(adjustedQty);
                    lstOrderDetailSummary.Add(new VCTWeb.Core.Domain.OrderDetail()
                    {
                        CustomerName = item.CustomerName,
                        AccountNumber = item.AccountNumber,
                        OrderId = item.OrderId,
                        OrderNumber = item.OrderNumber,
                        LineNumber = item.LineNumber,
                        RefNum = item.RefNum,
                        OrderedQty = item.OrderedQty,
                        ShippedQty = item.ShippedQty,
                        CancelledQty = item.CancelledQty,
                        OrderStatus = item.OrderStatus,
                        OrderDate = item.OrderDate,
                        ShippedDate = item.ShippedDate,
                        ReceivedQty = item.ReceivedQty,
                        AdjustQty = Convert.ToInt16(adjustedQty),
                        RemainingQty = Convert.ToInt16(remainingQty)
                    });
                }
                Session["ListOrderDetail"] = value;
                gdvShipandBill.DataSource = lstOrderDetailSummary;
                gdvShipandBill.DataBind();
                gdvShipandBill.Columns[15].Visible = CanAdjust;
                lblError.Text = (value.Count <= 0 ? "No Record Found." : "");
            }
        }

        public List<VCTWeb.Core.Domain.DispositionType> DispositionTypeList
        {
            set
            {
                ddlDispositionType.DataSource = value;
                ddlDispositionType.DataTextField = "Disposition";
                ddlDispositionType.DataValueField = "DispositionTypeId";
                ddlDispositionType.DataBind();

                ddlDispositionType.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
        }

        List<VCTWeb.Core.Domain.Customer> IeParPlusShipAndBillReport.CustomerNameList
        {
            set
            {
                this.ddlCustomerNameFilter.DataSource = null;
                this.ddlCustomerNameFilter.DataSource = value;
                this.ddlCustomerNameFilter.DataTextField = "NameAccount";
                this.ddlCustomerNameFilter.DataValueField = "AccountNumber";
                this.ddlCustomerNameFilter.DataBind();
                this.ddlCustomerNameFilter.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), "0"));
                ddlCustomerNameFilter.SelectedIndex = 0;
            }
        }

        public List<string> StateList
        {
            set
            {
                this.ddlStateFilter.DataSource = null;
                this.ddlStateFilter.DataSource = value;
                this.ddlStateFilter.DataBind();
                this.ddlStateFilter.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), "0"));
                ddlStateFilter.SelectedIndex = 0;
            }
        }

        public List<string> OwnershipStructureList
        {
            set
            {
                this.ddlOwnershipStructureFilter.DataSource = null;
                this.ddlOwnershipStructureFilter.DataSource = value;
                this.ddlOwnershipStructureFilter.DataBind();
                this.ddlOwnershipStructureFilter.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), "0"));
                ddlOwnershipStructureFilter.SelectedIndex = 0;
            }
        }

        public List<string> ManagementStructureList
        {
            set
            {
                this.ddlManagementStructureFilter.DataSource = null;
                this.ddlManagementStructureFilter.DataSource = value;
                this.ddlManagementStructureFilter.DataBind();
                this.ddlManagementStructureFilter.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), "0"));
                ddlManagementStructureFilter.SelectedIndex = 0;
            }
        }

        public List<string> BranchAgencyList
        {
            set
            {
                this.ddlBranchAgencyFilter.DataSource = null;
                this.ddlBranchAgencyFilter.DataSource = value;
                this.ddlBranchAgencyFilter.DataBind();
                this.ddlBranchAgencyFilter.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), "0"));
                ddlBranchAgencyFilter.SelectedIndex = 0;
            }
        }

        public List<ProductLine> ProductLineList
        {
            set
            {
                this.ddlProductLine.DataSource = null;
                this.ddlProductLine.DataSource = value;
                this.ddlProductLine.DataTextField = "ProductLineName";
                this.ddlProductLine.DataValueField = "ProductLineName";
                this.ddlProductLine.DataBind();
                this.ddlProductLine.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), "0"));
                ddlProductLine.SelectedIndex = 0;
            }
        }

        public List<string> CategoryList
        {
            set
            {
                this.ddlCategory.DataSource = null;
                this.ddlCategory.DataSource = value;
                this.ddlCategory.DataBind();
                this.ddlCategory.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), "0"));
                ddlCategory.SelectedIndex = 0;
            }
        }

        public List<string> SubCategory1List
        {
            set
            {
                this.ddlSubCategory1.DataSource = null;
                this.ddlSubCategory1.DataSource = value;
                this.ddlSubCategory1.DataBind();
                this.ddlSubCategory1.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), "0"));
                ddlSubCategory1.SelectedIndex = 0;
            }
        }

        public List<string> SubCategory2List
        {
            set
            {
                this.ddlSubCategory2.DataSource = null;
                this.ddlSubCategory2.DataSource = value;
                this.ddlSubCategory2.DataBind();
                this.ddlSubCategory2.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), "0"));
                ddlSubCategory2.SelectedIndex = 0;
            }
        }

        public List<string> SubCategory3List
        {
            set
            {
                this.ddlSubCategory3.DataSource = null;
                this.ddlSubCategory3.DataSource = value;
                this.ddlSubCategory3.DataBind();
                this.ddlSubCategory3.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), "0"));
                ddlSubCategory3.SelectedIndex = 0;
            }
        }

        public List<string> SalesRepresentativeList
        {
            set
            {
                ddlSalesRepresentativeFilter.SelectedValue = null;
                this.ddlSalesRepresentativeFilter.DataSource = null;
                this.ddlSalesRepresentativeFilter.DataSource = value;
                this.ddlSalesRepresentativeFilter.DataBind();
                this.ddlSalesRepresentativeFilter.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), "0"));
                ddlSalesRepresentativeFilter.SelectedIndex = 0;
            }
        }

        public List<string> ManagerList
        {
            set
            {
                this.ddlManagerFilter.DataSource = null;
                this.ddlManagerFilter.DataSource = value;
                this.ddlManagerFilter.DataBind();
                this.ddlManagerFilter.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), "0"));
                ddlManagerFilter.SelectedIndex = 0;
            }
        }

        public string SelectedCustomerAccountFilter
        {
            get
            {
                if (this.ddlCustomerNameFilter.SelectedIndex > 0)
                    return Convert.ToString(this.ddlCustomerNameFilter.SelectedValue);
                else
                    return string.Empty;
            }
            set
            {
                try
                {
                    this.ddlCustomerNameFilter.SelectedValue = value;
                }
                catch { }
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
            set
            {
                try
                {
                    this.ddlBranchAgencyFilter.Text = value;
                }
                catch { }
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
            set
            {
                try
                {
                    this.ddlManagerFilter.Text = value;
                }
                catch { }
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
            set
            {
                try
                {
                    this.ddlSalesRepresentativeFilter.Text = value;
                }
                catch { }
            }
        }

        public string SelectedStateFilter
        {
            get
            {
                if (this.ddlStateFilter.SelectedIndex > 0)
                    return Convert.ToString(this.ddlStateFilter.SelectedValue);
                else
                    return string.Empty;
            }
            set
            {
                try
                {
                    this.ddlStateFilter.Text = value;
                }
                catch { }
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
            set
            {
                try
                {
                    this.ddlOwnershipStructureFilter.Text = value;
                }
                catch { }
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
            set
            {
                try
                {
                    this.ddlManagementStructureFilter.Text = value;
                }
                catch { }
            }
        }

        public string SelectedProductLineFilter
        {
            get
            {
                if (this.ddlProductLine.SelectedIndex > 0)
                    return Convert.ToString(this.ddlProductLine.SelectedValue);
                else
                    return string.Empty;
            }
        }

        public string SelectedCategoryFilter
        {
            get
            {
                if (this.ddlCategory.SelectedIndex > 0)
                    return Convert.ToString(this.ddlCategory.SelectedValue);
                else
                    return string.Empty;
            }
        }

        public string SelectedSubCategory1Filter
        {
            get
            {
                if (this.ddlSubCategory1.SelectedIndex > 0)
                    return Convert.ToString(this.ddlSubCategory1.SelectedValue);
                else
                    return string.Empty;
            }
        }

        public string SelectedSubCategory2Filter
        {
            get
            {
                if (this.ddlSubCategory2.SelectedIndex > 0)
                    return Convert.ToString(this.ddlSubCategory2.SelectedValue);
                else
                    return string.Empty;
            }
        }

        public string SelectedSubCategory3Filter
        {
            get
            {
                if (this.ddlSubCategory3.SelectedIndex > 0)
                    return Convert.ToString(this.ddlSubCategory3.SelectedValue);
                else
                    return string.Empty;
            }
        }

        public DateTime? SelectedOrderStartDate
        {
            get
            {
                if (string.IsNullOrEmpty(txtOrderStartDate.Text))
                    return null;
                else
                    return Convert.ToDateTime(txtOrderStartDate.Text);
            }
        }

        public DateTime? SelectedOrderEndDate
        {
            get
            {
                if (string.IsNullOrEmpty(txtOrderEndDate.Text))
                    return null;
                else
                    return Convert.ToDateTime(txtOrderEndDate.Text);
            }
        }

        public DateTime? SelectedShippedStartDate
        {
            get
            {
                if (string.IsNullOrEmpty(txtShippedStartDate.Text))
                    return null;
                else
                    return Convert.ToDateTime(txtShippedStartDate.Text);
            }
        }

        public DateTime? SelectedShippedEndDate
        {
            get
            {
                if (string.IsNullOrEmpty(txtShippedEndDate.Text))
                    return null;
                else
                    return Convert.ToDateTime(txtShippedEndDate.Text);
            }
        }

        public int DispositionTypeId
        {
            get
            {
                return Convert.ToInt32(hdnDispositionTypeId.Value);
            }
        }

        public string Remarks
        {
            get
            {
                return (hdnRemarks.Value);
            }
        }

        public int Qty
        {
            get
            {
                return Convert.ToInt32(hndQty.Value);
            }
        }

        #endregion
    }
}

