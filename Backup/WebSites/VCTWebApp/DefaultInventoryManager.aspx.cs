using System;
using Microsoft.Practices.ObjectBuilder;
using VCTWebApp.Resources;
using VCTWeb.Core.Domain;
using VCTWebApp.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using VCTWebApp.Shell.Views;

namespace VCTWebApp
{

    public partial class DefaultInventoryManager : Microsoft.Practices.CompositeWeb.Web.UI.Page, IDefaultInventoryManagerView
    {
        #region Instance Variables

        private DefaultInventoryManagerPresenter presenter;
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
                this.presenter.OnViewInitialized();

                if (BranchDetail != null)
                {
                    hdnLongitude.Value = this.BranchDetail.Longitude.ToString();
                    hdnLatitude.Value = this.BranchDetail.Latitude.ToString();
                }
                this.LocalizePage();

                var jsonBranchDetail = new JavaScriptSerializer().Serialize(this.BranchDetail);
                hdnBranchData.Value = jsonBranchDetail;

                var jsonCaseList = new JavaScriptSerializer().Serialize(this.CasesList);
                hdnCasesData.Value = jsonCaseList;
            }
            this.presenter.OnViewLoaded();
        }
        #endregion

        #region Create New Presenter
        [CreateNew]
        public DefaultInventoryManagerPresenter Presenter
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

        #region IDefault Members
        public int InventoryUtilizationDay
        {
            get
            {
                return (Convert.ToInt32(ViewState["InventoryUtilizationDay"]));
            }
            set
            {
                ViewState["InventoryUtilizationDay"] = value;
            }
        }

        public List<VCTWeb.Core.Domain.Default> PartOrderList
        {
            set
            {
                gvPartOrderList.DataSource = value;
                gvPartOrderList.DataBind();
            }
        }

        public List<VCTWeb.Core.Domain.ActiveKitFamily> ActiveKitFamilyList
        {
            set
            {
                gvKitOrderList.DataSource = value;
                gvKitOrderList.DataBind();
            }
        }

        public List<VCTWeb.Core.Domain.PendingBuildKit> PendingBuildKitList
        {
            set
            {
                gdvPendingBuildKit.DataSource = value;
                gdvPendingBuildKit.DataBind();
            }
        }

        public VCTWeb.Core.Domain.Default BranchDetail
        {
            get
            {
                return (VCTWeb.Core.Domain.Default)ViewState["BranchDetail"];
            }
            set
            {
                ViewState["BranchDetail"] = value;
            }
        }

        public List<VCTWeb.Core.Domain.Default> CasesList
        {
            get
            {
                return (List<VCTWeb.Core.Domain.Default>)ViewState["CasesList"];
            }
            set
            {
                ViewState["CasesList"] = value;
            }
        }

        public int LocationId
        {
            get
            {
                return (Convert.ToInt32(Session["LoggedInLocationId"]));
            }
        }

        public DateTime BusinessDate
        {
            get
            {
                return (DateTime.Today);
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

                lblPartOrder.Text = string.Format(vctResource.GetString("Home_lblgvPartOrderHeader"), this.InventoryUtilizationDay);
                lblKitOrder.Text = string.Format(vctResource.GetString("Home_lblgvKitOrderHeader"), this.InventoryUtilizationDay);

                if (gvPartOrderList.Rows.Count > 0)
                {
                    ((gvPartOrderList.HeaderRow.FindControl("lblPartNumberHeader")) as Label).Text = vctResource.GetString("Home_lblgvPartNum");
                    ((gvPartOrderList.HeaderRow.FindControl("lblDescriptionHeader")) as Label).Text = vctResource.GetString("Home_lblgvDesc");
                    ((gvPartOrderList.HeaderRow.FindControl("lblTotalHeader")) as Label).Text = vctResource.GetString("Home_lblNumberOfOrders");
                }

                if (gvKitOrderList.Rows.Count > 0)
                {
                    ((gvKitOrderList.HeaderRow.FindControl("lblKitFamilyHeader")) as Label).Text = vctResource.GetString("Home_lblgvKitFamily");
                    ((gvKitOrderList.HeaderRow.FindControl("lblDescriptionHeader")) as Label).Text = vctResource.GetString("Home_lblgvDesc");
                    ((gvKitOrderList.HeaderRow.FindControl("lblNumberOfOrdersHeader")) as Label).Text = vctResource.GetString("Home_lblNumberOfOrders");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Event Handlers

        protected void gdvPendingBuildKit_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Build"))
                {
                    string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
                    Session["PendingBuildKitNumber"]= commandArgs[0];
                    Session["PendingBuildKitDescription"] = commandArgs[1];
                    Response.Redirect("~/VirtualBuildKit.aspx");
                }
            }
            catch (Exception ex)
            {
                //this.lblError.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString(ex.Message), this.lblHeader.Text);
            }
        }

        #endregion
    }
}