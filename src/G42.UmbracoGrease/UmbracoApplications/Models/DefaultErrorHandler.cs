using System;
using System.Linq;
using System.Web;
using G42.UmbracoGrease.G42AppSettings.PetaPocoModels;
using G42.UmbracoGrease.Helpers;
using G42.UmbracoGrease.Interfaces;
using Umbraco.Core.Logging;

namespace G42.UmbracoGrease.UmbracoApplications.Models
{
    public class DefaultErrorHandler : IG42ErrorHandler
    {
        public void Execute(object sender, EventArgs e, Exception ex)
        {
            var context = HttpContext.Current;

            var sendTo = G42GreaseAppSetting.GetAppSetting("G42.UmbracoGrease:ErrorEmailToCsv");

            var sendFrom = G42GreaseAppSetting.GetAppSetting("G42.UmbracoGrease:ErrorEmailFrom");

            if (sendTo != null && sendFrom != null)
            {
                var sendToList = sendTo.Value.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);

                if (sendToList.Any())
                {
                    LogHelper.Info<GreaseUmbracoApplication>("Sending email to the G42.UmbracoGreaseErrorEmailToCsv list: " + sendTo.Value);

                    foreach (var email in sendToList)
                    {
                        FormHelper.SendMail(email.Trim(),
                            G42GreaseAppSetting.GetAppSetting("G42.UmbracoGrease:ErrorEmailFrom").Value,
                            "Error reported by: " + HttpContext.Current.Request.Url.AbsoluteUri,
                            ex.Message,
                            true);
                    }
                }

                context.Server.ClearError();

                context.Response.Redirect(G42GreaseAppSetting.GetAppSetting("G42.UmbracoGrease:ErrorRedirectTo").Value);
            }
        }
    }
}