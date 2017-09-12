using System;
using Microsoft.Practices.ObjectBuilder;
using System.Data;
using System.Data.SqlClient;
using VCTWebApp.Web;
using VCTWebApp.Resources;
using System.Collections.Generic;
using VCTWeb.Core.Domain;
using System.Web.UI.WebControls;
using System.Globalization;

namespace VCTWebApp.Shell.Views
{
    public partial class VirtualCheckOut : Microsoft.Practices.CompositeWeb.Web.UI.Page, IVirtualCheckOutView
    {
        #region Instance Variables
        private VirtualCheckOutPresenter _presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        #endregion

        #region Create New Presenter
        [CreateNew]
        public VirtualCheckOutPresenter Presenter
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

        #region Private Properties
        private bool CanView
        {
            get
            {
                return ViewState[Common.CAN_VIEW] != null ? (bool)ViewState[Common.CAN_VIEW] : false;
            }
            set
            {
                ViewState[Common.CAN_VIEW] = value;
            }
        }

        #endregion

        #region Init / Load Page
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
               
                _presenter.OnViewInitialized();
                _presenter.PopulatePendingCasesList();
                this.LocalizePage();
                
            }                      
        }                
        #endregion

        #region IVirtualCheckOutView Implementations

        public long SelectedCaseId
        {
            get
            {
                return (Convert.ToInt64(lstPendingCases.SelectedValue));
            }
        }

        public string InventoryType
        {
            get
            {
                return (string)ViewState["CaseType"];
            }
            set
            {
                ViewState["CaseType"] = value;
                if (value == Constants.InventoryType.Kit.ToString())
                {
                    pnlDetailKit.Visible = true;
                    pnlKitGrid.Visible = true;
                }
                else
                {
                    pnlPartGrid.Visible = true;
                }
            }            
        }

        public string CaseNumber 
        { 
            set 
            {
                txtCaseNumber.Text = value;
            } 
        }
        
        public DateTime SurgeryDate 
        {
            set
            { 
                txtRequiredOn.Text = value.ToShortDateString();
            }
        }
        
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

        //public long KitFamilyId 
        //{
        //    get
        //    {
        //        return (long)ViewState["KitFamilyId"];
        //    }
        //    set
        //    {
        //        ViewState["KitFamilyId"] = value;
        //    }
        //}

        //public string KitFamily 
        //{
        //    set
        //    {
        //        txtKitFamily.Text = value;
        //    }
        //}

        //public string KitFamilyDesc
        //{
        //    set
        //    {
        //        txtKitFamilyDescription.Text = value;
        //    }
        //}

        public int Quantity
        {
            get 
            {
                return Convert.ToInt32(ViewState["Quantity"]);
            }
            set
            {
                ViewState["Quantity"] = value;
            }
        }

        public DateTime? ShippingDate 
        {
            set
            {
                if (value.HasValue)
                {
                    txtShippingDate.Text = Convert.ToDateTime(value).ToShortDateString();
                }
            }
        }

        public DateTime? RetrievalDate 
        {
            set
            { 
                if(value.HasValue)
                {
                    txtRetrievalDate.Text = Convert.ToDateTime(value).ToShortDateString();
                }
            }
        }

        public string ProcedureName 
        {
            set
            {
                txtProcedureName.Text = value;
            }
        }

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

        public List<VCTWeb.Core.Domain.VirtualCheckOut> ShippingKitList 
        {
            set
            {                
                gridKitTable.DataSource = value;
                gridKitTable.DataBind();
                hdnInventoryType.Value = Constants.InventoryType.Kit.ToString();
                txtBarCode.Focus();
            }
        }

        public List<VCTWeb.Core.Domain.VirtualCheckOut> ShippingPartList 
        {
            set
            {                
                gridCatalog.DataSource = value;
                gridCatalog.DataBind();
                hdnInventoryType.Value = Constants.InventoryType.Part.ToString();
                txtBarCode.Focus();
            }
        }

        public string TableXml
        {
            get
            {
                return CreateTableXml();
            }
        }

        //public List<VCTWeb.Core.Domain.VirtualBuilKit> BuildKitList
        //{
        //    set
        //    { 
        //        ViewState[""]
        //    }
        //}

        public List<CaseType> CaseTypeList
        {
            set
            {
                this.ddlCaseType.DataSource = value;
                this.ddlCaseType.DataTextField = "CaseTypeName";
                this.ddlCaseType.DataValueField = "CaseTypeName";
                this.ddlCaseType.DataBind();

                ddlCaseType.Items.Insert(0, new ListItem("-- All --", ""));
            }
        }

        public string CaseType
        {
            get
            {
                return (ddlCaseType.SelectedValue);
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

        //public string CaseState
        //{
        //    get
        //    {
        //        return (string)ViewState["CaseState"];
        //    }
        //    set
        //    {
        //        ViewState["CaseState"] = value;                
        //    }

        //}
        #endregion               

        #region Event Handlers
        protected void ddlCaseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlCaseType.SelectedValue == "0")
            //{
            //    ClearFields();
            //    lstPendingCases.Items.Clear();
            //}
            //else
            //{
                ClearFields();
                _presenter.PopulatePendingCasesList();
            //}
        }

        protected void lstPendingCases_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.lblError.Text = string.Empty;
                _presenter.OnViewLoaded();
                if (this.InventoryType == Constants.InventoryType.Kit.ToString())
                {
                    //pnlKitGrid.Height = 240;
                    //lstPendingCases.Height = 400;
                    pnlKitGrid.Visible = true;
                    pnlDetailKit.Visible = true;
                    pnlPartGrid.Visible = false;
                }
                else
                {
                    //pnlPartGrid.Height = 290;
                    //lstPendingCases.Height = 400;
                    pnlKitGrid.Visible = false;
                    pnlDetailKit.Visible = false;
                    pnlPartGrid.Visible = true;

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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string FinalStatus = "";
            if (IsValidPage())
            {
                FinalStatus = GetKitPartStatus();
                if (!_presenter.Save(FinalStatus))
                {
                    lblError.Text = "Problem in Web CheckOut for selected Kit/Part #. The selected Kit/Part(s) may be already Shipped. Please try after refreshing the page.";                                    
                }
                else
                {
                    ClearFields();
                    lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgSave"), this.lblHeader.Text) + "</font>";
                }
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            ClearFields();
            ddlCaseType.SelectedIndex = 0;
            _presenter.PopulatePendingCasesList();         
            
        }

        //protected void btnView_Click(object sender, EventArgs e)
        //{
        //    ClearFields();
        //    _presenter.PopulatePendingCasesList();

        //}     
        #endregion

        #region Private Methods
        private void ClearFields()
        { 
            txtCaseNumber.Text = "";
            txtRequiredOn.Text = "";
            txtShipFromLocation.Text = "";
            txtShipToLocation.Text = "";
            txtShipToLocationType.Text = "";
            //txtKitFamily.Text = "";
            //txtKitFamilyDescription.Text = "";
            //txtQuantity.Text = "";
            txtShippingDate.Text = "";
            txtRetrievalDate.Text = "";
            txtProcedureName.Text = "";
            pnlKitGrid.Visible = false;
            pnlPartGrid.Visible = false;
            lblError.Text = string.Empty;
        }

        private bool IsValidPage()
        {
            if (string.IsNullOrEmpty(lstPendingCases.SelectedValue))
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("WebCheckOut_SelectCase"), this.lblHeader.Text);
                return false;
            }
            if (!string.IsNullOrEmpty(this.InventoryType))
            {
                if (this.InventoryType == Constants.InventoryType.Kit.ToString())
                {
                    if (gridKitTable.Rows.Count < 1)
                    {
                        lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("WebCheckOut_KitMissing"), this.lblHeader.Text);
                        return false;
                    }

                    int Count = 0;
                    foreach (GridViewRow row in gridKitTable.Rows)
                    {
                        CheckBox chkSelect = row.FindControl("chkSelect") as CheckBox;
                        if (chkSelect.Enabled && chkSelect.Checked) 
                            Count += 1;
                    }

                    if (Count == 0)
                    {
                        lblError.Text = "Please select at least one item for checkout";
                        //lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("WebCheckOut_KitMissing"), this.lblHeader.Text);
                        return false;
                    }
                }
                else
                {
                    if (gridCatalog.Rows.Count < 1)
                    {
                        lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("WebCheckOut_PartMissing"), this.lblHeader.Text);
                        return false;
                    }

                    int Count = 0;
                    foreach (GridViewRow row in gridCatalog.Rows)
                    {
                        CheckBox chkSelect = row.FindControl("chkSelect") as CheckBox;
                        if (chkSelect.Enabled && chkSelect.Checked)
                            Count += 1;
                    }

                    if (Count == 0)
                    {
                        lblError.Text = "Please select at least one item for checkout";
                        //lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("WebCheckOut_KitMissing"), this.lblHeader.Text);
                        return false;
                    }
                }
            }
            
            
            return true;
        }

        private string CreateTableXml()
        {
            System.Text.StringBuilder TableXml = new System.Text.StringBuilder();
            TableXml.Append("<root>");

            if (InventoryType == Constants.InventoryType.Kit.ToString())
            {
                Int64 BuildKitId, CaseKitId;
                foreach (GridViewRow row in gridKitTable.Rows)
                {
                    CheckBox chkSelect = row.FindControl("chkSelect") as CheckBox;
                    if (chkSelect.Checked && chkSelect.Enabled) 
                    {
                        BuildKitId = Convert.ToInt64((row.FindControl("hdnBuildKitId") as HiddenField).Value);
                        CaseKitId = Convert.ToInt64((row.FindControl("hdnCaseKitId") as HiddenField).Value);

                        TableXml.Append("<CasesTable>");
                        TableXml.Append("<CaseKitId>"); TableXml.Append(CaseKitId); TableXml.Append("</CaseKitId>");
                        TableXml.Append("<BuildKitId>"); TableXml.Append(BuildKitId); TableXml.Append("</BuildKitId>");
                        TableXml.Append("</CasesTable>");
                    }
                }
            }
            else
            {
                Int64 CasePartId;
                foreach (GridViewRow row in gridCatalog.Rows)
                {
                    CheckBox chkSelect = row.FindControl("chkSelect") as CheckBox;
                    if (chkSelect.Checked && chkSelect.Enabled)
                    {
                        CasePartId = Convert.ToInt64((row.FindControl("hdnCasePartId") as HiddenField).Value);

                        TableXml.Append("<CasesTable>");
                        TableXml.Append("<CasePartId>"); TableXml.Append(CasePartId); TableXml.Append("</CasePartId>");
                        TableXml.Append("</CasesTable>");
                    }
                }
            }
            TableXml.Append("</root>");

            return TableXml.ToString();
        }

        private void LocalizePage()
        {
            lblHeader.Text = vctResource.GetString("WebCheckOut_lblHeader");
            lblExistingKit.Text = vctResource.GetString("Common_lblExistingKit");
            lblCaseNumber.Text = vctResource.GetString("Common_lblCaseNumber");
            lblRequiredOn.Text = vctResource.GetString("Common_lblRequiredOn");
            lblShipFromLocation.Text = vctResource.GetString("Common_lblShipFromLocation");
            lblShipToLocation.Text = vctResource.GetString("Common_lblShipToLocation");
            lblShipToLocationType.Text = vctResource.GetString("Common_lblShipToLocationType");
            //lblKitFamily.Text = vctResource.GetString("Common_lblKitFamily");
            //lblKitDescription.Text = vctResource.GetString("Common_lblKitDescription");
            //lblQuantity.Text = vctResource.GetString("Common_lblQuantity");
            lblShippingDate.Text = vctResource.GetString("Common_lblShippingDate");
            lblRetrivalDate.Text = vctResource.GetString("Common_lblRetrivalDate");
            lblProcedureName.Text = vctResource.GetString("Common_lblProcedureName");
            lblInventoryDetail.Text = vctResource.GetString("Common_lblInventoryDetail");

            //lblShippingDate.Text = vctResource.GetString("lblPartNumHeader");
            //lblShippingDate.Text = vctResource.GetString("lblPartNumHeader");           


        }

        private string GetKitPartStatus()
        {
            string CaseState = "";
            string FinalStatus = "";
            int ItemCount = 0;
            if (this.InventoryType.ToUpper() == "KIT")
            {
                foreach (GridViewRow row in gridKitTable.Rows)
                {
                    CheckBox chkSelect = row.FindControl("chkSelect") as CheckBox;
                    if (!chkSelect.Enabled)
                        ItemCount += 1;
                    else
                        if (chkSelect.Checked) ItemCount += 1;

                }

                if (ItemCount < this.Quantity)
                    CaseState = "Partial";
                else
                    CaseState = "Full";
            }
            else
            {
                foreach (GridViewRow row in gridCatalog.Rows)
                {
                    CheckBox chkSelect = row.FindControl("chkSelect") as CheckBox;
                    if (!chkSelect.Enabled)
                        ItemCount += 1;
                    else
                        if (chkSelect.Checked) ItemCount += 1;

                }

                if (ItemCount < this.Quantity)
                    CaseState = "Partial";
                else
                    CaseState = "Full";
            }

            if (CaseState == "Partial")
            {
                if (CaseStatus == "InventoryAssigned" || CaseStatus == "PartiallyInventoryAssigned" || CaseStatus == "PartiallyShipped")
                    FinalStatus = "PartiallyShipped";
                else
                    FinalStatus = CaseStatus;
            }
            else
            {
                if (CaseStatus == "InventoryAssigned" || CaseStatus == "PartiallyShipped")
                    FinalStatus = "Shipped";
                else
                    FinalStatus = CaseStatus;
            }

            //if (CaseState == "Full" && (CaseStatus == "PartiallyShipped" || CaseStatus == "InventoryAssigned"))
            //{
            //    FinalStatus = "Shipped";
            //}
            //else if (CaseState == "Partial" && (CaseStatus == "InventoryAssigned" || CaseStatus == "PartiallyInventoryAssigned" || CaseStatus == "PartiallyShipped"))
            //{
            //    FinalStatus = "PartiallyShipped";
            //}
            //else if (CaseState == "Partial" && CaseStatus == "PartiallyCheckedIn")
            //{
            //    FinalStatus = "PartiallyCheckedIn";
            //}
            

            return FinalStatus;
        }


        #endregion

        #region Protected Methods


        //protected void chkboxSelectAll_Kit_changed(object sender, EventArgs e)
        //{
        //    if (((System.Web.UI.WebControls.CheckBox)(sender)).Checked)
        //    {
        //        foreach (GridViewRow row in gridKitTable.Rows)
        //        {
        //            CheckBox chkSelect = row.FindControl("chkSelect") as CheckBox;
        //            if (!chkSelect.Checked)
        //            {
        //                chkSelect.Checked = true;
        //            }

        //        }
        //    }
        //}

        protected void gridKitTable_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                (e.Row.FindControl("lblKitFamilyHeader") as Label).Text = vctResource.GetString("Common_lblKitFamily");
                (e.Row.FindControl("lblDescriptionHeader") as Label).Text = vctResource.GetString("WebCheckOut_Description");
                (e.Row.FindControl("lblKitNumberHeader") as Label).Text = vctResource.GetString("WebCheckOut_AssignKitNumber");
                
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string KitStatus = (e.Row.FindControl("lblKitStatus") as Label).Text;
                if (KitStatus != "AssignedToCase")
                {
                    CheckBox chkSelect = e.Row.FindControl("chkSelect") as CheckBox;
                    chkSelect.Checked = true;
                    chkSelect.Enabled = false;
                } 

                GridView grdChild = (GridView)e.Row.FindControl("grdChild");
                Int64 BuildKitId = Convert.ToInt64((e.Row.FindControl("hdnBuildKitId") as HiddenField).Value);

                grdChild.DataSource = _presenter.PopulateBuildKitById(BuildKitId);
                grdChild.DataBind();
            }
        }

        protected void grdChild_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                (e.Row.FindControl("lblPartNoHeader") as Label).Text = vctResource.GetString("WebCheckOut_PartNo");
                (e.Row.FindControl("lblDescriptionHeader") as Label).Text = vctResource.GetString("WebCheckOut_Description");
                (e.Row.FindControl("lblLotNumHeader") as Label).Text = vctResource.GetString("WebCheckOut_AssignLotNo");
                (e.Row.FindControl("lblExpiryDateHeader") as Label).Text = vctResource.GetString("WebCheckOut_ExpirayDate");
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                bool IsNearExpiry = Convert.ToBoolean((e.Row.FindControl("hdnIsNearExpiry") as HiddenField).Value);
                if (IsNearExpiry)
                {
                    e.Row.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
        
        protected void gridCatalog_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                (e.Row.FindControl("lblPartNoHeader") as Label).Text = vctResource.GetString("WebCheckOut_PartNo");
                (e.Row.FindControl("lblDescriptionHeader") as Label).Text = vctResource.GetString("WebCheckOut_Description");
                (e.Row.FindControl("lblLotNumHeader") as Label).Text = vctResource.GetString("WebCheckOut_AssignLotNo");
                (e.Row.FindControl("lblExpiryDateHeader") as Label).Text = vctResource.GetString("WebCheckOut_ExpirayDate");
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string PartStatus = (e.Row.FindControl("lblPartStatus") as Label).Text;
                if (PartStatus != "AssignedToCase")
                {
                    CheckBox chkSelect = e.Row.FindControl("chkSelect") as CheckBox;
                    chkSelect.Checked = true;
                    chkSelect.Enabled = false;
                } 

                bool IsNearExpiry = Convert.ToBoolean((e.Row.FindControl("hdnIsNearExpiry") as HiddenField).Value);
                if (IsNearExpiry)
                {
                    e.Row.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
        #endregion



    }
}

