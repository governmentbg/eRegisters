using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.CommonWeb.JsonControls
{
    public abstract class ControlModelOptionListBase : ControlModelBase
    {
        public ControlModelOptionListBase(string type) : base(type)
        {
            Options = new List<ControlModelOptionElementBase>();
        }

        [JsonProperty(PropertyName = "options")]
        public IList<ControlModelOptionElementBase> Options { get; set; }

        public void AddOptionGroup(ControlModelOptionGroup optGroup)
        {
            Options.Add(new ControlModelOptionGroupWrapper(optGroup));
        }

    }
}
