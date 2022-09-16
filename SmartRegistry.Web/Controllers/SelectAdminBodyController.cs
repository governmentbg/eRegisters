using SmartRegistry.Domain.Common;
using SmartRegistry.Web.Models.SelectAdminBody;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartRegistry.Web.Controllers
{
    [AllowAnonymous]
    public class SelectAdminBodyController : BaseController
    {
        public ActionResult Index()
        {
            var userAdminBodies = new List<UserAdminBody>();

            if (SmartContext.CurrentUser == null) return View(userAdminBodies);
            if (SmartContext.CurrentUser.UserAdministrativeBodies == null) return View(userAdminBodies);

            if (SmartContext.CurrentUser.IsGlobalAdmin)
            {
                var admBodyDao = SmartContext.DbContext.GetAdministrativeBodiesDao();
                var admBodies = admBodyDao.GetAdministrativeBodies();

                foreach(var admBody in admBodies)
                {
                    var isCurrent = (SmartContext.CurrentAdminBody != null) && (admBody.Id == SmartContext.CurrentAdminBody.Id);
                    userAdminBodies.Add(new UserAdminBody()
                    {
                        UserId = admBody.Id,
                        AdminBodyName = admBody.Name,
                        IsCurrent = isCurrent
                    });
                }
            }
            else
            {
                foreach (var userAdmBody in SmartContext.CurrentUser.UserAdministrativeBodies)
                {
                    if (userAdmBody.AdministrativeBody.IsActive)
                    {
                        var isCurrent = (SmartContext.CurrentAdminBody != null) && (userAdmBody.AdministrativeBody.Id == SmartContext.CurrentAdminBody.Id);
                        userAdminBodies.Add(new UserAdminBody()
                        {
                            UserId = userAdmBody.AdministrativeBody.Id,
                            AdminBodyName = userAdmBody.AdministrativeBody.Name,
                            IsCurrent = isCurrent
                        });
                    }
                }
            }

            return View(userAdminBodies);
        }

        [HttpGet]
        public ActionResult Select(int id)
        {
            //setting up selected adminbody in cookie
            string value = string.Empty;
            var cookie = HttpContext.Request.Cookies[SmartRegistryConstants.ADMIN_BODY_USER_COOKIE];
            if (cookie != null)
            {
                cookie.Value = id.ToString();
                cookie.Expires = DateTime.Now.AddDays(1);
                HttpContext.Response.Cookies.Add(cookie);
            }
            else
            {
                var newcookie = new HttpCookie(SmartRegistryConstants.ADMIN_BODY_USER_COOKIE, id.ToString());
                newcookie.Expires = DateTime.Now.AddDays(1);
                HttpContext.Response.Cookies.Add(newcookie);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}