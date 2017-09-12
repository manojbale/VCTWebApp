using System;
using Microsoft.Practices.ObjectBuilder;
using System.Data;
using System.Data.SqlClient;
using VCTWebApp.Resources;
using VCTWeb.Core.Domain;
using VCTWebApp.Web;
using System.Globalization;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;
using System.Web;

namespace VCTWebApp.Shell.Views
{
    public partial class ViewCancelTransaction : Microsoft.Practices.CompositeWeb.Web.UI.Page, IViewCancelTransactionView
    {


        #region Instance Variables

        private ViewCancelTransactionPresenter presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private Helper helper = new Helper();
        private Security security = null;

        #endregion

        #region Create New Presenter
        [CreateNew]
        public ViewCancelTransactionPresenter Presenter
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
        private bool CanCancel
        {
            get
            {
                return ViewState[Common.CAN_DELETE] != null ? (bool)ViewState[Common.CAN_DELETE] : false;
            }
            set
            {
                ViewState[Common.CAN_DELETE] = value;
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
                    this.LocalizePage();
                    txtStartDate.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                    txtEndDate.Text = DateTime.Now.AddMonths(1).ToShortDateString();

                    this.presenter.OnViewInitialized();
                    this.PageIndex = 1;
                    presenter.PopulateCasesList(this.PageIndex, this.PageSize);

                    txtStartDate.Focus();
                    PopulatePager(TotalRecordCount, PageIndex);
                    ViewState["NextPageValue"] = null;

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

        #region IViewCancelTransactionView Members
        //public List<VCTWeb.Core.Domain.CaseType> CaseTypeList 
        //{
        //    get
        //    {
        //        return (List<VCTWeb.Core.Domain.CaseType>)ViewState["CaseTypeList"];
        //    }
        //    set
        //    {
        //        ViewState["CaseTypeList"] = value;
        //    }
        //}


        public int TotalRecordCount
        {
            get
            {
                return int.Parse(ViewState["TotalRecordCount"].ToString());
            }
            set
            {
                ViewState["TotalRecordCount"] = value;
            }
        }

        public List<VCTWeb.Core.Domain.ViewCancelTransaction> CasesOverAllList
        {
            set
            {
                //gdvSummary.DataSource = value;
                //gdvSummary.DataBind();

                //if (value.Count > 0 && value[0].CaseStatus == null)
                //{
                //    gdvSummary.Rows[0].Visible = false;
                //}
            }
        }

        public List<VCTWeb.Core.Domain.ViewCancelTransaction> CasesList
        {

            set
            {

             
                if (value.Count == 0 )
                {

                    lblError.Text = "No record found.";
                    TotalRecordCount = 0;

                    gdvRoutineCases.DataSource = value;
                    gdvRoutineCases.DataBind();

                    pnlPager.Visible = false;


                    //rptPager.DataSource = null;
                    //rptPager.DataBind();
                    //btnNext.Visible = false;
                    //btnPrevious.Visible = false;
                 

                }
                else
                {
                    lblError.Text = string.Empty;
                    gdvRoutineCases.PageSize = this.PageSize;
                    TotalRecordCount = value[0].TotalRecordCount;
                    gdvRoutineCases.DataSource = value;
                    gdvRoutineCases.DataBind();
                    pnlPager.Visible = true;
                   
                }
            }
        }

        public List<string> LocTypeList
        {
            get
            {
                return (List<string>)ViewState["LocTypeList"];
            }
            set
            {
                ViewState["LocTypeList"] = value;
            }
        }

        //public List<VCTWeb.Core.Domain.ViewCancelTransaction> ChildList
        //{
        //    get
        //    {
        //        return (List<VCTWeb.Core.Domain.ViewCancelTransaction>)ViewState["ChildCaseList"];
        //    }
        //    set
        //    {
        //        ViewState["ChildCaseList"] = value;
        //    }
        //}

        public Int64 CaseId
        {
            get
            {
                return (Int64)ViewState["CancelCaseId"];
            }
            set
            {
                ViewState["CancelCaseId"] = value;
            }
        }

        public DateTime StartDate
        {
            get
            {
                return Convert.ToDateTime(txtStartDate.Text + " 00:00:00");
            }
        }

        public DateTime EndDate
        {
            get
            {
                return Convert.ToDateTime(txtEndDate.Text + " 23:59:59");
            }
        }

        public Int32 LocationId
        {
            get
            {
                return Convert.ToInt32(Session["LoggedInLocationId"]);
            }
        }

        public String CaseStatus
        {
            get
            {
                return (String)ViewState["CaseStatus"];
            }
            set
            {
                ViewState["CaseStatus"] = value;
            }
        }

        public string CaseTypeSearch
        {
            get
            {
                if (Convert.ToString(ViewState["CastTypeSearch"]) != "")
                    return (string)ViewState["CastTypeSearch"];
                else
                    return "All";
            }
            set
            {
                ViewState["CastTypeSearch"] = value;
            }
        }

        public string CaseNumberSearch
        {
            get
            {
                return (Convert.ToString(ViewState["CaseNumber"]));
            }
            set
            {
                ViewState["CaseNumber"] = value;
            }
        }

        public string InvTypeSearch
        {
            get
            {
                if (Convert.ToString(ViewState["InvType"]) != "")
                    return (string)ViewState["InvType"];
                else
                    return "All";
            }
            set
            {
                ViewState["InvType"] = value;
            }
        }

        public string PartyNameSearch
        {
            get
            {
                return (Convert.ToString(ViewState["PartyName"]));
            }
            set
            {
                ViewState["PartyName"] = value;
            }
        }

        public string LocationTypeSearch
        {
            get
            {
                if (Convert.ToString(ViewState["LocType"]) != "")
                    return (string)ViewState["LocType"];
                else
                    return "All";
            }
            set
            {
                ViewState["LocType"] = value;
            }
        }

        public string CaseStatusSearch
        {
            get
            {
                if (Convert.ToString(ViewState["CaseStatus"]) != "")
                    return (string)ViewState["CaseStatus"];
                else
                    return "All";

            }
            set
            {
                ViewState["CaseStatus"] = value;

            }
        }

        public List<VCTWeb.Core.Domain.DispositionType> DispositionTypeList
        {
            set
            {
                ddlDispositionType.DataSource = value;
                ddlDispositionType.DataTextField = "Disposition";
                ddlDispositionType.DataValueField = "DispositionTypeId";
                ddlDispositionType.DataBind();

                ddlDispositionType.Items.Insert(0, new ListItem("-- Select --", "0"));
            }
        }

        public int DispositionTypeId
        {
            get
            {
                return Convert.ToInt32(hdnDispositionTypeId.Value);
            }
        }

        public string Remarks
        {
            get
            {
                return (hdnCancelRemarks.Value);
            }
        }

        public int PageSize
        {
            set
            {
                ViewState["PageSize"] = value;
            }
            get
            {
                return (int)ViewState["PageSize"];
            }

        }
        public int PageIndex
        {
            set
            {
                if (Convert.ToString(ViewState["PageIndex"]) != "")
                {
                    ViewState["PageIndex"] = 1;
                }
                else
                {
                    ViewState["PageIndex"] = value;
                }
            }
            get
            {
                return (int)ViewState["PageIndex"];
            }
        }
        #endregion

        #region Event Handlers
        //protected void btnSearch_Click(object sender, EventArgs e)
        //{
        //    this.CaseStatus = "New";
        //    LoadCasesList();
        //}

        //protected void ddlDispositionType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //if (ddlDispositionType.SelectedValue == "--Select--")
        //    //{
        //        this.presenter.OnViewInitialized();

        //    //    lblDetailHeader.Text = "";
        //    //    gdvRoutineCases.DataSource = null;
        //    //    gdvRoutineCases.DataBind();

        //    //}
        //    //else
        //    //{
        //    //    this.CaseStatus = "New";
        //    //    LoadCasesList();
        //    //}
        //}

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (IsValidPage())
            {
                this.PageIndex = 1;
                ViewState["NextPageValue"] = null;
                PopulateGrid();
                this.PopulatePager(TotalRecordCount, this.PageIndex);
            }

        }
        #endregion

        #region Protected Method
        //protected void gdvRoutineCases_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    GridPaging(gdvRoutineCases, e);            
        //}

        protected void gdvRoutineCases_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CancelTransaction")
            {
                this.CaseId = Convert.ToInt64(e.CommandArgument);
                if (!presenter.CaseTransactionCancel())
                    lblError.Text = "Problem in Cancelling for selected Case. The selected Case may be already Cancelled. Please try after refreshing the page.";
                else
                    lblError.Text = "";

                PopulateGrid();
            }
            else if (e.CommandName == "Invoice")
            {
                this.CaseId = Convert.ToInt64(e.CommandArgument);
                //Session["InvoiceAdvisoryCaseId"] = this.CaseId.ToString();
                //string str = "<script type='text/javascript'>window.open('" + "InvoiceAdvisory.aspx" + "','_blank','top=0,left=0,width=1000,height=700');</script>";
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Invoice Advisory", str);
                //Response.Redirect("InvoiceAdvisory.aspx", true);


            }
        }

