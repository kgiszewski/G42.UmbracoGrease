using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http.Filters;
using Newtonsoft.Json.Serialization;

namespace G42.UmbracoGrease.Filters
{
    /// <summary>
    /// Filter that forces camelCasing on the output of a controller.
    /// </summary>
    public class CamelCasingFilterAttribute : ActionFilterAttribute
    {
        private static JsonMediaTypeFormatter _camelCasingFormatter = new JsonMediaTypeFormatter();

        /// <summary>
        /// Initializes the <see cref="CamelCasingFilterAttribute"/> class.
        /// </summary>
        static CamelCasingFilterAttribute()
        {
            _camelCasingFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        /// <summary>
        /// Occurs after the action method is invoked.
        /// </summary>
        /// <param name="actionExecutedContext">The action executed context.</param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            ObjectContent content = actionExecutedContext.Response.Content as ObjectContent;
            if (content != null)
            {
                if (content.Formatter is JsonMediaTypeFormatter)
                {
                    actionExecutedContext.Response.Content = new ObjectContent(content.ObjectType, content.Value, _camelCasingFormatter);
                }
            }
        }
    }
}