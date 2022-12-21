using Newtonsoft.Json;
using SmartRegistry.CommonWeb.JsonHelpers;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SmartRegistry.Web.Controllers
{
    public class RegisterTransitionController : BaseController
    {
        // GET: RegisterTransition
        public ActionResult Index(int id)
        {
            ViewBag.RegisterId = id;

            var register = DbContext.GetRegistersDao().GetById(id);
            var regName = (register != null) ? register.Name : string.Empty;
            ViewBag.Title =  regName;

            return View();
        }


        [HttpGet]
        public ActionResult Create()
        {
            var regId = GetIntQueryParam("registerId");
            ViewBag.RegisterId = regId;
            ViewBag.GetControlsUrl = Url.Action("TransitionData", "RegisterTransition", new { registerId = regId });
            ViewBag.SaveControlsUrl = Url.Action("SaveTransitionData", "RegisterTransition");
           // ViewBag.PageName = "Създаване на бизнес процес ";
            ViewBag.PageName = Resources.Content.registertransition_create_header;

            return View("Edit");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var registerTrans = DbContext.GetRegisterTransitionDao().GetById(id);
            ViewBag.RegisterId = registerTrans.Register.Id;
            ViewBag.GetControlsUrl = Url.Action("TransitionData", "RegisterTransition", new { tranId = id, registerId = registerTrans.Register.Id });
            ViewBag.SaveControlsUrl = Url.Action("SaveTransitionData", "RegisterTransition");
          //  ViewBag.PageName = "Промяна на бизнес процес ";
            ViewBag.PageName = Resources.Content.registertransition_edit_header;


            return View("Edit");
        }

        public ActionResult GetFilterControls(int id)
        {
            var jsonHelper = new RegisterTransitionJsonHelper(SmartContext);
            var statesFilter = jsonHelper.GetFilters();

            var jsonData = JsonConvert.SerializeObject(statesFilter);

            return Content(jsonData, "application/json");
        }

        public ActionResult GetRegisterTransitions()
        {
            int? registerId = GetIntQueryParam("registerId");
            if (registerId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string jsonFilters = GetRequestData();
            try
            {

                var registerHead = DbContext.GetRegisterAttributeHeadDao().GetCurrentHeadForRegister((int)registerId);

                var jsonHelper = new RegisterTransitionJsonHelper(SmartContext);
                var statesFilter = jsonHelper.DeserializeFilters(jsonFilters, registerHead);

                var transitionService = SmartContext.RegisterTransitionService;
                var list = transitionService.GetAll(statesFilter);

                var importListTable = jsonHelper.GetStatesTable(GetApplicationPath(), list, statesFilter);

                var jsonData = JsonConvert.SerializeObject(importListTable);


                return Content(jsonData, "application/json");
            }
            catch (Exception exx)
            {
                return Content(exx.Message, "application/json");
            }
        }

        public ActionResult TransitionData(int? tranId, int? registerId)
        {
            RegisterAttributesHead registerHead = null;
            if (registerId != null) {
                registerHead = DbContext.GetRegisterAttributeHeadDao().GetCurrentHeadForRegister((int)registerId);
            }

            var registerTransService = SmartContext.RegisterTransitionService;
            RegisterTransition registerTrans = null;
            if (tranId != null)
            {
                registerTrans = registerTransService.GetTransition((int)tranId);
                registerHead = DbContext.GetRegisterAttributeHeadDao().GetCurrentHeadForRegister((int)registerTrans.Register.Id);
            }
            var jsonHelper = new RegisterTransitionJsonHelper(SmartContext);

            var areaData = jsonHelper.GetTransControlls(registerTrans, registerHead);
            var jsonData = JsonConvert.SerializeObject(areaData);

            return Content(jsonData, "application/json");
        }

        [HttpPost]
        public ActionResult SaveTransitionData()
        {
            string requestData = GetRequestData();
            try
            {
                var jsonHelper = new RegisterTransitionJsonHelper(SmartContext);
                var transitionMdl = jsonHelper.ParseAreaFromJson(requestData);

                var transService = SmartContext.RegisterTransitionService;

                var validationRes = transService.ValidateTransitionModel(transitionMdl);
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


                var transition = transService.SaveTransitionModel(transitionMdl);
                return Json(new { status = "Success", errorMessage = "", redirectUrl = this.Url.Action("Index", "RegisterTransition", new { id = transition.Register.Id }) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
             //   SmartContext.DbContext.RollbackTransaction();
                return Json(new { status = "Error", errorMessage = ex.Message, redirectUrl = string.Empty }, JsonRequestBehavior.AllowGet);

            }

        }
    }
}