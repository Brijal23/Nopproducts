using Microsoft.AspNetCore.Routing;
using Nop.Core;
using Nop.Plugin.Widgets.BasicPlugins.Components;
using Nop.Services.Cms;
using Nop.Services.Plugins;
using Nop.Web.Framework;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Widgets.BasicPlugins
{
    public class BasicPlugins:BasePlugin,IWidgetPlugin,IAdminMenuPlugin
    {
        private readonly IWebHelper _webHelper;
        public bool HideInWidgetList => false;

        public Type GetWidgetViewComponent(string widgetZone)
        {
            return typeof(WidgetsBasicPluginViewComponent);
        }

        public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string> { PublicWidgetZones.ProductDetailsTop });
        }

        //public override string GetConfigurationPageUrl()
        //{
        //    return $"{_webHelper.GetStoreLocation()}Admin/Customer/Register";
        //}

        public override Task InstallAsync()
        {
            return base.InstallAsync();
        }
        public Task ManageSiteMapAsync(SiteMapNode rootNode)
        {
            var menuItem = new SiteMapNode()
            {
                SystemName = "Basic widget plugin",
                Title = "Widget Plugin",
                ControllerName = "DemoProduct",
                ActionName = "ProductList",
                IconClass = "far fa-dot-circle",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", AreaNames.Admin } },
            };
            var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Basic Widget plugin");
            if (pluginNode != null)
                pluginNode.ChildNodes.Add(menuItem);
            else
                rootNode.ChildNodes.Add(menuItem);

            return Task.CompletedTask;
        }

        public override Task UninstallAsync()
        {
            return base.UninstallAsync();
        }
    }
}