using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using VCTWeb.Core.Domain;
using System.Data;
using System.IO;
using Microsoft.Practices.ObjectBuilder;
using VCTWebApp.Resources;
using VCTWebApp.Shell.Views;
using VCTWebApp.Web;
using System.Data.SqlClient;

namespace VCTWebApp
{
    public partial class EParPlusConsumptionRateReport : Microsoft.Practices.CompositeWeb.Web.UI.Page, IeParPlusConsumptionRateReport
    {
        #region Instance Variables
        List<ConsumptionRate> _lstConsumptionRate = new List<ConsumptionRate>();
        private eParPlusConsumptionRateReportPresenter _presenter;
        private readonly VCTWebAppResource _vctResource = new VCTWebAppResource();
        private Security _security;

        #endregion

        #region Properties

        public SortDirection Direction
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
                if (!IsPostBack)
                {
                    AuthorizedPage();
                    _presenter.OnViewInitialized();
                    txtEndDate.Text = DateTime.Now.ToString("d");
                    txtStartDate.Text = DateTime.Now.AddDays(-6).ToString("d");
                    btnExport.Visible = false;
                }
            }
            catch (SqlException)
            {
                //lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), lblHeader.Text);
            }
            catch (Exception ex)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, _vctResource.GetString(ex.Message), lblHeader.Text);
            }
        }

        #endregion

        #region Create New Presenter

        [CreateNew]
        public eParPlusConsumptionRateReportPresenter Presenter
        {
            get
            {
                return _presenter;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                _presenter = value;
                _presenter.View = this;
            }
        }

        #endregion

        #region Event Handlers

        protected void gdvConsumptionRate_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            var grdChild = e.Row.FindControl("grdChild") as GridView;
            var objConsumptionRate = (ConsumptionRate)e.Row.DataItem;
            if (grdChild == null) return;
            grdChild.DataSource = _lstConsumptionRate.Where(w => w.CustomerName == objConsumptionRate.CustomerName && w.AccountNumber == objConsumptionRate.AccountNumber).ToList();
            grdChild.DataBind();
        }

        protected void gdvConsumptionRate_Sorting(object sender, GridViewSortEventArgs e)
        {
            var sortImage = new Image();
            string sortingDirection;
            if (Direction == SortDirection.Ascending)
            {
                Direction = SortDirection.Descending;
                sortingDirection = "Desc";
                sortImage.ImageUrl = "./Images/sort_down.png";
            }
            else
            {
                Direction = SortDirection.Ascending;
                sortingDirection = "Asc";
                sortImage.ImageUrl = "./Images/sort_up.png";
            }

            if (Session["ListConsumptionRate"] != null)
            {
                var listManualConsumption = Session["ListConsumptionRate"] as List<ConsumptionRate>;

                if (sortingDirection == "Desc")
                {
                    if (listManualConsumption != null)
                    {
                        if (e.SortExpression.Trim() == "CustomerName")
                            listManualConsumption =
                                listManualConsumption.OrderByDescending(p => p.AccountNumber).ToList();
                        if (e.SortExpression.Trim() == "AccountNumber")
                            listManualConsumption =
                                listManualConsumption.OrderByDescending(p => p.AccountNumber).ToList();
                        if (e.SortExpression.Trim() == "ConsumedQty")
                            listManualConsumption = listManualConsumption.OrderByDescending(p => p.ConsumedQty).ToList();
                        if (e.SortExpression.Trim() == "NoOfDays")
                            listManualConsumption = listManualConsumption.OrderByDescending(p => p.NoOfDays).ToList();
                        if (e.SortExpression.Trim() == "ConsumptionRatePercent")
                            listManualConsumption =
                                listManualConsumption.OrderByDescending(p => p.ConsumptionRatePercent).ToList();
                    }
                }
                else
                {
                    if (listManualConsumption != null)
                    {
                        if (e.SortExpression.Trim() == "CustomerName")
                            listManualConsumption = listManualConsumption.OrderBy(p => p.AccountNumber).ToList();
                        if (e.SortExpression.Trim() == "AccountNumber")
                            listManualConsumption = listManualConsumption.OrderBy(p => p.AccountNumber).ToList();
                        if (e.SortExpression.Trim() == "ConsumedQty")
                            listManualConsumption = listManualConsumption.OrderBy(p => p.ConsumedQty).ToList();
                        if (e.SortExpression.Trim() == "NoOfDays")
                            listManualConsumption = listManualConsumption.OrderBy(p => p.NoOfDays).ToList();
                        if (e.SortExpression.Trim() == "ConsumptionRatePercent")
                            listManualConsumption =
                                listManualConsumption.OrderBy(p => p.ConsumptionRatePercent).ToList();
                    }
                }

                ListConsumptionRate = listManualConsumption;

                int columnIndex = 0;
                foreach (DataControlFieldHeaderCell headerCell in gdvConsumptionRate.HeaderRow.Cells)
                {
                    if (headerCell.ContainingField.SortExpression == e.SortExpression)
                    {
                        columnIndex = gdvConsumptionRate.HeaderRow.Cells.GetCellIndex(headerCell);
                    }
                }
                gdvConsumptionRate.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);
            }
        }

        protected void grdChild_Sorting(object sender, GridViewSortEventArgs e)
        {
            var gdvChild = (GridView)sender;

            var sortImage = new Image();
            string sortingDirection;

            if (Direction == SortDirection.Ascending)
            {
                Direction = SortDirection.Descending;
                sortingDirection = "Desc";
                sortImage.ImageUrl = "./Images/sort_down.png";
            }
            else
            {
                Direction = SortDirection.Ascending;
                sortingDirection = "Asc";
                sortImage.ImageUrl = "./Images/sort_up.png";
            }

            var accountNumber = string.Empty;
            var customerName = string.Empty;

            foreach (GridViewRow gvr in gdvChild.Rows)
            {
                var lblAccountNumber = gvr.FindControl("lblAccountNumber") as Label;
                var lblCustomerName = gvr.FindControl("lblCustomerName") as Label;
                if (lblAccountNumber == null || lblCustomerName == null) continue;
                accountNumber = lblAccountNumber.Text;
                customerName = lblCustomerName.Text;
                break;
            }

            if (accountNumber != string.Empty && customerName != string.Empty)
            {
                var listListConsumptionRate = Session["ListConsumptionRate"] as List<ConsumptionRate>;

                if (listListConsumptionRate != null)
                {
                    var listListConsumptionRateChild = listListConsumptionRate.FindAll(i => i.AccountNumber == accountNumber);
                    if (sortingDirection == "Desc")
                    {
                        if (e.SortExpression.Trim() == "RefNum")
                            listListConsumptionRateChild = listListConsumptionRateChild.OrderByDescending(p => p.RefNum).ToList();

                        if (e.SortExpression.Trim() == "PartDesc")
                            listListConsumptionRateChild = listListConsumptionRateChild.OrderByDescending(p => p.PartDesc).ToList();

                        if (e.SortExpression.Trim() == "ConsumedQty")
                            listListConsumptionRateChild = listListConsumptionRateChild.OrderByDescending(p => p.ConsumedQty).ToList();

                        if (e.SortExpression.Trim() == "NoOfDays")
                            listListConsumptionRateChild = listListConsumptionRateChild.OrderByDescending(p => p.NoOfDays).ToList();

                        if (e.SortExpression.Trim() == "ConsumptionRatePercent")
                            listListConsumptionRateChild = listListConsumptionRateChild.OrderByDescending(p => p.ConsumptionRatePercent).ToList();
                    }
                    else
                    {
                        if (e.SortExpression.Trim() == "RefNum")
                            listListConsumptionRateChild = listListConsumptionRateChild.OrderBy(p => p.RefNum).ToList();

                        if (e.SortExpression.Trim() == "PartDesc")
                            listListConsumptionRateChild = listListConsumptionRateChild.OrderBy(p => p.PartDesc).ToList();

                        if (e.SortExpression.Trim() == "ConsumedQty")
                            listListConsumptionRateChild = listListConsumptionRateChild.OrderBy(p => p.ConsumedQty).ToList();

                        if (e.SortExpression.Trim() == "NoOfDays")
                            listListConsumptionRateChild = listListConsumptionRateChild.OrderBy(p => p.NoOfDays).ToList();

                        if (e.SortExpression.Trim() == "ConsumptionRatePercent")
                            listListConsumptionRateChild = listListConsumptionRateChild.OrderBy(p => p.ConsumptionRatePercent).ToList();
                    }

                    gdvChild.DataSource = listListConsumptionRateChild;
                }
                gdvChild.DataBind();

                foreach (DataControlFieldHeaderCell headerCell in gdvChild.HeaderRow.Cells)
                {
                    if (headerCell.ContainingField.SortExpression != e.SortExpression) continue;
                    var columnIndex = gdvChild.HeaderRow.Cells.GetCellIndex(headerCell);
                    gdvChild.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);
                }
            }
        }

        protected void rblPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                tblTotalBar.Visible = false;
                gdvConsumptionRate.DataSource = null;
                gdvConsumptionRate.DataBind();
                btnExport.Visible = false;
                if (rblPeriod.SelectedIndex == 0)
                {
                    txtEndDate.Text = DateTime.Now.ToString("d");
                    txtStartDate.Text = DateTime.Now.AddDays(-6).ToString("d");
                    imgCalenderFrom.Visible = false;
                    Image1.Visible = false;
                }

                else if (rblPeriod.SelectedIndex == 1)
                {
                    txtEndDate.Text = DateTime.Now.ToString("d");
                    txtStartDate.Text = DateTime.Now.AddMonths(-1).ToString("d");
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
                lblError.Text = string.Format(CultureInfo.InvariantCulture, _vctResource.GetString(ex.Message), lblHeader.Text);
            }
        }

        protected void lnkFilterCustomerListData_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = string.Empty;
                if (rblPeriod.SelectedIndex == 2)
                {
                    var startDate = Convert.ToDateTime(txtStartDate.Text);
                    var endDate = Convert.ToDateTime(txtEndDate.Text);
                    if (startDate > endDate)
                        lblError.Text = "Start date should not be greater than End date.";
                    else
                        _presenter.PopulateReport();
                }
                else
                    _presenter.PopulateReport();
            }
            catch (Exception ex)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, _vctResource.GetString(ex.Message), lblHeader.Text);
            }
        }

        protected void lnkRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = string.Empty;
                _presenter.OnViewInitialized();
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

                txtEndDate.Text = DateTime.Now.ToString("d");
                txtStartDate.Text = DateTime.Now.AddDays(-6).ToString("d");
                rblPeriod.SelectedIndex = 0;
                imgCalenderFrom.Visible = false;
                Image1.Visible = false;

            }
            catch (Exception ex)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, _vctResource.GetString(ex.Message), lblHeader.Text);
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
                lblError.Text = string.Format(CultureInfo.InvariantCulture, _vctResource.GetString(ex.Message), lblHeader.Text);
            }
        }

        protected void ddlProductLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if (ddlProductLine.SelectedIndex > 0)
                _presenter.PopulateCategory();
            }
            catch (Exception ex)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, _vctResource.GetString(ex.Message), lblHeader.Text);
            }
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if (ddlCategory.SelectedIndex > 0)
                _presenter.PopulateSubCategory1();
            }
            catch (Exception ex)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, _vctResource.GetString(ex.Message), lblHeader.Text);
            }
        }

        protected void ddlSubCategory1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if (ddlSubCategory1.SelectedIndex > 0)
                _presenter.PopulateSubCategory2();
            }
            catch (Exception ex)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, _vctResource.GetString(ex.Message), lblHeader.Text);
            }
        }

        protected void ddlSubCategory2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if (ddlSubCategory2.SelectedIndex > 0)
                _presenter.PopulateSubCategory3();
            }
            catch (Exception ex)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, _vctResource.GetString(ex.Message), lblHeader.Text);
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
                    _presenter.FillDropdowns(string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, _vctResource.GetString(ex.Message), lblHeader.Text);
            }
        }


        protected void ddlBranchAgencyFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //If you select Branch / Agency , you can add the following filters: 
                //Manager, Sales Representative, State, Ownership Structure, Management Structure.
                _presenter.FillDropdowns("BranchAgency",
                    ddlBranchAgencyFilter.SelectedIndex > 0
                        ? Convert.ToString(ddlBranchAgencyFilter.SelectedValue)
                        : Convert.ToString(0));
            }
            catch (Exception ex)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, _vctResource.GetString(ex.Message), lblHeader.Text);
            }
        }

        protected void ddlManagerFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //If you select Manager, you can add the following filters:
                //Sales Representative, State, Ownership Structure, Management Structure.
                _presenter.FillDropdowns("Manager",
                    ddlManagerFilter.SelectedIndex > 0
                        ? Convert.ToString(ddlManagerFilter.SelectedValue)
                        : Convert.ToString(0));
            }
            catch (Exception ex)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, _vctResource.GetString(ex.Message), lblHeader.Text);
            }
        }

        protected void ddlSalesRepresentativeFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //If you select Sales Representative, you can add the following filters:
                //State, Ownership Structure, Management Structure.
                _presenter.FillDropdowns("SalesRepresentative",
                    ddlSalesRepresentativeFilter.SelectedIndex > 0
                        ? Convert.ToString(ddlSalesRepresentativeFilter.SelectedValue)
                        : Convert.ToString(0));
            }
            catch (Exception ex)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, _vctResource.GetString(ex.Message), lblHeader.Text);
            }
        }

        protected void ddlStateFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //If you select State, you can add the following filters:
                //Branch, Manager, Sales Representative, Ownership Structure, Management Structure.
                _presenter.FillDropdowns("State",
                    ddlStateFilter.SelectedIndex > 0
                        ? Convert.ToString(ddlStateFilter.SelectedValue)
                        : Convert.ToString(0));
            }
            catch (Exception ex)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, _vctResource.GetString(ex.Message), lblHeader.Text);
            }
        }

        protected void ddlOwnershipStructureFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //If you select Ownership Structure, you can add the following filters:
                //Branch , Manager, Sales Representative, State, Management Structure.
                _presenter.FillDropdowns("OwnershipStructure",
                    ddlOwnershipStructureFilter.SelectedIndex > 0
                        ? Convert.ToString(ddlOwnershipStructureFilter.SelectedValue)
                        : Convert.ToString(0));
            }
            catch (Exception ex)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, _vctResource.GetString(ex.Message), lblHeader.Text);
            }
        }

        protected void ddlManagementStructureFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //If you select Management Structure, you can add the following filters:
                //Branch / Agency, Manager, Sales Representative, State, Ownership Structure.
                _presenter.FillDropdowns("ManagementStructure",
                    ddlManagementStructureFilter.SelectedIndex > 0
                        ? Convert.ToString(ddlManagementStructureFilter.SelectedValue)
                        : Convert.ToString(0));
            }
            catch (Exception ex)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, _vctResource.GetString(ex.Message), lblHeader.Text);
            }
        }

        #endregion

        #region Private Methods

        private void AuthorizedPage()
        {
            _security = new Security();
            if (Session["LoggedInUser"] == null)
            {
                Response.Redirect("~/ErrorPage.aspx?ErrorKey=Common_msgSessionExpired");
            }
            else if (_security.HasAccess("ePar+.ConsumptionRateReport"))
            {
                //CanCancel = security.HasPermission("InventoryStockParts");
            }
            else
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        private void ExportInExcel()
        {
            grdViewExport.DataSource = null;
            grdViewExport.DataBind();
            var lstInventoryReportToExport = _presenter.PopulateReport();
            if (lstInventoryReportToExport == null || lstInventoryReportToExport.Count <= 0) return;
            var dtReportTable = new DataTable();
            dtReportTable.Columns.Add("Customer Name", typeof(string));
            dtReportTable.Columns.Add("Account Number", typeof(string));
            dtReportTable.Columns.Add("Ref #", typeof(string));
            dtReportTable.Columns.Add("Description", typeof(string));
            dtReportTable.Columns.Add("Consumed Qty", typeof(string));
            dtReportTable.Columns.Add("No. of Days", typeof(string));
            dtReportTable.Columns.Add("Cons. Rate %", typeof(string));
            foreach (var inventoryAmount in lstInventoryReportToExport)
            {
                DataRow dr = dtReportTable.NewRow();
                dr["Customer Name"] = inventoryAmount.CustomerName ?? string.Empty;
                dr["Account Number"] = inventoryAmount.AccountNumber ?? string.Empty;
                dr["Ref #"] = inventoryAmount.RefNum ?? string.Empty;
                dr["Description"] = inventoryAmount.PartDesc ?? string.Empty;
                dr["Consumed Qty"] = inventoryAmount.ConsumedQty.ToString(CultureInfo.InvariantCulture);
                dr["No. Of Days"] = inventoryAmount.NoOfDays.ToString(CultureInfo.InvariantCulture);
                dr["Cons. Rate %"] = inventoryAmount.ConsumptionRatePercent.ToString(CultureInfo.InvariantCulture);
                dtReportTable.Rows.Add(dr);
            }
            grdViewExport.DataSource = dtReportTable;
            grdViewExport.DataBind();
            string filename = "ConsumptionRate_" + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Year
                              + "_" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".xls";
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Charset = "";
            Response.ContentType = "application/vnd.xls";
            var stringWriter = new StringWriter();
            var htmlTextWriter = new HtmlTextWriter(stringWriter);
            grdViewExport.RenderControl(htmlTextWriter);
            Response.Write(stringWriter.ToString());
            grdViewExport.DataSource = null;
            grdViewExport.DataBind();
            Response.End();
        }

        #endregion

        #region IeParPlusConsumptionRateReport Members

        public List<ConsumptionRate> ListConsumptionRate
        {
            set
            {
                lblError.Text = "";

                tblTotalBar.Visible = false;

                gdvConsumptionRate.DataSource = null;
                gdvConsumptionRate.DataBind();
                var lstConsumptionRateSummary = new List<ConsumptionRate>();
                _lstConsumptionRate = value;

                if (_lstConsumptionRate != null && _lstConsumptionRate.Count > 0)
                {
                    btnExport.Visible = true;
                    tblTotalBar.Visible = true;
                }

                if (value != null)
                {
                    int noOfDays = 0;
                    if (_lstConsumptionRate != null && _lstConsumptionRate.Count > 0)
                        noOfDays = _lstConsumptionRate[0].NoOfDays;

                    if (_lstConsumptionRate != null)
                    {
                        var distinctCustomer = _lstConsumptionRate.Select(s => new { s.CustomerName, s.AccountNumber }).Distinct().ToList();

                        lstConsumptionRateSummary.AddRange(distinctCustomer.Select(customer => new ConsumptionRate(customer.CustomerName, customer.AccountNumber, _lstConsumptionRate.Where(w => w.CustomerName == customer.CustomerName && w.AccountNumber == customer.AccountNumber).Sum(s => s.ConsumedQty), noOfDays)));
                        //foreach (var customer in distinctCustomer)
                        //    lstConsumptionRateSummary.Add(new ConsumptionRate(customer.CustomerName, customer.AccountNumber, _lstConsumptionRate.Where(w => w.CustomerName == customer.CustomerName && w.AccountNumber == customer.AccountNumber).Sum(s => s.ConsumedQty), noOfDays));
                    }

                    Session["ListConsumptionRate"] = value;

                    gdvConsumptionRate.DataSource = lstConsumptionRateSummary;
                    gdvConsumptionRate.DataBind();
                    if (_lstConsumptionRate != null)
                    {
                        var consumedQty = _lstConsumptionRate.Sum(s => s.ConsumedQty);
                        var consumptionRate = noOfDays > 0 ? (Convert.ToDecimal(consumedQty) / Convert.ToDecimal(noOfDays)) : Convert.ToDecimal(0.0);
                        lblConsumedQtyTotal.Text = consumedQty.ToString(CultureInfo.InvariantCulture);
                        lblNoOfDays.Text = noOfDays.ToString(CultureInfo.InvariantCulture);
                        lblConsumptionRateTotal.Text = String.Format("{0:0.##}", consumptionRate);
                    }

                    lblError.Text = (value.Count <= 0 ? "No Record Found." : "");
                }
            }
        }

        List<VCTWeb.Core.Domain.Customer> IeParPlusConsumptionRateReport.CustomerNameList
        {
            set
            {
                ddlCustomerNameFilter.DataSource = null;
                ddlCustomerNameFilter.DataSource = value;
                ddlCustomerNameFilter.DataTextField = "NameAccount";
                ddlCustomerNameFilter.DataValueField = "AccountNumber";
                ddlCustomerNameFilter.DataBind();
                ddlCustomerNameFilter.Items.Insert(0, new ListItem(_vctResource.GetString("listItemAll"), "0"));
                ddlCustomerNameFilter.SelectedIndex = 0;
            }
        }

        public List<string> StateList
        {
            set
            {
                ddlStateFilter.DataSource = null;
                ddlStateFilter.DataSource = value;
                ddlStateFilter.DataBind();
                ddlStateFilter.Items.Insert(0, new ListItem(_vctResource.GetString("listItemAll"), "0"));
                ddlStateFilter.SelectedIndex = 0;
            }
        }

        public List<string> OwnershipStructureList
        {
            set
            {
                ddlOwnershipStructureFilter.DataSource = null;
                ddlOwnershipStructureFilter.DataSource = value;
                ddlOwnershipStructureFilter.DataBind();
                ddlOwnershipStructureFilter.Items.Insert(0, new ListItem(_vctResource.GetString("listItemAll"), "0"));
                ddlOwnershipStructureFilter.SelectedIndex = 0;
            }
        }

        public List<string> ManagementStructureList
        {
            set
            {
                ddlManagementStructureFilter.DataSource = null;
                ddlManagementStructureFilter.DataSource = value;
                ddlManagementStructureFilter.DataBind();
                ddlManagementStructureFilter.Items.Insert(0, new ListItem(_vctResource.GetString("listItemAll"), "0"));
                ddlManagementStructureFilter.SelectedIndex = 0;
            }
        }

        public List<string> BranchAgencyList
        {
            set
            {
                ddlBranchAgencyFilter.DataSource = null;
                ddlBranchAgencyFilter.DataSource = value;
                ddlBranchAgencyFilter.DataBind();
                ddlBranchAgencyFilter.Items.Insert(0, new ListItem(_vctResource.GetString("listItemAll"), "0"));
                ddlBranchAgencyFilter.SelectedIndex = 0;
            }
        }

        public List<ProductLine> ProductLineList
        {
            set
            {
                ddlProductLine.DataSource = null;
                ddlProductLine.DataSource = value;
                ddlProductLine.DataTextField = "ProductLineName";
                ddlProductLine.DataValueField = "ProductLineName";
                ddlProductLine.DataBind();
                ddlProductLine.Items.Insert(0, new ListItem(_vctResource.GetString("listItemAll"), "0"));
                ddlProductLine.SelectedIndex = 0;
            }
        }

        public List<string> CategoryList
        {
            set
            {
                ddlCategory.DataSource = null;
                ddlCategory.DataSource = value;
                ddlCategory.DataBind();
                ddlCategory.Items.Insert(0, new ListItem(_vctResource.GetString("listItemAll"), "0"));
                ddlCategory.SelectedIndex = 0;
            }
        }

        public List<string> SubCategory1List
        {
            set
            {
                ddlSubCategory1.DataSource = null;
                ddlSubCategory1.DataSource = value;
                ddlSubCategory1.DataBind();
                ddlSubCategory1.Items.Insert(0, new ListItem(_vctResource.GetString("listItemAll"), "0"));
                ddlSubCategory1.SelectedIndex = 0;
            }
        }

        public List<string> SubCategory2List
        {
            set
            {
                ddlSubCategory2.DataSource = null;
                ddlSubCategory2.DataSource = value;
                ddlSubCategory2.DataBind();
                ddlSubCategory2.Items.Insert(0, new ListItem(_vctResource.GetString("listItemAll"), "0"));
                ddlSubCategory2.SelectedIndex = 0;
            }
        }

        public List<string> SubCategory3List
        {
            set
            {
                ddlSubCategory3.DataSource = null;
                ddlSubCategory3.DataSource = value;
                ddlSubCategory3.DataBind();
                ddlSubCategory3.Items.Insert(0, new ListItem(_vctResource.GetString("listItemAll"), "0"));
                ddlSubCategory3.SelectedIndex = 0;
            }
        }

        public List<string> SalesRepresentativeList
        {
            set
            {
                ddlSalesRepresentativeFilter.SelectedValue = null;
                ddlSalesRepresentativeFilter.DataSource = null;
                ddlSalesRepresentativeFilter.DataSource = value;
                ddlSalesRepresentativeFilter.DataBind();
                ddlSalesRepresentativeFilter.Items.Insert(0, new ListItem(_vctResource.GetString("listItemAll"), "0"));
                ddlSalesRepresentativeFilter.SelectedIndex = 0;
            }
        }

        public List<string> ManagerList
        {
            set
            {
                ddlManagerFilter.DataSource = null;
                ddlManagerFilter.DataSource = value;
                ddlManagerFilter.DataBind();
                ddlManagerFilter.Items.Insert(0, new ListItem(_vctResource.GetString("listItemAll"), "0"));
                ddlManagerFilter.SelectedIndex = 0;
            }
        }

        public string SelectedCustomerAccountFilter
        {
            get
            {
                return ddlCustomerNameFilter.SelectedIndex > 0 ? Convert.ToString(ddlCustomerNameFilter.SelectedValue) : string.Empty;
            }
            set
            {
                ddlCustomerNameFilter.SelectedValue = value;
            }
        }

        public string SelectedBranchAgencyFilter
        {
            get
            {
                return ddlBranchAgencyFilter.SelectedIndex > 0 ? Convert.ToString(ddlBranchAgencyFilter.SelectedValue) : string.Empty;
            }
            set
            {
                ddlBranchAgencyFilter.Text = value;
            }
        }

        public string SelectedManagerFilter
        {
            get
            {
                return ddlManagerFilter.SelectedIndex > 0 ? Convert.ToString(ddlManagerFilter.SelectedValue) : string.Empty;
            }
            set
            {
                ddlManagerFilter.Text = value;
            }
        }

        public string SelectedSalesRepresentativeFilter
        {
            get
            {
                return ddlSalesRepresentativeFilter.SelectedIndex > 0 ? Convert.ToString(ddlSalesRepresentativeFilter.SelectedValue) : string.Empty;
            }
            set
            {
                ddlSalesRepresentativeFilter.Text = value;
            }
        }

        public string SelectedStateFilter
        {
            get
            {
                return ddlStateFilter.SelectedIndex > 0 ? Convert.ToString(ddlStateFilter.SelectedValue) : string.Empty;
            }
            set
            {
                ddlStateFilter.Text = value;
            }
        }

        public string SelectedOwnershipStructureFilter
        {
            get
            {
                return ddlOwnershipStructureFilter.SelectedIndex > 0 ? Convert.ToString(ddlOwnershipStructureFilter.SelectedValue) : string.Empty;
            }
            set
            {
                ddlOwnershipStructureFilter.Text = value;
            }
        }

        public string SelectedManagementStructureFilter
        {
            get
            {
                return ddlManagementStructureFilter.SelectedIndex > 0 ? Convert.ToString(ddlManagementStructureFilter.SelectedValue) : string.Empty;
            }
            set
            {
                ddlManagementStructureFilter.Text = value;
            }
        }

        public string SelectedProductLineFilter
        {
            get
            {
                return ddlProductLine.SelectedIndex > 0 ? Convert.ToString(ddlProductLine.SelectedValue) : string.Empty;
            }
        }

        public string SelectedCategoryFilter
        {
            get
            {
                return ddlCategory.SelectedIndex > 0 ? Convert.ToString(ddlCategory.SelectedValue) : string.Empty;
            }
        }

        public string SelectedSubCategory1Filter
        {
            get
            {
                return ddlSubCategory1.SelectedIndex > 0 ? Convert.ToString(ddlSubCategory1.SelectedValue) : string.Empty;
            }
        }

        public string SelectedSubCategory2Filter
        {
            get
            {
                return ddlSubCategory2.SelectedIndex > 0 ? Convert.ToString(ddlSubCategory2.SelectedValue) : string.Empty;
            }
        }

        public string SelectedSubCategory3Filter
        {
            get
            {
                return ddlSubCategory3.SelectedIndex > 0 ? Convert.ToString(ddlSubCategory3.SelectedValue) : string.Empty;
            }
        }

        public DateTime SelectedStartDate
        {
            get
            {
                return Convert.ToDateTime(txtStartDate.Text);
            }
        }

        public DateTime SelectedEndDate
        {
            get
            {
                return Convert.ToDateTime(txtEndDate.Text);
            }
        }

        #endregion


    }
}

