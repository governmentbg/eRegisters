using Newtonsoft.Json;
using Orak.Utils;
using SmartRegistry.CommonWeb.JsonControls;
using SmartRegistry.Domain.Common;
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
    public class WebServiceJsonHelper : BaseJsonHelper
    {
        public WebServiceJsonHelper(ISmartRegistryContext context)
            : base(context)
        {

        }

        public IList<ControlModelBase> GetEditControls(WebServiceISCIPR webServ, int registerId, string serviceType)
        {
            var regService = SmartContext.RegistersService;
            var register = regService.GetRegister(registerId);

            if (register == null) return null;

            var result = new List<ControlModelBase>();

            result.Add(new ControlModelHidden()
            {
                Col = 12,
                Label = "",
                Name = WebServiceViewModel.JSON_NAME_ID,
                Value = (webServ == null) ? string.Empty : webServ.Id.ToString()
            });

            result.Add(new ControlModelText()
            {
                Col = 12,
                Label = "Име",
                Required=true,
                Name = WebServiceViewModel.JSON_NAME_NAME,
                Value = (webServ == null) ? string.Empty : webServ.Name
            });

            result.Add(new ControlModelText()
            {
                Col = 12,
                Label = "Техническо име",
                Required = true,
                Name = WebServiceViewModel.JSON_NAME_SERVICEKEY,
                Value = (webServ == null) ? string.Empty : webServ.ServiceKey
            });

            if (serviceType == "management")
            {
                AddManagementServiceTypeControl(result, webServ);
            }

            result.Add(new ControlModelMultilineText()
            {
                Col = 12,
                Label = "Описание",
                Name = WebServiceViewModel.JSON_NAME_DESCRIPTION,
                Value = (webServ == null) ? string.Empty : webServ.Description
            });

            if (serviceType == "report")
            {
                RegisterAttributesHead attrHead = null;
                if (webServ != null)
                {
                    attrHead = webServ.AttributeHead;
                }
                else if (register != null)
                {
                    attrHead = DbContext.GetRegisterAttributeHeadDao().GetCurrentHeadForRegister(register.Id);
                }

                if (attrHead != null)
                {
                    // Request Conditions
                    var requestRepeater = new ControlModelRepeater()
                    {
                        Col = 12,
                        Label = "Настройки на заявката",
                        Name = WebServiceViewModel.JSON_NAME_REQUESTSETTINGS
                    };
                    IList<RegisterAttribute> allAttributes = attrHead.Attributes;
                    requestRepeater.ItemTemplate = CreateRequestConditionsItemTemplate(allAttributes, null);
                    if (webServ != null)
                    {
                        foreach(var cond in webServ.RequestConditions)
                        {
                            var reqItem = CreateRequestConditionsItemTemplate(allAttributes, cond);
                            requestRepeater.Values.Add(reqItem);
                        }
                    }
                    result.Add(requestRepeater);

                    // Response Attributes
                    var responseRepeater = new ControlModelRepeater()
                    {
                        Col = 12,
                        Label = "Атрибути в резултата",
                        Name = WebServiceViewModel.JSON_NAME_RESPONSEATTRIBUTELIST
                    };
                    responseRepeater.ItemTemplate = CreateResponseAttributeItemTemplate(allAttributes,null);
                    if ((webServ != null) && (webServ.ResponseAttributes != null))
                    {
                        foreach(var respAttr in webServ.ResponseAttributes)
                        {
                            var respItem = CreateResponseAttributeItemTemplate(allAttributes, respAttr);
                            responseRepeater.Values.Add(respItem);
                        }
                    }
                    result.Add(responseRepeater);
                }

            }

            return result;
        }

        private ControlModelRepeaterItemTemplate CreateResponseAttributeItemTemplate(IList<RegisterAttribute> allAttributes, WebServiceResponseAttribute respAttr)
        {
            var result = new ControlModelRepeaterItemTemplate();

            result.ElementId = (respAttr == null) ? string.Empty : respAttr.Id.ToString();

            // Condition Operator Combo Box
            var attrOptionList = new ControlModelOptionList()
            {
                Col = 12,
                Label = "Атрибут",
                Name = WebServiceViewModel.JSON_NAME_RESPONSEATTRIBUTE
            };
            RegisterAttribute regAttr = null;
            if (respAttr != null)
            {
                regAttr = respAttr.Attribute;
            }
            foreach (var attr in allAttributes)
            {
                AddAttributeToCombo(attrOptionList, attr, regAttr);
            }
            result.Controls.Add(attrOptionList);

            return result;
        }

        private ControlModelRepeaterItemTemplate CreateRequestConditionsItemTemplate(IList<RegisterAttribute> allAttributes, WebServiceRequestCondition cond)
        {
            var result = new ControlModelRepeaterItemTemplate();

            result.ElementId = (cond == null) ? string.Empty : cond.Id.ToString();

            // Condition Operator Combo Box
            var condOperatorControl = new ControlModelOptionList()
            {
                Col = 12,
                Label = "Условие",
                Name = WebServiceRequestConditionViewModel.JSON_NAME_CONDITION
            };
            AddRequestCondtionToCombo(condOperatorControl, ConditionOperator.Equal, cond);
            AddRequestCondtionToCombo(condOperatorControl, ConditionOperator.LessThan, cond);
            AddRequestCondtionToCombo(condOperatorControl, ConditionOperator.LessThanOrEqual, cond);
            AddRequestCondtionToCombo(condOperatorControl, ConditionOperator.GreaterThan, cond);
            AddRequestCondtionToCombo(condOperatorControl, ConditionOperator.GreaterThanOrEqual, cond);
            AddRequestCondtionToCombo(condOperatorControl, ConditionOperator.Like, cond);

            result.Controls.Add(condOperatorControl);

            // Attribute ComboBox
            var attrControl = new ControlModelOptionList()
            {
                Col = 12,
                Label = "Атрибут",
                Name = WebServiceRequestConditionViewModel.JSON_NAME_ATTRIBUTE
            };
            RegisterAttribute regAttr = null;
            if (cond != null)
            {
                regAttr = cond.Attribute;
            }
            foreach (var attr in allAttributes)
            {
                AddAttributeToCombo(attrControl, attr, regAttr);
            }
            result.Controls.Add(attrControl);

            result.Controls.Add(new ControlModelCheckbox()
            {
                Col = 12,
                Label = "Задължителен в заявката",
                Name = WebServiceRequestConditionViewModel.JSON_NAME_ISREQUIRED,
                Checked = (cond == null) ? false : cond.IsRequired
            });

            return result;
        }

        private void AddAttributeToCombo(ControlModelOptionList attrControl, RegisterAttribute attr, RegisterAttribute selectedAttr)
        {
            if (attr == null) return;

            var attrOption = new ControlModelOptionElement()
            {
                Label = attr.Name,
                Value = attr.Id.ToString()
            };
            attrControl.Options.Add(attrOption);

            if ((selectedAttr != null) && (selectedAttr.Id == attr.Id))
            {
                attrControl.SelectedValue = attrOption;
            }
        }

        private void AddRequestCondtionToCombo(ControlModelOptionList control, ConditionOperator condOper, WebServiceRequestCondition cond)
        {
            var condOperOption = new ControlModelOptionElement()
            {
                Label = EnumUtils.GetDisplayName(condOper),
                Value = ((int)condOper).ToString()
            };
            control.Options.Add(condOperOption);

            if ((cond != null) && (cond.ConditionOperator == condOper))
            {
                control.SelectedValue = condOperOption;
            }
        }


        private void AddManagementServiceTypeControl(IList<ControlModelBase> controls, WebServiceISCIPR webServ)
        {
            var serviceTypeControl = new ControlModelOptionList()
            {
                Col = 12,
                Label = "Тип",
                Required = true,
                Name = WebServiceViewModel.JSON_NAME_SERVICETYPE
            };

            AddServiceTypeToCombo(serviceTypeControl, WebServiceType.CreateRecord, webServ);
            AddServiceTypeToCombo(serviceTypeControl, WebServiceType.ChangeRecord, webServ);
            AddServiceTypeToCombo(serviceTypeControl, WebServiceType.RemoveRecord, webServ);

            controls.Add(serviceTypeControl);
        }

        private void AddServiceTypeToCombo(ControlModelOptionList control, WebServiceType webServiceType, WebServiceISCIPR webServ)
        {
            var webServTypeOption = new ControlModelOptionElement()
            {
                Label = EnumUtils.GetDisplayName(webServiceType),
                Value = ((int)webServiceType).ToString()
            };
            control.Options.Add(webServTypeOption);

            if ((webServ != null) && (webServ.ServiceType == webServiceType))
            {
                control.SelectedValue = webServTypeOption;
            }

        }

        public WebServiceViewModel ParseWebServiceFromJson(string json, int registerId)
        {
            var result = new WebServiceViewModel();
            result.RegisterId = registerId;

            dynamic webServData = JsonConvert.DeserializeObject(json);

            var idElem = webServData[0];
            if (idElem.name != WebServiceViewModel.JSON_NAME_ID) throw new Exception("Invalid JSON WebService data : missing Id as first element!");

            if (idElem.value != string.Empty)
            {
                result.Id = (int)idElem.value;
            }

            foreach (var elem in webServData)
            {
                if (elem.name == WebServiceViewModel.JSON_NAME_NAME)
                {
                    result.Name = elem.value;
                }
                if (elem.name == WebServiceViewModel.JSON_NAME_DESCRIPTION)
                {
                    result.Description = elem.value;
                }
                if (elem.name == WebServiceViewModel.JSON_NAME_SERVICEKEY)
                {
                    result.ServiceKey = elem.value;
                }
                if (elem.name == WebServiceViewModel.JSON_NAME_SERVICETYPE)
                {
                    if (elem.selectedValue != null)
                    {
                        result.ServiceType = (WebServiceType)elem.selectedValue.value;
                    }
                }
            }

            var attrHead = DbContext.GetRegisterAttributeHeadDao().GetCurrentHeadForRegister(result.RegisterId);
            result.RegisterAttrHeadId = attrHead.Id;

            return result;
        }
    }
}
