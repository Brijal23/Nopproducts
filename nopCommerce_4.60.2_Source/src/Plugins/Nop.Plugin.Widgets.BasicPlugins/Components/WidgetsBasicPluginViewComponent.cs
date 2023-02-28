using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widgets.BasicPlugins.Models;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.BasicPlugins.Components
{
    [ViewComponent(Name = "WidgetsBasicPlugins")]
    public class WidgetsBasicPluginViewComponent:NopViewComponent
    {
        public IViewComponentResult Invoke(string widgetZone, object additionalData)
        {
            var model = new Product
            {
                
            };
           
            return View("~/Plugins/Widgets.BasicPlugins/Views/ProductList.cshtml", model);
        }
    }
}
