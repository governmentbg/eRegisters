using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.CommonWeb.JsonControls
{
    public class AjaxInputResultObject
    {
        public AjaxInputResultObject(string value, string displayValue)
        {
            Value = value;
            DisplayValue = displayValue;
        }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        [JsonProperty(PropertyName = "displayValue")]
        public string DisplayValue { get; set; }
}
}
