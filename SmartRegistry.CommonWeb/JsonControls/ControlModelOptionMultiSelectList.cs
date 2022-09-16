using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.CommonWeb.JsonControls
{
    public class ControlModelOptionMultiSelectList : ControlModelOptionListBase
    {
        public ControlModelOptionMultiSelectList() : base("multiselect")
        {
        }

        [JsonProperty(PropertyName = "selectedValues")]
        public IList<ControlModelOptionElement> SelectedValues { get; set; }

    }
}
