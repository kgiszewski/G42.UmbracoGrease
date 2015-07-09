using System.Web.Http;
using G42.UmbracoGrease.Filters;
using G42.UmbracoGrease.G42AppSettings.PetaPocoModels;
using Umbraco.Core.Logging;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace G42.UmbracoGrease.G42AppSettings.Controllers
{
    [PluginController("G42UmbracoGrease")]
    public class AppSettingsApiController : UmbracoAuthorizedJsonController
    {
        [HttpGet]
        [CamelCasingFilter]
        public object Get(string key)
        {
            return G42GreaseAppSetting.Get(key);
        }

        [HttpGet]
        [CamelCasingFilter]
        public object GetAll()
        {
            return G42GreaseAppSetting.GetAll();
        }

        [HttpPost]
        public object Save(G42GreaseAppSetting model)
        {
            G42GreaseAppSetting.Save(model.Key, model.Value);

            return "saved";
        }

        [HttpPost]
        public object Remove([FromBody] int id)
        {
            G42GreaseAppSetting.Remove(id);

            return "removed";
        }

        [HttpPost]
        public object Add(G42GreaseAppSetting model)
        {
            LogHelper.Info<G42GreaseAppSetting>(model.Key + " " + model.Value);

            G42GreaseAppSetting.Add(model.Key, model.Value);

            return "added";
        }
    }
}