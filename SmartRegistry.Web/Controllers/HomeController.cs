using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using SmartRegistry.Domain.Common;
using SmartRegistry.Domain.QueryFilters;
using SmartRegistry.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SmartRegistry.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
       
        public ActionResult Index()
        {
            setCounters();


            return View();
        }

        private void setCounters()
        {
            var orgnCount = DbContext.GetAdministrativeBodiesDao().GetAdministrativeBodies().Count();
            var regFilter = new RegisterFilter()
            {
                AdminBody = SmartContext.CurrentAdminBody
            };
            var regCount = DbContext.GetRegistersDao().GetCount(regFilter);
            var unifCount = DbContext.GetUnifiedDataDao().GetCount();
            ViewBag.Organizations = orgnCount;
            ViewBag.Registers = regCount;
            ViewBag.UnifiedCount = unifCount;
        }

        public ActionResult About()
        {
            var loggedIn = User.Identity.IsAuthenticated;
            ViewBag.Message = $"User authenticated: {loggedIn}. User ID: {User.Identity.GetUserId()}";
           
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            HttpContext.GetOwinContext().Authentication.SignOut();
           //return RedirectToAction("Index", "Home");


            return View();
        }
    }
}