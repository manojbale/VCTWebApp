using System;
using Microsoft.Practices.ObjectBuilder;
using System.Data;
using VCTWebApp.Resources;
using VCTWeb.Core.Domain;
using VCTWebApp.Web;
using System.Data.SqlClient;
using System.Globalization;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Text;

namespace VCTWebApp.Shell.Views
{
    public partial class VirtualBuildKit : Microsoft.Practices.CompositeWeb.Web.UI.Page, IVirtualBuildKitView
    {
        
        #region Instance Variables

        private VirtualBuildKitPresenter presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private Helper helper = new Helper();
        private Security security = null;

        #endregion

        #region Create New Presenter

        [CreateNew]
        public VirtualBuildKitPresenter Presenter
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
                    this.Form.DefaultButton = this.btnSearch.UniqueID; //Set the default button to search.

                    this.DisplayMessageForMissingMasters();

                    this.txtKitNumber.Focus();

                    if (Session["PendingBuildKitNumber"] != null && Session["PendingBuildKitNumber"].ToString() != string.Empty)
                    {
                        this.txtKitNumber.Text = Session["PendingBuildKitNumber"].ToString() + "( " + Session["PendingBuildKitDescription"].ToString() + " )";
                        this.hdnKitNumber.Value = Session["PendingBuildKitNumber"].ToString();
                        Session["PendingBuildKitNumber"] = null;
                        btnSearch_Click(sender, e);
                    }
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

        #region Private Methods

        private bool IsValidInput()
        {
            if (string.IsNullOrEmpty(txtKitNumber.Text))
            {
                lblError.Text = vctResource.GetString("valKitType");
                txtKitNumber.Focus();
                return false;
            }

            int count = 0, selCount=0, lotExists=0, chkCount = 0;
            List<string> lstLotNum = new List<string>();
            foreach (GridViewRow row in gdVirtualBuildKit.Rows)
            {                
                CheckBox chkStatus = row.FindControl("chkStatus") as CheckBox;
                DropDownList ddlLotNum = row.FindControl("ddlLotNum") as DropDownList;

                if (chkStatus.Checked)
                {
                    count += 1;
                    
                    if (ddlLotNum.SelectedValue == "0")
                    {
                        //** dropdown not Selected for checked item************************************                                  
                        selCount += 1;                        
                    }                    
                    else if (ddlLotNum.SelectedValue != "0")
                    {
                        //** check duplicate selection for LotNum  ************************************                              
                        if (lstLotNum.Contains(ddlLotNum.SelectedValue))
                        {
                            lotExists += 1;
                        }
                        else
                        {
                            lstLotNum.Add(ddlLotNum.SelectedValue);
                        }
                    }                    
                }
                ////**Begin - item not Selected for selected LotNum************************************                
                //if (lotNum != "0")
                //{                                  
                //    if (chkStatus.Checked == false) chkCount += 1;
                //}
                ////**Begin - item not Selected for selected LotNum************************************
            }

            if (count == 0)
            {
                lblError.Text = vctResource.GetString("valLeastKitItem");                
                return false;
            }
            else if (selCount > 0)
            {
                lblError.Text = vctResource.GetString("valDropDownSelectForSelect");
                return false;
            }
            else if (lotExists > 0)
            {
                lblError.Text = vctResource.GetString("valDuplicateLotNum");
                return false;
            }
            //else if (chkCount > 0)
            //{
            //    lblError.Text = vctResource.GetString("valItemSelectForSelectedLotNum");
            //    return false;
            //}
            
            return true;
        }

        private void AuthorizedPage()
        {
            security = new Security();
            if (Session["LoggedInUser"] == null)
            {
                Response.Redirect("~/ErrorPage.aspx?ErrorKey=Common_msgSessionExpired");
            }
            else if (security.HasAccess("WebBuildKit"))
            {
                CanView = security.HasPermission("WebBuildKit");
            }
            else
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }

        private void LocalizePage()
        {
            try
            {
                string heading = string.Empty;
                heading = vctResource.GetString("Label_BuildKit_Header");
                
                lblHeader.Text = heading;
                Page.Title = heading;

                this.lblKitNumber.Text = vctResource.GetString("Label_BuildKit_KitNumber");
                                           
                String errorMessage = vctResource.GetString("required");
                this.rfv_KitNumber.ErrorMessage = errorMessage;
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
            //if (string.IsNullOrEmpty(newQuantity))
            //{
            //    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valQuantity"), this.lblHeader.Text);
            //    return false;
            //}
            //if (Convert.ToInt32(newQuantity) < 1)
            //{
            //    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valQuantityValue"), this.lblHeader.Text);
            //    return false;
            //}
            return true;
        }

        private bool ValidateItemsOnAdd(string newParNum, string newQuantity)
        {
            //if (string.IsNullOrEmpty(newParNum))
            //{
            //    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valPartNumberEmpty"), this.lblHeader.Text);
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(newQuantity))
            //{
            //    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valQuantity"), this.lblHeader.Text);
            //    return false;
            //}
            //else if (Convert.ToInt32(newQuantity) < 1)
            //{
            //    lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valQuantityValue"), this.lblHeader.Text);
            //    return false;
            //}
            ////else
            ////{
            ////    var item = this.LocationPARLevelList.Find(t => t.PartNum == newParNum);
            ////    if (item != null)
            ////    {
            ////        lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valPartNumberExists"), this.lblHeader.Text);
            ////        return false;
            ////    }
            ////}      



            return true;
        }
        
