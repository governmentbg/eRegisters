using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.CommonWeb.JsonControls
{
    public class ControlModelAjaxInput : ControlModelBase
    {
        public ControlModelAjaxInput()
            : base("input-ajax")
        {

        }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        [JsonProperty(PropertyName = "displayValue")]
        public string DisplayValue { get; set; }

        [JsonProperty(PropertyName = "ajaxUrl")]
        public string AjaxUrl { get; set; }

        [JsonProperty(PropertyName = "paramControlName")]
        public string ParamControlName { get; set; }
    }
}
