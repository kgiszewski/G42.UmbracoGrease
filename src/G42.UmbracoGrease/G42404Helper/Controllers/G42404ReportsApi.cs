using System.Web.Http;
using G42.UmbracoGrease.Core;
using G42.UmbracoGrease.Filters;
using G42.UmbracoGrease.G42404Helper.Models;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace G42.UmbracoGrease.G42404Helper.Controllers
{
    /// <summary>
    /// API controller that handles custom section interactions.
    /// </summary>
    [PluginController("G42UmbracoGrease")]
    public class G42404ReportsApiController : UmbracoAuthorizedJsonController
    {
        /// <summary>
        /// Returns 404's from the DB.
        /// </summary>
        /// <param name="countFilter">The count filter.</param>
        /// <returns></returns>
        [HttpGet]
        [CamelCasingFilter]
        public object Get404s(int countFilter)
        {
            return new G42Grease404TableModel(Grease.Services.G42404Service.GetResults(countFilter));
        }
    }
}