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
    public static class RedirectHelper
    {
        public static void EnsureAwsSsl()
        {
            //added this due to how the ssl forwarding is working on AWS
            if (HttpContext.Current.Request.Headers["X-Forwarded-Proto"] == "http")
            {
                HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri.Replace("http:", "https:"));
            }
        }

        public static void EnsureSsl()
        {
            if (HttpContext.Current.Request.Url.Port != 443)
            {
                HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri.Replace("http:", "https:"));
            }
        }

        public static void TryRedirect(IPublishedContent rootRedirectNode, string redirectDoctypeAlias, Func<IPublishedContent, Redirect> mapRedirectFunc)
        {
            if (HttpContext.Current == null)
                return;

            var context = HttpContext.Current;

            var redirects = _getRedirectConfig(rootRedirectNode, mapRedirectFunc, redirectDoctypeAlias);

            var redirect = redirects.FirstOrDefault(x => x.UrlToRedirect == GetCurrentPath());

            if (redirect != null)
            {
                LogHelper.Info<Redirect>(string.Format("Redirecting '{0}' to '{1}' with status {2}", redirect.UrlToRedirect, redirect.RedirectToUrl, redirect.StatusCode));
                context.Response.StatusCode = redirect.StatusCode;
                context.Response.RedirectLocation = redirect.RedirectToUrl;

                context.Response.Flush();  
            }

            LogHelper.Info<Redirect>(string.Format("No redirect found for '{0}', 404 issued", GetCurrentPath()));
        }

        public static void SetHttpStatus(int statusCode)
        {
            if (HttpContext.Current == null)
                return;

            var context = HttpContext.Current;

            context.Response.StatusCode = statusCode;
        }

        public static string GetCurrentPath()
        {
            if (HttpContext.Current == null)
                return "";

            var context = HttpContext.Current;

            return context.Request.QueryString["aspxerrorpath"] ?? context.Request.RawUrl;
        }

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