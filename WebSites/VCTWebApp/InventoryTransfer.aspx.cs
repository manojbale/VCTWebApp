using System;
using Microsoft.Practices.ObjectBuilder;
using System.Data;
using VCTWebApp.Resources;
using VCTWebApp.Web;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Globalization;
using VCTWeb.Core.Domain;
using System.Text;

namespace VCTWebApp.Shell.Views
{       

    public partial class InventoryTransfer : Microsoft.Practices.CompositeWeb.Web.UI.Page, IInventoryTransferView
    {

        #region Instance Variables
        private InventoryTransferPresenter presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private Security security = null;
        #endregion

        #region Create New Presenter
        [CreateNew]
        public InventoryTransferPresenter Presenter
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
            //if (radKit.Checked)
            //{
            //    tblKit.Visible = true;
            //    tblPart.Visible = false;
            //}
            //else
            //{
            //    tblPart.Visible = true;
            //    tblKit.Visible = false;
            //}

            if (!this.IsPostBack)
            {
                this.AuthorizedPage();
                presenter.OnViewInitialized();                
                this.LocalizePage();
            }
        }
        #endregion

        #region IInventory Transfer Implementations

        public List<VCTWeb.Core.Domain.InventoryTransfer> InventoryTransKitList
        {
            set
            {
                gdvPart.Visible = false;
                gdvKit.Visible = true;                
 
                gdvKit.DataSource = value;
                gdvKit.DataBind();

                if (value.Count > 0 && (string.IsNullOrEmpty(value[0].PartNum)))
                {
                    this.gdvKit.Rows[0].Visible = false;
                }
            }
        }

        public List<VCTWeb.Core.Domain.InventoryTransfer> InventoryTransPartList
        {
            set
            {
                gdvKit.Visible = false;  
                gdvPart.Visible = true;

                gdvPart.DataSource = value;
                gdvPart.DataBind();

                if (value.Count > 0 && (string.IsNullOrEmpty(value[0].PartNum)))
                {
                    this.gdvPart.Rows[0].Visible = false;
                }
            }
        }

