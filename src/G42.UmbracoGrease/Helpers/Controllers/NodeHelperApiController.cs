using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using G42.UmbracoGrease.Filters;
using Umbraco.Core.Logging;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace G42.UmbracoGrease.Helpers.Controllers
{
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

        [HttpPost]
        public object Reset()
        {
            NodeHelper.Instance = null;

            LogHelper.Info<NodeHelperApiController>("Reset");

            return "reset";
        }
    }
}