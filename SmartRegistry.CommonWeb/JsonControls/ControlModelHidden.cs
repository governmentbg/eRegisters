using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.CommonWeb.JsonControls
{
    public class ControlModelHidden : ControlModelBase
    {
        public ControlModelHidden()
            : base("hidden")
        {
        }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }
}
