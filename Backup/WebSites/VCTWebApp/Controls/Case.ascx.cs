using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VCTWebApp.Shell.Views;
using VCTWebApp.Resources;
using VCTWeb.Core.Domain;
using Microsoft.Practices.ObjectBuilder;
using System.Text;
using System.Globalization;
using System.Data;

namespace VCTWebApp.Controls
{
    public partial class Case : Microsoft.Practices.CompositeWeb.Web.UI.UserControl, ICaseView
    {
        #region Instance Variables
        private CasePresenter _presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private Helper helper = new Helper();
        #endregion

        #region Create New Presenter

        [CreateNew]
        public CasePresenter Presenter
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

        #region Delegate

        public delegate void CloseClickedHandler(bool flagBindGrid);
        public delegate void ShowCaseHandler();
        public event CloseClickedHandler OnCloseClicked;
        public event ShowCaseHandler OnShowPopup;

        #endregion

        #region ICaseView Implementations

        public List<CasePartDetailGroup> PartDetailList
        {
            get
            {
                return (List<CasePartDetailGroup>)ViewState["PartDetailList"];
            }
            set
            {
                ViewState["PartDetailList"] = value;

                this.gdvPartDetail.DataSource = value;
                this.gdvPartDetail.DataBind();
            }
        }

        //public List<CasePartDetailGroup> IndicativePartDetailList
        //{
        //    set
        //    {
        //        this.gdvIndicativeParts.DataSource = value;
        //        this.gdvIndicativeParts.DataBind();
        //    }
        //}

        public List<VCTWeb.Core.Domain.Users> SalesRep
        {
            set
            {
                this.ddlSalesRep.DataSource = value;
                this.ddlSalesRep.DataTextField = "FullName";
                this.ddlSalesRep.DataValueField = "UserName";
                this.ddlSalesRep.DataBind();

                this.ddlSalesRep.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect"), string.Empty));
            }
        }

        public List<VCTWeb.Core.Domain.CaseKitFamilyDetailGroup> KitFamilyList
        {
            get
            {
                return (List<VCTWeb.Core.Domain.CaseKitFamilyDetailGroup>)ViewState["KitFamilyList"];
            }
            set
            {
                ViewState["KitFamilyList"] = value;

                this.gdvKitFamilyDetail.DataSource = value;
                this.gdvKitFamilyDetail.DataBind();
            }
        }

        public string CaseKitFamilyDetailXml
        {
            get
            {
                StringBuilder xmlString = new StringBuilder();
                if (this.InventoryType == Constants.InventoryType.Kit.ToString())
                {
                    foreach (GridViewRow gvr in gdvKitFamilyDetail.Rows)
                    {
                        if (xmlString.Length == 0)
                            xmlString.Append("<root>");
                        HiddenField hdnKitFamilyId = gvr.FindControl("hdnKitFamilyId") as HiddenField;
                        Label lblQuantity = gvr.FindControl("lblQuantity") as Label;
                        xmlString.Append("<CaseKitFamilyDetail>");
                        xmlString.Append("<KitFamilyId>" + hdnKitFamilyId.Value + "</KitFamilyId>");
                        xmlString.Append("<Quantity>" + lblQuantity.Text + "</Quantity>");
                        xmlString.Append("</CaseKitFamilyDetail>");
                    }
                    if (xmlString.Length != 0)
                        xmlString.Append("</root>");
                }
                return xmlString.ToString();
            }
        }

        public string CasePartDetailXml
        {
            get
            {
                StringBuilder xmlString = new StringBuilder();
                if (this.InventoryType == Constants.InventoryType.Part.ToString())
                {
                    foreach (GridViewRow gvr in gdvPartDetail.Rows)
                    {
                        if (xmlString.Length == 0)
                            xmlString.Append("<root>");
                        Label lblPartNum = gvr.FindControl("lblPartNum") as Label;
                        Label lblQuantity = gvr.FindControl("lblQuantity") as Label;
                        xmlString.Append("<CasePartDetail>");
                        xmlString.Append("<PartNum>" + lblPartNum.Text + "</PartNum>");
                        xmlString.Append("<Quantity>" + lblQuantity.Text + "</Quantity>");
                        xmlString.Append("</CasePartDetail>");
                    }
                    if (xmlString.Length != 0)
                        xmlString.Append("</root>");
                }
                return xmlString.ToString();
            }
        }

