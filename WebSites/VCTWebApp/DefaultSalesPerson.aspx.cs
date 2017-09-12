using System;
using Microsoft.Practices.ObjectBuilder;
using VCTWebApp.Resources;
using VCTWeb.Core.Domain;
using VCTWebApp.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using VCTWebApp.Shell.Views;
using VCTWebApp.Shell;

namespace VCTWebApp
{
    public partial class DefaultSalesPerson : Microsoft.Practices.CompositeWeb.Web.UI.Page, IDefaultSalesPersonView
    {
        #region Instance Variables

        private DefaultSalesPersonPresenter presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private Helper helper = new Helper();
        private Security security = null;

        #endregion

        #region Init/Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.AuthorizedPage();
                LocalizePage();                
                this.presenter.OnViewInitialized();              
            }      
        }
        #endregion

        #region Create New Presenter
        [CreateNew]
        public DefaultSalesPersonPresenter Presenter
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

        #region IDefaultSalesPerson Members
        public List<VCTWeb.Core.Domain.ViewCancelTransaction> lstPendingCases 
        {
            set
            {
                gdvCases.DataSource = value;
                gdvCases.DataBind();

                if (value != null && value.Count > 0 && value[0].CaseId == 0)
                {
                    gdvCases.Rows[0].Visible = false;
                }
            }
        }

        public int LocationId
        {
            get
            {
                return (Convert.ToInt32(Session["LoggedInLocationId"]));                
            }
        }

        public List<VCTWeb.Core.Domain.ViewCancelTransaction> ChildList
        {
            get
            {
                return (List<VCTWeb.Core.Domain.ViewCancelTransaction>)ViewState["ChildGridList"];
            }
            set
            {
                ViewState["ChildGridList"] = value;
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

        #region Private Methods
        private void AuthorizedPage()
        {
            security = new Security();
            if (Session["LoggedInUser"] == null)
            {
                Response.Redirect("~/ErrorPage.aspx?ErrorKey=Common_msgSessionExpired");
            }
            //else if (security.HasAccess("KitFamily"))
            //{
            //    CanView = security.HasPermission("KitFamily.Manage");
            //}
            //else
            //    Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }

        private void LocalizePage()
        {
            try
            {
                string heading = string.Empty;
                heading = vctResource.GetString("Home_lblHeader");
                lblHeader.Text = heading;
                Page.Title = heading;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PopulateChildGridRow(GridViewRowEventArgs e)
        {
            string InventoryType = (e.Row.FindControl("hdnInventoryType") as HiddenField).Value;
            if (InventoryType.ToUpper() == "PART")
            {
                GridView grdChild = (GridView)e.Row.FindControl("grdChild");
                Int64 caseId = Convert.ToInt64((e.Row.FindControl("hdnCaseId") as HiddenField).Value);
                presenter.PopulateCaseItems("Part", caseId);
                grdChild.DataSource = this.ChildList;
                grdChild.DataBind();
            }
            else if (InventoryType.ToUpper() == "KIT")
            {
                GridView grdChildKit = (GridView)e.Row.FindControl("grdChildKit");
                Int64 caseId = Convert.ToInt64((e.Row.FindControl("hdnCaseId") as HiddenField).Value);
                presenter.PopulateCaseItems("Kit", caseId);
                grdChildKit.DataSource = this.ChildList;
                grdChildKit.DataBind();

            }


        }
        #endregion

        #region Event Handlers
        protected void gdvCases_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {                
                PopulateChildGridRow(e);             
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
                        gdvKitDetail.DataSource = presenter.PopulateBuildKitById(BuildKitId);
                        gdvKitDetail.DataBind();
                    }
                }
            }

        }
        #endregion
    }
}