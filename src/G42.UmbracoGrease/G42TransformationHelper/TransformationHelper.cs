﻿using System.IO;
using System.Web.Mvc;

namespace G42.UmbracoGrease.G42TransformationHelper
{
    public static class TransformationHelper
    {
        public static string RenderRazorViewToString(ControllerContext context, string viewName, object model)
        {
            using (var sw = new StringWriter())
            {
                var viewResult = System.Web.Mvc.ViewEngines.Engines.FindPartialView(context, viewName);
                var viewContext = new ViewContext(context, viewResult.View, new ViewDataDictionary(model), new TempDataDictionary(), sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}