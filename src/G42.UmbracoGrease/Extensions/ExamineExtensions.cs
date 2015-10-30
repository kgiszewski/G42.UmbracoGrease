using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Examine;
using Umbraco.Core.Logging;

namespace G42.UmbracoGrease.Extensions
{
    /// <summary>
    /// Extensions for Examine.
    /// </summary>
    public static class ExamineExtensions
    {
        /// <summary>
        /// Gets an Exmaine result by Umbraco node Id.
        /// </summary>
        /// <param name="examineManager">The examine manager.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="searchProviderCollection">The search provider collection.</param>
        /// <returns></returns>
        public static SearchResult GetResultById(this ExamineManager examineManager, int id, string searchProviderCollection)
        {
            var searcher = ExamineManager.Instance.SearchProviderCollection[searchProviderCollection];
            var searchCriteria = searcher.CreateSearchCriteria(Examine.SearchCriteria.BooleanOperation.Or);

            var query = searchCriteria.RawQuery(string.Format("+__NodeId: {0}", id));

            return searcher.Search(query).FirstOrDefault();
        }

        /// <summary>
        /// Gets the results by raw query.
        /// </summary>
        /// <param name="examineManager">The examine manager.</param>
        /// <param name="examineQuery">The examine query.</param>
        /// <param name="searchProviderCollection">The search provider collection.</param>
        /// <returns></returns>
        public static IEnumerable<SearchResult> GetResultsByRawQuery(this ExamineManager examineManager, string examineQuery, string searchProviderCollection)
        {
            var searcher = ExamineManager.Instance.SearchProviderCollection[searchProviderCollection];
            var searchCriteria = searcher.CreateSearchCriteria(Examine.SearchCriteria.BooleanOperation.Or);

            //LogHelper.Info<string>(examineQuery);

            var query = searchCriteria.RawQuery(examineQuery);

            return searcher.Search(query);
        }

        /// <summary>
        /// Converts a string to an Examine friendly string of alphanumeric, space and dash only. Optionally pass a replacement character.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="replacementCharacter">The replacement character.</param>
        /// <returns></returns>
        public static string ToExamineFriendly(this string input, string replacementCharacter = " ")
        {
            var rgx = new Regex("[^a-zA-Z0-9 -]");
            return rgx.Replace(input, replacementCharacter);
        }
    }
}