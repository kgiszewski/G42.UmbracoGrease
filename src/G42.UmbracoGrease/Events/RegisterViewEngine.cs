﻿using G42.UmbracoGrease.G42ViewEngines;
using Umbraco.Core;

namespace G42.UmbracoGrease.Events
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