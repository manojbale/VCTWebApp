using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VCTWebApp.Resources;
using VCTWeb.Core.Domain;
using VCTWebApp.Web;
using System.Xml;

namespace WebApplication.Controls
{
    public partial class MenuNavigation : System.Web.UI.UserControl
    {

        #region Instance Variables

        private string userName = string.Empty;
        private VCTWebAppResource vctResource = new VCTWebAppResource();
        private Helper helper = new Helper();
        private Security security = null;
        #endregion

        #region Private Methods

        private void BuildMenu()
        {
            security = new Security();
            GetMenuItem();
        }

        private void GetChildMenuItem(MenuItem parentMenuItem, XmlNodeList childNodes)
        {
            foreach (XmlNode node in childNodes)
            {
                string permission = string.Empty;
                string imageUrl = string.Empty;
                if (!string.IsNullOrEmpty(node.Attributes["Permission"].Value))
                    permission = node.Attributes["Permission"].Value;
                if (permission.Length == 0 || (security.HasAccess(permission)))
                {
                    string nodeTitle = node.Attributes["Title"].Value;
                    if (!string.IsNullOrEmpty(node.Attributes["ResourceKey"].Value))
                        nodeTitle = vctResource.GetString(node.Attributes["ResourceKey"].Value);
                    if (!string.IsNullOrEmpty(node.Attributes["ImageUrl"].Value))
                        imageUrl = node.Attributes["ImageUrl"].Value;
                    MenuItem childMenuItem = new MenuItem(nodeTitle, nodeTitle, imageUrl, node.Attributes["Url"].Value);
                    parentMenuItem.ChildItems.Add(childMenuItem);
                    if (node.ChildNodes != null && node.ChildNodes.Count > 0)
                    {
                        GetChildMenuItem(childMenuItem, node.ChildNodes);
                        if (childMenuItem.ChildItems.Count == 0)
                        {
                            parentMenuItem.ChildItems.Remove(childMenuItem);
                        }
                        childMenuItem.Selectable = false;
                    }
                }
            }
        }

        private void GetLoggedUserName()
        {
            if (HttpContext.Current.User != null)
                userName = HttpContext.Current.User.Identity.Name;
        }

        private void GetMenuItem()
        {
            XmlDocument appSiteMap = new XmlDocument();
            appSiteMap.Load(Server.MapPath("~/App_Data/MenuSiteMaps/Application.sitemap"));
            MenuItem parentMenuItem = null;
            foreach (XmlNode node in appSiteMap.DocumentElement.ChildNodes)
            {
                parentMenuItem = null;
                string permission = string.Empty;
                string imageUrl = string.Empty;
                if (!string.IsNullOrEmpty(node.Attributes["Permission"].Value))
                    permission = node.Attributes["Permission"].Value;
                if (permission.Length == 0 || (security.HasAccess(permission)))
                {
                    string nodeTitle = node.Attributes["Title"].Value;
                    if (!string.IsNullOrEmpty(node.Attributes["ResourceKey"].Value))
                        nodeTitle = vctResource.GetString(node.Attributes["ResourceKey"].Value);
                    if (!string.IsNullOrEmpty(node.Attributes["ImageUrl"].Value))
                        imageUrl = node.Attributes["ImageUrl"].Value;
                    parentMenuItem = new MenuItem(nodeTitle, nodeTitle, imageUrl, node.Attributes["Url"].Value);

                    if (node.ChildNodes != null && node.ChildNodes.Count > 0)
                    {
                        GetChildMenuItem(parentMenuItem, node.ChildNodes);

                        if (parentMenuItem.ChildItems.Count == 0)
                        {
                            parentMenuItem = null;
                        }
                    }
                }
                if (parentMenuItem != null)
                {
                    if (parentMenuItem.ChildItems.Count > 0)
                    {
                        parentMenuItem.Selectable = false;
                    }
                    mnuNavigation.Items.Add(parentMenuItem);
                }
            }
        }

        #endregion Private Methods

        #region Protected Methods

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    InitialSetting();
                }
                catch (Exception ex)
                {
                    //helper.LogException(ex);
                    throw ex;
                }
            }
        }

        #endregion Protected Methods

        #region Public Methods

        public void InitialSetting()
        {
            mnuNavigation.Items.Clear();
            GetLoggedUserName();
            if (!String.IsNullOrEmpty(userName))
            {
                BuildMenu();
            }
        }

        #endregion Public Methods
    }
}