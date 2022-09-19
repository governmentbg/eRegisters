using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.CommonWeb.JsonControls
{
    public class ControlModelMultilineText : ControlModelText
    {
        public ControlModelMultilineText()
            : base("textarea")
        {
        }

        [JsonProperty(PropertyName = "rows")]
        public int Rows { get; set; }

    }
}
