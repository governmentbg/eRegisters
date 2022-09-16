using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.CommonWeb.JsonControls
{
    public class TableDataCellTextModel : TableDataCellModelBase
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
    }
}
