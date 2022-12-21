using Newtonsoft.Json;
using SmartRegistry.CommonWeb.JsonHelpers;
using SmartRegistry.Domain.Entities;
using SmartRegistry.Domain.Services;
using SmartRegistry.Domain.ViewModels;
using SmartRegistry.Web.AuthHelp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SmartRegistry.Web.Controllers
{
    public class UserGroupsController : BaseController
    {
        [CustomAuthorize(PermissionEnum.ManageUsers)]
        public ActionResult Index()
        {
            return View();
        }
        [CustomAuthorize(PermissionEnum.ManageUsers)]
        public ActionResult Create()
        {
            ViewBag.GetControlsUrl = Url.Action("UserGroupData", "UserGroups");
            // ViewBag.PageName = "Създаване на потребителска група";
            ViewBag.PageName = "usergroups_create_header";
            return View("Edit");
        }

        [CustomAuthorize(PermissionEnum.ManageUsers)]
        public ActionResult Edit(int id)
        {
            ViewBag.GetControlsUrl = Url.Action("UserGroupData", "UserGroups", new { userGroupId = id });
            // ViewBag.PageName = "Редакция на потребителска група";
            ViewBag.PageName = "usergroups_edit_header";
            return View("Edit");
        }

        [HttpPost]
        [CustomAuthorize(PermissionEnum.ManageUsers)]
        public ActionResult SaveUserGroupData()
        {
            string requestData = GetRequestData();
            try
            {
                var jsonHelper = new UserGroupJsonHelper(SmartContext);
                var userGroupMdl = jsonHelper.ParseUserGroupFromJson(requestData);

                var usrGroupService = SmartContext.UserGroupsService;
                var validationRes = usrGroupService.ValidateUserGroupModel(userGroupMdl);
                if ((validationRes != null) && (validationRes.HasErrors))
                {
                    return Json(
                        new
                        {
                            status = "Error",
                            errorMessage = validationRes.ErrorMessage,
                            redirectUrl = string.Empty,
                            controlErrors = validationRes.ControlErrors
                        }, JsonRequestBehavior.AllowGet);
                }
                usrGroupService.SaveUserGroupModel(userGroupMdl);
            }
            catch (Exception ex)
            {
                SmartContext.DbContext.RollbackTransaction();
                return Json(new { status = "Error", errorMessage = ex.Message, redirectUrl = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Success", errorMessage = "", redirectUrl = this.Url.Action("Index", "UserGroups") }, JsonRequestBehavior.AllowGet);
        }

        
        [CustomAuthorize(PermissionEnum.ManageUsers)]
        public ActionResult UserGroupData(int? userGroupId)
        {
            var userGroupService = SmartContext.UserGroupsService;
            UserGroup userGroup = null;
            if (userGroupId != null)
            {
                userGroup = userGroupService.GetUserGroupById((int)userGroupId);
            }

            var jsonHelper = new UserGroupJsonHelper(SmartContext);

            var userData = jsonHelper.GetEditControls(userGroup);
            var jsonData = JsonConvert.SerializeObject(userData);

            return Content(jsonData, "application/json");
        }


        [HttpGet]
        [CustomAuthorize(PermissionEnum.ManageUsers)]
        public ActionResult GetFilterControls()
        {
            var jsonHelper = new UserGroupListJsonHelper(SmartContext);

            var userGroupFilters = jsonHelper.GetFilterControls();
            var jsonData = JsonConvert.SerializeObject(userGroupFilters);

            return Content(jsonData, "application/json");
        }


        [CustomAuthorize(PermissionEnum.ManageUsers)]
        public ActionResult GetUserGroupsData()
        {
            string jsonFilters = GetRequestData();
            try
            {
                var jsonHelper = new UserGroupListJsonHelper(SmartContext);
                var userGroupFilter = jsonHelper.DeserializeFilters(jsonFilters);

                var userGroupsService = SmartContext.UserGroupsService;
                var userGroupResult = userGroupsService.GetUserGroups(userGroupFilter);

                var userGroupsTable = jsonHelper.GetUserGroupsTable(GetApplicationPath(), userGroupResult);

                var jsonData = JsonConvert.SerializeObject(userGroupsTable);
                return Content(jsonData, "application/json");
            }
            catch (Exception exx)
            {
                return Content(exx.Message, "application/json");
            }
        }
    }
}