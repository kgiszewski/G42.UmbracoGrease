using System.Collections.Generic;
using System.Linq;
using G42.UmbracoGrease.Core;
using G42.UmbracoGrease.G42AppSettings.Models;
using Newtonsoft.Json;

namespace G42.UmbracoGrease.G42404Helper.Models
{
    public class G42Grease404ConfigModel
    {
        [JsonProperty("daysToRetain")]
        public int DaysToRetain { get; set; }

        public G42Grease404ConfigModel()
        {
            
        }

        public G42Grease404ConfigModel(IEnumerable<G42AppSetting> settings)
        {
            var settingsList = settings.ToList();

            var reportingInterval = settingsList.FirstOrDefault(x => x.Key == Constants._404_TRACKER_DEFAULT_DAYS_TO_RETAIN_KEY);

            if (reportingInterval != null)
            {
                int reportingIntervalValue;

                int.TryParse(reportingInterval.Value, out reportingIntervalValue);

                DaysToRetain = reportingIntervalValue;
            }
        }
    }
}