using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.Web.Models.ControlModels
{
    public class PageControlsModel
    {
        public int Id { get; set; }
        public List<PageControl> Fields { get; set; } = new List<PageControl>();
    }
}
