using System.Web.Mvc;
using G42.UmbracoGrease.Core;
using G42.UmbracoGrease.G42ErrorReporting.Models;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace G42.UmbracoGrease.G42ErrorReporting.Controllers
{
    [PluginController("G42UmbracoGrease")]
    public class G42ErrorReportingApiController : UmbracoAuthorizedJsonController
    {
        [HttpPost]
        public object Save(G42ErrorReportingConfigModel model)
        {
            Grease.Services.G42AppSettingsService.SaveErrorReportingConfig(model);

            return model;
        }

        [HttpGet]
        public object GetConfig()
        {
            return Grease.Services.G42AppSettingsService.GetErrorReportingConfig();
        }
    }
}