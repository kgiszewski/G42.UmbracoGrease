using Umbraco.Core;

namespace G42.UmbracoGrease.Helpers
{
    public static class DbHelper
    {
        public static DatabaseContext DbContext
        {
            get
            {
                return ApplicationContext.Current.DatabaseContext;
            }
        }
    }
}