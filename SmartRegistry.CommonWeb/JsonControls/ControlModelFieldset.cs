using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.CommonWeb.JsonControls
{
    public class ControlModelFieldset : ControlModelBase
    {
        public ControlModelFieldset()
            : base("fieldset")
        {
        }

        [JsonProperty(PropertyName = "controls")]
        public List<ControlModelBase> Controls { get; set; }

    }
}
