using Newtonsoft.Json;
using SmartRegistry.CommonWeb.JsonControls;
using SmartRegistry.Domain.Entities;
using SmartRegistry.Domain.Interfaces;
using SmartRegistry.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.CommonWeb.JsonHelpers
{
    public class RegisterJsonHelper : BaseJsonHelper
    {
        public RegisterJsonHelper(ISmartRegistryContext context)
            : base(context)
        {

        }

        public IList<AjaxInputResultObject> GetAjaxRegisterList(IList<Register> registers)
        {
            if (registers == null) return null;

            var res = new List<AjaxInputResultObject>();

            foreach(var reg in registers)
            {
                res.Add(new AjaxInputResultObject(reg.Id.ToString(), reg.Name));
            }

            return res;
        }


        public IList<AjaxInputResultObject> GetAjaxRegisterColList(IList<RegisterAttribute> attributes)
        {
            var res = new List<AjaxInputResultObject>();

            if (attributes == null) return res;

            foreach (var attr in attributes)
            {
                res.Add(new AjaxInputResultObject(attr.Id.ToString(), attr.Name));
            }

            return res;
        }


        public IList<ControlModelBase> GetRegisterControls(Register register)
        {
            var result = new List<ControlModelBase>();

            result.Add(new ControlModelHidden()
            {
                Col = 12,
                Label = "",
                Name = RegisterViewModel.JSON_NAME_ID,
                Value = (register == null) ? string.Empty : register.Id.ToString()
            });

            result.Add(new ControlModelText()
            {
                Col = 12,
                Label = Properties.Content.register_uri,
                Name = RegisterViewModel.JSON_NAME_URI,
                 Value = (register == null) ? Properties.Content.uri_auto_generated : register.URI,
                Enabled =false
            });

            result.Add(new ControlModelText()
            {
                Col = 12,
                Label = Properties.Content.register_name,
                Required=true,
                Name = RegisterViewModel.JSON_NAME_NAME,
                Value = (register == null) ? string.Empty : register.Name
            });

            result.Add(new ControlModelText()
            {
                Col = 12,
                Label = Properties.Content.register_technicalname,
                Required = true,
                Name = RegisterViewModel.JSON_NAMESPACE_API,
                Value = (register == null) ? string.Empty : register.NamespaceApi
            });

            result.Add(new ControlModelMultilineText()
            {
                Col = 12,
                Label = Properties.Content.register_public_description,
                Name = RegisterViewModel.JSON_NAME_DESCRIPTION,
                Rows = 10,
                Value = (register == null) ? string.Empty : register.Description
            });

            result.Add(new ControlModelText()
            {
                Col = 12,
                Label = Properties.Content.register_order,
                Name = RegisterViewModel.JSON_NAME_ADMINACT,
                Value = (register == null) ? string.Empty : register.AdministrativeAct
            });

            result.Add(new ControlModelMultilineText()
            {
                Col = 12,
                Label = Properties.Content.register_legalbasis,
                Name = RegisterViewModel.JSON_NAME_LEGALBASIS,
                Rows = 10,
                Value = (register == null) ? string.Empty : register.LegalBasis
            });

            result.Add(new ControlModelText()
            {
                Col = 12,
                Label = Properties.Content.register_url,
                Name = RegisterViewModel.JSON_NAME_URL,
                Value = (register == null) ? string.Empty : register.UrlAddress
            });

            AddStatusControl(result, ((register == null) || register.IsActive), RegisterViewModel.JSON_NAME_ISACTIVE);

		//	AddMultySelectGroups(result,register);

            AddGroupComboBox(result,register);

            return result;
        }

        private void AddGroupComboBox(List<ControlModelBase> result, Register register)
        {
            var combo = new ControlModelOptionMultiList();
            combo.Label = Properties.Content.register_theme;
            combo.Name = RegisterViewModel.JSON_NAME_AREA;
            combo.SelectedValues = new List<ControlModelOptionElement>();

            var allAreaGroups = DbContext.GetAreaGroupsDao().GetAll();
            foreach (AreaGroup argp in allAreaGroups) {

                var group1 = new ControlModelOptionGroup()
                {
                    Label = argp.Name
                };

                foreach (Area area in argp.AreaList) {

                    //group1.Options.Add(new ControlModelOptionElement()
                    //{
                    //    Label = area.Name,
                    //    Value = area.Id.ToString()
                    //});

                    var selectedBody = new ControlModelOptionElement()
                    {
                        Label = area.Name,
                        Value = area.Id.ToString()
                    };

                    group1.Options.Add(selectedBody);


                    if ((register != null) && register.RegisterAreas != null)
                    {
                        if (register.RegisterAreas.Any(x => x.Area == area))
                        {
                            combo.SelectedValues.Add(selectedBody);
                        }
                    }

                }

                combo.AddOptionGroup(group1);
              
            }

            result.Add(combo);

        }

        protected void AddMultySelectGroups(IList<ControlModelBase> listControls,Register register)
        {
            var bodyControl = new ControlModelOptionMultiList()
            {
                Col = 12,
                Label = "Теми",
                Name = RegisterViewModel.JSON_NAME_AREA
            };


            var optiongroup = new ControlModelOptionGroup();
            bodyControl.SelectedValues = new List<ControlModelOptionElement>();

            var allAreas = DbContext.GetAreasDao().GetAll();

            foreach (Area selectedArea in allAreas)
            {

                var selectedBody = new ControlModelOptionElement()
                {
                    Label = selectedArea.Name,
                    Value = selectedArea.Id.ToString()
                };

                optiongroup.Options.Add(selectedBody);

                if ((register != null) && register.RegisterAreas != null)
                {
                    if (register.RegisterAreas.Any(x=>x.Area == selectedArea))
                   {
                        bodyControl.SelectedValues.Add(selectedBody);
                    }
                }
            }
            var tt = new ControlModelOptionGroupWrapper(optiongroup);
            bodyControl.AddOptionGroup(optiongroup);
            listControls.Add(bodyControl);
        }

		
        public RegisterViewModel ParseRegisterFromJson(string json)
        {
            var result = new RegisterViewModel();

            dynamic registerData = JsonConvert.DeserializeObject(json);

            var idElem = registerData[0];
            if (idElem.name != RegisterViewModel.JSON_NAME_ID) throw new Exception("Invalid JSON Register data : missing Id as first element!");

            if (idElem.value != string.Empty)
            {
                result.Id = (int)idElem.value;
            }

            foreach (var elem in registerData)
            {
                if (elem.name == RegisterViewModel.JSON_NAME_URI)
                {
                    if ((bool)elem.enabled)
                    {
                        result.URI = elem.value;
                    }
                    else {
                        result.URI = string.Empty;
                    }
                   
                }
                if (elem.name == RegisterViewModel.JSON_NAME_NAME)
                {
                    result.Name = elem.value;
                }

                if (elem.name == RegisterViewModel.JSON_NAMESPACE_API)
                {
                    result.NamespaceApi = elem.value;
                }

                if (elem.name == RegisterViewModel.JSON_NAME_ADMINACT)
                {
                    result.AdministrativeAct = elem.value;
                }
                if (elem.name == RegisterViewModel.JSON_NAME_LEGALBASIS)
                {
                    result.LegalBasis = elem.value;
                }
                if (elem.name == RegisterViewModel.JSON_NAME_URL)
                {
                    result.UrlAddress = elem.value;
                }
                if (elem.name == RegisterViewModel.JSON_NAME_ISACTIVE)
                {
                    result.IsActive = (elem.selectedValue.value == 1);
                    result.ActiveValue = elem.selectedValue.value;
                }
                if (elem.name == RegisterViewModel.JSON_NAME_AREA)
                {
                   
                    if (result.AreaIds == null)
                    {
                        result.AreaIds = new List<int>();
                    }

                    var selected = elem.selectedValues;
                    foreach (var selElem in selected)
                    {
                        var strngSelected = (string)selElem.value;
                        var selInt = Int32.Parse(strngSelected);
                        result.AreaIds.Add(selInt);
                    }
                }
                if (elem.name == RegisterViewModel.JSON_NAME_DESCRIPTION)
                {
                    result.Description = elem.value;
                }
            }
            return result;
        }
    }
}
