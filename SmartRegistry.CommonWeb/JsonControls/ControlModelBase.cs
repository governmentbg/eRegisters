using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.CommonWeb.JsonControls
{
    public abstract class ControlModelBase
    {
        protected ControlModelBase(string type)
        {
            Type = type;
            Enabled = true;
        }

        [JsonProperty(PropertyName = "col")]
        public int Col { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "enabled")]
        public bool Enabled { get; set; }


        [JsonProperty(PropertyName = "required")]
        public bool Required { get; set; }
    }
}
