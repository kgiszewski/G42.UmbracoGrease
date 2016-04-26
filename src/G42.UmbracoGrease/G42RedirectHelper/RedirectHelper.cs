using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using G42.UmbracoGrease.G42RedirectHelper.Models;
using Umbraco.Core.Logging;
using Umbraco.Web;
using Umbraco.Core.Models;

namespace G42.UmbracoGrease.G42RedirectHelper
{
    /// <summary>
    /// Helper class that handles redirects.
    /// </summary>
    public static class RedirectHelper
    {
        /// <summary>
        /// Tries to find the redirect based on the requested URL.
        /// </summary>
        /// <param name="rootRedirectNode">The root redirect node.</param>
        /// <param name="redirectDoctypeAlias">The redirect doctype alias.</param>
        /// <param name="mapRedirectFunc">The map redirect function.</param>
        /// <returns></returns>
        public static bool TryRedirect(IPublishedContent rootRedirectNode, string redirectDoctypeAlias, Func<IPublishedContent, Redirect> mapRedirectFunc)
        {
            if (HttpContext.Current == null)
                return false;

            var context = HttpContext.Current;

            var redirects = _getRedirectConfig(rootRedirectNode, mapRedirectFunc, redirectDoctypeAlias);

            var redirect = redirects.FirstOrDefault(x => x.UrlToRedirect.ToLower() == GetCurrentPath().ToLower());

            if (redirect != null)
            {
                LogHelper.Info<Redirect>(string.Format("Redirecting '{0}' to '{1}' with status {2}", redirect.UrlToRedirect, redirect.RedirectToUrl, redirect.StatusCode));
                context.Response.StatusCode = redirect.StatusCode;
                context.Response.RedirectLocation = redirect.RedirectToUrl;
                context.Response.Flush();

                return true;
            }

            LogHelper.Info<Redirect>(string.Format("No redirect found for '{0}', 404 issued", GetCurrentPath()));

            return false;
        }

        /// <summary>
        /// Sets the HTTP status to the given value.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        public static void SetHttpStatus(int statusCode)
        {
            if (HttpContext.Current == null)
                return;

            var context = HttpContext.Current;

            context.Response.StatusCode = statusCode;
        }

        /// <summary>
        /// Gets the current path which is useful for custom error pages.
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentPath()
        {
            if (HttpContext.Current == null)
                return "";

            var context = HttpContext.Current;

            return context.Request.QueryString["aspxerrorpath"] ?? context.Request.RawUrl;
        }

        /// <summary>
        /// Gets the redirect configuration.
        /// </summary>
        /// <param name="rootRedirectNode">The root redirect node.</param>
        /// <param name="mapUrlFunc">The map URL function.</param>
        /// <param name="redirectDoctypeAlias">The redirect doctype alias.</param>
        /// <returns></returns>
       private static IEnumerable<Redirect> _getRedirectConfig(IPublishedContent rootRedirectNode, Func<IPublishedContent, Redirect> mapUrlFunc, string redirectDoctypeAlias)
        {
            var redirects = new List<Redirect>();

            var redirectNodes = rootRedirectNode.Descendants().Where(x => x.DocumentTypeAlias == redirectDoctypeAlias);

            foreach (var redirect in redirectNodes)
            {
                redirects.Add(mapUrlFunc(redirect));
            }

            return redirects;
        }
    }
}