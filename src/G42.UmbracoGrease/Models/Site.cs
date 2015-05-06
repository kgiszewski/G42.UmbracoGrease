using umbraco.cms.businesslogic.web;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace G42.UmbracoGrease.Models
{
    public class Site
    {
        public string Domain { get; set; }
        public int RootId { get; set; }

        public IPublishedContent Root
        {
            get
            {
                return _getUmbracoContent(RootId);
            }
        }

        public int HomeId { get; set; }
        public IPublishedContent Home
        {
            get
            {
                return _getUmbracoContent(HomeId);
            }
        }

        public int SiteSettingsId { get; set; }
        public IPublishedContent SiteSettings
        {
            get
            {
                return _getUmbracoContent(SiteSettingsId);
            }
        }

        private IPublishedContent _getUmbracoContent(int id)
        {
            return new UmbracoHelper(UmbracoContext.Current).TypedContent(id);
        }

        public virtual Site MapSite(Site site, Domain domain, IPublishedContent rootNode, IPublishedContent siteRootNode, IPublishedContent siteSettings)
        {
            return site;
        }
    }
}
