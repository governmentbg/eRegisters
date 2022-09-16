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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.CommonWeb.JsonHelpers
{
    public class SystemLogListJsonHelper : BaseJsonHelper
    {
        ISmartRegistryContext smartContext;

        public SystemLogListJsonHelper(ISmartRegistryContext context) 
            : base(context)
        {
            smartContext = context;
        }

        public IList<ControlModelBase> GetFilters()
        {
            var result = new List<ControlModelBase>();

            result.Add(new ControlModelText()
            {
                Col = 3,
                Label = "Потребител",
                Name = "NameFilter",
                Value = ""
            });

            if (smartContext.CurrentUser.IsGlobalAdmin) {
                result.Add(new ControlModelText()
                {
                    Col = 3,
                    Label = "Административен орган",
                    Name = "AdministrativeBody",
                    Value = ""
                });

            }
           

            AddTypeControl(result, null);

            return result;
        }

        protected void AddTypeControl(IList<ControlModelBase> listControls, bool? isActive)
        {
            var statusControl = new ControlModelOptionList()
            {
                Col = 3,
                Label = "Тип",
                Name = "Type"
            };

            var noStatus = new ControlModelOptionElement()
            {
                Label = "--Избери--",
                Value = "0"
            };

            statusControl.Options.Add(noStatus);
            statusControl.SelectedValue = noStatus;


            var orderedDataTypes = EnumUtils.GetValues<SystemLogEventTypeEnum>().OrderBy(x => EnumUtils.GetDisplayName(x));
            foreach (var dataType in orderedDataTypes)
            {
                var dataTypeOption = new ControlModelOptionElement()
                {
                    Label = EnumUtils.GetDisplayName(dataType),
                    Value = ((int)dataType).ToString()
                };
                statusControl.Options.Add(dataTypeOption);
            }

            listControls.Add(statusControl);
        }



        public SystemLogFilter DeserializeFilters(string jsonFilters)
        {
            var sysLogFilter = new SystemLogFilter();

            if (!string.IsNullOrEmpty(jsonFilters))
            {
                dynamic filters = JsonConvert.DeserializeObject(jsonFilters);

                foreach (var filter in filters)
                {
                
                    if (filter.name == "NameFilter")
                    {
                        string filterVal = filter.value;
                        if (!string.IsNullOrEmpty(filterVal))
                        {
                            sysLogFilter.Name = "%" + filter.value + "%";
                        }
                    }

                    if (filter.name == "AdminBody")
                    {
                        string filterVal = filter.value;
                        if (!string.IsNullOrEmpty(filterVal))
                        {
                            sysLogFilter.AdminBodyName = "%" + filter.value + "%";
                        }
                    }


                    if (filter.name == "Type")
                    {
                        if (filter.selectedValue != null)
                        {
                            int statFltValue = filter.selectedValue.value;
                            sysLogFilter.Type = (SystemLogEventTypeEnum)statFltValue;
                        }
                    }

                    AddPageAndOrderToFilter(sysLogFilter, filter);
                }
            }

            return sysLogFilter;
        }

     
        public TableDataModel GetSystemLogTable(string relativePathBase, PagedResult<SystemLogEvent> systemlogs, SystemLogFilter logFilter)
        {
            var result = new TableDataModel();
            result.CurrentPage = 1;
            result.NumberOfRowsPerPage = 5;
            result.TotalPages = 1;

            if (logFilter.OrderByColumn != null)
            {
                result.OrderColumn = logFilter.OrderByColumn;
            }

            if (logFilter.OrderByDirection == OrderDbEnum.Asc)
            {
                result.OrderDirection = "asc";
            }
            if (logFilter.OrderByDirection == OrderDbEnum.Desc)
            {
                result.OrderDirection = "desc";
            }

            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "InfoMessage",
                Label = "Инфо",
                Sortable = false
            });
            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "Type",
                Label = "Тип",
                Sortable = true
            });
            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "EventTime",
                Label = "Дата",
                Sortable = true
            });
            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "CurrentUser",
                Label = "Потребител",
                Sortable = true
            });
            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "AdministrativeBody",
                Label = "Административен орган",
                Sortable = true
            });

            result.CurrentPage = (int)systemlogs.CurrentPage;
            result.NumberOfRowsPerPage = (int)systemlogs.PageSize;
            result.TotalPages = systemlogs.PageCount;

            foreach (var sysLog in systemlogs.Results)
            {
                var row = new TableDataRowModel();

                row.ObjectId = sysLog.Id.ToString();
                row.AddTextCell("InfoMessage", sysLog.InfoMessage);
                row.AddTextCell("Type", EnumUtils.GetDisplayName(sysLog.Type));
                row.AddTextCell("EventTime", sysLog.EventTime.ToString());
                row.AddTextCell("CurrentUser", (sysLog.CurrentUser!=null) ? sysLog.CurrentUser.Name : string.Empty);
                row.AddTextCell("AdministrativeBody", (sysLog.AdministrativeBody != null) ? sysLog.AdministrativeBody.Name : string.Empty);

                result.Rows.Add(row);
            }

            return result;
        }

    }
}
