using System.Web.Http;
using G42.UmbracoGrease.Helpers;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace G42.UmbracoGrease.Controllers
{
    [PluginController("DeveloperDashboardApi")]
    public class DeveloperDashboardController : UmbracoAuthorizedJsonController
    {
        [HttpGet]
        public object ClearNodeHelper()
        {
            NodeHelper.Instance = null;

            return "Cleared";
        }
    }
}