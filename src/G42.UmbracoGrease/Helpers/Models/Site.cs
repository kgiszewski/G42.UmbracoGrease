using umbraco.cms.businesslogic.web;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace G42.UmbracoGrease.Helpers.Models
{
    public class Site
    {
        public string Domain { get; set; }
        public int RootId { get; set; }

        public IPublishedContent Root
        {
            get
            {
                return GetUmbracoContent(RootId);
            }
        }

        public int HomeId { get; set; }
        public IPublishedContent Home
        {
            get
            {
                return GetUmbracoContent(HomeId);
            }
        }

        public int SiteSettingsId { get; set; }
        public IPublishedContent SiteSettings
        {
            get
            {
                return GetUmbracoContent(SiteSettingsId);
            }
        }

        protected IPublishedContent GetUmbracoContent(int id)
        {
            return new UmbracoHelper(UmbracoContext.Current).TypedContent(id);
        }

        public virtual Site MapSite(Site site, Domain domain, IPublishedContent rootNode, IPublishedContent siteRootNode, IPublishedContent siteSettings)
        {
            return site;
        }
    }
}
