using System.Collections.Generic;

namespace SmartRegistry.Web.Models.ControlModels
{
    public class MultiSelectOption
    {
        public string GroupLabel { get { return "Javascript"; } }

        public List<Option> GroupValues { get; set; }
    }

    public class MultiSelectControl : PageControl
    {
        public List<MultiSelectOption> Options { get; set; }
    }
}
