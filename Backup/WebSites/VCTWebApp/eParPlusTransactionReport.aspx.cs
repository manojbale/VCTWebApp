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
    public partial class eParPlusTransactionReport : Microsoft.Practices.CompositeWeb.Web.UI.Page, IeParPlusTransactionReport
    {
        #region Instance Variables
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private Security security = null;
        private eParPlusTransactionReportPresenter presenter;
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
                    PopulateTransaction();
                    txtEndDate.Text = DateTime.Now.ToString("d");
                    txtStartDate.Text = DateTime.Now.AddDays(-6).ToString("d");
                    btnExport.Visible = false;
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
        public eParPlusTransactionReportPresenter Presenter
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

        protected void gdvTransaction_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    GridView grdChild = e.Row.FindControl("grdChild") as GridView;
                    VCTWeb.Core.Domain.EPPTransaction objEPPTransaction = (VCTWeb.Core.Domain.EPPTransaction)e.Row.DataItem;

                    List<VCTWeb.Core.Domain.EPPTransaction> lstTransaction = new List<EPPTransaction>();
                    List<VCTWeb.Core.Domain.EPPTransaction> lstTemp = new List<EPPTransaction>();

                    lstTemp = lstEPPTransaction.FindAll(i => i.CustomerName == objEPPTransaction.CustomerName && i.AccountNumber == objEPPTransaction.AccountNumber);

                    var distinctEPPTransaction = lstTemp.Select(s => new
                    {
                        CustomerName = objEPPTransaction.CustomerName,
                        AccountNumber = objEPPTransaction.AccountNumber,
                        RefNum = s.RefNum,
                        PartDesc = s.PartDesc,
                        LotNum = s.LotNum,
                        TagId = s.TagId 
                    }).Distinct().ToList();


                    foreach (var transaction in distinctEPPTransaction)
                    {
                        lstTransaction.Add(new VCTWeb.Core.Domain.EPPTransaction()
                        {
                            CustomerName = transaction.CustomerName,
                            AccountNumber = transaction.AccountNumber,
                            RefNum = transaction.RefNum,
                            PartDesc = transaction.PartDesc,
                            LotNum = transaction.LotNum,
                            TagId = transaction.TagId,
                            Count = lstEPPTransaction.Where(w => w.CustomerName == transaction.CustomerName && w.AccountNumber == transaction.AccountNumber && w.TagId == transaction.TagId).Count()
                        });
                    }
                    grdChild.DataSource = lstTransaction;
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
                    GridView grdChild2 = e.Row.FindControl("grdChild2") as GridView;
                    VCTWeb.Core.Domain.EPPTransaction objEPPTransaction = (VCTWeb.Core.Domain.EPPTransaction)e.Row.DataItem;
                    lstEPPTransaction = Session["ListEPPTransaction"] as List<EPPTransaction>;
                    List<VCTWeb.Core.Domain.EPPTransaction> lstEPPTransactionSummary = new List<EPPTransaction>();
                    lstEPPTransactionSummary = lstEPPTransaction.FindAll(s => s.AccountNumber == objEPPTransaction.AccountNumber
                       && s.RefNum == objEPPTransaction.RefNum && s.TagId == objEPPTransaction.TagId);
                    grdChild2.DataSource = lstEPPTransactionSummary;
                    grdChild2.DataBind();
                }
            }
            catch
            {

            }

            try
            {
                //if (e.Row.RowType == DataControlRowType.DataRow)
                //{
                //    GridView grdChild2 = e.Row.FindControl("grdChild2") as GridView;
                //    VCTWeb.Core.Domain.EPPTransaction objEPPTransaction = (VCTWeb.Core.Domain.EPPTransaction)e.Row.DataItem;

                //    List<VCTWeb.Core.Domain.EPPTransaction> lstTransaction = new List<EPPTransaction>();

                //    lstTransaction = lstEPPTransaction.FindAll(i => i.CustomerName == objEPPTransaction.CustomerName && i.AccountNumber == objEPPTransaction.AccountNumber && i.RefNum == objEPPTransaction.RefNum);

                //    var distinctEPPTransaction = lstTransaction.Select(s => new
                //    {
                //        CustomerName = objEPPTransaction.CustomerName,
                //        AccountNumber = objEPPTransaction.AccountNumber,
                //        RefNum = objEPPTransaction.RefNum,
                //        PartDesc = s.PartDesc,
                //        TagId = s.TagId
                //    }).Distinct().ToList();

                //    lstTransaction = new List<EPPTransaction>();

                //    foreach (var transaction in distinctEPPTransaction)
                //    {
                //        lstTransaction.Add(new VCTWeb.Core.Domain.EPPTransaction()
                //        {
                //            CustomerName = transaction.CustomerName,
                //            AccountNumber = transaction.AccountNumber,
                //            RefNum = transaction.RefNum,
                //            PartDesc = transaction.PartDesc,
                //            TagId = transaction.TagId,
                //            Count = lstEPPTransaction.Where(w => w.CustomerName == transaction.CustomerName && w.AccountNumber == transaction.AccountNumber && w.TagId == transaction.TagId).Count()
                //        });
                //    }

                //    grdChild2.DataSource = lstTransaction;
                //    grdChild2.DataBind();
                //}
            }
            catch
            {

            }
        }

        protected void grdChild2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //try
            //{
            //    if (e.Row.RowType == DataControlRowType.DataRow)
            //    {
            //        GridView grdChild3 = e.Row.FindControl("grdChild3") as GridView;
            //        VCTWeb.Core.Domain.EPPTransaction objEPPTransaction = (VCTWeb.Core.Domain.EPPTransaction)e.Row.DataItem;
            //        lstEPPTransaction = Session["ListEPPTransaction"] as List<EPPTransaction>;
            //        List<VCTWeb.Core.Domain.EPPTransaction> lstEPPTransactionSummary = new List<EPPTransaction>();
            //        lstEPPTransactionSummary = lstEPPTransaction.FindAll(s => s.AccountNumber == objEPPTransaction.AccountNumber
            //           && s.RefNum == objEPPTransaction.RefNum && s.TagId == objEPPTransaction.TagId);                   
            //        grdChild3.DataSource = lstEPPTransactionSummary;
            //        grdChild3.DataBind();
            //    }
            //}
            //catch
            //{

            //}
        }
        
        protected void gdvTransaction_Sorting(object sender, GridViewSortEventArgs e)
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

            if (Session["ListEPPTransaction"] != null)
            {
                List<EPPTransaction> ListTransaction = Session["ListEPPTransaction"] as List<EPPTransaction>;
                if (sortingDirection == "Desc")
                {
                    if (e.SortExpression.Trim() == "CustomerName")
                        ListTransaction = ListTransaction.OrderByDescending(p => p.AccountNumber).ToList();
                    if (e.SortExpression.Trim() == "AccountNumber")
                        ListTransaction = ListTransaction.OrderByDescending(p => p.AccountNumber).ToList();
                }
                else
                {
                    if (e.SortExpression.Trim() == "CustomerName")
                        ListTransaction = ListTransaction.OrderBy(p => p.AccountNumber).ToList();
                    if (e.SortExpression.Trim() == "AccountNumber")
                        ListTransaction = ListTransaction.OrderBy(p => p.AccountNumber).ToList();
                }

                ListEPPTransaction = ListTransaction;

                int columnIndex = 0;
                foreach (DataControlFieldHeaderCell headerCell in gdvTransaction.HeaderRow.Cells)
                {
                    if (headerCell.ContainingField.SortExpression == e.SortExpression)
                    {
                        columnIndex = gdvTransaction.HeaderRow.Cells.GetCellIndex(headerCell);
                    }
                }
                gdvTransaction.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);
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


            string AccountNumber = string.Empty;
            string CustomerName = string.Empty;

            foreach (GridViewRow gvr in gdvChild.Rows)
            {
                Label lblAccountNumber = gvr.FindControl("lblAccountNumber") as Label;
                Label lblCustomerName = gvr.FindControl("lblCustomerName") as Label;
                if (lblAccountNumber != null && lblCustomerName != null)
                {
                    AccountNumber = lblAccountNumber.Text;
                    CustomerName = lblCustomerName.Text;
                    break;
                }
            }

            if (AccountNumber != string.Empty)
            {
                //List<EPPTransaction> listChild = new List<EPPTransaction>();
                //lstEPPTransaction = Session["ListEPPTransaction"] as List<EPPTransaction>;
                //var distinctEPPTransaction = lstEPPTransaction.Select(s => new
                //{
                //    CustomerName = CustomerName,
                //    AccountNumber = AccountNumber,
                //    RefNum = s.RefNum,
                //    PartDesc = s.PartDesc
                //}).Distinct().ToList();


                //foreach (var transaction in distinctEPPTransaction)
                //{
                //    listChild.Add(new VCTWeb.Core.Domain.EPPTransaction()
                //    {
                //        CustomerName = transaction.CustomerName,
                //        AccountNumber = transaction.AccountNumber,
                //        RefNum = transaction.RefNum,
                //        PartDesc = transaction.PartDesc
                //    });
                //}


                List<VCTWeb.Core.Domain.EPPTransaction> lstTransaction = new List<EPPTransaction>();
                List<VCTWeb.Core.Domain.EPPTransaction> lstTemp = new List<EPPTransaction>();
                lstEPPTransaction = Session["ListEPPTransaction"] as List<EPPTransaction>;
                lstTemp = lstEPPTransaction.FindAll(i => i.CustomerName == CustomerName && i.AccountNumber == AccountNumber);

                var distinctEPPTransaction = lstTemp.Select(s => new
                {
                    CustomerName = CustomerName,
                    AccountNumber = AccountNumber,
                    RefNum = s.RefNum,
                    PartDesc = s.PartDesc,
                    LotNum = s.LotNum,
                    TagId = s.TagId
                }).Distinct().ToList();


                foreach (var transaction in distinctEPPTransaction)
                {
                    lstTransaction.Add(new VCTWeb.Core.Domain.EPPTransaction()
                    {
                        CustomerName = transaction.CustomerName,
                        AccountNumber = transaction.AccountNumber,
                        RefNum = transaction.RefNum,
                        PartDesc = transaction.PartDesc,
                        LotNum = transaction.LotNum,
                        TagId = transaction.TagId,
                        Count = lstEPPTransaction.Where(w => w.CustomerName == transaction.CustomerName && w.AccountNumber == transaction.AccountNumber && w.TagId == transaction.TagId).Count()
                    });
                }
               
                if (sortingDirection == "Desc")
                {
                    if (e.SortExpression.Trim() == "RefNum")
                        lstTransaction = lstTransaction.OrderByDescending(p => p.RefNum).ToList();

                    if (e.SortExpression.Trim() == "PartDesc")
                        lstTransaction = lstTransaction.OrderByDescending(p => p.PartDesc).ToList();

                    if (e.SortExpression.Trim() == "LotNum")
                        lstTransaction = lstTransaction.OrderByDescending(p => p.LotNum).ToList();

                    if (e.SortExpression.Trim() == "TagId")
                        lstTransaction = lstTransaction.OrderByDescending(p => p.TagId).ToList();

                    if (e.SortExpression.Trim() == "Count")
                        lstTransaction = lstTransaction.OrderByDescending(p => p.Count).ToList();
                }
                else
                {
                    if (e.SortExpression.Trim() == "RefNum")
                        lstTransaction = lstTransaction.OrderBy(p => p.RefNum).ToList();

                    if (e.SortExpression.Trim() == "PartDesc")
                        lstTransaction = lstTransaction.OrderBy(p => p.PartDesc).ToList();

                    if (e.SortExpression.Trim() == "LotNum")
                        lstTransaction = lstTransaction.OrderBy(p => p.LotNum).ToList();

                    if (e.SortExpression.Trim() == "TagId")
                        lstTransaction = lstTransaction.OrderBy(p => p.TagId).ToList();

                    if (e.SortExpression.Trim() == "Count")
                        lstTransaction = lstTransaction.OrderBy(p => p.Count).ToList();
                }


                //gdvChild.DataSource = listChild;
                gdvChild.RowDataBound += grdChild_RowDataBound;
                gdvChild.DataSource = lstTransaction;
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

        protected void grdChild2_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridView gdvChild2 = (GridView)sender;

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


            string AccountNumber = string.Empty;
            string RefNum = string.Empty;
            string CustomerName = string.Empty;

            foreach (GridViewRow gvr in gdvChild2.Rows)
            {
                Label lblAccountNumber = gvr.FindControl("lblAccountNumber") as Label;
                Label lblRefNum = gvr.FindControl("lblRefNum") as Label;
                //Label lblCustomerName = gvr.FindControl("lblCustomerName") as Label;
                if (lblAccountNumber != null && lblRefNum != null)
                {
                    AccountNumber = lblAccountNumber.Text;
                    RefNum = lblRefNum.Text;
                    break;
                }
            }

            if (AccountNumber != string.Empty && RefNum != string.Empty)
            {
                lstEPPTransaction = Session["ListEPPTransaction"] as List<EPPTransaction>;
                List<VCTWeb.Core.Domain.EPPTransaction> lstTransaction = new List<EPPTransaction>();
                lstTransaction = lstEPPTransaction.FindAll(i => i.AccountNumber == AccountNumber && i.RefNum == RefNum);
                var distinctEPPTransaction = lstTransaction.Select(s => new
                {
                    AccountNumber = AccountNumber,
                    RefNum = RefNum,
                    PartDesc = s.PartDesc,
                    TagId = s.TagId
                }).Distinct().ToList();

                lstTransaction = new List<EPPTransaction>();
                foreach (var transaction in distinctEPPTransaction)
                {
                    lstTransaction.Add(new VCTWeb.Core.Domain.EPPTransaction()
                    {
                        AccountNumber = transaction.AccountNumber,
                        RefNum = transaction.RefNum,
                        PartDesc = transaction.PartDesc,
                        TagId = transaction.TagId,
                        Count = lstEPPTransaction.Where(w => w.AccountNumber == transaction.AccountNumber && w.TagId == transaction.TagId).Count()
                    });
                }
                if (sortingDirection == "Desc")
                {
                    if (e.SortExpression.Trim() == "TagId")
                        lstTransaction = lstTransaction.OrderByDescending(p => p.TagId).ToList();

                    if (e.SortExpression.Trim() == "Count")
                        lstTransaction = lstTransaction.OrderByDescending(p => p.Count).ToList();
                }
                else
                {
                    if (e.SortExpression.Trim() == "TagId")
                        lstTransaction = lstTransaction.OrderBy(p => p.TagId).ToList();

                    if (e.SortExpression.Trim() == "Count")
                        lstTransaction = lstTransaction.OrderBy(p => p.Count).ToList();
                }

                gdvChild2.RowDataBound += grdChild2_RowDataBound;
                gdvChild2.DataSource = lstTransaction;
                gdvChild2.DataBind();
                int columnIndex = 0;
                foreach (DataControlFieldHeaderCell headerCell in gdvChild2.HeaderRow.Cells)
                {
                    if (headerCell.ContainingField.SortExpression == e.SortExpression)
                    {
                        columnIndex = gdvChild2.HeaderRow.Cells.GetCellIndex(headerCell);
                        gdvChild2.HeaderRow.Cells[columnIndex].Controls.Add(sortImage);
                    }
                }
            }
        }


        protected void rblPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gdvTransaction.DataSource = null;
                gdvTransaction.DataBind();
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

                ddlTransaction.SelectedIndex = 0;

                rblPeriod.SelectedIndex = 0;
                imgCalenderFrom.Visible = false;
                Image1.Visible = false;
                lblError.Text = string.Empty;

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

        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        private void ExportInExcel()
        {
            grdViewExport.DataSource = null;
            grdViewExport.DataBind();
            List<EPPTransaction> lstReportData = presenter.PopulateReport();
            if (lstReportData != null && lstReportData.Count > 0)
            {
                DataTable dtReportTable = new DataTable();
                dtReportTable.Columns.Add("Customer Name", typeof(string));
                dtReportTable.Columns.Add("Account Number", typeof(string));
                dtReportTable.Columns.Add("Ref #", typeof(string));
                dtReportTable.Columns.Add("Description", typeof(string));
                dtReportTable.Columns.Add("Lot Num", typeof(string));
                dtReportTable.Columns.Add("RFID Tag Id", typeof(string));
                dtReportTable.Columns.Add("Transaction", typeof(string));
                dtReportTable.Columns.Add("Updated On", typeof(string));


                foreach (VCTWeb.Core.Domain.EPPTransaction item in lstReportData)
                {
                    DataRow dr;
                    dr = dtReportTable.NewRow();
                    dr["Customer Name"] = item.CustomerName != null ? item.CustomerName.ToString() : string.Empty;
                    dr["Account Number"] = item.AccountNumber != null ? item.AccountNumber.ToString() : string.Empty;
                    dr["Ref #"] = item.RefNum != null ? item.RefNum.ToString() : string.Empty;
                    dr["Description"] = item.PartDesc != null ? item.PartDesc.ToString() : string.Empty;
                    dr["Lot Num"] = item.LotNum != null ? item.LotNum.ToString() : string.Empty;
                    dr["RFID Tag Id"] = item.TagId != null ? item.TagId.ToString() : string.Empty;
                    dr["Transaction"] = item.StatusDescription != null ? item.StatusDescription.ToString() : string.Empty;
                    dr["Updated On"] = item.UpdatedOn.ToString("MM/dd/yyyy hh:mm:ss tt");
                    dtReportTable.Rows.Add(dr);
                }

                grdViewExport.DataSource = dtReportTable;
                grdViewExport.DataBind();

                string filename = "TransactionReport_" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString()
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

        private void SetFieldsBlank()
        {
            gdvTransaction.DataSource = null;
            gdvTransaction.DataBind();
        }

        List<VCTWeb.Core.Domain.EPPTransaction> lstEPPTransaction = new List<EPPTransaction>();

        private void PopulateReport()
        {
            //if (ddlLocation.SelectedIndex != 0)
            //{
            //    List<VCTWeb.Core.Domain.EPPTransaction> lstEPPTransactionSummary = new List<VCTWeb.Core.Domain.EPPTransaction>();
            //    lstEPPTransaction = new eParPlusRepository().FetchTransactionReportByParentLocationId(Convert.ToInt32(ddlLocation.SelectedValue), Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text), ddlTransaction.SelectedValue);


            //    var distinctLocationStatus = lstEPPTransaction.Select(s => new { LocationName = s.LocationName, ItemStatus = s.ItemStatus, StatusDescription = s.StatusDescription }).Distinct().ToList();
            //    foreach (var locationStatus in distinctLocationStatus)
            //    {
            //        lstEPPTransactionSummary.Add(new VCTWeb.Core.Domain.EPPTransaction()
            //        {
            //            LocationName = locationStatus.LocationName,
            //            ItemStatus = locationStatus.ItemStatus,
            //            StatusDescription = locationStatus.StatusDescription,
            //            Count = lstEPPTransaction.Where(w => w.LocationName == locationStatus.LocationName && w.ItemStatus == locationStatus.ItemStatus).Count()
            //        });
            //        //location, lstEPPTransaction.Where(w => w.LocationName == location).Sum(s => s.PARLevelQty), lstEPPTransaction.Where(w => w.LocationName == location).Sum(s => s.ConsumedQty)));
            //    }
            //    gdvTransaction.DataSource = lstEPPTransactionSummary;
            //    gdvTransaction.DataBind();
            //}
        }


        private void PopulateTransaction()
        {
            List<ItemStatus> lstItemStatus = new EParPlusRepository().FetchAllEppItemStatus().Where(i => i.IsExceptionalStatus == false).ToList();
            ddlTransaction.DataSource = lstItemStatus;
            ddlTransaction.DataTextField = "StatusDescription";
            ddlTransaction.DataValueField = "ItemStatusCode";
            ddlTransaction.DataBind();
            //ddlLocation.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll")));
            ddlTransaction.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), string.Empty));
        }

        #endregion

        #region IeParPlusTransactionReport Members

        public List<EPPTransaction> ListEPPTransaction
        {
            set
            {
                lblError.Text = "";
                gdvTransaction.DataSource = null;
                gdvTransaction.DataBind();
                btnExport.Visible = ((value != null && value.Count > 0) ? true : false);
                if (value != null)
                {
                    List<VCTWeb.Core.Domain.EPPTransaction> lstEPPTransactionSummary = new List<VCTWeb.Core.Domain.EPPTransaction>();
                    lstEPPTransaction = value;
                    var distinctCustomer = lstEPPTransaction.Select(s => new { CustomerName = s.CustomerName, AccountNumber = s.AccountNumber }).Distinct().ToList();
                    foreach (var customer in distinctCustomer)
                    {
                        lstEPPTransactionSummary.Add(new VCTWeb.Core.Domain.EPPTransaction()
                        {
                            CustomerName = customer.CustomerName,
                            AccountNumber = customer.AccountNumber
                        });
                    }
                    Session["ListEPPTransaction"] = value;
                    gdvTransaction.DataSource = lstEPPTransactionSummary;
                    gdvTransaction.DataBind();
                    lblError.Text = (value.Count <= 0 ? "No Record Found." : "");
                }
            }
        }

        List<VCTWeb.Core.Domain.Customer> IeParPlusTransactionReport.CustomerNameList
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

        public string SelectedItemStatus
        {
            get
            {
                if (this.ddlTransaction.SelectedIndex > 0)
                    return Convert.ToString(this.ddlTransaction.SelectedValue);
                else
                    return string.Empty;
            }
        }

        #endregion
    }
}

