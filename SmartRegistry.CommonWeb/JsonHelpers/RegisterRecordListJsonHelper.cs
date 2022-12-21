using Newtonsoft.Json;
using SmartRegistry.CommonWeb.JsonControls;
using SmartRegistry.Domain.Common;
using SmartRegistry.Domain.Entities;
using SmartRegistry.Domain.Interfaces;
using SmartRegistry.Domain.QueryFilters;
using SmartRegistry.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SmartRegistry.CommonWeb.JsonHelpers
{
    public class RegisterRecordListJsonHelper : BaseJsonHelper
    {
        public RegisterRecordListJsonHelper(ISmartRegistryContext context)
            : base(context)
        {

        }

        public TableDataModel GetRegisterRecordsTable(string relativePathBase,
            PagedResult<RegisterRecord> registerRecords,
            int registerId,
            IList<RegisterAttribute> allAttributes, RegisterListType publicSite)
        {
           
            var result = new TableDataModel();
            result.CurrentPage = 1;
            result.NumberOfRowsPerPage = 5;
            result.TotalPages = 1;

            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "URI",
                Label = "URI",
                Sortable = false
            });

            foreach (var attr in allAttributes)
            {
                if (attr.UnifiedData.DataType != UnifiedDataTypeEnum.Composite)
                {
                    result.Columns.Add(new TableColumnTitleModel()
                    {
                        Key = "ColumnAttribute_" + attr.Id.ToString(),
                        Label = attr.Name,
                        Sortable = false
                    });
                }
            }

            var regHead = DbContext.GetRegisterAttributeHeadDao().GetCurrentHeadForRegister(registerId);
            if (regHead.RegisterStatesList.Count != 0)
            {
                result.Columns.Add(new TableColumnTitleModel()
                {
                    Key = "State",
                    Label = Properties.Content.base_status_name,
                    Sortable = false
                });
            }

            result.Columns.Add(new TableColumnTitleModel()
            {
                Key = "Actions",
                Label = Properties.Content.base_options,
                Sortable = false
            });


            result.CurrentPage = (int)registerRecords.CurrentPage;
            result.NumberOfRowsPerPage = (int)registerRecords.PageSize;
            result.TotalPages = registerRecords.PageCount;

            foreach (var regRec in registerRecords.Results)
            {
                var row = new TableDataRowModel();
               

                row.ObjectId = regRec.Id.ToString();
                row.AddTextCell("URI", (regRec.URI == null) ? string.Empty : regRec.URI);

                foreach (var recAttr in allAttributes)
                {
                    if (recAttr.UnifiedData.DataType != UnifiedDataTypeEnum.Composite)
                    {
                        var textVal = string.Empty;
                        var regRecVal = regRec.GetValue(recAttr);
                        if (regRecVal != null)
                        {
                            if (recAttr.UnifiedData.DataType == UnifiedDataTypeEnum.Referential)
                            {
                                textVal = regRec.GetReferentialTextForAttribute(recAttr, SmartContext);
                            }
                            else
                            {
                                textVal = regRecVal.ToString();
                            }
                        }
                        row.AddTextCell("ColumnAttribute_" + recAttr.Id.ToString(), textVal);
                    }
                }

                if (regHead.RegisterStatesList.Count != 0)
                {
                    row.AddTextCell("State", (regRec.RegisterState == null) ? string.Empty : regRec.RegisterState.Name);
                }
              

                var actions = new TableDataCellActionsModel();

                if (publicSite != RegisterListType.Public)
                {
                    actions.AddAction("edit", Properties.Content.records_hint_edit, relativePathBase + "RegisterRecords/Edit/" + regRec.Id.ToString());
                    row.AddActionsCell("actions", actions);
                }
                else
                {
                    actions.AddAction("edit", Properties.Content.records_public_hint_edit, relativePathBase + "Registers/ShowDataText/" + regRec.Id.ToString());
                    row.AddActionsCell("actions", actions);
                }

                result.Rows.Add(row);
            }

            return result;
        }

        public IList<ControlModelBase> GetFilters(IList<RegisterAttribute> filterAttributes, int registerId, RegisterListType regType)
        {
            var result = new List<ControlModelBase>();

            if (filterAttributes != null)
            {
                foreach (var attr in filterAttributes)
                {
                    if (regType == RegisterListType.Public)
                    {
                        if (attr.IsPublic)
                        {
                            CreateFilterForAttribute(result, attr);
                        }
                    }
                    else
                    {
                        CreateFilterForAttribute(result, attr);
                    }

                }
            }

            var regHead = DbContext.GetRegisterAttributeHeadDao().GetCurrentHeadForRegister(registerId);
            if (regHead.RegisterStatesList.Count != 0)
            {
                result.Add(new ControlModelText()
                {
                    Col = 3,
                    Label = "Статус",
                    Name = "State",
                    Value = ""
                });
            }

            return result;
        }

        private void CreateFilterForAttribute(List<ControlModelBase> result, RegisterAttribute attr)
        {
            if (result == null) return;
            if (attr == null) return;

            switch (attr.UnifiedData.DataType)
            {
                case UnifiedDataTypeEnum.String:
                case UnifiedDataTypeEnum.Text:
                    result.Add(new ControlModelText()
                    {
                        Col = 3,
                        Label = attr.Name,
                        Name = "FilterAttribute_" + attr.Id.ToString(),
                        Value = ""
                    });
                    break;
                case UnifiedDataTypeEnum.Integer:
                    result.Add(new ControlModelNumber()
                    {
                        Col = 1,
                        Label = attr.Name,
                        Name = "FilterAttribute_" + attr.Id.ToString(),
                        Value = null,
                        Step = 1
                    });
                    break;
                case UnifiedDataTypeEnum.Decimal:

                    result.Add(new ControlModelNumber()
                    {
                        Col = 1,
                        Label = attr.Name,
                        Name = "FilterAttribute_" + attr.Id.ToString(),
                        Value = null,
                        Step = Convert.ToDecimal(0.0001)
                    });
                    break;
            }
        }

        public RegisterRecordFilter DeserializeFilters(string jsonFilters, int registerId)
        {
            var result = new RegisterRecordFilter();
            result.Register = DbContext.GetRegistersDao().GetById(registerId);

            if (string.IsNullOrEmpty(jsonFilters)) return result;

            dynamic filters = JsonConvert.DeserializeObject(jsonFilters);

            foreach (var jsonFilter in filters)
            {
                string filterName = jsonFilter.name;

                if ((!string.IsNullOrEmpty(filterName)) && (filterName.StartsWith("FilterAttribute_")))
                {
                    var attrId = int.Parse(filterName.Substring(16));

                    var attribute = DbContext.GetRegisterAttributeDao().GetById(attrId);
                    AddAttributeFilter(attribute, result, jsonFilter);
                }

                if ((!string.IsNullOrEmpty(filterName)) && (filterName.Equals("State"))) {                  
                    result.State = "%" + jsonFilter.value + "%";
                }

                AddPageAndOrderToFilter(result, jsonFilter);
            }


            return result;
        }

        private void AddAttributeFilter(RegisterAttribute attribute, RegisterRecordFilter result, dynamic jsonFilter)
        {
            if (attribute == null) return;

            switch (attribute.UnifiedData.DataType)
            {
                case UnifiedDataTypeEnum.String:
                case UnifiedDataTypeEnum.Text:
                    string fltVal = jsonFilter.value;
                    if (!string.IsNullOrEmpty(fltVal))
                    {
                        result.AttributeFilters.Add(new RegisterAttributeTextFilter()
                        {
                            Attribute = attribute,
                            FilterValue = "%" + fltVal + "%"
                        });
                    }
                    break;
                case UnifiedDataTypeEnum.Integer:
                    string fltValInt = jsonFilter.value;
                    if (!string.IsNullOrEmpty(fltValInt))
                    {
                        int attrVal = Int32.Parse(fltValInt.ToString());                      
                        result.AttributeFilters.Add(new RegisterAttributeIntFilter()
                        {
                            Attribute = attribute,
                            FilterValue = attrVal
                        });
                       
                    }
                    break;
                case UnifiedDataTypeEnum.Decimal:
                    string fltDcmValue = jsonFilter.value;
                    if (!string.IsNullOrEmpty(fltDcmValue))
                    {
                        fltDcmValue = fltDcmValue.Replace(",", ".");
                        var value = decimal.Parse(fltDcmValue, new NumberFormatInfo() { NumberDecimalSeparator = "." });
                        result.AttributeFilters.Add(new RegisterAttributeDecimalFilter()
                        {
                                Attribute = attribute,
                                FilterValue = value
                        });                        
                    }
                    break;
            }
        }

    }
}

