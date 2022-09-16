using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.CommonWeb.JsonControls
{
    public class ControlModelRepeaterItemTemplate
    {
        public ControlModelRepeaterItemTemplate()
        {
            Controls = new List<ControlModelBase>();
        }

        [JsonProperty(PropertyName = "elementId")]
        public string ElementId { get; set; }

        [JsonProperty(PropertyName = "controls")]
        public IList<ControlModelBase> Controls { get; set; }
    }
}
