using System;
using System.Collections.Generic;
using G42.UmbracoGrease.Helpers;
using Umbraco.Core.Persistence;

namespace G42.UmbracoGrease.Reports.PetaPocoModels
{
    [TableName("SearchTrackerKeywords")]
    [PrimaryKey("id")]
    public class SearchTrackerKeyword
    {
        public int Id { get; set; }
        public string Domain { get; set; }
        public string Keyword { get; set; }
        public int Count { get; set; }
        public DateTime LastUsedOn { get; set; }

        internal static void Add(string domain, string keyword)
        {
            var currentKeyword =
                DbHelper.DbContext.Database.SingleOrDefault<SearchTrackerKeyword>("SELECT * FROM searchTrackerKeywords WHERE domain = @0 AND keyword = @1", domain,  keyword);

            if (currentKeyword == null)
            {
                _insert(domain, keyword);
            }
            else
            {
                currentKeyword.Count++;
                currentKeyword.LastUsedOn = DateTime.UtcNow;
                DbHelper.DbContext.Database.Save(currentKeyword);
            }
        }

        public static IEnumerable<SearchTrackerKeyword> Get(int countFilter = 1)
        {
            return
                DbHelper.DbContext.Database.Fetch<SearchTrackerKeyword>(
                    "SELECT * FROM searchTrackerKeywords WHERE count >= @0", countFilter);
        }

        private static void _insert(string domain, string keyword)
        {
            DbHelper.DbContext.Database.Save(new SearchTrackerKeyword()
            {
                Domain = domain,
                Keyword = keyword,
                Count = 1,
                LastUsedOn = DateTime.UtcNow
            });
        }
    }
}