using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Services;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Configuration;
using Microsoft.Practices.CompositeWeb.Authorization;
using Microsoft.Practices.CompositeWeb.EnterpriseLibrary.Services;
using System.Collections.Specialized;
using System.Xml;
using System.Web;

namespace VCTWebApp.Shell
{
    public class ShellModuleInitializer : ModuleInitializer
    {
        private const string AuthorizationSection = "compositeWeb/authorization";

        public override void Load(CompositionContainer container)
        {
            base.Load(container);

            AddGlobalServices(container.Parent.Services);
            AddModuleServices(container.Services);
            //RegisterSiteMapInformation(container.Services.Get<ISiteMapBuilderService>(true));
        }

        protected virtual void AddGlobalServices(IServiceCollection globalServices)
        {
            //globalServices.AddNew<EnterpriseLibraryAuthorizationService, IAuthorizationService>();
            globalServices.AddNew<SiteMapBuilderService, ISiteMapBuilderService>();
        }

        protected virtual void AddModuleServices(IServiceCollection moduleServices)
        {
            // TODO: register services that can be accesed only by the Shell module
        }

        //protected virtual void RegisterSiteMapInformation(ISiteMapBuilderService siteMapBuilderService)
        //{
        //    ////SiteMapNodeInfo moduleNode = new SiteMapNodeInfo("Home", "~/Default.aspx", "Home", "Home");
        //    ////siteMapBuilderService.AddNode(moduleNode);

        //    ////siteMapBuilderService.RootNode.Url = "~/Default.aspx";
        //    ////siteMapBuilderService.RootNode.Title = "VCTWebApp";

        //    //SiteMapNodeInfo configurationModule = new SiteMapNodeInfo("Configuration", string.Empty, "Configuration", string.Empty, null, PermissionAndImageUrlAttributes(string.Empty, "../Images/MenuIcons/icon_configuration.png"), null, "mnuConfiguration");
        //    //siteMapBuilderService.AddNode(configurationModule, 1);

        //    //SiteMapNodeInfo authorizationNode = new SiteMapNodeInfo("Authorization", string.Empty, "Authorization", null, null, PermissionAndImageUrlAttributes(string.Empty, "../Images/MenuIcons/icon_authorization.png"), null, "mnuAuthorization");
        //    //siteMapBuilderService.AddNode(authorizationNode, configurationModule);

        //    //SiteMapNodeInfo roleNode = new SiteMapNodeInfo("Role", "~/Role.aspx", "Role", null, null, PermissionAndImageUrlAttributes("Role", "../Images/MenuIcons/icon_role.png"), null, "mnuRole");
        //    //siteMapBuilderService.AddNode(roleNode, authorizationNode);
        //    //SiteMapNodeInfo userNode = new SiteMapNodeInfo("User", "~/User.aspx", "User", null, null, PermissionAndImageUrlAttributes("User", "../Images/MenuIcons/icon_user.png"), null, "mnuUser");
        //    //siteMapBuilderService.AddNode(userNode, authorizationNode);
        //    ////SiteMapNodeInfo changePasswordNode = new SiteMapNodeInfo("ChangePassword", "~/Settings/ChangePassword.aspx", "Change Password", null, null, PermissionAndImageUrlAttributes("Password", "../Images/MenuIcons/icon_change_password.png"), null, "mnuChangePassword");
        //    ////siteMapBuilderService.AddNode(changePasswordNode, authorizationNode);

        //    //SiteMapNodeInfo configurationSetting = new SiteMapNodeInfo("ConfigurationSetting", "~/ConfigurationSetting.aspx", "Configuration Setting", null, null, PermissionAndImageUrlAttributes("Configuration", "../Images/MenuIcons/icon_configuration.png"), null, "mnuConfigurationSettings");
        //    //siteMapBuilderService.AddNode(configurationSetting, authorizationNode);

        //    //SiteMapNodeInfo requestModule = new SiteMapNodeInfo("Request", string.Empty, "Requests", string.Empty, null, PermissionAndImageUrlAttributes(string.Empty, "../Images/MenuIcons/icon_request.png"), null, "mnuRequest");
        //    //siteMapBuilderService.AddNode(requestModule, 2);

