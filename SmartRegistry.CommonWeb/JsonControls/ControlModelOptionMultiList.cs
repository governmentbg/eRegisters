using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.CommonWeb.JsonControls
{
    public class ControlModelOptionMultiList : ControlModelOptionListBase
    {
        public ControlModelOptionMultiList() : base("multiselect")
        {
         
        }

        [JsonProperty(PropertyName = "selectedValue")]
        public ControlModelOptionElement SelectedValue { get; set; }

        [JsonProperty(PropertyName = "selectedValues")]
        public IList<ControlModelOptionElement> SelectedValues { get; set; }
    }
}
