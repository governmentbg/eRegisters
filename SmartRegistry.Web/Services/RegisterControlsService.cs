using SmartRegistry.Web.Models;
using SmartRegistry.Web.Models.ControlModels;
using System;
using System.Collections.Generic;

namespace SmartRegistry.Web.Services
{
    public class RegisterControlsService : IControlsService
    {
        public PageControlsModel Create(int id)
        {
            PageControlsModel ctrls = new PageControlsModel
            {
                Id = id
            };

            ctrls.Fields.Add(
               new PageControl()
               {
                   Id = 324,
                   Label = "УРИ",
                   Required = false,
                   Type = ControlType.Text,
                   Value = ""
               }
           );

            ctrls.Fields.Add(
                new PageControl()
                {
                    Id = 325,
                    Label = "Наименование",
                    Required = false,
                    Type = ControlType.Text,
                    Value = ""
                }
            );

            ctrls.Fields.Add(new SelectControl()
            {
                Id = 24,
                Label = "Административен орган",
                Required = false,
                Type = ControlType.Select,
                Value = "",
                Options = new List<Option>()
                  {
                      new Option(){Label="Председател на Патентно ведомство 1", Value= 1 },
                      new Option(){Label="Председател на Патентно ведомство 2", Value= 2 },
                      new Option(){Label="Председател на Патентно ведомство 3", Value= 3 },
                      new Option(){Label="Председател на Патентно ведомство 4", Value= 4 }
                  }
            });

            var options = new List<Option>() {
                new Option() { Label = "Земеделие", Value = 1 },
                new Option(){Label="Транспорт", Value= 2 },
                new Option(){Label="Туризм", Value= 3 },
                new Option(){Label="Банков сектор", Value= 4 }
            };

            //options.Add("Район на планиране", new List<Option>() {
            //    new Option() { Label = "Югозападен", Value =  5},
            //    new Option(){Label="Южен централен", Value= 6 },
            //    new Option(){Label="Югоизточен", Value= 7 },
            //    new Option(){Label="Североизточен", Value= 8 },
            //    new Option(){Label="Северен", Value= 9 }
            //});

            var multiSelectOption = new MultiSelectOption();
            multiSelectOption.GroupValues = options;


            ctrls.Fields.Add(new MultiSelectControl()
            {
                Id = 24,
                Label = "Сектор",
                Required = false,
                Type = ControlType.Multiselect,
                Value = "",
                Options = new List<MultiSelectOption>() { multiSelectOption }
            });

            ctrls.Fields.Add(
               new PageControl()
               {
                   Id = 325,
                   Label = "Закон",
                   Required = false,
                   Type = ControlType.Text,
                   Value = ""
               }
           );

            ctrls.Fields.Add(
               new PageControl()
               {
                   Id = 325,
                   Label = "URL",
                   Required = false,
                   Type = ControlType.Text,
                   Value = ""
               }
           );

            ctrls.Fields.Add(
               new PageControl()
               {
                   Id = 325,
                   Label = "Законово основание за водене на регистър",
                   Required = false,
                   Type = ControlType.Textarea,
                   Value = ""
               }
           );

            ctrls.Fields.Add(new SelectControl()
            {
                Id = 24,
                Label = "Административен орган",
                Required = false,
                Type = ControlType.Select,
                Value = "",
                Options = new List<Option>()
                  {
                      new Option(){Label="Активен", Value= 1 },
                      new Option(){Label="Неактивен", Value= 2 }
                  }
            });

            return ctrls;
        }
    }
}
