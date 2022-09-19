using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.CommonWeb.JsonControls
{
    public class ControlModelOptionGroup 
    {
        public ControlModelOptionGroup()
        {
            Options = new List<ControlModelOptionElement>();
        }

        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

        [JsonProperty(PropertyName = "options")]
        public IList<ControlModelOptionElement> Options { get; private set; }
       
    }
}
