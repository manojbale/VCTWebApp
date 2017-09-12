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
using System.Web.Script.Serialization;
using Microsoft.Practices.ObjectBuilder;

namespace VCTWebApp
{
    public partial class AgingOrders : Microsoft.Practices.CompositeWeb.Web.UI.Page, IAgingOrdersView
    {

        #region Instance Variables

        private AgingOrdersPresenter presenter;
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

                hdnAgingOrdersData.Value = new JavaScriptSerializer().Serialize(this.AgingOrdersList);                
                hdnAgingBranchData.Value = new JavaScriptSerializer().Serialize(this.BranchDetail);

                if (BranchDetail != null)
                {
                    hdnAgingLongitude.Value = BranchDetail.Longitude.ToString();
                    hdnAgingLatitude.Value = BranchDetail.Latitude.ToString();
                }
            }            
        }
        #endregion

        #region Create New Presenter
        [CreateNew]
        public AgingOrdersPresenter Presenter
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

        #region IAgingOrders Members   

        public List<VCTWeb.Core.Domain.Default> AgingOrdersList
        {
            get
            {
                return (List<VCTWeb.Core.Domain.Default>)ViewState["AgingOrder"];
            }
            set
            {
                ViewState["AgingOrder"] = value;
            }
        }

        public VCTWeb.Core.Domain.Default BranchDetail
        {
            get 
            {
                return (VCTWeb.Core.Domain.Default)ViewState["BranchData"];
            }
            set
            {
                ViewState["BranchData"] = value;                
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
        #endregion
             
    }
}