        public long CaseId
        {
            get
            {
                return (long)ViewState["CaseId"];
            }
            set
            {
                ViewState["CaseId"] = value;
            }
        }

        public string CaseNumber
        {
            get
            {
                return lblHeader.Text == vctResource.GetString("lblCase") ? null : lblHeader.Text.Substring(lblHeader.Text.LastIndexOf(" ")).Trim();
            }
            set
            {
                lblHeader.Text = value == null ? vctResource.GetString("lblCase") : vctResource.GetString("lblCase") + " # " + value.ToString(CultureInfo.InvariantCulture);
            }
        }

        public DateTime SurgeryDate
        {
            get
            {
                return Convert.ToDateTime(txtSurgeryDate.Text);
            }
            set
            {
                txtSurgeryDate.Text = string.Format(CultureInfo.CurrentCulture, value.ToString("d"));
                hdnSurgeryDate.Value = txtSurgeryDate.Text;
            }
        }

        public DateTime? ShippingDate
        {
            set
            {
                DateTime dtDefault = new DateTime(1, 1, 1);
                if (value != null && value != dtDefault)
                    this.txtShippingDate.Text = Convert.ToDateTime(value, CultureInfo.CurrentCulture).ToString("d");
                else
                    this.txtShippingDate.Text = string.Empty;
            }
        }

        public DateTime? RetrievalDate
        {
            set
            {
                DateTime dtDefault = new DateTime(1, 1, 1);
                if (value != null && value != dtDefault)
                    this.txtRetrievalDate.Text = Convert.ToDateTime(value, CultureInfo.CurrentCulture).ToString("d");
                else
                    this.txtRetrievalDate.Text = string.Empty;
            }
        }

        public string PatientName
        {
            get
            {
                return txtPatientName.Text;
            }
            set
            {
                txtPatientName.Text = value;
            }
        }

        public string SpecialInstructions
        {
            get
            {
                return txtSplInstructions.Text;
            }
            set
            {
                txtSplInstructions.Text = value;
            }
        }

        public string Physician
        {
            get
            {
                return txtPhysician.Text;
            }
            set
            {
                txtPhysician.Text = value;
            }
        }

        public string InventoryType
        {
            get
            {
                if (pnlIndicativeParts.Visible)
                    return Constants.InventoryType.Kit.ToString();
                else
                    return Constants.InventoryType.Part.ToString();
            }
            set
            {
                if (value == Constants.InventoryType.Kit.ToString())
                {
                    pnlDetailKit.Visible = true;
                    pnlIndicativeParts.Visible = true;
                    pnlPartDetail.Visible = false;
                    lblRetrievalDate.Visible = true;
                    txtRetrievalDate.Visible = true;
                    KitTab.Enabled = false;
                    ReplenishmentTab.Enabled = true;
                }
                else
                {
                    pnlDetailKit.Visible = false;
                    pnlIndicativeParts.Visible = false;
                    pnlPartDetail.Visible = true;
                    lblRetrievalDate.Visible = false;
                    txtRetrievalDate.Visible = false;
                    KitTab.Enabled = true;
                    ReplenishmentTab.Enabled = false;
                }
            }
        }

        public double? TotalPrice
        {
            get
            {
                if (!string.IsNullOrEmpty(txtTotalPrice.Text))
                {
                    double outTest;
                    if (double.TryParse(txtTotalPrice.Text, out outTest))
                    {
                        return outTest;
                    }
                }
                return null;
            }
            set
            {
                txtTotalPrice.Text = value.ToString();
            }
        }

