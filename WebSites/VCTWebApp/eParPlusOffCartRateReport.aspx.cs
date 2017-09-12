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
    public partial class eParPlusOffCartRateReport : Microsoft.Practices.CompositeWeb.Web.UI.Page, IeParPlusOffCartRateReport
    {
        #region Instance Variables
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private Security security = null;
        private eParPlusOffCartRateReportPresenter presenter;
        List<VCTWeb.Core.Domain.InventoryOffCartRate> listInventoryOffCartRate = new List<InventoryOffCartRate>();
        #endregion

        #region Private Properties

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
                    this.presenter.OnViewInitialized();
                    txtEndDate.Text = DateTime.Now.ToString("d");
                    txtStartDate.Text = DateTime.Now.AddDays(-6).ToString("d");
                    btnExport.Visible = false;
                }
                else
                    lblError.Text = string.Empty;
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
        public eParPlusOffCartRateReportPresenter Presenter
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

        protected void gdvInventoryAmount_Sorting(object sender, GridViewSortEventArgs e)
        {
            var sortImage = new Image();
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

            if (Session["ListInventoryOffCartRate"] != null)
            {
                List<InventoryOffCartRate> listInventoryAmount = Session["ListInventoryOffCartRate"] as List<InventoryOffCartRate>;
                if (sortingDirection == "Desc")
                {
                    if (e.SortExpression.Trim() == "CustomerName")
                        listInventoryAmount = listInventoryAmount.OrderByDescending(p => p.AccountNumber).ToList();
                    if (e.SortExpression.Trim() == "AccountNumber")
                        listInventoryAmount = listInventoryAmount.OrderByDescending(p => p.AccountNumber).ToList();
                }
                else
                {
                    if (e.SortExpression.Trim() == "CustomerName")
                        listInventoryAmount = listInventoryAmount.OrderBy(p => p.AccountNumber).ToList();
                    if (e.SortExpression.Trim() == "AccountNumber")
                        listInventoryAmount = listInventoryAmount.OrderBy(p => p.AccountNumber).ToList();
                }

                ListOffCartAmount = listInventoryAmount;

                int columnIndex = 0;
                foreach (DataControlFieldHeaderCell headerCell in gdvInventoryAmount.HeaderRow.Cells)
                {
                    if (headerCell.ContainingField.SortExpression == e.SortExpression)
                    {
                        columnIndex = gdvInventoryAmount.HeaderRow.Cells.GetCellIndex(headerCell);
                    }
                }
                gdvInventoryAmount.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);
            }
        }

        protected void gdvInventoryAmount_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    GridView grdChild = e.Row.FindControl("grdChild") as GridView;
                    InventoryOffCartRate objInventoryOffCartRate = (InventoryOffCartRate)e.Row.DataItem;
                    grdChild.DataSource = listInventoryOffCartRate.FindAll(i => i.AccountNumber == objInventoryOffCartRate.AccountNumber && i.CustomerName == objInventoryOffCartRate.CustomerName);
                    grdChild.DataBind();
                }
            }
            catch
            {

            }
        }

        protected void rblPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gdvInventoryAmount.DataSource = null;
                gdvInventoryAmount.DataBind();
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
                //this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        protected void lnkFilterCustomerListData_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = string.Empty;
                if (rblPeriod.SelectedIndex == 2)
                {
                    DateTime StartDate = Convert.ToDateTime(txtStartDate.Text);
                    DateTime EndDate = Convert.ToDateTime(txtEndDate.Text);
                    if (StartDate > EndDate)
                        lblError.Text = "Start date should not be greater than End date.";
                    else
                        presenter.PopulateReport();
                }
                else
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

                txtEndDate.Text = DateTime.Now.ToString("d");
                txtStartDate.Text = DateTime.Now.AddDays(-6).ToString("d");
                rblPeriod.SelectedIndex = 0;
                imgCalenderFrom.Visible = false;
                Image1.Visible = false;
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

        private void AuthorizedPage()
        {
            security = new Security();
            if (Session["LoggedInUser"] == null)
            {
                Response.Redirect("~/ErrorPage.aspx?ErrorKey=Common_msgSessionExpired");
            }
            else if (security.HasAccess("ePar+.ConsumptionRateReport"))
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
            List<InventoryOffCartRate> lstReportData = presenter.PopulateReport();
            if (lstReportData != null && lstReportData.Count > 0)
            {
                DataTable dtReportTable = new DataTable();
                dtReportTable.Columns.Add("Customer Name", typeof(string));
                dtReportTable.Columns.Add("Account Number", typeof(string));
                dtReportTable.Columns.Add("Product Line", typeof(string));
                dtReportTable.Columns.Add("Product Line Desc", typeof(string));
                dtReportTable.Columns.Add("Ref #", typeof(string));
                dtReportTable.Columns.Add("Description", typeof(string));
                dtReportTable.Columns.Add("Lot Num", typeof(string));
                dtReportTable.Columns.Add("RFID Tag Id", typeof(string));
                dtReportTable.Columns.Add("Check In Date", typeof(string));
                dtReportTable.Columns.Add("Consumption Date", typeof(string));
                dtReportTable.Columns.Add("Off-Cart Times", typeof(string));


                foreach (VCTWeb.Core.Domain.InventoryOffCartRate item in lstReportData)
                {
                    DataRow dr;
                    dr = dtReportTable.NewRow();
                    dr["Customer Name"] = item.CustomerName != null ? item.CustomerName.ToString() : string.Empty;
                    dr["Account Number"] = item.AccountNumber != null ? item.AccountNumber.ToString() : string.Empty;                                        
                    dr["Product Line"] = item.ProductLine != null ? item.ProductLine.ToString() : string.Empty;
                    dr["Product Line Desc"] = item.ProductLineDesc != null ? item.ProductLineDesc.ToString() : string.Empty;
                    dr["Ref #"] = item.RefNum != null ? item.RefNum.ToString() : string.Empty;
                    dr["Description"] = item.PartDesc != null ? item.PartDesc.ToString() : string.Empty;
                    dr["Lot Num"] = item.LotNum != null ? item.LotNum.ToString() : string.Empty;
                    dr["RFID Tag Id"] = item.TagId != null ? item.TagId.ToString() : string.Empty;
                    dr["Check In Date"] = item.CheckInDate.ToString("MM/dd/yyyy hh:mm:ss tt");
                    dr["Consumption Date"] = item.CheckInDate.ToString("MM/dd/yyyy hh:mm:ss tt");                   
                    dr["Off-Cart Times"] = item.OffCartCount.ToString();
                    dtReportTable.Rows.Add(dr);
                }

                grdViewExport.DataSource = dtReportTable;
                grdViewExport.DataBind();

                string filename = "Off-CartRateReport_" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString()
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

        #region IeParPlusitemsReport Members

        public List<InventoryOffCartRate> ListOffCartAmount
        {
            set
            {
                lblError.Text = "";
                gdvInventoryAmount.DataSource = null;
                gdvInventoryAmount.DataBind();
                if (value == null) return;
                listInventoryOffCartRate = value;
                Session["ListInventoryOffCartRate"] = value;
                btnExport.Visible = ((value != null && value.Count > 0) ? true : false);
                var lstitemSummary = new List<InventoryOffCartRate>();
                var distinctLocationPart = listInventoryOffCartRate.Select(s => new { CustomerName = s.CustomerName, AccountNumber = s.AccountNumber }).Distinct().ToList();
                foreach (var locationStatus in distinctLocationPart)
                {
                    lstitemSummary.Add(new InventoryOffCartRate()
                    {
                        CustomerName = locationStatus.CustomerName,
                        AccountNumber = locationStatus.AccountNumber
                    });
                }
                gdvInventoryAmount.DataSource = lstitemSummary;
                gdvInventoryAmount.DataBind();
                lblError.Text = (value.Count <= 0 ? "No Record Found." : "");
            }
        }

        List<VCTWeb.Core.Domain.Customer> IeParPlusOffCartRateReport.CustomerNameList
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

