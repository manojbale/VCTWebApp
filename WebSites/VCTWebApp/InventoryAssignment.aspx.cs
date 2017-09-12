using System;
using Microsoft.Practices.ObjectBuilder;
using System.Data;
using System.Web.UI.WebControls;
using VCTWebApp.Resources;
using System.Data.SqlClient;
using System.Globalization;
using System.Collections.Generic;
using System.Text;
using VCTWeb.Core.Domain;
using VCTWebApp.Web;

namespace VCTWebApp.Shell.Views
{
    public partial class InventoryAssignment : Microsoft.Practices.CompositeWeb.Web.UI.Page, IInventoryAssignmentView
    {
        #region Instance Variables
        private InventoryAssignmentPresenter _presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private Security security = null;
        #endregion

        #region Protected Methods
        protected void btnFindMatch_Click(object sender, EventArgs e)
        {
            Session["CaseIdToSearch"] = this.lstPendingCases.SelectedValue;
            Session["QuantityToSearch"] = this.RequiredKitQuantity - this.KitstobeAssigned.Count;
            Response.Redirect("~/CaseFindMatch.aspx");

        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            string TmpCaseStatus = "";
            int TotCount = 0, Count = 0, TotRecdCount = 0;
            List<string> lstIDs = new List<string>();
            if (this.InventoryType == VCTWeb.Core.Domain.Constants.InventoryType.Part.ToString())
            {
                string casePartDetailXmlString = "<root>";
                for (int i = 0; i < gridCatalog.Rows.Count; i++)
                {
                    TotCount += 1;

                    HiddenField hdnCasePartId = gridCatalog.Rows[i].FindControl("hdnCasePartId") as HiddenField;
                    DropDownList ddlLotNumber = gridCatalog.Rows[i].FindControl("ddlLotNumber") as DropDownList;
                    CheckBox chkSelect = gridCatalog.Rows[i].FindControl("chkSelect") as CheckBox;

                    if (!ddlLotNumber.Enabled)
                    {
                        Count += 1;
                        //lstIDs.Add(ddlLotNumber.SelectedValue);
                    }
                    else
                    {
                        if (chkSelect.Checked && ddlLotNumber.SelectedValue == "0")
                        {
                            
                            lblError.Text = "<font color='red'>" + "Please assign  Lot Number to the Items" + "</font>"; 
                            return;
                        }

                        if (chkSelect.Checked && ddlLotNumber.SelectedValue != "0")
                        {
                            Count += 1;
                            TotRecdCount += 1;
                            //if (ddlLotNumber.SelectedIndex < 0 || ddlLotNumber.SelectedItem.ToString() == "")
                            //{
                            //    TotRecdCount -= 1;
                            //    Count -= 1;
                            //    //lblError.Text = "Please assign Lot Number to all the Items";
                            //    //return;
                            //}
                            //else 
                            if (lstIDs.Contains(ddlLotNumber.SelectedValue))
                            {

                                
                                lblError.Text = "<font color='red'>" + "Please assign different Lot Number to the Items" + "</font>"; 
                                return;
                            }
                            lstIDs.Add(ddlLotNumber.SelectedValue);

                            casePartDetailXmlString += "<CasePartDetail>";
                            casePartDetailXmlString += "<CasePartId>" + hdnCasePartId.Value + "</CasePartId>";
                            casePartDetailXmlString += "<LocationPartDetailId>" + ddlLotNumber.SelectedValue + "</LocationPartDetailId>";
                            casePartDetailXmlString += "</CasePartDetail>";

                        }
                    }
                }
                casePartDetailXmlString += "</root>";

                if (TotRecdCount <= 0)
                {
                    lblError.Text = "<font color='red'>" + "Please assign Lot Number to at least one Item" + "</font>"; 
                    return;
                }

                if (Count < TotCount)
                {
                    if (CaseStatus == "New" || CaseStatus == "InternallyRequested")
                        TmpCaseStatus = "PartiallyInventoryAssigned";
                    else
                        TmpCaseStatus = CaseStatus;
                }
                else
                {
                    if (CaseStatus == "New" || CaseStatus == "PartiallyInventoryAssigned" || CaseStatus == "InternallyRequested")
                        TmpCaseStatus = "InventoryAssigned";
                    else
                        TmpCaseStatus = CaseStatus;
                }

                if (!Presenter.AssignPartInventory(casePartDetailXmlString, TmpCaseStatus))
                {
                    lblError.Text = "Problem in assigning inventory. The selected item(s) may not be available currently. Please select other item(s) or try after refreshing the page.";
                    return;
                }
            }
            else
            {
                string buildKitIds = string.Empty;
                for (int i = 0; i < gridKitTable.Rows.Count; i++)
                {
                    TotCount += 1;
                    DropDownList ddlKitNumber = gridKitTable.Rows[i].FindControl("ddlKitNumber") as DropDownList;
                    CheckBox chkSelect = gridKitTable.Rows[i].FindControl("chkSelect") as CheckBox;

                    if (!ddlKitNumber.Enabled)
                    {
                        Count += 1;
                        //lstIDs.Add(ddlKitNumber.SelectedValue);
                    }
                    else
                    {
                        if (chkSelect.Checked && ddlKitNumber.SelectedValue == "0")
                        {
                            lblError.Text = "<font color='red'>" + "Please assign  Kit Number to the Items" + "</font>"; 
                            return;
                        }
                        if (chkSelect.Checked && ddlKitNumber.SelectedValue != "0")
                        {
                            TotRecdCount += 1;
                            Count += 1;
                            
                            if (lstIDs.Contains(ddlKitNumber.SelectedValue))
                            {
                                lblError.Text = "<font color='red'>" + "Please assign different Kit to the Items" + "</font>"; ;
                                return;
                            }
                            lstIDs.Add(ddlKitNumber.SelectedValue);

                            buildKitIds += (buildKitIds == string.Empty ? string.Empty : ",") + ddlKitNumber.SelectedValue;

                        }
                    }
                }

                if (TotRecdCount <= 0)
                {
                    lblError.Text = "<font color='red'>" + "Please assign Kit to at least one Item" + "</font>"; 
                    return;
                }


                if (Count < TotCount)
                {
                    if (CaseStatus == "New" || CaseStatus == "InternallyRequested")
                        TmpCaseStatus = "PartiallyInventoryAssigned";
                    else
                        TmpCaseStatus = CaseStatus;
                }
                else
                {
                    if (CaseStatus == "New" || CaseStatus == "PartiallyInventoryAssigned" || CaseStatus == "InternallyRequested")
                        TmpCaseStatus = "InventoryAssigned";
                    else
                        TmpCaseStatus = CaseStatus;
                }


                if (!this._presenter.AssignKitInventory(buildKitIds, TmpCaseStatus))
                {
                    lblError.Text = "Problem in assigning inventory. The selected item(s) may not be available currently. Please select other item(s) or try after refreshing the page.";
                    return;
                }
            }
            lblError.Text = "Inventory assigned successfully for the selected Case";
            lblError.ForeColor = System.Drawing.Color.Blue;
            ClearFields();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ClearNotifications();
            if (!this.IsPostBack)
            {               
                hdnInventoryType.Value = string.Empty;
                this.AuthorizedPage();
                this._presenter.OnViewInitialized();
                //this.LocalizePage();
            }
        }

