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
    public class UserListJsonHelper : BaseJsonHelper
    {
        ISmartRegistryContext smartContext;

        public UserListJsonHelper(ISmartRegistryContext context)
            : base(context)
        {
            smartContext = context;
        }

        public IList<ControlModelBase> GetUsersFilterControls()
        {
            var result = new List<ControlModelBase>();

            result.Add(new ControlModelText()
            {
                Col = 3,
                Label = "Име",
                Name = "NameFilter",
                Value = ""
            });

            result.Add(new ControlModelText()
            {
                Col = 3,
                Label = "Групи",
                Name = "GroupFilter",
                Value = ""
            });

            AddStatusControl(result, null);

            return result;
        }

        public UserFilter DeserializeFilters(string jsonFilters)
        {
            var result = new UserFilter();

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
                            result.Name = "%" + fltVal + "%";
                        }
                    }
                    if (filter.name == "GroupFilter")
                    {
                        string fltVal = filter.value;
                        if (!string.IsNullOrEmpty(fltVal))
                        {
                            result.GroupName = "%" + fltVal + "%";
                        }
                    }
                    bool? statusFlt = CheckForStatusValue(filter);
                    if (statusFlt != null) result.IsActive = statusFlt;

                    AddPageAndOrderToFilter(result, filter);
                }
            }

            //if (!smartContext.CurrentUser.IsGlobalAdmin)
            //{
            //    result.AdminBody = smartContext.CurrentAdminBody;
            //}


            return result;
        }

        public TableDataModel GetUsersTable(string relativePathBase, PagedResult<User> usersResult)
        {
            var result = new TableDataModel();
            result.CurrentPage = usersResult.CurrentPage;
            result.NumberOfRowsPerPage = usersResult.PageSize;
            result.TotalPages = usersResult.PageCount;

            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "UserName",
                Label = "Име",
                Sortable = true
            });
            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "Groups",
                Label = "Групи",
                Sortable = true
            });
            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "Status",
                Label = "Статус",
                Sortable = true
            });
            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "Actions",
                Label = "Опции",
                Sortable = false
            });

            foreach (var user in usersResult.Results)
            {
                string groups = string.Empty;

                foreach (UserGroup ug in user.UserGroups)
                {
                    groups = groups + ug.Name + " <br> ";
                }

                var row = new TableDataRowModel();

                row.ObjectId = user.Id.ToString();
                row.AddTextCell("UserName", user.Name);
                var statusStr = "Активен";
                if (!user.IsActive) statusStr = "Неактивен";
                row.AddTextCell("Groups", groups);

                row.AddTextCell("Status", statusStr);

                var actions = new TableDataCellActionsModel();
                actions.AddAction("edit", "Редакция", relativePathBase + "Users/Edit/" + user.Id);
                actions.AddAction("resetPassword", "Смяна Автентикация", relativePathBase + "Users/Reset/" + user.Id);
                row.AddActionsCell("actions", actions);

                result.Rows.Add(row);
            }

            return result;
        }
    }
}
