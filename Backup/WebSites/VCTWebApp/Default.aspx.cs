using System;
using Microsoft.Practices.ObjectBuilder;
using VCTWebApp.Resources;
using VCTWeb.Core.Domain;
using VCTWebApp.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.Script.Serialization;


namespace VCTWebApp.Shell.Views
{
    public partial class Default : System.Web.UI.Page
    {
        #region Instance Variables
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.AuthorizedPage();
                LocalizePage();

                if (Convert.ToString(Session["LoggedInRole"]).Trim().Contains("ePar+"))
                {
                    //Server.Transfer("DefaultSalesPerson.aspx");
                }
                else if (Convert.ToString(Session["LoggedInRole"]).Trim() == "Sales Rep.")
                {
                    Server.Transfer("DefaultSalesPerson.aspx");
                }
                else
                {
                    Server.Transfer("DefaultInventoryManager.aspx");
                }
                ExpirePageCache();
            }
        }

        private void ExpirePageCache()
        {
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now - new TimeSpan(1, 0, 0));
            Response.Cache.SetLastModified(DateTime.Now);
            Response.Cache.SetAllowResponseInBrowserHistory(false);

        }

        private void AuthorizedPage()
        {
            if (Session["LoggedInUser"] == null)
            {
                Response.Redirect("~/ErrorPage.aspx?ErrorKey=Common_msgSessionExpired");
            }
        }

        private void LocalizePage()
        {
            string heading = string.Empty;
            heading = vctResource.GetString("Home_lblHeader");
            //lblHeader.Text = heading;
            Page.Title = heading;
        }

    }
}

