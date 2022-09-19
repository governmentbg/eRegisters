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
    public class RegisterRightsJsonHelper : BaseJsonHelper
    {
        public RegisterRightsJsonHelper(ISmartRegistryContext context)
            : base(context)
        {

        }

        public IList<ControlModelBase> GetRegisterRightsControls(
            Register register, 
            IList<RegisterPermission> allPermissions,
            IList<UserGroup> allowedUserGroups)
        {
            var result = new List<ControlModelBase>();

            result.Add(new ControlModelHidden()
            {
                Col = 12,
                Label = "",
                Name = "Id",
                Value = (register == null) ? string.Empty : register.Id.ToString()
            });

            var repeater = new ControlModelRepeater();
            repeater.Col = 12;
            repeater.Label = "Права за Регистър";
            repeater.Name = "UserRightsRepeater";
            repeater.ItemTemplate = CreateRightsItemTemplate(register, null, allowedUserGroups, allPermissions);

            foreach (var userGroup in allowedUserGroups)
            {
                var newItem = CreateRightsItemTemplate(register, userGroup, allowedUserGroups, allPermissions);
                if (newItem != null)
                {
                    repeater.Values.Add(newItem);
                }
            }

            result.Add(repeater);

            return result;
        }

        private ControlModelRepeaterItemTemplate CreateRightsItemTemplate(Register register, UserGroup userGroup, IList<UserGroup> allUserGroups, IList<RegisterPermission> permissions)
        {
            IList<RegisterRight> rights = null;
            if (userGroup != null)
            {
                rights = userGroup.RegisterRights.Where(x => (x.Register == register) && x.HasRight).ToList();
                if ((rights == null) || (rights.Count == 0))
                {
                    return null;
                }
            }

            var result = new ControlModelRepeaterItemTemplate();

            result.ElementId = (userGroup == null) ? string.Empty : userGroup.Id.ToString();

            var userGroupsCombo = new ControlModelOptionList()
            {
                Col = 12,
                Label = "Потребителска група",
                Name = "UserGroupCombo",
                Required=true,
                Enabled = (userGroup == null)
            };
            foreach (var uGr in allUserGroups)
            {
                var userGroupOption = new ControlModelOptionElement()
                {
                    Label = uGr.Name,
                    Value = uGr.Id.ToString()
                };
                userGroupsCombo.Options.Add(userGroupOption);

                if ((userGroup != null) && (uGr.Id == userGroup.Id))
                {
                    userGroupsCombo.SelectedValue = userGroupOption;
                }
            }

            result.Controls.Add(userGroupsCombo);

            foreach (var perm in permissions)
            {
                bool isChecked = true;
                if (userGroup != null)
                {
                    isChecked = userGroup.RegisterRights.Any(
                        x => (x.Permission == perm.Id) && x.HasRight);
                }

                var checkBox = new ControlModelCheckbox()
                {
                    Col = 12,
                    Label = perm.Name,
                    Name = "permission" + ((int)perm.Id).ToString(),
                    Checked = isChecked
                };
                result.Controls.Add(checkBox);
            }

            return result;
        }

        public RegisterRightsViewModel ParseRightsFromJson(string json)
        {
            var result = new RegisterRightsViewModel();

            dynamic registerRightsData = JsonConvert.DeserializeObject(json);

            var idElem = registerRightsData[0];
            if (idElem.name != "Id") throw new Exception("Invalid JSON Register Rights data : missing Id as first element!");
            if (idElem.value == string.Empty) throw new Exception("Invalid JSON Register Rights data : id register not set!");

            result.RegisterId = (int)idElem.value;

            foreach (var rootElem in registerRightsData)
            {
                if (rootElem.name == "UserRightsRepeater")
                {
                    foreach (var usrGrTemplate in rootElem.values)
                    {
                        RegisterRightViewModel procUsrGrp = ProcessUserGroupElement(usrGrTemplate);
                        result.UserGroupRegisterRights.Add(procUsrGrp);
                    }
                }
            }

            return result;
        }

        private RegisterRightViewModel ProcessUserGroupElement(dynamic usrGrTemplate)
        {
            var result = new RegisterRightViewModel();

            foreach (var elem in usrGrTemplate.controls)
            {
                string elemName = elem.name;

                if (elemName == "UserGroupCombo")
                {
                    long usrGroupId;
                    if ((elem.selectedValue == null) || (elem.selectedValue.value == null))
                    {
                        result.UserGroupId = 0;
                    }
                    else
                    {
                        result.UserGroupId = DynamicToLong(elem.selectedValue.value);
                    }
                }

                if (elemName.StartsWith("permission"))
                {
                    var perm = (RegisterPermissionEnum)int.Parse(elemName.Substring(10));
                    result.Rights[perm] = elem.value;
                }
            }

           return result;
        }
    }
}
