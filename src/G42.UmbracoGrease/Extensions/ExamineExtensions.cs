using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Examine;
using Umbraco.Core.Logging;

namespace G42.UmbracoGrease.Extensions
{
    public static class ExamineExtensions
    {
        public static SearchResult GetResultById(this ExamineManager examineManager, int id, string searchProviderCollection)
        {
            var searcher = ExamineManager.Instance.SearchProviderCollection[searchProviderCollection];
            var searchCriteria = searcher.CreateSearchCriteria(Examine.SearchCriteria.BooleanOperation.Or);

            var query = searchCriteria.RawQuery(string.Format("+__NodeId: {0}", id));

            return searcher.Search(query).FirstOrDefault();
        }

        public static IEnumerable<SearchResult> GetResultsByRawQuery(this ExamineManager examineManager, string examineQuery, string searchProviderCollection)
        {
            var searcher = ExamineManager.Instance.SearchProviderCollection[searchProviderCollection];
            var searchCriteria = searcher.CreateSearchCriteria(Examine.SearchCriteria.BooleanOperation.Or);

            LogHelper.Info<string>(examineQuery);

            var query = searchCriteria.RawQuery(examineQuery);

            return searcher.Search(query);
        }

        public static string ToExamineFriendly(this string input, string replacementCharacter = " ")
        {
            var rgx = new Regex("[^a-zA-Z0-9 -]");
            return rgx.Replace(input, replacementCharacter);
        }
    }
}