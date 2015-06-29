using System.Web.Http;
using G42.UmbracoGrease.AppSettings.PetaPocoModels;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace G42.UmbracoGrease.AppSettings.Controllers
{
    [PluginController("G42UmbracoGrease")]
    public class AppSettingsApiController : UmbracoAuthorizedJsonController
    {
        [HttpGet]
        public object Get(string key)
        {
            return G42GreaseAppSetting.GetAppSetting(key);
        }

        [HttpPost]
        public object Set(G42GreaseAppSetting model)
        {
            G42GreaseAppSetting.SetAppSetting(model.Key, model.Value);

            return "saved";
        }
    }
}