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
    public class RegisterStatesController : BaseController
    {       
        public ActionResult Index(int id)
        {
            ViewBag.RegisterId = id;

            var register = DbContext.GetRegistersDao().GetById(id);
            var regName = (register != null) ? register.Name : string.Empty;
            ViewBag.Title = regName;
            
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            var regId = GetIntQueryParam("registerId");
            ViewBag.RegisterId = regId;
            ViewBag.GetControlsUrl = Url.Action("StateData", "RegisterStates", new { registerId = regId });
            ViewBag.SaveControlsUrl = Url.Action("SaveStateData", "RegisterStates");
            ViewBag.PageName = Resources.Content.registerstates_create_header;


            return View("Edit");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var register = DbContext.GetRegisterStatesDao().GetById(id).Register;
            ViewBag.RegisterId = register.Id;
            ViewBag.GetControlsUrl = Url.Action("StateData", "RegisterStates", new { stateId = id , registerId = register.Id });
            ViewBag.SaveControlsUrl = Url.Action("SaveStateData", "RegisterStates");
            ViewBag.PageName = Resources.Content.registerstates_edit_header;


            return View("Edit");
        }


        public ActionResult GetFilterControls(int id)
        {
          
            var registerHead = DbContext.GetRegisterAttributeHeadDao().GetCurrentHeadForRegister(id);

            var jsonHelper = new RegisterStatesJsonHelper(SmartContext);
            var statesFilter = jsonHelper.GetFilters();

            var jsonData = JsonConvert.SerializeObject(statesFilter);

            return Content(jsonData, "application/json");

        }

        public ActionResult GetRegisterStates()
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

                var jsonHelper = new RegisterStatesJsonHelper(SmartContext);
                var statesFilter = jsonHelper.DeserializeFilters(jsonFilters,registerHead);

                var statesService = SmartContext.RegisterStatesService;
                var list = statesService.GetAll(statesFilter);

                var importListTable = jsonHelper.GetStatesTable(GetApplicationPath(), list, statesFilter);

                var jsonData = JsonConvert.SerializeObject(importListTable);


                return Content(jsonData, "application/json");
            }
            catch (Exception exx)
            {
                return Content(exx.Message, "application/json");
            }
        }



        public ActionResult StateData(int? stateId, int? registerId)
        {
            RegisterAttributesHead registerHead = null;
            if (registerId != null) {
                registerHead = DbContext.GetRegisterAttributeHeadDao().GetCurrentHeadForRegister((int)registerId);
            }

            var registerStateService = SmartContext.RegisterStatesService;
            RegisterState registerState = null;
            if (stateId != null)
            {
                registerState = registerStateService.GetState((int)stateId);
                registerHead = DbContext.GetRegisterAttributeHeadDao().GetCurrentHeadForRegister((int)registerState.Register.Id);
            }
            var jsonHelper = new RegisterStatesJsonHelper(SmartContext);

            var areaData = jsonHelper.GetStateControlls(registerState, registerHead);
            var jsonData = JsonConvert.SerializeObject(areaData);

            return Content(jsonData, "application/json");
        }

        [HttpPost]
        public ActionResult SaveStateData()
        {
            string requestData = GetRequestData();
            try
            {
                var jsonHelper = new RegisterStatesJsonHelper(SmartContext);
                var stateMdl = jsonHelper.ParseAreaFromJson(requestData);

                var statesService = SmartContext.RegisterStatesService;

                var validationRes = statesService.ValidateStateModel(stateMdl);
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


                var state = statesService.SaveStateModel(stateMdl);
                //   return Json(new { status = "Success", errorMessage = "", redirectUrl = this.Url.Action("Index", "RegisterStates", new { id = state.RegisterHead.Register.Id }) }, JsonRequestBehavior.AllowGet);
                return Json(new { status = "Success", errorMessage = "", redirectUrl = this.Url.Action("Index", "RegisterStates", new { id = state.Register.Id }) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                SmartContext.DbContext.RollbackTransaction();
                return Json(new { status = "Error", errorMessage = ex.Message, redirectUrl = string.Empty }, JsonRequestBehavior.AllowGet);

            }
          
        }
    }
}