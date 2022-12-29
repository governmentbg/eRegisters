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
    public class UserGroupJsonHelper : BaseJsonHelper
    {
        public UserGroupJsonHelper(ISmartRegistryContext context)
            : base(context)
        {

        }

        public IList<ControlModelBase> GetEditControls(UserGroup userGroup)
        {
            var result = new List<ControlModelBase>();

            result.Add(new ControlModelHidden()
            {
                Col = 12,
                Label = "",
                Name = UserGroupViewModel.JSON_NAME_ID,
                Value = (userGroup == null) ? string.Empty : userGroup.Id.ToString()
            });

            result.Add(new ControlModelText()
            {
                Col = 12,
                Label = Properties.Content.usergroup_filter_name,
                Required=true,
                Name = UserGroupViewModel.JSON_NAME_NAME,
                Value = (userGroup == null) ? string.Empty : userGroup.Name
            });

            AddRoleControl(result, userGroup);

            bool? isActive = null;
            if (userGroup != null)
            {
                isActive = userGroup.IsActive;
            }
            AddStatusControl(result, isActive, UserGroupViewModel.JSON_NAME_ISACTIVE);

            AddPermissionControls(result, userGroup);

            return result;
        }

        protected void AddRoleControl(IList<ControlModelBase> listControls, UserGroup userGroup)
        {
            if (!SmartContext.CurrentUser.IsGlobalAdmin) return;

            var roleControl = new ControlModelOptionList()
            {
                Col = 12,
                Label = Properties.Content.usergroup_type,
                Required = true,
                Name = UserGroupViewModel.JSON_NAME_ROLE
            };

            var usersRole = new ControlModelOptionElement()
            {
                Label = Properties.Content.user_role,
                Value = ((int)UserGroupRole.Users).ToString()
            };

            var globalAdminsRole = new ControlModelOptionElement()
            {
                Label = Properties.Content.globaladmin_role,
                Value = ((int)UserGroupRole.GlobalAdmins).ToString()
            };

            var localAdminsRole = new ControlModelOptionElement()
            {
                Label = Properties.Content.localadmin_role,
                Value = ((int)UserGroupRole.LocalAdmins).ToString()
            };

            roleControl.Options.Add(usersRole);
            roleControl.Options.Add(globalAdminsRole);
            roleControl.Options.Add(localAdminsRole);
            if ((userGroup == null) || (userGroup.Role == UserGroupRole.Users))
            {
                roleControl.SelectedValue = usersRole;
            }
            else if (userGroup.Role == UserGroupRole.GlobalAdmins)
            {
                roleControl.SelectedValue = globalAdminsRole;
            }
            else if (userGroup.Role == UserGroupRole.LocalAdmins)
            {
                roleControl.SelectedValue = localAdminsRole;
            }
            listControls.Add(roleControl);
        }

        protected void AddPermissionControls(IList<ControlModelBase> listControls, UserGroup userGroup)
        {
            listControls.Add(new ControlModelCheckbox()
            {
                Col = 12,
                Label = Properties.Content.permission_manageuser,
                Name = "HasRightManageUsers",
                Checked = (userGroup == null) ? false : userGroup.Rights.Any(x => x.Permission == PermissionEnum.ManageUsers)
            });
            listControls.Add(new ControlModelCheckbox()
            {
                Col = 12,
                Label = Properties.Content.permission_deactivate,
                Name = "HasRightDeactivateUser",
                Checked = (userGroup == null) ? false : userGroup.Rights.Any(x => x.Permission == PermissionEnum.DeactivateUsers)
            });
            listControls.Add(new ControlModelCheckbox()
            {
                Col = 12,
                Label = Properties.Content.permission_adminbody,
                Name = "HasRightAdminBodies",
                Checked = (userGroup == null) ? false : userGroup.Rights.Any(x => x.Permission == PermissionEnum.AccessAdminBodies)
            });
            listControls.Add(new ControlModelCheckbox()
            {
                Col = 12,
                Label = Properties.Content.permission_info_oboject,
                Name = "HasRightListInfoObjects",
                Checked = (userGroup == null) ? false : userGroup.Rights.Any(x => x.Permission == PermissionEnum.AccessInfoObjects)
            });
            listControls.Add(new ControlModelCheckbox()
            {
                Col = 12,
                Label = Properties.Content.permission_manage_info_object,
                Name = "HasRightManageInfoObjects",
                Checked = (userGroup == null) ? false : userGroup.Rights.Any(x => x.Permission == PermissionEnum.ManageInfoObjects)
            });
            listControls.Add(new ControlModelCheckbox()
            {
                Col = 12,
                Label = Properties.Content.permission_journal,
                Name = "HasRightJournalLog",
                Checked = (userGroup == null) ? false : userGroup.Rights.Any(x => x.Permission == PermissionEnum.AccessJournalLog)
            });
            listControls.Add(new ControlModelCheckbox()
            {
                Col = 12,
                Label = Properties.Content.permission_importdata,
                Name = "HasRightImportData",
                Checked = (userGroup == null) ? false : userGroup.Rights.Any(x => x.Permission == PermissionEnum.AccessImportData)
            });
        }

        public UserGroupViewModel ParseUserGroupFromJson(string jsonData)
        {
            var mdl = new UserGroupViewModel();
            mdl.Permissions = new List<PermissionEnum>();

            dynamic userGroupData = JsonConvert.DeserializeObject(jsonData);

            var idElem = userGroupData[0];
            if (idElem.name != UserGroupViewModel.JSON_NAME_ID) throw new Exception("Invalid JSON UserGroup data : missing Id as first element!");

            if (idElem.value != string.Empty)
            {
                mdl.Id = (int)idElem.value;
            }

            foreach (var elem in userGroupData)
            {
                if (elem.name == UserGroupViewModel.JSON_NAME_NAME)
                {
                    mdl.Name = elem.value;
                }
                if (elem.name == UserGroupViewModel.JSON_NAME_ISACTIVE)
                {
                    mdl.IsActive = (elem.selectedValue.value == 1);
                    mdl.ActiveValue = elem.selectedValue.value;
                }
                if (elem.name == UserGroupViewModel.JSON_NAME_ROLE)
                {
                    int selValue = elem.selectedValue.value;

                    mdl.Role = (UserGroupRole)selValue;
                }
                SetPermissions(mdl, elem);
            }


            return mdl;
        }

        public void SetPermissions(UserGroupViewModel userGroupModel, dynamic elem)
        {
            if (elem.name == "HasRightManageUsers")
            {
                if (elem.value == "true")
                {
                    userGroupModel.Permissions.Add(PermissionEnum.ManageUsers);
                }
            }
            if (elem.name == "HasRightDeactivateUser")
            {
                if (elem.value == "true")
                {
                    userGroupModel.Permissions.Add(PermissionEnum.DeactivateUsers);
                }
            }
            if (elem.name == "HasRightAdminBodies")
            {
                if (elem.value == "true")
                {
                    userGroupModel.Permissions.Add(PermissionEnum.AccessAdminBodies);
                }
            }
            if (elem.name == "HasRightAdminBodyAdditionalInfo")
            {
                if (elem.value == "true")
                {
                    userGroupModel.Permissions.Add(PermissionEnum.AdminBodiesInfo);
                }
            }
            if (elem.name == "HasRightListInfoObjects")
            {
                if (elem.value == "true")
                {
                    userGroupModel.Permissions.Add(PermissionEnum.AccessInfoObjects);
                }
            }
            if (elem.name == "HasRightManageInfoObjects")
            {
                if (elem.value == "true")
                {
                    userGroupModel.Permissions.Add(PermissionEnum.ManageInfoObjects);
                }
            }
            if (elem.name == "HasRightJournalLog")
            {
                if (elem.value == "true")
                {
                    userGroupModel.Permissions.Add(PermissionEnum.AccessJournalLog);
                }
            }
            if (elem.name == "HasRightImportData")
            {
                if (elem.value == "true")
                {
                    userGroupModel.Permissions.Add(PermissionEnum.AccessImportData);
                }
            }
        }

    }
}
