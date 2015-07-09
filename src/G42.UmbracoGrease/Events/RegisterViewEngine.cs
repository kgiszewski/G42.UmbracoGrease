using System;
using G42.UmbracoGrease.G42AppSettings.PetaPocoModels;
using G42.UmbracoGrease.ViewEngines;
using Umbraco.Core;
using Umbraco.Core.Logging;

namespace G42.UmbracoGrease.Events
{
    public class RegisterViewEngine : ApplicationEventHandler
    {
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            try
            {
                var disableSetting = G42GreaseAppSetting.Get("G42.UmbracoGrease:ViewEngineDisabled");

                if (disableSetting == null || disableSetting.Value != "1")
                {
                    LogHelper.Info<RegisterViewEngine>("Registering Grease ViewEngine");
                    System.Web.Mvc.ViewEngines.Engines.Add(new G42ViewEngine());
                }
                else
                {
                    LogHelper.Info<RegisterViewEngine>("Disabling Grease ViewEngine");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error<Exception>(ex.Message, ex);
                LogHelper.Error<RegisterViewEngine>("Likely failed due tables not yet created.", ex);
            }

            base.ApplicationStarting(umbracoApplication, applicationContext);
        }
    }
}