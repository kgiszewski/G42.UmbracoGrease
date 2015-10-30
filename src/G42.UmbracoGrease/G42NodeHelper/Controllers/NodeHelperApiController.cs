using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using G42.UmbracoGrease.Filters;
using Umbraco.Core.Logging;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace G42.UmbracoGrease.G42NodeHelper.Controllers
{
    /// <summary>
    /// API Controller that handled dashboard NodeHelper interactions.
    /// </summary>
    [PluginController("G42UmbracoGrease")]
    public class NodeHelperApiController : UmbracoAuthorizedJsonController
    {
        [HttpGet]
        [CamelCasingFilter]
        public object Get()
        {
            var sites = new List<object>();

            if (NodeHelper.IsInitialized())
            {
                foreach (var site in NodeHelper.Instance.Sites.OrderBy(x => x.Domain))
                {
                    sites.Add(new
                    {
                        Domain = site.Domain,
                        RootId = site.RootId,
                        CreatedOn = NodeHelper.Instance.CreatedOn
                    });
                }
            }

            return sites;
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object Reset()
        {
            NodeHelper.Clear();

            LogHelper.Info<NodeHelperApiController>("Reset");

            return "reset";
        }
    }
}