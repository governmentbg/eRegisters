using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.CommonWeb.JsonControls
{
    public class ControlModelOptionList : ControlModelOptionListBase
    {
        public ControlModelOptionList() : base("select")
        {
         
        }

        [JsonProperty(PropertyName = "selectedValue")]
        public ControlModelOptionElement SelectedValue { get; set; }
    }
}
