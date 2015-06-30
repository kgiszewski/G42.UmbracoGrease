using G42.UmbracoGrease.G42NodeHelper;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace G42.UmbracoGrease.Extensions
{
    public static class IPublishedContentExtensions
    {
        public static string CoalesceHtmlTitle(this IPublishedContent content, string prefixPropertyAlias = "pageTitlePrefix", string htmlTitleAlias = "htmlTitle")
        {
            if (content == null || NodeHelper.Instance.CurrentSite.SiteSettings == null)
                return "";

            var umbHelper = new UmbracoHelper();
            return NodeHelper.Instance.CurrentSite.SiteSettings.GetPropertyValue(prefixPropertyAlias) + " " + umbHelper.Coalesce(content.GetPropertyValue(htmlTitleAlias), content.Name);
        }

        public static string CoalesceNavigationTitle(this IPublishedContent content, string propertyAlias = "navigationTitle")
        {
            if (content == null)
                return "";

            var umbHelper = new UmbracoHelper();
            return umbHelper.Coalesce(content.GetPropertyValue(propertyAlias), content.Name);
        }
    }
}