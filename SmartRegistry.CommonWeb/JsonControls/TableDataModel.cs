using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.CommonWeb.JsonControls
{
    public class TableDataModel
    {
        public TableDataModel()
        {
            Columns = new List<TableColumnTitleModel>();
            Rows = new List<TableDataRowModel>();
        }

        [JsonProperty(PropertyName = "columns")]
        public IList<TableColumnTitleModel> Columns { get; private set; }

        [JsonProperty(PropertyName = "rows")]
        public IList<TableDataRowModel> Rows { get; private set; }


        [JsonProperty(PropertyName = "totalPages")]
        public int TotalPages { get; set; }

        [JsonProperty(PropertyName = "currentPage")]
        public int CurrentPage { get; set; }

        [JsonProperty(PropertyName = "numberOfRowsPerPage")]
        public int NumberOfRowsPerPage { get; set; }

        [JsonProperty(PropertyName = "orderColumn")]
        public string OrderColumn { get; set; }

        [JsonProperty(PropertyName = "orderDirection")]
        public string OrderDirection { get; set; }
    }
}