        public string CaseStatus
        {
            get
            {
                return lblCaseStatusValue.Text;
            }
            set
            {
                string val = string.IsNullOrEmpty(value) ? "Not Saved" : value;
                lblCaseStatusValue.Text = val;
            }
        }

        public string SelectedSalesRep
        {
            get
            {
                if (ddlSalesRep.SelectedIndex > 0)
                {
                    return ddlSalesRep.SelectedValue;
                }
                return null;
            }
            set
            {
                if (value == null)
                    ddlSalesRep.SelectedIndex = 0;
                else
                    ddlSalesRep.SelectedValue = value.Trim().ToString();
            }
        }

        public string SelectedProcedureName
        {
            get
            {
                if (string.IsNullOrEmpty(hdnProcedureNameCase.Value))
                    return null;
                else
                    return hdnProcedureNameCase.Value;
            }
            set
            {
                txtProcedureNameCase.Text = value.Trim().ToString();
                if (value == null)
                    hdnProcedureNameCase.Value = string.Empty;
                else
                    hdnProcedureNameCase.Value = value.ToString();
            }
        }

        //public Int64? SelectedKitFamilyId
        //{
        //    get
        //    {
        //        if (ddlKitFamily.SelectedIndex > 0)
        //            return Convert.ToInt64(ddlKitFamily.SelectedValue);
        //        else
        //            return null;
        //    }
        //    set
        //    {
        //        if (value == 0 || value == null)
        //            ddlKitFamily.SelectedIndex = 0;
        //        else
        //            ddlKitFamily.SelectedValue = value.ToString();
        //    }
        //}

        //public Int64? SelectedKitFamilyId
        //{
        //    get
        //    {                
        //        if (!((string.IsNullOrEmpty(hdnChildKitFamilyId.Value)) || (hdnChildKitFamilyId.Value == "0")))
        //            return Convert.ToInt64(hdnChildKitFamilyId.Value);
        //        else
        //            return null;
        //    }
        //    set
        //    {
        //        if (value == 0 || value == null)
        //            hdnChildKitFamilyId.Value = "0";                
        //        else
        //            hdnChildKitFamilyId.Value = value.ToString();
        //    }
        //}

        //public string KitFamilyName
        //{            
        //    set
        //    {
        //        txtChildKitFamily.Text = value;
        //    }
        //}

        public Int64? SelectedParty
        {
            get
            {
                if (string.IsNullOrEmpty(hdnShipToPartyIdCase.Value))
                    return null;
                else
                    return Convert.ToInt64(hdnShipToPartyIdCase.Value);
            }
            set
            {
                if (value == null)
                    hdnShipToPartyIdCase.Value = string.Empty;
                else
                    hdnShipToPartyIdCase.Value = value.ToString();
            }
        }


        public string SelectedPartyName
        {
            get
            {
                return txtHospitalCase.Text;
            }
            set
            {
                txtHospitalCase.Text = value;
            }
        }


        //public int Quantity
        //{
        //    get { return Convert.ToInt32(txtQuantity.Text); }
        //    set { txtQuantity.Text = value.ToString(); }
        //}


        #endregion

        #region private methods

        private void ShowHideTabs()
        {
            if (CaseId > 0)
            {
                KitTab.Visible = false;
                ReplenishmentTab.Visible = false;
            }
            else
            {
                KitTab.Visible = true;
                ReplenishmentTab.Visible = true;
            }
        }

