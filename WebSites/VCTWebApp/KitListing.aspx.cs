using System;
using Microsoft.Practices.ObjectBuilder;
using System.Data;
using System.Web.UI.WebControls;
using VCTWebApp.Resources;
using System.Data.SqlClient;
using System.Globalization;
using System.Collections.Generic;
using System.Text;
using VCTWebApp.Web;

namespace VCTWebApp.Shell.Views
{
    public partial class KitListing : Microsoft.Practices.CompositeWeb.Web.UI.Page, IKitListingView
    {

        #region Instance Variables
        private KitListingPresenter _presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private Security security = null;
        #endregion

        #region Create New Presenter
        [CreateNew]
        public KitListingPresenter Presenter
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
            ClearNotifications();
            if (!this.IsPostBack)
            {
                this.AuthorizedPage();
                this._presenter.OnViewInitialized();
                this.LocalizePage();

                //btnDelete.Attributes.Add("onclick", "javascript:return " + "confirm('" + vctResource.GetString("msgDeleteConfirm") + "')");
            }
        }
        #endregion              

        #region IKitListingView Implementations

        //public List<VCTWeb.Core.Domain.Procedures> ProcedureList
        //{
        //    set
        //    {
        //        this.ddlProcedureName.DataSource = value;
        //        this.ddlProcedureName.DataTextField = "Name";
        //        this.ddlProcedureName.DataValueField = "Name";
        //        this.ddlProcedureName.DataBind();

        //        this.ddlProcedureName.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect"), string.Empty));
        //    }
        //}

        public Int64 SelectedKitFamilyId
        {
            get
            {
                //return (Convert.ToInt64(ddlHeadKitFamily.SelectedValue));
                return (Convert.ToInt64(hdnHeadKitFamilyId.Value));
            }
        }

        public List<VCTWeb.Core.Domain.KitTable> KitTableList
        {
            get
            {
                return (List<VCTWeb.Core.Domain.KitTable>)ViewState["KitTableList"];
            }
            set
            {
                ViewState["KitTableList"] = value;
                this.gridKitTable.DataSource = value;
                this.gridKitTable.DataBind();
                if (value.Count > 0 && String.IsNullOrEmpty(value[0].Catalognumber))
                {
                    this.gridKitTable.Rows[0].Visible = false;
                }
            }
        }

        public List<VCTWeb.Core.Domain.KitListing> KitListingList
        {
            set
            {
                lstExistingKit.DataSource = value;
                lstExistingKit.DataTextField = "KitNumber";
                lstExistingKit.DataValueField = "KitNumber";
                lstExistingKit.DataBind();

            }
        }

        private void RemoveEmptyRow(List<VCTWeb.Core.Domain.KitTable> value)
        {
            if (value != null)
            {
                var item = value.Find(t => t.Catalognumber == null);
                if (item != null)
                {
                    value.Remove(item);
                }
            }
        }

        public List<VCTWeb.Core.Domain.KitFamily> KitFamilyList
        {
            set
            {
                //this.ddlHeadKitFamily.DataSource = value;
                //this.ddlHeadKitFamily.DataTextField = "KitFamilyName";
                //this.ddlHeadKitFamily.DataValueField = "KitFamilyId";
                //this.ddlHeadKitFamily.DataBind();

                //this.ddlHeadKitFamily.Items.Insert(0, new ListItem(vctResource.GetString("DropDownlistItemAll"), "0"));

                //this.ddlKitFamily.DataSource = value;
                //this.ddlKitFamily.DataTextField = "KitFamilyName";
                //this.ddlKitFamily.DataValueField = "KitFamilyId";
                //this.ddlKitFamily.DataBind();

                //this.ddlKitFamily.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect"), "0"));
            }
        }

        public string Procedure
        {
            get
            {
                return null;
                //if (ddlProcedureName.SelectedIndex > 0)
                //{
                //    return ddlProcedureName.SelectedValue;
                //}
                //return null;
                //if (string.IsNullOrEmpty(txtProcedureName.Text))
                //{
                //    return null;
                //}
                //return txtProcedureName.Text.Trim();
            }
            set
            {
                //if (value == null)
                //    ddlProcedureName.SelectedIndex = 0;
                //else
                //    ddlProcedureName.SelectedValue = value.ToString();
                //txtProcedureName.Text = value.ToString().Trim();
            }
        }

        public VCTWeb.Core.Domain.KitTable IndiviualKitItem { get; set; }

        public string KitTableXml
        {
            get
            {
                return CreateKitTableXml();
            }
        }