        private string CreateVirtualBuildKitTableXml()
        {
            // Edit Mode no xml is being sent. Items are Added/edited indiviually.          
            StringBuilder VirtualKitXml = new StringBuilder();
            VirtualKitXml.Append("<root>");
            int count = 0;
            
            foreach (GridViewRow row in gdVirtualBuildKit.Rows)
            {
                CheckBox chk = row.FindControl("chkStatus") as CheckBox;
                if (chk.Checked)
                {
                    count += 1;

                    string partNum = (row.FindControl("lblPartNumber") as Label).Text;
                    string partDesc = (row.FindControl("lblDesc") as Label).Text;
                    string partLotNum = (row.FindControl("ddlLotNum") as DropDownList).SelectedItem.Text;                    
                    Int64 LocationPartDetailId = Convert.ToInt64((row.FindControl("ddlLotNum") as DropDownList).SelectedValue);

                    VirtualKitXml.Append("<VirtualBuilKit>");                    
                    VirtualKitXml.Append("<LocationId>"); VirtualKitXml.Append(Session["LoggedInLocationId"]); VirtualKitXml.Append("</LocationId>");
                    VirtualKitXml.Append("<PartNum>"); VirtualKitXml.Append(partNum); VirtualKitXml.Append("</PartNum>");
                    VirtualKitXml.Append("<Description>"); VirtualKitXml.Append(partDesc); VirtualKitXml.Append("</Description>");
                    VirtualKitXml.Append("<LotNum>"); VirtualKitXml.Append(partLotNum); VirtualKitXml.Append("</LotNum>");
                    VirtualKitXml.Append("<LocationPartDetailId>"); VirtualKitXml.Append(LocationPartDetailId); VirtualKitXml.Append("</LocationPartDetailId>");
                    VirtualKitXml.Append("</VirtualBuilKit>");
                }
            }
            VirtualKitXml.Append("</root>");
            if (count == 0)
            {
                VirtualKitXml.Clear();
            }

            return VirtualKitXml.ToString();
        }

        private void GetKitNumber()
        {
            string kitNumber = txtKitNumber.Text.Trim();
            if (kitNumber.Contains("("))
            {
                string[] arr = kitNumber.Split('(');
                hdnKitNumber.Value = arr[0].Trim();
            }
            else
            {
                hdnKitNumber.Value = kitNumber;
            }
        }
        #endregion        
                
        #region IVirtualBuilKitView Members
        public List<VirtualBuilKit> VirtualBuildKitList
        {            
            get
            {
                return (List<VirtualBuilKit>)ViewState["VirtualBuilKit"];
            }
            set
            {
                ViewState["VirtualBuilKit"] = value;
                this.gdVirtualBuildKit.DataSource = value;
                this.gdVirtualBuildKit.DataBind();
            }
        }

        public string KitNumber
        {
            get 
            {
                return (hdnKitNumber.Value);
            }

            set
            {
                hdnKitNumber.Value = value;
                txtKitNumber.Text = value;
            }
        }

        public List<VCTWeb.Core.Domain.VirtualBuilKit> LotNumberList
        {
            get 
            {
                return (List<VCTWeb.Core.Domain.VirtualBuilKit>)ViewState["LotNumberList"];
            }
            set
            {
                ViewState["LotNumberList"] = value;
            }
        }

        public List<VCTWeb.Core.Domain.VirtualBuilKit> SelectedBuildKitList
        { 
            get 
            {
                return (List<VCTWeb.Core.Domain.VirtualBuilKit>)ViewState["SelectedBuildKit"];
            }
            set
            {
                ViewState["SelectedBuildKit"] = value;
            }
        }

        public string VirtualBuildKitTableXml
        {
            get
            {
                return CreateVirtualBuildKitTableXml();
            }
        }

        //public string KitNumberDesc
        //{
        //    get 
        //    {
        //        return (hdnKitNumDesc.Value);
        //    }
        //    set
        //    {
        //        hdnKitNumDesc.Value = value;    
        //    }
        //}

        public int LocationId
        {
            get
            {
                return (Convert.ToInt32(Session["LoggedInLocationId"]));
            }
        }

        public string PartNum
        {
            get
            {
                return (string)ViewState["PartNum"];
            }
            set
            {
                ViewState["PartNum"] = value;
            }
        }

        public Int64 KitFamilyId
        {
            get {
                return (Int64)ViewState["KitFamilyId"];
            }
            set
            {
                ViewState["KitFamilyId"] = value;
            }
        }
        #endregion
        
