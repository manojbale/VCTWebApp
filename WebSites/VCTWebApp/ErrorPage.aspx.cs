using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Web.Security;
using VCTWebApp.Resources;

namespace VCTWebApp.Web
{
    public partial class ErrorPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            VCTWebAppResource vctResource = new VCTWebAppResource();
            if (Request.QueryString[Common.ERROR_KEY] != null)
            {
                string errorKey = (string)Request.QueryString[Common.ERROR_KEY];
                if (string.Compare(errorKey, "Common_msgSessionExpired") == 0)
                {
                    lblErrorMessage.Text = string.Format(CultureInfo.InvariantCulture, vctResource.GetString("Common_msgSessionExpired"), "<br><a class='SiteLinkUnderline' href='Login.aspx'>" + vctResource.GetString("Common_msgLoginPage") + "</a>");
                    Session.Abandon();
                    FormsAuthentication.SignOut();
                }
                else
                    lblErrorMessage.Text = vctResource.GetString(Request.QueryString[Common.ERROR_KEY]);
            }
            else
            {
                lblErrorMessage.Text = vctResource.GetString("Error_msgUnknownError");
            }
        }
    }
}
