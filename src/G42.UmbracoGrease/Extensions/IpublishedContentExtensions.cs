using G42.UmbracoGrease.Helpers;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace G42.UmbracoGrease.Extensions
{
    public static class IpublishedContentExtensions
    {
        public static bool IsExternalLink(this IPublishedContent content)
        {
            if (content == null)
                return false;

            return (content.DocumentTypeAlias == "NavigationItem");
        }

        public static string CoalesceHtmlTitle(this IPublishedContent content)
        {
            if (content == null)
                return "";

            var umbHelper = new UmbracoHelper();
            return NodeHelper.Instance.CurrentSite.SiteSettings.GetPropertyValue("pageTitlePrefix") + " " + umbHelper.Coalesce(content.GetPropertyValue("htmlTitle"), content.Name);
        }

        public static string CoalesceNavigationTitle(this IPublishedContent content)
        {
            if (content == null)
                return "";

            var umbHelper = new UmbracoHelper();
            return umbHelper.Coalesce(content.GetPropertyValue("navigationTitle"), content.Name);
        }
    }
}