using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.CommonWeb.JsonControls
{
    public class ControlModelOptionGroupWrapper : ControlModelOptionElementBase
    {
        public ControlModelOptionGroupWrapper(ControlModelOptionGroup optionGroup)
        {
            OptionGroup = optionGroup.Options;
            Label = optionGroup.Label;
        }

        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

        [JsonProperty(PropertyName = "groupOptions")]
        public IList<ControlModelOptionElement> OptionGroup { get; set; }
    }
}
