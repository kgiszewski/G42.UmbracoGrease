using System;
using System.Web;
using G42.UmbracoGrease.Helpers;
using Umbraco.Core.Persistence;

namespace G42.UmbracoGrease.Reports.PetaPocoModels
{
    [TableName("G42GreaseSearchTrackerSearches")]
    [PrimaryKey("id")]
    public class G42GreaseSearchTrackerSearch
    {
        public int Id { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
        public string Keywords { get; set; }
        public DateTime SearchedOn { get; set; }

        public static void Add(string keywords)
        {
            var context = HttpContext.Current;

            var domain = context.Request.Url.Host;

            foreach (var keyword in keywords.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries))
            {
                G42GreaseSearchTrackerKeyword.Add(domain, keyword);
            }

            DbHelper.DbContext.Database.Save(new G42GreaseSearchTrackerSearch()
            {
                Domain = domain,
                Path = RedirectHelper.GetCurrentPath(),
                Keywords = keywords,
                SearchedOn = DateTime.UtcNow
            });
        }
    }
}