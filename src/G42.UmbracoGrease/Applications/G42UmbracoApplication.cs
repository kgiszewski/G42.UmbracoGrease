using System;
using System.Configuration;
using System.Linq;
using System.Web;
using G42.UmbracoGrease.Helpers;
using G42.UmbracoGrease.Interfaces;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Web;

namespace G42.UmbracoGrease.Applications
{
    public class G42UmbracoApplication : UmbracoApplication
    {
        protected new void Application_Error(object sender, EventArgs e)
        {
            base.Application_Error(sender, e);

            var lastError = Server.GetLastError();

            if (lastError != null)
            {
                var errorHandlerType = PluginManager.Current.ResolveTypes<IG42ErrorHandler>().FirstOrDefault();

                if (errorHandlerType == null)
                {
                    LogHelper.Info<string>("Sending email to the ErrorEmailToCsv list...");

                    var sendTo = ConfigurationManager.AppSettings["G42.UmbracoGrease:ErrorEmailToCsv"].Split(
                        new[] {','}, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var email in sendTo)
                    {
                        FormHelper.SendMail(email, ConfigurationManager.AppSettings["G42.UmbracoGrease:ErrorEmailFrom"],
                            "Error reported by: " + HttpContext.Current.Request.Url.AbsoluteUri, lastError.Message, true);
                    }

                    Server.ClearError();

                    Response.Redirect(ConfigurationManager.AppSettings["G42.UmbracoGrease:ErrorRedirectTo"]);
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