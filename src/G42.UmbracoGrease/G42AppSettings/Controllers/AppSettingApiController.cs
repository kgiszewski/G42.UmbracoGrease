using System.Web.Http;
using G42.UmbracoGrease.Filters;
using G42.UmbracoGrease.G42AppSettings.PetaPocoModels;
using Umbraco.Core.Logging;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace G42.UmbracoGrease.G42AppSettings.Controllers
{
    /// <summary>
    /// API controller that handles app setting interactions.
    /// </summary>
    [PluginController("G42UmbracoGrease")]
    public class AppSettingsApiController : UmbracoAuthorizedJsonController
    {
        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        [HttpGet]
        [CamelCasingFilter]
        public object Get(string key)
        {
            return G42GreaseAppSetting.Get(key);
        }

        /// <summary>
        /// Gets all the keys.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CamelCasingFilter]
        public object GetAll()
        {
            return G42GreaseAppSetting.GetAll();
        }

        /// <summary>
        /// Saves the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public object Save(G42GreaseAppSetting model)
        {
            G42GreaseAppSetting.Save(model.Key, model.Value);

            return "saved";
        }

        /// <summary>
        /// Removes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public object Remove([FromBody] int id)
        {
            G42GreaseAppSetting.Remove(id);

            return "removed";
        }

        /// <summary>
        /// Adds the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public object Add(G42GreaseAppSetting model)
        {
            LogHelper.Info<G42GreaseAppSetting>(model.Key + " " + model.Value);

            G42GreaseAppSetting.Add(model.Key, model.Value);

            return "added";
        }
    }
}