        public List<VCTWeb.Core.Domain.InventoryTransfer> RegionBranchList 
        {
            get
            {
                return (List<VCTWeb.Core.Domain.InventoryTransfer>)ViewState["RegionBranchList"];
            }
            set
            {
                ViewState["RegionBranchList"] = value;

                ddlRegionBranchLocation.DataSource = value;
                ddlRegionBranchLocation.DataTextField = "LocationName";
                ddlRegionBranchLocation.DataValueField = "LocationId";
                ddlRegionBranchLocation.DataBind();
                ddlRegionBranchLocation.Items.Insert(0, new ListItem("-- Select --", "0"));

                ddlToRegionBranch.DataSource = value;
                ddlToRegionBranch.DataTextField = "LocationName";
                ddlToRegionBranch.DataValueField = "LocationId";
                ddlToRegionBranch.DataBind();
                ddlToRegionBranch.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
        }

        public List<VCTWeb.Core.Domain.InventoryTransfer> ToLocationList
        {
            get
            { 
                    return (List<VCTWeb.Core.Domain.InventoryTransfer>)ViewState["TransferToLocations"];
            }
            set
            {
                ViewState["TransferToLocations"] = value;
            }
        }

        public Int32 FromLocationId
        {
            get
            {
                return (Convert.ToInt32(ddlRegionBranchLocation.SelectedValue));
            }
        }

        public int InventoryDays 
        {
            get
            { 
                return (int.Parse(txtInventoryDays.Text));
            }
            set
            {
                ViewState["InventoryNotInUseDays"] = value;
                txtInventoryDays.Text = value.ToString();
            }
        }

        public VCTWeb.Core.Domain.Constants.InventoryType InventoryType 
        {
            get
            { 
                return(radKit.Checked ? VCTWeb.Core.Domain.Constants.InventoryType.Kit : VCTWeb.Core.Domain.Constants.InventoryType.Part);
            }
        }

        public string TableXml
        {
            get
            {
                return CreateTableXml();
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

        #endregion

        #region Event Handlers    
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (ddlRegionBranchLocation.SelectedValue == "0")
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("InventoryTransfer_RegionAndBranchMissing"), this.lblHeader.Text);
                return;
            }
            else if (ddlToRegionBranch.SelectedValue == "0")
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("InventoryTransfer_ToLocationShouldBeSelected"), this.lblHeader.Text);
                return;
            }
            else if (ddlRegionBranchLocation.SelectedValue == ddlToRegionBranch.SelectedValue)
            {
                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("InventoryTransfer_ToLocationShouldBeDifferent"), this.lblHeader.Text);
                return;
            }
            else
            {
                pnlAdHocKit.Visible = false;
                pnlAdHocPart.Visible = false;
                pnlUnUtilizeKit.Visible = false;
                pnlUnUtilizePart.Visible = false;

                if (radKit.Checked)
                {
                    if (radUnUtilizeOpt.Checked)
                    {
                        pnlUnUtilizeKit.Visible = true;
                        presenter.OnViewLoaded();
                    }
                    else
                    {
                        pnlAdHocKit.Visible = true;
                    }

                }
                else
                {
                    if (radUnUtilizeOpt.Checked)
                    {
                        pnlUnUtilizePart.Visible = true;
                        presenter.OnViewLoaded();
                    }
                    else
                    {
                        pnlAdHocPart.Visible = true;
                        presenter.PopulateEmptyKitTable();
                    }
                }

            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string result = "";
            try
            {
                
                if (IsValidPage())
                {
                    presenter.Save(out result);
                    this.lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgCommonforSaved"), result, this.lblHeader.Text) + "</font>";
                    //this.lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgCreated"), this.lblHeader.Text) + "</font>";
                    ClearFields();
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

        #endregion

        #region Private Methods

        private void AuthorizedPage()
        {
            security = new Security();
            if (Session["LoggedInUser"] == null)
            {
                Response.Redirect("~/ErrorPage.aspx?ErrorKey=Common_msgSessionExpired");
            }
            else if (security.HasAccess("InventoryTransfer"))
            {
                //CanCancel = security.HasPermission("InventoryTransfer");
            }
            else
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }

        private void ClearFields()
        {
            presenter.OnViewInitialized();
            gdvKit.DataSource = null;
            gdvKit.DataBind();

            gdvPart.DataSource = null;
            gdvPart.DataBind();

            txtKitFamily.Text = string.Empty;
            txtDes.Text = string.Empty;
            txtTransferQty.Text = string.Empty;
        }

        private bool IsValidPage()
        {

            if (Page.IsValid)
            {
                if (ddlRegionBranchLocation.SelectedValue == "0")
                {
                    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("InventoryTransfer_RegionAndBranchMissing"), this.lblHeader.Text);
                    return false;
                }
                else if (ddlToRegionBranch.SelectedValue == "0")
                {
                    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("InventoryTransfer_ToLocationShouldBeSelected"), this.lblHeader.Text);
                    return false;
                }
                else if (ddlRegionBranchLocation.SelectedValue == ddlToRegionBranch.SelectedValue)
                {
                    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("InventoryTransfer_ToLocationShouldBeDifferent"), this.lblHeader.Text);
                    return false;
                }                
                else if (this.InventoryType == Constants.InventoryType.Part)
                {
                 
                    bool chkStatus;
                    Int32 AvailableQty, TransferQty, ToLocationId, Count=0;
                    
                    if (radUnUtilizeOpt.Checked)
                    {
                        if (string.IsNullOrEmpty(txtInventoryDays.Text))
                        {
                            lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("InventoryTransfer_InventoryDaysMissing"), this.lblHeader.Text);
                            return false;
                        }

                        foreach (GridViewRow row in gdvPart.Rows)
                        {
                            chkStatus = (row.FindControl("chkSelect") as CheckBox).Checked;
                            AvailableQty = int.Parse((row.FindControl("lblAvailableQty") as Label).Text);
                            TextBox txtQuantity = row.FindControl("txtQuantity") as TextBox;
                            if (txtQuantity != null && (!string.IsNullOrEmpty(txtQuantity.Text)))
                            {
                                TransferQty = int.Parse(txtQuantity.Text);
                            }
                            else
                            {
                                TransferQty = 0;
                            }
                            //ToLocationId = int.Parse((row.FindControl("ddlTransferToLocation") as DropDownList).SelectedValue);

                            if (chkStatus == true)
                            {
                                Count += 1;
                                if (TransferQty <= 0)
                                {
                                    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("InventoryTransfer_QtyShouldGreaterThanZero"), this.lblHeader.Text);
                                    return false;
                                }

                                if (TransferQty > AvailableQty)
                                {
                                    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("InventoryTransfer_TransferQtyShouldGreaterAvailQty"), this.lblHeader.Text);
                                    return false;
                                }

                                //if (ToLocationId == 0)
                                //{
                                //    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("InventoryTransfer_ToLocationShouldBeSelected"), this.lblHeader.Text);
                                //    return false;
                                //}

                                //if (Convert.ToInt32(ddlRegionBranchLocation.SelectedValue) == ToLocationId)
                                //{
                                //    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("InventoryTransfer_ToLocationShouldBeDifferent"), this.lblHeader.Text);
                                //    return false;
                                //}

                            }

                        }

                        if (Count == 0)
                        {
                            lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("InventoryTransfer_PartMissing"), this.lblHeader.Text);
                            return false;
                        }

                    }
                    else
                    {
                        if (gdvPartDetails != null && gdvPartDetails.Rows.Count != 0)
                        {
                            string PartNum = (gdvPartDetails.Rows[0].FindControl("lblPartNum") as Label).Text;
                            if (string.IsNullOrEmpty(PartNum))
                            {
                                lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("InventoryTransfer_PartMissing"), this.lblHeader.Text);
                                return false;
                            }
                        }
                    }
                }
                else if (this.InventoryType == Constants.InventoryType.Kit)
                {                   
                    bool chkStatus;
                    Int32 AvailableQty, TransferQty, ToLocationId, Count = 0;

                    if (radUnUtilizeOpt.Checked)
                    {
                        if (string.IsNullOrEmpty(txtInventoryDays.Text))
                        {
                            lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("InventoryTransfer_InventoryDaysMissing"), this.lblHeader.Text);
                            return false;
                        }

                        foreach (GridViewRow row in gdvKit.Rows)
                        {
                            chkStatus = (row.FindControl("chkSelect") as CheckBox).Checked;
                            AvailableQty = int.Parse((row.FindControl("lblAvailableQty") as Label).Text);
                            TransferQty = int.Parse((row.FindControl("txtQuantity") as TextBox).Text);
                            //ToLocationId = int.Parse((row.FindControl("ddlTransferToLocation") as DropDownList).SelectedValue);

                            if (chkStatus == true)
                            {
                                Count += 1;
                                if (TransferQty <= 0)
                                {
                                    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("InventoryTransfer_QtyShouldGreaterThanZero"), this.lblHeader.Text);
                                    return false;
                                }

                                if (TransferQty > AvailableQty)
                                {
                                    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("InventoryTransfer_TransferQtyShouldGreaterAvailQty"), this.lblHeader.Text);
                                    return false;
                                }

                                //if (ToLocationId == 0)
                                //{
                                //    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("InventoryTransfer_ToLocationShouldBeSelected"), this.lblHeader.Text);
                                //    return false;
                                //}

                                //if (Convert.ToInt32(ddlRegionBranchLocation.SelectedValue) == ToLocationId)
                                //{
                                //    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("InventoryTransfer_ToLocationShouldBeDifferent"), this.lblHeader.Text);
                                //    return false;
                                //}

                            }

                        }

                        if (Count == 0)
                        {
                            lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("InventoryTransfer_KitMissing"), this.lblHeader.Text);
                            return false;
                        }

                    }
                    else
                    {
                        int.TryParse(txtTransferQty.Text, out TransferQty);
                        //int.TryParse(txtAvailableQty.Text, out AvailableQty);

                        if (TransferQty <= 0)
                        {
                            lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("InventoryTransfer_QtyShouldGreaterThanZero"), this.lblHeader.Text);
                            return false;
                        }

                        //if (TransferQty > AvailableQty)
                        //{
                        //    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("InventoryTransfer_TransferQtyShouldGreaterAvailQty"), this.lblHeader.Text);
                        //    return false;
                        //}
                    }


                }

            }

            return true;
        }
             
        private string CreateTableXml()
        {
            System.Text.StringBuilder TableXml = new System.Text.StringBuilder();
            TableXml.Append("<root>");
            List<VCTWeb.Core.Domain.InventoryTransfer> oModelList = new List<VCTWeb.Core.Domain.InventoryTransfer>();

            if (radPart.Checked)
            {
                if (radUnUtilizeOpt.Checked)
                {
                    foreach (GridViewRow row in gdvPart.Rows)
                    {
                        CheckBox chkSelect = row.FindControl("chkSelect") as CheckBox;

                        if (chkSelect.Checked)
                        {
                            oModelList.Add(new VCTWeb.Core.Domain.InventoryTransfer()
                            {
                                PartNum = (row.FindControl("lblPartNum") as Label).Text,
                                TransferQty = Convert.ToInt32((row.FindControl("txtQuantity") as TextBox).Text),
                                LocationId = Convert.ToInt32(ddlToRegionBranch.SelectedValue)
                            });

                            //TableXml.Append("<CasesTable>");
                            //TableXml.Append("<PartNum>"); TableXml.Append((row.FindControl("lblPartNum") as Label).Text); TableXml.Append("</PartNum>");
                            //TableXml.Append("<Quantity>"); TableXml.Append((row.FindControl("txtQuantity") as TextBox).Text); TableXml.Append("</Quantity>");
                            ////TableXml.Append("<ToLocationId>"); TableXml.Append((row.FindControl("ddlTransferToLocation") as DropDownList).SelectedValue); TableXml.Append("</ToLocationId>");
                            //TableXml.Append("<ToLocationId>"); TableXml.Append(ddlToRegionBranch.SelectedValue); TableXml.Append("</ToLocationId>");
                            //TableXml.Append("</CasesTable>");
                        }
                    }
                }
                else
                {
                    foreach (GridViewRow row in gdvPartDetails.Rows)
                    {
                        oModelList.Add(new VCTWeb.Core.Domain.InventoryTransfer()
                        {
                            PartNum = (row.FindControl("lblPartNum") as Label).Text,
                            TransferQty = Convert.ToInt32((row.FindControl("lblPARLevelQty") as Label).Text),
                            LocationId = Convert.ToInt32(ddlToRegionBranch.SelectedValue)
                        });
                    }
                }

                GetXML("PART", ref TableXml, oModelList);
            }
            else
            {

                if (radUnUtilizeOpt.Checked)
                {


                    foreach (GridViewRow row in gdvKit.Rows)
                    {
                        CheckBox chkSelect = row.FindControl("chkSelect") as CheckBox;

                        if (chkSelect.Checked)
                        {
                            oModelList.Add(new VCTWeb.Core.Domain.InventoryTransfer()
                            {
                                KitFamilyId = Convert.ToInt64((row.FindControl("hdnKitFamilyId") as HiddenField).Value),
                                TransferQty = Convert.ToInt32((row.FindControl("txtQuantity") as TextBox).Text),
                                LocationId = Convert.ToInt32(ddlToRegionBranch.SelectedValue)
                            });

                            //TableXml.Append("<CasesTable>");
                            //TableXml.Append("<KitFamilyId>"); TableXml.Append((row.FindControl("hdnKitFamilyId") as HiddenField).Value); TableXml.Append("</KitFamilyId>");
                            //TableXml.Append("<Quantity>"); TableXml.Append((row.FindControl("txtQuantity") as TextBox).Text); TableXml.Append("</Quantity>");
                            ////TableXml.Append("<ToLocationId>"); TableXml.Append((row.FindControl("ddlTransferToLocation") as DropDownList).SelectedValue); TableXml.Append("</ToLocationId>");
                            //TableXml.Append("<ToLocationId>"); TableXml.Append(ddlToRegionBranch.SelectedValue); TableXml.Append("</ToLocationId>");
                            //TableXml.Append("</CasesTable>");
                        }
                    }

                }
                else
                {
                    oModelList.Add(new VCTWeb.Core.Domain.InventoryTransfer()
                    {
                        KitFamilyId = Convert.ToInt64(hdnKitFamilyId.Value),
                        TransferQty = Convert.ToInt32(txtTransferQty.Text),
                        LocationId = Convert.ToInt32(ddlToRegionBranch.SelectedValue)
                    });
                }

                GetXML("KIT",ref TableXml, oModelList);
            }

            TableXml.Append("</root>");

            return TableXml.ToString();
        }

        private void GetXML(string InventoryType,ref StringBuilder TableXml, List<VCTWeb.Core.Domain.InventoryTransfer> oModel)
        {
            if (InventoryType.ToUpper() == "KIT")
            {
                foreach (var item in oModel)
                {
                    TableXml.Append("<CasesTable>");
                    TableXml.Append("<KitFamilyId>"); TableXml.Append( item.KitFamilyId.ToString()); TableXml.Append("</KitFamilyId>");
                    TableXml.Append("<Quantity>"); TableXml.Append(item.TransferQty.ToString()); TableXml.Append("</Quantity>");  
                    TableXml.Append("<ToLocationId>"); TableXml.Append( item.LocationId.ToString() ); TableXml.Append("</ToLocationId>");
                    TableXml.Append("</CasesTable>");
                }
            }
            else
            {
                foreach (var item in oModel)
                {
                    TableXml.Append("<CasesTable>");
                    TableXml.Append("<PartNum>"); TableXml.Append(item.PartNum); TableXml.Append("</PartNum>");
                    TableXml.Append("<Quantity>"); TableXml.Append(item.TransferQty.ToString()); TableXml.Append("</Quantity>");
                    TableXml.Append("<ToLocationId>"); TableXml.Append(item.LocationId.ToString()); TableXml.Append("</ToLocationId>");
                    TableXml.Append("</CasesTable>");
                }
            }
        }

        private void LocalizePage()
        {
            lblLocation.Text = vctResource.GetString("IT_lblLocation");
            lblKit.Text = vctResource.GetString("IT_lblKit");
            lblParts.Text = vctResource.GetString("IT_lblParts");
            lblInventoryNotInUse.Text = vctResource.GetString("IT_lblInventoryNotInUse");

            String errorMessage = vctResource.GetString("required");
            rfv_RegionBranchLocation.Text = errorMessage;
            rfv_InventoryDays.Text = errorMessage;
        }
        #endregion
        
        #region Protected Methods
        protected void gdvPart_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.Header)
            {
                (e.Row.FindControl("lblSelectHeader") as Label).Text = vctResource.GetString("IT_gv_Select");
                (e.Row.FindControl("lblPartNumHeader") as Label).Text = vctResource.GetString("IT_gv_PartNum");
                (e.Row.FindControl("lblDescriptionHeader") as Label).Text = vctResource.GetString("IT_gv_Description");
                (e.Row.FindControl("lblLastUsageHeader") as Label).Text = vctResource.GetString("IT_gv_LastUsage");
                (e.Row.FindControl("lblAvailableQtyHeader") as Label).Text = vctResource.GetString("IT_gv_AvailableQty");
                (e.Row.FindControl("lblTransferQtyHeader") as Label).Text = vctResource.GetString("IT_gv_TransferQty");
                //(e.Row.FindControl("lblTransferToLocation") as Label).Text = vctResource.GetString("IT_gv_TransferTo");
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //DropDownList ddlTransferToLoc = e.Row.FindControl("ddlTransferToLocation") as DropDownList;

                //ddlTransferToLoc.DataSource = this.RegionBranchList;
                //ddlTransferToLoc.DataTextField = "LocationName";
                //ddlTransferToLoc.DataValueField = "LocationId";
                //ddlTransferToLoc.DataBind();

                //ddlTransferToLoc.Items.Insert(0, new ListItem(" -- Select -- ", "0"));                              
               
            }

        }

        protected void gdvKit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                (e.Row.FindControl("lblSelectHeader") as Label).Text = vctResource.GetString("IT_gv_Select");
                (e.Row.FindControl("lblKitFamilyHeader") as Label).Text = vctResource.GetString("IT_gv_KitFamily");
                (e.Row.FindControl("lblDescriptionHeader") as Label).Text = vctResource.GetString("IT_gv_Description");
                (e.Row.FindControl("lblLastUsageHeader") as Label).Text = vctResource.GetString("IT_gv_LastUsage");
                (e.Row.FindControl("lblAvailableQtyHeader") as Label).Text = vctResource.GetString("IT_gv_AvailableQty");
                (e.Row.FindControl("lblTransferQtyHeader") as Label).Text = vctResource.GetString("IT_gv_TransferQty");
                //(e.Row.FindControl("lblTransferToLocation") as Label).Text = vctResource.GetString("IT_gv_TransferTo");
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //DropDownList ddlTransferToLoc = e.Row.FindControl("ddlTransferToLocation") as DropDownList;

                //ddlTransferToLoc.DataSource = this.RegionBranchList;
                //ddlTransferToLoc.DataTextField = "LocationName";
                //ddlTransferToLoc.DataValueField = "LocationId";
                //ddlTransferToLoc.DataBind();

                //ddlTransferToLoc.Items.Insert(0, new ListItem(" -- Select -- ", "0"));
            }

        }

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
                        lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valPartNumberEmpty"), this.lblHeader.Text);
                    }
                    else
                    {
                        TextBox txtNewPartQty = gdvPartDetails.HeaderRow.FindControl("txtNewPartQty") as TextBox;
                        TextBox txtNewDescription = gdvPartDetails.HeaderRow.FindControl("txtNewDescription") as TextBox;
                        if (txtNewPartQty != null)
                        {
                            if (ValidateItemsOnAdd(hdnPartNumNew.Value, txtNewPartQty.Text))
                            {

                                if (this.KitFamilyPartsList.Exists(t => t.CatalogNumber == hdnPartNumNew.Value))
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
                                        CatalogNumber = hdnPartNumNew.Value,
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

        protected void radUnUtilizeOpt_CheckedChanged(object sender, EventArgs e)
        {
            pnlAdHocKit.Visible = false;
            pnlAdHocPart.Visible = false;
            pnlUnUtilizeKit.Visible = false;
            pnlUnUtilizePart.Visible = false;

            if (radUnUtilizeOpt.Checked)
            {
                lblInventoryNotInUse.Visible = true;
                txtInventoryDays.Visible = true;
                txtInventoryDays.Text = Convert.ToString(ViewState["InventoryNotInUseDays"]);
            }
            
        }

        protected void radAdhocOpt_CheckedChanged(object sender, EventArgs e)
        {
            pnlAdHocKit.Visible = false;
            pnlAdHocPart.Visible = false;
            pnlUnUtilizeKit.Visible = false;
            pnlUnUtilizePart.Visible = false;

            if (radAdhocOpt.Checked)
            {
                lblInventoryNotInUse.Visible = false;
                txtInventoryDays.Visible = false;
                txtInventoryDays.Text = "0";
            }
        }
        
        
    }

        
}



