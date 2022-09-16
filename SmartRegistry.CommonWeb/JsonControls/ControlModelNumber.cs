using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.CommonWeb.JsonControls
{
    public class ControlModelNumber : ControlModelBase
    {
        public ControlModelNumber()
            : base("number")
        {
        }

        [JsonProperty(PropertyName = "value")]
        public decimal Value { get; set; }

        [JsonProperty(PropertyName = "max")]
        public decimal? MaxValue { get; set; }

        [JsonProperty(PropertyName = "min")]
        public decimal? MinValue { get; set; }

        [JsonProperty(PropertyName = "step")]
        public decimal? Step { get; set; }
    }
}
