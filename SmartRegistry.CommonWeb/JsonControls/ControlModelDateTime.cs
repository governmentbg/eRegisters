using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.CommonWeb.JsonControls
{
    public class ControlModelDateTime : ControlModelBase
    {
        public ControlModelDateTime()
            : base("datetime")
        {
        }

        [JsonProperty(PropertyName = "value")]
        public DateTime? Value { get; set; }
    }
}
