using System;
using System.Linq;
using System.Web;
using G42.UmbracoGrease.AppSettings.PetaPocoModels;
using G42.UmbracoGrease.Helpers;
using G42.UmbracoGrease.Interfaces;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Web;

namespace G42.UmbracoGrease.UmbracoApplications
{
    public class GreaseUmbracoApplication : UmbracoApplication
    {
        protected new void Application_Error(object sender, EventArgs e)
        {
            base.Application_Error(sender, e);

            var disabledSetting = G42GreaseAppSetting.GetAppSetting("G42.UmbracoGrease:ErrorHandlerDisabled");

            if(disabledSetting == null || disabledSetting.Value != "1")
            {
                var lastError = Server.GetLastError();

                if (lastError != null)
                {
                    var errorHandlerType = PluginManager.Current.ResolveTypes<IG42ErrorHandler>().FirstOrDefault();

                    var sendTo =
                        G42GreaseAppSetting.GetAppSetting("G42.UmbracoGrease:ErrorEmailToCsv")
                            .Value.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);

                    if (errorHandlerType == null)
                    {
                        if (sendTo.Any())
                        {
                            LogHelper.Info<GreaseUmbracoApplication>("Sending email to the ErrorEmailToCsv list...");

                            foreach (var email in sendTo)
                            {
                                FormHelper.SendMail(email.Trim(),
                                    G42GreaseAppSetting.GetAppSetting("G42.UmbracoGrease:ErrorEmailFrom").Value,
                                    "Error reported by: " + HttpContext.Current.Request.Url.AbsoluteUri,
                                    lastError.Message,
                                    true);
                            }

                            Server.ClearError();

                            Response.Redirect(
                                G42GreaseAppSetting.GetAppSetting("G42.UmbracoGrease:ErrorRedirectTo").Value);
                        }
                    }
                    else
                    {
                        var errorHandler = ((IG42ErrorHandler) Activator.CreateInstance(errorHandlerType));

                        errorHandler.Execute(sender, e);
                    }
                }
            }
        }
    }
}