        protected void gdvRoutineCases_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                LocalizeGridRow(e);

                DropDownList ddlCaseType = e.Row.FindControl("ddlCaseType") as DropDownList;
                ddlCaseType.DataSource = presenter.PopulateDispositionTypes();
                ddlCaseType.DataTextField = "CaseTypeName";
                ddlCaseType.DataValueField = "CaseTypeName";
                ddlCaseType.DataBind();
                ddlCaseType.Items.Insert(0, new ListItem("All", "All"));

                DropDownList ddlLocType = e.Row.FindControl("ddlLocType") as DropDownList;

                // ddlLocType.DataSource = presenter.PopulateLocationType();
                ddlLocType.DataSource = presenter.PopulateLocationAndPartyType();
                ddlLocType.DataBind();
                ddlLocType.Items.Insert(0, "All");
                // var LocType = this.CasesList.Where(t=> t.CaseId != 0).Select(t => t.LocationType).Distinct();

                //if (this.LocTypeList != null)
                //{
                //ddlLocType.DataSource = this.LocTypeList;
                //ddlLocType.DataBind();
                //}
                //ddlLocType.Items.Insert(0, "All");
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string CaseStatus = (e.Row.FindControl("hdnCaseStatus") as HiddenField).Value;
                Image imgCaseStatus = e.Row.FindControl("imgCaseStatus") as Image;
                string InvType = (e.Row.FindControl("lblInventoryType") as Label).Text;

