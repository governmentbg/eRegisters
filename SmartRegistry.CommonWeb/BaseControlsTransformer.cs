using SmartRegistry.CommonWeb.JsonControls;
using SmartRegistry.Domain.Interfaces;
using SmartRegistry.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.CommonWeb
{
    public class BaseControlsTransformer
    {
        protected ISmartRegistryContext SmartContext { get; private set; }

        protected IDbContext DbContext
        {
            get
            {
                return SmartContext.DbContext;
            }
        }

        public BaseControlsTransformer(ISmartRegistryContext smartContext)
        {
            SmartContext = smartContext;
        }

        protected int DynamicToInt(dynamic elemValue)
        {
            int result = 0;
            if (elemValue is string)
            {
                result = int.Parse(elemValue);
            }
            else
            {
                result = (int)elemValue;
            }
            return result;
        }

        protected long DynamicToLong(dynamic elemValue)
        {
            long result = 0;
            if (elemValue is string)
            {
                result = long.Parse(elemValue);
            }
            else
            {
                result = (long)elemValue;
            }
            return result;
        }

        protected void AddStatusControl(IList<ControlModelBase> listControls, bool isActive)
        {
            var statusControl = new ControlModelOptionList()
            {
                Col = 12,
                Label = "Статус",
                Name = "Status"
            };

            var activeStatus = new ControlModelOptionElement()
            {
                Label = "Активен",
                Value = "1"
            };
            var inActiveStatus = new ControlModelOptionElement()
            {
                Label = "Неактивен",
                Value = "2"
            };

            statusControl.Options.Add(activeStatus);
            statusControl.Options.Add(inActiveStatus);
            if ((isActive))
            {
                statusControl.SelectedValue = activeStatus;
            }
            else
            {
                statusControl.SelectedValue = inActiveStatus;
            }
            listControls.Add(statusControl);
        }

    }
}