        public string SelectedKitNumber
        {
            get
            {
                if (lstExistingKit.SelectedIndex >= 0)
                {
                    return lstExistingKit.SelectedValue.ToString();
                }
                return null;
            }
        }

        public string KitNumber
        {
            get { return txtKitNumber.Text; }
            set { txtKitNumber.Text = value; }
        }

        public string KitName
        {
            get { return txtKitName.Text; }
            set { txtKitName.Text = value; }
        }

        public string KitDescription
        {
            get { return txtKitDescription.Text; }
            set { txtKitDescription.Text = value; }
        }

        public int? NumberOfSets
        {
            get { return null; }
        }

        public string Aisle
        {
            get { return string.Empty; }
        }

        public string Row
        {
            get { return string.Empty; }
        }

        public string Tier
        {
            get { return string.Empty; }
        }

        public DateTime? DateCreated
        {
            get { return null; }
        }

        public DateTime? PMSchedule
        {
            get { return null; }
        }

        public string Lubricate
        {
            get { return string.Empty; }
        }

        public bool IsManuallyAdded
        {
            get { return true; }
        }

        public Int64? KitFamilyId
        {
            get
            {
               

                if (string.IsNullOrEmpty(txtKitFamily.Text.Trim()))
                {
                    return null;
                }
                return Convert.ToInt64(hdnKitFamilyId.Value);

            }
            set
            {
              
                if (value == 0 || value == null)
                {
                    txtKitFamily.Text = "";
                    hdnKitFamilyId.Value = "0";
                }
                else
                {
                   

                    txtKitFamily.Text = this.KitName;
                    hdnKitFamilyId.Value = value.ToString();   
                    
                }
            }
        }

        public string KitFamily
        {
            get
            {                
                return Convert.ToString(txtKitFamily.Text.Trim());
            }
            set {

                txtKitFamily.Text = value;
            }
           
        }

        public string KitFamilyHead
        {
            get
            {
                return Convert.ToString(txtHeadKitFamily.Text.Trim());
            }

        }

        public int LocationId
        {
            get
            {                
                return (Convert.ToInt32(Session["LoggedInLocationId"]));
            }
           
        }

        public Decimal RentalFee
        {
            get {
                if (string.IsNullOrEmpty(txtRentalFee.Text))
                    return 0;
                else
                    return Convert.ToDecimal(txtRentalFee.Text);
            }
            set { 
                txtRentalFee.Text = Convert.ToString(value); 
            }
        }

        public bool IsActive
        {
            get { return chkIsActive.Checked; }
            set { chkIsActive.Checked = value; }
        }
        #endregion

        #region Event Handlers
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (Presenter.Delete())
                {
                    lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgKitDelete"), this.lblHeader.Text) + "</font>";
                    ClearFields();
                    EnableDisableControls();
                    Presenter.PopulateKitListingList();
                    Presenter.PopulateEmptyKitTable();
                    txtKitNumber.Attributes.Add("Style", "background-color:#FFFFFF;");
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                ClearFields();
                Presenter.PopulateEmptyKitTable();
                EnableDisableControls();
                txtKitNumber.Attributes.Add("Style", "background-color:#FFFFFF;");

            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateInputs())
                {

                    if (Presenter.SaveKitListing())
                    {
                        lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgKitListingSaved"), this.lblHeader.Text) + "</font>";
                        ClearFields();
                        EnableDisableControls();
                        Presenter.PopulateKitListingList();
                        Presenter.PopulateEmptyKitTable();
                    }
                    else
                    {
                        lblError.Text = vctResource.GetString("msgUnableToSaveRecord");
                        //lblError.Text = "Unable to Save KitListing";
                    }

                }
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        #endregion

        #region Protected Methods
        //protected void gridKitTable_RowEditing(object sender, GridViewEditEventArgs e)
        //{
        //    try
        //    {
        //        gridKitTable.EditIndex = e.NewEditIndex;

        //        this.KitTableList = this.KitTableList;
        //    }
        //    catch (Exception ex)
        //    {
        //        this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
        //    }
        //}

        //protected void gridKitTable_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        //{
        //    try
        //    {
        //        gridKitTable.EditIndex = -1;

        //        this.KitTableList = this.KitTableList;
        //    }
        //    catch (Exception ex)
        //    {
        //        this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
        //    }
        //}              

        //protected void ddlHeadKitFamily_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    _presenter.SetFieldsBlanks();            
        //    _presenter.PopulateKitListingList();
        //}

        protected void txtHeadKitFamily_TextChanged(object sender, EventArgs e)
        {
            _presenter.SetFieldsBlanks();
            _presenter.PopulateKitListingList();
        }

