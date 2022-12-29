using SmartRegistry.CommonWeb.JsonControls;
using SmartRegistry.Domain.Interfaces;
using SmartRegistry.Domain.QueryFilters;
using SmartRegistry.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.CommonWeb
{
    public class BaseJsonHelper
    {
        protected ISmartRegistryContext SmartContext { get; private set; }

        protected IDbContext DbContext
        {
            get
            {
                return SmartContext.DbContext;
            }
        }

        public BaseJsonHelper(ISmartRegistryContext smartContext)
        {
            SmartContext = smartContext;
        }

        protected int? DynamicToInt(dynamic elemValue)
        {
            int? result = null;
            if (elemValue != null)
            {
                if (elemValue.Value.GetType() == typeof(string))
                {
                    string strValue = (string)elemValue;
                    if (!string.IsNullOrEmpty(strValue))
                    {
                        result = int.Parse(strValue);
                    }
                }
                else
                {
                    result = (int?)elemValue;
                }
            }
            return result;
        }

        protected long? DynamicToLong(dynamic elemValue)
        {
            long? result = null;
            if (elemValue != null)
            {
                if (elemValue.Value.GetType() == typeof(string))
                {
                    string strValue = (string)elemValue;
                    if (!string.IsNullOrEmpty(strValue))
                    {
                        result = long.Parse(strValue);
                    }
                }
                else
                {
                    result = (long?)elemValue;
                }
            }
            return result;
        }

        protected void AddStatusControl(IList<ControlModelBase> listControls, bool? isActive, string controlName = "StatusFilter")
        {
            var statusControl = new ControlModelOptionList()
            {
                Col = 3,
                Label = Properties.Content.base_status_name,
                Required = true,
                Name = controlName
            };


            var noStatus = new ControlModelOptionElement()
            {
                Label = Properties.Content.base_status_option_0,
                Value = "0"
            };
            var activeStatus = new ControlModelOptionElement()
            {
                Label = Properties.Content.base_status_option_1,
                Value = "1"
            };
            var inActiveStatus = new ControlModelOptionElement()
            {
                Label = Properties.Content.base_status_option_2,
                Value = "2"
            };
            statusControl.Options.Add(noStatus);
            statusControl.Options.Add(activeStatus);
            statusControl.Options.Add(inActiveStatus);
            if (isActive == null)
            {
                statusControl.SelectedValue = noStatus;
            }
            else
            {
                if (((bool)isActive))
                {
                    statusControl.SelectedValue = activeStatus;
                }
                else
                {
                    statusControl.SelectedValue = inActiveStatus;
                }
            }
            listControls.Add(statusControl);
        }

        protected bool? CheckForStatusValue(dynamic jsonElem)
        {
            bool? result = null;
            if (jsonElem.name == "StatusFilter")
            {
                if (jsonElem.selectedValue != null)
                {
                    int statFltValue = jsonElem.selectedValue.value;
                    if (statFltValue == 1) result = true;
                    if (statFltValue == 2) result = false;
                }
            }
            return result;
        }

        protected void AddPageAndOrderToFilter(QueryFilterBase filter, dynamic jsonElem)
        {
            if (jsonElem.currentPage != null)
            {
                filter.PageNumber = (int)jsonElem.currentPage.Value;
            }
            if (jsonElem.numberOfRowsPerPage != null)
            {
                filter.PageSize = (int)jsonElem.numberOfRowsPerPage.Value;
            }
            if (jsonElem.orderColumn != null)
            {
                filter.OrderByColumn = (string)jsonElem.orderColumn.Value;
            }
            if (jsonElem.orderDirection != null)
            {
                string fltVal = (string)jsonElem.orderDirection.Value;
                if ("asc".Equals(fltVal))
                {
                    filter.OrderByDirection = OrderDbEnum.Asc;                  
                }
                if ("desc".Equals(fltVal))
                {
                    filter.OrderByDirection = OrderDbEnum.Desc;                   
                }
            }
        }



    }
}
