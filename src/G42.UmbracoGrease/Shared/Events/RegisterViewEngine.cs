using G42.UmbracoGrease.ViewEngines;
using Umbraco.Core;

namespace G42.UmbracoGrease.Shared.Events
{
    public class RegisterViewEngine : ApplicationEventHandler
    {
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication,
            ApplicationContext applicationContext)
        {
            System.Web.Mvc.ViewEngines.Engines.Add(new G42ViewEngine());

            base.ApplicationStarting(umbracoApplication, applicationContext);
        }
    }
}