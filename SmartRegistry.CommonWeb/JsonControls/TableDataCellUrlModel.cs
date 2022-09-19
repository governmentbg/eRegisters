using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.CommonWeb.JsonControls
{
    public class TableDataCellUrlModel : TableDataCellModelBase
    {
        [JsonProperty(PropertyName = "link")]
        public string Type { get { return "link"; } }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "href")]
        public string Href { get; set; }
    }
}
