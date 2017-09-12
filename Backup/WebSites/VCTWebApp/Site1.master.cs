using System;
using Microsoft.Practices.ObjectBuilder;
using VCTWeb.Core.Domain;
using VCTWebApp.Resources;
using System.Web;
using VCTWebApp.Web;
using System.Web.Security;
using System.Web.UI;

namespace VCTWebApp.Shell.MasterPages
{
    public partial class Site1 : Microsoft.Practices.CompositeWeb.Web.UI.MasterPage, ISite1View
    {
        #region Instance Variables

        private Site1Presenter _presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private ConfigSetting configSetting = new ConfigSetting();

        #endregion

        #region Private Methods

        private void ExpirePageCache()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now - new TimeSpan(1, 0, 0));
            Response.Cache.SetLastModified(DateTime.Now);
            Response.Cache.SetAllowResponseInBrowserHistory(false);

        }

        private void LocalizePage()
        {
            lnkLogout.Text = vctResource.GetString("labelLogout");
            hplChangePassowrd.Text = vctResource.GetString("mnuChangePassword");
        }

        private void ActivateChangePassword()
        {
            bool isActiveDirectory = false;
            if (!string.IsNullOrEmpty(ConfigSetting.GetValue(ConfigSetting.IS_ACTIVE_DIRECTORY)))
                isActiveDirectory = bool.Parse(ConfigSetting.GetValue(ConfigSetting.IS_ACTIVE_DIRECTORY));

            if ((isActiveDirectory == false) && (string.Compare(Page.Title.Trim(), vctResource.GetString("mnuChangePassword").Trim()) != 0) && (string.Compare(Page.Title.Trim(), vctResource.GetString("mnuForgotPassword").Trim()) != 0) && (string.Compare(Page.Title.Trim(), vctResource.GetString("capLogin").Trim()) != 0) && (string.Compare(Page.Title.Trim(), vctResource.GetString("capLocateKit").Trim()) != 0) && (string.Compare(Page.Title.Trim(), vctResource.GetString("capInvoiceAdvisory").Trim()) != 0) && (string.Compare(Page.Title.Trim(), vctResource.GetString("capError").Trim()) != 0))
                this.hplChangePassowrd.Visible = true;
            else
                this.hplChangePassowrd.Visible = false;

            if ((isActiveDirectory == false) && (string.Compare(Page.Title.Trim(), vctResource.GetString("mnuForgotPassword").Trim()) != 0) && (string.Compare(Page.Title.Trim(), vctResource.GetString("capLogin").Trim()) != 0) && (string.Compare(Page.Title.Trim(), vctResource.GetString("capLocateKit").Trim()) != 0) && (string.Compare(Page.Title.Trim(), vctResource.GetString("capInvoiceAdvisory").Trim()) != 0) && (string.Compare(Page.Title.Trim(), vctResource.GetString("capError").Trim()) != 0))
                this.ucMenuNavigation.Visible = true;
            else
                this.ucMenuNavigation.Visible = false;
        }


        #endregion

        #region Page Load

        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            //First, check for the existence of the Anti-XSS cookie
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;

            //If the CSRF cookie is found, parse the token from the cookie.
            //Then, set the global page variable and view state user
            //key. The global variable will be used to validate that it matches in the view state form field in the Page.PreLoad
            //method.
            if (requestCookie != null
            && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                //Set the global token variable so the cookie value can be
                //validated against the value in the view state form field in
                //the Page.PreLoad method.
                _antiXsrfTokenValue = requestCookie.Value;

                //Set the view state user key, which will be validated by the
                //framework during each request
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            //If the CSRF cookie is not found, then this is a new session.
            else
            {
                //Generate a new Anti-XSRF token
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");

                //Set the view state user key, which will be validated by the
                //framework during each request
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                //Create the non-persistent CSRF cookie
                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    //Set the HttpOnly property to prevent the cookie from
                    //being accessed by client side script
                    HttpOnly = true,

                    //Add the Anti-XSRF token to the cookie value
                    Value = _antiXsrfTokenValue
                };

                //If we are using SSL, the cookie should be set to secure to
                //prevent it from being sent over HTTP connections
                if (FormsAuthentication.RequireSSL &&
                Request.IsSecureConnection)
                    responseCookie.Secure = true;

                //Add the CSRF cookie to the response
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            //During the initial page load, add the Anti-XSRF token and user
            //name to the ViewState
            if (!IsPostBack)
            {
                //Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;

                //If a user name is assigned, set the user name
                ViewState[AntiXsrfUserNameKey] =
                Context.User.Identity.Name ?? String.Empty;
            }
            //During all subsequent post backs to the page, the token value from
            //the cookie should be validated against the token in the view state
            //form field. Additionally user name should be compared to the
            //authenticated users name
            else
            {
                //Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                || (string)ViewState[AntiXsrfUserNameKey] !=
                (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((string.Compare(Page.Title.Trim(), vctResource.GetString("capLogin").Trim()) != 0) && (string.Compare(Page.Title.Trim(), vctResource.GetString("capError").Trim()) != 0) && (string.Compare(Page.Title.Trim(), vctResource.GetString("mnuForgotPassword").Trim()) != 0))
            {
                this.CheckSessionTimeout();
            }
            if (!this.IsPostBack)
            {
                ExpirePageCache();
                this.configSetting.Reset();
                LocalizePage();
                //lblUserName.Text = Common.GetFullNameOfUser(Common.GetCurrentUser());
                //if (lblUserName.Text.Trim() != string.Empty)
                //{
                //    lblUser.Text = vctResource.GetString("labelLogedInUser") + "&nbsp;"; //"User: ";
                //}
                //else
                //{
                //    lblUser.Text = string.Empty;
                //}

                if (string.Compare(Page.Title.Trim(), vctResource.GetString("capLogin").Trim()) == 0 || string.Compare(Page.Title.Trim(), vctResource.GetString("capLocateKit").Trim()) == 0 || string.Compare(Page.Title.Trim(), vctResource.GetString("mnuForgotPassword").Trim()) == 0 || string.Compare(Page.Title.Trim(), vctResource.GetString("capInvoiceAdvisory").Trim()) == 0 || string.Compare(Page.Title.Trim(), vctResource.GetString("capError").Trim()) == 0)
                    this.lnkLogout.Visible = false;
                else
                    this.lnkLogout.Visible = true;


                ActivateChangePassword();

                _presenter.OnViewInitialized();

            }
            if (Session["LoggedInUser"] != null)
            {
                //lblRegion.Text = "Region/SO : ";
                //lblSO.Text = settings.LoggedInRegion + "/" + settings.LoggedInSalesOffice;
                lblUser.Text = Session["LoggedInLocationType"].ToString() + " : " + Session["LoggedInLocation"].ToString() + " | User : " + Session["LoggedInUser"].ToString();
                //lblUserName.Text = settings.LoggedInUser;
                //lnkLogout.Visible = true;
            }
            else 
            {
                lblUser.Text = string.Empty;
                if ((string.Compare(Page.Title.Trim(), vctResource.GetString("capLogin").Trim()) != 0) && (string.Compare(Page.Title.Trim(), vctResource.GetString("capError").Trim()) != 0) && (string.Compare(Page.Title.Trim(), vctResource.GetString("mnuForgotPassword").Trim()) != 0))
                {
                    Response.Redirect("~/ErrorPage.aspx?ErrorKey=Common_msgSessionExpired");
                }
            }
        }

        #endregion

        #region Create New Presenter

        [CreateNew]
        public Site1Presenter Presenter
        {
            get
            {
                return this._presenter;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this._presenter = value;
                this._presenter.View = this;
            }
        }

        #endregion

        #region ISite1View Members

        public void SetVersion(string version)
        {
            //version = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileVersion;
            //((VCTWeb.Core.Domain.Configuration)Session["Version"]).KeyValue = version;
            this.lblVersion.Text = vctResource.GetString("capVersion") + "&nbsp;" + version;
            this.lblToYear.Text = DateTime.Now.ToUniversalTime().AddHours(-5).Year.ToString();
        }

        #endregion

        #region Event Handlers

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            Response.Redirect("~/Login.aspx");
        }


        #endregion

        #region Checking Session Expiration

        private void CheckSessionTimeout()
        {

            int milliSecondsTimeOut = (this.Session.Timeout * 60000) - 30000;

            string errorPageUrl = string.Empty;
            errorPageUrl = "./ErrorPage.aspx?ErrorKey=Common_msgSessionExpired";

            string script = @"
                    var myTimeOut; 
                    clearTimeout(myTimeOut); " +
                    "var sessionTimeout = " + milliSecondsTimeOut.ToString() + ";" +
                    "function doRedirect(){ window.location.href='" + errorPageUrl + "'; }" +
                    @"myTimeOut=setTimeout('doRedirect()', sessionTimeout); ";

            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(),
                  "CheckSessionOut", script, true);
        }

        #endregion

        #region Public Properties

        public bool IsPrintingServerSide
        {
            get
            {
                return Common.IsPrintingServerSide;
            }
        }

        #endregion
    }
}
