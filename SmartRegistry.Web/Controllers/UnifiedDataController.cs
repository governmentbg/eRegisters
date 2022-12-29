using log4net;
using Newtonsoft.Json;
using SmartRegistry.CommonWeb.JsonHelpers;
using SmartRegistry.Domain.Entities;
using SmartRegistry.Domain.Services;
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
    [Authorize]
    public class UnifiedDataController : BaseController
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(UnifiedDataController));

        // GET: UniData
        [CustomAuthorize(PermissionEnum.AccessInfoObjects)]
        public ActionResult Index()
        {
            ViewBag.UserAccess = SmartContext.CurrentUser;
            _logger.Debug("Index");
            return View();
        }

        [HttpGet]
        [CustomAuthorize(PermissionEnum.AccessInfoObjects)]
        public ActionResult GetFilterControls()
        {
            var jsonHelper = new UnifiedDataListJsonHelper(SmartContext);

            var userGroupFilters = jsonHelper.GetFilterControls();
            var jsonData = JsonConvert.SerializeObject(userGroupFilters);

            return Content(jsonData, "application/json");
        }

        [CustomAuthorize(PermissionEnum.AccessInfoObjects)]
        public ActionResult GetUniDataList()
        {
            string jsonFilters = GetRequestData();
            try
            {
                var jsonHelper = new UnifiedDataListJsonHelper(SmartContext);
                var uniDataFilter = jsonHelper.DeserializeFilters(jsonFilters);

                var uniDataService = SmartContext.UnifiedDataService;
                var uniDataResult = uniDataService.GetUnifiedDataList(uniDataFilter);

                var uniDataTable = jsonHelper.GetUniDataTable(GetApplicationPath(), uniDataResult, uniDataFilter);

                var jsonData = JsonConvert.SerializeObject(uniDataTable);
                return Content(jsonData, "application/json");
            }
            catch (Exception exx)
            {
                return Content(exx.Message, "application/json");
            }
        }

        [HttpGet]
        [CustomAuthorize(PermissionEnum.ManageInfoObjects)]
        public ActionResult Create()
        {
            ViewBag.GetControlsUrl = Url.Action("UniDataControls", "UnifiedData");
           // ViewBag.PageName = "Създаване на информационен обект";
            ViewBag.PageName = "unifieddata_create_unified_header";
            return View("Edit");
        }

        public ActionResult CreateReferential()
        {
            ViewBag.GetControlsUrl = Url.Action("UniDataReferentialControls", "UnifiedData");
            //  ViewBag.PageName = "Създаване на съставен обект";
            ViewBag.PageName = "unifieddata_create_referential_header";
            return View("EditReferential");
        }

        public ActionResult CreateComposite()
        {
            ViewBag.GetControlsUrl = Url.Action("UniDataCompositeControls", "UnifiedData");
          //  ViewBag.PageName = "Създаване на съставен обект";
            ViewBag.PageName = "unifieddata_create_composite_header";
            return View("EditComposite");
        }

        [HttpGet]
        [CustomAuthorize(PermissionEnum.ManageInfoObjects)]
        public ActionResult EditReferential(int id)
        {
            ViewBag.GetControlsUrl = Url.Action("UniDataReferentialControls", "UnifiedData", new { uniDataId = id });
           // ViewBag.PageName = "Редакция на референтен обект";
            ViewBag.PageName = "unifieddata_edit_referential_header";

            return View("EditReferential");
        }

        [HttpGet]
        [CustomAuthorize(PermissionEnum.ManageInfoObjects)]
        public ActionResult EditComposite(int id)
        {
            ViewBag.GetControlsUrl = Url.Action("UniDataCompositeControls", "UnifiedData", new { uniDataId = id });
           // ViewBag.PageName = "Редакция на съставен обект";
            ViewBag.PageName = "unifieddata_edit_composite_header";

            return View("EditComposite");
        }

        [HttpGet]
        [CustomAuthorize(PermissionEnum.ManageInfoObjects)]
        public ActionResult Edit(int id)
        {
            ViewBag.GetControlsUrl = Url.Action("UniDataControls", "UnifiedData", new { uniDataId = id });
            // ViewBag.PageName = "Редакция на информационен обект";
            ViewBag.PageName = "unifieddata_edit_unified_header";

            return View("Edit");
        }

        [CustomAuthorize(PermissionEnum.AccessInfoObjects)]
        public ActionResult UniDataControls(int? uniDataId)
        {
            var uniDataService = SmartContext.UnifiedDataService;
            UnifiedData uniData = null;
            if (uniDataId != null)
            {
                uniData = uniDataService.GetUnifiedDataById((int)uniDataId);
            }

            var jsonHelper = new UnifiedDataJsonHelper(SmartContext);

            var uniDataControls = jsonHelper.GetEditControls(uniData);
            var jsonData = JsonConvert.SerializeObject(uniDataControls);

            return Content(jsonData, "application/json");
        }


        [CustomAuthorize(PermissionEnum.AccessInfoObjects)]
        public ActionResult UniDataReferentialControls(int? uniDataId)
        {
            var uniDataService = SmartContext.UnifiedDataService;
            UnifiedData uniData = null;
            if (uniDataId != null)
            {
                uniData = uniDataService.GetUnifiedDataById((int)uniDataId);
            }

            var jsonHelper = new UnifiedDataJsonHelper(SmartContext);

            var uniDataControls = jsonHelper.GetEditReferentialControls(GetFullApplicationPath(),uniData);
            var jsonData = JsonConvert.SerializeObject(uniDataControls);

            return Content(jsonData, "application/json");
        }

        [CustomAuthorize(PermissionEnum.AccessInfoObjects)]
        public ActionResult UniDataCompositeControls(int? uniDataId)
        {
            var uniDataService = SmartContext.UnifiedDataService;
            UnifiedData uniData = null;
            if (uniDataId != null)
            {
                uniData = uniDataService.GetUnifiedDataById((int)uniDataId);
            }

            var jsonHelper = new UnifiedDataJsonHelper(SmartContext);

            var uniDataControls = jsonHelper.GetEditCompositeControls(uniData);
            var jsonData = JsonConvert.SerializeObject(uniDataControls);

            return Content(jsonData, "application/json");
        }
        
        [HttpPost]
        [CustomAuthorize(PermissionEnum.ManageInfoObjects)]
        public ActionResult SaveUnifiedData()
        {
            string requestData = GetRequestData();
            try
            {
                var jsonHelper = new UnifiedDataJsonHelper(SmartContext);
                var uniDataMdl = jsonHelper.ParseUnifiedDataFromJson(requestData);

                var uniDataService = SmartContext.UnifiedDataService;
                var validationRes = uniDataService.ValidateUniDataModel(uniDataMdl);
                if ((validationRes != null) && (validationRes.HasErrors))
                {
                    return Json(
                        new { 
                            status = "Error", 
                            errorMessage = validationRes.ErrorMessage, 
                            redirectUrl = string.Empty,
                            controlErrors = validationRes.ControlErrors
                        }, JsonRequestBehavior.AllowGet);
                }
                uniDataService.SaveUniDataModel(uniDataMdl);
            }
            catch (Exception ex)
            {
                SmartContext.DbContext.RollbackTransaction();
                return Json(new { status = "Error", errorMessage = ex.Message, redirectUrl = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Success", errorMessage = "", redirectUrl = this.Url.Action("Index", "UnifiedData") }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomAuthorize(PermissionEnum.ManageInfoObjects)]
        public ActionResult SaveCompositeUnifiedData()
        {
            string requestData = GetRequestData();
            try
            {
                var jsonHelper = new UnifiedDataJsonHelper(SmartContext);
                var uniDataMdl = jsonHelper.ParseUnifiedDataFromJson(requestData);
                uniDataMdl.DataType = UnifiedDataTypeEnum.Composite;
                uniDataMdl.HasMultipleValues = false;

                var uniDataService = SmartContext.UnifiedDataService;

                var validationRes = uniDataService.ValidateUniDataModel(uniDataMdl);
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

                uniDataService.SaveUniDataModel(uniDataMdl);
            }
            catch (Exception ex)
            {
                SmartContext.DbContext.RollbackTransaction();
                return Json(new { status = "Error", errorMessage = ex.Message, redirectUrl = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Success", errorMessage = "", redirectUrl = this.Url.Action("Index", "UnifiedData") }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomAuthorize(PermissionEnum.ManageInfoObjects)]
        public ActionResult SaveReferentialUnifiedData()
        {
            string requestData = GetRequestData();
            try
            {
                var jsonHelper = new UnifiedDataJsonHelper(SmartContext);
                var uniDataMdl = jsonHelper.ParseUnifiedDataFromJson(requestData);
                uniDataMdl.DataType = UnifiedDataTypeEnum.Referential;
                uniDataMdl.HasMultipleValues = false;

                var uniDataService = SmartContext.UnifiedDataService;

                var validationRes = uniDataService.ValidateUniDataModel(uniDataMdl);
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

                uniDataService.SaveUniDataModel(uniDataMdl);
            }
            catch (Exception ex)
            {
                SmartContext.DbContext.RollbackTransaction();
                return Json(new { status = "Error", errorMessage = ex.Message, redirectUrl = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = "Success", errorMessage = "", redirectUrl = this.Url.Action("Index", "UnifiedData") }, JsonRequestBehavior.AllowGet);
        }

    }
}