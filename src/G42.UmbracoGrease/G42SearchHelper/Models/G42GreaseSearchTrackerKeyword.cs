using System;
using Umbraco.Core.Persistence;

namespace G42.UmbracoGrease.G42SearchHelper.Models
{
    /// <summary>
    /// Model that represents a search keyword in the DB.
    /// </summary>
    [TableName("G42GreaseSearchTrackerKeywords")]
    [PrimaryKey("id")]
    public class G42GreaseSearchTrackerKeyword
    {
        public int Id { get; set; }
        public string Domain { get; set; }
        public string Keyword { get; set; }
        public int Count { get; set; }
        public DateTime LastUsedOn { get; set; }
    }
}