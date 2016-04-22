using System;
using Umbraco.Core.Persistence;

namespace G42.UmbracoGrease.G42SearchHelper.Models
{
    /// <summary>
    /// Model that represents a single search in the DB.
    /// </summary>
    [TableName("G42GreaseSearchTrackerSearches")]
    [PrimaryKey("id")]
    public class G42GreaseSearchTrackerSearch
    {
        public int Id { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
        public string Keywords { get; set; }
        public DateTime SearchedOn { get; set; }
    }
}