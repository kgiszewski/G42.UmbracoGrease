using System.Security.Cryptography;
using System.Text;

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
    }
}