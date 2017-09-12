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
using System.Drawing;

namespace VCTWebApp.Shell.Views
{
    public partial class eParPlusLowInventoryReport : Microsoft.Practices.CompositeWeb.Web.UI.Page, IeParPlusLowInventoryReport
    {
        #region Instance Variables
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private eParPlusLowInventoryReportPresenter presenter;
        private Security security = null;
        List<VCTWeb.Core.Domain.LowInventory> lstLowInventory = new List<LowInventory>();
        #endregion

        #region Properties

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
                    this.presenter.OnViewInitialized();
                    this.AuthorizedPage();
                    btnExport.Visible = false;
                    btnSendEmail.Visible = false;
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

        [CreateNew]
        public eParPlusLowInventoryReportPresenter Presenter
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

        protected void gdvLowInventory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    GridView grdChild = e.Row.FindControl("grdChild") as GridView;
                    VCTWeb.Core.Domain.LowInventory objLowInventory = (VCTWeb.Core.Domain.LowInventory)e.Row.DataItem;
                    grdChild.DataSource = lstLowInventory.FindAll(w => w.CustomerName == objLowInventory.CustomerName && w.AccountNumber == objLowInventory.AccountNumber).ToList();
                    grdChild.DataBind();
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
                    VCTWeb.Core.Domain.LowInventory objLowInventory = (VCTWeb.Core.Domain.LowInventory)e.Row.DataItem;
                    if (objLowInventory.BackOrderQty > 0)
                    {
                        Label lblLowInvQty = e.Row.FindControl("lblLowInvQty") as Label;
                        if (lblLowInvQty != null)
                        {
                            lblLowInvQty.ForeColor = Color.Red;
                            lblLowInvQty.Font.Underline = true;
                            lblLowInvQty.ToolTip = "Back Order Qty: " + objLowInventory.BackOrderQty.ToString();
                        }
                    }
                }
            }
            catch
            {

            }
        }

        protected void gdvLowInventory_Sorting(object sender, GridViewSortEventArgs e)
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

            if (Session["ListLowInventory"] != null)
            {
                //Sort the data.
                List<LowInventory> listLowInventory = Session["ListLowInventory"] as List<LowInventory>;
                if (sortingDirection == "Desc")
                {
                    if (e.SortExpression.Trim() == "CustomerName")
                        listLowInventory = listLowInventory.OrderByDescending(p => p.AccountNumber).ToList();
                    if (e.SortExpression.Trim() == "AccountNumber")
                        listLowInventory = listLowInventory.OrderByDescending(p => p.AccountNumber).ToList();
                    if (e.SortExpression.Trim() == "LowInvQty")
                        listLowInventory = listLowInventory.OrderByDescending(p => p.LowInvQty).ToList();
                }
                else
                {
                    if (e.SortExpression.Trim() == "CustomerName")
                        listLowInventory = listLowInventory.OrderBy(p => p.AccountNumber).ToList();
                    if (e.SortExpression.Trim() == "AccountNumber")
                        listLowInventory = listLowInventory.OrderBy(p => p.AccountNumber).ToList();
                    if (e.SortExpression.Trim() == "LowInvQty")
                        listLowInventory = listLowInventory.OrderBy(p => p.LowInvQty).ToList();
                }

                ListLowInventory = listLowInventory;

                int columnIndex = 0;
                foreach (DataControlFieldHeaderCell headerCell in gdvLowInventory.HeaderRow.Cells)
                {
                    if (headerCell.ContainingField.SortExpression == e.SortExpression)
                    {
                        columnIndex = gdvLowInventory.HeaderRow.Cells.GetCellIndex(headerCell);
                    }
                }
                gdvLowInventory.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);
            }
        }

        protected void grdChild_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridView gdvChild = (GridView)sender;

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

            HiddenField hndAccountNumber = null;
            string AccountNumber = string.Empty;

            foreach (GridViewRow gvr in gdvChild.Rows)
            {
                hndAccountNumber = gvr.FindControl("hndAccountNumber") as HiddenField;
                if (hndAccountNumber != null)
                    break;
            }

            if (hndAccountNumber != null)
            {
                AccountNumber = hndAccountNumber.Value.Split(',')[0].Trim();
                //Sort the data.
                List<LowInventory> listLowInventory = Session["ListLowInventory"] as List<LowInventory>;
                List<LowInventory> listLowInventoryChild = new List<LowInventory>();
                listLowInventoryChild = listLowInventory.FindAll(i => i.AccountNumber == AccountNumber);
                if (sortingDirection == "Desc")
                {
                    if (e.SortExpression.Trim() == "ProductLine")
                        listLowInventoryChild = listLowInventoryChild.OrderByDescending(p => p.ProductLine).ToList();

                    if (e.SortExpression.Trim() == "ProductLineDesc")
                        listLowInventoryChild = listLowInventoryChild.OrderByDescending(p => p.ProductLineDesc).ToList();

                    if (e.SortExpression.Trim() == "RefNum")
                        listLowInventoryChild = listLowInventoryChild.OrderByDescending(p => p.RefNum).ToList();

                    if (e.SortExpression.Trim() == "PartDesc")
                        listLowInventoryChild = listLowInventoryChild.OrderByDescending(p => p.PartDesc).ToList();

                    if (e.SortExpression.Trim() == "Size")
                        listLowInventoryChild = listLowInventoryChild.OrderByDescending(p => p.Size).ToList();

                    if (e.SortExpression.Trim() == "LowInvQty")
                        listLowInventoryChild = listLowInventoryChild.OrderByDescending(p => p.LowInvQty).ToList();

                    if (e.SortExpression.Trim() == "OrderedProductQty")
                        listLowInventoryChild = listLowInventoryChild.OrderByDescending(p => p.OrderedProductQty).ToList();

                    if (e.SortExpression.Trim() == "InvLevelQty")
                        listLowInventoryChild = listLowInventoryChild.OrderByDescending(p => p.InvLevelQty).ToList();

                    if (e.SortExpression.Trim() == "PARLevelQty")
                        listLowInventoryChild = listLowInventoryChild.OrderByDescending(p => p.PARLevelQty).ToList();
                }
                else
                {

                    if (e.SortExpression.Trim() == "ProductLine")
                        listLowInventoryChild = listLowInventoryChild.OrderBy(p => p.ProductLine).ToList();

                    if (e.SortExpression.Trim() == "ProductLineDesc")
                        listLowInventoryChild = listLowInventoryChild.OrderBy(p => p.ProductLineDesc).ToList();

                    if (e.SortExpression.Trim() == "RefNum")
                        listLowInventoryChild = listLowInventoryChild.OrderBy(p => p.RefNum).ToList();

                    if (e.SortExpression.Trim() == "PartDesc")
                        listLowInventoryChild = listLowInventoryChild.OrderBy(p => p.PartDesc).ToList();

                    if (e.SortExpression.Trim() == "Size")
                        listLowInventoryChild = listLowInventoryChild.OrderBy(p => p.Size).ToList();

                    if (e.SortExpression.Trim() == "LowInvQty")
                        listLowInventoryChild = listLowInventoryChild.OrderBy(p => p.LowInvQty).ToList();

                    if (e.SortExpression.Trim() == "OrderedProductQty")
                        listLowInventoryChild = listLowInventoryChild.OrderBy(p => p.OrderedProductQty).ToList();

                    if (e.SortExpression.Trim() == "InvLevelQty")
                        listLowInventoryChild = listLowInventoryChild.OrderBy(p => p.InvLevelQty).ToList();

                    if (e.SortExpression.Trim() == "PARLevelQty")
                        listLowInventoryChild = listLowInventoryChild.OrderBy(p => p.PARLevelQty).ToList();

                }

                gdvChild.DataSource = listLowInventoryChild;
                gdvChild.DataBind();

                int columnIndex = 0;
                foreach (DataControlFieldHeaderCell headerCell in gdvChild.HeaderRow.Cells)
                {
                    if (headerCell.ContainingField.SortExpression == e.SortExpression)
                    {
                        columnIndex = gdvChild.HeaderRow.Cells.GetCellIndex(headerCell);
                        gdvChild.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);
                    }
                }
            }
        }


        protected void lnkFilterCustomerListData_Click(object sender, EventArgs e)
        {
            try
            {
                presenter.PopulateReport();
            }
            catch (Exception ex)
            {

            }
        }


        protected void btnSendEmail_Click(object sender, EventArgs e)
        {
            try
            {
                bool isEmailSent = presenter.SendLowInventoryNotificationEmail();
                if (isEmailSent)
                {
                    lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgEmailSentSucessfully"), lblHeader.Text) + "</font>";
                }
                else
                {
                    lblError.Text = "<font color='red'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgEmailSentFailed"), lblHeader.Text) + "</font>";
                }
            }
            catch (Exception ex)
            {
                var str = ex.Message;
                if (str.ToUpper().Contains("THE SPECIFIED STRING IS NOT IN THE FORM REQUIRED FOR AN E-MAIL ADDRESS"))
                {
                    lblError.Text = "<font color='red'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgInvalidEmailConfigured"), lblHeader.Text) + "</font>";
                }
                else if (str.ToUpper().Contains("NO EMAIL CONFIGURED"))
                {
                    lblError.Text = "<font color='red'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgNoEmailConfigured"), lblHeader.Text) + "</font>";
                }
                else if (str.ToUpper().Contains("THE SERVER RESPONSE WAS: 5.7.1 UNABLE TO RELAY"))
                {
                    lblError.Text = "<font color='red'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgInvalidEmailConfigured"), lblHeader.Text) + "</font>";
                }
                else
                {
                    lblError.Text = "<font color='red'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgGenericErrorMessage"), lblHeader.Text) + "</font>";
                }
            }
        }

        protected void lnkRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = string.Empty;

                this.presenter.OnViewInitialized();
                btnExport.Visible = false;
                btnSendEmail.Visible = false;

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

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        private void ExportInExcel()
        {
            grdViewExport.DataSource = null;
            grdViewExport.DataBind();
            List<LowInventory> lstInventoryReportToExport = presenter.PopulateReport();
            if (lstInventoryReportToExport != null && lstInventoryReportToExport.Count > 0)
            {
                DataTable dtReportTable = new DataTable();
                dtReportTable.Columns.Add("Customer Name", typeof(string));
                dtReportTable.Columns.Add("Account Number", typeof(string));

                dtReportTable.Columns.Add("Product Line", typeof(string));
                dtReportTable.Columns.Add("Product Line Desc", typeof(string));
                dtReportTable.Columns.Add("Ref #", typeof(string));

                dtReportTable.Columns.Add("Description", typeof(string));
                dtReportTable.Columns.Add("Size", typeof(string));

                dtReportTable.Columns.Add("Required Order", typeof(string));
                dtReportTable.Columns.Add("Already On Order", typeof(string));
                dtReportTable.Columns.Add("Current Inventory", typeof(string));
                dtReportTable.Columns.Add("PAR Level", typeof(string));



                foreach (VCTWeb.Core.Domain.LowInventory inventoryAmount in lstInventoryReportToExport)
                {
                    DataRow dr;
                    dr = dtReportTable.NewRow();
                    dr["Customer Name"] = inventoryAmount.CustomerName != null ? inventoryAmount.CustomerName.ToString() : string.Empty;
                    dr["Account Number"] = inventoryAmount.AccountNumber != null ? inventoryAmount.AccountNumber.ToString() : string.Empty;

                    dr["Product Line"] = inventoryAmount.ProductLine != null ? inventoryAmount.ProductLine.ToString() : string.Empty;
                    dr["Product Line Desc"] = inventoryAmount.ProductLineDesc != null ? inventoryAmount.ProductLineDesc.ToString() : string.Empty;
                    dr["Ref #"] = inventoryAmount.RefNum != null ? inventoryAmount.RefNum.ToString() : string.Empty;

                    dr["Description"] = inventoryAmount.PartDesc != null ? inventoryAmount.PartDesc.ToString() : string.Empty;
                    dr["Size"] = inventoryAmount.Size != null ? inventoryAmount.Size.ToString() : string.Empty;

                    dr["Required Order"] = inventoryAmount.LowInvQty.ToString();
                    dr["Already On Order"] = inventoryAmount.OrderedProductQty.ToString();
                    dr["Current Inventory"] = inventoryAmount.InvLevelQty.ToString();
                    dr["PAR Level"] = inventoryAmount.PARLevelQty.ToString();
                    dtReportTable.Rows.Add(dr);
                }

                grdViewExport.DataSource = dtReportTable;
                grdViewExport.DataBind();

                string filename = "LowInventoryReport_" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString()
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

        private void AuthorizedPage()
        {
            security = new Security();
            if (Session["LoggedInUser"] == null)
            {
                Response.Redirect("~/ErrorPage.aspx?ErrorKey=Common_msgSessionExpired");
            }
            else if (security.HasAccess("ePar+.LowInventoryReport"))
            {
                //CanCancel = security.HasPermission("InventoryStockParts");
            }
            else
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }

        #endregion

        #region IeParPlusLowInventoryReport Members

        public List<LowInventory> ListLowInventory
        {
            set
            {
                lblError.Text = "";
                lstLowInventory = value;
                gdvLowInventory.DataSource = null;
                gdvLowInventory.DataBind();

                if (lstLowInventory != null && lstLowInventory.Count > 0)
                {
                    btnExport.Visible = true;
                    btnSendEmail.Visible = true;
                }

                List<VCTWeb.Core.Domain.LowInventory> lstInventoryAmountSummary = new List<VCTWeb.Core.Domain.LowInventory>();
                if (lstLowInventory != null)
                {
                    var distinctCustomerList = lstLowInventory.Select(s => new { CustomerName = s.CustomerName, AccountNumber = s.AccountNumber }).Distinct().ToList();
                    foreach (var locationStatus in distinctCustomerList)
                    {
                        int RequiredOrder = 0;
                        var itemList = lstLowInventory.FindAll(x => x.CustomerName == locationStatus.CustomerName && x.AccountNumber == locationStatus.AccountNumber);
                        foreach (var item in itemList)
                        {
                            RequiredOrder += item.LowInvQty;
                        }

                        lstInventoryAmountSummary.Add(new VCTWeb.Core.Domain.LowInventory()
                        {
                            CustomerName = locationStatus.CustomerName,
                            AccountNumber = locationStatus.AccountNumber,
                            LowInvQty = RequiredOrder
                        });
                    }
                    Session["ListLowInventory"] = value;
                    gdvLowInventory.DataSource = lstInventoryAmountSummary;
                    gdvLowInventory.DataBind();
                    lblError.Text = (value.Count <= 0 ? "No Record Found." : "");
                }
            }
            get
            {
                return Session["ListLowInventory"] as List<LowInventory>;
            }
        }

        List<VCTWeb.Core.Domain.Customer> IeParPlusLowInventoryReport.CustomerNameList
        {
            set
            {
                this.ddlCustomerNameFilter.DataSource = null;
                this.ddlCustomerNameFilter.DataSource = value;
                this.ddlCustomerNameFilter.DataTextField = "NameAccount";
                this.ddlCustomerNameFilter.DataValueField = "AccountNumber";
                this.ddlCustomerNameFilter.DataBind();
                this.ddlCustomerNameFilter.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), "0"));
                //ddlCustomerNameFilter.SelectedIndex = 0;
            }
        }

        public List<string> BranchAgencyList
        {
            set
            {
                this.ddlBranchAgencyFilter.DataSource = null;
                this.ddlBranchAgencyFilter.DataSource = value;
                //this.ddlSalesRepresentativeFilter.DataTextField = "BranchAgency";
                //this.ddlSalesRepresentativeFilter.DataValueField = "BranchAgency";
                this.ddlBranchAgencyFilter.DataBind();
                this.ddlBranchAgencyFilter.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), "0"));
                //ddlBranchAgencyFilter.SelectedIndex = 0;
            }
        }

        public List<string> ManagerList
        {
            set
            {
                this.ddlManagerFilter.DataSource = null;
                this.ddlManagerFilter.DataSource = value;
                //this.ddlManagerFilter.DataTextField = "Manager";
                //this.ddlManagerFilter.DataValueField = "Manager";
                this.ddlManagerFilter.DataBind();
                this.ddlManagerFilter.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), "0"));
                //ddlManagerFilter.SelectedIndex = 0;
            }
        }

        public List<string> SalesRepresentativeList
        {
            set
            {
                ddlSalesRepresentativeFilter.SelectedValue = null;
                this.ddlSalesRepresentativeFilter.DataSource = null;
                this.ddlSalesRepresentativeFilter.DataSource = value;
                //this.ddlSalesRepresentativeFilter.DataTextField = "SalesRepresentative";
                //this.ddlSalesRepresentativeFilter.DataValueField = "SalesRepresentative";
                this.ddlSalesRepresentativeFilter.DataBind();
                this.ddlSalesRepresentativeFilter.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), "0"));
                //ddlSalesRepresentativeFilter.SelectedIndex = 0;
            }
        }

        public List<string> StateList
        {
            set
            {
                this.ddlStateFilter.DataSource = null;
                this.ddlStateFilter.DataSource = value;
                //this.ddlSalesRepresentativeFilter.DataTextField = "State";
                //this.ddlSalesRepresentativeFilter.DataValueField = "State";
                this.ddlStateFilter.DataBind();
                this.ddlStateFilter.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), "0"));
                //ddlStateFilter.SelectedIndex = 0;
            }
        }

        public List<string> OwnershipStructureList
        {
            set
            {
                this.ddlOwnershipStructureFilter.DataSource = null;
                this.ddlOwnershipStructureFilter.DataSource = value;
                //this.ddlSalesRepresentativeFilter.DataTextField = "OwnershipStructure";
                //this.ddlSalesRepresentativeFilter.DataValueField = "OwnershipStructure";
                this.ddlOwnershipStructureFilter.DataBind();
                this.ddlOwnershipStructureFilter.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), "0"));
                //ddlOwnershipStructureFilter.SelectedIndex = 0;
            }
        }

        public List<string> ManagementStructureList
        {
            set
            {
                this.ddlManagementStructureFilter.DataSource = null;
                this.ddlManagementStructureFilter.DataSource = value;
                //this.ddlSalesRepresentativeFilter.DataTextField = "ManagementStructure";
                //this.ddlSalesRepresentativeFilter.DataValueField = "ManagementStructure";
                this.ddlManagementStructureFilter.DataBind();
                this.ddlManagementStructureFilter.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), "0"));
                //ddlManagementStructureFilter.SelectedIndex = 0;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------


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
                this.ddlCategory.DataBind();

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
                this.ddlSubCategory1.DataBind();
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
                this.ddlSubCategory2.DataBind();
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
                this.ddlSubCategory3.DataBind();
                this.ddlSubCategory3.DataSource = value;
                this.ddlSubCategory3.DataBind();
                this.ddlSubCategory3.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), "0"));
                ddlSubCategory3.SelectedIndex = 0;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------------------


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

        #endregion
    }
}

