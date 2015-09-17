﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Security;
using System.Web;
using System.Web.Mvc;
using G42.UmbracoGrease.G42TransformationHelper;
using G42.UmbracoGrease.G42TransformationHelper.Models;
using HtmlAgilityPack;

namespace G42.UmbracoGrease.Extensions
{
    public static class StringExtensions
    {
        public static string TruncateAtWord(this string text, int maxCharacters, string trailingStringIfTextCut = "…")
        {
            if (text == null || (text = text.Trim()).Length <= maxCharacters)
                return text;

            var trailLength = trailingStringIfTextCut.StartsWith("&") ? 1 : trailingStringIfTextCut.Length;
            maxCharacters = maxCharacters - trailLength >= 0 ? maxCharacters - trailLength : 0;
            var pos = text.LastIndexOf(" ", maxCharacters);
            if (pos >= 0)
                return text.Substring(0, pos) + trailingStringIfTextCut;

            return string.Empty;
        }

        public static string HighlightKeywords(this string text, string query)
        {
            var keywords = query.Split(' ');

            foreach (var keyword in keywords)
            {
                var regex = new Regex(Regex.Escape(keyword), RegexOptions.IgnoreCase);

                foreach (Match match in regex.Matches(text))
                {
                    text = text.Replace(match.Value, "<strong>" + match.Value + "</strong>");
                }
            }

            return text;
        }

        public static string ToXmlSafeString(this string input)
        {
            var escapedString = SecurityElement.Escape(input);

            return input == escapedString ? input : "<![CDATA[" + input + "]]>";
        }

        public static string ToHttpsUrl(this string input)
        {
            return input.Replace("http:", "https:");
        }

        public static string ToAzureBlobUrl(this string input, bool useSameProtocolAsRequest = true)
        {
            var request = HttpContext.Current.Request;
            var requestProtocol = (request.Url.AbsoluteUri.StartsWith("http://")) ? "http" : "https";
            var domain = request.Url.Host;

            if (domain.EndsWith(".local"))
            {
                return input.ToDesiredProtocol(requestProtocol, useSameProtocolAsRequest);
            }

            return input.Replace("://", string.Format("://{0}/remote.axd/", domain)).ToDesiredProtocol(requestProtocol, useSameProtocolAsRequest);
        }

        public static string ToDesiredProtocol(this string input, string desiredProtocol = "http", bool performReplacement = true)
        {
            if (!performReplacement)
            {
                return input;
            }

            var formattedProtocol = string.Format("{0}://", desiredProtocol);

            if (input.StartsWith("http://"))
            {
                return input.Replace("http://", formattedProtocol);
            }

            return input.Replace("https://", formattedProtocol);
        }

        public static int ToIntFromDoubleString(this string input)
        {
            return Convert.ToInt32(Math.Truncate(Convert.ToDouble(input)));
        }

        public static IHtmlString TransformImages(this string input, ControllerContext context, string[] classesToTransform, string pathToPartial = "~/Views/Partials/ImageTransformations.cshtml")
        {
            if (String.IsNullOrWhiteSpace(input))
            {
                return new HtmlString(input);
            }

            var document = new HtmlDocument();
            document.LoadHtml(input);

            var spans = document.DocumentNode.SelectNodes("//span");
            if (spans == null)
                return new HtmlString(input);

            foreach (var span in spans)
            {
                var classAttribute = span.Attributes["class"];

                if (classAttribute != null && !String.IsNullOrWhiteSpace(classAttribute.Value))
                {
                    var spanClasses = classAttribute.Value.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    var classValue = classAttribute.Value.Trim();

                    if (classesToTransform.Intersect(spanClasses).Any())
                    {
                        var img = span.Descendants().FirstOrDefault(x => x.Name == "img");

                        if (img != null)
                        {
                            var renderedTemplate = "";

                            //remove style attr
                            var styleAttr = img.Attributes["style"];
                            if (styleAttr != null)
                            {
                                styleAttr.Value = "";
                            }

                            var relAttr = img.Attributes["rel"];

                            var classAttr = img.Attributes["class"];

                            var cropUrl = img.Attributes["src"].Value;

                            //for some reason the httputility decoders won't do this
                            cropUrl = cropUrl.Replace("&amp;", "&");
                            
                            var queryString = cropUrl.Substring(cropUrl.IndexOf('?') + 1);

                            var parameterDictionary = new Dictionary<string, string>();

                            var pairs = queryString.Split(new[] { "&" }, StringSplitOptions.RemoveEmptyEntries);

                            foreach (var pair in pairs)
                            {
                                var kvp = pair.Split(new[] { "=" }, StringSplitOptions.RemoveEmptyEntries);

                                parameterDictionary.Add(kvp[0], kvp[1].Replace("px", ""));
                            }

                            cropUrl = string.Format("{0}?width={1}&height={2}", cropUrl.Substring(0, cropUrl.IndexOf('?')) .ToAzureBlobUrl(), parameterDictionary["width"].ToIntFromDoubleString(), parameterDictionary["height"].ToIntFromDoubleString());

                            var inlineStyle = new ImageTransformation()
                            {
                                Type = classValue,
                                Text = span.InnerText,
                                Html = span.InnerHtml,
                                Meta = new ImageTag()
                                {
                                    Src = cropUrl,
                                    Title = img.Attributes["alt"].Value,
                                    Alt = img.Attributes["alt"].Value,
                                    Classes = (classAttr != null) ? classAttr.Value : "",
                                    Rel = (relAttr != null) ? relAttr.Value : ""
                                }
                            };

                            renderedTemplate = TransformationHelper.RenderRazorViewToString(context, pathToPartial, inlineStyle).Trim();

                            var newNode = HtmlNode.CreateNode(renderedTemplate);
                            span.ParentNode.ReplaceChild(newNode, span);
                        }
                    }
                }
            }

            return new HtmlString(document.DocumentNode.OuterHtml);
        }

        public static string ToOnlyAlphanumeric(this string input, bool allowSpaces = true, bool allowDashes = true, bool allowUnderscore = true, string replacementString = "")
        {
            var rgx = new Regex("[^a-zA-Z0-9 _-]");
            var output = rgx.Replace(Regex.Escape(input), replacementString);

            if (!allowSpaces)
            {
                output = output.Replace(" ", replacementString);
            }

            if (!allowDashes)
            {
                output = output.Replace("-", replacementString);
            }

            if (!allowUnderscore)
            {
                output = output.Replace("_", replacementString);
            }

            return output;
        }
    }
}