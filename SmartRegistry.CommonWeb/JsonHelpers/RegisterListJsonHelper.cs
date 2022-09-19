﻿using Newtonsoft.Json;
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
using System.Web;

namespace SmartRegistry.CommonWeb.JsonHelpers
{
    public class RegisterListJsonHelper : BaseJsonHelper
    {
        public RegisterListJsonHelper(ISmartRegistryContext context) 
            : base(context)
        {

        }

        public IList<ControlModelBase> GetFilters(RegisterListType listType)
        {
            var result = new List<ControlModelBase>();

            result.Add(new ControlModelText()
            {
                Col = 2,
                Label = "УРИ",
                Name = "UriFilter",
                Value = ""
            });

            result.Add(new ControlModelText()
            {
                Col = 3,
                Label = "Наименование",
                Name = "NameFilter",
                Value = ""
            });

            result.Add(new ControlModelText()
            {
                Col = 3,
                Label = "Административен орган",
                Name = "AdminBodyFilter",
                Value = ""
            });

            result.Add(new ControlModelText()
            {
                Col = 3,
                Label = "Таксономия",
                Name = "RegistersArea",
                Value = ""
            });

            if (listType != RegisterListType.Public)
            {
                AddStatusControl(result, null);
            }

            return result;
        }


        public IList<ControlModelBase> GetFiltersShort()
        {
            var result = new List<ControlModelBase>();

            result.Add(new ControlModelText()
            {
                Col = 3,
                Label = "Наименование",
                Name = "NameFilter",
                Value = ""
            });

            return result;
        }

        public RegisterFilter DeserializeFilters(string jsonFilters)
        {
            var registerFilter = new RegisterFilter();

            if (!string.IsNullOrEmpty(jsonFilters))
            {
                dynamic filters = JsonConvert.DeserializeObject(jsonFilters);

                foreach (var filter in filters)
                {
                    if (filter.name == "UriFilter")
                    {
                        string filterVal = filter.value;
                        if (!string.IsNullOrEmpty(filterVal))
                        {
                            registerFilter.Uri = "%" + filter.value + "%";
                        }
                    }
                    if (filter.name == "NameFilter")
                    {
                        string filterVal = filter.value;
                        if (!string.IsNullOrEmpty(filterVal))
                        {
                            registerFilter.Name = "%" + filter.value + "%";
                        }
                    }
                    if (filter.name == "AdminBodyFilter")
                    {
                        string filterVal = filter.value;
                        if (!string.IsNullOrEmpty(filterVal))
                        {
                            registerFilter.AdminBodyName = "%" + filter.value + "%";
                        }
                    }
                    if (filter.name == "RegistersArea")
                    {
                        string filterVal = filter.value;
                        if (!string.IsNullOrEmpty(filterVal))
                        {
                            registerFilter.RegistersArea = "%" + filter.value + "%";
                        }
                    }

                    bool? statusFlt = CheckForStatusValue(filter);
                    if (statusFlt != null) registerFilter.IsActive = statusFlt;

                    AddPageAndOrderToFilter(registerFilter, filter);
                }
            }

          
            return registerFilter;
        }

