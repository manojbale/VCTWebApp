using System;
using Microsoft.Practices.ObjectBuilder;
using System.Data;
using VCTWebApp.Web;
using VCTWebApp.Resources;
using VCTWeb.Core.Domain;
using System.Data.SqlClient;
using System.Globalization;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web;
using System.Text;

namespace VCTWebApp.Shell.Views
{
    public partial class ReturnInventoryRMA : Microsoft.Practices.CompositeWeb.Web.UI.Page, IReturnInventoryRMAView
    {
       
        #region Instance Variables

        private ReturnInventoryRMAPresenter presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private Helper helper = new Helper();
        private Security security = null;

        #endregion

        #region Create New Presenter

        [CreateNew]
        public ReturnInventoryRMAPresenter Presenter
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
                    GetFormControls();
                    gdvKit.DataSource = null;
                    gdvKit.DataBind();
                    this.LocalizePage();
                    this.Form.DefaultButton = this.btnSave.UniqueID; //Set the default button to search.
                    if (Convert.ToString(Session["LoggedInLocationType"]).ToUpper() == "AREA")
                    {
                        //radRTCorp.Enabled = false;
                        //radRTRegion.Enabled = false;
                        //ddlLocation.Enabled = false;    
                    }
                    else if (Convert.ToString(Session["LoggedInLocationType"]).ToUpper() == "REGION")
                    {
                        radRTRegion.Enabled = false;
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
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool ValidatePage()
        {
            if (!Page.IsValid)
            {
                lblError.Text = vctResource.GetString("msgCommonError");
                return false;
            }
            else
            {
                if (radRFHospital.Checked && ddlLocation.SelectedValue == "0")
                {
                    //lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valLeastKitItem"), this.lblHeader.Text);
                    lblError.Text = "please select party Location";
                    return false;
                }
                else if (radRFSelf.Checked && ddlLocation.SelectedValue == "0")
                {
                    //lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("valLeastKitItem"), this.lblHeader.Text);
                    lblError.Text = "please select Location";
                    return false;
                }

                if (radCase.Checked == true)
                {
                    if (string.IsNullOrEmpty(txtCaseNumber.Text.Trim()))
                    {
                        lblError.Text = "please enter Case Number";
                        return false;
                    }
                    if (this.ReturnInventoryKitList == null || this.ReturnInventoryKitList.Count == 0 || (this.ReturnInventoryKitList.Count > 0 && string.IsNullOrEmpty(this.ReturnInventoryKitList[0].PartNum)))
                    {
                        lblError.Text = "Please select Part # for return.";
                        return false;
                    }
                    else
                    {
                        int count = 0;
                        foreach (GridViewRow row in gdvKit.Rows)
                        {
                            CheckBox chkReturn = (row.FindControl("chkReturn") as CheckBox);
                            CheckBox chkSeekReturn = (row.FindControl("chkSeekReturn") as CheckBox);
                            string DispositionTypeId = (row.FindControl("lblDispositionTypeId") as TextBox).Text.Trim();

                            if (chkReturn.Checked && chkReturn.Enabled)
                            {
                                count += 1;
                            }

                            if (chkReturn.Checked && chkReturn.Enabled && (DispositionTypeId == "0" || string.IsNullOrEmpty(DispositionTypeId)))
                            {
                                lblError.Text = "Disposition type must be selected for selected Part. ";
                                return false;
                            }
                            else if (chkReturn.Checked == false && chkSeekReturn.Checked == true)
                            {
                                lblError.Text = "Please check return checkbox to return part #.";
                                return false;
                            }
                        }

                        if (count == 0)
                        {
                            lblError.Text = "Please select Part # for return.";
                            return false;
                        }
                    }
                }
                else
                {
                    if (this.ReturnInventoryPartList == null || this.ReturnInventoryPartList.Count == 0 || (this.ReturnInventoryPartList.Count > 0 && string.IsNullOrEmpty(this.ReturnInventoryPartList[0].PartNum)))
                    {
                        lblError.Text = "Please select Part # for return.";
                        return false;
                    }
                                     

                    int Count = 0;
                    foreach (GridViewRow row in gdvPart.Rows)
                    {
                        Count += 1;
                        //CheckBox chkReturn = (row.FindControl("chkReturn") as CheckBox);
                        string DispositionTypeId = (row.FindControl("lblDispositionTypeId") as TextBox).Text.Trim();

                        //if (chkReturn.Checked)
                        //{
                        //    count += 1;
                        //}
                        //if (chkReturn.Checked && DispositionTypeId == "0")
                        if (DispositionTypeId == "0")
                        {
                            lblError.Text = "Disposition type must be selected for selected Part. ";
                            return false;
                        }
                    }

                    if (Count == 0)
                    {
                        lblError.Text = "Please select Part # for return.";
                        return false;
                    }

                    List<VCTWeb.Core.Domain.ReturnInventoryRMA> lstRMAParts = this.ReturnInventoryPartList;

                    for (int i = 0; i < lstRMAParts.Count; i++)
                    {
                        Int64 LocationPartDetailId = lstRMAParts[i].LocationPartDetailId;
                        if (lstRMAParts.Exists(t => t.Index > i + 1 && t.LocationPartDetailId != 0 && t.LocationPartDetailId == LocationPartDetailId))
                        {
                            lblError.Text = "Please select unique Case Number for Disposition";
                            return false;
                        }
                    }

                }


                                
            }

            return true;
        }

        private string CreateTableXml()
        {            
            StringBuilder XmlTable = new StringBuilder();
            XmlTable.Append("<root>");
            int count = 0;

            Int64 LocationPartDetailId = 0, CasePartId = 0;
            string DispositionTypeId, DispositionRemarks, PartNum, LotNum, Quantity, TmpValue;
            CheckBox chkSeekReturn, chk;

            if (this.InventoryType.ToLower() == "case")
            {
                foreach (GridViewRow row in gdvKit.Rows)
                {
                    DispositionTypeId="";
                    DispositionRemarks = "";

                    chk = row.FindControl("chkReturn") as CheckBox;                    
                    DispositionTypeId =  (row.FindControl("lblDispositionTypeId") as TextBox).Text.Trim();
                    DispositionRemarks = (row.FindControl("lblDispositionRemarks") as TextBox).Text.Trim();
                    
                    if (chk.Checked && chk.Enabled && DispositionTypeId != "0")
                    {
                        count += 1;

                        CasePartId = Convert.ToInt64((row.FindControl("hdnCasePartId") as HiddenField).Value);
                        LocationPartDetailId = Convert.ToInt64((row.FindControl("hdnLocationPartDetailId") as HiddenField).Value);

                        chkSeekReturn = row.FindControl("chkSeekReturn") as CheckBox;

                        XmlTable.Append("<Cases>");
                        XmlTable.Append("<SeekReturn>"); XmlTable.Append(chkSeekReturn.Checked); XmlTable.Append("</SeekReturn>");
                        XmlTable.Append("<PartNum>"); XmlTable.Append(""); XmlTable.Append("</PartNum>");
                        XmlTable.Append("<LotNum>"); XmlTable.Append(""); XmlTable.Append("</LotNum>");
                        XmlTable.Append("<Quantity>"); XmlTable.Append("1"); XmlTable.Append("</Quantity>");
                        XmlTable.Append("<CasePartId>"); XmlTable.Append(CasePartId); XmlTable.Append("</CasePartId>");
                        XmlTable.Append("<LocationPartDetailId>"); XmlTable.Append(LocationPartDetailId); XmlTable.Append("</LocationPartDetailId>");
                        XmlTable.Append("<DispositionTypeId>"); XmlTable.Append(DispositionTypeId); XmlTable.Append("</DispositionTypeId>");
                        XmlTable.Append("<Remarks>"); XmlTable.Append(DispositionRemarks); XmlTable.Append("</Remarks>");
                        XmlTable.Append("</Cases>");
                    }
                }
            }
            else
            {
                foreach (GridViewRow row in gdvPart.Rows)
                {
                    //CheckBox chk = row.FindControl("chkReturn") as CheckBox;
                    DispositionTypeId = (row.FindControl("lblDispositionTypeId") as TextBox).Text;
                    DispositionRemarks = (row.FindControl("lblDispositionRemarks") as TextBox).Text;
                    //LPID = Convert.ToInt64((row.FindControl("ddlCaseNumber") as DropDownList).SelectedValue);
                    //if (chk.Checked && DispositionTypeId != "0")
                    //{
                    count += 1;

                    chkSeekReturn = row.FindControl("chkSeekReturn") as CheckBox;
                    PartNum = (row.FindControl("lblPartNum") as Label).Text;
                    LotNum = (row.FindControl("lblLotNum") as Label).Text;
                    //Quantity = (row.FindControl("lblQty") as Label).Text;
                    Quantity = "1";
                    
                    TmpValue = (row.FindControl("ddlCaseNumber") as DropDownList).SelectedValue;
                    if (TmpValue.Contains("_"))
                    {
                        string[] arr = TmpValue.Split('_');
                        CasePartId = Convert.ToInt64(arr[0]);
                        LocationPartDetailId = Convert.ToInt64(arr[1]);
                    }
                    else
                    {
                        CasePartId = 0;
                        LocationPartDetailId = 0;
                    }

                    XmlTable.Append("<Cases>");
                    XmlTable.Append("<SeekReturn>"); XmlTable.Append(chkSeekReturn.Checked); XmlTable.Append("</SeekReturn>");
                    XmlTable.Append("<PartNum>"); XmlTable.Append(PartNum); XmlTable.Append("</PartNum>");
                    XmlTable.Append("<LotNum>"); XmlTable.Append(LotNum); XmlTable.Append("</LotNum>");
                    XmlTable.Append("<Quantity>"); XmlTable.Append(Quantity); XmlTable.Append("</Quantity>");
                    XmlTable.Append("<CasePartId>"); XmlTable.Append(CasePartId); XmlTable.Append("</CasePartId>");
                    XmlTable.Append("<LocationPartDetailId>"); XmlTable.Append(LocationPartDetailId); XmlTable.Append("</LocationPartDetailId>");
                    XmlTable.Append("<DispositionTypeId>"); XmlTable.Append(DispositionTypeId); XmlTable.Append("</DispositionTypeId>");
                    XmlTable.Append("<Remarks>"); XmlTable.Append(DispositionRemarks); XmlTable.Append("</Remarks>");
                    XmlTable.Append("</Cases>");
                    //}
                }
            }

            XmlTable.Append("</root>");
            if (count == 0)
            {
                XmlTable.Clear();
            }

            return XmlTable.ToString();
        }
        
        private void ClearControls()
        {
            ddlLocation.SelectedValue = "0";
            txtCaseNumber.Text = string.Empty;
            txtPartNum.Text = string.Empty;
            txtQty.Text = string.Empty;
            txtNewDescription.Text = string.Empty;
            
            gdvKit.DataSource = null;
            gdvKit.DataBind();

            gdvPart.DataSource = null;
            gdvPart.DataBind();

            this.ReturnInventoryPartList.RemoveAll(t => t.PartNum != null);
            this.ReturnInventoryKitList.RemoveAll(t => t.PartNum != null);
        }
        
        #endregion

        #region IReturnInventoryRMA Members

        public List<VCTWeb.Core.Domain.ReturnInventoryRMA> ReturnInventoryKitList 
        {
            get
            {
                return (List<VCTWeb.Core.Domain.ReturnInventoryRMA>)ViewState["ReturnInventoryList"];
            }
            set
            {
                ViewState["ReturnInventoryList"] = value;
                gdvKit.DataSource = value;
                gdvKit.DataBind();

                if (value != null && value.Count > 0 && string.IsNullOrEmpty(value[0].PartNum))
                {
                    gdvKit.Rows[0].Visible = false;
                }
            }
        }

        public List<VCTWeb.Core.Domain.ReturnInventoryRMA> ReturnInventoryPartList
        {
            get
            {
                return (List<VCTWeb.Core.Domain.ReturnInventoryRMA>)ViewState["ReturnInventoryList"];
            }
            set
            {
                ViewState["ReturnInventoryList"] = value;

                gdvPart.DataSource = value;
                gdvPart.DataBind();

                if (value != null && value.Count > 0 && (string.IsNullOrEmpty(value[0].PartNum)))
                {
                    gdvPart.Rows[0].Visible = false;
                }
            }
        }

        public List<VCTWeb.Core.Domain.Party> PartyList
        {
            set
            {
                ddlLocation.DataSource = value;
                ddlLocation.DataTextField = "Name";
                ddlLocation.DataValueField = "PartyId";
                ddlLocation.DataBind();
                ddlLocation.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
        }

        public List<VCTWeb.Core.Domain.Region> RegionList
        {
            set
            {
                ddlLocation.DataSource = value;
                ddlLocation.DataTextField = "RegionName";
                ddlLocation.DataValueField = "RegionId";
                ddlLocation.DataBind();
                ddlLocation.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
        }

        public List<VCTWeb.Core.Domain.Corp> CorpList
        {
            set
            {
                ddlLocation.DataSource = value;
                ddlLocation.DataTextField = "LocationName";
                ddlLocation.DataValueField = "LocationId";
                ddlLocation.DataBind();
                ddlLocation.Items.Insert(0, new ListItem("-- Select --", "0"));                
            }
        }

        public string CaseNum 
        {
            get
            {
                return txtCaseNumber.Text.Trim();
            }
        }

        public string TableXml
        {
            get
            { 
                return (CreateTableXml());
            }
        }

        public string ReturnFrom
        {
            get
            {
                return ((radRFHospital.Checked) ? "Hospital" : "Location");
            }
        }

        public string InventoryType
        {
            get
            {
                return ((radCase.Checked) ? "Case" : Constants.InventoryType.Part.ToString());                
            }
        }

        public Int64 PartyId
        {
            get
            {
                return (Convert.ToInt64(ddlLocation.SelectedValue));
            }
        }

        public Int32 ToLocationId
        {
            get
            {
                return (Convert.ToInt32(ddlLocation.SelectedValue));
            }
        }

        public List<VCTWeb.Core.Domain.DispositionType> DispositionList
        {
            set
            {
                ddlDispositionType.DataSource = value;
                ddlDispositionType.DataTextField = "Disposition";
                ddlDispositionType.DataValueField = "DispositionTypeId";
                ddlDispositionType.DataBind();
                ddlDispositionType.Items.Insert(0, new ListItem(" -- Select -- ", "0"));
            }
        }

        #endregion
                
        #region Protected Methods

        protected void txtCaseNumber_TextChanged(object sender, EventArgs e)
        {
            presenter.OnViewLoaded();
        }

        protected void gdvKit_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            
            if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
            {
            
                int DispositionTypeId = int.Parse((e.Row.FindControl("hdnDispositionTypeId") as HiddenField).Value);

                if (DispositionTypeId > 0)
                {
                    CheckBox chkReturn = e.Row.FindControl("chkReturn") as CheckBox;
                    chkReturn.Checked = true;
                    chkReturn.Enabled = false;

                    bool SeekReturn = Convert.ToBoolean((e.Row.FindControl("hdnSeekReturn") as HiddenField).Value);
                    CheckBox chkSeekReturn = e.Row.FindControl("chkSeekReturn") as CheckBox;
                    chkSeekReturn.Checked = SeekReturn;
                    chkSeekReturn.Enabled = false;

                    (e.Row.FindControl("ImgBtnCancel") as ImageButton).Visible = false;

                }
               
            }
        }

        int index =0;        
        protected void gdvPart_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string PartNum = (e.Row.FindControl("lblPartNum") as Label).Text;
                string LotNum = (e.Row.FindControl("lblLotNum") as Label).Text;
                DropDownList ddlCaseNum = e.Row.FindControl("ddlCaseNumber") as DropDownList;

                if (!(string.IsNullOrEmpty(PartNum) || string.IsNullOrEmpty(LotNum)))
                {
                    Int64 PartyId = 0;
                    if (radRFHospital.Checked)
                    {
                        PartyId = Convert.ToInt64(ddlLocation.SelectedValue);
                    }
                    List<KeyValuePair<string, string>> lstCases = presenter.GetCasesListByPartAndLotNum(PartNum, LotNum, PartyId);
                    if (lstCases != null && lstCases.Count > 0)
                    {
                        ddlCaseNum.DataSource = lstCases;
                        ddlCaseNum.DataTextField = "Value";
                        ddlCaseNum.DataValueField = "Key";
                        ddlCaseNum.DataBind();
                    }
                }
                ddlCaseNum.Items.Insert(0, new ListItem("-- Select --", "0"));

                if (this.ReturnInventoryPartList != null && this.ReturnInventoryPartList.Count > 0 && this.ReturnInventoryPartList[index].CasePartId != 0)
                {
                    //ddlCaseNum.SelectedValue = this.ReturnInventoryPartList[index].LocationPartDetailId.ToString();
                    string SelectedCaseNum = this.ReturnInventoryPartList[index].CasePartId.ToString() + "_" + this.ReturnInventoryPartList[index].LocationPartDetailId.ToString();
                    ddlCaseNum.SelectedValue = SelectedCaseNum;
                }
                
                index += 1;

                //List<VCTWeb.Core.Domain.DispositionType> lstDispositionType;
                //if (ViewState["DispositionTypeList"] == null)
                //{
                //    HttpContext.Current.Items["DispositionTypeList"] = presenter.PopulateDispositionType();
                //}

                //lstDispositionType = (List<VCTWeb.Core.Domain.DispositionType>)HttpContext.Current.Items["DispositionTypeList"];

                //DropDownList ddlDispositionType = (e.Row.FindControl("ddlDispositionType") as DropDownList);
                //ddlDispositionType.DataSource = lstDispositionType;
                //ddlDispositionType.DataTextField = "Disposition";
                //ddlDispositionType.DataValueField = "DispositionTypeId";
                //ddlDispositionType.DataBind();
                //ddlDispositionType.Items.Insert(0, new ListItem(" -- Select -- ", "0"));
            }
           
        }
        
