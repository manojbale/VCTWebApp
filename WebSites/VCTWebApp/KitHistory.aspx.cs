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
using System.Linq;

namespace VCTWebApp.Shell.Views
{
    public partial class KitHistory : Microsoft.Practices.CompositeWeb.Web.UI.Page, IKitHistoryView
    {
        
        #region Instance Variables

        private KitHistoryPresenter presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private Helper helper = new Helper();
        private Security security = null;

        #endregion

        #region Create New Presenter

        [CreateNew]
        public KitHistoryPresenter Presenter
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
                    
                    this.DisplayMessageForMissingMasters();

                    this.txtKitNumber.Focus();
                    lblError.Text = string.Empty;

                    //if (Session["PendingBuildKitNumber"] != null && Session["PendingBuildKitNumber"].ToString() != string.Empty)
                    //{
                    //    this.txtKitNumber.Text = Session["PendingBuildKitNumber"].ToString() + "( " + Session["PendingBuildKitDescription"].ToString() + " )";
                    //    this.hdnKitNumber.Value = Session["PendingBuildKitNumber"].ToString();
                    //    Session["PendingBuildKitNumber"] = null;
                    //    btnSearch_Click(sender, e);
                    //}
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
            else if (security.HasAccess("KitHistory"))
            {
                CanView = security.HasPermission("KitHistory");
            }
            else
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }

        private void LocalizePage()
        {
            try
            {
                string heading = string.Empty;
                heading = vctResource.GetString("mnuKitHistory");

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
                
        #region IKitHistoryView Members
        public List<KitHistoryCaseDetail> KitHistoryCaseDetailList
        {            
            get
            {
                return (List<KitHistoryCaseDetail>)ViewState["KitHistoryCaseDetailList"];
            }
            set
            {
                ViewState["KitHistoryCaseDetailList"] = value;
                if (value != null && value.Count>0)
                {
                    var lst = value.Select(k => new { k.CaseNumber, k.CaseStatus, k.PartyName, k.ProcedureName, k.SalesRep, k.SurgeryDate }).Distinct();
                    this.gdvCaseDetail.DataSource = lst;
                    this.gdvCaseDetail.DataBind();
                    lblError.Text = string.Empty;
                    
                }
                else
                {
                    
                    this.gdvCaseDetail.DataSource = null;
                    this.gdvCaseDetail.DataBind();
                    lblError.Text="No Record Found.";
                    
                  
                }
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
        #endregion
        
        #region Protected Methods
        int tmpIndex = 0;
        string tmpPartNum = "";
        protected void gdvCaseDetail_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //Label lblHeaderPartNum = e.Row.FindControl("lblHeaderPartNum") as Label;
                //lblHeaderPartNum.Text = vctResource.GetString("Label_BuildKit_gvPartNum");

                //Label lblHeaderDesc = e.Row.FindControl("lblHeaderDesc") as Label;
                //lblHeaderDesc.Text = vctResource.GetString("Label_BuildKit_gvDesc");

                //Label lblHeaderLotNum = e.Row.FindControl("lblHeaderLotNum") as Label;
                //lblHeaderLotNum.Text = vctResource.GetString("Label_BuildKit_gvLotNum");
            }
            else if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
            {
                Label lblCaseNumber = e.Row.FindControl("lblCaseNumber") as Label;
                List<KitHistoryCaseDetail> lst = this.KitHistoryCaseDetailList.FindAll(k => k.CaseNumber == lblCaseNumber.Text);
                GridView grdChild = (GridView)e.Row.FindControl("grdChild");
                grdChild.DataSource = lst;
                grdChild.DataBind();
            }
        }        
        #endregion

        #region Event Handlers
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            if (string.IsNullOrEmpty(hdnValid.Value))
            {
                lblError.Text = vctResource.GetString("valInvalidKitNumber");
                this.gdvCaseDetail.DataSource = null;
                this.gdvCaseDetail.DataBind();
                return;
            }
            else
            {
                GetKitNumber();
                presenter.PopulateKitHistoryCaseDetailList();
            }
            
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                this.lblError.Text = string.Empty;
                txtKitNumber.Text = string.Empty;
                hdnKitNumber.Value = string.Empty;
                gdvCaseDetail.DataSource = null;
                gdvCaseDetail.DataBind();

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




    }
}

