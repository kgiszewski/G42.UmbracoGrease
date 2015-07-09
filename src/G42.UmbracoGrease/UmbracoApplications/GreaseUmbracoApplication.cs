using System;
using System.Linq;
using G42.UmbracoGrease.G42AppSettings.PetaPocoModels;
using G42.UmbracoGrease.Interfaces;
using G42.UmbracoGrease.UmbracoApplications.Models;
using Umbraco.Core;
using Umbraco.Web;

namespace G42.UmbracoGrease.UmbracoApplications
{
    public class GreaseUmbracoApplication : UmbracoApplication
    {
        protected new void Application_Error(object sender, EventArgs e)
        {
            base.Application_Error(sender, e);

            var disabledSetting = G42GreaseAppSetting.Get("G42.UmbracoGrease:ErrorHandlerDisabled");

            if(disabledSetting == null || disabledSetting.Value != "1")
            {
                var lastError = Server.GetLastError();

                if (lastError != null)
                {
                    var errorHandlerType = PluginManager.Current.ResolveTypes<IG42ErrorHandler>().FirstOrDefault();

                    if (errorHandlerType == null)
                    {
                        new DefaultErrorHandler().Execute(sender, e, lastError);
                    }
                    else
                    {
                        var errorHandler = ((IG42ErrorHandler) Activator.CreateInstance(errorHandlerType));

                        errorHandler.Execute(sender, e, lastError);
                    }
                }
            }
        }
    }
}