        //protected void ddlKitFamily_SelectedIndexChanged(object sender, EventArgs e)
        //{            
        //    Presenter.PopulateKitTableList();
        //}
        protected void txtKitFamily_TextChanged(object sender, EventArgs e)
        {
            if (KitFamilyId == 0)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valKitFamily"), this.lblHeader.Text);
                return;
            }
            else
            {
                Presenter.PopulateKitTableList();
            }
           
        }

        protected void gridKitTable_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    this.LocalizeKitTableGrid(e);
                }
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        protected void gridKitTable_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("UpdateRec"))
                {
                    //TextBox txtCatalogNum = gridKitTable.Rows[gridKitTable.EditIndex].FindControl("txtCatalogNum") as TextBox;
                    TextBox txtCatalogDesc = gridKitTable.Rows[gridKitTable.EditIndex].FindControl("txtCatalogDesc") as TextBox;
                    TextBox txtQuantity = gridKitTable.Rows[gridKitTable.EditIndex].FindControl("txtQuantity") as TextBox;
                    if (txtCatalogDesc != null && txtQuantity != null)
                    {
                        bool valid = ValidateItems(txtCatalogDesc.Text, txtQuantity.Text);
                        if (valid == true)
                        {
                            //this.KitTableList[gridKitTable.EditIndex].Catalognumber = txtCatalogNum.Text;
                            this.KitTableList[gridKitTable.EditIndex].Description = txtCatalogDesc.Text;
                            this.KitTableList[gridKitTable.EditIndex].Quantity = Convert.ToInt32(txtQuantity.Text);

                            // Edit Mode
                            if (lstExistingKit.SelectedIndex >= 0)
                            {
                                this.IndiviualKitItem = this.KitTableList[gridKitTable.EditIndex];
                                if (Presenter.UpdateIndiviualKitItems())
                                {
                                    lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgKitItemUpdate"), this.lblHeader.Text) + "</font>";
                                    gridKitTable.EditIndex = -1;
                                    this.KitTableList = this.KitTableList;

                                }
                            }
                            else
                            {
                                gridKitTable.EditIndex = -1;
                                this.KitTableList = this.KitTableList;
                            }
                        }
                    }
                }
                if (e.CommandName.Equals("AddNewRow"))
                {
                    if (hdnCatalogNumberNew.Value == string.Empty)
                    {
                        lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valCatalogNumber"), this.lblHeader.Text);
                    }
                    else
                    {

                        TextBox txtNewCatalogNum = gridKitTable.FooterRow.FindControl("txtNewCatalogNum") as TextBox;
                        TextBox txtNewCatalogDesc = gridKitTable.FooterRow.FindControl("txtNewCatalogDesc") as TextBox;
                        TextBox txtNewQuantity = gridKitTable.FooterRow.FindControl("txtNewQuantity") as TextBox;


                        if (txtNewCatalogNum != null && txtNewCatalogDesc != null && txtNewQuantity != null)
                        {
                            bool valid = ValidateItemsOnAdd(txtNewCatalogNum.Text, txtNewCatalogDesc.Text, txtNewQuantity.Text);
                            if (valid == true)
                            {
                                this.KitTableList.Add(new VCTWeb.Core.Domain.KitTable()
                                {
                                    KitNumber = txtKitNumber.Text,
                                    ItemNumber = this.KitTableList.Count + 1,
                                    Catalognumber = txtNewCatalogNum.Text,
                                    Description = txtNewCatalogDesc.Text,
                                    Quantity = Convert.ToInt32(txtNewQuantity.Text)
                                });
                                RemoveEmptyRow(this.KitTableList);
                                this.KitTableList = this.KitTableList;
                                // Edit Mode
                                if (lstExistingKit.SelectedIndex >= 0)
                                {
                                    IndiviualKitItem = KitTableList[KitTableList.Count - 1];
                                    if (Presenter.AddIndiviualKitItems())
                                    {
                                        lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgKitItemAdded"), this.lblHeader.Text) + "</font>";
                                    }
                                }
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                }
                if (e.CommandName.Equals("DeleteRec"))
                {
                    if (Convert.ToInt32(e.CommandArgument) > -1)
                    {
                        // Edit Mode
                        if (lstExistingKit.SelectedIndex >= 0)
                        {
                            IndiviualKitItem = KitTableList[Convert.ToInt32(e.CommandArgument)];
                            if (Presenter.DeleteIndiviualKitItems())
                            {
                                lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgKitItemDeleted"), this.lblHeader.Text) + "</font>";
                            }
                        }
                        //List<VCTWeb.Core.Domain.KitTable> list = (List<VCTWeb.Core.Domain.KitTable>)ViewState["KitTableList"];
                        this.KitTableList.RemoveAt(Convert.ToInt32(e.CommandArgument));
                        if (this.KitTableList.Count == 0)
                        {
                            this.KitTableList.Add(new VCTWeb.Core.Domain.KitTable());
                            this.KitTableList = this.KitTableList;
                        }
                        else
                        {
                            this.KitTableList = this.KitTableList;
                        }
                    }
                }

                //gridKitTable.EditIndex = -1;
                //gridKitTable.DataSource = KitTableList;
                //gridKitTable.DataBind();                
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        protected void lstExistingKit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.lblError.Text = string.Empty;
                txtKitNumber.ReadOnly = true;
                txtKitNumber.Attributes.Add("Style", "background-color:#e4e4e4;");
                //btnDelete.Enabled = true;
                Presenter.OnViewLoaded();
                Presenter.PopulateKitTableList();
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
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
            else if (security.HasAccess("KitListing"))
            {
                //CanCancel = security.HasPermission("KitListing.Manage");
            }
            else
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }

        private void LocalizeKitTableGrid(GridViewRowEventArgs e)
        {
            try
            {
                e.Row.Cells[0].Text = vctResource.GetString("lblPartNumHeader");
                e.Row.Cells[1].Text = vctResource.GetString("lblDescriptionHeader");
                e.Row.Cells[2].Text = vctResource.GetString("lblActionHeader");
            }
            catch (Exception)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("Error_msgUnknownError"), this.lblHeader.Text);
            }
        }

        private void LocalizePage()
        {
            try
            {
                string heading = string.Empty;
                heading = vctResource.GetString("mnuKitListing");
                lblHeader.Text = heading;
                Page.Title = heading;

                this.lblExistingKit.Text = vctResource.GetString("lblExistingKit");
                this.lblKitFamily.Text = vctResource.GetString("lblKitFamily");
                this.lblKitDescription.Text = vctResource.GetString("lblKitDescription");
                //this.lblLocation.Text = vctResource.GetString("lblLocation");
                this.lblInventoryItem.Text = vctResource.GetString("lblInventoryItem");
                this.chkIsActive.Text = vctResource.GetString("chkIsActive");
                this.lblKitNumber.Text = vctResource.GetString("lblKitNumber");
                this.lblKitName.Text = vctResource.GetString("lblKitName");

                String errorMessage = vctResource.GetString("required");

                this.rfv_KitNumber.ErrorMessage = errorMessage;
                this.rfv_KitName.ErrorMessage = errorMessage;
                this.rfv_KitFamily.ErrorMessage = errorMessage;

                //this.lblProcedureName.Text = vctResource.GetString("lblProcedureNameKitListing");
                //this.btnNew.Text = vctResource.GetString("btnReset");
                //this.btnSave.Text = vctResource.GetString("btnSave");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void EnableDisableControls()
        {
            txtKitNumber.ReadOnly = false;
            txtKitNumber.Enabled = true;
            txtKitNumber.Attributes.Remove("style");
            //txtKitNumber.BackColor = System.Drawing.Color.White;
            //btnDelete.Enabled = false; ;
        }

        private void ClearFields()
        {
            try
            {
                Presenter.SetFieldsBlanks();                
                //ddlHeadKitFamily.SelectedValue = "0";
                txtHeadKitFamily.Text = "";
                hdnHeadKitFamilyId.Value = "0";
                txtRentalFee.Text = "0";
                _presenter.PopulateKitListingList();

            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        private bool ValidateItemsOnAdd(string txtNewCatalogNum, string txtNewCatalogDesc, string txtNewQuantity)
        {
            if (string.IsNullOrEmpty(txtNewCatalogNum))
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valCatalogNumberEmpty"), this.lblHeader.Text);
                return false;
            }
            var item = this.KitTableList.Find(t => t.Catalognumber == txtNewCatalogNum);
            if (item != null)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valCatalogNumberExists"), this.lblHeader.Text);
                return false;
            }
            if (string.IsNullOrEmpty(txtNewCatalogDesc))
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valCatalogDescEmpty"), this.lblHeader.Text);
                return false;
            }
            if (string.IsNullOrEmpty(txtNewQuantity))
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valQuantity"), this.lblHeader.Text);
                return false;
            }
            if (Convert.ToInt32(txtNewQuantity) < 1)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valQuantityValue"), this.lblHeader.Text);
                return false;
            }
            return true;
        }

        private bool ValidateItems(string txtCatalogDesc, string txtQuantity)
        {
            //if ((string.IsNullOrEmpty(txtCatalogNum.Text)) || IsCatalogNumberDuplicate(txtCatalogNum.Text, gridKitTable.EditIndex))
            //{
            //    lblError.Text = "Catalog Number is Empty or already exists";
            //    return false;
            //}
            if (string.IsNullOrEmpty(txtCatalogDesc))
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valCatalogDescEmpty"), this.lblHeader.Text);
                return false;
            }
            if (string.IsNullOrEmpty(txtQuantity))
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valQuantity"), this.lblHeader.Text);
                return false;
            }
            if (Convert.ToInt32(txtQuantity) < 1)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valQuantityValue"), this.lblHeader.Text);
                return false;
            }
            return true;
        }

        private bool IsCatalogNumberDuplicate(string catalogNumber, int index)
        {
            var item = this.KitTableList.Find(t => t.Catalognumber == catalogNumber);
            if (item != null && KitTableList.IndexOf(item) != index)
            {
                return true;
            }
            return false;
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrEmpty(txtKitNumber.Text))
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valKitListingKitNumber"), this.lblHeader.Text);
                return false;
            }
            if (IsKitNumberDuplicate())
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valDuplicateKitNumber"), this.lblHeader.Text);
                return false;
            }
            if (string.IsNullOrEmpty(txtKitName.Text))
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valKitName"), this.lblHeader.Text);
                return false;
            }
            //if (ddlKitFamily.SelectedIndex <= 0)
            if (string.IsNullOrEmpty(txtKitFamily.Text.Trim()) || (hdnKitFamilyId.Value=="0"))
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valKitFamily"), this.lblHeader.Text);
                return false;
            }
            //if (ddlSalesOfficeLocation.SelectedIndex <= 0)
            //{
            //    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valKitListingLocation"), this.lblHeader.Text);
            //    return false;
            //}
            if (string.IsNullOrEmpty(txtRentalFee.Text.Trim()))
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valKitFamilyCurrency"), this.lblHeader.Text);
                return false;
            }
            if (chkIsActive.Checked == false)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valIsActive"), this.lblHeader.Text);
                return false;
            }
            if (KitTableList.Find(t => string.IsNullOrEmpty(t.Catalognumber)) != null)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valLeastKitItem"), this.lblHeader.Text);
                return false;
            }

            int count = 0;
            foreach (GridViewRow row in gridKitTable.Rows)
            {
                CheckBox chkStatus = (row.FindControl("chkStatus")) as CheckBox;
                if (chkStatus.Checked) count += 1;
            }
            if (count == 0)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valLeastKitItem"), this.lblHeader.Text);
                return false;
            }
            return true;
        }

        private bool IsKitNumberDuplicate()
        {
            // Edit Mode
            if (lstExistingKit.SelectedIndex >= 0)
            {
                return false;
            }
            // logic for finding unique kit number.
            return Presenter.IsKitNumberDuplicate(txtKitNumber.Text);
        }

        private string CreateKitTableXml()
        {
            //// Edit Mode no xml is being sent. Items are Added/edited indiviually.
            //if (string.IsNullOrEmpty(lstExistingKit.SelectedValue))
            //{
            //    return null;
            //}
            StringBuilder kitTableXml = new StringBuilder();
            kitTableXml.Append("<root>");
            int count = 0;
         
            string CatelogNumber, Description;
            foreach (GridViewRow row in gridKitTable.Rows)
            {
                CheckBox chkStatus = row.FindControl("chkStatus") as CheckBox;
                if (chkStatus.Checked)
                {
                    CatelogNumber = (row.FindControl("lblCatalogNum") as Label).Text;
                    Description = (row.FindControl("lblCatalogDesc") as Label).Text;

                    kitTableXml.Append("<KitTable>");
                    kitTableXml.Append("<ItemNumber>"); kitTableXml.Append(++count); kitTableXml.Append("</ItemNumber>");
                    kitTableXml.Append("<CatalogNumber>"); kitTableXml.Append(CatelogNumber); kitTableXml.Append("</CatalogNumber>");
                    kitTableXml.Append("<Description>"); kitTableXml.Append(Description); kitTableXml.Append("</Description>");
                    kitTableXml.Append("<Qty>"); kitTableXml.Append(1); kitTableXml.Append("</Qty>");
                    kitTableXml.Append("</KitTable>");
                }
            }

            kitTableXml.Append("</root>");
            return kitTableXml.ToString();
        }

        private void ClearNotifications()
        {
            lblError.Text = string.Empty;
        }
        #endregion

        
            

    }
}

