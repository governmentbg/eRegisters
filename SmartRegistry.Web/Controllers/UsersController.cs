using Newtonsoft.Json;
using SmartRegistry.CommonWeb.JsonHelpers;
using SmartRegistry.Domain.Common;
using SmartRegistry.Domain.Entities;
using SmartRegistry.Domain.Interfaces;
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
    public class UsersController : BaseController
    {
        private IMessageService _messageService;

        [CustomAuthorize(PermissionEnum.ManageUsers)]
        public ActionResult Index()
        {

            string baseUrl1 = HttpContext.Request.Url.AbsolutePath;
            string baseUrl2 = HttpContext.Request.Url.AbsoluteUri;

            return View();
        }

        [CustomAuthorize(PermissionEnum.ManageUsers)]
        public ActionResult Create()
        {
            ViewBag.GetControlsUrl = Url.Action("UserData", "Users");
            ViewBag.PageName = "Създаване на потребител";

            return View("Edit");
        }



        [CustomAuthorize(PermissionEnum.ManageUsers)]
        public ActionResult Edit(int id)
        {
            ViewBag.GetControlsUrl = Url.Action("UserData", "Users", new { userId = id });
            ViewBag.PageName = "Редакция на потребител";

            return View("Edit");
        }

        [CustomAuthorize(PermissionEnum.ManageUsers)]
        public ActionResult Reset(int id)
        {
          //  ViewBag.GetControlsUrl = Url.Action("UserData", "Users", new { userId = id });
            ViewBag.PageName = "Генерирай линк за потвърждение на акаунта";
         
            var user = DbContext.GetUsersDao().GetById((int)id);

            return View("Reset", user);
        }

        [CustomAuthorize(PermissionEnum.ManageUsers)]
        public ActionResult ResetAndSendMail(User model)
        {
            //  ViewBag.GetControlsUrl = Url.Action("UserData", "Users", new { userId = id });
            ViewBag.PageName = "Генерирай линк за потвърждение на акаунта";
          //  var id = Int32.Parse(Request.QueryString[0].ToString());
            try
            {
                var usrDao = SmartContext.DbContext.GetUsersDao();
                var selectedUser = usrDao.GetById(model.Id);

                selectedUser.Email = model.Email;
                selectedUser.Name = model.Name;
             
                string shortGuid = ToShortString(Guid.NewGuid());
                selectedUser.ResetCode = shortGuid;
                usrDao.Update(selectedUser);


                if (!string.IsNullOrEmpty(selectedUser.ResetCode))
                {
                    _messageService = new MessageService();
                    _messageService.SendMessage(ResetAccMessage.GetSubject(), ResetAccMessage.GetBody(selectedUser.ResetCode), selectedUser.Email);

                }


            }
            catch (Exception ee) {


            }

        
            return RedirectToAction("Index");
        }


        [CustomAuthorize(PermissionEnum.ManageUsers)]
        public ActionResult UserData(int? userId)
        {
            var userService = SmartContext.UsersService;
            User user = null;
            if (userId != null)
            {
                user = userService.GetUserById((int)userId);
            }
            var userGroups = userService.GetAllowedUserGroupsForUserEdit();

            var jsonHelper = new UserJsonHelper(SmartContext);

            var userData = jsonHelper.GetEditControls(user, userGroups);
            var jsonData = JsonConvert.SerializeObject(userData);

            return Content(jsonData, "application/json");
        }

        [HttpPost]
        [CustomAuthorize(PermissionEnum.ManageUsers)]
        public ActionResult SaveUserData()
        {
            string requestData = GetRequestData();
            try
            {
                var jsonHelper = new UserJsonHelper(SmartContext);
                var userMdl = jsonHelper.ParseUserFromJson(requestData);

                var usrService = SmartContext.UsersService;
                var validationRes = usrService.ValidateUserModel(userMdl);
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
                usrService.SaveUserModel(userMdl);
            }
            catch (Exception ex)
            {
                SmartContext.DbContext.RollbackTransaction();
                return Json(new { status = "Error", errorMessage = ex.Message, redirectUrl = string.Empty }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { status = "Success", errorMessage = "", redirectUrl = this.Url.Action("Index", "Users") }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [CustomAuthorize(PermissionEnum.ManageUsers)]
        public ActionResult GetFilterControls()
        {
            var jsonHelper = new UserListJsonHelper(SmartContext);

            var usersFilters = jsonHelper.GetUsersFilterControls();
            var jsonData = JsonConvert.SerializeObject(usersFilters);

            return Content(jsonData, "application/json");
        }




        [CustomAuthorize(PermissionEnum.ManageUsers)]
        public ActionResult GetUsersList()
        {
            string jsonFilters = GetRequestData();
            try
            {
                var jsonHelper = new UserListJsonHelper(SmartContext);
                var usersFilter = jsonHelper.DeserializeFilters(jsonFilters);

          

                var usersService = SmartContext.UsersService;
                var usersResult = usersService.GetUsers(usersFilter);
                var usersTable = jsonHelper.GetUsersTable(GetApplicationPath(), usersResult);

                var jsonData = JsonConvert.SerializeObject(usersTable);
                return Content(jsonData, "application/json");
            }
            catch (Exception exx) {
                return Content(exx.Message, "application/json");
            }
        }

        public string ToShortString(Guid guid)
        {
            var base64Guid = Convert.ToBase64String(guid.ToByteArray());

            // Replace URL unfriendly characters
            base64Guid = base64Guid.Replace('+', '-').Replace('/', '_');

            // Remove the trailing ==
            return base64Guid.Substring(0, base64Guid.Length - 2);
        }

        public Guid FromShortString(string str)
        {
            str = str.Replace('_', '/').Replace('-', '+');
            var byteArray = Convert.FromBase64String(str + "==");
            return new Guid(byteArray);
        }
    }
}