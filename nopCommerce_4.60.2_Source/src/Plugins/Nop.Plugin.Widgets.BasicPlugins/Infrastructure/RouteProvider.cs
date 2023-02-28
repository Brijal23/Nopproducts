using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc.Routing;
using Nop.Web.Infrastructure;

namespace Nop.Plugin.Widgets.BasicPlugins.Infrastructure
{
    /// <summary>
    /// Represents the plugin route provider
    /// </summary>
    public class RouteProvider : BaseRouteProvider,IRouteProvider
    {
        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="endpointRouteBuilder">Route builder</param>
        public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            var lang = GetLanguageRoutePattern();

            endpointRouteBuilder.MapControllerRoute(name: ProductDefaults.SystemName,
                             pattern: "Admin/DemoProduct/ProductList",
                             defaults: new { controller = "DemoProduct", action = "ProductList" });
        }

        /// <summary>
        /// Gets a priority of route provider
        /// </summary>
        public int Priority => 0;
    }
}