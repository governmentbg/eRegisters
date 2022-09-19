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
    public class TestControlsTransformer : BaseControlsTransformer
    {
        public TestControlsTransformer(ISmartRegistryContext appContext)
            : base(appContext)
        {
        }

        public IList<ControlModelBase> GetRepeaterControls(string relativePathBase)
        {
            var result = new List<ControlModelBase>();

            result.Add(new ControlModelHidden()
            {
                Col = 12,
                Label = "",
                Name = "Id",
                Value = 23.ToString()
            });

            result.Add(new ControlModelAjaxInput()
            {
                Col = 12,
                Label = "Регистър",
                Name = "Register",
                Value = 25.ToString(),
                DisplayValue = "Регистър на спортни обединения",
                AjaxUrl = relativePathBase + "Registers/GetAllRegistersAjax?id=32432",
                ParamControlName = string.Empty
            });

            result.Add(new ControlModelAjaxInput()
            {
                Col = 12,
                Label = "Колона",
                Name = "Id",
                Value = 288.ToString(),
                DisplayValue = "Наименование",
                AjaxUrl = relativePathBase + "Registers/GetRegisterColsAjax?id=32432",
                ParamControlName = "Register"
            });


            AddGroupComboBox(result);

            result.Add(new ControlModelText()
            {
                Col = 12,
                Label = "Наименование",
                Name = "Name",
                Value = "Забранена редакция",
                Enabled = true,
                Required = true
            });

            result.Add(new ControlModelCheckbox()
            {
                Col = 12,
                Label = "Връща обща грешка",
                Name = "HasCommonError",
                Checked = true
            });

            result.Add(new ControlModelCheckbox()
            {
                Col = 12,
                Label = "Връща грешка за контроли",
                Name = "HasControlErrors",
                Checked = true
            });

            result.Add(new ControlModelPassword()
            {
                Col = 12,
                Label = "Парола",
                Name = "Password",
                Value = "12345"
            });

            result.Add(new ControlModelNumber()
            {
                Col = 12,
                Label = "Нецяло число",
                Name = "NumericField",
                Step = 0.0001M,
                Value = 0.23M
            });

            result.Add(new ControlModelNumber()
            {
                Col = 12,
                Label = "Цяло число",
                Name = "IntegerField",
                Step = 1M,
                Value = 3M,
                MinValue = 0M
            });

            result.Add(new ControlModelDate()
            {
                Col = 12,
                Label = "Дата",
                Name = "Date",
                Value = DateTime.Today
            });

            result.Add(new ControlModelDateTime()
            {
                Col = 12,
                Label = "Дата и час",
                Name = "DateTime",
                Value = DateTime.Now
            });

            result.Add(new ControlModelMultilineText()
            {
                Col = 12,
                Label = "Описание",
                Name = "Description",
                Rows = 4,
                Value = "Първи ред" + Environment.NewLine + "втори ред"
                + Environment.NewLine + "трети ред"
            });

            var repeater = new ControlModelRepeater();
            repeater.Col = 12;
            repeater.Label = "Права за достъп";
            repeater.Name = "UserRights";
            repeater.ItemTemplate = CreateItemTemplate(string.Empty, 1, false, false);
            repeater.Values.Add(CreateItemTemplate("1011", 1, true, false));
            repeater.Values.Add(CreateItemTemplate("1012", 3, false, true));
            result.Add(repeater);


            var fieldsetModel = new ControlModelFieldset();
            fieldsetModel.Label = "Тестов фиелдсет1";
            fieldsetModel.Controls = new List<ControlModelBase>();

            fieldsetModel.Controls.Add(new ControlModelText()
            {
                Col = 12,
                Label = "Наименование",
                Name = "Name",
                Value = "Забранена редакция",
                Enabled = false
            });

            fieldsetModel.Controls.Add(new ControlModelDate()
            {
                Col = 12,
                Label = "Дата",
                Name = "Date",
                Value = DateTime.Today
            });

            fieldsetModel.Controls.Add(new ControlModelCheckbox()
            {
                Col = 12,
                Label = "Връща обща грешка",
                Name = "HasCommonError",
                Checked = true
            });

            result.Add(fieldsetModel);

            return result;
        }


        public ControlModelFieldset GetFieldsetControl()
        {
           
            var result = new ControlModelFieldset();
            result.Label = "Тестов фиелдсет1";
           // result.Type = "fieldset";
            result.Controls = new List<ControlModelBase>();


            result.Controls.Add(new ControlModelText()
            {
                Col = 12,
                Label = "Наименование",
                Name = "Name",
                Value = "Забранена редакция",
                Enabled = false
            });

            result.Controls.Add(new ControlModelDate()
            {
                Col = 12,
                Label = "Дата",
                Name = "Date",
                Value = DateTime.Today
            });

            result.Controls.Add(new ControlModelCheckbox()
            {
                Col = 12,
                Label = "Връща обща грешка",
                Name = "HasCommonError",
                Checked = true
            });

            return result;
        }

        private void AddGroupComboBox(List<ControlModelBase> result)
        {
            var combo = new ControlModelOptionMultiList();
            combo.Label = "Тест Избор от групи!";

            var group1 = new ControlModelOptionGroup()
            {
                Label = "Главна Група 1"
            };

            group1.Options.Add(new ControlModelOptionElement()
            {
                Label = "Подгрупа 1.1",
                Value = "1.1"
            });
            group1.Options.Add(new ControlModelOptionElement()
            {
                Label = "Подгрупа 1.2",
                Value = "1.2"
            });
            group1.Options.Add(new ControlModelOptionElement()
            {
                Label = "Подгрупа 1.3",
                Value = "1.3"
            });
            combo.AddOptionGroup(group1);


            var group2 = new ControlModelOptionGroup()
            {
                Label = "Главна Група 2"
            };

            group2.Options.Add(new ControlModelOptionElement()
            {
                Label = "Подгрупа 2.1",
                Value = "2.1"
            });
            group2.Options.Add(new ControlModelOptionElement()
            {
                Label = "Подгрупа 2.2",
                Value = "2.2"
            });
            combo.AddOptionGroup(group2);

            result.Add(combo);

        }

        private ControlModelRepeaterItemTemplate CreateItemTemplate(string elementId, int comboId, bool firstCheck, bool secondCheck)
        {
            var result = new ControlModelRepeaterItemTemplate();

            result.ElementId = elementId;

            var combo = new ControlModelOptionList()
            {
                Col = 12,
                Label = "Потребителска група",
                Name = "UserGroupName"
            };

            var grp1 = new ControlModelOptionElement()
            {
                Label = "Потребителска група 1",
                Value = "1"
            };
            var grp2 = new ControlModelOptionElement()
            {
                Label = "Потребителска група 2",
                Value = "2"
            };
            var grp3 = new ControlModelOptionElement()
            {
                Label = "Потребителска група 3",
                Value = "3"
            };
            var grp4 = new ControlModelOptionElement()
            {
                Label = "Потребителска група 4",
                Value = "4"
            };

            combo.Options.Add(grp1);
            combo.Options.Add(grp2);
            combo.Options.Add(grp3);
            combo.Options.Add(grp4);
            if (comboId == 1) { combo.SelectedValue = grp1;  }
            else if (comboId == 2) { combo.SelectedValue = grp2; }
            else if (comboId == 3) { combo.SelectedValue = grp3; }
            else { combo.SelectedValue = grp4; }
            result.Controls.Add(combo);

            result.Controls.Add(new ControlModelCheckbox()
            {
                Col = 12,
                Label = "Право 1",
                Name = "HasRight1",
                Checked = firstCheck
            });
            result.Controls.Add(new ControlModelCheckbox()
            {
                Col = 12,
                Label = "Право 2",
                Name = "HasRight2",
                Checked = secondCheck
            });

            return result;
        }
    }
}

