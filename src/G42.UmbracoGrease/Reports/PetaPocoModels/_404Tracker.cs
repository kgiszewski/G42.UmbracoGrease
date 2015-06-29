using System;
using System.Collections.Generic;
using System.Web;
using G42.UmbracoGrease.Helpers;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;

namespace G42.UmbracoGrease.Reports.PetaPocoModels
{
    [PrimaryKey("id")]
    [TableName("_404Tracker")]
    public class _404Tracker
    {
        public long Id { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
        public string Referrer { get; set; }
        public string UserAgent { get; set; }
        public DateTime UpdatedOn { get; set; }

        [ResultColumn]
        public int Count { get; set; }

        [ResultColumn]
        public DateTime LastTried { get; set; }

        public static IEnumerable<_404Tracker> Get(int countFilter = 1)
        {
            return DbHelper.DbContext.Database.Fetch<_404Tracker>(@"
                SELECT *, (SELECT MAX(updatedOn) FROM _404Tracker WHERE domain = t1.domain AND path = t1.path) AS lastTried
                FROM (
	                SELECT domain, path, COUNT(id) AS count
	                FROM _404Tracker
	                GROUP BY domain, path
                ) AS t1
                WHERE t1.count >= @0
            ", countFilter);
        }

        public static void Add()
        {
            var context = HttpContext.Current;
            var referrer = "";

            if (context.Request.UrlReferrer != null)
            {
                referrer = context.Request.UrlReferrer.AbsoluteUri;
            }

            try
            {
                DbHelper.DbContext.Database.Save(new _404Tracker()
                {
                    Domain = context.Request.Url.Host,
                    Path = RedirectHelper.GetCurrentPath(),
                    Referrer = referrer,
                    UserAgent = context.Request.UserAgent,
                    UpdatedOn = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                LogHelper.Error<Exception>(ex.Message, ex);
            }
        }
    }
}