                String CaseType = (e.Row.FindControl("lblCaseType") as Label).Text;
                Int64? PartyId = Convert.ToInt64((e.Row.FindControl("hdnPartyId") as HiddenField).Value);
                String PartyName = (e.Row.FindControl("hdnPartyName") as HiddenField).Value;
                Int32 LocationId = Convert.ToInt32((e.Row.FindControl("hdnLocationId") as HiddenField).Value);
                String FrLocName = (e.Row.FindControl("hdnFrLocName") as HiddenField).Value;
                Int32 ToLocationId = Convert.ToInt32((e.Row.FindControl("hdnToLocationId") as HiddenField).Value);
                String ToLocName = (e.Row.FindControl("hdnToLocName") as HiddenField).Value;
                String LTParty = (e.Row.FindControl("hdnLTParty") as HiddenField).Value;
                String FromLTName = (e.Row.FindControl("hdnFromLTName") as HiddenField).Value;
                String ToLTName = (e.Row.FindControl("hdnToLTName") as HiddenField).Value;

                if (CaseType == "InventoryTransfer")
                {
                    if (Convert.ToInt32(Session["LoggedInLocationId"]) == LocationId)
                    {
                        if (PartyId.HasValue && PartyId > 0)
                        {
                            (e.Row.FindControl("lblFromToType") as Label).Text = "From";
                            (e.Row.FindControl("lblLocationName") as Label).Text = PartyName;
                            (e.Row.FindControl("lblLocationType") as Label).Text = LTParty;
                        }
                        else
                        {
                            (e.Row.FindControl("lblFromToType") as Label).Text = "To";
                            (e.Row.FindControl("lblLocationName") as Label).Text = ToLocName;
                            (e.Row.FindControl("lblLocationType") as Label).Text = ToLTName;
                        }
                    }
                    else if (Convert.ToInt32(Session["LoggedInLocationId"]) == ToLocationId)
                    {
                        (e.Row.FindControl("lblFromToType") as Label).Text = "From";

                        (e.Row.FindControl("lblLocationName") as Label).Text = FrLocName;
                        (e.Row.FindControl("lblLocationType") as Label).Text = FromLTName;
                    }
                    else
                    {
                        (e.Row.FindControl("lblFromToType") as Label).Text = "From / To";
                        (e.Row.FindControl("lblLocationName") as Label).Text = FrLocName + " - " + ToLocName;
                        (e.Row.FindControl("lblLocationType") as Label).Text = FromLTName;
                    }
                }
                else if (CaseType == "ReturnInventoryRMATransfer")
                {
                    if (Convert.ToInt32(Session["LoggedInLocationId"]) == LocationId)
                    {
                        if (PartyId.HasValue && PartyId > 0)
                        {
                            (e.Row.FindControl("lblFromToType") as Label).Text = "From";
                            (e.Row.FindControl("lblLocationName") as Label).Text = PartyName;
                            (e.Row.FindControl("lblLocationType") as Label).Text = LTParty;
                        }
                        else
                        {
                            (e.Row.FindControl("lblFromToType") as Label).Text = "To";
                            (e.Row.FindControl("lblLocationName") as Label).Text = ToLocName;
                            (e.Row.FindControl("lblLocationType") as Label).Text = ToLTName;
                        }
                    }
                    else if (Convert.ToInt32(Session["LoggedInLocationId"]) == ToLocationId)
                    {
                        (e.Row.FindControl("lblFromToType") as Label).Text = "From";

                        (e.Row.FindControl("lblLocationName") as Label).Text = FrLocName;
                        (e.Row.FindControl("lblLocationType") as Label).Text = FromLTName;
                    }

                }
                else
                {
                    if (Convert.ToInt32(Session["LoggedInLocationId"]) == LocationId)
                    {
                        (e.Row.FindControl("lblFromToType") as Label).Text = "To";
                        if (PartyId.HasValue && PartyId > 0)
                        {
                            (e.Row.FindControl("lblLocationName") as Label).Text = PartyName;
                            (e.Row.FindControl("lblLocationType") as Label).Text = LTParty;
                        }
                        else
                        {
                            (e.Row.FindControl("lblLocationName") as Label).Text = ToLocName;
                            (e.Row.FindControl("lblLocationType") as Label).Text = ToLTName;
                        }
                    }
                    else if (Convert.ToInt32(Session["LoggedInLocationId"]) == ToLocationId)
                    {
                        (e.Row.FindControl("lblFromToType") as Label).Text = "From";

                        //if (PartyId.HasValue && PartyId > 0)
                        //{
                        //    (e.Row.FindControl("lblLocationName") as Label).Text = PartyName;
                        //    (e.Row.FindControl("lblLocationType") as Label).Text = LTParty;
                        //}
                        //else
                        //{
                        (e.Row.FindControl("lblLocationName") as Label).Text = FrLocName;
                        (e.Row.FindControl("lblLocationType") as Label).Text = FromLTName;
                        //}
                    }
                }



