using System.Web.Http;
using G42.UmbracoGrease.Core;
using G42.UmbracoGrease.G42404Helper.Models;
using G42.UmbracoGrease.Shared.Models;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace G42.UmbracoGrease.Shared.Controllers
{
    [PluginController("G42UmbracoGrease")]
    public class G42GeneralApiController : UmbracoAuthorizedJsonController
    {
        [HttpGet]
        public object GetConfig()
        {
            return Grease.Services.G42GeneralService.GetGeneralConfig();
        }

        [HttpPost]
        public object Save(G42GeneralConfigModel model)
        {
            return Grease.Services.G42GeneralService.SaveGeneralConfig(model);
        }
    }
}