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
using VCTWebApp.Shell.Views;

namespace VCTWebApp
{
    public partial class VirtualReCheckIn : Microsoft.Practices.CompositeWeb.Web.UI.Page, IVirtualReCheckInView
    {

        #region Private Variables
        
        GridView grdChild;
        Int64 BuildKitId, CaseKitId;
        CheckBox chkKitSelect, chkPartSelect;

        #endregion

        #region Instance Variables
        private VirtualReCheckInPresenter presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        #endregion

        #region Create New Presenter
        [CreateNew]
        public VirtualReCheckInPresenter Presenter
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

        #region IVirtualReCheckInView Implementations

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
                gridCatalog.DataSource = value;
                gridCatalog.DataBind();
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

        public Int64? PartyId
        {
            get
            {
                return (Int64)ViewState["PartyId"];
            }
            set
            {
                if (value.HasValue)
                    ViewState["PartyId"] = value;
                else
                    ViewState["PartyId"] = "0";
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
                    lstPendingCases.Height = 470;
                    pnlKitGrid.Visible = true;
                    pnlDetailKit.Visible = true;
                    pnlPartGrid.Visible = false;
                }
                else
                {
                    pnlKitGrid.Height = 120;
                    lstPendingCases.Height = 350;
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
                if (!presenter.Save())
                {
                    lblError.Text = "Problem in Web CheckIn Adjustment for selected Kit/Part #. The selected Kit/Part(s) may be already ReCheckIn. Please try after refreshing the page.";                                    
                }
                else
                {
                    ClearFields();
                    lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgSave"), this.lblHeader.Text) + "</font>";

                }
                //else
                //{
                //    lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgCommonError"), this.lblHeader.Text) + "</font>";
                //}
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
                    if (this.InventoryType == Constants.InventoryType.Kit.ToString())
                    {
                        if (gridKitTable.Rows.Count < 1)
                        {
                            lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("WebCheckIn_KitMissing"), this.lblHeader.Text);
                            return false;
                        }
                    }
                    else
                    {
                        if (gridCatalog.Rows.Count < 1)
                        {
                            lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("WebCheckIn_PartMissing"), this.lblHeader.Text);
                            return false;
                        }
                    }
                }

                Int32 Count = 0;
                string OldStatus;
                foreach (GridViewRow row in gridKitTable.Rows)
                {
                    CheckBox chkKitSelect = row.FindControl("chkKitSelect") as CheckBox;
                    if ((chkKitSelect.Enabled) && (chkKitSelect.Checked))
                    {

                        GridView ChildGrid = row.FindControl("grdChild") as GridView;
                        foreach (GridViewRow childRow in ChildGrid.Rows)
                        {
                            OldStatus = "";
                            OldStatus = (childRow.FindControl("hdnPartStatus") as HiddenField).Value.ToUpper();

                            if (OldStatus.Contains(","))
                            {
                                OldStatus = OldStatus.Substring(0, OldStatus.IndexOf(','));
                            }

                            CheckBox chkPartSelect = childRow.FindControl("chkPartSelect") as CheckBox;
                             if (OldStatus == "MISSING" && chkPartSelect.Enabled && chkPartSelect.Checked)
                             {
                                 //if (OldStatus == "MISSING")
                                 //{
                                 if (!(childRow.FindControl("radMissing") as RadioButton).Checked)
                                 {
                                     Count += 1;
                                 }
                                 //}
                             }
                        }

                    }
                }

