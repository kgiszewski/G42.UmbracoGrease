using System;
using System.Configuration;
using System.Linq;
using System.Web;
using G42.UmbracoGrease.Helpers;
using G42.UmbracoGrease.Shared.Interfaces;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Web;

namespace G42.UmbracoGrease.Applications
{
    public class GreaseUmbracoApplication : UmbracoApplication
    {
        protected new void Application_Error(object sender, EventArgs e)
        {
            base.Application_Error(sender, e);

            var lastError = Server.GetLastError();

            if (lastError != null)
            {
                var errorHandlerType = PluginManager.Current.ResolveTypes<IG42ErrorHandler>().FirstOrDefault();

                var sendTo = ConfigurationManager.AppSettings["G42.UmbracoGrease:ErrorEmailToCsv"].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (errorHandlerType == null)
                {
                    if (sendTo.Any())
                    {
                        LogHelper.Info<GreaseUmbracoApplication>("Sending email to the ErrorEmailToCsv list...");

                        foreach (var email in sendTo)
                        {
                            FormHelper.SendMail(email.Trim(),
                                ConfigurationManager.AppSettings["G42.UmbracoGrease:ErrorEmailFrom"],
                                "Error reported by: " + HttpContext.Current.Request.Url.AbsoluteUri, lastError.Message,
                                true);
                        }

                        Server.ClearError();

                        Response.Redirect(ConfigurationManager.AppSettings["G42.UmbracoGrease:ErrorRedirectTo"]);
                    }
                }
                else
                {
                    var errorHandler = ((IG42ErrorHandler)Activator.CreateInstance(errorHandlerType));

                    errorHandler.Execute(sender, e);
                }
            }
        }
    }
}