using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.CommonWeb.JsonControls
{
    public class TableDataRowModel
    {
        public TableDataRowModel ()
        {
            Cells = new List<TableDataCellModelBase>();
        }


        [JsonProperty(PropertyName = "objectId")]
        public string ObjectId { get; set; }

        [JsonProperty(PropertyName = "cells")]
        public IList<TableDataCellModelBase> Cells { get; private set; }

        public void AddTextCell(string key, string text)
        {
            Cells.Add(new TableDataCellTextModel()
            {
                Key = key, 
                Text = text
            });
        }
        public void AddUrlCell(string key, string text, string url)
        {
            Cells.Add(new TableDataCellUrlModel()
            {
                Key = key,
                Text = text,
                Href = url
            });
        }
        public void AddActionsCell(string key, TableDataCellActionsModel actions)
        {
            actions.Key = key;
            Cells.Add(actions);
        }
    }
}
