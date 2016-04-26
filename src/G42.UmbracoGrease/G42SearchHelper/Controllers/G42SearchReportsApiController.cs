using System.Web.Http;
using G42.UmbracoGrease.Core;
using G42.UmbracoGrease.Filters;
using G42.UmbracoGrease.G42SearchHelper.Models;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace G42.UmbracoGrease.G42SearchHelper.Controllers
{
    /// <summary>
    /// API controller that handles custom section interactions.
    /// </summary>
    [PluginController("G42UmbracoGrease")]
    public class G42SearchReportsApiController : UmbracoAuthorizedJsonController
    {
        [HttpGet]
        [CamelCasingFilter]
        public object GetKeywords(int countFilter)
        {
            return new G42GreaseSearchTableModel(Grease.Services.G42SearchService.Get(countFilter));
        }
    }
}