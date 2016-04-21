using System;
using Umbraco.Core.Persistence;

namespace G42.UmbracoGrease.G42404Helper.Models
{
    /// <summary>
    /// Model that represents the 404 data stored in the DB.
    /// </summary>
    [PrimaryKey("id")]
    [TableName("G42Grease404Tracker")]
    public class G42Grease404Tracker
    {
        public long Id { get; set; }
        public long DomainPathId { get; set; }
        public string Referrer { get; set; }
        public string UserAgent { get; set; }
        public DateTime AddedOn { get; set; }
        public string IpAddress { get; set; }
    }
}