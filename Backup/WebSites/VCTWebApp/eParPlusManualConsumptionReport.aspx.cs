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
using System.Globalization;

namespace VCTWebApp.Shell.Views
{
    public partial class eParPlusManualConsumptionReport : Microsoft.Practices.CompositeWeb.Web.UI.Page, IeParPlusManualConsumption
    {
        #region Instance Variables
        private readonly VCTWebAppResource _vctResource = new VCTWebAppResource();
        private Security _security;
        private EParPlusManualConsumptionPresenter _presenter;
        List<ManualConsumptionReport> _lstManualConsumption = new List<ManualConsumptionReport>();
        #endregion

        #region Properties


        private bool CanRevertConsume
        {
            get
            {
                return ViewState[Common.CAN_REVERT_CONSUME] != null ? (bool)ViewState[Common.CAN_REVERT_CONSUME] : false;
            }
            set
            {
                ViewState[Common.CAN_REVERT_CONSUME] = value;
            }
        }

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
                    btnExport.Visible = false;
                }
                else
                    lblError.Text = string.Empty;
            }
            catch (SqlException ex)
            {
                //lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), lblHeader.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region Create New Presenter

        [CreateNew]
        public EParPlusManualConsumptionPresenter Presenter
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

        protected void gdvManualConsumption_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    GridView grdChild = e.Row.FindControl("grdChild") as GridView;
                    VCTWeb.Core.Domain.ManualConsumptionReport objManualConsumption = (VCTWeb.Core.Domain.ManualConsumptionReport)e.Row.DataItem;
                    grdChild.DataSource = _lstManualConsumption.FindAll(x => x.CustomerName == objManualConsumption.CustomerName && x.AccountNumber == objManualConsumption.AccountNumber);
                    grdChild.DataBind();
                    grdChild.Columns[10].Visible = CanRevertConsume;
                }
            }
            catch
            {

            }
        }
        
        protected void grdChild_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    var objManualConsumption = (ManualConsumptionReport)e.Row.DataItem;

                    if (objManualConsumption.IsConsumed)
                    {
                        var linkButton = e.Row.FindControl("lnkRevertConsumed") as LinkButton;
                        if (linkButton != null)
                        {
                            linkButton.OnClientClick = "";
                            linkButton.Enabled = false;
                        }
                        var imgRevertConsume = e.Row.FindControl("imgRevertConsume") as Image;
                        if (imgRevertConsume != null)
                        {
                            imgRevertConsume.ImageUrl = "~/Images/undo_gray.png";
                            imgRevertConsume.ToolTip = "Item marked as consumed, So this item cannot be reverted.";
                            imgRevertConsume.Enabled = false;
                        }
                    }
                }
            }
            catch
            {

            }
        }

        protected void grdChild_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("RevertConsumed"))
                {
                    var tagId = Convert.ToString(e.CommandArgument);

                    var row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);

                    var lblAccountNumber = row.Cells[0].FindControl("lblAccountNumber") as Label;
                    if (lblAccountNumber != null)
                    {
                        var accountNumber = lblAccountNumber.Text.Trim();

                        if (!string.IsNullOrEmpty(tagId))
                        {
                            if (Presenter.RevertManualConsumption(accountNumber, tagId))
                            {
                                lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, _vctResource.GetString("msgPARLevelDelete"), lblHeader.Text) + "</font>";
                                lnkFilterCustomerListData_Click(sender, e);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, _vctResource.GetString(ex.Message), lblHeader.Text);
            }
        }

        protected void gdvManualConsumption_Sorting(object sender, GridViewSortEventArgs e)
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

            if (Session["ListManualConsumptionReport"] != null)
            {
                var listManualConsumption = Session["ListManualConsumptionReport"] as List<ManualConsumptionReport>;

                if (sortingDirection == "Desc")
                {
                    if (e.SortExpression.Trim() == "CustomerName")
                        listManualConsumption = listManualConsumption.OrderByDescending(p => p.AccountNumber).ToList();
                    if (e.SortExpression.Trim() == "AccountNumber")
                        listManualConsumption = listManualConsumption.OrderByDescending(p => p.AccountNumber).ToList();
                }
                else
                {
                    if (e.SortExpression.Trim() == "CustomerName")
                        listManualConsumption = listManualConsumption.OrderBy(p => p.AccountNumber).ToList();
                    if (e.SortExpression.Trim() == "AccountNumber")
                        listManualConsumption = listManualConsumption.OrderBy(p => p.AccountNumber).ToList();
                }

                ListManualConsumptionReport = listManualConsumption;

                int columnIndex = 0;
                foreach (DataControlFieldHeaderCell headerCell in gdvManualConsumption.HeaderRow.Cells)
                {
                    if (headerCell.ContainingField.SortExpression == e.SortExpression)
                    {
                        columnIndex = gdvManualConsumption.HeaderRow.Cells.GetCellIndex(headerCell);
                    }
                }
                gdvManualConsumption.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);
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
                var listManualConsumptionReport = Session["ListManualConsumptionReport"] as List<ManualConsumptionReport>;

                if (listManualConsumptionReport != null)
                {
                    List<ManualConsumptionReport> listManualConsumptionReportChild = listManualConsumptionReport.FindAll(i => i.AccountNumber == accountNumber);
                    if (sortingDirection == "Desc")
                    {
                        if (e.SortExpression.Trim() == "ProductLine")
                            listManualConsumptionReportChild = listManualConsumptionReportChild.OrderByDescending(p => p.ProductLine).ToList();

                        if (e.SortExpression.Trim() == "ProductLineDesc")
                            listManualConsumptionReportChild = listManualConsumptionReportChild.OrderByDescending(p => p.ProductLineDesc).ToList();

                        if (e.SortExpression.Trim() == "RefNum")
                            listManualConsumptionReportChild = listManualConsumptionReportChild.OrderByDescending(p => p.RefNum).ToList();

                        if (e.SortExpression.Trim() == "PartDesc")
                            listManualConsumptionReportChild = listManualConsumptionReportChild.OrderByDescending(p => p.PartDesc).ToList();

                        if (e.SortExpression.Trim() == "LotNum")
                            listManualConsumptionReportChild = listManualConsumptionReportChild.OrderByDescending(p => p.LotNum).ToList();

                        if (e.SortExpression.Trim() == "TagId")
                            listManualConsumptionReportChild = listManualConsumptionReportChild.OrderByDescending(p => p.TagId).ToList();

                        if (e.SortExpression.Trim() == "UpdatedOn")
                            listManualConsumptionReportChild = listManualConsumptionReportChild.OrderByDescending(p => p.UpdatedOn).ToList();

                        if (e.SortExpression.Trim() == "UpdatedBy")
                            listManualConsumptionReportChild = listManualConsumptionReportChild.OrderByDescending(p => p.UpdatedBy).ToList();

                    }
                    else
                    {
                        if (e.SortExpression.Trim() == "ProductLine")
                            listManualConsumptionReportChild = listManualConsumptionReportChild.OrderBy(p => p.ProductLine).ToList();

                        if (e.SortExpression.Trim() == "ProductLineDesc")
                            listManualConsumptionReportChild = listManualConsumptionReportChild.OrderBy(p => p.ProductLineDesc).ToList();

                        if (e.SortExpression.Trim() == "RefNum")
                            listManualConsumptionReportChild = listManualConsumptionReportChild.OrderBy(p => p.RefNum).ToList();

                        if (e.SortExpression.Trim() == "PartDesc")
                            listManualConsumptionReportChild = listManualConsumptionReportChild.OrderBy(p => p.PartDesc).ToList();

                        if (e.SortExpression.Trim() == "LotNum")
                            listManualConsumptionReportChild = listManualConsumptionReportChild.OrderBy(p => p.LotNum).ToList();

                        if (e.SortExpression.Trim() == "TagId")
                            listManualConsumptionReportChild = listManualConsumptionReportChild.OrderBy(p => p.TagId).ToList();

                        if (e.SortExpression.Trim() == "UpdatedOn")
                            listManualConsumptionReportChild = listManualConsumptionReportChild.OrderBy(p => p.UpdatedOn).ToList();

                        if (e.SortExpression.Trim() == "UpdatedBy")
                            listManualConsumptionReportChild = listManualConsumptionReportChild.OrderBy(p => p.UpdatedBy).ToList();
                    }

                    gdvChild.DataSource = listManualConsumptionReportChild;
                }
                gdvChild.DataBind();

                foreach (DataControlFieldHeaderCell headerCell in gdvChild.HeaderRow.Cells)
                {
                    if (headerCell.ContainingField.SortExpression == e.SortExpression)
                    {
                        var columnIndex = gdvChild.HeaderRow.Cells.GetCellIndex(headerCell);
                        gdvChild.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);
                    }
                }
            }
        }

        protected void rblPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gdvManualConsumption.DataSource = null;
                gdvManualConsumption.DataBind();
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
                //lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), lblHeader.Text);
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

                rblPeriod.SelectedIndex = 0;
                imgCalenderFrom.Visible = false;
                Image1.Visible = false;

                txtEndDate.Text = DateTime.Now.ToString("d");
                txtStartDate.Text = DateTime.Now.AddDays(-6).ToString("d");
                rblPeriod.SelectedIndex = 0;
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
                _presenter.PopulateCategory();
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
                _presenter.PopulateSubCategory1();
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
                _presenter.PopulateSubCategory2();
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
                _presenter.PopulateSubCategory3();
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
                    _presenter.FillDropdowns(string.Empty, string.Empty);
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
                _presenter.FillDropdowns("BranchAgency",
                    ddlBranchAgencyFilter.SelectedIndex > 0
                        ? Convert.ToString(ddlBranchAgencyFilter.SelectedValue)
                        : Convert.ToString(0));
            }
            catch { }
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
            catch { }
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
            catch { }
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
            catch { }
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
            catch { }
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
            catch { }
        }

        #endregion

        #region Private Methods

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        private void ExportInExcel()
        {
            grdViewExport.DataSource = null;
            grdViewExport.DataBind();
            List<ManualConsumptionReport> lstReportData = _presenter.PopulateReport();
            if (lstReportData != null && lstReportData.Count > 0)
            {
                var dtReportTable = new DataTable();
                dtReportTable.Columns.Add("Customer Name", typeof(string));
                dtReportTable.Columns.Add("Account Number", typeof(string));
                dtReportTable.Columns.Add("Product Line", typeof(string));
                dtReportTable.Columns.Add("Product Line Desc", typeof(string));
                dtReportTable.Columns.Add("Ref #", typeof(string));
                dtReportTable.Columns.Add("Description", typeof(string));                
            
                dtReportTable.Columns.Add("Lot Num", typeof(string));
                dtReportTable.Columns.Add("RFID Tag Id", typeof(string));
                dtReportTable.Columns.Add("Manual Consumed On", typeof(string));
                dtReportTable.Columns.Add("Manual Consumed By", typeof(string));
                dtReportTable.Columns.Add("Consumed Date", typeof(string));

                foreach (var item in lstReportData)
                {
                    DataRow dr;
                    dr = dtReportTable.NewRow();
                    dr["Customer Name"] = item.CustomerName ?? string.Empty;
                    dr["Account Number"] = item.AccountNumber ?? string.Empty;
                    dr["Product Line"] = item.ProductLine ?? string.Empty;
                    dr["Product Line Desc"] = item.ProductLineDesc ?? string.Empty;
                    dr["Ref #"] = item.RefNum ?? string.Empty;
                    dr["Description"] = item.PartDesc ?? string.Empty;

                    dr["Lot Num"] = item.LotNum ?? string.Empty;
                    dr["RFID Tag Id"] = item.TagId ?? string.Empty;
                    dr["Manual Consumed On"] = item.UpdatedOn.ToString("MM/dd/yyyy hh:mm:ss tt");
                    dr["Manual Consumed By"] = item.UpdatedBy ?? string.Empty;

                    var consumedDate = Convert.ToDateTime(item.ConsumedDate).ToString("MM/dd/yyyy hh:mm:ss tt");
                    if (consumedDate == "01/01/0001 12:00:00 AM")
                        consumedDate = "---";

                    //dr["Consumed Date"] = item.ConsumedDate != null ? Convert.ToDateTime(item.ConsumedDate).ToString("MM/dd/yyyy hh:mm:ss tt") : "---";
                    dr["Consumed Date"] = consumedDate;
                    
                    dtReportTable.Rows.Add(dr);
                }

                grdViewExport.DataSource = dtReportTable;
                grdViewExport.DataBind();

                string filename = "ManualConsumptionReport_" + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Year
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
        }

        protected string GetDate(object strDt)
        {
            DateTime dt1;
            return DateTime.TryParse(strDt.ToString(), out dt1)
                ? (dt1.ToString("MM/dd/yyyy") != "01/01/0001" ? dt1.ToString("MM/dd/yyyy hh:mm:ss tt") : "---")
                : "";
        }

        private void AuthorizedPage()
        {
            _security = new Security();
            if (Session["LoggedInUser"] == null)
            {
                Response.Redirect("~/ErrorPage.aspx?ErrorKey=Common_msgSessionExpired");
            }
            else if (_security.HasAccess("ePar+.ManualConsumptionReport"))
            {
                CanRevertConsume = _security.HasPermission("ePar+.ManualConsumptionReport.Revert");
            }
            else
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }

        #endregion

        #region IeParPlusManualConsumption Members

        public List<ManualConsumptionReport> ListManualConsumptionReport
        {
            set
            {
                lblError.Text = "";
                gdvManualConsumption.DataSource = null;
                gdvManualConsumption.DataBind();

                if (value == null)
                    return;

                _lstManualConsumption = value;

                if (_lstManualConsumption != null && _lstManualConsumption.Count > 0)
                    btnExport.Visible = true;

                var lstInventoryAmountSummary = new List<ManualConsumptionReport>();
                var distinctLocationPart = _lstManualConsumption.Select(s => new {s.CustomerName, s.AccountNumber }).Distinct().ToList();
                foreach (var locationStatus in distinctLocationPart)
                {
                    lstInventoryAmountSummary.Add(new ManualConsumptionReport
                    {
                        CustomerName = locationStatus.CustomerName,
                        AccountNumber = locationStatus.AccountNumber
                    });
                }
                Session["ListManualConsumptionReport"] = value;
                gdvManualConsumption.DataSource = lstInventoryAmountSummary;
                gdvManualConsumption.DataBind();
                lblError.Text = (value.Count <= 0 ? "No Record Found." : "");
            }
        }

        List<VCTWeb.Core.Domain.Customer> IeParPlusManualConsumption.CustomerNameList
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
                try
                {
                    ddlCustomerNameFilter.SelectedValue = value;
                }
                catch { }
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
                try
                {
                    ddlBranchAgencyFilter.Text = value;
                }
                catch { }
            }
        }

        public string SelectedManagerFilter
        {
            get {
                return ddlManagerFilter.SelectedIndex > 0 ? Convert.ToString(ddlManagerFilter.SelectedValue) : string.Empty;
            }
            set
            {
                try
                {
                    ddlManagerFilter.Text = value;
                }
                catch { }
            }
        }

        public string SelectedSalesRepresentativeFilter
        {
            get {
                return ddlSalesRepresentativeFilter.SelectedIndex > 0 ? Convert.ToString(ddlSalesRepresentativeFilter.SelectedValue) : string.Empty;
            }
            set
            {
                try
                {
                    ddlSalesRepresentativeFilter.Text = value;
                }
                catch { }
            }
        }

        public string SelectedStateFilter
        {
            get {
                return ddlStateFilter.SelectedIndex > 0 ? Convert.ToString(ddlStateFilter.SelectedValue) : string.Empty;
            }
            set
            {
                try
                {
                    ddlStateFilter.Text = value;
                }
                catch { }
            }
        }

        public string SelectedOwnershipStructureFilter
        {
            get {
                return ddlOwnershipStructureFilter.SelectedIndex > 0 ? Convert.ToString(ddlOwnershipStructureFilter.SelectedValue) : string.Empty;
            }
            set
            {
                try
                {
                    ddlOwnershipStructureFilter.Text = value;
                }
                catch { }
            }
        }

        public string SelectedManagementStructureFilter
        {
            get {
                return ddlManagementStructureFilter.SelectedIndex > 0 ? Convert.ToString(ddlManagementStructureFilter.SelectedValue) : string.Empty;
            }
            set
            {
                try
                {
                    ddlManagementStructureFilter.Text = value;
                }
                catch { }
            }
        }


        public string SelectedProductLineFilter
        {
            get {
                return ddlProductLine.SelectedIndex > 0 ? Convert.ToString(ddlProductLine.SelectedValue) : string.Empty;
            }
        }

        public string SelectedCategoryFilter
        {
            get {
                return ddlCategory.SelectedIndex > 0 ? Convert.ToString(ddlCategory.SelectedValue) : string.Empty;
            }
        }

        public string SelectedSubCategory1Filter
        {
            get {
                return ddlSubCategory1.SelectedIndex > 0 ? Convert.ToString(ddlSubCategory1.SelectedValue) : string.Empty;
            }
        }

        public string SelectedSubCategory2Filter
        {
            get {
                return ddlSubCategory2.SelectedIndex > 0 ? Convert.ToString(ddlSubCategory2.SelectedValue) : string.Empty;
            }
        }

        public string SelectedSubCategory3Filter
        {
            get {
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

