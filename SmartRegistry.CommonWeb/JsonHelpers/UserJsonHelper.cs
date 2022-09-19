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
    public class UserJsonHelper : BaseJsonHelper
    {
        public UserJsonHelper(ISmartRegistryContext context)
            : base(context)
        {

        }

        public IList<ControlModelBase> GetEditControls(User user, IList<UserGroup> userGroups)
        {
            var result = new List<ControlModelBase>();

            result.Add(new ControlModelHidden()
            {
                Col = 12,
                Label = "",
                Name = UserViewModel.JSON_NAME_ID,
                Value = (user == null) ? string.Empty : user.Id.ToString()
            });

            result.Add(new ControlModelText()
            {
                Col = 12,
                Label = "Име",
                Required = true,
                Name = UserViewModel.JSON_NAME_NAME,             
                Value = (user == null) ? string.Empty : user.Name
            });

            string userIdent = string.Empty;
            if ((user != null) && (user.Identificators != null))
            {
                var ident = user.Identificators.Where(x => x.IdentificatorType.IdentificatorType == "PNO")
                    .FirstOrDefault();
                if (ident != null)
                {
                    userIdent = ident.Identificator;
                }
            }

            result.Add(new ControlModelText()
            {
                Col = 12,
                Label = "Телефон",
                Name = UserViewModel.JSON_NAME_PHONE,
                Value = (user == null) ? string.Empty : user.Phone
            });

            result.Add(new ControlModelText()
            {
                Col = 12,
                Label = "Имейл",
                Required = true,
                Name = UserViewModel.JSON_NAME_EMAIL,
                Value = (user == null) ? string.Empty : user.Email
            });

            bool selectedIsActive = true;
            if (user != null) selectedIsActive = user.IsActive;
            AddStatusControl(result, selectedIsActive, UserViewModel.JSON_NAME_ISACTIVE);

            AddMultySelectGroups(result, user, userGroups);

            return result;
        }

        protected void AddMultySelectGroups(IList<ControlModelBase> listControls, User user, IList<UserGroup> userGroups)
        {
            var bodyControl = new ControlModelOptionMultiList()
            {
                Col = 12,
                Label = "Потребителски групи",
                Required = true,
                Name = UserViewModel.JSON_NAME_USERGROUPS
            };


            var t = new ControlModelOptionGroup();
            bodyControl.SelectedValues = new List<ControlModelOptionElement>();

            foreach (UserGroup ab in userGroups)
            {

                var selectedBody = new ControlModelOptionElement()
                {
                    Label = ab.Name,
                    Value = ab.Id.ToString()
                };

                t.Options.Add(selectedBody);

                if ((user != null) && user.UserGroups != null)
                {
                    if (user.UserGroups.Contains(ab))
                    {
                        bodyControl.SelectedValues.Add(selectedBody);
                    }
                }
            }
            var tt = new ControlModelOptionGroupWrapper(t);
            bodyControl.AddOptionGroup(t);
            listControls.Add(bodyControl);
        }

        public UserViewModel ParseUserFromJson(string requestData)
        {
            var result = new UserViewModel();

            dynamic userData = JsonConvert.DeserializeObject(requestData);

            var idElem = userData[0];
            if (idElem.name != UserViewModel.JSON_NAME_ID) throw new Exception("Invalid JSON User data : missing Id as first element!");

            if (idElem.value != string.Empty)
            {
                result.Id = (int)idElem.value;
            }

            foreach (var elem in userData)
            {
                if (elem.name == UserViewModel.JSON_NAME_NAME)
                {
                    result.Name = elem.value;
                }
                if (elem.name == UserViewModel.JSON_NAME_ISACTIVE)
                {
                    result.IsActive = (elem.selectedValue.value == 1);
                    result.ActiveValue = elem.selectedValue.value;
                }
                if (elem.name == UserViewModel.JSON_NAME_PHONE)
                {
                    result.Phone = elem.value;
                }
                if (elem.name == UserViewModel.JSON_NAME_EMAIL)
                {
                    result.Email = elem.value;
                }

                if (elem.name == UserViewModel.JSON_NAME_USERGROUPS)
                {
                    if (result.UserGroupIds == null)
                    {
                        result.UserGroupIds = new List<long>();
                    }

                    var selected = elem.selectedValues;
                    foreach (var selElem in selected)
                    {
                        var strngSelected = (string)selElem.value;
                        var selInt = Int64.Parse(strngSelected);
                        result.UserGroupIds.Add(selInt);
                    }
               } 
            }

            return result;
        }
    }
}
