using System;
using Microsoft.Practices.ObjectBuilder;
using System.Data;
using System.Collections.Generic;
using VCTWebApp.Web;
using VCTWebApp.Resources;
using VCTWeb.Core.Domain;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI.WebControls;


namespace VCTWebApp.Shell.Views
{
    public partial class NewProductTransfer : Microsoft.Practices.CompositeWeb.Web.UI.Page, INewProductTransfer
    {
        #region Instance Variables
        private NewProductTransferPresenter presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private Helper helper = new Helper();
        private Security security = null;
        #endregion

        #region Create New Presenter
        [CreateNew]
        public NewProductTransferPresenter Presenter
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
                    txtTransDate.Focus();                    
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
        
        #region INewProductTransfer View  Members
        public List<VCTWeb.Core.Domain.KitFamily> KitFamilyList
        {            
            set
            {
                if (value != null)
                {
                   // ddlKitFamilyId.DataSource = value;
                   // ddlKitFamilyId.DataTextField = "KitFamilyName";
                   // ddlKitFamilyId.DataValueField = "KitFamilyId";
                   // ddlKitFamilyId.DataBind();

                   //this.ddlKitFamilyId.Items.Insert(0, new System.Web.UI.WebControls.ListItem(vctResource.GetString("listItemSelect"), "0"));
                                        
                }
            }
        }

        public List<VCTWeb.Core.Domain.KitFamilyParts> KitPartsList
        {
            get
            {
                return (List<VCTWeb.Core.Domain.KitFamilyParts>)ViewState["KitPartsDetail"];
            }
            set 
            {
                ViewState["KitPartsDetail"] = value;
                gvKitParts.DataSource = value;
                gvKitParts.DataBind();
                if (value.Count > 0 && String.IsNullOrEmpty(value[0].CatalogNumber))
                {
                    this.gvKitParts.Rows[0].Visible = false;
                }
            }
        }

        public List<VCTWeb.Core.Domain.NewProductTransfer> KitFamilyLocationList
        {
            //get
            //{
            //    return (List<VCTWeb.Core.Domain.AugmentTransfer>)ViewState["LocationList"];
            //}
            set
            {
                gvLocations.DataSource = value;
                gvLocations.DataBind();
                if (value.Count > 0 && String.IsNullOrEmpty(value[0].LocationName))
                {
                    this.gvLocations.Rows[0].Visible = false;
                }
            }
        }

        public Int64 KitFamilyId
        {
            get 
            {
                //return (Convert.ToInt64(ddlKitFamilyId.SelectedValue) );
                return (Convert.ToInt64(hdnKitFamilyId.Value));
            }
        }

        public String KitFamilyName
        {
            get
            {                
                return (txtKitFamily.Text.Trim());
            }
        }
              
        public string KitFamilyLocationsTableXML
        {
            get 
            {
                return (CreateKitFamilyLocationsTableXML());
            }
        }

        public string KitFamilyPartsTableXML
        {
            get
            {
                return (CreateKitFamilyPartsTableXML());
            }
        }

        public Int32 LocationId
        {
            get
            {
                return (Convert.ToInt32(Session["LoggedInLocationId"]));
            }
        }

        public DateTime TransDate
        {
            get
            {
                return (Convert.ToDateTime(txtTransDate.Text));
            }
            set
            {
                txtTransDate.Text = Convert.ToDateTime(value).ToShortDateString();
            }
        }
        #endregion                         
             
        #region Event Handlers
        //protected void btnSearch_Click(object sender, EventArgs e)
        //{
        //    presenter.OnViewLoaded();
        //}

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string result;
            if (ValidatePage("Save"))
            {
                if (presenter.SaveNewProductTransfer(out result))
                {                    
                    this.lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgCommonforSaved"), result, this.lblHeader.Text) + "</font>";
                    ClearFields();
                }
                else
                {
                    lblError.Text = vctResource.GetString("msgUnableToSaveRecord");
                }
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            ClearFields();
            lblError.Text = string.Empty;
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (ValidatePage("KitFamily"))
            {
                lblError.Text = string.Empty;
                if (!string.IsNullOrEmpty(txtKitFamily.Text) && (this.KitFamilyId == 0))
                {
                    lblError.Text = "<font color ='red'>" + "Please select valid kit family" + "</font>";
                }
                else if (!string.IsNullOrEmpty(txtKitFamily.Text))
                {
                    lblError.Text = string.Empty;
                    presenter.OnViewLoaded();
                }
                //else
                //{
                //     lblError.Text = string.Empty;
                //}
            }
        }