        private void EnableDisableButtons()
        {
            if (this.CaseStatus == "Not Saved" || this.CaseStatus == string.Empty)
            {
                btnCaseReset.Visible = true;
            }
            else
            {
                btnCaseReset.Visible = false;
            }

            if (this.CaseStatus == Constants.CaseStatus.New.ToString() || this.CaseStatus == "Not Saved" || this.CaseStatus == string.Empty)
            {
                btnSave.Visible = true;
                gdvKitFamilyDetail.Enabled = true;
                pnlDetail.Enabled = true;
                pnlDetailKit.Enabled = true;
                pnlGrid1.Enabled = true;
                imgCalenderFrom.Visible = true;
                pnlGrid2.Enabled = true;

            }
            else
            {
                btnSave.Visible = false;
                gdvKitFamilyDetail.Enabled = false;
                pnlDetail.Enabled = false;
                pnlDetailKit.Enabled = false;
                pnlGrid1.Enabled = false;
                imgCalenderFrom.Visible = false;
                pnlGrid2.Enabled = false;
            }


        }

        private void LocalizePage()
        {
            try
            {
                this.KitTab.Text = vctResource.GetString("lblKit");
                this.ReplenishmentTab.Text = vctResource.GetString("lblReplenishment");
                this.lblSalesRep.Text = vctResource.GetString("lblSalesRep");
                this.lblSurgeryDate.Text = vctResource.GetString("lblSurgeryDate");
                this.lblShippingDate.Text = vctResource.GetString("lblShippingDate");
                this.lblRetrievalDate.Text = vctResource.GetString("lblRetrievalDate");
                this.lblParty.Text = vctResource.GetString("lblParty");
                this.lblPhysician.Text = vctResource.GetString("lblPhysician");
                this.lblCaseStatus.Text = vctResource.GetString("lblCaseStatus");
                this.lblSplInstruction.Text = vctResource.GetString("lblSplInstruction");
                this.lblProcedureName.Text = vctResource.GetString("lblProcedureNameCase");
                //this.lblKitFamily.Text = vctResource.GetString("lblKitFamilyCase");
                //this.lblQuantity.Text = vctResource.GetString("lblQuantity");
                this.lblTotalPrice.Text = vctResource.GetString("lblTotalPrice");
                this.lblPatientName.Text = vctResource.GetString("lblPatientName");
                //this.chkFlexibleDates.Text = vctResource.GetString("chkFlexibleDates");
                //this.lblIndicativeParts.Text = vctResource.GetString("lblIndicativeParts");
                this.lblPartsDetails.Text = vctResource.GetString("lblPartsDetails");
                this.lblPatientName.Text = vctResource.GetString("lblPatientName");
                //this.lblLocation.Text = vctResource.GetString("lblLocation");
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("Error_msgUnknownError"), this.lblHeader.Text);
            }
        }

        private void ClearNotifications()
        {
            lblError.Text = string.Empty;
        }

