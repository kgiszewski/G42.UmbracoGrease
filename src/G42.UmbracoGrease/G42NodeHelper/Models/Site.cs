using umbraco.cms.businesslogic.web;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace G42.UmbracoGrease.G42NodeHelper.Models
{
    /// <summary>
    /// Model that represents a default website.
    /// </summary>
    public class Site
    {
        public string Domain { get; set; }
        public int RootId { get; set; }

        /// <summary>
        /// Gets the root.
        /// </summary>
        /// <value>
        /// The root IPublishedContent.
        /// </value>
        public IPublishedContent Root
        {
            get
            {
                return GetUmbracoContent(RootId);
            }
        }

        public int HomeId { get; set; }
        /// <summary>
        /// Gets the home.
        /// </summary>
        /// <value>
        /// The home IPublishedContent.
        /// </value>
        public IPublishedContent Home
        {
            get
            {
                return GetUmbracoContent(HomeId);
            }
        }

        public int SiteSettingsId { get; set; }
        /// <summary>
        /// Gets the site settings.
        /// </summary>
        /// <value>
        /// The site settings IPublishedContent.
        /// </value>
        public IPublishedContent SiteSettings
        {
            get
            {
                return GetUmbracoContent(SiteSettingsId);
            }
        }

        /// <summary>
        /// Gets the content of the umbraco given an Id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        protected IPublishedContent GetUmbracoContent(int id)
        {
            return new UmbracoHelper(UmbracoContext.Current).TypedContent(id);
        }

        /// <summary>
        /// This method provides extension point to modify the NodeHelper behavior.
        /// </summary>
        /// <param name="site">The site.</param>
        /// <param name="domain">The domain.</param>
        /// <param name="rootNode">The root node.</param>
        /// <param name="siteRootNode">The site root node.</param>
        /// <param name="siteSettings">The site settings.</param>
        /// <returns></returns>
        public virtual Site MapSite(Site site, Domain domain, IPublishedContent rootNode, IPublishedContent siteRootNode, IPublishedContent siteSettings)
        {
            return site;
        }
    }
}