                imgCaseStatus.ImageUrl = "~/Images/CaseStatus/" + CaseStatus + ".png";

                PopulateChildGridRow(e);
                PopulateInvoiceLink(e);
                DisableGridCancelRow(e);
            }

        }

        protected void grdChildKit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //LocalizeGridRow(e);                
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView gdvKitDetail = e.Row.FindControl("grdChildKitDetail") as GridView;
                if (gdvKitDetail != null)
                {
                    Int64 BuildKitId = Convert.ToInt64((e.Row.FindControl("hdnBuildKitId") as HiddenField).Value);
                    if (BuildKitId != 0)
                    {
                        Int64 CaseId = Convert.ToInt64((e.Row.FindControl("hdnCaseId") as HiddenField).Value);
                        string CaseStatus = (e.Row.FindControl("hdnCaseStatus") as HiddenField).Value.ToUpper();

                        if (CaseStatus == "INVENTORYASSIGNED")
                            gdvKitDetail.DataSource = presenter.PopulateKitItems(BuildKitId);
                        else
                            gdvKitDetail.DataSource = presenter.PopulateKitItems(BuildKitId, CaseId);

                        gdvKitDetail.DataBind();
                    }
                }

            }

        }

        protected void grdChild_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                bool IsNearExpiry = Convert.ToBoolean((e.Row.FindControl("hdnIsNearExpiry") as HiddenField).Value);
                if (IsNearExpiry)
                {
                    e.Row.ForeColor = System.Drawing.Color.Red;
                }
                e.Row.EnableViewState = false;
            }

        }

        protected void grdChildKitDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                bool IsNearExpiry = Convert.ToBoolean((e.Row.FindControl("hdnIsNearExpiry") as HiddenField).Value);
                if (IsNearExpiry)
                {
                    e.Row.ForeColor = System.Drawing.Color.Red;
                }
                e.Row.EnableViewState = false;
            }

        }

        protected void gdvRoutineCases_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gdvRoutineCases.PageIndex = e.NewPageIndex;
            PopulateGrid();
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["NextPageValue"] = null;
                this.lblError.Text = string.Empty;
                txtStartDate.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                txtEndDate.Text = DateTime.Now.AddMonths(1).ToShortDateString();

                this.presenter.OnViewInitialized();
                PageIndex=1;
                txtStartDate.Focus();


                this.CaseTypeSearch = (gdvRoutineCases.HeaderRow.FindControl("ddlInvType") as DropDownList).Items[0].Value;
                this.CaseNumberSearch = string.Empty;
                this.InvTypeSearch = (gdvRoutineCases.HeaderRow.FindControl("ddlInvType") as DropDownList).Items[0].Value;
                this.PartyNameSearch = string.Empty;
                this.LocationTypeSearch = (gdvRoutineCases.HeaderRow.FindControl("ddlLocType") as DropDownList).Items[0].Value;
                this.CaseStatusSearch = (gdvRoutineCases.HeaderRow.FindControl("ddlCaseStatus") as DropDownList).Items[0].Value;

                presenter.PopulateCasesList(int.Parse(ViewState["PageIndex"].ToString()), int.Parse(ViewState["PageSize"].ToString()));

                (gdvRoutineCases.HeaderRow.FindControl("ddlCaseType") as DropDownList).SelectedValue = this.CaseTypeSearch;
                (gdvRoutineCases.HeaderRow.FindControl("txtCaseNumber") as TextBox).Text = this.CaseNumberSearch;
                (gdvRoutineCases.HeaderRow.FindControl("ddlInvType") as DropDownList).SelectedValue = this.InvTypeSearch;
                (gdvRoutineCases.HeaderRow.FindControl("txtPartyName") as TextBox).Text = this.PartyNameSearch;
                (gdvRoutineCases.HeaderRow.FindControl("ddlLocType") as DropDownList).SelectedValue = this.LocationTypeSearch;
                (gdvRoutineCases.HeaderRow.FindControl("ddlCaseStatus") as DropDownList).SelectedValue = this.CaseStatusSearch;
                this.PopulatePager(int.Parse(ViewState["TotalRecordCount"].ToString()), this.PageIndex);



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
            else if (security.HasAccess("ViewCancelTransaction"))
            {
                CanCancel = security.HasPermission("ViewCancelTransaction.Cancel");
            }
            else
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }

        private void LocalizePage()
        {
            try
            {
                string heading = string.Empty;
                heading = vctResource.GetString("ViewCancelTransaction");
                lblHeader.Text = heading;
                Page.Title = heading;

                String errorMessage = vctResource.GetString("required");
                //rfvDispositionType.ErrorMessage = errorMessage;
                rfvStartDate.ErrorMessage = errorMessage;
                rfvEndDate.ErrorMessage = errorMessage;

                //lblDispositionType.Text = vctResource.GetString("ViewCancelTrans_lblDispositionType");
                lblStartDate.Text = vctResource.GetString("ViewCancelTrans_lblStartDate");
                lblEndDate.Text = vctResource.GetString("ViewCancelTrans_lblEndDate");


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LocalizeGridRow(GridViewRowEventArgs e)
        {
            (e.Row.FindControl("lblCaseTypeHeader") as Label).Text = vctResource.GetString("ViewCancelTrans_Header_lblCaseTypeHeader");
            (e.Row.FindControl("lblCaseNumberHeader") as Label).Text = vctResource.GetString("ViewCancelTrans_Header_lblCaseNumberHeader");
            (e.Row.FindControl("lblInventoryTypeHeader") as Label).Text = vctResource.GetString("ViewCancelTrans_Header_lblInventoryTypeHeader");
            (e.Row.FindControl("lblSurgeryDateHeader") as Label).Text = vctResource.GetString("ViewCancelTrans_Header_lblSurgeryDateHeader");
            (e.Row.FindControl("lblPartyNameHeader") as Label).Text = vctResource.GetString("ViewCancelTrans_Header_lblToLocationHeader");
            (e.Row.FindControl("lblLocationTypeHeader") as Label).Text = vctResource.GetString("ViewCancelTrans_Header_lblLocationTypeHeader");
            (e.Row.FindControl("lblCaseStatusHeader") as Label).Text = vctResource.GetString("ViewCancelTrans_Header_lblCaseStatusHeader");
            (e.Row.FindControl("lblInvoiceHeader") as Label).Text = vctResource.GetString("ViewCancelTrans_Header_lblInvoiceHeader");
            (e.Row.FindControl("lblActionHeader") as Label).Text = vctResource.GetString("ViewCancelTrans_Header_lblActionHeader");

        }

        private void DisableGridCancelRow(GridViewRowEventArgs e)
        {
            ImageButton lnkCancel = (ImageButton)e.Row.FindControl("lnkCancel");
            //LinkButton lnkCancel = (LinkButton)e.Row.FindControl("lnkCancel");
            HiddenField hdnButtonStatus = e.Row.FindControl("hdnButtonStatus") as HiddenField;
            string CaseStatus = Convert.ToString(System.Web.UI.DataBinder.Eval(e.Row.DataItem, "CaseStatus"));
            string DispositionType = (e.Row.FindControl("hdnDispositionType") as HiddenField).Value;
            string Remarks = (e.Row.FindControl("hdnRemarks") as HiddenField).Value;
            string CaseType = (e.Row.FindControl("lblCaseType") as Label).Text;
            string RowType = (e.Row.FindControl("hdnRowType") as HiddenField).Value;

            if (CaseStatus == "Cancelled")
            {
                lnkCancel.ImageUrl = "~/images/message.png";
                lnkCancel.Enabled = false;
                lnkCancel.ToolTip = DispositionType + " \n " + Remarks;
            }
            else
            {
                lnkCancel.ImageUrl = "~/images/delete2.png";
                //lnkCancel.ImageUrl = "~/images/Cancel.gif";                
                Int32 LocationId = Convert.ToInt32((e.Row.FindControl("hdnLocationId") as HiddenField).Value);

                if ((CaseType == "ReturnInventoryRMATransfer" && CaseStatus == "Shipped") || ((RowType.ToLower() == "parent") && (CaseStatus == "New" || CaseStatus == "InventoryAssigned")))
                {
                    lnkCancel.Visible = true;
                }
                else
                {
                    if (LocationId != Convert.ToInt32(Session["LoggedInLocationId"]) || !this.CanCancel || !(CaseStatus == "New" || CaseStatus == "InventoryAssigned"))
                    {
                        lnkCancel.Visible = false;
                    }
                    else
                    {
                        lnkCancel.Visible = true;
                    }
                }
            }

        }

        private void PopulateChildGridRow(GridViewRowEventArgs e)
        {
            string InventoryType = (e.Row.FindControl("hdnInventoryType") as HiddenField).Value;
            if (InventoryType.ToUpper() == "PART")
            {
                GridView grdChild = (GridView)e.Row.FindControl("grdChild");
                Int64 caseId = Convert.ToInt64((e.Row.FindControl("hdnCaseId") as HiddenField).Value);

                grdChild.DataSource = presenter.PopulateCaseItems("Part", caseId);
                grdChild.DataBind();

            }
            else if (InventoryType.ToUpper() == "KIT")
            {
                GridView grdChildKit = (GridView)e.Row.FindControl("grdChildKit");
                Int64 caseId = Convert.ToInt64((e.Row.FindControl("hdnCaseId") as HiddenField).Value);

                grdChildKit.DataSource = presenter.PopulateCaseItems("Kit", caseId);
                grdChildKit.DataBind();
            }

        }

        private void PopulateInvoiceLink(GridViewRowEventArgs e)
        {
            VCTWeb.Core.Domain.ViewCancelTransaction obj = (VCTWeb.Core.Domain.ViewCancelTransaction)e.Row.DataItem;

            if (obj != null)
            {
                if (obj.PartyId > 0 && ((obj.InventoryType == Constants.InventoryType.Kit.ToString() && obj.CaseStatus == Constants.CaseStatus.CheckedIn.ToString()) || (obj.InventoryType == Constants.InventoryType.Part.ToString() && (CaseStatus == Constants.CaseStatus.Shipped.ToString() || obj.CaseStatus == Constants.CaseStatus.Delivered.ToString()))))
                {
                    (e.Row.FindControl("ImgBtnInvoice") as ImageButton).Visible = true;
                    string fullURL = "window.open('InvoiceAdvisory.aspx?CaseId=" + obj.CaseId.ToString() + "', '_blank', 'top=0,left=0,width=1150,height=700');return false;";
                    (e.Row.FindControl("ImgBtnInvoice") as ImageButton).Attributes.Add("OnClick", fullURL);
                }
            }
        }

        private bool IsValidPage()
        {
            //if (ddlDispositionType.SelectedValue == "--Select--")
            //{
            //    lblError.Text = vctResource.GetString("valDispositionType");
            //    ddlDispositionType.Focus();
            //    return false;    
            //}
            //else 
            if (Page.IsValid)
            {
                //if (string.IsNullOrEmpty(txtStartDate.Text))
                //{
                //    lblError.Text = vctResource.GetString("valStartDate");
                //    txtStartDate.Focus();
                //    return false;
                //}
                if (string.IsNullOrEmpty(txtEndDate.Text))
                {
                    lblError.Text = vctResource.GetString("valEndDate");
                    txtEndDate.Focus();
                    return false;
                }

                DateTime startDate = Convert.ToDateTime(txtStartDate.Text);
                DateTime endDate = Convert.ToDateTime(txtEndDate.Text);

                if (startDate > endDate)
                {
                    lblError.Text = vctResource.GetString("ViewCancelTrans_InvalidDate");
                    txtStartDate.Focus();
                    return false;
                }
            }

            return true;
        }

        private void PopulateGrid()
        {
            this.CaseTypeSearch = (gdvRoutineCases.HeaderRow.FindControl("ddlCaseType") as DropDownList).SelectedValue;
            this.CaseNumberSearch = (gdvRoutineCases.HeaderRow.FindControl("txtCaseNumber") as TextBox).Text;
            this.InvTypeSearch = (gdvRoutineCases.HeaderRow.FindControl("ddlInvType") as DropDownList).SelectedValue;
            this.PartyNameSearch = (gdvRoutineCases.HeaderRow.FindControl("txtPartyName") as TextBox).Text;
            this.LocationTypeSearch = (gdvRoutineCases.HeaderRow.FindControl("ddlLocType") as DropDownList).SelectedValue;
            this.CaseStatusSearch = (gdvRoutineCases.HeaderRow.FindControl("ddlCaseStatus") as DropDownList).SelectedValue;

            presenter.PopulateCasesList(int.Parse(ViewState["PageIndex"].ToString()), int.Parse(ViewState["PageSize"].ToString()));

            (gdvRoutineCases.HeaderRow.FindControl("ddlCaseType") as DropDownList).SelectedValue = this.CaseTypeSearch;
            (gdvRoutineCases.HeaderRow.FindControl("txtCaseNumber") as TextBox).Text = this.CaseNumberSearch;
            (gdvRoutineCases.HeaderRow.FindControl("ddlInvType") as DropDownList).SelectedValue = this.InvTypeSearch;
            (gdvRoutineCases.HeaderRow.FindControl("txtPartyName") as TextBox).Text = this.PartyNameSearch;
            (gdvRoutineCases.HeaderRow.FindControl("ddlLocType") as DropDownList).SelectedValue = this.LocationTypeSearch;
            (gdvRoutineCases.HeaderRow.FindControl("ddlCaseStatus") as DropDownList).SelectedValue = this.CaseStatusSearch;

        }

        public int TotalPageCount
        {
            get
            {
                return int.Parse(ViewState["TotalPageCount"].ToString());
            }
            set { TotalPageCount = value; }
        }

        private void PopulatePager(int recordCount, int currentPage)
        {

            double dblPageCount = (double)((decimal)recordCount / decimal.Parse(PageSize.ToString()));
            int pageCount = (int)Math.Ceiling(dblPageCount);
            ViewState["TotalPageCount"] = pageCount;
            ViewState["RemainingPageCount"] = pageCount;
            List<ListItem> pages = new List<ListItem>();
            if (pageCount >= 10)
            {

                for (int i = 1; i <= 10; i++)
                {
                    pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                }

            }
            else
            {
                for (int i = 1; i <= pageCount; i++)
                {
                    pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                }
            }

            rptPager.DataSource = pages;
            rptPager.DataBind();

            btnPrevious.Visible = false;
            if (pageCount > 10)
                btnNext.Visible = true;
            else
                btnNext.Visible = false;
        }

        protected void Page_Changed(object sender, EventArgs e)
        {
            int iPageIndex = int.Parse((sender as LinkButton).CommandArgument);
            ViewState["PageIndex"] = iPageIndex;
            PopulateGrid();

            if (rptPager.Items.Count > 0)
            {
                for (int count = 0; count < rptPager.Items.Count; count++)
                {
                    LinkButton lnkPagerLink = (LinkButton)rptPager.Items[count].FindControl("lnkPage");
                    if (lnkPagerLink.Text == iPageIndex.ToString())
                    {
                        lnkPagerLink.Enabled = false;
                        lnkPagerLink.CssClass = "ClickedPage";
                    }
                    else
                    {
                        lnkPagerLink.Enabled = true;
                        lnkPagerLink.CssClass = "NotClickedPage";
                        
                    }
                }
            }
          
            
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            int ConstantPageToDisplay = 10;

            //int remainingPages = int.Parse(ViewState["RemainingPageCount"].ToString()) - 10;
            int remainingPages = int.Parse(ViewState["RemainingPageCount"].ToString()) - ConstantPageToDisplay;
            ViewState["RemainingPageCount"] = remainingPages;


            List<ListItem> pages = new List<ListItem>();
            if (remainingPages >= ConstantPageToDisplay)
            {
                if (ViewState["NextPageValue"] == null)
                {
                    for (int i = ConstantPageToDisplay+1; i <= ConstantPageToDisplay + 10; i++)
                    {
                        pages.Add(new ListItem(i.ToString(), i.ToString(), true));
                    }
                    rptPager.DataSource = pages;
                    rptPager.DataBind();
                    btnNext.Visible = true;
                    btnPrevious.Visible = true;
                    ViewState["NextPageValue"] = 20;
                   
                }
                else
                {


                    for (int i = int.Parse(ViewState["NextPageValue"].ToString()) + 1; i <= int.Parse(ViewState["NextPageValue"].ToString()) + ConstantPageToDisplay; i++)
                    {
                        pages.Add(new ListItem(i.ToString(), i.ToString(), true));
                    }
                    rptPager.DataSource = pages;
                    rptPager.DataBind();
                    ViewState["NextPageValue"] = int.Parse(ViewState["NextPageValue"].ToString()) + ConstantPageToDisplay;

                    if (int.Parse(ViewState["NextPageValue"].ToString()) < int.Parse(ViewState["TotalPageCount"].ToString()))
                    {
                        btnNext.Visible = true;
                        btnPrevious.Visible = true;
                    }
                    else
                    {
                        btnNext.Visible = false;
                        btnPrevious.Visible = true;
                    }                 


                }
            }
            else
            {
                for (int i = int.Parse(ViewState["TotalPageCount"].ToString()) - ConstantPageToDisplay; i <= int.Parse(ViewState["TotalPageCount"].ToString()); i++)
                {
                    pages.Add(new ListItem(i.ToString(), i.ToString(), true));
                }
                rptPager.DataSource = pages;
                rptPager.DataBind();
                btnNext.Visible = false;
                btnPrevious.Visible = true;
                if (ViewState["NextPageValue"] != null)
                {
                    ViewState["NextPageValue"] = int.Parse(ViewState["NextPageValue"].ToString()) + ConstantPageToDisplay;
                }


            }
         
        }


        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            List<ListItem> pages = new List<ListItem>();
            int remainingPages = int.Parse(ViewState["RemainingPageCount"].ToString())+ 10;
            ViewState["RemainingPageCount"] = remainingPages;


            if (int.Parse(ViewState["TotalPageCount"].ToString()) - 10 < 10)
            {
                for (int i = 1; i <= 10; i++)
                {
                    pages.Add(new ListItem(i.ToString(), i.ToString(), true));
                }
                rptPager.DataSource = pages;
                rptPager.DataBind();              
                btnNext.Visible = true;
                btnPrevious.Visible = false;
            }

            else if (int.Parse(ViewState["TotalPageCount"].ToString()) - 10 >= 10)
            {

                if (int.Parse(ViewState["NextPageValue"].ToString()) == 20)
                {
                    for (int i = 1; i <= int.Parse(ViewState["NextPageValue"].ToString()) - 10; i++)
                    {
                        pages.Add(new ListItem(i.ToString(), i.ToString(), true));
                    }
                    rptPager.DataSource = pages;
                    rptPager.DataBind();
                    ViewState["NextPageValue"] = int.Parse(ViewState["NextPageValue"].ToString()) - 10;
                    btnNext.Visible = true;
                    btnPrevious.Visible = false;
                }
                else
                {
                    for (int i = int.Parse(ViewState["NextPageValue"].ToString()) - 19; i <= int.Parse(ViewState["NextPageValue"].ToString()) - 10; i++)
                    {
                        pages.Add(new ListItem(i.ToString(), i.ToString(), true));
                    }

                    rptPager.DataSource = pages;
                    rptPager.DataBind();
                    ViewState["NextPageValue"] = int.Parse(ViewState["NextPageValue"].ToString()) - 10;

                    if (int.Parse(ViewState["NextPageValue"].ToString()) < int.Parse(ViewState["TotalPageCount"].ToString()))
                    {
                        btnNext.Visible = true;
                        btnPrevious.Visible = true;
                    }
                    else
                    {
                        btnNext.Visible = false;
                        btnPrevious.Visible = true;
                    }

                }
            }
            
        }

        #endregion

    }

}