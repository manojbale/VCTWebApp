using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using VCTWeb.Core.Domain;
using VCTWebApp.Web;
using VCTWebApp.Resources;
using VCTWebApp.Shell.Views;
using Microsoft.Practices.ObjectBuilder;
using System.Text;

namespace VCTWebApp
{
    public partial class HospitalInventoryTransfer : Microsoft.Practices.CompositeWeb.Web.UI.Page, IHospitalInventoryTransfer
    {

        #region Instance Variables

        private HospitalInventoryTransferPresenter presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private Security security = null;

        #endregion

        #region Create New Presenter
        [CreateNew]
        public HospitalInventoryTransferPresenter Presenter
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
                //this.AuthorizedPage();
                presenter.OnViewInitialized();
                this.LocalizePage();
            }
        }
        #endregion

        #region IInventory Transfer Implementations
        public List<VCTWeb.Core.Domain.PartyAvailableCatalog> PartyAvailableCatalogList
        {
            get
            {
                return (List<VCTWeb.Core.Domain.PartyAvailableCatalog>)ViewState["PartyAvailableCatalogList"];
            }
            set
            {
                ViewState["PartyAvailableCatalogList"] = value;

                this.gdvPartDetails.DataSource = value;
                this.gdvPartDetails.DataBind();
                if (value.Count > 0 && String.IsNullOrEmpty(value[0].CatalogNumber))
                {
                    this.gdvPartDetails.Rows[0].Visible = false;
                }
            }
        }

        public List<VCTWeb.Core.Domain.Party> PartyList
        {
            set
            {
                ddlFromParty.DataSource = value;
                ddlFromParty.DataTextField = "Name";
                ddlFromParty.DataValueField = "PartyId";
                ddlFromParty.DataBind();

                ddlFromParty.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect"), "0"));


                ddlToParty.DataSource = value;
                ddlToParty.DataTextField = "Name";
                ddlToParty.DataValueField = "PartyId";
                ddlToParty.DataBind();

                ddlToParty.Items.Insert(0, new ListItem(vctResource.GetString("listItemSelect"), "0"));
            }
        }

        public List<VCTWeb.Core.Domain.HospitalInventoryTransfer> PartList
        {
            get
            {
                return (List<VCTWeb.Core.Domain.HospitalInventoryTransfer>)ViewState["PartList"];
            }
            set
            {
                ViewState["PartList"] = value;
            }
        }

        public string InventoryType
        {
            get
            {

                //if (rblstInventoryType.SelectedIndex == 0)
                //    return "Kit";
                //else
                    return "Part";
            }
        }

        public Int64 FromParty
        {
            get
            {
                return (Convert.ToInt64(ddlFromParty.SelectedValue));
            }
        }

        public Int64 ToParty
        {
            get
            {
                return (Convert.ToInt64(ddlToParty.SelectedValue));
            }
        }

        public string TableXml
        {
            get
            {
                return (CreateTableXml());
            }
        }
        #endregion

        #region Event Handlers

        protected void ddlFromParty_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["FromPartyId"] = ddlFromParty.SelectedValue;
            presenter.PopulateEmptyTable();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (ValidateSearch())
            {
                Session["FromPartyId"] = ddlFromParty.SelectedValue;
                presenter.PopulateEmptyTable();
            }

        }

        protected void gdvPartDetails_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                gdvPartDetails.EditIndex = e.NewEditIndex;
                this.PartyAvailableCatalogList = this.PartyAvailableCatalogList;

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
                this.PartyAvailableCatalogList = this.PartyAvailableCatalogList;

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
                        lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valRefNumberEmpty"), this.lblHeader.Text);
                        
                    }
                    else
                    {
                        if (ValidateSearch())
                        {
                           
                            TextBox txtNewTransferQty = gdvPartDetails.HeaderRow.FindControl("txtNewTransferQty") as TextBox;
                            TextBox txtNewLotNum = gdvPartDetails.HeaderRow.FindControl("txtNewLotNum") as TextBox;
                            TextBox txtNewDescription = gdvPartDetails.HeaderRow.FindControl("txtNewDescription") as TextBox;
                            TextBox txtNewAvailableQty = gdvPartDetails.HeaderRow.FindControl("txtNewAvailableQty") as TextBox;

                            
                            if (txtNewTransferQty != null)
                            {
                                if (txtNewTransferQty.Text.Trim()==string.Empty )
                                {  
                                    lblError.Text = "Please enter Transfer Quantity";
                                    return;
                                }
                                else if( int.Parse(txtNewTransferQty.Text) == 0)
                                {
                                 
                                    lblError.Text = "Transfer Quantity should be greater than zero";
                                    return;
                                }
                                else if (int.Parse(txtNewTransferQty.Text) > int.Parse(txtNewAvailableQty.Text))
                                {
                                  
                                    lblError.Text = "Transfer Quantity can't be greater than the Available Quantity";
                                    return;
                                }

                                if (ValidateItemsOnAdd(hdnPartNumNew.Value, txtNewTransferQty.Text))
                                {

                                 
                                    if (this.PartyAvailableCatalogList.Exists(t => t.CatalogNumber == hdnPartNumNew.Value && t.LotNum == txtNewLotNum.Text))
                                    {
                                        this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valPartLotExists"), this.lblHeader.Text);
                                    }
                                    else
                                    {
                                        this.PartyAvailableCatalogList.Add(new PartyAvailableCatalog()
                                        {
                                            CatalogNumber = hdnPartNumNew.Value,
                                            LotNum = txtNewLotNum.Text,
                                            AvailableQty = int.Parse(txtNewAvailableQty.Text),
                                            Description = txtNewDescription.Text,
                                            TransferQty = int.Parse(txtNewTransferQty.Text)
                                        });

                                        lblError.Text = "<font color='blue'>" + vctResource.GetString("msgItemAdded") + "</font>";
                                        hdnPartNumNew.Value = string.Empty;
                                    }

                                    RemoveEmptyRow(this.PartyAvailableCatalogList);
                                    this.PartyAvailableCatalogList = this.PartyAvailableCatalogList;
                                }
                            }

                        }

                    }
                }
                else if (e.CommandName.Equals("UpdateRec"))
                {
                    TextBox txtTransferQty = gdvPartDetails.Rows[gdvPartDetails.EditIndex].FindControl("txtTransferQty") as TextBox;
                    Label lblAvailableQty = gdvPartDetails.Rows[gdvPartDetails.EditIndex].FindControl("lblAvailableQty") as Label;

                    if (ValidateItemsOnEdit(txtTransferQty.Text, lblAvailableQty.Text))
                    {
                        this.PartyAvailableCatalogList[gdvPartDetails.EditIndex].TransferQty = int.Parse(txtTransferQty.Text);
                        gdvPartDetails.EditIndex = -1;
                        this.PartyAvailableCatalogList = this.PartyAvailableCatalogList;

                        lblError.Text = "<font color='blue'>" + vctResource.GetString("msgItemUpdated") + "</font>";
                    }

                }
                else if (e.CommandName.Equals("DeleteRec"))
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    if (rowIndex >= 0)
                    {

                        this.PartyAvailableCatalogList.RemoveAt(rowIndex);
                        if (this.PartyAvailableCatalogList.Count == 0)
                        {
                            this.PartyAvailableCatalogList.Add(new PartyAvailableCatalog());
                            this.PartyAvailableCatalogList = this.PartyAvailableCatalogList;
                        }
                        else
                        {
                            this.PartyAvailableCatalogList = this.PartyAvailableCatalogList;
                        }

                        lblError.Text = "<font color='blue'>" + vctResource.GetString("msgItemDeleted") + "</font>";

                    }
                }
            }
            catch (Exception ex)
            {
                this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        private void RemoveEmptyRow(List<VCTWeb.Core.Domain.PartyAvailableCatalog> value)
        {
            if (value != null)
            {
                value.RemoveAll(t => t.CatalogNumber == null);
            }
        }

        private bool ValidateItemsOnEdit(string newQuantity, string availableQty)
        {
            if (string.IsNullOrEmpty(newQuantity))
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valQuantity"), this.lblHeader.Text);
                return false;
            }
            else if (Convert.ToInt32(newQuantity) < 1)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valQuantityValue"), this.lblHeader.Text);
                return false;
            }
            else if (Convert.ToInt32(newQuantity) > Convert.ToInt32(availableQty))
            {
                lblError.Text = "Transfer Quantity can't be greater than the Available Quantity";
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

        #endregion

        #region Private Variables
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string result = "";
            try
            {

                if (IsValidPage())
                {
                    if (!presenter.Save(out result))
                    {
                        this.lblError.Text = "Problem in transferring inventory for selected Part(s). The selected Part(s) may already be transferred. Please try after refreshing the page.";
                    }
                    else
                    {
                        this.lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgCommonforSaved"), result, this.lblHeader.Text) + "</font>";
                        ClearFields();
                    }

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            ClearFields();
            lblError.Text = string.Empty;
        }

        private bool ValidateSearch()
        {
            lblError.Text = "";
            if (ddlFromParty.SelectedValue == "0")
            {
                lblError.Text = "From Party must be selected";
                return false;
            }
            else if (ddlToParty.SelectedValue == "0")
            {
                lblError.Text = "To Party must be selected";
                return false;
            }
            else if (ddlFromParty.SelectedValue == ddlToParty.SelectedValue)
            {
                lblError.Text = "From Party and To Party Location should be different.";
                return false;
            }

            return true;
        }

        private string CreateTableXml()
        {
            StringBuilder TableXML = new StringBuilder();

            TableXML.Append("<root>");

            string PartNum;
            string LotNum;
            //LocationPartDetailId, PartCurrentStatus, PartStatus;            
            int Qty = 0;

            foreach (GridViewRow row in gdvPartDetails.Rows)
            {
                PartNum = (row.FindControl("lblPartNum") as Label).Text;
                LotNum = (row.FindControl("lblLotNum") as Label).Text;
                Qty = Int16.Parse((row.FindControl("lblTransferQty") as Label).Text);

                TableXML.Append("<CasesTable>");
                TableXML.Append("<PartNum>"); TableXML.Append(PartNum); TableXML.Append("</PartNum>");
                TableXML.Append("<LotNum>"); TableXML.Append(LotNum); TableXML.Append("</LotNum>");
                TableXML.Append("<Quantity>"); TableXML.Append(Qty); TableXML.Append("</Quantity>");
                TableXML.Append("</CasesTable>");
            }

            TableXML.Append("</root>");
            return TableXML.ToString();
        }

        private void ClearFields()
        {
            ddlFromParty.SelectedValue = "0";
            ddlToParty.SelectedValue = "0";
            hdnPartNumNew.Value = string.Empty;
            presenter.PopulateEmptyTable();
        }

        private void LocalizePage()
        {

        }

        private bool IsValidPage()
        {
            bool flag = true;

            if (Page.IsValid)
            {
                lblError.Text = "";
                if (ddlFromParty.SelectedValue == "0")
                {
                    lblError.Text = "From Party must be selected";
                    return false;
                }
                else if (ddlToParty.SelectedValue == "0")
                {
                    lblError.Text = "To Party must be selected";
                    return false;
                }
                else if (ddlFromParty.SelectedValue == ddlToParty.SelectedValue)
                {
                    lblError.Text = "From Party and To Party Location should be different.";
                    return false;
                }

                if (gdvPartDetails == null || gdvPartDetails.Rows.Count == 0 || string.IsNullOrEmpty((gdvPartDetails.Rows[0].FindControl("lblPartNum") as Label).Text))
                {
                    lblError.Text = "Part # missing for Inventory Transfer.";
                    return false;
                }

            }

            return flag;
        }

        private void AuthorizedPage()
        {
            security = new Security();
            if (Session["LoggedInUser"] == null)
            {
                Response.Redirect("~/ErrorPage.aspx?ErrorKey=Common_msgSessionExpired");
            }
            else if (security.HasAccess("InventoryTransfer"))
            {
                CanView = security.HasPermission("InventoryTransfer");
            }
            else
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }
        #endregion

    }
}