        //    //SiteMapNodeInfo createRequestNode = new SiteMapNodeInfo("Request.Create", "~/CreateRequest.aspx", "Create", null, null, PermissionAndImageUrlAttributes("Request", "../Images/MenuIcons/icon_createRequest.png"), null, "mnuCreateRequest");
        //    //siteMapBuilderService.AddNode(createRequestNode, requestModule);

        //    //SiteMapNodeInfo viewPendingRequestNode = new SiteMapNodeInfo("PendingRequests.View", "~/RegionalOfficeRequest.aspx", "Pending Requests", null, null, PermissionAndImageUrlAttributes("Request", "../Images/MenuIcons/icon_viewPendingRequest.png"), null, "mnuviewPendingRequest");
        //    //siteMapBuilderService.AddNode(viewPendingRequestNode, requestModule);

        //    //SiteMapNodeInfo reportsModule = new SiteMapNodeInfo("Reports", string.Empty, "Reports", string.Empty, null, PermissionAndImageUrlAttributes(string.Empty, "../Images/MenuIcons/icon_reports.png"), null, "mnuReports");
        //    //siteMapBuilderService.AddNode(reportsModule, 3);

        //    XmlDocument apps = new XmlDocument();
        //    apps.Load(HttpContext.Current.Server.MapPath("~/App_Data/MenuSiteMaps/Application.sitemap"));
        //    for (int i = 1; i <= apps.DocumentElement.ChildNodes.Count; i++)
        //    {
        //        XmlNode node = apps.DocumentElement.ChildNodes[i - 1];
        //        SiteMapNodeInfo siteMapNode = new SiteMapNodeInfo(node.Attributes["Key"].Value, node.Attributes["Url"].Value, node.Attributes["Title"].Value, string.Empty, null, PermissionAndImageUrlAttributes(node.Attributes["Permission"].Value, node.Attributes["ImageUrl"].Value), null, node.Attributes["ResourceKey"].Value);
        //        siteMapBuilderService.AddNode(siteMapNode, i);
        //        if (node.Attributes["Url"].Value == string.Empty && node.HasChildNodes)
        //        {
        //            LoadSubMenus(siteMapNode, node, siteMapBuilderService);
        //        }
        //    }
        //}

        //private void LoadSubMenus(SiteMapNodeInfo parentSiteMapNode, XmlNode parentXMLNode, ISiteMapBuilderService siteMapBuilderService)
        //{
        //    foreach (XmlNode node in parentXMLNode.ChildNodes)
        //    {
        //        SiteMapNodeInfo siteMapNode = new SiteMapNodeInfo(node.Attributes["Key"].Value, node.Attributes["Url"].Value, node.Attributes["Title"].Value, string.Empty, null, PermissionAndImageUrlAttributes(node.Attributes["Permission"].Value, node.Attributes["ImageUrl"].Value), null, node.Attributes["ResourceKey"].Value);
        //        siteMapBuilderService.AddNode(siteMapNode, parentSiteMapNode);
        //        if (node.Attributes["Url"].Value == string.Empty && node.HasChildNodes)
        //        {
        //            LoadSubMenus(siteMapNode, node, siteMapBuilderService);
        //        }
        //    }
        //}

        //private NameValueCollection PermissionAndImageUrlAttributes(string permission, string imageUrl)
        //{
        //    NameValueCollection attributes = new NameValueCollection(2);
        //    attributes.Add("ImageUrl", imageUrl);
        //    attributes.Add("Permission", permission);
        //    return attributes;
        //}

        public override void Configure(IServiceCollection services, System.Configuration.Configuration moduleConfiguration)
        {
            IAuthorizationRulesService authorizationRuleService = services.Get<IAuthorizationRulesService>();
            if (authorizationRuleService != null)
            {
                AuthorizationConfigurationSection authorizationSection = moduleConfiguration.GetSection(AuthorizationSection) as AuthorizationConfigurationSection;
                if (authorizationSection != null)
                {
                    foreach (AuthorizationRuleElement ruleElement in authorizationSection.ModuleRules)
                    {
                        authorizationRuleService.RegisterAuthorizationRule(ruleElement.AbsolutePath, ruleElement.RuleName);
                    }
                }
            }
        }
    }
}
