using Newtonsoft.Json;
using SmartRegistry.CommonWeb.JsonHelpers;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartRegistry.Web.Controllers
{
    public class AreaController : BaseController
    {
        // GET: Area
        public ActionResult Index()
        {

            ViewBag.Title = "Теми ";
            ViewBag.ListAction = "GetAreaList";
                       
            return View();
        }


        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.GetControlsUrl = Url.Action("AreaData", "Area");
            ViewBag.SaveControlsUrl = Url.Action("SaveAreaData", "Area");
            ViewBag.PageName = "Създаване на тема ";
            return View("Edit");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.GetControlsUrl = Url.Action("AreaData", "Area", new { areaId = id });
            ViewBag.SaveControlsUrl = Url.Action("SaveAreaData", "Area");
            ViewBag.PageName = "Редакция на тема";

            return View("Edit");
        }

        public ActionResult GetFilterControls()
        {
            var jsonHelper = new AreaGroupListJsonHelper(SmartContext);
            var areasFilter = jsonHelper.GetFilters();

            var jsonData = JsonConvert.SerializeObject(areasFilter);

            return Content(jsonData, "application/json");
        }

        public ActionResult GetAreaList()
        {
            string jsonFilters = GetRequestData();
            try
            {
                var jsonHelper = new AreaGroupListJsonHelper(SmartContext);
                var logFilter = jsonHelper.DeserializeFilters(jsonFilters);

                var areaGrpService = SmartContext.AreaGroupsService;
                var list = areaGrpService.GetAll(logFilter);
                
                var areaListTable = jsonHelper.GetAreaTable(GetApplicationPath(), list, logFilter);

                var jsonData = JsonConvert.SerializeObject(areaListTable);


                return Content(jsonData, "application/json");
            }
            catch (Exception exx)
            {
                return Content(exx.Message, "application/json");
            }
        }

        public ActionResult AreaData(int? areaId)
        {

            var areaGroupService = SmartContext.AreaGroupsService;
            AreaGroup areaGrp = null;
            if (areaId != null)
            {
                areaGrp = areaGroupService.GetAreaGroup((int)areaId);
            }
            var jsonHelper = new AreaGroupListJsonHelper(SmartContext);

            var areaData = jsonHelper.GetAreaControlls(areaGrp);
            var jsonData = JsonConvert.SerializeObject(areaData);

            return Content(jsonData, "application/json");
        }

        [HttpPost]
        public ActionResult SaveAreaData()
        {
            string requestData = GetRequestData();
            try
            {
                var jsonHelper = new AreaGroupListJsonHelper(SmartContext);
                var areaMdl = jsonHelper.ParseAreaFromJson(requestData);

                var areaGrpService = SmartContext.AreaGroupsService;
                areaGrpService.SaveAreaModel(areaMdl);
            }
            catch (Exception ex)
            {
                SmartContext.DbContext.RollbackTransaction();
                return Json(new { status = "Error", errorMessage = ex.Message, redirectUrl = string.Empty }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { status = "Success", errorMessage = "", redirectUrl = this.Url.Action("Index", "Area") }, JsonRequestBehavior.AllowGet);
        }
    }
}