        #region Protected Methods
        int tmpIndex = 0;
        string tmpPartNum = "";
        protected void gdVirtualBuildKit_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //Label lblAddRequestId = e.Row.FindControl("lblAddRequestId") as Label;
                //lblAddRequestId.Text = vctResource.GetString("Label_BuildKit_gvSelect");

                Label lblHeaderPartNum = e.Row.FindControl("lblHeaderPartNum") as Label;
                lblHeaderPartNum.Text = vctResource.GetString("Label_BuildKit_gvPartNum");

                Label lblHeaderDesc = e.Row.FindControl("lblHeaderDesc") as Label;
                lblHeaderDesc.Text = vctResource.GetString("Label_BuildKit_gvDesc");

                Label lblHeaderLotNum = e.Row.FindControl("lblHeaderLotNum") as Label;
                lblHeaderLotNum.Text = vctResource.GetString("Label_BuildKit_gvLotNum");
            }
            else if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
            {
                DropDownList ddlLotNum = e.Row.FindControl("ddlLotNum") as DropDownList;
                                
                string PartNum = (e.Row.FindControl("lblPartNumber") as Label).Text;
                if (tmpPartNum != PartNum)
                {
                    tmpPartNum = PartNum;
                    tmpIndex = 0;
                }
                else
                {
                    tmpIndex += 1;
                }
                this.PartNum = PartNum;
                presenter.PopulateLotNumbers();
                ddlLotNum.DataSource = this.LotNumberList;
                ddlLotNum.DataTextField = "LotNum";
                ddlLotNum.DataValueField = "LocationPartDetailId";
                ddlLotNum.DataBind();

                ddlLotNum.Items.Insert(0, new ListItem("", "0"));
                if (this.LotNumberList.Count > 0 && (this.LotNumberList.Count - 1 >= tmpIndex))
                {
                    ddlLotNum.SelectedValue = this.LotNumberList[tmpIndex].LocationPartDetailId.ToString();
                }
                //HiddenField hdnLocationPartDetailId = e.Row.FindControl("hdnLocationPartDetailId") as HiddenField;
                //if (Convert.ToInt64(hdnLocationPartDetailId.Value) != 0)
                //{
                //    CheckBox chkStatus = e.Row.FindControl("chkStatus") as CheckBox;
                //    chkStatus.Checked = true;

                //    HiddenField hdnLotNum = e.Row.FindControl("hdnLotNum") as HiddenField;

                //    ddlLotNum.SelectedValue = hdnLocationPartDetailId.Value;
                //}

                
            }
        }        
        #endregion

        #region Event Handlers
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            GetKitNumber();

            presenter.PopulateVirtualBuildKitList();
            if (this.VirtualBuildKitList.Count == 0)
            {
                lblError.Text = vctResource.GetString("valInvalidKitNumber");
            }
            else
            {
                IList<long> lstSelectedLot = new List<long>();

                foreach (GridViewRow row in gdVirtualBuildKit.Rows)
                {
                    string partNum = (row.FindControl("lblPartNumber") as Label).Text;
                    int index = this.SelectedBuildKitList.FindIndex(x => x.PartNum == partNum && x.KitItemId == 0);

                    if (index != -1)
                    {                        
                        CheckBox chkStatus = row.FindControl("chkStatus") as CheckBox;
                        chkStatus.Checked = true;

                        Int64 SelectedVal = this.SelectedBuildKitList[index].LocationPartDetailId;
                        
                        DropDownList ddlLotNum = (DropDownList)row.FindControl("ddlLotNum");
                        ddlLotNum.SelectedValue = SelectedVal.ToString();
                        lstSelectedLot.Add(Convert.ToInt64(ddlLotNum.SelectedValue));

                        this.SelectedBuildKitList[index].KitItemId = 1;
                    }

                }                             

            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid && IsValidInput())
            {
                //Constants.ResultStatus resultStatus = presenter.SaveVirtualBuildKit();
                if (!presenter.SaveVirtualBuildKit())
                    lblError.Text = "Problem in assigning Part #. The selected Part(s) may not be available currently. Please select other Part(s) or try after refreshing the page.";                
                else                
                    this.lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgUpdated"), this.lblHeader.Text) + "</font>";

                //this.txtKitNumber.Focus();

                //if (resultStatus == Constants.ResultStatus.Created)
                //{
                //    this.txtKitNumber.Focus();
                //    this.lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgCreated"), this.lblHeader.Text) + "</font>";
                //}
                //else if (resultStatus == Constants.ResultStatus.Updated)
                //{
                   
                //    this.txtKitNumber.Focus();
                //    this.lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgUpdated"), this.lblHeader.Text) + "</font>";
                //}
                //presenter.OnViewInitialized();
                
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                this.lblError.Text = string.Empty;
                presenter.OnViewInitialized();
                this.txtKitNumber.Focus();
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

        protected void chkSelect_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;

            foreach (GridViewRow row in gdVirtualBuildKit.Rows)
            {
                CheckBox chkOpt = row.FindControl("chkStatus") as CheckBox;

                if (chk.Checked)
                    chkOpt.Checked = true;
                else
                    chkOpt.Checked = false;
            }

        }

   




    }
}

