namespace G42.UmbracoGrease.Helpers
{
    /// <summary>
    /// Helper class to get IP address related information.
    /// </summary>
    public static class IpHelper
    {
        /// <summary>
        /// Gets the IP address.
        /// </summary>
        /// <returns></returns>
        public static string GetIpAddress()
        {
            var context = System.Web.HttpContext.Current;
            
            var ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                var addresses = ipAddress.Split(',');
                
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }
    }
}