                if (Count == 0)
                {
                    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("WebCheckIn_PartMissing"), this.lblHeader.Text);
                    return false;
                }

            }

            return true;
        }

        private string CreateTableXml()
        {
            System.Text.StringBuilder TableXml = new System.Text.StringBuilder();
            TableXml.Append("<root>");

            string LocationPartDetailId, PartCurrentStatus, PartStatus;
            GridView ChildGrid;

            foreach (GridViewRow row in gridKitTable.Rows)
            {
                chkKitSelect = row.FindControl("chkKitSelect") as CheckBox;
                if ((chkKitSelect.Enabled) && (chkKitSelect.Checked))
                {
                    CaseKitId = Convert.ToInt64((row.FindControl("hdnCaseKitId") as HiddenField).Value);
                    BuildKitId = Convert.ToInt64((row.FindControl("hdnBuildKitId") as HiddenField).Value);
                    ChildGrid = row.FindControl("grdChild") as GridView;

                    foreach (GridViewRow childRow in ChildGrid.Rows)
                    {
                         chkPartSelect = childRow.FindControl("chkPartSelect") as CheckBox;
                         if ((chkPartSelect.Enabled) && (chkPartSelect.Checked))
                         {
                             PartCurrentStatus = "";

                             PartStatus = (childRow.FindControl("hdnPartStatus") as HiddenField).Value.ToUpper();

                             if (PartStatus.Contains(","))
                             {
                                 PartStatus = PartStatus.Substring(0, PartStatus.IndexOf(','));
                             }

                             if (PartStatus == "MISSING")
                             {                                 
                                 LocationPartDetailId = (childRow.FindControl("hdnLocPartDetailId") as HiddenField).Value;
                                 if (LocationPartDetailId.Contains(","))
                                 {
                                     string[] arr = LocationPartDetailId.Split(',');
                                     LocationPartDetailId = arr[0];
                                 }

                                 if ((childRow.FindControl("radAvailable") as RadioButton).Checked == true)
                                     PartCurrentStatus = "Available";
                                 else if ((childRow.FindControl("radConsume") as RadioButton).Checked == true)
                                     PartCurrentStatus = "Consumed";
                                 else if ((childRow.FindControl("radMissing") as RadioButton).Checked == true)
                                     PartCurrentStatus = "Missing";
                                 else if ((childRow.FindControl("radDamage") as RadioButton).Checked == true)
                                     PartCurrentStatus = "Damaged";

                                 TableXml.Append("<CasesTable>");
                                 TableXml.Append("<CaseKitId>"); TableXml.Append(CaseKitId); TableXml.Append("</CaseKitId>");
                                 TableXml.Append("<BuildKitId>"); TableXml.Append(BuildKitId); TableXml.Append("</BuildKitId>");
                                 TableXml.Append("<LocationPartDetailId>"); TableXml.Append(LocationPartDetailId); TableXml.Append("</LocationPartDetailId>");                                 
                                 TableXml.Append("<PartStatus>"); TableXml.Append(PartCurrentStatus); TableXml.Append("</PartStatus>");
                                 TableXml.Append("</CasesTable>");
                             }

                         }

                    }

                }

            }

            TableXml.Append("</root>");

            return TableXml.ToString();
        }

        private void LocalizePage()
        {
            lblHeader.Text = vctResource.GetString("WebReCheckIn_lblHeader");
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
                grdChild = (GridView)e.Row.FindControl("grdChild");
                BuildKitId = Convert.ToInt64((e.Row.FindControl("hdnBuildKitId") as HiddenField).Value);
                CaseKitId = Convert.ToInt64((e.Row.FindControl("hdnCaseKitId") as HiddenField).Value);

                List<VCTWeb.Core.Domain.VirtualCheckOut> lstKitFamily = presenter.PopulateCheckOutBuildKitById(CaseKitId);

                chkKitSelect = (e.Row.FindControl("chkKitSelect") as CheckBox);
                if (lstKitFamily != null && lstKitFamily.Exists(t => t.PartStatus.ToUpper() == "MISSING"))                                    
                    chkKitSelect.Enabled = true;
                else
                    chkKitSelect.Enabled = false;

                grdChild.DataSource = lstKitFamily;
                grdChild.DataBind();
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
                {
                    e.Row.ForeColor = System.Drawing.Color.Red;
                }

                string PartStatus = (e.Row.FindControl("hdnPartStatus") as HiddenField).Value.ToUpper();

                if (PartStatus == "ASSIGNEDTOKIT" || PartStatus == "AVAILABLE")
                {
                    (e.Row.FindControl("radAvailable") as RadioButton).Checked = true;
                }
                else if (PartStatus == "CONSUMED")
                {
                    (e.Row.FindControl("radConsume") as RadioButton).Checked = true;
                }
                else if (PartStatus == "MISSING")
                {
                    (e.Row.FindControl("radMissing") as RadioButton).Checked = true;
                }
                else if (PartStatus == "DAMAGED")
                {
                    (e.Row.FindControl("radDamage") as RadioButton).Checked = true;
                }

                if (PartStatus != "MISSING")
                {
                    CheckBox chkPartSelect = (e.Row.FindControl("chkPartSelect") as CheckBox);
                    chkPartSelect.Enabled = false;
                    (e.Row.FindControl("radAvailable") as RadioButton).Enabled = false;
                    (e.Row.FindControl("radConsume") as RadioButton).Enabled = false;
                    (e.Row.FindControl("radMissing") as RadioButton).Enabled = false;
                    (e.Row.FindControl("radDamage") as RadioButton).Enabled = false;
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

