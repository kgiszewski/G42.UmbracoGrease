using System.Web.Http;
using G42.UmbracoGrease.Filters;
using G42.UmbracoGrease.Reports.Models;
using G42.UmbracoGrease.Reports.Models;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace G42.UmbracoGrease.Reports.Controllers
{
    /// <summary>
    /// API controller that handles custom section interactions.
    /// </summary>
    [PluginController("G42UmbracoGrease")]
    public class ReportsApiController : UmbracoAuthorizedJsonController
    {
        [HttpGet]
        [CamelCasingFilter]
        public object GetKeywords(int countFilter)
        {
            return new G42GreaseSearchTableModel(G42GreaseSearchTrackerKeyword.Get(countFilter));
        }
    }
}