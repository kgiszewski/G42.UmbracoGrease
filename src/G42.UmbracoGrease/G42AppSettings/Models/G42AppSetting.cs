using System;
using Newtonsoft.Json;
using Umbraco.Core.Persistence;

namespace G42.UmbracoGrease.G42AppSettings.Models
{
    [TableName("G42GreaseAppSettings")]
    [PrimaryKey("id")]
    public class G42AppSetting
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("updatedOn")]
        public DateTime UpdatedOn { get; set; }
    }
}