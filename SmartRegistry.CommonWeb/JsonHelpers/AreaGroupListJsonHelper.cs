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
    public class AreaGroupListJsonHelper : BaseJsonHelper
    {
        public AreaGroupListJsonHelper(ISmartRegistryContext context) 
            : base(context)
        {

        }

        public IList<ControlModelBase> GetFilters()
        {
            var result = new List<ControlModelBase>();

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
                Label = "Описание",
                Name = "DescFilter",
                Value = ""
            });

            return result;
        }

        public AreaGroupFilter DeserializeFilters(string jsonFilters)
        {
            var areaFilter = new AreaGroupFilter();

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
                            areaFilter.Name = "%" + filter.value + "%";
                        }
                    }

                    if (filter.name == "DescFilter")
                    {
                        string filterVal = filter.value;
                        if (!string.IsNullOrEmpty(filterVal))
                        {
                            areaFilter.DescFilter = "%" + filter.value + "%";
                        }
                    }

                    AddPageAndOrderToFilter(areaFilter, filter);
                }
            }

            return areaFilter;
        }

     
        public TableDataModel GetAreaTable(string relativePathBase, PagedResult<AreaGroup> areaList,AreaGroupFilter arfilter)
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
            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "SubAreas",
                Label = "Подтема",
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


            result.CurrentPage = (int)areaList.CurrentPage;
            result.NumberOfRowsPerPage = (int)areaList.PageSize;
            result.TotalPages = areaList.PageCount;

            foreach (var area in areaList.Results)
            {
                var row = new TableDataRowModel();

                string areas = string.Empty;

                foreach (Area ar in area.AreaList)
                {
                    areas = areas + ar.Name + " <br> ";
                }
                

                row.ObjectId = area.Id.ToString();
                row.AddTextCell("Name", area.Name);
                row.AddTextCell("Description", area.Description);
                row.AddTextCell("SubAreas", areas);
                row.AddTextCell("InsDateTime", area.InsDateTime.ToString());
                row.AddTextCell("CreatedBy", (area.CreatedBy!=null) ? area.CreatedBy.Name : string.Empty);

                var actions = new TableDataCellActionsModel();
                actions.AddAction("edit", "Редакция", relativePathBase + "Area/Edit/" + area.Id);
                row.AddActionsCell("actions", actions);

                result.Rows.Add(row);
            }


            return result;
        }

        public IList<ControlModelBase> GetAreaControlls(AreaGroup areaGrp)
        {
            var result = new List<ControlModelBase>();

            result.Add(new ControlModelHidden()
            {
                Col = 12,
                Label = "",
                Name = AreaViewGroupModel.JSON_NAME_ID,
                Value = (areaGrp == null) ? string.Empty : areaGrp.Id.ToString()
            });

            result.Add(new ControlModelText()
            {
                Col = 12,
                Label = "Име",
                Name = AreaViewGroupModel.JSON_NAME_NAME,
                Value = (areaGrp == null) ? string.Empty : areaGrp.Name
            });

            result.Add(new ControlModelText()
            {
                Col = 12,
                Label = "Описание",
                Name = AreaViewGroupModel.JSON_NAME_DESCRIPTION,
                Value = (areaGrp == null) ? string.Empty : areaGrp.Description
            });

            var repeater = new ControlModelRepeater();
            repeater.Col = 12;
            repeater.Label = "Под теми";
            repeater.Name = AreaViewGroupModel.JSON_NAME_AREAS;
            repeater.ItemTemplate = CreateStructureItemTemplate(null);
            if (areaGrp != null)
            {
                repeater.Values = new List<ControlModelRepeaterItemTemplate>();
                foreach (var area in areaGrp.AreaList)
                {
                    var attrControls = CreateStructureItemTemplate(area);
                    repeater.Values.Add(attrControls);
                }
            }
            result.Add(repeater);



            return result;
        }

        private ControlModelRepeaterItemTemplate CreateStructureItemTemplate(Area attribute)
        {
            var result = new ControlModelRepeaterItemTemplate();

            result.ElementId = (attribute == null) ? string.Empty : attribute.Id.ToString();

            result.Controls.Add(new ControlModelText()
            {
                Col = 12,
                Label = "Наименование",
                Name = AreaViewGroupModel.JSON_NAME_NAME,
                Value = (attribute == null) ? string.Empty : attribute.Name
            });

            result.Controls.Add(new ControlModelText()
            {
                Col = 12,
                Label = "Описание",
                Name = AreaViewGroupModel.JSON_NAME_DESCRIPTION,
                Value = (attribute == null) ? string.Empty : attribute.Description
            });


            return result;
        }


        public AreaViewGroupModel ParseAreaFromJson(string json)
        {
            var result = new AreaViewGroupModel();

            dynamic areaData = JsonConvert.DeserializeObject(json);

            var idElem = areaData[0];
            if (idElem.name != AreaViewGroupModel.JSON_NAME_ID) throw new Exception("Invalid JSON Area data : missing Id as first element!");

            if (idElem.value != string.Empty)
            {
                result.Id = (int)idElem.value;
            }

            foreach (var elem in areaData)
            {
                
                if (elem.name == AreaViewGroupModel.JSON_NAME_NAME)
                {
                    result.Name = elem.value;
                }
                if (elem.name == AreaViewGroupModel.JSON_NAME_DESCRIPTION)
                {
                    result.Description = elem.value;
                }

                if (elem.name == AreaViewGroupModel.JSON_NAME_AREAS)
                {
                    result.Areas = new List<AreaViewModel>();
                    foreach (var z in elem.values)
                    {
                        AreaViewModel areaModel = FillAreasModelFromJsonElement(z);
                        result.Areas.Add(areaModel);
                    }
                }
            }

            return result;
        }

        private AreaViewModel FillAreasModelFromJsonElement(dynamic elemAttr)
        {
            AreaViewModel attrModel = new AreaViewModel();

            if ((elemAttr.elementId != null) && (elemAttr.elementId != string.Empty))
            {
                attrModel.Id = DynamicToInt(elemAttr.elementId);
            }
            
            foreach (var elem in elemAttr.controls)
            {              
                    if (elem.name == AreaViewModel.JSON_NAME_NAME)
                    {
                        attrModel.Name = elem.value;
                    }

                    if (elem.name == AreaViewModel.JSON_NAME_DESCRIPTION)
                    {
                        attrModel.Description = elem.value;
                    }
            }

            return attrModel;
        }

    }
}
