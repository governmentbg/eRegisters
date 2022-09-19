using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.CommonWeb.JsonControls
{
    public abstract class TableDataCellModelBase
    {
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }

    }
}
