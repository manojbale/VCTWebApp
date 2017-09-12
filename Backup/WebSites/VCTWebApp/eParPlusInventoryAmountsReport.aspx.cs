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
    public partial class eParPlusInventoryAmountsReport : Microsoft.Practices.CompositeWeb.Web.UI.Page, IeParPlusInventoryAmountsReport
    {
        #region Instance Variables
        private readonly VCTWebAppResource vctResource = new VCTWebAppResource();
        private Security _security = null;
        private EParPlusInventoryAmountsPresenter _presenter;
        List<InventoryAmount> _lstInventoryAmount = new List<InventoryAmount>();
        #endregion

        #region Private Properties

        private bool CanConsume
        {
            get
            {
                return ViewState[Common.CAN_CONSUME] != null ? (bool)ViewState[Common.CAN_CONSUME] : false;
            }
            set
            {
                ViewState[Common.CAN_CONSUME] = value;
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
                if (!IsPostBack)
                {
                    AuthorizedPage();
                    _presenter.OnViewInitialized();
                    PopulateTransaction();
                    btnExport.Visible = false;
                    btnSendEmail.Visible = false;
                }
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
        public EParPlusInventoryAmountsPresenter Presenter
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

        protected void UpdateTimer_Tick(object sender, EventArgs e)
        {
            string message = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(SelectedCustomerAccountFilter) && Session["ManualScanInitiatedAt"] != null)
                {
                    var manaulScanCompletionInternval = Convert.ToInt16(vctResource.GetString("ManaulScanCompletionInternval")) * Convert.ToInt32(Session["ManualScanNumberOfShelves"]);
                    if (_presenter.IsManaulScanCompleted(SelectedCustomerAccountFilter, Convert.ToDateTime(Session["ManualScanInitiatedAt"])))
                        message = vctResource.GetString("ManualScanSuccessMessage");
                    else if (DateTime.UtcNow.Subtract(Convert.ToDateTime(Session["ManualScanInitiatedAt"])).TotalSeconds > manaulScanCompletionInternval)
                        message = vctResource.GetString("ManualScanErrorMessage");
                    else
                        lblError.Text = string.Format(vctResource.GetString("ManualScanRunningMessage"), ddlCustomerNameFilter.SelectedItem.Text);
                }
                else
                {
                    message = vctResource.GetString("ManualScanErrorMessage");
                }
            }
            catch
            {
                message = vctResource.GetString("ManualScanErrorMessage");
            }

            if (!string.IsNullOrEmpty(message))
            {
                tmrUpdateTimer.Enabled = false;
                imgInitiateManualScan.ImageUrl = "~/Images/RFID.png";
                lblError.Text = message;
            }
        }

        protected void chkNearExpiryDayFilter_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkNearExpiryDayFilter.Checked)
                {
                    txtNearExpiryDay.Enabled = true;
                    //Fetch Dafault Near Expiry Value and fill in text box.
                }
                else
                {
                    txtNearExpiryDay.Enabled = false;
                    txtNearExpiryDay.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {

            }
        }

        #region GridView Events

        protected void gdvInventoryAmount_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    var grdChild = e.Row.FindControl("grdChild") as GridView;
                    _lstInventoryAmount = Session["ListInventoryAmount"] as List<InventoryAmount>;
                    var objInventoryAmount = (InventoryAmount)e.Row.DataItem;
                    var lstInventoryAmountSummary = new List<InventoryAmount>();
                    if (_lstInventoryAmount != null)
                    {
                        var distinctCutomerData = _lstInventoryAmount.FindAll(s => s.CustomerName == objInventoryAmount.CustomerName && s.AccountNumber == objInventoryAmount.AccountNumber);
                        var lstRefNum = distinctCutomerData.Select(i => i.RefNum).Distinct();

                        foreach (var refNum in lstRefNum)
                        {
                            var inventoryAmount = new InventoryAmount();
                            inventoryAmount.CustomerName = objInventoryAmount.CustomerName;
                            inventoryAmount.AccountNumber = objInventoryAmount.AccountNumber;
                            inventoryAmount.RefNum = refNum;
                            inventoryAmount.LastScanned = distinctCutomerData.First(f => f.CustomerName == objInventoryAmount.CustomerName && f.RefNum == refNum).LastScanned;
                            inventoryAmount.PartDesc = distinctCutomerData.First(f => f.CustomerName == objInventoryAmount.CustomerName && f.RefNum == refNum).PartDesc;
                            inventoryAmount.IsNearExpiry = distinctCutomerData.Any(a => a.CustomerName == objInventoryAmount.CustomerName && a.RefNum == refNum && a.IsNearExpiry == true);
                            inventoryAmount.Qty = distinctCutomerData.Count(w => w.CustomerName == objInventoryAmount.CustomerName && w.RefNum == refNum);
                            //inventoryAmount.Qty = distinctCutomerData.Where(w => w.CustomerName == objInventoryAmount.CustomerName && w.RefNum == refNum).Count();
                            inventoryAmount.PARLevelQty = distinctCutomerData.First(f => f.CustomerName == objInventoryAmount.CustomerName && f.RefNum == refNum).PARLevelQty;
                            var offcartQty = distinctCutomerData.Count(w => w.CustomerName == objInventoryAmount.CustomerName && w.RefNum == refNum && w.ItemStatus == "OFFCART");
                            //var offcartQty = distinctCutomerData.Where(w => w.CustomerName == objInventoryAmount.CustomerName && w.RefNum == refNum && w.ItemStatus == "OFFCART").Count();
                            inventoryAmount.OffCartQty = offcartQty;
                            lstInventoryAmountSummary.Add(inventoryAmount);
                        }
                    }
                    if (grdChild != null)
                    {
                        grdChild.DataSource = lstInventoryAmountSummary;
                        grdChild.DataBind();
                    }
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
                    VCTWeb.Core.Domain.InventoryAmount objInventoryAmount = (VCTWeb.Core.Domain.InventoryAmount)e.Row.DataItem;

                    List<VCTWeb.Core.Domain.InventoryAmount> lstInventoryAmountSummary = new List<InventoryAmount>();


                    var distinctInventoryAmount = _lstInventoryAmount.FindAll(X => X.CustomerName == objInventoryAmount.CustomerName &&
                        X.AccountNumber == objInventoryAmount.AccountNumber &&
                        X.RefNum == objInventoryAmount.RefNum);

                    int AssetNearExpiryDays = 0;
                    foreach (var inventoryAmount in distinctInventoryAmount)
                    {
                        AssetNearExpiryDays = inventoryAmount.AssetNearExpiryDays;
                        lstInventoryAmountSummary.Add(new VCTWeb.Core.Domain.InventoryAmount()
                        {
                            CustomerName = inventoryAmount.CustomerName,
                            AccountNumber = inventoryAmount.AccountNumber,
                            RefNum = inventoryAmount.RefNum,
                            LastScanned = inventoryAmount.LastScanned,
                            PartDesc = inventoryAmount.PartDesc,
                            IsNearExpiry = inventoryAmount.IsNearExpiry,
                            TagId = inventoryAmount.TagId,
                            LotNum = inventoryAmount.LotNum,
                            ExpiryDt = inventoryAmount.ExpiryDt,
                            ItemStatusDescription = inventoryAmount.ItemStatusDescription,
                            IsManuallyConsumed = inventoryAmount.IsManuallyConsumed
                        });
                    }

                    //if (chkNearExpiryDayFilter.Checked)                    
                    //    AssetNearExpiryDays = Convert.ToInt32(txtNearExpiryDay.Text.Trim());

                    grdChild2.DataSource = lstInventoryAmountSummary;
                    grdChild2.DataBind();
                    grdChild2.HeaderRow.Cells[4].Text = "Expiry Date<br/>(Near Expiry : " + Convert.ToString(AssetNearExpiryDays) + " Days)";
                    if (objInventoryAmount.IsNearExpiry)
                        e.Row.ForeColor = System.Drawing.Color.Red;

                    grdChild2.Columns[5].Visible = CanConsume;
                }
            }
            catch
            {

            }
        }

        protected void grdChild2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    VCTWeb.Core.Domain.InventoryAmount objInventoryAmount = (VCTWeb.Core.Domain.InventoryAmount)e.Row.DataItem;
                    if (objInventoryAmount.IsNearExpiry)
                    {
                        e.Row.ForeColor = System.Drawing.Color.Red;
                    }
                    LinkButton linkButton = e.Row.FindControl("lnkConsumed") as LinkButton;
                    linkButton.Visible = true;
                    //if (objInventoryAmount.ItemStatusDescription == "On-Cart")
                    //    linkButton.Visible = true;
                    //else
                    //    linkButton.Visible = false;
                    //if (objInventoryAmount.IsManuallyConsumed)
                    //{
                    //    LinkButton linkButton = e.Row.FindControl("lnkConsumed") as LinkButton;
                    //    linkButton.Enabled = false;
                    //    linkButton.OnClientClick = "";
                    //    Image imgConsumed = e.Row.FindControl("imgConsumed") as Image;
                    //    imgConsumed.ImageUrl = "~/Images/thumbsup.png";
                    //    imgConsumed.ToolTip = "Item already markes as Manually consumed.";
                    //}
                }
            }
            catch
            {

            }
        }

        protected void grdChild2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("ConsumedRec"))
                {
                    var tagId = Convert.ToString(e.CommandArgument);
                    var row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);

                    var lblAccountNumber = row.Cells[0].FindControl("lblAccountNumber") as Label;
                    if (lblAccountNumber != null)
                    {
                        var accountNumber = lblAccountNumber.Text.Trim();
                        if (!string.IsNullOrEmpty(tagId))
                        {
                            if (Presenter.SaveManualConsumption(accountNumber, tagId))
                            {
                                lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgPARLevelDelete"), lblHeader.Text) + "</font>";
                                btnFilterData_Click(sender, e);
                            }
                        }
                    }
                }

                try
                {
                    if (e.CommandName.Equals("TagHistoryClick"))
                    {
                        string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
                        ucTagHistoryPopUp.PopulateData(Convert.ToString(commandArgs[0]), Convert.ToString(commandArgs[1]));
                        mpeKitDetail.Show();
                    }
                }
                catch (Exception ex)
                {
                    //lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), lblHeader.Text);
                }
            }
            catch (Exception ex)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), lblHeader.Text);
            }
        }

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

            if (Session["ListInventoryAmount"] != null)
            {
                List<InventoryAmount> listInventoryAmount = Session["ListInventoryAmount"] as List<InventoryAmount>;
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

                ListInventoryAmount = listInventoryAmount;

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


            if (AccountNumber != string.Empty && CustomerName != string.Empty)
            {
                List<InventoryAmount> listLowInventoryChild = new List<InventoryAmount>();
                _lstInventoryAmount = Session["ListInventoryAmount"] as List<InventoryAmount>;
                var distinctLocationPart = _lstInventoryAmount.Select(s => new { CustomerName = CustomerName, AccountNumber = AccountNumber, RefNum = s.RefNum, PARLevelQty = s.PARLevelQty }).Distinct().ToList();

                foreach (var locationStatus in distinctLocationPart)
                {
                    listLowInventoryChild.Add(new VCTWeb.Core.Domain.InventoryAmount()
                    {
                        CustomerName = locationStatus.CustomerName,
                        AccountNumber = locationStatus.AccountNumber,
                        RefNum = locationStatus.RefNum,
                        LastScanned = _lstInventoryAmount.First(f => f.CustomerName == locationStatus.CustomerName && f.RefNum == locationStatus.RefNum).LastScanned,
                        PartDesc = _lstInventoryAmount.First(f => f.CustomerName == locationStatus.CustomerName && f.RefNum == locationStatus.RefNum).PartDesc,
                        IsNearExpiry = _lstInventoryAmount.Any(a => a.CustomerName == locationStatus.CustomerName && a.RefNum == locationStatus.RefNum && a.IsNearExpiry == true),
                        Qty = _lstInventoryAmount.Where(w => w.CustomerName == locationStatus.CustomerName && w.RefNum == locationStatus.RefNum).Count(),
                        PARLevelQty = locationStatus.PARLevelQty
                    });
                }

                if (sortingDirection == "Desc")
                {
                    if (e.SortExpression.Trim() == "RefNum")
                        listLowInventoryChild = listLowInventoryChild.OrderByDescending(p => p.RefNum).ToList();

                    if (e.SortExpression.Trim() == "PartDesc")
                        listLowInventoryChild = listLowInventoryChild.OrderByDescending(p => p.PartDesc).ToList();

                    if (e.SortExpression.Trim() == "LastScanned")
                        listLowInventoryChild = listLowInventoryChild.OrderByDescending(p => p.LastScanned).ToList();

                    if (e.SortExpression.Trim() == "Qty")
                        listLowInventoryChild = listLowInventoryChild.OrderByDescending(p => p.Qty).ToList();

                    if (e.SortExpression.Trim() == "PARLevelQty")
                        listLowInventoryChild = listLowInventoryChild.OrderByDescending(p => p.PARLevelQty).ToList();
                }
                else
                {
                    if (e.SortExpression.Trim() == "RefNum")
                        listLowInventoryChild = listLowInventoryChild.OrderBy(p => p.RefNum).ToList();

                    if (e.SortExpression.Trim() == "PartDesc")
                        listLowInventoryChild = listLowInventoryChild.OrderBy(p => p.PartDesc).ToList();

                    if (e.SortExpression.Trim() == "LastScanned")
                        listLowInventoryChild = listLowInventoryChild.OrderBy(p => p.LastScanned).ToList();

                    if (e.SortExpression.Trim() == "Qty")
                        listLowInventoryChild = listLowInventoryChild.OrderBy(p => p.Qty).ToList();

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

            foreach (GridViewRow gvr in gdvChild2.Rows)
            {
                Label lblAccountNumber = gvr.FindControl("lblAccountNumber") as Label;
                Label lblRefNum = gvr.FindControl("lblRefNum") as Label;
                if (lblAccountNumber != null && lblRefNum != null)
                {
                    AccountNumber = lblAccountNumber.Text;
                    RefNum = lblRefNum.Text;
                    break;
                }
            }

            if (AccountNumber != string.Empty && RefNum != string.Empty)
            {
                //Sort the data.
                List<InventoryAmount> listLowInventory = Session["ListInventoryAmount"] as List<InventoryAmount>;
                List<InventoryAmount> listLowInventoryChild = new List<InventoryAmount>();
                listLowInventoryChild = listLowInventory;

                listLowInventoryChild = listLowInventory.FindAll(i => i.AccountNumber == AccountNumber && i.RefNum == RefNum);

                if (sortingDirection == "Desc")
                {
                    if (e.SortExpression.Trim() == "LotNum")
                        listLowInventoryChild = listLowInventoryChild.OrderByDescending(p => p.LotNum).ToList();

                    if (e.SortExpression.Trim() == "TagId")
                        listLowInventoryChild = listLowInventoryChild.OrderByDescending(p => p.TagId).ToList();

                    if (e.SortExpression.Trim() == "ItemStatus")
                        listLowInventoryChild = listLowInventoryChild.OrderByDescending(p => p.ItemStatusDescription).ToList();

                    if (e.SortExpression.Trim() == "ExpiryDt")
                        listLowInventoryChild = listLowInventoryChild.OrderByDescending(p => p.ExpiryDt).ToList();
                }
                else
                {
                    if (e.SortExpression.Trim() == "LotNum")
                        listLowInventoryChild = listLowInventoryChild.OrderBy(p => p.LotNum).ToList();

                    if (e.SortExpression.Trim() == "TagId")
                        listLowInventoryChild = listLowInventoryChild.OrderBy(p => p.TagId).ToList();

                    if (e.SortExpression.Trim() == "ItemStatus")
                        listLowInventoryChild = listLowInventoryChild.OrderBy(p => p.ItemStatusDescription).ToList();

                    if (e.SortExpression.Trim() == "ExpiryDt")
                        listLowInventoryChild = listLowInventoryChild.OrderBy(p => p.ExpiryDt).ToList();

                }

                gdvChild2.DataSource = listLowInventoryChild;
                gdvChild2.DataBind();

                //ListInventoryAmount = listLowInventoryChild;

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

        #endregion GridView Events

        #region Dropdown Events

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
                if (ddlBranchAgencyFilter.SelectedIndex > 0)
                {
                    _presenter.FillDropdowns("BranchAgency", Convert.ToString(ddlBranchAgencyFilter.SelectedValue));
                }
                else
                {
                    _presenter.FillDropdowns("BranchAgency", Convert.ToString(0));
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
                    _presenter.FillDropdowns("Manager", Convert.ToString(ddlManagerFilter.SelectedValue));
                }
                else
                {
                    _presenter.FillDropdowns("Manager", Convert.ToString(0));
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
                    _presenter.FillDropdowns("SalesRepresentative", Convert.ToString(ddlSalesRepresentativeFilter.SelectedValue));
                }
                else
                {
                    _presenter.FillDropdowns("SalesRepresentative", Convert.ToString(0));
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
                    _presenter.FillDropdowns("State", Convert.ToString(ddlStateFilter.SelectedValue));
                }
                else
                {
                    _presenter.FillDropdowns("State", Convert.ToString(0));
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
                    _presenter.FillDropdowns("OwnershipStructure", Convert.ToString(ddlOwnershipStructureFilter.SelectedValue));
                }
                else
                {
                    _presenter.FillDropdowns("OwnershipStructure", Convert.ToString(0));
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
                    _presenter.FillDropdowns("ManagementStructure", Convert.ToString(ddlManagementStructureFilter.SelectedValue));
                }
                else
                {
                    _presenter.FillDropdowns("ManagementStructure", Convert.ToString(0));
                }
            }
            catch { }
        }

        #endregion Dropdown Events

        #region Button Events

        protected void btnInitiateManualScan_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = string.Empty;

                if (string.IsNullOrEmpty(SelectedCustomerAccountFilter))
                {
                    lblError.Text = vctResource.GetString("ManualScanCustomerValidation");
                }
                else
                {
                    var numberOfShelves = _presenter.InitiateEppManualScan(SelectedCustomerAccountFilter);
                    lblError.Text = "Manual scan Initiated for Customer : '" + ddlCustomerNameFilter.SelectedItem.Text + "'";
                    Session["ManualScanInitiatedAt"] = DateTime.UtcNow;
                    Session["ManualScanNumberOfShelves"] = numberOfShelves;
                    imgInitiateManualScan.ImageUrl = "~/Images/processing_image.gif";
                    tmrUpdateTimer.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
        }

        protected void btnFilterData_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = string.Empty;
                gdvInventoryAmount.DataSource = null;
                gdvInventoryAmount.DataBind();

                if (chkNearExpiryDayFilter.Checked)
                {
                    if (string.IsNullOrEmpty(txtNearExpiryDay.Text.Trim()))
                    {
                        lblError.Text = "<font color='red'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valEnterNearExpiryDays"), lblHeader.Text) + "</font>";
                        txtNearExpiryDay.Focus();
                        return;
                    }
                }
                _presenter.PopulateReport();
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = string.Empty;
                _presenter.OnViewInitialized();
                chkNearExpiryDayFilter.Checked = false;
                txtNearExpiryDay.Text = string.Empty;
                txtNearExpiryDay.Enabled = false;
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

        protected void btnSendEmail_Click(object sender, EventArgs e)
        {
            try
            {
                bool isEmailSent = _presenter.SendInventoryAmountNotificationEmail();
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

        #endregion Button Events

        #endregion

        #region Private Methods

        private void AuthorizedPage()
        {
            _security = new Security();
            if (Session["LoggedInUser"] == null)
            {
                Response.Redirect("~/ErrorPage.aspx?ErrorKey=Common_msgSessionExpired");
            }
            else if (_security.HasAccess("ePar+.InventoryAmountsReport"))
            {
                CanConsume = _security.HasPermission("ePar+.InventoryAmountsReport.ManualConsumption");
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
            var lstReportData = _presenter.PopulateReport();
            if (lstReportData != null && lstReportData.Count > 0)
            {
                var dtReportTable = new DataTable();
                dtReportTable.Columns.Add("Customer Name", typeof(string));
                dtReportTable.Columns.Add("Account Number", typeof(string));

                dtReportTable.Columns.Add("Ref #", typeof(string));
                dtReportTable.Columns.Add("Description", typeof(string));
                dtReportTable.Columns.Add("Last Scanned", typeof(string));

                //dtReportTable.Columns.Add("Quantity", typeof(string));
                dtReportTable.Columns.Add("PAR Level Qty", typeof(string));

                dtReportTable.Columns.Add("Lot Num", typeof(string));
                dtReportTable.Columns.Add("RFID Tag Id", typeof(string));
                dtReportTable.Columns.Add("Status", typeof(string));
                dtReportTable.Columns.Add("Expiry Date", typeof(string));


                foreach (var item in lstReportData)
                {
                    var dr = dtReportTable.NewRow();
                    dr["Customer Name"] = item.CustomerName ?? string.Empty;
                    dr["Account Number"] = item.AccountNumber ?? string.Empty;

                    dr["Ref #"] = item.RefNum ?? string.Empty;
                    dr["Description"] = item.PartDesc ?? string.Empty;
                    dr["Last Scanned"] = item.LastScanned.ToString("MM/dd/yyyy hh:mm:ss tt");

                    //dr["Quantity"] = item.Qty.ToString();
                    dr["PAR Level Qty"] = Convert.ToString(item.PARLevelQty);

                    dr["Lot Num"] = item.LotNum ?? string.Empty;
                    dr["RFID Tag Id"] = item.TagId ?? string.Empty;
                    dr["Status"] = item.ItemStatusDescription ?? string.Empty;
                    dr["Expiry Date"] = item.ExpiryDt.ToString("MM/dd/yyyy");

                    dtReportTable.Rows.Add(dr);
                }

                grdViewExport.DataSource = dtReportTable;
                grdViewExport.DataBind();

                string filename = "InventoryAmountReport_" + DateTime.Now.Month.ToString() + DateTime.Now.Day + DateTime.Now.Year
                                    + "_" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute + DateTime.Now.Second + ".xls";

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

        private void PopulateTransaction()
        {
            var lstItemStatus = new EParPlusRepository().FetchAllEppItemStatus("ONCART,OFFCART").Where(i => i.IsExceptionalStatus == false).ToList();
            ddlTransaction.DataSource = lstItemStatus;
            ddlTransaction.DataTextField = "StatusDescription";
            ddlTransaction.DataValueField = "ItemStatusCode";
            ddlTransaction.DataBind();
            ddlTransaction.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), string.Empty));
        }

        #endregion

        #region IeParPlusInventoryAmountsReport Members

        public List<InventoryAmount> ListInventoryAmount
        {
            set
            {
                lblError.Text = "";
                gdvInventoryAmount.DataSource = null;
                gdvInventoryAmount.DataBind();
                if (value == null) return;
                _lstInventoryAmount = value;
                if (_lstInventoryAmount.Count > 0)
                {
                    btnExport.Visible = true;
                    btnSendEmail.Visible = true;
                }
                var lstInventoryAmountSummary = new List<InventoryAmount>();
                var distinctLocationPart = _lstInventoryAmount.Select(s => new { s.CustomerName, s.AccountNumber }).Distinct().ToList();
                foreach (var locationStatus in distinctLocationPart)
                {
                    lstInventoryAmountSummary.Add(new InventoryAmount()
                    {
                        CustomerName = locationStatus.CustomerName,
                        AccountNumber = locationStatus.AccountNumber
                    });
                }
                Session["ListInventoryAmount"] = value;
                gdvInventoryAmount.DataSource = lstInventoryAmountSummary;
                gdvInventoryAmount.DataBind();
                lblError.Text = (value.Count <= 0 ? "No Record Found." : "");
            }
            get
            {
                return Session["ListInventoryAmount"] as List<InventoryAmount>;
            }
        }

        public int ExpirationDaysFilter
        {
            get
            {
                var expirationDays = 0;
                if (chkNearExpiryDayFilter.Checked && !string.IsNullOrEmpty(txtNearExpiryDay.Text.Trim()))
                {
                    try
                    {
                        expirationDays = Convert.ToInt32(txtNearExpiryDay.Text.Trim());
                    }
                    catch { expirationDays = 0; }
                }
                return expirationDays;
            }
        }

        List<VCTWeb.Core.Domain.Customer> IeParPlusInventoryAmountsReport.CustomerNameList
        {
            set
            {
                ddlCustomerNameFilter.DataSource = null;
                ddlCustomerNameFilter.DataSource = value;
                ddlCustomerNameFilter.DataTextField = "NameAccount";
                ddlCustomerNameFilter.DataValueField = "AccountNumber";
                ddlCustomerNameFilter.DataBind();
                ddlCustomerNameFilter.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), "0"));
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
                ddlStateFilter.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), "0"));
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
                ddlOwnershipStructureFilter.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), "0"));
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
                ddlManagementStructureFilter.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), "0"));
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
                ddlBranchAgencyFilter.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), "0"));
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
                ddlProductLine.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), "0"));
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
                ddlCategory.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), "0"));
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
                ddlSubCategory1.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), "0"));
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
                ddlSubCategory2.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), "0"));
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
                ddlSubCategory3.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), "0"));
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
                ddlSalesRepresentativeFilter.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), "0"));
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
                ddlManagerFilter.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), "0"));
                ddlManagerFilter.SelectedIndex = 0;
            }
        }


        public string SelectedCustomerAccountFilter
        {
            get
            {
                if (ddlCustomerNameFilter.SelectedIndex > 0)
                    return Convert.ToString(ddlCustomerNameFilter.SelectedValue);
                return string.Empty;
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
                if (ddlBranchAgencyFilter.SelectedIndex > 0)
                    return Convert.ToString(ddlBranchAgencyFilter.SelectedValue);
                return string.Empty;
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
            get
            {
                if (ddlManagerFilter.SelectedIndex > 0)
                    return Convert.ToString(ddlManagerFilter.SelectedValue);
                return string.Empty;
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
            get
            {
                if (ddlSalesRepresentativeFilter.SelectedIndex > 0)
                    return Convert.ToString(ddlSalesRepresentativeFilter.SelectedValue);
                return string.Empty;
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
            get
            {
                if (ddlStateFilter.SelectedIndex > 0)
                    return Convert.ToString(ddlStateFilter.SelectedValue);
                return string.Empty;
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
            get
            {
                if (ddlOwnershipStructureFilter.SelectedIndex > 0)
                    return Convert.ToString(ddlOwnershipStructureFilter.SelectedValue);
                return string.Empty;
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
            get
            {
                if (ddlManagementStructureFilter.SelectedIndex > 0)
                    return Convert.ToString(ddlManagementStructureFilter.SelectedValue);
                return string.Empty;
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
            get
            {
                if (ddlProductLine.SelectedIndex > 0)
                    return Convert.ToString(ddlProductLine.SelectedValue);
                return string.Empty;
            }
        }

        public string SelectedCategoryFilter
        {
            get
            {
                if (ddlCategory.SelectedIndex > 0)
                    return Convert.ToString(ddlCategory.SelectedValue);
                return string.Empty;
            }
        }

        public string SelectedSubCategory1Filter
        {
            get
            {
                if (ddlSubCategory1.SelectedIndex > 0)
                    return Convert.ToString(ddlSubCategory1.SelectedValue);
                return string.Empty;
            }
        }

        public string SelectedSubCategory2Filter
        {
            get
            {
                if (ddlSubCategory2.SelectedIndex > 0)
                    return Convert.ToString(ddlSubCategory2.SelectedValue);
                return string.Empty;
            }
        }

        public string SelectedSubCategory3Filter
        {
            get
            {
                if (ddlSubCategory3.SelectedIndex > 0)
                    return Convert.ToString(ddlSubCategory3.SelectedValue);
                return string.Empty;
            }
        }

        public string SelectedItemStatus
        {
            get
            {
                if (ddlTransaction.SelectedIndex > 0)
                    return Convert.ToString(ddlTransaction.SelectedValue);
                return string.Empty;
            }
        }


        #endregion


    }
}

