using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.CommonWeb.JsonControls
{
    public class TableDataCellActionsModel : TableDataCellModelBase
    {
        public TableDataCellActionsModel()
        {
            Actions = new List<TableDataRowAction>();
        }

        public void AddAction(string key, string name, string url)
        {
            Actions.Add(new TableDataRowAction()
            {
                Key = key,
                Name = name,
                Url = url
            });
        }

        [JsonProperty(PropertyName = "actions")]
        public IList<TableDataRowAction> Actions { get; set; }
    }
}
