using Umbraco.Core;
using Umbraco.Core.Cache;
using Umbraco.Core.Logging;
using Umbraco.Web.Cache;

namespace G42.UmbracoGrease.G42AppSettings.Events
{
    public class AppSettingsCacheUpdate : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.ApplicationStarted(umbracoApplication, applicationContext);

            CacheRefresherBase<PageCacheRefresher>.CacheUpdated += _publishedPageCacheRefresherCacheUpdated;
        }

        private static void _publishedPageCacheRefresherCacheUpdated(PageCacheRefresher sender, CacheRefresherEventArgs e)
        {
            LogHelper.Info<AppSettingsCacheUpdate>("Page cache refreshed.");
            G42AppSettings.Cache.AppSettingsCache.Clear();
        }
    }
}