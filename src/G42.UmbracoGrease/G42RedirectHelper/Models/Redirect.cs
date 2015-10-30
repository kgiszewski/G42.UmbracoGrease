namespace G42.UmbracoGrease.G42RedirectHelper.Models
{
    /// <summary>
    /// Model that represents redirect data.
    /// </summary>
    public class Redirect
    {
        public int StatusCode { get; set; }
        public string UrlToRedirect { get; set; }
        public string RedirectToUrl { get; set; }
    }
}