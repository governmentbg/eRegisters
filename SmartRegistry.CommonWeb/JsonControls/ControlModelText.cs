using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.CommonWeb.JsonControls
{
    public class ControlModelText : ControlModelBase
    {
        protected ControlModelText(string type)
            :base(type)
        {

        }

        public ControlModelText()
            : base("text")
        {
        }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }
}
