using SmartRegistry.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using SmartRegistry;
using SmartRegistry.Web.Models;
using SmartRegistry.Domain.Common;
using SmartRegistry.Domain.Entities;
using System.Threading;
using SmartRegistry.Web.AuthHelp;

namespace SmartRegistry.Web.Controllers
{
    public class AdministrativeBodiesController : BaseController
    {
        // GET: AdministrativeBodies

        [CustomAuthorize(PermissionEnum.AccessAdminBodies)]
        public ActionResult Index(int? page,string name,string types,string status)
        {

           
            string strTypeValue = String.Empty;
            string strStatusValue = String.Empty;

            if (Request.Form["Types"] != null)
            {
                types = Request.Form["Types"].ToString();
            }
            if (Request.Form["Status"] != null)
            {
                status = Request.Form["Status"].ToString();
            }

            var service = SmartContext.AdministrativeBodyService;
            var result = service.GetAllAdministrtiveBodiesFromDB();

            if (!String.IsNullOrEmpty(name))
            {
                result = result.Where(x => x.Name.Contains(name)).ToList();
            }

            if (!String.IsNullOrEmpty(types))
            {
                result = result.Where(x => types.Contains(((int)x.Kind).ToString())).ToList();
            }

            if (!String.IsNullOrEmpty(status) && status!="All")
            {
                result = result.Where(x => status.Contains(x.IsActive.ToString())).ToList();
            }


            ViewBag.SearchName = name;
            ViewBag.Types = types;
            ViewBag.Status = status;


            IEnumerable<EnumValues> clientValues =
            from AdministrativeBodyKind c in Enum.GetValues(typeof(AdministrativeBodyKind))
            select new EnumValues
            {
            ID = (int)c,
            Name = c.ToString()
            };
            ViewBag.ClientValues = clientValues;

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(result.ToPagedList(pageNumber, pageSize));
        }

        [CustomAuthorize(PermissionEnum.AccessAdminBodies)]
        public ActionResult ShowInfo(string Id)
        {

            var service = SmartContext.AdministrativeBodyService;
            var result = service.GetExtendedInfo(long.Parse(Id));

            return View(result);
        }

        [CustomAuthorize(PermissionEnum.AccessAdminBodies)]
        public ActionResult Sync()
        {

            var service = SmartContext.AdministrativeBodyService;
            service.SyncAdminRegisterDB();

            return View();
        }

    }
}