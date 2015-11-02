using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.IO;
using Umbraco.Core.Logging;
using Umbraco.Core.IO;

namespace G42.UmbracoGrease.ViewEngines
{
    /// <summary>
    /// A class that extends the RazorViewEngine that allows for nested view folders to be detected automatically.
    /// </summary>
    public class G42ViewEngine : RazorViewEngine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="G42ViewEngine"/> class.
        /// </summary>
        public G42ViewEngine()
        {
            var viewsPath = IOHelper.MapPath("~/Views");

            var directories = Directory.GetDirectories(viewsPath);

            var pathList = new List<string>();

            foreach (var directory in directories.Where(x => !x.ToLower().Contains("partials")))
            {
                var folder = Path.GetFileName(directory);

                var path = string.Format("~/Views/{0}/{{0}}.cshtml", folder);

                pathList.Add(path);

                LogHelper.Info<G42ViewEngine>("Registering view engine path: " + folder);
            }

            ViewLocationFormats = ViewLocationFormats.Union(pathList).ToArray();
        }
    }
}