        protected void gdvPart_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "remove")
            {                
                //string[] partLotNum = e.CommandArgument.ToString().Split(',');
                //this.ReturnInventoryPartList.RemoveAll(t => t.PartNum == partLotNum[0] && t.LotNum == partLotNum[1]);
                //this.ReturnInventoryPartList = this.ReturnInventoryPartList;

                Int32 Index = int.Parse(e.CommandArgument.ToString());
                this.ReturnInventoryPartList.RemoveAll(t => t.Index == Index);
               
                List<VCTWeb.Core.Domain.ReturnInventoryRMA> lstRMAParts = this.ReturnInventoryPartList;
                for (int i = 1; i <= lstRMAParts.Count; i++)
                {
                    lstRMAParts[i - 1].Index = Index = i;                    
                }
                this.ReturnInventoryPartList = lstRMAParts;

            }
        }
         
        #endregion
    

        #region Event Handlers

        protected void ddlCaseNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPartDetails();
        }
        
        protected void radReturnFrom_CheckedChanged(object sender, EventArgs e)
        {
            GetFormControls();
        }

        protected void radReturnTo_CheckedChanged(object sender, EventArgs e)
        {
            GetFormControls();
        }
        
        private void GetFormControls()
        {
            if (radRFHospital.Checked)
            {
                pnlReturnTo.Visible = false;                
                lblToLocation.Text = "Hospital :";

                presenter.PopulateHospital();
            }
            else
            {
                pnlReturnTo.Visible = true;

                if (radRTCorp.Checked)
                {
                    lblToLocation.Text = "Corp :";
                    presenter.PopulateCorp();
                }
                else
                {
                    lblToLocation.Text = "Region :";
                    presenter.PopulateRegion();
                }
            }
            txtCaseNumber.Text = "";
            txtPartNum.Text = "";
            txtNewDescription.Text = "";
            hdnPartDesc.Value = "";
            txtQty.Text = "";

            gdvKit.DataSource = null;
            gdvKit.DataBind();

            gdvPart.DataSource = null;
            gdvPart.DataBind();
            
            lblError.Text = "";

           

            presenter.PopulateReturnInventoryList();
        }

        protected void radInventoryType_CheckedChanged(object sender, EventArgs e)
        {
            if (radCase.Checked)
            {
                pnlKit.Visible = true;
                pnlPart.Visible = false;

                rowCaseNum.Visible = true;
                tblPart.Visible = false;
                txtCaseNumber.Text = string.Empty;
            }
            else
            {
                pnlPart.Visible = true;
                pnlKit.Visible = false;

                rowCaseNum.Visible = false;
                tblPart.Visible = true;
                txtCaseNumber.Text = string.Empty;
            }
            
            txtCaseNumber.Text = "";
            txtPartNum.Text = "";
            txtNewDescription.Text = "";
            hdnPartDesc.Value = "";
            txtQty.Text = "";

            gdvKit.DataSource = null;
            gdvKit.DataBind();

            gdvPart.DataSource = null;
            gdvPart.DataBind();

            lblError.Text = "";



            presenter.PopulateReturnInventoryList();
        }

        protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(radRFHospital.Checked)            
                Session["RMALocationId"] = "Party_" + ddlLocation.SelectedValue + "_" + Convert.ToString(Session["LoggedInLocationId"]); 
            else
                Session["RMALocationId"] = "Location_" + Convert.ToString(Session["LoggedInLocationId"]) +"_"+ ddlLocation.SelectedValue; 
        }
        
        protected void ÏmgBtnAdd_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (ddlLocation.SelectedValue == "0")
            {
                if (radRFSelf.Checked)                
                    lblError.Text = "please select Location";                
                else                
                    lblError.Text = "please select Party";

                txtCaseNumber.Text = string.Empty;
                txtNewDescription.Text = string.Empty;
                hdnPartDesc.Value = string.Empty;
            }
            else
            {
                string LotNum = "", PartNum ="";
                //Int64 LocationPartDetailId = 0;
                if (!hdnPartDetail.Value.Contains("#"))
                {
                    lblError.Text = "Part # is missing";
                }
                else
                {
                    string[] arr = hdnPartDetail.Value.Split('#');                    
                    //LocationPartDetailId = Convert.ToInt64(hdnLocationPartDetailId.Value);
                    PartNum = hdnPartNum.Value;
                    LotNum = arr[1].Trim();
                }
                                
                this.ReturnInventoryPartList.RemoveAll(t => t.PartNum == null);

                if (string.IsNullOrEmpty(txtPartNum.Text.Trim()))
                {
                    lblError.Text = "Part # is missing";
                }
                else if (string.IsNullOrEmpty(hdnPartDesc.Value.Trim()))
                {
                    //lblError.Text = "Part Description is missing";
                    lblError.Text = "Please select valid part.";
                }
                else if (string.IsNullOrEmpty(txtQty.Text.Trim()))
                {
                    lblError.Text = "Part Quantity is missing";
                }
                else if (int.Parse(txtQty.Text) < 1)
                {
                    lblError.Text = "Part Quantity should be greater than zero";
                }
                //else if (this.ReturnInventoryPartList.Exists(t => t.PartNum == txtPartNum.Text.Trim() && t.LotNum == LotNum))
                //{
                //    lblError.Text = "Part # already exists";
                //}
                else
                {

                    //LoadPartDetails();
                    int index = this.ReturnInventoryPartList.Count + 1;
                    int TotalQty = int.Parse(txtQty.Text);

                    List<VCTWeb.Core.Domain.ReturnInventoryRMA> lstRMAParts = this.ReturnInventoryPartList;
                    for (int i = 1; i <= TotalQty; i++)
                    {
                        lstRMAParts.Add(new VCTWeb.Core.Domain.ReturnInventoryRMA()
                        {
                            Index =  index,
                            PartNum = txtPartNum.Text.Trim(),
                            Description = hdnPartDesc.Value.Trim(),
                            LotNum = LotNum,
                            Qty = 1
                            //LocationPartDetailId = LocationPartDetailId
                        });

                        index += 1;
                    }
                    this.ReturnInventoryPartList = lstRMAParts;

                    txtPartNum.Text = "";
                    txtNewDescription.Text = "";
                    hdnPartDesc.Value = "";
                    txtQty.Text = "";
                    lblError.Text = "";
                }

            }
        }

        protected void btnSaveRemarks_Click(object sender, EventArgs e)
        {
            LoadPartDetails();
        }

        private void LoadPartDetails()
        {
            index = 0;
            int i = 0;
            List<VCTWeb.Core.Domain.ReturnInventoryRMA> lstRMAParts = this.ReturnInventoryPartList;

            if (lstRMAParts.Count > 0)
            {
                bool SeekReturn;
                string TmpValue, DispositionTypeId, DispositionRemarks;
                Int64 LocationPartDetailId=0, CasePartId=0;

                foreach (GridViewRow row in gdvPart.Rows)
                {
                    CasePartId = 0;
                    LocationPartDetailId = 0;
                    //LocationPartDetailId = Convert.ToInt64((row.FindControl("ddlCaseNumber") as DropDownList).SelectedValue);
                    TmpValue = (row.FindControl("ddlCaseNumber") as DropDownList).SelectedValue;
                    if (TmpValue.Contains("_"))
                    {
                        string[] arr = TmpValue.Split('_');
                        CasePartId = Convert.ToInt64(arr[0]);
                        LocationPartDetailId = Convert.ToInt64(arr[1]);
                    }

                    DispositionTypeId = (row.FindControl("lblDispositionTypeId") as TextBox).Text;
                    DispositionRemarks = (row.FindControl("lblDispositionRemarks") as TextBox).Text;
                    SeekReturn = (row.FindControl("chkSeekReturn") as CheckBox).Checked;

                    lblError.Text = "";
                    if (string.IsNullOrEmpty(DispositionTypeId))
                    {
                        lstRMAParts[i].DispositionTypeId = 0;
                        lstRMAParts[i].DispositionType = "";
                    }
                    else
                    {
                        lstRMAParts[i].DispositionTypeId = int.Parse(DispositionTypeId);
                        lstRMAParts[i].DispositionType = DispositionRemarks;
                    }
                    lstRMAParts[i].SeekReturn = SeekReturn;
                    lstRMAParts[i].LocationPartDetailId = LocationPartDetailId;
                    lstRMAParts[i].CasePartId = CasePartId;
                    i += 1;

                }
            }

            this.ReturnInventoryPartList = lstRMAParts;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {            
            string result = string.Empty;
            try
            {
                this.lblError.Text = string.Empty;

                if (ValidatePage())
                {
                    if (!presenter.Save(out result))
                    {                        
                        lblError.Text = "Problem in Return Inventory Case for selected Part #. The selected Part(s) may be already Returned. Please try after refreshing the page.";                                    
                    }
                    else
                    {
                        ClearControls();
                        radRFHospital.Focus();
                        this.lblError.Text = "<font color='blue'>" + string.Format(CultureInfo.InvariantCulture, vctResource.GetString("msgCommonforSaved"), result, this.lblHeader.Text) + "</font>";
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
        
        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                this.lblError.Text = string.Empty;
                ClearControls();
                this.radRFHospital.Focus();
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
     
    }

}

