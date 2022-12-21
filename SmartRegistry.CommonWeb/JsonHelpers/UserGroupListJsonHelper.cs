using Newtonsoft.Json;
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
    public class UserGroupListJsonHelper : BaseJsonHelper
    {
        public UserGroupListJsonHelper(ISmartRegistryContext context)
            : base(context)
        {

        }

        public IList<ControlModelBase> GetFilterControls()
        {
            var result = new List<ControlModelBase>();

            //result.Add(new ControlModelText()
            //{
            //    Col = 1,
            //    Label = "Административен орган",
            //    Name = "AdminBodyFilter",
            //    Value = ""
            //});
            result.Add(new ControlModelText()
            {
                Col = 3,
                Label = Properties.Content.usergroup_filter_name,
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

        public UserGroupFilter DeserializeFilters(string jsonFilters)
        {

            var userGroupFilter = new UserGroupFilter();

            if (!string.IsNullOrEmpty(jsonFilters))
            {
                dynamic filters = JsonConvert.DeserializeObject(jsonFilters);

                foreach (var filter in filters)
                {
                    if (filter.name == "NameFilter")
                    {                       
                        string fltVal = filter.value;
                        if (!string.IsNullOrEmpty(fltVal))
                        {
                            userGroupFilter.Name = "%" + fltVal + "%";
                        }
                    }
                    if (filter.name == "StatusFilter")
                    {
                        if (filter.selectedValue != null)
                        {
                            int statFltValue = filter.selectedValue.value;
                            if (statFltValue == 1) userGroupFilter.IsActive = true;
                            if (statFltValue == 2) userGroupFilter.IsActive = false;
                        }
                    }
                    AddPageAndOrderToFilter(userGroupFilter, filter);
                }
            }

            return userGroupFilter;
        }

        public TableDataModel GetUserGroupsTable(string relativePathBase, PagedResult<UserGroup> userGroupResult)
        {
            var result = new TableDataModel();
            result.CurrentPage = userGroupResult.CurrentPage;
            result.NumberOfRowsPerPage = userGroupResult.PageSize;
            result.TotalPages = userGroupResult.PageCount;

            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "UserGroupName",
                Label = Properties.Content.usergroup_name,
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


            foreach (var userGr in userGroupResult.Results)
            {
                var row = new TableDataRowModel();

                row.ObjectId = userGr.Id.ToString();
                row.AddTextCell("UserGroupName", userGr.Name);
                var statusStr = "Активен";
                if (!userGr.IsActive) statusStr = "Неактивен";
                row.AddTextCell("Status", statusStr);

                var actions = new TableDataCellActionsModel();
                actions.AddAction("edit", Properties.Content.usergroup_hint_edit, relativePathBase + "UserGroups/Edit/" + userGr.Id);
               // actions.AddAction("history", "Редакция", relativePathBase + "/UserGroups/History/" + userGr.Id);
                row.AddActionsCell("actions", actions);

                result.Rows.Add(row);
            }

            return result;
        }
    }
}
