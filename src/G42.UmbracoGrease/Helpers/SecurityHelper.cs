using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace G42.UmbracoGrease.Helpers
{
    /// <summary>
    /// Helper that handles cryptographic and security related tasks.
    /// </summary>
    public static class SecurityHelper
    {
        /// <summary>
        /// Calculates the MD5 hash of the given input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string CalculateMd5Hash(string input)
        {
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();

            for (var i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Ensures the AWS SSL when using a shared SSL environment.
        /// </summary>
        /// <param name="enableLocal">if set to <c>true</c> [enable local].</param>
        public static void EnsureAwsSsl(bool enableLocal = false)
        {
            if (HttpContext.Current.Request.Url.Host.EndsWith(".local") && !enableLocal)
            {
                return;
            }

            //added this due to how the ssl forwarding is working on AWS
            if (HttpContext.Current.Request.Headers["X-Forwarded-Proto"] == "http")
            {
                HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri.Replace("http:", "https:"));
            }
        }

        /// <summary>
        /// Ensures the SSL on normal setups.
        /// </summary>
        /// <param name="enableLocal">if set to <c>true</c> [enable local].</param>
        public static void EnsureSsl(bool enableLocal = false)
        {
            if (HttpContext.Current.Request.Url.Host.EndsWith(".local") && !enableLocal)
            {
                return;
            }

            if (HttpContext.Current.Request.Url.Port != 443)
            {
                HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri.Replace("http:", "https:"));
            }
        }
    }
}