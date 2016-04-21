using System;
using Umbraco.Core.Persistence;

namespace G42.UmbracoGrease.G42404Helper.Models
{
    [PrimaryKey("id")]
    [TableName("G42Grease404TrackerDomainPaths")]
    public class G42Grease404DomainPath
    {
        public long Id { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime LastVisited { get; set; }
    }
}