using System.Collections.Generic;

namespace SmartRegistry.Web.Models.ControlModels
{
    public class SelectControl : PageControl
    {
        public virtual List<Option> Options { get; set; } = new List<Option>();
    }
}
