using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.CommonWeb.JsonControls
{
    public class ControlModelCheckbox : ControlModelBase
    {
        public ControlModelCheckbox()
            : base("checkbox")
        {

        }

        [JsonProperty(PropertyName = "value")]
        public bool Checked { get; set; }

    }
}
