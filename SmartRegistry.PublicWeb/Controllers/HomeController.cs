using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartRegistry.PublicWeb.Controllers
{
    public class HomeController : BasePublicController
    {
        public ActionResult Index()
        {
            var areaList = DbContext.GetAreaGroupsDao().GetAll();

            setCounters();

            return View(areaList);
        }

        private void setCounters()
        {
          
            var orgnCount = DbContext.GetRegistersDao().GetDistinctAdminBodyCount();            
            var regCount = DbContext.GetRegistersDao().GetCount(null); // no filter - to get all registers
            ViewBag.Organizations = orgnCount;
            ViewBag.Registers = regCount;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Theme(int? id)
        {
            ViewBag.SelectedTheme = id;

            var areaList = DbContext.GetAreaGroupsDao().GetAll();

            return View(areaList);
        }
    }
}