using Newtonsoft.Json;
using SmartRegistry.CommonWeb.JsonHelpers;
using SmartRegistry.Domain.Entities;
using SmartRegistry.Domain.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SmartRegistry.Web.Controllers
{
    public class RegisterRecordsController : BaseController
    {
        // GET: RegisterRecords
        public ActionResult Index(int id)
        {
            ViewBag.RegisterId = id;
           

            if ((SmartContext.CurrentUser == null)
                || (!SmartContext.CurrentUser.HasRegisterRight(id, RegisterPermissionEnum.HasAccess)))
            {
               return new HttpStatusCodeResult(403);
            }

            var register = DbContext.GetRegistersDao().GetById(id);
            var regName = (register != null) ? register.Name : string.Empty;
            ViewBag.Title = "Вписване - " + regName;
           

            return View();
        }

        public ActionResult GetFilterControls(int id)
        {
            var regService = SmartContext.RegistersService;
            var filterAttributes = regService.GetAttributesForFilter(id);

            var jsonHelper = new RegisterRecordListJsonHelper(SmartContext);
            var recordFilters = jsonHelper.GetFilters(filterAttributes, id,RegisterListType.List);

            var jsonData = JsonConvert.SerializeObject(recordFilters);

            return Content(jsonData, "application/json");
        }

        public ActionResult GetRegisterRecords()
        {
            int? registerId = GetIntQueryParam("registerId");
            if (registerId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string jsonFilters = GetRequestData();
            try
            {
                var regRecordsService = SmartContext.RegisterRecordsService;
                var registerService = SmartContext.RegistersService;

                var jsonHelper = new RegisterRecordListJsonHelper(SmartContext);
                var regFilter = jsonHelper.DeserializeFilters(jsonFilters, (int)registerId);

                if (regFilter.Register == null) throw new Exception("Не е зададен регистър във филтъра");

                var attributesToList = registerService.GetAttributesToList((int)registerId);

                var registerRecordResult = regRecordsService.GetRegisterRecords(regFilter);
                var registerRecordsTable = jsonHelper.GetRegisterRecordsTable(GetApplicationPath(), registerRecordResult, (int)registerId, attributesToList,RegisterListType.List);

                var jsonData = JsonConvert.SerializeObject(registerRecordsTable);
                return Content(jsonData, "application/json");
            }
            catch (Exception exx)
            {
                return Content(exx.Message, "application/json");
            }
        }

        public ActionResult Create(int registerId)
        {
           // ViewBag.PageName = "Вписване на обстоятелства";
          
            var register = DbContext.GetRegistersDao().GetById(registerId);
            var regName = (register != null) ? register.Name : string.Empty;
            ViewBag.PageName = "Вписване на обстоятелства - " + regName;
            ViewBag.ParentId = registerId;
            ViewBag.Register = registerId;

            ViewBag.GetControlsUrl = Url.Action("GetRecordControls", "RegisterRecords", new { registerId = registerId });
            ViewBag.SaveControlsUrl = Url.Action("SaveRegisterRecord", "RegisterRecords");

            return View("Edit");
        }

        public ActionResult Edit(int id)
        {
          //  ViewBag.PageName = "Промяна на обстоятелства";

            var registerRec = SmartContext.RegisterRecordsService.GetRegisterRecord(id);
            var regName = (registerRec != null) ? registerRec.Register.Name : string.Empty;
            ViewBag.PageName = "Промяна на обстоятелства - " + regName;
            ViewBag.ParentId = id;
            ViewBag.Register = registerRec.Register.Id;

            ViewBag.GetControlsUrl = Url.Action("GetRecordControls", "RegisterRecords", new { recordId = id });
            ViewBag.SaveControlsUrl = Url.Action("SaveRegisterRecord", "RegisterRecords");

            return View();
        }

        public ActionResult GetRecordControls(int? registerId, long? recordId)
        {
            var regRecService = SmartContext.RegisterRecordsService;

            int regId = 0;
            RegisterRecord regRecord = null;
            if (recordId != null)
            {
                regRecord = regRecService.GetRegisterRecord((long)recordId);
                if (regRecord == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NoContent);
                }
                regId = regRecord.Register.Id;
            }
            else
            {
                if (registerId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                regId = (int)registerId;
            }

            var regService = SmartContext.RegistersService;
            var allAttributes = regService.GetEditableAttributes(regId);

            var jsonHelper = new RegisterRecordJsonHelper(SmartContext);
            var recControls = jsonHelper.GetRecordEditControls(GetFullApplicationPath(),allAttributes, regRecord, regId);
            var jsonData = JsonConvert.SerializeObject(recControls);

            return Content(jsonData, "application/json");
        }

        [HttpGet]
        public ActionResult GetDataForRefAttribute(int id, string filterValue, string search)
        {
            var regRecService = SmartContext.RegisterRecordsService;
            var refDataList = regRecService.GetRefAttributeData(id, filterValue, search);

            var jsonHelper = new RegisterRecordJsonHelper(SmartContext);
            var resultList = jsonHelper.GetRefAttributeAjaxList(refDataList);

            var jsonData = JsonConvert.SerializeObject(resultList);


            return Content(jsonData, "application/json");
        }


        [HttpPost]
        public ActionResult SaveRegisterRecord()
        {
            int registerId = 0;
            string requestData = GetRequestData();
            try
            {
                var jsonHelper = new RegisterRecordJsonHelper(SmartContext);
                var attrValuesMdl = jsonHelper.ParseAttributeValuesFromJson(requestData);

                var regService = SmartContext.RegisterRecordsService;

                var validationRes = regService.ValidateRegisterRecordModel(attrValuesMdl);
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


                regService.SaveRegisterRecordModel(attrValuesMdl);
                registerId = (int)attrValuesMdl.RegisterId;
            }
            catch (Exception ex)
            {
                SmartContext.DbContext.RollbackTransaction();
                return Json(new { status = "Error", errorMessage = ex.Message, redirectUrl = string.Empty }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { status = "Success", errorMessage = "", redirectUrl = this.Url.Action("Index", "RegisterRecords", new { id = registerId } ) }, JsonRequestBehavior.AllowGet);
        }

    }
}