        //protected void ddlKitFamilyId_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ValidatePage("KitFamily"))
        //    {
        //        presenter.OnViewLoaded();
        //        presenter.PopulateKitFamilyLocations();
        //    }
        //}

        protected void txtKitFamily_TextChanged(object sender, EventArgs e)
        {            
            if (ValidatePage("KitFamily"))
            {

                if( !string.IsNullOrEmpty(txtKitFamily.Text) &&  ( this.KitFamilyId == 0))
                {
                    lblError.Text = "<font color ='red'>" + "Please select valid kit family" + "</font>";
                }
                else  if( !string.IsNullOrEmpty(txtKitFamily.Text)) 
                {
                    lblError.Text = string.Empty;
                    presenter.OnViewLoaded();
                }
                //else
                //{
                //     lblError.Text = string.Empty;
                //}
            }

        }
        #endregion

        #region Protected Method
        protected void gvKitParts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {                    
                    this.LocalizeKitTableGrid("KitParts", e);
                }
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        protected void gvLocations_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    this.LocalizeKitTableGrid("KitLocations", e);
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string CaseStatus = (e.Row.FindControl("lblStatus") as Label).Text;
                    if (CaseStatus.ToUpper() == "NO")
                    {                        
                        (e.Row.FindControl("lblStatus") as Label).ForeColor = System.Drawing.Color.Red;
                        (e.Row.FindControl("chkStatus") as CheckBox).Enabled = false;
                        (e.Row.FindControl("txtQuantity") as TextBox).Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        //protected void CalendarExtender1_DayRender(object sender, DayRenderEventArgs e)
        //{ 
        //    if(e.Day.Date < DateTime.Today)
        //    { 
        //        e.Day.IsSelectable= false;
        //    }
        //}
        #endregion                

        #region Private Methods
        private void AuthorizedPage()
        {
            security = new Security();
            if (Session["LoggedInUser"] == null)
            {
                Response.Redirect("~/ErrorPage.aspx?ErrorKey=Common_msgSessionExpired");
            }
            else if (security.HasAccess("NewProductTransfer"))
            {
                CanView = security.HasPermission("NewProductTransfer");
            }
            else
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }

        private void LocalizePage()
        {
            try
            {
                string heading = string.Empty;
                heading = vctResource.GetString("valNewProductTransfer");
                lblHeader.Text = heading;
                Page.Title = heading;

                this.lblSearchHeading.Text = vctResource.GetString("lblSearchHeading");
                this.lblTransDate.Text = vctResource.GetString("lblAddKitRequiredOn");
                this.lblgvHeading.Text = vctResource.GetString("lblgvHeading");



                String errorMessage = vctResource.GetString("required");
               // this.rfvKitFamilyId.ErrorMessage = errorMessage;
                this.rfvTransDate.ErrorMessage = errorMessage;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ClearFields()
        {
            try
            {
                hdnKitFamilyId.Value = "0";
                Presenter.SetFieldsBlanks();
                //ddlKitFamilyId.SelectedValue = "0";                
                txtKitFamily.Text = string.Empty;
                txtTransDate.Text = string.Empty;
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        private string CreateKitFamilyLocationsTableXML()
        {
            Int32 qty = 0, count = 0;
            System.Text.StringBuilder kitTableXml = new System.Text.StringBuilder();

            kitTableXml.Append("<root>");

            foreach (GridViewRow row in gvLocations.Rows)
            {
                CheckBox chk = row.FindControl("chkStatus") as CheckBox;
                if (chk.Checked)
                {

                    qty = int.Parse((row.FindControl("txtQuantity") as TextBox).Text);
                    if (qty > 0)
                    {
                        int hdnLocationId = int.Parse((row.FindControl("hdnLocationId") as HiddenField).Value);
                        count += 1;
                        kitTableXml.Append("<KitFamilyLocation>");
                        kitTableXml.Append("<LocationId>"); kitTableXml.Append(hdnLocationId); kitTableXml.Append("</LocationId>");
                        kitTableXml.Append("<Quantity>"); kitTableXml.Append(qty); kitTableXml.Append("</Quantity>");
                        kitTableXml.Append("</KitFamilyLocation>");
                    }

                }
            }

            kitTableXml.Append("</root>");
            if (count == 0)
            {
                kitTableXml.Clear();
            }

            return kitTableXml.ToString();
        }

        private string CreateKitFamilyPartsTableXML()
        {
            Int32 qty = 0, count = 0;
            System.Text.StringBuilder kitPartsTableXml = new System.Text.StringBuilder();

            kitPartsTableXml.Append("<root>");

            foreach (GridViewRow row in gvKitParts.Rows)
            {
                string CatalogNumber = (row.FindControl("lblCatalogNumber") as Label).Text;
                string Qty = (row.FindControl("lblQuantity") as Label).Text;

                count += 1;
                kitPartsTableXml.Append("<KitFamilyParts>");
                kitPartsTableXml.Append("<CatalogNumber>"); kitPartsTableXml.Append(CatalogNumber); kitPartsTableXml.Append("</CatalogNumber>");
                kitPartsTableXml.Append("<Quantity>"); kitPartsTableXml.Append(Qty); kitPartsTableXml.Append("</Quantity>");
                kitPartsTableXml.Append("</KitFamilyParts>");

            }

            kitPartsTableXml.Append("</root>");
            if (count == 0)
            {
                kitPartsTableXml.Clear();
            }

            return kitPartsTableXml.ToString();
        }

        private void LocalizeKitTableGrid(string type, GridViewRowEventArgs e)
        {
            try
            {
                if (type == "KitParts")
                {
                    (e.Row.FindControl("lblPartNum") as Label).Text = vctResource.GetString("lblPartNumHeader");
                    (e.Row.FindControl("lblDesc") as Label).Text = vctResource.GetString("lblDescriptionHeader");
                    (e.Row.FindControl("lblQty") as Label).Text = vctResource.GetString("lblQuantityHeader");
                }
                else
                {
                    (e.Row.FindControl("lblSelect") as Label).Text = vctResource.GetString("lblSelectHeader");
                    (e.Row.FindControl("lblLocationNameHeader") as Label).Text = vctResource.GetString("lblLocationsHeader");
                    (e.Row.FindControl("lblLocationTypeNameHeader") as Label).Text = vctResource.GetString("lblLocationsTypeNameHeader");
                    (e.Row.FindControl("lblQuantity") as Label).Text = vctResource.GetString("lblQuantityHeader");
                }
            }
            catch (Exception)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("Error_msgUnknownError"), this.lblHeader.Text);
            }
        }

        private bool ValidatePage(string ValidationType)
        {
            //if (ddlKitFamilyId.SelectedValue == "0")
            //{
            //    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valKitFamily"), this.lblHeader.Text);
            //    return false;
            //}            
            if (string.IsNullOrEmpty(txtKitFamily.Text.Trim()))
            {
                //lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valKitFamily"), this.lblHeader.Text);
                lblError.Text = "<font color ='red'>" + "Please enter  kit family" + "</font>";
                return false;
            }            
            
            if (ValidationType.ToUpper() == "KITFAMILY")
            {
                lblError.Text = "";
                return true;    
            }
            else if (ValidationType.ToUpper() == "SAVE")
            {
                if (string.IsNullOrEmpty(txtTransDate.Text.Trim()))
                {
                    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valRequiredOnDate"), this.lblHeader.Text);
                    return false;
                }
                else if (gvKitParts.Rows.Count == 0)
                {
                    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valKitMustHavePart"), this.lblHeader.Text);
                    return false;
                }
                else if (gvLocations.Rows.Count == 0)
                {
                    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valMustHaveLocation"), this.lblHeader.Text);
                    return false;
                }

                foreach (GridViewRow row in gvKitParts.Rows)
                {
                    string CatalogNumber = (row.FindControl("lblCatalogNumber") as Label).Text;
                    if (string.IsNullOrEmpty(CatalogNumber))
                    {
                        lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valKitMustHavePart"), this.lblHeader.Text);
                        return false;
                    }
                }



                int count = 0;
                foreach (GridViewRow row in gvLocations.Rows)
                {
                    CheckBox chkStatus = row.FindControl("chkStatus") as CheckBox;
                    TextBox txtQuantity = row.FindControl("txtQuantity") as TextBox;

                    int Qty = 0;
                    if (!string.IsNullOrEmpty(txtQuantity.Text))
                    {
                         Qty = int.Parse(txtQuantity.Text);
                    }

                    if (chkStatus.Checked)
                    {
                        count += 1;
                    }

                    if (chkStatus.Checked && Qty == 0)
                    {
                        lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valQtyForSelect"), this.lblHeader.Text);
                        return false;
                    }
                    else if (chkStatus.Checked == false && Qty > 0)
                    {
                        lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valSelectForQty"), this.lblHeader.Text);
                        return false;
                    }

                }

                if (count == 0)
                {
                    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valSelectOne"), this.lblHeader.Text);
                    return false;
                }
            }

            return true;
        }
        #endregion      

       

      
             
    }
}

