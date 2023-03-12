using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace OpenAi.Models.ViewModels
{
    public class BlockListLayoutViewModel
    {
        [JsonProperty("Umbraco.BlockList")]
        public List<UmbracoBlockList> UmbracoBlockList { get; set; } = new List<UmbracoBlockList>();
    }

    public class BlockListViewModel
    {
        [JsonProperty("layout")]
        public BlockListLayoutViewModel Layout { get; set; }
        [JsonProperty("contentData")]
        public List<JObject> ContentData { get; set; }
        [JsonProperty("settingsData")]
        public List<object> SettingsData { get; set; }

    }

    public class UmbracoBlockList
    {
        [JsonProperty("contentUdi")]
        public string ContentUdi { get; set; }
    }


}