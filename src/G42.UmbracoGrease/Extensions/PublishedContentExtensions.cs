using G42.UmbracoGrease.G42NodeHelper;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace G42.UmbracoGrease.Extensions
{
    /// <summary>
    /// IPublishedContent Extensions.
    /// </summary>
    public static class PublishedContentExtensions
    {
        /// <summary>
        /// Coalesces the HTML title from a property stored within NodeHelper's SiteSettings property.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="prefixPropertyAlias">The prefix property alias.</param>
        /// <param name="htmlTitleAlias">The HTML title alias.</param>
        /// <returns></returns>
        public static string CoalesceHtmlTitle(this IPublishedContent content, string prefixPropertyAlias = "pageTitlePrefix", string htmlTitleAlias = "htmlTitle")
        {
            if (content == null || NodeHelper.Instance.CurrentSite.SiteSettings == null)
                return "";

            var umbHelper = new UmbracoHelper();

            return NodeHelper.Instance.CurrentSite.SiteSettings.GetPropertyValue(prefixPropertyAlias) + " " + umbHelper.Coalesce(content.GetPropertyValue(htmlTitleAlias), content.Name);
        }

        /// <summary>
        /// Coalesces the navigation title from the given property alias. (i.e. Use the custom property or default to the node name).
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="propertyAlias">The property alias.</param>
        /// <returns></returns>
        public static string CoalesceNavigationTitle(this IPublishedContent content, string propertyAlias = "navigationTitle")
        {
            if (content == null)
                return "";

            var umbHelper = new UmbracoHelper();

            return umbHelper.Coalesce(content.GetPropertyValue(propertyAlias), content.Name);
        }
    }
}