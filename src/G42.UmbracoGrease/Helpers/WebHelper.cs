using System.IO;
using System.Net;
using System.Web;

namespace G42.UmbracoGrease.Helpers
{
    public static class WebHelper
    {
        public static string MakeWebRequest(string url)
        {
            var request = WebRequest.Create(url);

            using (var response = request.GetResponse())
            {
                var dataStream = response.GetResponseStream();

                if (dataStream == null)
                {
                    return "";
                }

                var reader = new StreamReader(dataStream);

                var responseFromServer = reader.ReadToEnd();

                reader.Close();
                response.Close();

                return responseFromServer;
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