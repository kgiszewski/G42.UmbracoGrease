using System.Collections.Generic;
using System.Linq;
using G42.UmbracoGrease.Core;
using G42.UmbracoGrease.G42AppSettings.Models;
using Newtonsoft.Json;

namespace G42.UmbracoGrease.Shared.Models
{
    public class G42GeneralConfigModel
    {
        [JsonProperty("viewEngineEnable")]
        public bool ViewEngineEnable { get; set; }

        public G42GeneralConfigModel()
        {
            
        }

        public G42GeneralConfigModel(IEnumerable<G42AppSetting> settings)
        {
            var settingsList = settings.ToList();

            var viewEngineEnabled = settingsList.FirstOrDefault(x => x.Key == Constants.VIEW_ENGINE_ENABLE_KEY);

            if (viewEngineEnabled != null)
            {
                ViewEngineEnable = (viewEngineEnabled.Value == "1");
            }
        }
    }
}