        public TableDataModel GetRegistersTable(string relativePathBase, PagedResult<Register> registers, RegisterListType listType, RegisterFilter regFilter)
        {
            var result = new TableDataModel();
            result.CurrentPage = 1;
            result.NumberOfRowsPerPage = 5;
            result.TotalPages = 1;

            if (regFilter.OrderByColumn != null)
            {
                result.OrderColumn = regFilter.OrderByColumn;
            }

            if (regFilter.OrderByDirection == OrderDbEnum.Asc)
            {
                result.OrderDirection = "asc";
            }
            if (regFilter.OrderByDirection == OrderDbEnum.Desc)
            {
                result.OrderDirection = "desc";
            }

            if (result.OrderColumn == "IsActive")
            {
                result.OrderColumn = "Status";
            }

            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "URI",
                Label = "УРИ",
                Sortable = true
            });
            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "Name",
                Label = "Наименование",
                Sortable = true
            });
            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "AdministrativeBody",
                Label = "Административен орган (ПАД)",
                Sortable = true
            });
            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "Area",
                Label = "Таксономия",
                Sortable = false
            });
            if (listType != RegisterListType.Public)
            {
                result.Columns.Add(new TableColumnTitleModel()
                {
                    Key = "URL",
                    Label = "URL",
                    Sortable = false
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
            }

            result.CurrentPage = (int)registers.CurrentPage;
            result.NumberOfRowsPerPage = (int)registers.PageSize;
            result.TotalPages = registers.PageCount;

            foreach (var register in registers.Results)
            {
                var row = new TableDataRowModel();

                row.ObjectId = register.Id.ToString();
                row.AddTextCell("URI", register.URI);
                row.AddTextCell("Name", register.Name);
                row.AddTextCell("AdministrativeBody", register.AdministrativeBody.Name);
                row.AddTextCell("URL", register.UrlAddress);

                var statusStr = "Активен";
                if (!register.IsActive) statusStr = "Неактивен";
                row.AddTextCell("Status", statusStr);

                string area = string.Empty;

                foreach (RegisterArea ar in register.RegisterAreas) {                  
                    area = area + ar.Area.AreaGroup.Name + "/" + ar.Area.Name  +" <br> "; 
                }
                row.AddTextCell("Area", area);

                var actions = new TableDataCellActionsModel();

                var registerService = SmartContext.RegistersService;
                var attributesToList = registerService.GetAttributesToList((int)register.Id);
             

                switch (listType)
                {
                    case RegisterListType.List :
                        actions.AddAction("edit", "Редакция", relativePathBase + "Registers/Edit/" + register.Id);

                        if ((SmartContext.CurrentUser != null)
                            && (SmartContext.CurrentUser.HasRegisterRight(register.Id, RegisterPermissionEnum.ManageRegisterRights)))
                        {
                            actions.AddAction("rights", "Права", relativePathBase + "Registers/RegisterRights/" + register.Id);
                        }
                        if ((SmartContext.CurrentUser != null) && (
                            (SmartContext.CurrentUser.HasRegisterRight(register.Id, RegisterPermissionEnum.CreateRecords))
                            || (SmartContext.CurrentUser.HasRegisterRight(register.Id, RegisterPermissionEnum.ChangeRecords))
                            || (SmartContext.CurrentUser.HasRegisterRight(register.Id, RegisterPermissionEnum.DeleteRecords))
                            )&& attributesToList!=null)
                        {                           
                            actions.AddAction("view", "Записи в регистър", relativePathBase + "RegisterRecords/Index/" + register.Id);                           
                        }

                        break;
                    case RegisterListType.Structure:
                        actions.AddAction("edit", "Структура", relativePathBase + "Registers/EditStructure/" + register.Id);
                        actions.AddAction("view", "Уеб услуги", relativePathBase + "WebServices/List/" + register.Id);
                        actions.AddAction("webService", "Уеб услуги", relativePathBase + "WebServices/List/" + register.Id);
                        break;
                }
                row.AddActionsCell("actions", actions);

                result.Rows.Add(row);
            }

            return result;
        }

        public TableDataModel GetRegistersTablePublic(string relativePathBase, PagedResult<Register> registers)
        {
            var result = new TableDataModel();
            result.CurrentPage = 1;
            result.NumberOfRowsPerPage = 5;
            result.TotalPages = 1;

            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "Name",
                Label = "Име",
                Sortable = true
            });
            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "Description",
                Label = "Описание",
                Sortable = true
            });
        

            result.CurrentPage = (int)registers.CurrentPage;
            result.NumberOfRowsPerPage = (int)registers.PageSize;
            result.TotalPages = registers.PageCount;

            foreach (var register in registers.Results)
            {
                var row = new TableDataRowModel();
               
                row.ObjectId = register.Id.ToString();

                string baseUrl3 = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path);
                var newUrl = baseUrl3.Remove(baseUrl3.Length - 8);
                newUrl = newUrl + "data/"+ register.Id.ToString();

                row.AddUrlCell("Name", register.Name, newUrl);
                row.AddTextCell("Description", register.Name);


                result.Rows.Add(row);
            }

            return result;
        }


    }
}