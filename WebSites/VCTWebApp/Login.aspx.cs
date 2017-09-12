using System;
using Microsoft.Practices.ObjectBuilder;
using VCTWeb.Core.Domain;
using VCTWebApp.Resources;
using System.Web.Security;
using VCTWebApp.Web;
using System.Web;

namespace VCTWebApp.Shell.Views
{
    public partial class Login : Microsoft.Practices.CompositeWeb.Web.UI.Page, ILoginView
    {
        #region Instance Variables

        private LoginPresenter _presenter;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private Helper helper = new Helper();

        #endregion

        #region Public Proprties

        [CreateNew]
        public LoginPresenter Presenter
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

        #region Private Methods

        private void LocalizePage()
        {

            try
            {
                string heading = string.Empty;
                heading = vctResource.GetString("capLogin");
                lblHeader.Text = heading;
                Page.Title = heading;
                //lblRegion.Text = vctResource.GetString("lblRegion");
                lblUserName.Text = vctResource.GetString("lblUserName");
                lblPassword.Text = vctResource.GetString("lblPassword");
            }
            catch
            {
                throw;
            }
        }

        private void UserAuthentication(string userName, string password, bool isActiveDirectory)
        {
            try
            {
                Security userdata = null;
                string fullname = string.Empty;
                if (isActiveDirectory && userName != Constants.ROOT_USER)
                {
                    string domain = string.Empty;
                    if (!string.IsNullOrEmpty(ConfigSetting.GetValue(ConfigSetting.DOMAIN)))
                        domain = ConfigSetting.GetValue(ConfigSetting.DOMAIN);
                    if (!_presenter.AuthenticateAgainstActiveDirectory(userName, password, domain, out fullname))
                    {
                        lblError.Text = vctResource.GetString("Login_msgInvalidCredentials");
                        InsertLogggingDetails(userName, false);
                        txtUserName.Focus();
                        return;
                    }
                    else
                    {
                        userdata = new Security(userName, password, domain);
                        userdata.FullName = fullname;
                    }
                }
                else
                {
                    if (!_presenter.AuthenticateAgainstDatabase(userName, password, out fullname))
                    {
                        lblError.Text = vctResource.GetString("Login_msgInvalidCredentials");
                        InsertLogggingDetails(userName, false);
                        txtUserName.Focus();
                        return;
                    }
                    else
                    {
                        userdata = new Security(userName, password);
                        userdata.FullName = fullname;
                    }

                }

                string usd = userdata.GetDataAsString();
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddMinutes(60), false, usd);

                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                Response.Cookies.Add(authCookie);
                Common.SetCurrentUserInSession(userdata.User, userdata.FullName);

                InsertLogggingDetails(userName, true);
                RedirectToPage(userName);
            }
            catch
            {
                throw;
            }
        }

        private void InsertLogggingDetails(string userName, bool isSuccess)
        {
            try
            {
                this._presenter.InsertLogggingDetails(Request.UserHostAddress, userName, isSuccess);
            }
            catch
            {
                throw;
            }
        }

        private void RedirectToPage(string userName)
        {
            try
            {
                Users user = this.Presenter.FetchUserByUserName(userName);
                Session["LoggedInLocationId"] = user.LocationId;
                Session["LoggedInLocation"] = user.Location;
                Session["LoggedInLocationType"] = user.LocationType;
                //Session["LoggedInRegionId"] = ddlRegion.SelectedValue;
                //Session["LoggedInRegion"] = ddlRegion.SelectedItem.Text;;
                Session["LoggedInUser"] = txtUserName.Text.Trim();
                Session["LoggedInRole"] = this.Presenter.GetUserRoleByUserName(userName);
                Session["TimeZoneOffset"] = this.hdnTimeZoneOffset.Value;
                //if (Session["LoggedInRole"].ToString().Equals("LIM", StringComparison.OrdinalIgnoreCase))
                //{
                //    Response.Redirect("~/CreateRequest.aspx");
                //}
                //else if (Session["LoggedInRole"].ToString().Equals("RIM", StringComparison.OrdinalIgnoreCase))
                //{
                //    Response.Redirect("~/RegionalOfficeRequest.aspx");
                //}
                //else
                //{
                //    Response.Redirect("~/Default.aspx");
                //}
                //Response.Redirect("~/Default.aspx");
                string page = Request.QueryString["ReturnUrl"];
                if (String.IsNullOrEmpty(page))
                {
                    if (Convert.ToString(Session["LoggedInRole"]).Trim().Contains("ePar+"))
                    {
                        Response.Redirect("~/Default.aspx");
                    }
                    else if (Convert.ToString(Session["LoggedInRole"]).Trim().ToUpper().Contains("VISUALTRAY"))
                    {
                        Response.Redirect(vctResource.GetString("VisualTrayURL"));
                    }
                    else if (Convert.ToString(Session["LoggedInRole"]).Trim() == "Sales Rep.")
                    {
                        Response.Redirect("~/DefaultSalesPerson.aspx");
                    }
                    else
                    {
                        Response.Redirect("~/DefaultInventoryManager.aspx");
                    }
                }
                else
                {
                    if (page.Contains("/"))
                        page = page.Substring(page.LastIndexOf("/") + 1);
                    Response.Redirect("~/" + page);
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }

        #endregion

        #region Protected Methods

        ///// <summary>
        ///// Handles the Load event of the Page control.
        ///// </summary>
        ///// <param name="sender">The source of the event.</param>
        ///// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    helper.LogInformation(User.Identity.Name, "Login", "Starting initialization");
                    LocalizePage();
                    this.Presenter.OnViewInitialized();

                    helper.LogInformation(User.Identity.Name, "Login", "Ending initialization");
                }
                catch
                {
                    throw;
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if ((!string.IsNullOrEmpty(txtUserName.Text.Trim())) && (!string.IsNullOrEmpty(txtPassword.Text.Trim())))
            {

                bool isActiveDirectory = false;
                if (!string.IsNullOrEmpty(ConfigSetting.GetValue(ConfigSetting.IS_ACTIVE_DIRECTORY)))
                {
                    isActiveDirectory = bool.Parse(ConfigSetting.GetValue(ConfigSetting.IS_ACTIVE_DIRECTORY));
                    helper.LogInformation(User.Identity.Name, "Login", "Starting authentication");
                    UserAuthentication(txtUserName.Text.Trim(), txtPassword.Text.Trim(), isActiveDirectory);
                    helper.LogInformation(User.Identity.Name, "Login", "Ending authentication");
                    return;
                }
                else
                    helper.LogInformation(User.Identity.Name, "Login", "Active Directory Settings could not be retrieved correctly.");
            }
            else
                lblError.Text = vctResource.GetString("Login_msgInvalidCredentials");
            if (this.txtUserName.Enabled)
                this.txtUserName.Focus();
        }

        protected void btnForgotPassword_click(object sender, EventArgs e)
        {
            Response.Redirect("~/ForgotPassword.aspx");
        }

        #endregion

        #region ILoginView Members

        //public System.Collections.Generic.List<Region> RegionList
        //{
        //    set
        //    {
        //        this.ddlRegion.DataSource = value;
        //        this.ddlRegion.DataTextField = "RegionName";
        //        this.ddlRegion.DataValueField = "RegionId";
        //        this.ddlRegion.DataBind();
        //    }
        //}

        #endregion
    }
}

