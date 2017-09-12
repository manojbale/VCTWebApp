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
    public partial class VirtualCheckIn : Microsoft.Practices.CompositeWeb.Web.UI.Page, IVirtualCheckInView
    {

        #region Private Variables

        bool IsNearExpiry;
        string PartStatus;
        Int64 BuildKitId, CaseKitId;
        CheckBox chkSelect, chkStatus;
        #endregion

        #region Instance Variables
        private VirtualCheckInPresenter presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private Security security = null;
        #endregion

        #region Create New Presenter
        [CreateNew]
        public VirtualCheckInPresenter Presenter
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
                presenter.OnViewInitialized();                
                this.LocalizePage();
            }
        }
        #endregion

        #region IVirtualCheckInView Implementations

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
                if (value.HasValue)
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

        public List<VCTWeb.Core.Domain.VirtualCheckOut> CheckedInKitList
        {
            set
            {
                gridKitTable.DataSource = value;
                gridKitTable.DataBind();
            }
        }

        public List<VCTWeb.Core.Domain.VirtualCheckOut> CheckedInPartList
        {
            set
            {
                if (this.CaseTypeByCaseId == "ReturnInventoryRMATransfer")
                {
                    gridCatalog.DataSource = null;
                    gridCatalog.DataBind();

                    gridRMA.DataSource = value;
                    gridRMA.DataBind();
                }
                else
                {
                    gridRMA.DataSource = null;
                    gridRMA.DataBind();

                    gridCatalog.DataSource = value;
                    gridCatalog.DataBind();
                }
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

                ddlCaseType.Items.Insert(0, new ListItem("-- All --", "0"));
            }
        }

        public string CaseType
        {
            get
            {
                return (ddlCaseType.SelectedValue);
            }

        }

        public string CaseTypeByCaseId
        {
            set
            {
                ViewState["CaseTypeByCaseId"] = value;
            }
            get
            {
                return (string)ViewState["CaseTypeByCaseId"];
            }


        }

        public Int64? PartyId
        {
            get
            {
                return (Int64)ViewState["PartyId"];
            }
            set
            {
                if(value.HasValue)                
                    ViewState["PartyId"] = value;
                else
                    ViewState["PartyId"] = "0";
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

        public string CaseState
        {
            get
            {
                return (string)ViewState["CaseState"];
            }
            set
            {
                ViewState["CaseState"] = value;
            }
        }
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
                presenter.PopulatePendingCasesList();
            //}
        }

        protected void lstPendingCases_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.lblError.Text = string.Empty;
                presenter.OnViewLoaded();
                if (this.InventoryType == Constants.InventoryType.Kit.ToString())
                {
                    pnlKitGrid.Height = 240;
                    lstPendingCases.Height = 400;
                    pnlKitGrid.Visible = true;
                    pnlDetailKit.Visible = true;
                    pnlPartGrid.Visible = false;
                }
                else
                {
                    pnlKitGrid.Height = 170;
                    lstPendingCases.Height = 400;
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
            if (IsValidPage())
            {
                string Result = string.Empty, FinalStatus = "";

                FinalStatus = GetKitPartStatus();

                if (!presenter.SaveDetails(out Result, FinalStatus))
                {
                    lblError.Text = "Problem in Web CheckIn for selected Kit/Part #. The selected Kit/Part(s) may be already CheckedIn. Please try after refreshing the page.";                                    
                }
                else
                {
                    
                    if (string.IsNullOrEmpty(Result) && this.CaseTypeByCaseId != "ReturnInventoryRMATransfer")
                    {
                        Result = txtCaseNumber.Text;                    
                    }
                    ClearFields();
                    presenter.OnViewInitialized();   
                    this.lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgCommonforSaved"), Result, this.lblHeader.Text) + "</font>";                                       
                }  
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            ClearFields();
            ddlCaseType.SelectedIndex = 0;
            presenter.PopulatePendingCasesList();

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
            if (!Page.IsValid)
            {
                lblError.Text = vctResource.GetString("msgCommonError");
                return false;
            }
            else
            {
                if (string.IsNullOrEmpty(lstPendingCases.SelectedValue))
                {
                    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("WebCheckIn_SelectCase"), this.lblHeader.Text);
                    return false;
                }
                if (!string.IsNullOrEmpty(this.InventoryType))
                {
                    GridView gv;

                    if (this.InventoryType == Constants.InventoryType.Kit.ToString())
                    {
                        gv = gridKitTable;
                        if (gridKitTable.Rows.Count < 1)
                        {
                            lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("WebCheckIn_KitMissing"), this.lblHeader.Text);
                            return false;
                        }
                    }
                    else
                    {
                      
                        if (this.CaseTypeByCaseId != "ReturnInventoryRMATransfer")
                        {
                            gv = gridCatalog;
                            if (gridCatalog.Rows.Count < 1)
                            {
                                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("WebCheckIn_PartMissing"), this.lblHeader.Text);
                                return false;
                            }

                        }
                        else
                        {
                            gv = gridRMA;
                            if (gridRMA.Rows.Count < 1)
                            {
                                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("WebCheckIn_PartMissing"), this.lblHeader.Text);
                                return false;
                            }
                        }

                    }

                    int Count = 0;
                    foreach (GridViewRow row in gv.Rows)
                    {
                        CheckBox chkSelect = row.FindControl("chkSelect") as CheckBox;
                        if (chkSelect.Enabled && chkSelect.Checked)
                            Count += 1;
                    }

                    if (Count == 0)
                    {
                        lblError.Text = "Please select at least one item for CheckIn";
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
                string LocationPartDetailId = "";
                GridView ChildGrid;

                foreach (GridViewRow row in gridKitTable.Rows)
                {
                    chkSelect = row.FindControl("chkSelect") as CheckBox;
                    if (chkSelect.Enabled && chkSelect.Checked)
                    {

                        ChildGrid = row.FindControl("grdChild") as GridView;
                        foreach (GridViewRow childRow in ChildGrid.Rows)
                        {
                            BuildKitId = Convert.ToInt64((row.FindControl("hdnBuildKitId") as HiddenField).Value);
                            CaseKitId = Convert.ToInt64((row.FindControl("hdnCaseKitId") as HiddenField).Value);

                            TableXml.Append("<CasesTable>");
                            TableXml.Append("<CaseKitId>"); TableXml.Append(CaseKitId); TableXml.Append("</CaseKitId>");
                            TableXml.Append("<BuildKitId>"); TableXml.Append(BuildKitId); TableXml.Append("</BuildKitId>");

                            LocationPartDetailId = (childRow.FindControl("hdnLocPartDetailId") as HiddenField).Value;
                            if (LocationPartDetailId.Contains(","))
                            {
                                string[] arr = LocationPartDetailId.Split(',');
                                LocationPartDetailId = arr[0];
                            }

                            TableXml.Append("<LocationPartDetailId>"); TableXml.Append(LocationPartDetailId); TableXml.Append("</LocationPartDetailId>");

                            if (this.PartyId > 0)
                            {
                                //Checked in case for Hospital                                                        
                                if ((childRow.FindControl("radConsume") as RadioButton).Checked)
                                {
                                    TableXml.Append("<Status>"); TableXml.Append("Consumed"); TableXml.Append("</Status>");
                                }
                                else if ((childRow.FindControl("radMissing") as RadioButton).Checked)
                                {
                                    TableXml.Append("<Status>"); TableXml.Append("Missing"); TableXml.Append("</Status>");
                                }
                                else if ((childRow.FindControl("radDamage") as RadioButton).Checked)
                                {
                                    TableXml.Append("<Status>"); TableXml.Append("Damaged"); TableXml.Append("</Status>");
                                }
                                else if ((childRow.FindControl("radAvailable") as CheckBox).Checked)
                                {
                                    TableXml.Append("<Status>"); TableXml.Append("AssignedToKit"); TableXml.Append("</Status>");
                                }

                            }
                            else
                            {
                                //Checked in case for Branch / Region                            
                                if ((childRow.FindControl("radMissing") as RadioButton).Checked)
                                {
                                    TableXml.Append("<Status>"); TableXml.Append("Missing"); TableXml.Append("</Status>");
                                }
                                else if ((childRow.FindControl("radDamage") as RadioButton).Checked)
                                {
                                    TableXml.Append("<Status>"); TableXml.Append("Damaged"); TableXml.Append("</Status>");
                                }
                                else if ((childRow.FindControl("radAvailable") as CheckBox).Checked)
                                {
                                    TableXml.Append("<Status>"); TableXml.Append("AssignedToKit"); TableXml.Append("</Status>");
                                }
                            }

                            TableXml.Append("</CasesTable>");
                        }

                    }

                }

            }
            else
            {
                Int64 CasePartId = 0, LocationPartDetailId = 0;
                string PartStatus = "", PartNum = "";
                GridView gdv = null;
                CheckBox chkSendReplacement;

                if (this.CaseTypeByCaseId == "ReturnInventoryRMATransfer")
                    gdv = gridRMA;
                else
                    gdv = gridCatalog;

                foreach (GridViewRow row in gdv.Rows)
                {
                    chkSelect = row.FindControl("chkSelect") as CheckBox;
                    if (chkSelect.Enabled && chkSelect.Checked)
                    {
                        CasePartId = Convert.ToInt64((row.FindControl("hdnCasePartId") as HiddenField).Value);
                        LocationPartDetailId = Convert.ToInt64((row.FindControl("hdnLocPartDetailId") as HiddenField).Value);
                        PartNum = (row.FindControl("lblCatalogNum") as Label).Text;
                        
                        TableXml.Append("<CasesTable>");
                        
                        if (this.CaseTypeByCaseId == "ReturnInventoryRMATransfer")
                        {     
                            if ((row.FindControl("radAvailable") as RadioButton).Checked)
                                PartStatus = "Available";
                            else if ((row.FindControl("radMissing") as RadioButton).Checked)
                                PartStatus = "Missing";
                            else if ((row.FindControl("radDamage") as RadioButton).Checked)
                                PartStatus = "Damaged";
                            else if ((row.FindControl("radExpire") as RadioButton).Checked)
                                PartStatus = "Expired";

                            chkSendReplacement = row.FindControl("chkSendReplacement") as CheckBox;
                            
                            TableXml.Append("<LocationPartDetailId>"); TableXml.Append(LocationPartDetailId); TableXml.Append("</LocationPartDetailId>");
                            TableXml.Append("<CasePartId>"); TableXml.Append(CasePartId); TableXml.Append("</CasePartId>");
                            TableXml.Append("<SendReplacement>"); TableXml.Append(chkSendReplacement.Checked); TableXml.Append("</SendReplacement>");
                            TableXml.Append("<PartNum>"); TableXml.Append(PartNum); TableXml.Append("</PartNum>");
                            TableXml.Append("<PartStatus>"); TableXml.Append(PartStatus); TableXml.Append("</PartStatus>");
                        }
                        else
                        {
                            TableXml.Append("<LocationPartDetailId>"); TableXml.Append(LocationPartDetailId); TableXml.Append("</LocationPartDetailId>");
                            TableXml.Append("<CasePartId>"); TableXml.Append(CasePartId); TableXml.Append("</CasePartId>");
                            TableXml.Append("<SendReplacement>"); TableXml.Append("0"); TableXml.Append("</SendReplacement>");
                            TableXml.Append("<PartNum>"); TableXml.Append(PartNum); TableXml.Append("</PartNum>");
                            TableXml.Append("<PartStatus>"); TableXml.Append(""); TableXml.Append("</PartStatus>");
                        }

                        TableXml.Append("</CasesTable>");

                    }

                }


            }         

            TableXml.Append("</root>");

            return TableXml.ToString();
        }

        private void LocalizePage()
        {
            lblHeader.Text = vctResource.GetString("WebCheckIn_lblHeader");
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
                GridView gdv;
                if (this.CaseTypeByCaseId == "ReturnInventoryRMATransfer")
                    gdv = gridRMA;
                else
                    gdv = gridCatalog;

                foreach (GridViewRow row in gdv.Rows)
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
                if (CaseStatus == "Shipped" || CaseStatus == "PartiallyShipped")
                    FinalStatus = "PartiallyCheckedIn";
                else
                    FinalStatus = CaseStatus;
            }
            else
            {
                if (CaseStatus == "Shipped" || CaseStatus == "PartiallyCheckedIn")
                    FinalStatus = "CheckedIn";
                else
                    FinalStatus = CaseStatus;
            }

            //if (CaseState == "Full" && CaseStatus == "PartiallyCheckedIn")
            //{
            //    FinalStatus = "CheckedIn";
            //}
            //else if (CaseState == "Partial" && CaseStatus == "PartiallyCheckedIn")
            //{
            //    FinalStatus = "PartiallyCheckedIn";
            //}
            //else if (CaseState == "Partial" && CaseStatus == "PartiallyShipped")
            //{
            //    FinalStatus = "PartiallyCheckedIn";
            //}
                      
            return FinalStatus;
        }

        private void AuthorizedPage()
        {
            security = new Security();
            if (Session["LoggedInUser"] == null)
            {
                Response.Redirect("~/ErrorPage.aspx?ErrorKey=Common_msgSessionExpired");
            }
            else if (security.HasAccess("VirtualCheckIn"))
            {
                CanView = security.HasPermission("VirtualCheckIn");
            }
            else
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }
        #endregion

        #region Protected Methods
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
                CheckBox chkSelect = e.Row.FindControl("chkSelect") as CheckBox;
                if (KitStatus != "Shipped")
                {
                    //CheckBox chkSelect = e.Row.FindControl("chkSelect") as CheckBox;
                    chkSelect.Checked = true;
                    chkSelect.Enabled = false;
                } 

                GridView grdChild = (GridView)e.Row.FindControl("grdChild");
                BuildKitId =  Convert.ToInt64((e.Row.FindControl("hdnBuildKitId") as HiddenField).Value);
                CaseKitId = Convert.ToInt64((e.Row.FindControl("hdnCaseKitId") as HiddenField).Value);

                grdChild.DataSource = presenter.PopulateCheckOutKitByCaseKitId(CaseKitId);
                grdChild.DataBind();
                if(chkSelect.Checked)                {
                    
                    grdChild.Enabled = false;
                }
            }
        }

        protected void grdChild_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Int64 PartyId = 0;
            if (this.PartyId.HasValue)
            {
                PartyId = Convert.ToInt64(this.PartyId);
            }
            if (PartyId == 0)
            {
                e.Row.Cells[5].Visible = false;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                bool IsNearExpiry = Convert.ToBoolean((e.Row.FindControl("hdnIsNearExpiry") as HiddenField).Value);
                if (IsNearExpiry)
                    e.Row.ForeColor = System.Drawing.Color.Red;

                PartStatus = (e.Row.FindControl("hdnPartStatus") as HiddenField).Value;

                if (PartStatus == "AssignedToKit")
                    (e.Row.FindControl("radAvailable") as RadioButton).Checked = true;
                else if (PartStatus == "Consumed")
                    (e.Row.FindControl("radConsume") as RadioButton).Checked = true;
                else if (PartStatus == "Missing")
                    (e.Row.FindControl("radMissing") as RadioButton).Checked = true;
                else if (PartStatus == "Damaged")
                    (e.Row.FindControl("radDamage") as RadioButton).Checked = true;

                string SelRepPartStatus = (e.Row.FindControl("hdnPartStatus") as HiddenField).Value;
               

            }
        }

        protected void gridCatalog_RowDataBound(object sender, GridViewRowEventArgs e)
        {           
            if (e.Row.RowType == DataControlRowType.Header)
            {
                (e.Row.FindControl("lblPartNoHeader") as Label).Text = vctResource.GetString("WebCheckOut_PartNo");
                //(e.Row.FindControl("lblDescriptionHeader") as Label).Text = vctResource.GetString("WebCheckOut_Description");
                (e.Row.FindControl("lblLotNumHeader") as Label).Text = vctResource.GetString("WebCheckOut_AssignLotNo");
                (e.Row.FindControl("lblExpiryDateHeader") as Label).Text = vctResource.GetString("WebCheckOut_ExpirayDate");
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                PartStatus = (e.Row.FindControl("lblPartStatus") as Label).Text;
                if (PartStatus != "Shipped")
                {
                    CheckBox chkSelect = e.Row.FindControl("chkSelect") as CheckBox;
                    chkSelect.Checked = true;
                    chkSelect.Enabled = false;
                } 

                IsNearExpiry = Convert.ToBoolean((e.Row.FindControl("hdnIsNearExpiry") as HiddenField).Value);
                if (IsNearExpiry)
                    e.Row.ForeColor = System.Drawing.Color.Red;

            }
        }

        protected void gridRMA_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        
            if (e.Row.RowType == DataControlRowType.Header)
            {
                (e.Row.FindControl("lblPartNoHeader") as Label).Text = vctResource.GetString("WebCheckOut_PartNo");
                //(e.Row.FindControl("lblDescriptionHeader") as Label).Text = vctResource.GetString("WebCheckOut_Description");
                (e.Row.FindControl("lblLotNumHeader") as Label).Text = vctResource.GetString("WebCheckOut_AssignLotNo");
                (e.Row.FindControl("lblExpiryDateHeader") as Label).Text = vctResource.GetString("WebCheckOut_ExpirayDate");
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                PartStatus = (e.Row.FindControl("hdnPartStatus") as HiddenField).Value;
                string LocationPartDetailId = (e.Row.FindControl("hdnLocPartDetailId") as HiddenField).Value;
                CheckBox chkSelect = e.Row.FindControl("chkSelect") as CheckBox;
                
                if ((LocationPartDetailId == "0" && (!string.IsNullOrEmpty(PartStatus))) || (LocationPartDetailId != "0" && PartStatus != "Shipped"))
                {                    
                    chkSelect.Checked = true;
                    chkSelect.Enabled = false;
                }

                IsNearExpiry = Convert.ToBoolean((e.Row.FindControl("hdnIsNearExpiry") as HiddenField).Value);
                if (IsNearExpiry)
                    e.Row.ForeColor = System.Drawing.Color.Red;
                                
                if (PartStatus == "AssignedToKit")
                    (e.Row.FindControl("radAvailable") as RadioButton).Checked = true;               
                else if (PartStatus == "Missing")
                    (e.Row.FindControl("radMissing") as RadioButton).Checked = true;
                else if (PartStatus == "Damaged")
                    (e.Row.FindControl("radDamage") as RadioButton).Checked = true;
                else if (PartStatus == "Expired")
                    (e.Row.FindControl("radExpire") as RadioButton).Checked = true;
                                
                string PartDesc = (e.Row.FindControl("hdnDescription") as HiddenField).Value;

                Label lblPartNum = e.Row.FindControl("lblCatalogNum") as Label;
                lblPartNum.CssClass = "label-title";
                lblPartNum.ToolTip = PartDesc;

                Label lblLotNum = e.Row.FindControl("lblLotNum") as Label;
                lblLotNum.CssClass = "label-title";
                lblLotNum.ToolTip = PartDesc;

                Label lblDispositionType = e.Row.FindControl("lblDispositionType") as Label;
                if (lblDispositionType.Text.ToLower() == "others")
                {
                    lblDispositionType.CssClass = "label-title";
                }
                lblDispositionType.ToolTip = (e.Row.FindControl("hdnRemarks") as HiddenField).Value;

                CheckBox chkSeekReturn = e.Row.FindControl("chkSeekReturn") as CheckBox;
                

                if (!(chkSelect.Enabled && chkSeekReturn.Checked))
                {
                    CheckBox chkSendReplacement = e.Row.FindControl("chkSendReplacement") as CheckBox;
                    bool SendReplacementFlag = Convert.ToBoolean((e.Row.FindControl("hdnSendReplacement") as HiddenField).Value);
                    chkSendReplacement.Checked = SendReplacementFlag;
                    chkSendReplacement.Enabled = false;
                }
                //else
                //{
                //    bool SendReplacement = Convert.ToBoolean((e.Row.FindControl("hdnSendReplacement") as HiddenField).Value);
                //    chkSendReplacement.Checked = SendReplacement;
                //    chkSendReplacement.Enabled = false;
                //}
                    
            }
        }
        
        

        #endregion
               
    }
}

