using System.IO;
using System.Net;

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
    }
}