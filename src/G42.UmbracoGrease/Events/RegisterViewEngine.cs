using System;
using G42.UmbracoGrease.ViewEngines;
using Umbraco.Core;
using Umbraco.Core.Logging;

namespace G42.UmbracoGrease.Events
{
    /// <summary>
    /// Class that allows for nested views in the Umbraco Backoffice file system.
    /// </summary>
    public class RegisterViewEngine : ApplicationEventHandler
    {
        /// <summary>
        /// Overridable method to execute when All resolvers have been initialized but resolution is not frozen so they can be modified in this method
        /// </summary>
        /// <param name="umbracoApplication"></param>
        /// <param name="applicationContext"></param>
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            try
            {
                //TODO: CONNECT THIS TO DASHBOARD
                var disableSetting = false;

                if (!disableSetting)
                {
                    LogHelper.Info<RegisterViewEngine>("Registering Grease ViewEngine...");
                    System.Web.Mvc.ViewEngines.Engines.Add(new G42ViewEngine());
                }
                else
                {
                    LogHelper.Info<RegisterViewEngine>("Disabling Grease ViewEngine...");
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