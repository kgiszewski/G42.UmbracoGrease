using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace G42.UmbracoGrease.Helpers
{
    /// <summary>
    /// Helper class that performs HTTP requests on behalf of the application.
    /// </summary>
    public static class WebHelper
    {
        /// <summary>
        /// Gets the specified URL async and returns the response as a response string.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public async static Task<string> Get(string url)
        {
            using (var client = new HttpClient())
            {
                return await client.GetStringAsync(url).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Posts to the specified URL async and returns the response as a string.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public async static Task<string> Post(string url, HttpContent content)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(url, content).ConfigureAwait(false);

                return await response.Content.ReadAsStringAsync();
            }
        }

        /// <summary>
        /// Gets the headers of the current request. Optionally add HTML formatting and skip cookies in the output.
        /// </summary>
        /// <param name="useHtml">if set to <c>true</c> use HTML.</param>
        /// <param name="skipCookies">if set to <c>true</c> skip cookies.</param>
        /// <returns></returns>
        public static string GetHeaders(bool useHtml = true, bool skipCookies = true)
        {
            var request = HttpContext.Current.Request;

            var headers = "";

            foreach (var key in request.Headers.AllKeys)
            {
                if (skipCookies && key.ToLower() == "cookie")
                {
                    continue;
                }

                var open = "";
                var close = "";

                if (useHtml)
                {
                    open = "<p>";
                    close = "</p>";
                }

                headers += string.Format("{2}{0,-25}=>{1}{3}\n", key, request.Headers[key], open, close);
            }

            return headers;
        }
    }
}