        protected void ddlCaseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ClearFields();
                lstPendingCases.Items.Clear();
                Presenter.PopulatePendingCasesList();
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        protected void lstPendingCases_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Presenter.OnViewLoaded();
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

        protected void gridKitTable_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                this.PreviousKitIndex = 0;
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlKitNumber = e.Row.FindControl("ddlKitNumber") as DropDownList;
                if (ddlKitNumber != null)
                {
                    HiddenField hdnKitNumber = e.Row.FindControl("hdnKitNumber") as HiddenField;
                    HiddenField hdnBuildKitId = e.Row.FindControl("hdnBuildKitId") as HiddenField;
                    HiddenField hdnKitFamilyId = e.Row.FindControl("hdnKitFamilyId") as HiddenField;
                    if (!string.IsNullOrEmpty(hdnKitNumber.Value))
                    {
                        ddlKitNumber.Items.Add(new ListItem(hdnKitNumber.Value, hdnBuildKitId.Value));
                        ddlKitNumber.Enabled = false;

                        CheckBox chkSelect = e.Row.FindControl("chkSelect") as CheckBox;
                        chkSelect.Checked = true;
                        chkSelect.Enabled = false;
                    }
                   else
                   {
                       List<InventoryStockKit> lst = this.KitstobeAssigned.FindAll(l => l.KitFamilyId == Convert.ToInt64(hdnKitFamilyId.Value));

                       ddlKitNumber.DataSource = lst;
                       ddlKitNumber.DataTextField = "KitNumber";
                       ddlKitNumber.DataValueField = "BuildKitId";
                       ddlKitNumber.DataBind();

                        
                    //    //if (e.Row.RowIndex < this.KitstobeAssigned.Count)
                    //    //    ddlKitNumber.SelectedIndex = e.Row.RowIndex;

                    //    if (this.PreviousKitIndex < this.KitstobeAssigned.Count)
                    //    {
                    //        ddlKitNumber.SelectedIndex = this.PreviousKitIndex;
                    //        this.PreviousKitIndex++;
                    //    }
                        ddlKitNumber.Items.Insert(0, new ListItem("", "0"));
                        ddlKitNumber.SelectedIndex = 0;
                      
                    //    //ddlKitNumber .Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect")));
                    }
                }
            }
        }

        protected void gridCatalog_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                this.PreviousPartIndex = 0;
                this.PreviousPartNumber = "";
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblCatalogNum = e.Row.FindControl("lblCatalogNum") as Label;
                DropDownList ddlLotNumber = e.Row.FindControl("ddlLotNumber") as DropDownList;
                HiddenField hdnLocationPartDetailId = e.Row.FindControl("hdnLocationPartDetailId") as HiddenField;

                if (ddlLotNumber != null)
                {
                    if (Convert.ToInt64(hdnLocationPartDetailId.Value) > 0)
                    {
                        HiddenField hdnLotNum = e.Row.FindControl("hdnLotNum") as HiddenField;
                        ddlLotNumber.Items.Add(new ListItem(hdnLotNum.Value, hdnLocationPartDetailId.Value));
                        ddlLotNumber.Enabled = false;

                        CheckBox chkSelect = e.Row.FindControl("chkSelect") as CheckBox;
                        chkSelect.Checked = true;
                        chkSelect.Enabled = false;
                    }
                    else
                    {
                        if (this.PreviousPartNumber == lblCatalogNum.Text)
                        {
                            this.PreviousPartIndex++;
                        }
                        else
                        {
                            this.PreviousPartNumber = lblCatalogNum.Text;
                            this.PreviousPartIndex = 0;
                        }
                        List<InventoryStockPart> lst = this.LotstobeAssigned.FindAll(l => l.PartNum == lblCatalogNum.Text);
                        //ddlLotNumber.DataSource = Presenter.GetLotNumbersToBeAssigned(lblCatalogNum.Text);
                        ddlLotNumber.DataSource = lst;
                        ddlLotNumber.DataTextField = "LotNum";
                        ddlLotNumber.DataValueField = "LocationPartDetailId";
                        ddlLotNumber.DataBind();

                        //if (this.PreviousPartIndex < lst.Count)
                        //    ddlLotNumber.SelectedIndex = this.PreviousPartIndex;

                        ddlLotNumber.Items.Insert(0, new ListItem("", "0"));
                        ddlLotNumber.SelectedIndex = 0;
                        //ddlLotNumber.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect")));
                    }
                }
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
            else if (security.HasAccess("InventoryAssignment"))
            {
                //CanCancel = security.HasPermission("InventoryAssignment");
            }
            else
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }

        private void ClearFields()
        {
            this.PreviousPartNumber = string.Empty;
            this.PreviousPartIndex = 0;

            txtCaseNumber.Text = string.Empty;
            txtRequiredOn.Text = string.Empty;
            txtShipFromLocation.Text = string.Empty;
            txtShipToLocation.Text = string.Empty;
            txtShipToLocationType.Text = string.Empty;

            //txtKitFamily.Text = string.Empty;
            //txtKitFamilyDescription.Text = string.Empty;
            //txtQuantity.Text = string.Empty;
            txtShippingDate.Text = string.Empty;
            txtRetrievalDate.Text = string.Empty;
            txtProcedureName.Text = string.Empty;


            gridKitTable.DataSource = null;
            gridKitTable.DataBind();
            gridCatalog.DataSource = null;
            gridCatalog.DataBind();

            lstPendingCases.SelectedIndex = -1;
        }

        private void ClearNotifications()
        {
            lblError.Text = string.Empty;
        }

        //private void LocalizePage()
        //{
        //    try
        //    {
        //        string heading = string.Empty;
        //        heading = vctResource.GetString("mnuKitListing");
        //        lblHeader.Text = heading;
        //        Page.Title = heading;

        //        this.lblExistingKit.Text = vctResource.GetString("lblExistingKit");
        //        this.lblKitFamily.Text = vctResource.GetString("lblKitFamily");
        //        this.lblKitDescription.Text = vctResource.GetString("lblKitDescription");
        //        this.lblLocation.Text = vctResource.GetString("lblLocation");
        //        this.lblInventoryItem.Text = vctResource.GetString("lblInventoryItem");
        //        this.chkIsActive.Text = vctResource.GetString("chkIsActive");
        //        this.lblKitNumber.Text = vctResource.GetString("lblKitNumber");
        //        this.lblKitName.Text = vctResource.GetString("lblKitName");
        //        this.lblProcedureName.Text = vctResource.GetString("lblProcedureNameKitListing");
        //        //this.btnNew.Text = vctResource.GetString("btnReset");
        //        //this.btnSave.Text = vctResource.GetString("btnSave");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //private void EnableDisableControls()
        //{
        //    txtKitNumber.ReadOnly = false;
        //    //btnDelete.Enabled = false; ;
        //}

        //private bool ValidateInputs()
        //{
        //    if (string.IsNullOrEmpty(txtKitNumber.Text))
        //    {
        //        lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valKitListingKitNumber"), this.lblHeader.Text);
        //        return false;
        //    }
        //    if (IsKitNumberDuplicate())
        //    {
        //        lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valDuplicateKitNumber"), this.lblHeader.Text);
        //        return false;
        //    }
        //    if (string.IsNullOrEmpty(txtKitName.Text))
        //    {
        //        lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valKitName"), this.lblHeader.Text);
        //        return false;
        //    }
        //    if (ddlKitFamily.SelectedIndex <= 0)
        //    {
        //        lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valKitFamily"), this.lblHeader.Text);
        //        return false;
        //    }
        //    if (ddlSalesOfficeLocation.SelectedIndex <= 0)
        //    {
        //        lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valKitListingLocation"), this.lblHeader.Text);
        //        return false;
        //    }
        //    if (chkIsActive.Checked == false)
        //    {
        //        lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valIsActive"), this.lblHeader.Text);
        //        return false;
        //    }
        //    if (KitTableList.Find(t => string.IsNullOrEmpty(t.Catalognumber)) != null)
        //    {
        //        lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valLeastKitItem"), this.lblHeader.Text);
        //        return false;
        //    }
        //    return true;
        //}

        //private string CreateKitTableXml()
        //{
        //    // Edit Mode no xml is being sent. Items are Added/edited indiviually.
        //    if (lstExistingKit.SelectedIndex >= 0)
        //    {
        //        return null;
        //    }
        //    StringBuilder kitTableXml = new StringBuilder();
        //    kitTableXml.Append("<root>");
        //    int count = 0;
        //    foreach (var item in this.KitTableList)
        //    {
        //        kitTableXml.Append("<KitTable>");
        //        kitTableXml.Append("<ItemNumber>"); kitTableXml.Append(++count); kitTableXml.Append("</ItemNumber>");
        //        kitTableXml.Append("<CatalogNumber>"); kitTableXml.Append(item.Catalognumber); kitTableXml.Append("</CatalogNumber>");
        //        kitTableXml.Append("<Description>"); kitTableXml.Append(item.Description); kitTableXml.Append("</Description>");
        //        kitTableXml.Append("<Qty>"); kitTableXml.Append(item.Quantity); kitTableXml.Append("</Qty>");
        //        kitTableXml.Append("</KitTable>");
        //    }
        //    kitTableXml.Append("</root>");
        //    return kitTableXml.ToString();
        //}
        //private void ClearNotifications()
        //{
        //    lblError.Text = string.Empty;
        //}
        #endregion

        #region Create New Presenter
        [CreateNew]
        public InventoryAssignmentPresenter Presenter
        {
            get
            {
                return this._presenter;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this._presenter = value;
                this._presenter.View = this;
            }
        }
        #endregion

        #region IInventoryAssignmentView Implementations

        public List<VCTWeb.Core.Domain.CaseSmall> PendingCasesList
        {
            set
            {
                lstPendingCases.DataSource = value;
                lstPendingCases.DataTextField = "CaseNumber";
                lstPendingCases.DataValueField = "CaseId";
                lstPendingCases.DataBind();
            }
        }

        public List<VCTWeb.Core.Domain.KitFamilSmall> KitDetailList
        {
            set
            {
                gridKitTable.DataSource = value;
                gridKitTable.DataBind();
                hdnInventoryType.Value = Constants.InventoryType.Kit.ToString();

                List<VCTWeb.Core.Domain.KitFamilSmall> tmp = value.FindAll(k => String.IsNullOrEmpty(k.KitNumber));

                this.RequiredKitQuantity = tmp.Count;

                if (this.InventoryType == Constants.InventoryType.Kit.ToString() && (this.KitstobeAssigned == null || this.RequiredKitQuantity > this.KitstobeAssigned.Count))
                {
                    btnFindMatch.Visible = true;
                }
                else
                {
                    btnFindMatch.Visible = false;
                }
            }
        }

        public List<VCTWeb.Core.Domain.ItemDetail> ItemDetailList
        {
            set
            {
                gridCatalog.DataSource = value;
                gridCatalog.DataBind();
                hdnInventoryType.Value = Constants.InventoryType.Part.ToString();
            }
        }

        public long SelectedCaseId
        {
            get
            {
                if (lstPendingCases.SelectedValue != null)
                    return Convert.ToInt64(lstPendingCases.SelectedValue);
                else
                    return 0;
            }
        }

        public string CaseNumber
        {
            set
            {
                txtCaseNumber.Text = value;
            }
        }

        public string SelectedCaseType
        {
            get { return ddlCaseType.SelectedValue; }
        }

        //public string KitFamily
        //{
        //    get { return txtKitFamily.Text; }
        //    set { txtKitFamily.Text = value; }
        //}

        //public long KitFamilyId
        //{
        //    get { return Convert.ToInt64(ViewState["KitFamilyId"]); }
        //    set { ViewState["KitFamilyId"] = value; }
        //}

        //public int Quantity
        //{
        //    get { return Convert.ToInt32(txtQuantity.Text); }
        //    set { txtQuantity.Text = value.ToString(); }
        //}

        //public string KitFamilyDesc
        //{
        //    set { txtKitFamilyDescription.Text = value; }
        //}

        public string ShipFromLocation
        {
            set
            {
                txtShipFromLocation.Text = value;
            }
        }

        public string ShipToLocation
        {
            set
            {
                txtShipToLocation.Text = value;
            }
        }

        public string ShipToLocationType
        {
            set
            {
                txtShipToLocationType.Text = value;
            }
        }

        public DateTime SurgeryDate
        {
            set { txtRequiredOn.Text = string.Format(CultureInfo.CurrentCulture, value.ToString("d")); }
        }

        public DateTime? ShippingDate
        {
            set { txtShippingDate.Text = value == null ? string.Empty : string.Format(CultureInfo.CurrentCulture, Convert.ToDateTime(value).ToString("d")); }
        }

        public DateTime? RetrievalDate
        {
            set { txtRetrievalDate.Text = value == null ? string.Empty : string.Format(CultureInfo.CurrentCulture, Convert.ToDateTime(value).ToString("d")); }
        }

        public string ProcedureName
        {
            set { txtProcedureName.Text = value; }
        }

        public List<VCTWeb.Core.Domain.InventoryStockKit> KitstobeAssigned
        {
            get
            {
                return ViewState["KitstobeAssigned"] as List<InventoryStockKit>;
            }
            set
            {
                ViewState["KitstobeAssigned"] = value;
            }
        }

        public List<VCTWeb.Core.Domain.InventoryStockPart> LotstobeAssigned
        {
            get
            {
                return ViewState["LotstobeAssigned"] as List<InventoryStockPart>;
            }
            set
            {
                ViewState["LotstobeAssigned"] = value;
            }
        }

        public string PreviousPartNumber
        {
            get
            {
                return ViewState["PreviousPartNumber"] as string;
            }
            set
            {
                ViewState["PreviousPartNumber"] = value;
            }
        }

        public int PreviousPartIndex
        {
            get
            {
                return (int)ViewState["PreviousPartIndex"];
            }
            set
            {
                ViewState["PreviousPartIndex"] = value;
            }
        }

        public int PreviousKitIndex
        {
            get
            {
                return (int)ViewState["PreviousKitIndex"];
            }
            set
            {
                ViewState["PreviousKitIndex"] = value;
            }
        }

        public int RequiredKitQuantity
        {
            get
            {
                return (int)ViewState["RequiredKitQuantity"];
            }
            set
            {
                ViewState["RequiredKitQuantity"] = value;
            }
        }

        public string InventoryType
        {
            
            get
            {
                if (pnlKitGrid.Visible == true)
                {
                                    
                    return Constants.InventoryType.Kit.ToString();
                }
                else
                {
                  
                   
                    return Constants.InventoryType.Part.ToString();
                }
            }
            set
            {
                if (value == Constants.InventoryType.Kit.ToString())
                {
                    pnlDetail2.Visible = true;
                    txtBarCode.Focus();
                    this.pnlKitGrid.Visible = true;
                    this.pnlPartGrid.Visible = false;

                    btnFindMatch.Visible = false;
                }
                else
                {
                    pnlDetail2.Visible = false;
                    txtBarCode.Focus();
                    this.pnlKitGrid.Visible = false;
                    this.pnlPartGrid.Visible = true;

                    btnFindMatch.Visible = false;
                }
            }
        }

        public List<CaseType> CaseTypeList
        {
            set
            {
                this.ddlCaseType.DataSource = value;
                this.ddlCaseType.DataTextField = "CaseTypeName";
                this.ddlCaseType.DataValueField = "CaseTypeName";
                this.ddlCaseType.DataBind();

                ddlCaseType.Items.Insert(0, new ListItem(vctResource.GetString("listItemAll"), string.Empty));
            }
        }

        public string CaseStatus
        {
            get
            {
                return (string)ViewState["CaseStatus"];
            }
            set
            {
                ViewState["CaseStatus"] = value;
                lblCaseStatus.Text = value;
            }
        }
        #endregion
    }
}

