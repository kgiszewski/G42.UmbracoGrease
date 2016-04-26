using G42.UmbracoGrease.G42404Helper.Services;
using G42.UmbracoGrease.G42AppSettings.Services;
using G42.UmbracoGrease.G42ErrorReporting.Services;
using G42.UmbracoGrease.G42SearchHelper.Services;
using G42.UmbracoGrease.Shared.Services;

namespace G42.UmbracoGrease.Core
{
    public class Services
    {
        public G42404Service G42404Service;
        public G42SearchService G42SearchService;
        public G42AppSettingsService G42AppSettingsService;
        public G42GeneralService G42GeneralService;
        public G42ErrorReportingService G42ErrorReportingService;

        public Services(G42404Service g42404Service, G42SearchService g42SearchService, G42AppSettingsService g42AppSettingsService, G42GeneralService g42GeneralService, G42ErrorReportingService g42ErrorReportingService)
        {
            G42404Service = g42404Service;
            G42SearchService = g42SearchService;
            G42AppSettingsService = g42AppSettingsService;
            G42GeneralService = g42GeneralService;
            G42ErrorReportingService = g42ErrorReportingService;
        }
    }
}