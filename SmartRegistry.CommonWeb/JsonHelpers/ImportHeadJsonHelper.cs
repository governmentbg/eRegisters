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
    public class ImportHeadJsonHelper : BaseJsonHelper
    {
        public ImportHeadJsonHelper(ISmartRegistryContext context) 
            : base(context)
        {

        }

        public IList<ControlModelBase> GetFilters()
        {
            var result = new List<ControlModelBase>();

            result.Add(new ControlModelText()
            {
                Col = 3,
                Label = "Име на регистър",
                Name = "RegisterName",
                Value = ""
            });

            result.Add(new ControlModelText()
            {
                Col = 3,
                Label = "Име на файл",
                Name = "FileName",
                Value = ""
            });


            var statusControl = new ControlModelOptionList()
            {
                Col = 3,
                Label = "Статус",
                Required = true,
                Name = "Status"
            };

            var noStatus = new ControlModelOptionElement()
            {
                Label = "--Избери--",
                Value = "-1"
            };

            var upleadedStatus = new ControlModelOptionElement()
            {
                Label = "Качен файл",
                Value = ((int)ImportHeadStatus.UploadedFile).ToString()
            };
            var errorStatus = new ControlModelOptionElement()
            {
            Label = "С грешка",
            Value = ((int)ImportHeadStatus.HasValidationErrors).ToString()
            };
            var validatedStatus = new ControlModelOptionElement()
            {
                Label = "Валидиран",
                Value = ((int)ImportHeadStatus.Validated).ToString()
            };
            var proccessedStatus = new ControlModelOptionElement()
            {
                Label = "Обработен",
                Value = ((int)ImportHeadStatus.Processed).ToString()
            };

            statusControl.Options.Add(noStatus);
            statusControl.Options.Add(upleadedStatus);
            statusControl.Options.Add(errorStatus);
            statusControl.Options.Add(validatedStatus);
            statusControl.Options.Add(proccessedStatus);
            result.Add(statusControl);

            return result;
        }

        public ImportHeadFilter DeserializeFilters(string jsonFilters)
        {
            var importFilter = new ImportHeadFilter();

            if (!string.IsNullOrEmpty(jsonFilters))
            {
                dynamic filters = JsonConvert.DeserializeObject(jsonFilters);

                foreach (var filter in filters)
                {

                    //if (filter.name == "RegisterName")
                    //{
                    //    string filterVal = filter.value;
                    //    if (!string.IsNullOrEmpty(filterVal))
                    //    {
                    //        importFilter.Register.Name = "%" + filter.value + "%";
                    //    }
                    //}
                    if (filter.name == "Status")
                    {
                        if (filter.selectedValue != null)
                        {
                            int statFltValue = filter.selectedValue.value;
                            if (statFltValue == 1) importFilter.Status = ImportHeadStatus.HasValidationErrors; 
                            if (statFltValue == 2) importFilter.Status = ImportHeadStatus.Validated;
                            if (statFltValue == 3) importFilter.Status = ImportHeadStatus.Processed;
                            if (statFltValue == 0) importFilter.Status = ImportHeadStatus.UploadedFile;
                        }

                    }

                    AddPageAndOrderToFilter(importFilter, filter);
                }
            }

            return importFilter;
        }

     
        public TableDataModel GetImportTable(string relativePathBase, PagedResult<ImportHead> importList,ImportHeadFilter arfilter)
        {
            var result = new TableDataModel();
            result.CurrentPage = 1;
            result.NumberOfRowsPerPage = 5;
            result.TotalPages = 1;

            if (arfilter.OrderByColumn != null) {
                result.OrderColumn = arfilter.OrderByColumn;
            }

            if (arfilter.OrderByDirection == OrderDbEnum.Asc) { 
             result.OrderDirection = "asc";
            }
            if (arfilter.OrderByDirection == OrderDbEnum.Desc)
            {
                result.OrderDirection = "desc";
            }


            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "RegisterName",
                Label = "Име на регистър",
                Sortable = true
            });
            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "FileName",
                Label = "Име на файл",
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
                Key = "InsDateTime",
                Label = "Дата",
                Sortable = true
            });
            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "CreatedBy",
                Label = "Създадена от",
                Sortable = true
            });
            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "Actions",
                Label = "Опции",
                Sortable = false
            });


            result.CurrentPage = (int)importList.CurrentPage;
            result.NumberOfRowsPerPage = (int)importList.PageSize;
            result.TotalPages = importList.PageCount;

            foreach (var importHead in importList.Results)
            {
                var row = new TableDataRowModel();

                row.ObjectId = importHead.Id.ToString();
                row.AddTextCell("RegisterName", importHead.Register.Name);
                row.AddTextCell("FileName", importHead.UserFileName);
                row.AddTextCell("Status", importHead.Status.ToString());
                row.AddTextCell("InsDateTime", importHead.InsDateTime.ToString());
                row.AddTextCell("CreatedBy", (importHead.CreatedBy!=null) ? importHead.CreatedBy.Name : string.Empty);

                var actions = new TableDataCellActionsModel();
                actions.AddAction("view", "Преглед", relativePathBase + "Import/Review/" + importHead.Id);
                row.AddActionsCell("actions", actions);

                result.Rows.Add(row);
            }


            return result;
        }

      



    }
}
