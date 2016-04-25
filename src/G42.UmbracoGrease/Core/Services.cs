using G42.UmbracoGrease.G42404Helper.Services;
using G42.UmbracoGrease.G42AppSettings.Services;
using G42.UmbracoGrease.G42SearchHelper.Services;

namespace G42.UmbracoGrease.Core
{
    public class Services
    {
        public G42404Service G42404Service;
        public G42SearchService G42SearchService;
        public G42AppSettingsService G42AppSettingsService;

        public Services(G42404Service g42404Service, G42SearchService g42SearchService, G42AppSettingsService g42AppSettingsService)
        {
            G42404Service = g42404Service;
            G42SearchService = g42SearchService;
            G42AppSettingsService = g42AppSettingsService;
        }
    }
}