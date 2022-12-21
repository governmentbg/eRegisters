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
    public class RegisterStatesJsonHelper : BaseJsonHelper
    {
        public RegisterStatesJsonHelper(ISmartRegistryContext context) 
            : base(context)
        {

        }

        public IList<ControlModelBase> GetFilters()
        {
            var result = new List<ControlModelBase>();

            result.Add(new ControlModelText()
            {
                Col = 3,
                Label = Properties.Content.state_fillter_name,
                Name = "NameFilter",
                Value = ""
            });
                     
            return result;
        }

        public RegisterStatesFilter DeserializeFilters(string jsonFilters,RegisterAttributesHead registerHead)
        {
            var statesFilter = new RegisterStatesFilter();

            if (registerHead != null) {

                statesFilter.RegisterAttributesHead = registerHead;
            }


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
                            statesFilter.Name = "%" + filter.value + "%";
                        }
                    }


                    AddPageAndOrderToFilter(statesFilter, filter);
                }
            }

            return statesFilter;
        }

     
        public TableDataModel GetStatesTable(string relativePathBase, PagedResult<RegisterState> statesList, RegisterStatesFilter stateFilter)
        {
            var result = new TableDataModel();
            result.CurrentPage = 1;
            result.NumberOfRowsPerPage = 5;
            result.TotalPages = 1;

            if (stateFilter.OrderByColumn != null) {
                result.OrderColumn = stateFilter.OrderByColumn;
            }

            if (stateFilter.OrderByDirection == OrderDbEnum.Asc) { 
             result.OrderDirection = "asc";
            }
            if (stateFilter.OrderByDirection == OrderDbEnum.Desc)
            {
                result.OrderDirection = "desc";
            }

            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "Name",
                Label = Properties.Content.state_name,
                Sortable = true
            });
            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "InitialState",
                Label = Properties.Content.state_initial,
                Sortable = true
            });           
            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "InsDateTime",
                Label = Properties.Content.state_insdatetime,
                Sortable = true
            });
            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "CreatedBy",
                Label = Properties.Content.state_createdby,
                Sortable = true
            });
            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "Actions",
                Label = Properties.Content.base_options,
                Sortable = false
            });


            result.CurrentPage = (int)statesList.CurrentPage;
            result.NumberOfRowsPerPage = (int)statesList.PageSize;
            result.TotalPages = statesList.PageCount;

            foreach (var state in statesList.Results)
            {
                var row = new TableDataRowModel();

                row.ObjectId = state.Id.ToString();
                row.AddTextCell("Name", state.Name);

                if (state.InitialState == 1)
                {                    
                    row.AddTextCell("InitialState", "Да");
                }
                else {
                    row.AddTextCell("InitialState", "Не");
                }
              
                row.AddTextCell("InsDateTime", state.InsDateTime.ToString());
                row.AddTextCell("CreatedBy", (state.CreatedBy!=null) ? state.CreatedBy.Name : string.Empty);

                var actions = new TableDataCellActionsModel();
                actions.AddAction("edit", Properties.Content.states_hint_edit, relativePathBase + "RegisterStates/Edit/" + state.Id);
                row.AddActionsCell("actions", actions);

                result.Rows.Add(row);
            }


            return result;
        }

        public IList<ControlModelBase> GetStateControlls(RegisterState regState,RegisterAttributesHead regHead)
        {
            var result = new List<ControlModelBase>();

            result.Add(new ControlModelHidden()
            {
                Col = 12,
                Label = "",
                Name = RegisterStateViewModel.JSON_NAME_ID,
                Value = (regState == null) ? string.Empty : regState.Id.ToString()
            });

            result.Add(new ControlModelHidden()
            {
                Col = 12,
                Label = "",
                Name = RegisterStateViewModel.JSON_NAME_HEADID,
                 Value = (regState == null) ? regHead.Id.ToString() : regHead.Id.ToString()

            });

            result.Add(new ControlModelText()
            {
                Col = 12,
                Label = Properties.Content.state_name,
                Name = RegisterStateViewModel.JSON_NAME_NAME,
                Value = (regState == null) ? string.Empty : regState.Name
            });



            result.Add(new ControlModelCheckbox
            {
                Col = 12,
                Label = Properties.Content.state_initial,
                Name = RegisterStateViewModel.JSON_NAME_INITIAL,
                Checked = (regState == null) ? false :  (regState.InitialState == 1) ? true : false

            });

            var repeater = new ControlModelRepeater();
            repeater.Col = 12;
            repeater.Label = Properties.Content.base_attributes;
            repeater.Name = RegisterStateViewModel.JSON_NAME_ATTRIBUTES;
            repeater.ItemTemplate = CreateStructureItemTemplate(null, regHead);
            if (regState != null)
            {
                repeater.Values = new List<ControlModelRepeaterItemTemplate>();
                foreach (var attribute in regState.AttributeList)
                {
                    var attrControls = CreateStructureItemTemplate(attribute, regHead);
                    repeater.Values.Add(attrControls);
                }
            }
            result.Add(repeater);



            return result;
        }

        private ControlModelRepeaterItemTemplate CreateStructureItemTemplate(RegisterStateAttribute attribute,RegisterAttributesHead regHead)
        {
            var result = new ControlModelRepeaterItemTemplate();

            result.ElementId = (attribute == null) ? string.Empty : attribute.Id.ToString();

            var attributeList = new ControlModelOptionList()
            {
                Col = 12,
                Label = Properties.Content.base_attribute,
                Required = true,
                Name = RegisterStateAttributeViewModel.JSON_NAME_NAME
            };

            foreach (RegisterAttribute regAttr in regHead.Attributes) {

                var attributeOption = new ControlModelOptionElement()
                {
                    Label = regAttr.Name,
                    Value = regAttr.Id.ToString()
                };

                attributeList.Options.Add(attributeOption);
            }

            if (attribute != null) {
                var activeStatus = new ControlModelOptionElement()
                {
                    Label = attribute.RegisterAttribute.Name,
                    Value = attribute.RegisterAttribute.Id.ToString()
                };

                attributeList.SelectedValue = activeStatus;
            }

            result.Controls.Add(attributeList);


            return result;
        }


        public RegisterStateViewModel ParseAreaFromJson(string json)
        {
            var result = new RegisterStateViewModel();

            dynamic areaData = JsonConvert.DeserializeObject(json);

            var idElem = areaData[0];
            if (idElem.name != RegisterStateViewModel.JSON_NAME_ID) throw new Exception("Invalid JSON Area data : missing Id as first element!");

            if (idElem.value != string.Empty)
            {
                result.Id = (int)idElem.value;
            }

            foreach (var elem in areaData)
            {
                if (elem.name == RegisterStateViewModel.JSON_NAME_HEADID)
                {
                    result.HeadId = elem.value;
                }
                if (elem.name == RegisterStateViewModel.JSON_NAME_NAME)
                {
                    result.Name = elem.value;
                }
                if (elem.name == RegisterStateViewModel.JSON_NAME_INITIAL)
                {
                    result.Initial = elem.value;
                }

                if (elem.name == RegisterStateViewModel.JSON_NAME_ATTRIBUTES)
                {
                    result.Attributes = new List<RegisterStateAttributeViewModel>();
                    foreach (var z in elem.values)
                    {
                       var attrModel = FillAreasModelFromJsonElement(z);
                       result.Attributes.Add(attrModel);
                    }
                }
            }

            return result;
        }

        private RegisterStateAttributeViewModel FillAreasModelFromJsonElement(dynamic elemAttr)
        {
            
            RegisterStateAttributeViewModel attrModel = new RegisterStateAttributeViewModel();           

            foreach (var elem in elemAttr.controls)
            {
               
                int selValue = elem.selectedValue.value;
                attrModel.Id = selValue;
              
            }

            return attrModel;
        }

    }
}
