using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VCTWebApp.Shell.Views;
using VCTWebApp.Resources;
using VCTWeb.Core.Domain;
using VCTWebApp.Web;
using System.Data.SqlClient;
using System.Globalization;
using Microsoft.Practices.ObjectBuilder;
using System.Data;
using System.Text;

namespace VCTWebApp
{
    public partial class KitFamily : Microsoft.Practices.CompositeWeb.Web.UI.Page, IKitFamilyView
    {
       
        #region Instance Variables

        private KitFamilyPresenter presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private Helper helper = new Helper();
        private Security security = null;

        #endregion

        #region Create New Presenter
        [CreateNew]
        public KitFamilyPresenter Presenter
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

        #region Init/Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                  
                    this.AuthorizedPage();
                    this.presenter.OnViewInitialized();
                    this.LocalizePage();
                    //this.ShowBadgeNumberControls();
                    this.Form.DefaultButton = this.btnSave.UniqueID; //Set the default button to save.

                    this.DisplayMessageForMissingMasters();

                    this.txtName.Focus();
                }
            }
            catch (SqlException ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion                        

        #region IKitFamilyView Members
        public List<KitType> KitTypeList
        {
            set
            {
                this.ddlKitType.DataSource = value;
                this.ddlKitType.DataTextField = "KitTypeName";
                this.ddlKitType.DataValueField = "KitTypeName";
                this.ddlKitType.DataBind();

                this.ddlKitType.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect"), "0"));
            }
        }

        public bool Active
        {
            get
            {
                return this.chkActive.Checked;
            }
            set
            {
                this.chkActive.Checked = value;
            }
        }

        public Int16 NumberOfTubs
        {
            get
            {
                return Convert.ToInt16(this.txtNumberOfTubs.Text);
            }
            set
            {
                this.txtNumberOfTubs.Text = value.ToString();
            }
        }

        public long SelectedKitFamilyId
        {
            get
            {
                if (this.lstExistingKitFamilies.SelectedIndex >= 0)
                    return Convert.ToInt64(this.lstExistingKitFamilies.SelectedValue);
                else
                    return 0;                
            }
        }
        
        public List<VCTWeb.Core.Domain.KitFamily> KitFamilyList
        {         
            set
            {         
                this.lstExistingKitFamilies.DataSource = value;
                this.lstExistingKitFamilies.DataTextField = "KitFamilyName";
                this.lstExistingKitFamilies.DataValueField = "KitFamilyId";
                this.lstExistingKitFamilies.DataBind();
            }
        }

        public string Name
        {
            get
            {
                return this.txtName.Text.Trim();
            }
            set
            {
                this.txtName.Text = value;
            }
        }

        public string Description
        {
            get
            {
                return this.txtDescription.Text.Trim();
            }
            set
            {
                this.txtDescription.Text = value;
            }
        }

        public string KitType
        {
            get
            {
                return this.ddlKitType.SelectedValue;
            }
            set
            {
                this.ddlKitType.SelectedValue = value;
            }
        }

        public List<VCTWeb.Core.Domain.KitFamilyParts> KitFamilyPartsList
        {
            get
            {
              
                return (List<KitFamilyParts>)ViewState["KitFamilyList"];
            }
            set
            {

                ViewState["KitFamilyList"] = value;

                this.gdvPartDetails.DataSource = value;
                this.gdvPartDetails.DataBind();

                if (value.Count > 0 && String.IsNullOrEmpty(value[0].CatalogNumber))
                {
                    this.gdvPartDetails.Rows[0].Visible = false;
                }
            }
        }

        public List<VCTWeb.Core.Domain.KitFamilyLocations> KitFamilyLocationList
        {
            get
            {
                return (List<VCTWeb.Core.Domain.KitFamilyLocations>)ViewState["KitFamilyLocationList"];
            }
            set
            {
                ViewState["KitFamilyLocationList"] = value;

                this.gvKitFamilyLocation.DataSource = value;
                this.gvKitFamilyLocation.DataBind();
                if (value.Count > 0 && String.IsNullOrEmpty(Convert.ToString(value[0].KitFamilyLocationId)))
                {
                    this.gvKitFamilyLocation.Rows[0].Visible = false;
                }
            }
        }

        public string KitFamilyPartTableXml
        {
            get
            {
                return CreateKitFamilyPartTableXml();
            }
        }

        public string KitFamilyLocationTableXml
        {
            get
            {
                return CreateKitFamilyLocationTableXml();
            }
        }
        #endregion

        #region Event Handlers
        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                this.lblError.Text = string.Empty;
                presenter.OnViewInitialized();
                this.txtName.Focus();
            }
            catch (SqlException ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            
            try
            {
                this.lblError.Text = string.Empty;
                if (Page.IsValid && IsValidInput())
                {

                    if (!ValidateQuantity())
                    {
                        lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valQuantity"), this.lblHeader.Text);
                    }
                    else if (! hasValidData())
                    {
                        lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valLeastKitItem"), this.lblHeader.Text);
                    }
                    else
                    {
                        Constants.ResultStatus resultStatus = presenter.Save();
                        if (resultStatus == Constants.ResultStatus.Created)
                        {
                            presenter.OnViewInitialized();
                            this.txtName.Focus();

                            this.lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgCreated"), this.lblHeader.Text) + "</font>";
                        }
                        else if (resultStatus == Constants.ResultStatus.Updated)
                        {
                            presenter.OnViewInitialized();
                            this.txtName.Focus();

                            this.lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgUpdated"), this.lblHeader.Text) + "</font>";
                        }
                        else if (resultStatus == Constants.ResultStatus.DuplicateKitFamily)
                        {
                            this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgKitFamily"), this.lblHeader.Text);
                        }
                        else if (resultStatus == Constants.ResultStatus.InUse)
                        {
                            this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgInUseKitFamily"), this.lblHeader.Text);
                        }
                        else
                        {
                            //ToDo - Handle the error part
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                if (string.Compare(Common.MSG_VAL_EXISTS, ex.Message.Trim(), true, CultureInfo.InvariantCulture) == 0 || string.Compare(Common.MSG_BADGE_EXISTS, ex.Message.Trim(), true, CultureInfo.InvariantCulture) == 0)
                {
                    this.lblError.Text = string.Format(CultureInfo.InvariantCulture, this.vctResource.GetString(ex.Message), this.vctResource.GetString("mnuUser"));
                }
                else
                {
                    throw ex;
                }
            }
        }
        #endregion

        #region Protected Methods

        protected void gdvPartDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gdvPartDetails.EditIndex = e.NewEditIndex;
                this.KitFamilyPartsList = this.KitFamilyPartsList;
             

            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        protected void gdvPartDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                gdvPartDetails.EditIndex = -1;
                this.KitFamilyPartsList = this.KitFamilyPartsList;
               

            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        protected void gdvPartDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("AddNewRow"))
                {
                    if (hdnPartNumNew.Value == string.Empty)
                    {
                        lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valSelectPartNumberEmpty"), this.lblHeader.Text);
                    }
                    else
                    {
                        TextBox txtNewPartQty = gdvPartDetails.HeaderRow.FindControl("txtNewPartQty") as TextBox;
                        TextBox txtNewDescription = gdvPartDetails.HeaderRow.FindControl("txtNewDescription") as TextBox;
                        TextBox txtNewPartNum = gdvPartDetails.HeaderRow.FindControl("txtNewPartNum") as TextBox;
                        if (txtNewPartQty != null)
                        {
                            //if (ValidateItemsOnAdd(hdnPartNumNew.Value, txtNewPartQty.Text))
                            if (ValidateItemsOnAdd(txtNewPartNum.Text, txtNewPartQty.Text))
                            {

                                //if (this.KitFamilyPartsList.Any(t => t.CatalogNumber == hdnPartNumNew.Value))
                                if (this.KitFamilyPartsList.Any(t => t.CatalogNumber == txtNewPartNum.Text))
                                {
                                    this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valPartNumberExists"), this.lblHeader.Text);
                                    //foreach (var item in this.KitFamilyPartsList)
                                    //{
                                    //    if (item.CatalogNumber == hdnPartNumNew.Value)
                                    //    {
                                    //        item.Quantity += 1;
                                    //    }
                                    //}
                                }
                                else
                                {
                                    this.KitFamilyPartsList.Add(new KitFamilyParts()
                                    {
                                        //CatalogNumber = hdnPartNumNew.Value,
                                        CatalogNumber = txtNewPartNum.Text,
                                        Description = txtNewDescription.Text,
                                        Quantity = int.Parse(txtNewPartQty.Text)
                                    });
                                    
                                    lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgKitFamilyPartAdded"), this.lblHeader.Text) + "</font>";
                                }

                                RemoveEmptyRow(this.KitFamilyPartsList);
                                this.KitFamilyPartsList = this.KitFamilyPartsList;
                            }
                        }
                    }
                }
                else if (e.CommandName.Equals("UpdateRec"))
                {
                  
                    HiddenField hdnKitFamilyItemId = gdvPartDetails.Rows[gdvPartDetails.EditIndex].FindControl("hdnKitFamilyItemId") as HiddenField;

                    TextBox txtPartQty = gdvPartDetails.Rows[gdvPartDetails.EditIndex].FindControl("txtPartQty") as TextBox;

                    if (ValidateItemsOnEdit(txtPartQty.Text))
                    {
                        this.KitFamilyPartsList[gdvPartDetails.EditIndex].Quantity = int.Parse(txtPartQty.Text);
                        gdvPartDetails.EditIndex = -1;
                        this.KitFamilyPartsList = this.KitFamilyPartsList;

                        lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgKitFamilyPartUpdate"), this.lblHeader.Text) + "</font>";
                    }

                }
                else if (e.CommandName.Equals("DeleteRec"))
                {
                    long KitFamilyPartId = Convert.ToInt64(e.CommandArgument);
                    if (KitFamilyPartId > -1)
                    {

                        this.KitFamilyPartsList.RemoveAt(Convert.ToInt32(e.CommandArgument));
                        if (this.KitFamilyPartsList.Count == 0)
                        {
                            this.KitFamilyPartsList.Add(new KitFamilyParts());
                            this.KitFamilyPartsList = this.KitFamilyPartsList;
                        }
                        else
                        {
                            this.KitFamilyPartsList = this.KitFamilyPartsList;
                        }

                        lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgKitFamilyPartDelete"), this.lblHeader.Text) + "</font>";

                    }
                }
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        private void RemoveEmptyRow(List<KitFamilyParts> value)
        {
            if (value != null)
            {
                value.RemoveAll(t => t.CatalogNumber == null);                
            }
        }

        protected void lstExistingKitFamilies_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.lblError.Text = string.Empty;
                this.gdvPartDetails.EditIndex = -1;
                presenter.OnViewLoaded();
                this.txtName.Focus();
                
            }
            catch (SqlException ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Private Methods
        private bool IsValidInput()
        {
            if (ddlKitType.SelectedIndex <= 0)
            {
                lblError.Text = vctResource.GetString("valKitType");
                ddlKitType.Focus();
                return false;
            }
            else if (KitFamilyPartsList.Find(t => string.IsNullOrEmpty(t.CatalogNumber)) != null)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valLeastKitItem"), this.lblHeader.Text);
                return false;
            }
            return true;
        }

        private void AuthorizedPage()
        {
            security = new Security();
            if (Session["LoggedInUser"] == null)
            {
                Response.Redirect("~/ErrorPage.aspx?ErrorKey=Common_msgSessionExpired");
            }
            else if (security.HasAccess("KitFamily"))
            {
                CanView = security.HasPermission("KitFamily.Manage");
            }
            else
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }

        private void LocalizePage()
        {
            try
            {
                string heading = string.Empty;
                heading = vctResource.GetString("mnuKitFamily");
                lblHeader.Text = heading;
                Page.Title = heading;

                this.lblExistingKitFamilies.Text = vctResource.GetString("labelExistingKitFamilies");
                this.lblName.Text = vctResource.GetString("labelName");
                this.lblDescription.Text = vctResource.GetString("labelDescription");
                this.lblKitType.Text = vctResource.GetString("labelKitType");
                this.lblNumberOfTubs.Text = vctResource.GetString("labelNumberOfTubs");
                this.chkActive.Text = vctResource.GetString("labelActive");
                String errorMessage = vctResource.GetString("required");

                this.rfv_Name.ErrorMessage = errorMessage;
                this.rfv_KitType.ErrorMessage = errorMessage;
                this.rfv_NumberOfTube.ErrorMessage = errorMessage;
                //this.btnNew.Text = vctResource.GetString("btnReset");
                //this.btnSave.Text = vctResource.GetString("btnSave");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DisplayMessageForMissingMasters()
        {

        }

        private bool ValidateItemsOnEdit(string newQuantity)
        {
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
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valSelectPartNumberEmpty"), this.lblHeader.Text);
               
                return false;
            }
            else if (string.IsNullOrEmpty(newQuantity))
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valQuantity"), this.lblHeader.Text);
                return false;
            }
            else if (Convert.ToInt32(newQuantity) < 1)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valQuantityValue"), this.lblHeader.Text);
                return false;
            }
            //else
            //{
            //    var item = this.LocationPARLevelList.Find(t => t.PartNum == newParNum);
            //    if (item != null)
            //    {
            //        lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valPartNumberExists"), this.lblHeader.Text);
            //        return false;
            //    }
            //}      



            return true;
        }

        private bool ValidateQuantity()
        {
            bool flag = false;
            foreach (GridViewRow row in gdvPartDetails.Rows)
            {
                TextBox txtBox =(row.FindControl("txtPartQty") as TextBox);
                if (txtBox != null)
                {
                    if (string.IsNullOrEmpty(txtBox.Text))
                    {
                        flag = false;
                        break;
                    }
                    else if (!string.IsNullOrEmpty(txtBox.Text))
                    {
                        if (Convert.ToInt32(txtBox.Text) == 0)
                        {
                            flag = false;
                            break;
                        }
                    }
                    else
                    {
                        flag = true;
                    }
                }
                else
                {
                    flag = true;
                }
                   
            }
            return flag;
            
        }

        private bool hasValidData()
        {
            bool flag = false;
            //int LocCount = 0, PartCount = 0;
            int PartCount = 0;
            

            //foreach (GridViewRow row in gvKitFamilyLocation.Rows)
            //{
            //    CheckBox chkLocationStatus = row.FindControl("chkStatus") as CheckBox;
            //    if (chkLocationStatus.Checked) LocCount += 1;
            //}

            foreach (GridViewRow row in gdvPartDetails.Rows)
            {
                string PartNum = (row.FindControl("lblPartNum") as Label).Text;
                if (!(string.IsNullOrEmpty(PartNum)))
                {
                    PartCount += 1;
                }
               
                
            }

            //if (LocCount != 0 && PartCount != 0) flag = true;
            if (PartCount != 0) flag = true;

            return flag;
        }

        private string CreateKitFamilyPartTableXml()
        {
            // Edit Mode no xml is being sent. Items are Added/edited indiviually.          
            StringBuilder kitTableXml = new StringBuilder();
            kitTableXml.Append("<root>");
            int count = 0;
            foreach (var item in this.KitFamilyPartsList)
            {
                kitTableXml.Append("<KitFamilyItem>");
                kitTableXml.Append("<KitFamilyId>"); kitTableXml.Append(++count); kitTableXml.Append("</KitFamilyId>");
                kitTableXml.Append("<CatalogNumber>"); kitTableXml.Append(item.CatalogNumber); kitTableXml.Append("</CatalogNumber>");
                kitTableXml.Append("<Quantity>"); kitTableXml.Append(item.Quantity); kitTableXml.Append("</Quantity>");
                kitTableXml.Append("</KitFamilyItem>");
            }
            kitTableXml.Append("</root>");
            return kitTableXml.ToString();
        }

        private string CreateKitFamilyLocationTableXml()
        {
            // Edit Mode no xml is being sent. Items are Added/edited indiviually.          
            StringBuilder kitTableXml = new StringBuilder();
            kitTableXml.Append("<root>");
            int count = 0;
            //foreach (var item in this.KitFamilyLocationList)
            foreach (GridViewRow row in gvKitFamilyLocation.Rows)
            {
                CheckBox chk = row.FindControl("chkStatus") as CheckBox;
                if (chk.Checked && chk.Enabled == true)
                {
                    count += 1;
                    Int32 LocationId = Convert.ToInt32((row.FindControl("hdnLocationId") as HiddenField).Value);
                    kitTableXml.Append("<KitFamilyLocation>");
                    kitTableXml.Append("<LocationId>"); kitTableXml.Append(LocationId); kitTableXml.Append("</LocationId>");
                    kitTableXml.Append("</KitFamilyLocation>");
                }
            }
            kitTableXml.Append("</root>");
            if (count == 0)
            {
                kitTableXml.Clear();
            }

            return kitTableXml.ToString();
        }
        #endregion        

    }
}

