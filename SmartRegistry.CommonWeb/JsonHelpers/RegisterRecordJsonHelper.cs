using Newtonsoft.Json;
using SmartRegistry.CommonWeb.JsonControls;
using SmartRegistry.Domain.Entities;
using SmartRegistry.Domain.Interfaces;
using SmartRegistry.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.CommonWeb.JsonHelpers
{
    public class RegisterRecordJsonHelper : BaseJsonHelper
    {
        public RegisterRecordJsonHelper(ISmartRegistryContext context)
            : base(context)
        {

        }

        public IList<ControlModelBase> GetRecordEditControls(string relativePathBase, IList<RegisterAttribute> allAttributes, RegisterRecord regRecord, int registerId, int? stateId)
        {
            var result = new List<ControlModelBase>();
            var resultComposites = new List<ControlModelFieldset>();

            result.Add(new ControlModelHidden()
            {
                Col = 12,
                Label = "",
                Name = "RegisterId",
                Value = registerId.ToString()
            });

            result.Add(new ControlModelHidden()
            {
                Col = 12,
                Label = "",
                Name = "StateId",
                Value = (stateId == null) ?  "0" : stateId.ToString()
            });

            result.Add(new ControlModelHidden()
            {
                Col = 12,
                Label = "",
                Name = "Id",
                Value = (regRecord == null) ? string.Empty : regRecord.Id.ToString()
            });

            result.Add(new ControlModelText()
            {
                Col = 12,
                Label = "УРИ",
                Name = "URI",
                Value = (regRecord == null) ? "*генерира се*" : regRecord.URI,
                Enabled = false

            });
            
            foreach (var attr in allAttributes)
            {
                if (attr.ParentAttribute != null) {
                    continue;
                }

                if (attr.UnifiedData.DataType == UnifiedDataTypeEnum.Composite){
                    var fieldsetModel = new ControlModelFieldset();
                    fieldsetModel.Label = attr.Name;
                    fieldsetModel.Name = "Attribute_" + attr.Id.ToString();
                    fieldsetModel.Controls = new List<ControlModelBase>();

                    var listst = getAllAttributes(allAttributes,attr);

                    foreach (var compElement in listst)
                    {                       
                        var ctrl = CreateAttributeControl(relativePathBase, compElement, regRecord);

                        if (ctrl != null)
                        {
                            fieldsetModel.Controls.Add(ctrl);
                        }
                    }

                    result.Add(fieldsetModel);

                }else {
                    var ctrl = CreateAttributeControl(relativePathBase, attr, regRecord);

                    if (ctrl != null)
                    {
                        result.Add(ctrl);
                    }
                }

            }



            return result;
        }

        private List<RegisterAttribute> getAllAttributes(IList<RegisterAttribute> regList,RegisterAttribute atr)
        {
            List<RegisterAttribute> attrList = new List<RegisterAttribute>();                 
            var subList = regList.Where(x=>x.ParentAttribute==atr).ToList();
            foreach (var subAttri in subList) {

                var elements = regList.Where(x => x.ParentAttribute == subAttri).ToList();

                if (elements.Count == 0)
                {
                    attrList.Add(subAttri);
                }
                else
                {
                    var testagain = getAllAttributes(regList, subAttri);
                    attrList.AddRange(testagain);
                }
            }

            return attrList;

        }


        private ControlModelBase CreateAttributeControl(string relativePathBase, RegisterAttribute attr, RegisterRecord regRecord)
        {
            ControlModelBase result = null;

            switch (attr.UnifiedData.DataType)
            {
                case UnifiedDataTypeEnum.String:
                    result = CreateStringAttributeControl(attr, regRecord);
                    break;
                case UnifiedDataTypeEnum.Text:
                    result = CreateTextAttributeControl(attr, regRecord);
                    break;
                case UnifiedDataTypeEnum.Date:
                    result = CreateDateAttributeControl(attr, regRecord);
                    break;
                case UnifiedDataTypeEnum.Integer:
                    result = CreateIntAttributeControl(attr, regRecord);
                    break;
                case UnifiedDataTypeEnum.Decimal:
                    result = CreateDecimalAttributeControl(attr, regRecord);
                    break;
                case UnifiedDataTypeEnum.Referential:
                    result = CreateReferentialAttributeControl(relativePathBase, attr, regRecord);
                    break;

            }

            if (result != null)
            {
                result.Label = attr.Name;
                result.Name = "Attribute_" + attr.Id.ToString();
            }
          
            return result;
        }

        private ControlModelOptionList CreateReferentialAttributeControl(string relativePathBase, RegisterAttribute attr, RegisterRecord regRecord)
        {
            RegisterRecord refRec = null;
            if (regRecord != null)
            {
                refRec = regRecord.GetReferentialRecordForAttribute(attr, SmartContext);
            }

            //var result = new ControlModelAjaxInput()
            //{
            //    Value = (refRec == null) ? null : refRec.Id.ToString(),
            //    DisplayValue = (refRec == null) ? null : regRecord.GetReferentialTextForAttribute(attr, SmartContext),
            //    AjaxUrl = relativePathBase + "RegisterRecords/GetDataForRefAttribute?id=" + attr.Id.ToString()
            //};

            var result = new ControlModelOptionList();

            var regRecService = SmartContext.RegisterRecordsService;
            var refDataList = regRecService.GetRefAttributeData(attr.Id, string.Empty, string.Empty);

            foreach(var pair in refDataList)
            {
                var optElem = new ControlModelOptionElement()
                {
                    Label = pair.Value,
                    Value = pair.Key
                };

                result.Options.Add(optElem);

                if ((refRec != null) && (refRec.Id.ToString() == pair.Key))
                {
                    result.SelectedValue = optElem;
                }
            }

            return result;
        }

        private ControlModelText CreateStringAttributeControl(RegisterAttribute attr, RegisterRecord regRecord)
        {
            string strValue = null;
            if (regRecord != null)
            {
                var attrValue = regRecord.GetVarcharValue(attr);
                if (attrValue != null)
                {
                    strValue = attrValue.Value;
                }
            }

            var result = new ControlModelText()
            {
                Value = strValue
            };

            return result;
        }

        public IList<AjaxInputResultObject> GetRefAttributeAjaxList(IList<KeyValuePair<string, string>> refDataList)
        {
            var result = new List<AjaxInputResultObject>();

            foreach(var pair in refDataList)
            {
                result.Add(new AjaxInputResultObject(pair.Key, pair.Value));
            }

            return result;
        }

        private ControlModelText CreateTextAttributeControl(RegisterAttribute attr, RegisterRecord regRecord)
        {
            string strValue = null;
            if (regRecord != null)
            {
                var attrValue = regRecord.GetTextValue(attr);
                if (attrValue != null)
                {
                    strValue = attrValue.Value;
                }
            }

            var result = new ControlModelMultilineText()
            {
                Value = strValue
            };

            return result;
        }

        private ControlModelNumber CreateIntAttributeControl(RegisterAttribute attr, RegisterRecord regRecord)
        {
            int strValue = 0;
            if (regRecord != null)
            {
                var attrValue = regRecord.GetIntValue(attr);
                if ((attrValue != null) && (attrValue.Value != null))
                {
                    strValue = (int)attrValue.Value;
                }
            }

            var result = new ControlModelNumber
            {
                Value = Decimal.Parse(strValue.ToString()),
                Step = 1
            };

            return result;
        }
        private ControlModelNumber CreateDecimalAttributeControl(RegisterAttribute attr, RegisterRecord regRecord)
        {
            decimal strValue = 0;
            if (regRecord != null)
            {
                var attrValue = regRecord.GetDecimalValue(attr);
                if ((attrValue != null) && (attrValue.Value != null))
                {
                    strValue = (decimal)attrValue.Value;
                }
            }

            var result = new ControlModelNumber()
            {
                Value = strValue,
                Step = 0.0001M
            };

            return result;
        }

        private ControlModelFieldset CreateCompositeAttributeControl(RegisterAttribute attr, RegisterRecord regRecord)
        {

            string strValue = null;
            //if (regRecord != null)
            //{
            //    var attrValue = regRecord.GetVarcharValue(attr);
            //    if (attrValue != null)
            //    {
            //        strValue = attrValue.Value;
            //    }

            //}
            var fieldsetModel = new ControlModelFieldset();
            fieldsetModel.Label = attr.UnifiedData.Name;
            fieldsetModel.Controls = new List<ControlModelBase>();

            //foreach (UnifiedDataComposite ud in attr.UnifiedData.CompositeList) {


            //}

       

            return fieldsetModel;
        }


        private ControlModelDate CreateDateAttributeControl(RegisterAttribute attr, RegisterRecord regRecord)
        {
            DateTime? dtValue = null;
            if (regRecord != null)
            {
                var attrValue = regRecord.GetDateTimeValue(attr);
                if (attrValue != null)
                {
                    dtValue = attrValue.Value;
                }
            }

            var result = new ControlModelDate()
            {
                Value = dtValue
            };

            return result;
        }

        public RegisterRecordViewModel ParseAttributeValuesFromJson(string json)
        {
            var result = new RegisterRecordViewModel();

            dynamic regRecordData = JsonConvert.DeserializeObject(json);

            foreach (var elem in regRecordData)
            {
                string elemName = elem.name;
                if (elemName == "Id")
                {
                    result.Id = DynamicToLong(elem.value);
                }
                else if (elemName == "RegisterId")
                {
                    result.RegisterId = elem.value;
                }
                else if (elemName == "StateId")
                {
                    result.StateId = elem.value;
                }
                else if (elemName == "URI") {
                    if ((bool)elem.enabled)
                    {
                        result.URI = elem.value;
                    }
                    else {
                        result.URI = string.Empty;
                    }
                 
                }
                else
                {
                    if (elemName.StartsWith("Attribute_"))
                    {                       
                        if (elem.type == "fieldset")
                        {
                            foreach (var selElem in elem.controls)
                            {
                                string elName = selElem.name;
                                int attrId2 = int.Parse(elName.Substring(10));
                                result.ValueList.Add(attrId2, GetValueForAttributeFromJson(selElem));
                            }
                        }
                        else {
                            int attrId = int.Parse(elemName.Substring(10));
                            result.ValueList.Add(attrId, GetValueForAttributeFromJson(elem));
                        }
                    }
                }
            }

            return result;
        }

        private object GetValueForAttributeFromJson(dynamic elem)
        {
            object result = null;
            string elemType = elem.type;

            switch (elemType)
            {
                case "textarea":
                case "text":
                    result = (string)elem.value;
                    break;
                case "date":
                case "datetime":
                    try
                    {
                        DateTime dtVal = elem.value;
                        result = dtVal;
                        //result = DateTime.Parse(dtVal);
                        //result = DateTime.ParseExact(dtVal, "yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss.fffffffK",
                        //System.Globalization.CultureInfo.InvariantCulture);
                        // result = DateTime.ParseExact(dtVal,"yyyy-MM-dd",CultureInfo.InvariantCulture);
                    }
                    catch {
                        result = null;
                    }
                 
                    break;
                case "number":                   
                    result = (decimal)elem.value;
                    break;
                case "select":
                    if (elem.selectedValue != null)
                    {
                        result = (string)elem.selectedValue.value;
                    }
                    break;

            }

            return result;
        }
    }
}
