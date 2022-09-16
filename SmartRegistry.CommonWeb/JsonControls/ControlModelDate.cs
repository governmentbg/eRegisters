using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.CommonWeb.JsonControls
{
    public class ControlModelDate : ControlModelBase
    {
        public ControlModelDate()
            : base("date")
        {
        }

        [JsonProperty(PropertyName = "value")]
        [JsonConverter(typeof(CustomDateJsonConverter))]
        public DateTime? Value { get; set; }

    }

    public class CustomDateJsonConverter : IsoDateTimeConverter
    {
        public CustomDateJsonConverter()
        {
            base.DateTimeFormat = "yyyy-MM-dd";
        }
    }
}
