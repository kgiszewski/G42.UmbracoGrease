namespace G42.UmbracoGrease.G42TransformationHelper.Models
{
    /// <summary>
    /// Model that represents a transformed image.
    /// </summary>
    public class ImageTransformation
    {
        public string Type { get; set; }
        public string Text { get; set; }
        public string Html { get; set; }
        public object Meta { get; set; }
    }
}