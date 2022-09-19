﻿using Newtonsoft.Json;
using Orak.Utils;
using SmartRegistry.CommonWeb.JsonControls;
using SmartRegistry.Domain.Common;
using SmartRegistry.Domain.Entities;
using SmartRegistry.Domain.Interfaces;
using SmartRegistry.Domain.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.CommonWeb.JsonHelpers
{
    public class WebServiceListJsonHelper : BaseJsonHelper
    {
        public WebServiceListJsonHelper(ISmartRegistryContext context)
            : base(context)
        {

        }

        public object GetWebServicesFilterControls()
        {
            var result = new List<ControlModelBase>();

            result.Add(new ControlModelText()
            {
                Col = 1,
                Label = "Име",
                Name = "Name",
                Value = ""
            });

            return result;
        }

        public WebServiceFilter DeserializeFilters(string jsonFilters)
        {
            var result = new WebServiceFilter();

            if (!string.IsNullOrEmpty(jsonFilters))
            {
                dynamic filters = JsonConvert.DeserializeObject(jsonFilters);

                foreach (var filter in filters)
                {
                    if (filter.name == "Name")
                    {
                        string fltVal = filter.value;
                        if (!string.IsNullOrEmpty(fltVal))
                        {
                            result.Name = "%" + fltVal + "%";
                        }
                    }

                    AddPageAndOrderToFilter(result, filter);
                }
            }

            return result;
        }

        public TableDataModel GetWebServicesTable(string relativePathBase, PagedResult<WebServiceISCIPR> webServResult, WebServiceFilter webServiceFilter)
        {
            var result = new TableDataModel();

            result.CurrentPage = webServResult.CurrentPage;
            result.NumberOfRowsPerPage = webServResult.PageSize;
            result.TotalPages = webServResult.PageCount;
            if (webServiceFilter != null)
            {
                result.OrderColumn = webServiceFilter.OrderByColumn;
                result.OrderDirection = (webServiceFilter.OrderByDirection == OrderDbEnum.Asc) ? "asc" : "desc";
            }

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
                Sortable = false
            });
            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "ServiceTypeName",
                Label = "Тип на услугата",
                Sortable = true
            });
            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "ApiName",
                Label = "Техническо име на услугата ",
                Sortable = false
            });
            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "Actions",
                Label = "Действия",
                Sortable = false
            });

            foreach (var webServ in webServResult.Results)
            {
                var row = new TableDataRowModel();

                row.ObjectId = webServ.Id.ToString();
                row.AddTextCell("Name", webServ.Name);
                row.AddTextCell("Description", webServ.Description);
                row.AddTextCell("ServiceTypeName", EnumUtils.GetDisplayName(webServ.ServiceType));

                var apiName = webServ.Register.AdministrativeBody.NamespaceApi + "." + webServ.Register.NamespaceApi + "." + webServ.ServiceKey;
                row.AddTextCell("ApiName", apiName);

                var actions = new TableDataCellActionsModel();
                actions.AddAction("edit", "Редакция", relativePathBase + "WebServices/Edit/" + webServ.Id);
                actions.AddAction("view", "Download XSD", relativePathBase + "WebServices/DownloadXSD/" + webServ.Id);
                actions.AddAction("download", "Download XSD", relativePathBase + "WebServices/DownloadXSD/" + webServ.Id);
                row.AddActionsCell("actions", actions);

                result.Rows.Add(row);
            }

            return result;
        }
    }
}
