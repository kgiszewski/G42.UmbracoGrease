﻿using System.Web.Http;
using G42.UmbracoGrease.Filters;
using G42.UmbracoGrease.Reports.Models;
using G42.UmbracoGrease.Reports.PetaPocoModels;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace G42.UmbracoGrease.Reports.Controllers
{
    /// <summary>
    /// API controller that handles custom section interactions.
    /// </summary>
    [PluginController("G42UmbracoGrease")]
    public class ReportsApiController : UmbracoAuthorizedJsonController
    {
        /// <summary>
        /// Returns 404's from the DB.
        /// </summary>
        /// <param name="countFilter">The count filter.</param>
        /// <returns></returns>
        [HttpGet]
        [CamelCasingFilter]
        public object Get404s(int countFilter)
        {
            return new G42Grease404TableModel(G42Grease404Tracker.Get(countFilter));
        }

        [HttpGet]
        [CamelCasingFilter]
        public object GetKeywords(int countFilter)
        {
            return new G42GreaseSearchTableModel(G42GreaseSearchTrackerKeyword.Get(countFilter));
        }
    }
}