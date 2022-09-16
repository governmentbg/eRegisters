using Newtonsoft.Json;
using Orak.Utils;
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
    public class UnifiedDataJsonHelper : BaseJsonHelper
    {
        public UnifiedDataJsonHelper(ISmartRegistryContext context)
            : base(context)
        {

        }

        public IList<ControlModelBase> GetEditControls(UnifiedData uniData)
        {
            var result = new List<ControlModelBase>();


            result.Add(new ControlModelHidden()
            {
                Col = 12,
                Label = "",
                Name = UnifiedDataViewModel.JSON_NAME_ID,
                Value = (uniData == null) ? string.Empty : uniData.Id.ToString()
            });

            setCommonControls(uniData,result);

         
            result.Add(new ControlModelCheckbox()
            {
                Col = 12,
                Label = "Приема много стойности",
                Name = UnifiedDataViewModel.JSON_NAME_HASMULTIPLE,
                Checked = (uniData == null) ? false : uniData.HasMultipleValues
            });

            var dataTypeControl = new ControlModelOptionList()
            {
                Col = 12,
                Label = "Тип",
                Required = true,
                Name = UnifiedDataViewModel.JSON_NAME_DATATYPE
            };

            var orderedDataTypes = EnumUtils.GetValues<UnifiedDataTypeEnum>().OrderBy(x => EnumUtils.GetDisplayName(x));
            foreach (var dataType in orderedDataTypes)
            {
                AddDataTypeToCombo(dataTypeControl, dataType, uniData);
            }

            result.Add(dataTypeControl);

            AddStatusControl(result, ((uniData == null) || uniData.IsActive), UnifiedDataViewModel.JSON_NAME_ISACTIVE);

            return result;
        }

        private void setCommonControls(UnifiedData uniData,List<ControlModelBase> result)
        {
            result.Add(new ControlModelText()
            {
                Col = 12,
                Label = "УРИ",
                Name = UnifiedDataViewModel.JSON_NAME_URI,
                Enabled = false,
                Value = (uniData == null) ? "*генерира се*" : uniData.URI
            });

            result.Add(new ControlModelText()
            {
                Col = 12,
                Label = "Име",
                Name = UnifiedDataViewModel.JSON_NAME_NAME,
                Required=true,
                Value = (uniData == null) ? string.Empty : uniData.Name
            });

            result.Add(new ControlModelText()
            {
                Col = 12,
                Label = "Техническо име",
                Name = UnifiedDataViewModel.JSON_NAME_NAMESPACEAPI,
                Required = true,
                Value = (uniData == null) ? string.Empty : uniData.NamespaceApi
            });

            result.Add(new ControlModelMultilineText()
            {
                Col = 12,
                Label = "Описание",
                Name = UnifiedDataViewModel.JSON_NAME_DESCRIPTION,
                Rows = 10,
                Value = (uniData == null) ? string.Empty : uniData.Description
            });
        }


        public IList<ControlModelBase> GetEditReferentialControls(string relativePathBase, UnifiedData uniData)
        {
            if ((uniData != null) && (uniData.DataType != UnifiedDataTypeEnum.Referential))
            {
                throw new Exception("Information object is not of type REFERENTIAL!");
            }

            var result = new List<ControlModelBase>();

            result.Add(new ControlModelHidden()
            {
                Col = 12,
                Label = "",
                Name = UnifiedDataViewModel.JSON_NAME_ID,
                Value = (uniData == null) ? string.Empty : uniData.Id.ToString()
            });

            setCommonControls(uniData, result);

            Register refRegister = null;
            RegisterAttribute refAttr = null;
            if (uniData != null)
            {
                refRegister = uniData.ReferentialRegister;
                refAttr = uniData.ReferentialAttribute;
            }

            result.Add(new ControlModelAjaxInput()
            {
                Col = 12,
                Label = "Референтен регистър",
                Name = UnifiedDataViewModel.JSON_NAME_REF_REGISTER,
                Value = (refRegister == null) ? null : refRegister.Id.ToString(),
                DisplayValue = (refRegister == null) ? null : refRegister.Name.ToString(),
                AjaxUrl = relativePathBase + "Registers/GetAllRegistersAjax?id=32432",
                ParamControlName = string.Empty,
                Required = true
            });

            result.Add(new ControlModelAjaxInput()
            {
                Col = 12,
                Label = "Референтен Атрибут",
                Name = UnifiedDataViewModel.JSON_NAME_REF_ATTRIBUTE,
                Value = (refAttr == null) ? null : refAttr.Id.ToString(),
                DisplayValue = (refAttr == null) ? null : refAttr.Name.ToString(),
                AjaxUrl = relativePathBase + "Registers/GetRegisterColsAjax?id=32432",
                ParamControlName = UnifiedDataViewModel.JSON_NAME_REF_REGISTER,
                Required = true
            });


            AddStatusControl(result, ((uniData == null) || uniData.IsActive), UnifiedDataViewModel.JSON_NAME_ISACTIVE);

            return result;
        }

        public IList<ControlModelBase> GetEditCompositeControls(UnifiedData uniData)
        {
            var result = new List<ControlModelBase>();

            result.Add(new ControlModelHidden()
            {
                Col = 12,
                Label = "",
                Name = UnifiedDataViewModel.JSON_NAME_ID,
                Value = (uniData == null) ? string.Empty : uniData.Id.ToString()
            });

            setCommonControls(uniData, result);

            AddStatusControl(result, ((uniData == null) || uniData.IsActive), UnifiedDataViewModel.JSON_NAME_ISACTIVE);

            var repeater = new ControlModelRepeater();
            repeater.Col = 12;
            repeater.Label = "Инфо обект";
            repeater.Name = UnifiedDataViewModel.JSON_NAME_COMPOSITELIST;
            repeater.ItemTemplate = CreateStructureItemTemplate(null);

            if (uniData!=null && uniData.CompositeList != null && uniData.CompositeList.Count!=0)
            {
                repeater.Values = new List<ControlModelRepeaterItemTemplate>();
                foreach (var uData in uniData.CompositeList)
                {
                    var attrControls = CreateStructureItemTemplate(uData);
                    repeater.Values.Add(attrControls);
                }
            }

            result.Add(repeater);

            return result;
        }



        private ControlModelRepeaterItemTemplate CreateStructureItemTemplate(UnifiedDataComposite attribute)
        {
            var result = new ControlModelRepeaterItemTemplate();

            result.ElementId = (attribute == null) ? string.Empty : attribute.Id.ToString();

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

                if ((attribute != null) && (attribute.ContainedUnifiedData.Id == uniData.Id))
                {
                    uniDataCombo.SelectedValue = uniDataOption;
                }
            }
            result.Controls.Add(uniDataCombo);


            return result;
        }

        private void AddDataTypeToCombo(ControlModelOptionList control, UnifiedDataTypeEnum dataType, UnifiedData uniData)
        {
            if (dataType == UnifiedDataTypeEnum.NoType) return;
            if (dataType == UnifiedDataTypeEnum.Composite) return;
            if (dataType == UnifiedDataTypeEnum.OptionValue) return;

            var dataTypeOption = new ControlModelOptionElement()
            {
                Label = EnumUtils.GetDisplayName(dataType),
                Value = ((int)dataType).ToString()
            };
            control.Options.Add(dataTypeOption);

            if ((uniData != null) && (uniData.DataType == dataType))
            {
                control.SelectedValue = dataTypeOption;
            }
        }

        public UnifiedDataViewModel ParseUnifiedDataFromJson(string json)
        {
            dynamic uniData = JsonConvert.DeserializeObject(json);

            var result = new UnifiedDataViewModel();

            var idElem = uniData[0];
            if (idElem.name != UnifiedDataViewModel.JSON_NAME_ID) throw new Exception("Invalid JSON User data : missing Id as first element!");

            if (idElem.value != string.Empty)
            {
                result.Id = (int)idElem.value;
            }

            foreach (var elem in uniData)
            {
                if (elem.name == UnifiedDataViewModel.JSON_NAME_URI)
                {
                    if ((bool)elem.enabled)
                    {
                        result.URI = elem.value;
                    }
                    else {
                        result.URI = string.Empty;
                    }
                   
                }
                if (elem.name == UnifiedDataViewModel.JSON_NAME_NAME)
                {
                    result.Name = elem.value;
                }

                if (elem.name == UnifiedDataViewModel.JSON_NAME_NAMESPACEAPI) {
                    result.NamespaceApi = elem.value;
                }

                if (elem.name == UnifiedDataViewModel.JSON_NAME_DESCRIPTION)
                {
                    result.Description = elem.value;
                }
                if (elem.name == UnifiedDataViewModel.JSON_NAME_HASMULTIPLE)
                {
                    result.HasMultipleValues = elem.value;
                }
                if (elem.name == UnifiedDataViewModel.JSON_NAME_ISACTIVE)
                {
                    if (elem.selectedValue != null)
                    {
                        result.IsActive = (elem.selectedValue.value == 1);
                        result.ActiveValue = elem.selectedValue.value;
                    }
                }
                if (elem.name == UnifiedDataViewModel.JSON_NAME_DATATYPE)
                {
                    if (elem.selectedValue != null)
                    {
                        result.DataType = (UnifiedDataTypeEnum)elem.selectedValue.value;
                    }
                }
                if (elem.name == UnifiedDataViewModel.JSON_NAME_COMPOSITELIST)
                {
                    result.CompositeDataList = new List<int>();
                    foreach (var z in elem.values)
                    {
                        int uniId = FillModelFromJson(z);
                        result.CompositeDataList.Add(uniId);
                    }
                }
                if (elem.name == UnifiedDataViewModel.JSON_NAME_REF_REGISTER)
                {
                    //if (elem.value != null)
                    //{
                    //    result.RefRegisterId = (int)elem.value;
                    //}
                    if (elem.displayValue != null)
                    {
                        result.RefRegisterId = (int)elem.displayValue.value;
                    }
                }
                if (elem.name == UnifiedDataViewModel.JSON_NAME_REF_ATTRIBUTE)
                {
                    //if (elem.value != null)
                    //{
                    //    result.RefAttributeId = (int)elem.value;
                    //}
                    if (elem.value != null)
                    {
                        result.RefAttributeId = (int)elem.displayValue.value;
                    }
                }

            }
            return result;
        }

        private int FillModelFromJson(dynamic elemAttr)
        {
            var selectedID = 0;

            if ((elemAttr.elementId != null) && (elemAttr.elementId != string.Empty))
            {
                selectedID = DynamicToInt(elemAttr.elementId);
            }

            foreach (var elem in elemAttr.controls)
            {
                if (elem.selectedValue != null)
                {
                    var selected = elem.selectedValue;
                    var selectedStuff = selected.value;

                    selectedID = Convert.ToInt32((string)selectedStuff) ;
                }

             
            }

            return selectedID;
        }
    }
}