        private bool ValidateCase()
        {
            if (ddlSalesRep.SelectedIndex <= 0)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valSelectSalesRep"));
                return false;
            }
            if (string.IsNullOrEmpty(hdnShipToPartyIdCase.Value))
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valSeclectHospital"));
                return false;
            }
            if (string.IsNullOrEmpty(txtSurgeryDate.Text))
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valSurgeryDate"));
                return false;
            }
            //DateTime result;
            //if (!DateTime.TryParse(txtSurgeryDate.Text, out result))
            //{
            //    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valSurgeryDateInvalid"));
            //    return false;
            //}
            //if (ddlProcedureName.SelectedIndex <= 0)


            if (this.InventoryType == Constants.InventoryType.Kit.ToString())
            {
                if (string.IsNullOrEmpty(hdnProcedureNameCase.Value))
                {
                    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valSelectProcedure"));
                    return false;
                }
                //if(string.IsNullOrEmpty(txtChildKitFamily.Text) || double.Parse(hdnChildKitFamilyId.Value) <= 0)
                ////if (ddlKitFamily.SelectedIndex <= 0)
                //{
                //    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valSelectKitFamily"));
                //    return false;
                //}
                //int qty = 0;
                //if (!int.TryParse(txtQuantity.Text, out qty) || qty == 0)
                //{
                //    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valQuantity"));
                //    return false;
                //}
                if (string.IsNullOrEmpty(this.CaseKitFamilyDetailXml))
                {
                    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valKitFamilySelect"));
                    return false;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(this.CasePartDetailXml))
                {
                    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valItemSelect"));
                    return false;
                }
            }

            return true;
        }


        private void ClearFields()
        {
            ddlSalesRep.SelectedIndex = 0;
            txtShippingDate.Text = string.Empty;
            txtRetrievalDate.Text = string.Empty;
            txtSurgeryDate.Text = hdnSurgeryDate.Value;
            txtHospitalCase.Text = string.Empty;
            hdnShipToPartyIdCase.Value = string.Empty;
            txtPhysician.Text = string.Empty;
            hdnPhysicianId.Value = string.Empty;
            txtSplInstructions.Text = string.Empty;
            txtProcedureNameCase.Text = string.Empty;
            hdnProcedureNameCase.Value = string.Empty;
            txtPatientName.Text = string.Empty;
            txtTotalPrice.Text = string.Empty;

        }
        #endregion

        #region Event Handlers

      

        protected void btnCaseReset_Click(object sender, EventArgs e)
        {


            ClearFields();
            if (this.InventoryType == Constants.InventoryType.Kit.ToString())
            {
              
                this.KitFamilyList.Clear();
                this.KitFamilyList = this.KitFamilyList;
               
            }
            else if (this.InventoryType == Constants.InventoryType.Part.ToString())
            {
                this.PartDetailList.Clear();
                this.PartDetailList = this.PartDetailList;

            }
        }


        protected void gdvPartDetail_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("AddNewRow"))
                {
                    TextBox txtNewQuantity = gdvPartDetail.HeaderRow.FindControl("txtNewQuantity") as TextBox;
                    if (txtNewQuantity != null)
                    {
                        if (ValidateItemsOnAdd(hdnPartNumNew.Value, txtNewQuantity.Text))
                        {
                            this.PartDetailList.Add(new CasePartDetailGroup()
                            {
                                PartNum = hdnPartNumNew.Value,
                                Description = hdnDescriptionNew.Value,
                                Quantity = Convert.ToInt32(txtNewQuantity.Text),
                                Selected = true
                            });
                            this.PartDetailList = this.PartDetailList;
                        }
                    }
                }
                else if (e.CommandName.Equals("DeleteRec"))
                {
                    string partNum = e.CommandArgument.ToString();
                    this.PartDetailList.RemoveAll(c => c.PartNum == partNum);
                    this.PartDetailList = this.PartDetailList;
                }
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        protected void gdvKitFamilyDetail_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("AddNewRow"))
                {
                    TextBox txtNewQuantity = gdvKitFamilyDetail.HeaderRow.FindControl("txtNewQuantity") as TextBox;
                    if (txtNewQuantity != null)
                    {
                        if (ValidateKitFamiilyOnAdd(hdnKitFamilyIdNew.Value, txtNewQuantity.Text))
                        {
                            this.KitFamilyList.Add(new VCTWeb.Core.Domain.CaseKitFamilyDetailGroup()
                            {
                                KitFamilyId = Convert.ToInt64(hdnKitFamilyIdNew.Value),
                                KitFamilyName = hdnKitFamilyNameNew.Value,
                                Quantity = Convert.ToInt32(txtNewQuantity.Text),
                                Selected = true
                            });
                            hdnKitFamilyIdNew.Value = string.Empty;
                            this.KitFamilyList = this.KitFamilyList;
                        }
                    }
                }
                else if (e.CommandName.Equals("DeleteRec"))
                {
                    long kitFamilyId = Convert.ToInt64(e.CommandArgument);
                    this.KitFamilyList.RemoveAll(c => c.KitFamilyId == kitFamilyId);
                    this.KitFamilyList = this.KitFamilyList;
                }
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        private void LocalizePartDetailGrid(GridViewRowEventArgs e)
        {
            try
            {
                ((Label)e.Row.FindControl("lblPartNumHeader")).Text = vctResource.GetString("lblPartNumHeader");
                ((Label)e.Row.FindControl("lblDescriptionHeader")).Text = vctResource.GetString("lblDescriptionHeader");
                ((Label)e.Row.FindControl("lblQuantityHeader")).Text = vctResource.GetString("lblQuantityHeader");
                ((Label)e.Row.FindControl("lblActionHeader")).Text = vctResource.GetString("lblActionHeader");
            }
            catch (Exception)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("Error_msgUnknownError"), this.lblHeader.Text);
            }
        }

        private void LocalizeKitFamilyDetailGrid(GridViewRowEventArgs e)
        {
            try
            {
                ((Label)e.Row.FindControl("lblKitFamilyHeader")).Text = vctResource.GetString("lblKitFamilyHeader");
                ((Label)e.Row.FindControl("lblQuantityHeader")).Text = vctResource.GetString("lblQuantityHeader");
                ((Label)e.Row.FindControl("lblActionHeader")).Text = vctResource.GetString("lblActionHeader");
            }
            catch (Exception)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("Error_msgUnknownError"), this.lblHeader.Text);
            }
        }

        //private void LocalizeIndicativePartsGrid(GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        ((Label)e.Row.FindControl("lblPartNumHeader")).Text = vctResource.GetString("lblPartNumHeader");
        //        ((Label)e.Row.FindControl("lblDescriptionHeader")).Text = vctResource.GetString("lblDescriptionHeader");
        //        ((Label)e.Row.FindControl("lblSelectHeader")).Text = vctResource.GetString("lblSelectHeader");
        //    }
        //    catch (Exception)
        //    {
        //        this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("Error_msgUnknownError"), this.lblHeader.Text);
        //    }
        //}

        private bool ValidateKitFamiilyOnAdd(string newKitFamilyId, string newQuantity)
        {
            if (string.IsNullOrEmpty(newKitFamilyId))
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valKitFamilyEmpty"), this.lblHeader.Text);
                return false;
            }
            if (Convert.ToInt64(newKitFamilyId) < 1)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valKitFamilyEmpty"), this.lblHeader.Text);
                return false;
            }
            var item = this.KitFamilyList.Find(c => c.KitFamilyId == Convert.ToInt64(newKitFamilyId));
            if (item != null)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valKitFamilyExists"), this.lblHeader.Text);
                return false;
            }
            if (string.IsNullOrEmpty(newQuantity))
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valQuantity"), this.lblHeader.Text);
                return false;
            }
            if (Convert.ToInt32(newQuantity) < 1)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valQuantityValue"), this.lblHeader.Text);
                return false;
            }
            return true;
        }

        private bool ValidateItemsOnAdd(string newParNum, string newQuantity)
        {
            if (string.IsNullOrEmpty(newParNum))
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valPartNumberEmpty"), this.lblHeader.Text);
                return false;
            }
            var item = this.PartDetailList.Find(c => c.PartNum == newParNum);
            if (item != null)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valPartNumberExists"), this.lblHeader.Text);
                return false;
            }
            if (string.IsNullOrEmpty(newQuantity))
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valQuantity"), this.lblHeader.Text);
                return false;
            }
            if (Convert.ToInt32(newQuantity) < 1)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valQuantityValue"), this.lblHeader.Text);
                return false;
            }
            return true;
        }

        protected void btnCaseCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (Presenter.CancelCase())
                {
                    Response.Redirect(Request.RawUrl);
                }
                else
                {
                    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgCancelCase"));
                }
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("Error_msgUnknownError"), this.lblHeader.Text);
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            if (OnCloseClicked != null)
                OnCloseClicked(false);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClearNotifications();
                if (!this.IsPostBack)
                {
                    this._presenter.OnViewInitialized();
                    this.LocalizePage();

                }
                
                Presenter.OnViewLoaded();
                string strSelectedTpe = hdnSelectedType.Value;
               

                //btnCaseCancel.Attributes.Add("onclick", "javascript:return " + "confirm('" + vctResource.GetString("msgCancelCaseConfirm") + "')");
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("Error_msgUnknownError"), this.lblHeader.Text);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateCase())
                {
                    if (Presenter.Save())
                    {
                        lblError.Text = "<font color='blue'>" + vctResource.GetString("msgCaseSaved") + "</font>";
                        Session["CaseCreated"] = txtSurgeryDate.Text.ToString() + "_" + hdnSelectedType.Value;
                        Response.Redirect(Request.RawUrl, false);
                        //if (OnCloseClicked != null)
                        //    OnCloseClicked(true);
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("Error_msgUnknownError"), this.lblHeader.Text);
            }
        }

        //protected void btnAssign_Click(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(this.CaseNumber))
        //    {
        //        Session["InventoryCaseId"] = this.CaseId.ToString();
        //        Response.Redirect("~/InventoryAssignment.aspx");
        //    }
        //    else
        //    {
        //        lblError.Text = "Case Not Saved yet.";
        //    }
        //}
        #endregion

        #region public methods
        public void PopulateCase()
        {
            try
            {
                Presenter.BindCaseById();
                EnableDisableButtons();
                ShowHideTabs();
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("Error_msgUnknownError"), this.lblHeader.Text);
            }
        }
        #endregion

        //protected void gdvIndicativeParts_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Row.RowType == DataControlRowType.Header)
        //        {
        //            this.LocalizeIndicativePartsGrid(e);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
        //    }
        //}

        protected void gdvPartDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    this.LocalizePartDetailGrid(e);
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (this.CaseStatus == Constants.CaseStatus.New.ToString() || this.CaseStatus == "Not Saved" || this.CaseStatus == string.Empty)
                    {
                    }
                    else
                    {
                        LinkButton linkButton = e.Row.FindControl("lnkDelete") as LinkButton;
                        linkButton.OnClientClick = null;
                    }
                }
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        protected void gdvKitFamilyDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    this.LocalizeKitFamilyDetailGrid(e);
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (this.CaseStatus == Constants.CaseStatus.New.ToString() || this.CaseStatus == "Not Saved" || this.CaseStatus == string.Empty)
                    {
                    }
                    else
                    {
                        LinkButton linkButton = e.Row.FindControl("lnkDelete") as LinkButton;
                        linkButton.OnClientClick = null;
                    }
                }
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        protected void KitTab_Click(object sender, EventArgs e)
        {
            this.InventoryType = Constants.InventoryType.Kit.ToString();
        }

        protected void ReplenishmentTab_Click(object sender, EventArgs e)
        {
            this.InventoryType = Constants.InventoryType.Part.ToString();
        }

        //protected void ddlKitFamily_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //DataTable dt = new DataTable();
        //    //dt.Columns.Add("ItemNumber", typeof(string));
        //    //dt.Columns.Add("Description", typeof(string));

        //    //dt.Rows.Add("71343204", "OXINIUM FEM HD 12/14 32MM +4");
        //    //dt.Rows.Add("71421144", "LGN HM ST 7-8 5MM LTMD-RTLA");
        //    //dt.Rows.Add("71676511", "INTERTAN 1.5 10MMX36CM 125D LT");
        //    //dt.Rows.Add("75006708", "PLUS PROMOS HUMERAL HD R22/H17+4");
        //    //dt.Rows.Add("75017208", "POLARCUP PE-INSERT 22 SZ 45");

        //    //gdvIndicativeParts.DataSource = dt;
        //    //gdvIndicativeParts.DataBind();
        //    Presenter.PopulateKitFamilyItems();
        //}

        //protected void txtChildKitFamily_TextChanged(object sender, EventArgs e)
        //{
        //    Presenter.PopulateKitFamilyItems();
        //}


        protected void btnGetKitFamily_Click(object sender, EventArgs e)
        {
            Presenter.PopulateKitFamilyByProcedureName(txtProcedureNameCase.Text, txtPhysician.Text.Trim());
        }


    }
}