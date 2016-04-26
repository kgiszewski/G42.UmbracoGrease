using System.IO;
using System.Web.Mvc;

namespace G42.UmbracoGrease.G42TransformationHelper
{
    /// <summary>
    /// Helper class that handles transformations.
    /// </summary>
    public static class TransformationHelper
    {
        /// <summary>
        /// Renders a named razor view from a given model.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
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