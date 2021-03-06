﻿using System;
using System.Linq;
using G42.UmbracoGrease.Core;
using G42.UmbracoGrease.Interfaces;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Web;

namespace G42.UmbracoGrease.G42ErrorReporting
{
    /// <summary>
    /// Custom UmbracoApplication that extends the typical Global.asax implementations on MVC sites.
    /// </summary>
    public class GreaseUmbracoApplication : UmbracoApplication
    {
        /// <summary>
        /// Extension point that handles uncaught errors.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected new void Application_Error(object sender, EventArgs e)
        {
            base.Application_Error(sender, e);

            var enabled = Grease.Services.G42AppSettingsService.GetValue<bool>(Core.Constants.ERROR_REPORTING_ENABLE_KEY);

            if(enabled)
            {
                var lastError = Server.GetLastError();

                if (lastError != null)
                {
                    var errorHandlerType = PluginManager.Current.ResolveTypes<IG42ErrorHandler>().FirstOrDefault(x => x != typeof (DefaultErrorHandler));

                    if (errorHandlerType == null)
                    {
                        LogHelper.Info<IG42ErrorHandler>("Executing default error handler...");

                        var errorHandler = new DefaultErrorHandler();

                        try
                        {
                            errorHandler.Execute(sender, e, lastError);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error<Exception>(ex.Message, ex);
                        }

                        LogHelper.Info<IG42ErrorHandler>("Finished executing default handler.");
                    }
                    else
                    {
                        var errorHandler = ((IG42ErrorHandler) Activator.CreateInstance(errorHandlerType));

                        LogHelper.Info<IG42ErrorHandler>("Using " + errorHandler.GetType());

                        errorHandler.Execute(sender, e, lastError);
                    }
                }
            }
        }
    }
}