using Newtonsoft.Json;
using SmartRegistry.CommonWeb.JsonControls;
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
    public class RegisterStructureJsonHelper : BaseJsonHelper
    {
        public RegisterStructureJsonHelper(ISmartRegistryContext context)
            : base(context)
        {

        }

        public IList<ControlModelBase> GetEditRegisterStructureControls(Register register)
        {
            var result = new List<ControlModelBase>();

            result.Add(new ControlModelHidden()
            {
                Col = 12,
                Label = "",
                Name = "Id",
                Value = (register == null) ? string.Empty : register.Id.ToString()
            });

            // 
            var repeater = new ControlModelRepeater();
            repeater.Col = 12;
            repeater.Label = "Атрибути на регистър";
            repeater.Name = "RegisterAttributes";
            repeater.ItemTemplate = CreateStructureItemTemplate(null);
            repeater.Values = new List<ControlModelRepeaterItemTemplate>();
            if (register != null)
            {
                var registerHeadDao = SmartContext.DbContext.GetRegisterAttributeHeadDao();
                var currentAttributeHead = registerHeadDao.GetCurrentHeadForRegister(register.Id);

                if ((currentAttributeHead != null) && (currentAttributeHead.Attributes != null))
                {
                    foreach (var attribute in currentAttributeHead.Attributes)
                    {
                        if (attribute.ParentAttribute == null) { 
                        var attrControls = CreateStructureItemTemplate(attribute);
                        repeater.Values.Add(attrControls);
                        }
                    }
                }
            }
            
            if (repeater.Values.Count == 0)
            {
                var attrControls = CreateStructureItemTemplate(null);
                repeater.Values.Add(attrControls);
            }
            result.Add(repeater);

            return result;
        }

        private ControlModelRepeaterItemTemplate CreateStructureItemTemplate(RegisterAttribute attribute)
        {
            var result = new ControlModelRepeaterItemTemplate();


            result.ElementId = (attribute == null) ? string.Empty : attribute.Id.ToString();

            result.Controls.Add(new ControlModelText()
            {
                Col = 12,
                Label = "Наименование",
                Name = "Name",
                Required=true,
                Value = (attribute == null) ? string.Empty : attribute.Name
            });

            result.Controls.Add(new ControlModelText()
            {
                Col = 12,
                Label = "Техническо наименование",
                Name = "ApiName",
                Required = true,
                Value = (attribute == null) ? string.Empty : attribute.ApiName
            });


            var uniDataCombo = new ControlModelOptionList()
            {
                Col = 12,
                Label = "Информационен обект",
                Required = true,
                Name = "InfoObject"
            };
            var uniDataDao = SmartContext.DbContext.GetUnifiedDataDao();
            var uniDatas = uniDataDao.GetAllList(new UnifiedDataFilter()
            {
                OrderByColumn = "Name"
            });

            foreach (var uniData in uniDatas)
            {
                var uniDataOption = new ControlModelOptionElement()
                {
                    Label = uniData.Name,
                    Value = uniData.Id.ToString()
                };
                uniDataCombo.Options.Add(uniDataOption);

                if ((attribute != null) && (attribute.UnifiedData.Id == uniData.Id))
                {
                    uniDataCombo.SelectedValue = uniDataOption;
                }
            }
            result.Controls.Add(uniDataCombo);
            result.Controls.Add(new ControlModelCheckbox()
            {
                Col = 12,
                Label = "По този атрибут може да се филтрира списъка",
                Name = "CanFilterAttribute",
                Checked = (attribute == null) ? true : attribute.CanFilter
            });
            result.Controls.Add(new ControlModelCheckbox()
            {
                Col = 12,
                Label = "Има публичен достъп до този атрибут",
                Name = "IsPublicAttribute",
                Checked = (attribute == null) ? true : attribute.IsPublic
            });
            result.Controls.Add(new ControlModelCheckbox()
            {
                Col = 12,
                Label = "Стойностите се експортират към OpenData портала",
                Name = "ExportOpenData",
                Checked = (attribute == null) ? true : attribute.ExportOpenData
            });
            result.Controls.Add(new ControlModelCheckbox()
            {
                Col = 12,
                Label = "Стойностите ще се достъпват през Уеб Услугите и Реджикс",
                Name = "ExportWebServices",
                Checked = (attribute == null) ? true : attribute.ExportWebServices
            });

            return result;
        }

        public RegisterStructureViewModel ParseRegisterAttributesFromJson(string json)
        {
            RegisterStructureViewModel result = new RegisterStructureViewModel();

            dynamic registerHeadData = JsonConvert.DeserializeObject(json);

            var idElem = registerHeadData[0];
            if (idElem.name != "Id") throw new Exception("Invalid JSON Register Structure data : missing Id as first element!");
            if (idElem.value == string.Empty) throw new Exception("Invalid JSON Register Structure data : id register not set!");

            result.RegisterId = (int)idElem.value;

            foreach (var rootElem in registerHeadData)
            {
                if (rootElem.name == "RegisterAttributes")
                {
                    foreach (var elem in rootElem.values)
                    {
                        RegisterAttributeViewModel attrModel = FillAttributeModelFromJsonElement(elem);
                        result.Attributes.Add(attrModel);
                    }
                }
            }

            return result;
        }

        private RegisterAttributeViewModel FillAttributeModelFromJsonElement(dynamic elemAttr)
        {
            RegisterAttributeViewModel attrModel = new RegisterAttributeViewModel();

            if ((elemAttr.elementId != null) && (elemAttr.elementId != string.Empty))
            {
                attrModel.Id = DynamicToInt(elemAttr.elementId);
            }

            foreach (var elem in elemAttr.controls)
            {
                if (elem.name == "Name")
                {
                    attrModel.Name = elem.value;
                }

                if (elem.name == "ApiName")
                {
                    attrModel.ApiName = elem.value;
                }

                if (elem.name == "InfoObject")
                {
                    if (elem.selectedValue != null)
                    {
                        attrModel.UnifiedDataId = (int)elem.selectedValue.value;
                    }
                }
                if (elem.name == "CanFilterAttribute")
                {
                    attrModel.CanFilter = elem.value;
                }
                if (elem.name == "IsPublicAttribute")
                {
                    attrModel.IsPublic = elem.value;
                }
                if (elem.name == "ExportOpenData")
                {
                    attrModel.ExportOpenData = elem.value;
                }
                if (elem.name == "ExportWebServices")
                {
                    attrModel.ExportWebServices = elem.value;
                }
            }

            return attrModel;
        }

    }
}
