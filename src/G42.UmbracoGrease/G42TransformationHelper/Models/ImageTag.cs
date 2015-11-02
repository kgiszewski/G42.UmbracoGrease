﻿namespace G42.UmbracoGrease.G42TransformationHelper.Models
{
    /// <summary>
    /// Model that represents an image tag to be used on a partial.
    /// </summary>
    public class ImageTag
    {
        public string Rel { get; set; }
        public string Src { get; set; }
        public string Title { get; set; }
        public string Alt { get; set; }
        public string Classes { get; set; }
    }
}