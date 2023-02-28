using Nop.Core;

namespace Nop.Plugin.Widgets.BasicPlugins
{
    /// <summary>
    /// Represents plugin constants
    /// </summary>
    public static class ProductDefaults
    {
        /// <summary>
        /// Gets the system name
        /// </summary>
        public static string SystemName => "Widgets.BasicPlugins";

        /// <summary>
        /// Gets the user agent used to request third-party services
        /// </summary>
        public static string ApplicationName => "nopCommerce-WidgetsBasicPlugin";

        /// <summary>
        /// Gets the configuration route name
        /// </summary>
        public static string ApplicationVersion => $"v{NopVersion.CURRENT_VERSION}";
        /// <summary>
        /// Gets the name of autosuggest component
        /// </summary>

    }
}