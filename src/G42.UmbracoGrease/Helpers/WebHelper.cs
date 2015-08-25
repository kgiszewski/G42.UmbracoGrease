using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace G42.UmbracoGrease.Helpers
{
    public static class WebHelper
    {
        public async static Task<string> Get(string url)
        {
            using (var client = new HttpClient())
            {
                return await client.GetStringAsync(url).ConfigureAwait(false);
            }
        }

        public async static Task<string> Post(string url, HttpContent content)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(url, content).ConfigureAwait(false);

                return await response.Content.ReadAsStringAsync();
            }
        }

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