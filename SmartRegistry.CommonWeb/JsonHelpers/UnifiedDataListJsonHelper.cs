using Newtonsoft.Json;
using Orak.Utils;
using SmartRegistry.CommonWeb.JsonControls;
using SmartRegistry.Domain.Common;
using SmartRegistry.Domain.Entities;
using SmartRegistry.Domain.Interfaces;
using SmartRegistry.Domain.QueryFilters;
using SmartRegistry.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.CommonWeb.JsonHelpers
{
    public class UnifiedDataListJsonHelper : BaseJsonHelper
    {
        public UnifiedDataListJsonHelper(ISmartRegistryContext context)
            : base(context)
        {

        }

        public IList<ControlModelBase> GetFilterControls()
        {
            var result = new List<ControlModelBase>();

            result.Add(new ControlModelText()
            {
                Col = 2,
                Label = Properties.Content.unified_uri,
                Name = "UriFilter",
                Value = ""
            });

            result.Add(new ControlModelText()
            {
                Col = 3,
                Label = Properties.Content.unified_filter_name,
                Name = "NameFilter",
                Value = ""
            });

            var statusFilter = new ControlModelOptionList()
            {
                Col = 3,
                Label = Properties.Content.base_status_name,
                Name = "StatusFilter"
            };
            var val1 = new ControlModelOptionElement()
            {
                Label = Properties.Content.base_status_option_0,
                Value = "0"
            };
            statusFilter.Options.Add(val1);
            statusFilter.Options.Add(new ControlModelOptionElement()
            {
                Label = Properties.Content.base_status_option_1,
                Value = "1"
            });
            statusFilter.Options.Add(new ControlModelOptionElement()
            {
                Label = Properties.Content.base_status_option_2,
                Value = "2"
            });
            statusFilter.SelectedValue = val1;
            result.Add(statusFilter);



            return result;
        }

        public TableDataModel GetUniDataTable(string relativePathBase, PagedResult<UnifiedData> uniDataResult, UnifiedDataFilter filter)
        {
            var result = new TableDataModel();
            result.CurrentPage = (int)uniDataResult.CurrentPage;
            result.NumberOfRowsPerPage = (int)uniDataResult.PageSize;
            result.TotalPages = uniDataResult.PageCount;
            result.OrderColumn = filter.OrderByColumn;
            result.OrderDirection = (filter.OrderByDirection == OrderDbEnum.Asc) ? "asc" : "desc";  

            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "URI",
                Label = Properties.Content.unified_uri,
                Sortable = true
            });
            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "Name",
                Label = Properties.Content.unified_name,
                Sortable = true
            });
            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "Description",
                Label = Properties.Content.unified_description,
                Sortable = true
            });
            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "DataTypeName",
                Label = Properties.Content.unified_type,
                Sortable = true
            });
            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "Status",
                Label = Properties.Content.base_status_name,
                Sortable = true
            });
            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "Actions",
                Label = Properties.Content.base_options,
                Sortable = false
            });

            foreach (var uniData in uniDataResult.Results)
            {
                var row = new TableDataRowModel();

                row.ObjectId = uniData.Id.ToString();
                row.AddTextCell("URI", uniData.URI);
                row.AddTextCell("Name", uniData.Name);
                row.AddTextCell("Description", uniData.Description);

                var dtName = EnumUtils.GetDisplayName(uniData.DataType);
                row.AddTextCell("DataTypeName", dtName);

                var statusStr = "Активен";
                if (!uniData.IsActive) statusStr = "Неактивен";
                row.AddTextCell("Status", statusStr);

                var actions = new TableDataCellActionsModel();

                if (SmartContext.CurrentUser.HasPermission(PermissionEnum.ManageInfoObjects))
                {
                  
                    if (uniData.DataType == UnifiedDataTypeEnum.Composite)
                    {
                        actions.AddAction("edit", Properties.Content.unified_hint_edit, relativePathBase + "UnifiedData/EditComposite/" + uniData.Id);
                    }
                    else if (uniData.DataType == UnifiedDataTypeEnum.Referential)
                    {
                        actions.AddAction("edit", Properties.Content.unified_hint_edit, relativePathBase + "UnifiedData/EditReferential/" + uniData.Id);
                    }
                    else
                    {
                    
                        actions.AddAction("edit", Properties.Content.unified_hint_edit, relativePathBase + "UnifiedData/Edit/" + uniData.Id);
                    }
                }
                row.AddActionsCell("actions", actions);

                result.Rows.Add(row);
            }

            return result;
        }

        public UnifiedDataFilter DeserializeFilters(string jsonFilters)
        {
            var uniDataFilter = new UnifiedDataFilter()
            { 
                PageNumber = 1,
                PageSize = 5
            };

            if (!string.IsNullOrEmpty(jsonFilters))
            {
                dynamic filters = JsonConvert.DeserializeObject(jsonFilters);

                foreach (var filter in filters)
                {
                    if (filter.name == "UriFilter")
                    {
                        string fltVal = filter.value;
                        if (!string.IsNullOrEmpty(fltVal))
                        {
                            uniDataFilter.URI = "%" + fltVal + "%";
                        }
                    }
                    if (filter.name == "NameFilter")
                    {
                        string fltVal = filter.value;
                        if (!string.IsNullOrEmpty(fltVal))
                        {
                            uniDataFilter.Name = "%" + fltVal + "%";
                        }
                    }
                    if (filter.name == "StatusFilter")
                    {
                        uniDataFilter.IsActive = null;
                        if (filter.selectedValue != null)
                        {
                            int statFltValue = filter.selectedValue.value;
                            if (statFltValue == 1) uniDataFilter.IsActive = true;
                            if (statFltValue == 2) uniDataFilter.IsActive = false;
                        }
                    }
                    if (filter.currentPage != null)
                    {
                        var pageVal = filter.currentPage.Value;
                        if (pageVal is string)
                        {
                            uniDataFilter.PageNumber = int.Parse(pageVal);
                        }
                        else
                        {
                            uniDataFilter.PageNumber = (int)pageVal;
                        }
                    }
                    if (filter.numberOfRowsPerPage != null)
                    {
                        var pageSizeVal = filter.numberOfRowsPerPage.Value;
                        if (pageSizeVal is string)
                        {
                            uniDataFilter.PageSize = int.Parse(pageSizeVal);
                        }
                        else
                        {
                            uniDataFilter.PageSize = (int)pageSizeVal;
                        }
                    }
                    if (filter.orderColumn != null)
                    {
                        uniDataFilter.OrderByColumn = filter.orderColumn;

                        if (filter.orderDirection == "desc")
                        {
                            uniDataFilter.OrderByDirection = OrderDbEnum.Desc;
                        }
                        else
                        {
                            uniDataFilter.OrderByDirection = OrderDbEnum.Asc;
                        }
                    }
                }
            }

            return uniDataFilter;
        }
    }
}
