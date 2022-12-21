using Newtonsoft.Json;
using SmartRegistry.CommonWeb.JsonControls;
using SmartRegistry.Domain.Common;
using SmartRegistry.Domain.Entities;
using SmartRegistry.Domain.Interfaces;
using SmartRegistry.Domain.QueryFilters;
using SmartRegistry.Domain.Services;
using SmartRegistry.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.CommonWeb.JsonHelpers
{
    public class RegisterTransitionJsonHelper : BaseJsonHelper
    {
        public RegisterTransitionJsonHelper(ISmartRegistryContext context) 
            : base(context)
        {

        }

        public IList<ControlModelBase> GetFilters()
        {
            var result = new List<ControlModelBase>();

            result.Add(new ControlModelText()
            {
                Col = 3,
                Label = Properties.Content.transition_start,
                Name = "StartState",
                Value = ""
            });
            result.Add(new ControlModelText()
            {
                Col = 3,
                Label = Properties.Content.transition_end,
                Name = "EndState",
                Value = ""
            });

            return result;
        }

        public RegisterTransitionFilter DeserializeFilters(string jsonFilters,RegisterAttributesHead registerHead)
        {
            var transFilter = new RegisterTransitionFilter();

            if (registerHead != null) {

                transFilter.RegisterAttributesHead = registerHead;
            }


            if (!string.IsNullOrEmpty(jsonFilters))
            {
                dynamic filters = JsonConvert.DeserializeObject(jsonFilters);

                foreach (var filter in filters)
                {
                
                    if (filter.name == "StartState")
                    {
                        string filterVal = filter.value;
                        if (!string.IsNullOrEmpty(filterVal))
                        {
                            transFilter.StartState = "%" + filter.value + "%";
                        }
                    }
                    if (filter.name == "EndState")
                    {
                        string filterVal = filter.value;
                        if (!string.IsNullOrEmpty(filterVal))
                        {
                            transFilter.EndState = "%" + filter.value + "%";
                        }
                    }

                    AddPageAndOrderToFilter(transFilter, filter);
                }
            }

            return transFilter;
        }

     
        public TableDataModel GetStatesTable(string relativePathBase, PagedResult<RegisterTransition> transList, RegisterTransitionFilter transFilter)
        {
            var result = new TableDataModel();
            result.CurrentPage = 1;
            result.NumberOfRowsPerPage = 5;
            result.TotalPages = 1;

            if (transFilter.OrderByColumn != null) {
                result.OrderColumn = transFilter.OrderByColumn;
            }

            if (transFilter.OrderByDirection == OrderDbEnum.Asc) { 
             result.OrderDirection = "asc";
            }
            if (transFilter.OrderByDirection == OrderDbEnum.Desc)
            {
                result.OrderDirection = "desc";
            }
            
            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "StartState",
                Label = Properties.Content.transition_start,
                Sortable = true
            });           
            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "EndState",
                Label = Properties.Content.transition_end,
                Sortable = true
            });
            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "GroupRights",
                Label = Properties.Content.transition_rights,
                Sortable = true
            });
            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "Actions",
                Label = Properties.Content.base_options,
                Sortable = false
            });


            result.CurrentPage = (int)transList.CurrentPage;
            result.NumberOfRowsPerPage = (int)transList.PageSize;
            result.TotalPages = transList.PageCount;

            foreach (var transition in transList.Results)
            {
                var row = new TableDataRowModel();

                row.ObjectId = transition.Id.ToString();

                row.AddTextCell("StartState", transition.StartState.Name);
                row.AddTextCell("EndState", transition.EndState.Name);

                string groups = string.Empty;
                foreach (RegisterTransitionRight ug in transition.RightsList)
                {
                    groups = groups + ug.UserGroup.Name + " <br> ";
                }

                row.AddTextCell("GroupRights", groups);

                var actions = new TableDataCellActionsModel();
                actions.AddAction("edit", Properties.Content.transition_hint_edit, relativePathBase + "RegisterTransition/Edit/" + transition.Id);
                row.AddActionsCell("actions", actions);

                result.Rows.Add(row);
            }


            return result;
        }

        public IList<ControlModelBase> GetTransControlls(RegisterTransition regTransition,RegisterAttributesHead regHead)
        {
            var result = new List<ControlModelBase>();

            result.Add(new ControlModelHidden()
            {
                Col = 12,
                Label = "",
                Name = RegisterTransitionViewModel.JSON_NAME_ID,
                Value = (regTransition == null) ? string.Empty : regTransition.Id.ToString()
            });

            result.Add(new ControlModelHidden()
            {
                Col = 12,
                Label = "",
                Name = RegisterTransitionViewModel.JSON_NAME_HEADID,
                Value = (regTransition == null) ? regHead.Id.ToString() : regHead.Id.ToString()
            });

            setTransitions(result, regHead, regTransition);
            AddMultySelectUserGroups(result, regHead, regTransition);


            return result;
        }

        private void setTransitions(List<ControlModelBase> result, RegisterAttributesHead head, RegisterTransition regTransition)
        {


            var startState = new ControlModelOptionList()
            {
                Col = 12,
                Label = Properties.Content.transition_start,
                Required = true,
                Name =  RegisterTransitionViewModel.JSON_NAME_START
            };

            var endState = new ControlModelOptionList()
            {
                Col = 12,
                Label = Properties.Content.transition_end,
                Required = true,
                Name = RegisterTransitionViewModel.JSON_NAME_END
            };

            RegisterStatesFilter stateFilter = new RegisterStatesFilter();
            stateFilter.RegisterAttributesHead = head;
            var list = DbContext.GetRegisterStatesDao().GetAll(stateFilter);
            foreach (RegisterState state in list.Results) {

                var stateOption = new ControlModelOptionElement()
                {
                    Label = state.Name,
                    Value = state.Id.ToString()
                };

                startState.Options.Add(stateOption);
                endState.Options.Add(stateOption);

                if (regTransition != null ) {
                    if (regTransition.StartState == state) {
                        startState.SelectedValue = stateOption;
                    }
                    if (regTransition.EndState == state)
                    {
                        endState.SelectedValue = stateOption;
                    }
                }
            }

            result.Add(startState);
            result.Add(endState);
        }

        protected void AddMultySelectUserGroups(IList<ControlModelBase> result, RegisterAttributesHead head, RegisterTransition regTransition)
        {

          
            //UserGroupFilter ugf = new UserGroupFilter();
            //ugf.AdminBody = head.Register.AdministrativeBody;
            //var groupList = DbContext.GetUserGroupsDao().GetAllUserGroups(ugf);

           var grouplist= DbContext.RegisterRightsDao.GetRightForRegisterStructure(head.Register);

            var optionGroup = new ControlModelOptionGroup();
            var bodyControl = new ControlModelOptionMultiList()
            {
                Col = 12,
                Label = Properties.Content.base_usergroup,
                Required = true,
                Name = RegisterTransitionViewModel.JSON_NAME_RIGHTS
            };
            bodyControl.SelectedValues = new List<ControlModelOptionElement>();

            foreach (RegisterRight ug in grouplist) {

                var selectedBody = new ControlModelOptionElement()
                {
                    Label = ug.UserGroup.Name,
                    Value = ug.UserGroup.Id.ToString()
                };

                optionGroup.Options.Add(selectedBody);

                if ((regTransition != null) && regTransition.RightsList != null)
                {
                    if (regTransition.RightsList.Where(x=>x.UserGroup==ug.UserGroup).ToList().Count>0)
                    {
                        bodyControl.SelectedValues.Add(selectedBody);
                    }
                }
            }
            var wrapper = new ControlModelOptionGroupWrapper(optionGroup);
            bodyControl.AddOptionGroup(optionGroup);
            result.Add(bodyControl);

         
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


        public RegisterTransitionViewModel ParseAreaFromJson(string json)
        {
            var result = new RegisterTransitionViewModel();

            dynamic areaData = JsonConvert.DeserializeObject(json);

            var idElem = areaData[0];
            if (idElem.name != RegisterTransitionViewModel.JSON_NAME_ID) throw new Exception("Invalid JSON Area data : missing Id as first element!");

            if (idElem.value != string.Empty)
            {
                result.Id = (int)idElem.value;
            }

            foreach (var elem in areaData)
            {
                if (elem.name == RegisterTransitionViewModel.JSON_NAME_HEADID)
                {
                    result.HeadId = elem.value;
                }
                if (elem.name == RegisterTransitionViewModel.JSON_NAME_START)
                {                 
                    var selected = elem.selectedValue;
                    var selectedId = selected.value;
                    result.StartState = selectedId;
                }
                if (elem.name == RegisterTransitionViewModel.JSON_NAME_END)
                {
                    var selected = elem.selectedValue;
                    var selectedId = selected.value;
                    result.EndState = selectedId;
                }

                if (elem.name == RegisterTransitionViewModel.JSON_NAME_RIGHTS)
                {
                    var selected = elem.selectedValues;
                    result.Rights = new List<int>();
                    foreach (var selElem in selected)
                    {
                        var strngSelected = (string)selElem.value;
                        var selInt = Int32.Parse(strngSelected);
                        result.Rights.Add(selInt);
                    }
                }
            }

            return result;
        }

    }
}
