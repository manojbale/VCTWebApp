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
    public partial class LocationsKitFamily : Microsoft.Practices.CompositeWeb.Web.UI.Page
    {

        #region Instance Variables
        
        private Helper helper = new Helper();                
        private VCTWebAppResource vctResource = new VCTWebAppResource();        
        private Security security = null;

        #endregion

        #region Init/Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.AuthorizedPage();
                //this.presenter.OnViewInitialized();
                //pageload PopulateLocation();
                PopulateKitStockLevel();

              
            }
        }
        #endregion

        #region Private Properties

        //private bool CanView
        //{
        //    get
        //    {
        //        return ViewState[Common.CAN_VIEW] != null ? (bool)ViewState[Common.CAN_VIEW] : false;
        //    }
        //    set
        //    {
        //        ViewState[Common.CAN_VIEW] = value;
        //    }
        //}

        #endregion

        #region Create New Presenter
        //[CreateNew]
        //public AgingOrdersPresenter Presenter
        //{
        //    get
        //    {
        //        return this.presenter;
        //    }
        //    set
        //    {
        //        if (value == null)
        //            throw new ArgumentNullException("value");

        //        this.presenter = value;
        //        this.presenter.View = this;
        //    }
        //}
        #endregion

        #region ILocationKitFamily Members

     
        #endregion               


        #region Private Methods
       
         private void AuthorizedPage()
        {
            security = new Security();
            if (Session["LoggedInUser"] == null)
            {
                Response.Redirect("~/ErrorPage.aspx?ErrorKey=Common_msgSessionExpired");
            }
            else if (security.HasAccess("InventoryStockKits"))
            {
                //CanCancel = security.HasPermission("InventoryStockKits");
            }
            else
                Response.Redirect(Common.UNAUTHORIZED_PAGE);
        }

        private void PopulateKitStockLevel()
        {
            Int32 LocationId = Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"]);
            List<KitStockLevel> lstKitFamily = new KitFamilyRepository().GetKitStockLevelByLocationId(0, LocationId,0);

            List<Location> Locations = new LocationRepository().GetLocationByParentLocationId(Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"]));
             foreach (var item in Locations)
             {
                 if(!lstKitFamily.Exists(t=> t.LocationId == item.LocationId))                 
                 { 
                     KitStockLevel oModel = new KitStockLevel();
                     oModel.LocationId = item.LocationId;
                     oModel.LocationName = item.LocationName;
                     oModel.Longitude = item.Longitude;
                     oModel.Latitude = item.Latitude;
                     lstKitFamily.Add(oModel);
                 }
             }

            hdnSelfLocationKitFamily.Value = new JavaScriptSerializer().Serialize(lstKitFamily.FindAll(t => t.LocationId == LocationId));
            hdnChildLocationKitFamily.Value = new JavaScriptSerializer().Serialize(lstKitFamily.FindAll(t => t.LocationId != LocationId));

            
            if (lstKitFamily != null && lstKitFamily.FindAll(t => t.LocationId != LocationId).Count > 0)
            {
                hdnLongitude.Value = lstKitFamily.FindAll(t => t.LocationId != LocationId)[0].Longitude.ToString();
                hdnLatitude.Value = lstKitFamily.FindAll(t => t.LocationId != LocationId)[0].Latitude.ToString();
            }
        }

        #endregion

    }
}