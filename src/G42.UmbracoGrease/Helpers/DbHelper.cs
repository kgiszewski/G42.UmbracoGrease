using Umbraco.Core;

namespace G42.UmbracoGrease.Helpers
{
    /// <summary>
    /// Simple helper class that grabs a reference to available DB's.
    /// </summary>
    public static class DbHelper
    {
        /// <summary>
        /// Gets the Umbraco database context.
        /// </summary>
        /// <value>
        /// The database context.
        /// </value>
        public static DatabaseContext DbContext
        {
            get
            {
                return ApplicationContext.Current.DatabaseContext;
            }
        }
    }
}