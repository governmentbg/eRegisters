using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.CommonWeb.JsonControls
{
    public class ControlModelRepeater : ControlModelBase
    {
        public ControlModelRepeater() : base("repeater")
        {
            Values = new List<ControlModelRepeaterItemTemplate>();
        }

        [JsonProperty(PropertyName = "itemTemplate")]
        public ControlModelRepeaterItemTemplate ItemTemplate { get; set; }

        [JsonProperty(PropertyName = "values")]
        public IList<ControlModelRepeaterItemTemplate> Values { get; set; }
    }
}
