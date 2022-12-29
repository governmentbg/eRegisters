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
            ViewBag.Title = regName;

            var regHead = DbContext.GetRegisterAttributeHeadDao().GetCurrentHeadForRegister(register.Id);

            ViewBag.InitialStates = regHead.RegisterStatesList.Where(x => x.InitialState == 1).ToList(); 



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

        public ActionResult Create(int registerId,int? stateId)
        {
           
            var register = DbContext.GetRegistersDao().GetById(registerId);
            var regName = (register != null) ? register.Name : string.Empty;
            ViewBag.PageName = regName;
            ViewBag.ParentId = registerId;
            ViewBag.Register = registerId;
            ViewBag.EntityId = 5;          
            ViewBag.GetControlsUrl = Url.Action("GetRecordControls", "RegisterRecords", new { registerId = registerId});

            if (stateId != null) { 
                ViewBag.GetControlsUrl = Url.Action("GetRecordControls", "RegisterRecords", new { stateId = stateId });
          
                var regStateService = SmartContext.RegisterStatesService;
                var regState = regStateService.GetState((int)stateId);
               // var transitionList = SmartContext.RegisterTransitionService.GetEndTransitions(regState);
             //   ViewBag.EndList = transitionList;
                ViewBag.State = regState.Name;
            }

            ViewBag.SaveControlsUrl = Url.Action("SaveRegisterRecord", "RegisterRecords");

           

            return View("Edit");
        }

        public ActionResult Edit(int id, int? stateId)
        {
            //  ViewBag.PageName = "Промяна на обстоятелства";

            var registerRec = SmartContext.RegisterRecordsService.GetRegisterRecord(id);
            var regName = (registerRec != null) ? registerRec.Register.Name : string.Empty;
            ViewBag.PageName =  regName;
            ViewBag.ParentId = id;
            ViewBag.Register = registerRec.Register.Id;

            if (registerRec.RegisterState != null)
            {
                var transitionList = SmartContext.RegisterTransitionService.GetEndTransitions(registerRec.RegisterState);
                var list = SmartContext.RegisterTransitionService.CheckForRights(transitionList);
                ViewBag.EndList = list;
                ViewBag.State = registerRec.RegisterState.Name;
            }
            if (stateId!=null) {
                var selectedState = SmartContext.RegisterStatesService.GetState((int)stateId);
                //var transitionList = SmartContext.RegisterTransitionService.GetEndTransitions(selectedState);
                ViewBag.EndList = null;
                ViewBag.State = selectedState.Name;     
            }

            ViewBag.GetControlsUrl = Url.Action("GetRecordControls", "RegisterRecords", new { recordId = id , stateId = stateId });          
            ViewBag.SaveControlsUrl = Url.Action("SaveRegisterRecord", "RegisterRecords");
            TempData["name"] = stateId;

            return View();
        }
               
        public ActionResult GetRecordControls(int? registerId, int? stateId, long? recordId)
        {
            var regRecService = SmartContext.RegisterRecordsService;
            var regStateService = SmartContext.RegisterStatesService;
          
            var endTrans = GetIntQueryParam("amp;stateId");

            int regId = 0;
            RegisterRecord regRecord = null;
            RegisterState regState = null;

            if (stateId != null)
            {

                regState = regStateService.GetState((int)stateId);
                if (regState == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NoContent);
                }
                regId = regState.Register.Id;
            }

            if (recordId != null)
            {
                regRecord = regRecService.GetRegisterRecord((long)recordId);
                if (regRecord == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NoContent);
                }
                regId = regRecord.Register.Id;

                if (regRecord.RegisterState != null) {
                    stateId = regRecord.RegisterState.Id;
                    regState = regStateService.GetState((int)stateId);
                }
                if (endTrans != null) {
                    stateId = endTrans;
                    regState = regStateService.GetState((int)stateId);
                }
            }

            if (registerId != null)
            {
                if (registerId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                regId = (int)registerId;    
            }

            var regService = SmartContext.RegistersService;
            var allAttributes = regService.GetEditableAttributes(regId);

            if (stateId != null) {
                allAttributes= filterStateAttributes(regState, allAttributes);               
            }

            var jsonHelper = new RegisterRecordJsonHelper(SmartContext);
            var recControls = jsonHelper.GetRecordEditControls(GetFullApplicationPath(),allAttributes, regRecord, regId, stateId);
            var jsonData = JsonConvert.SerializeObject(recControls);

            return Content(jsonData, "application/json");
        }

        private List<RegisterAttribute> filterStateAttributes(RegisterState regState,IList<RegisterAttribute> allAttributes)
        {
            List<RegisterAttribute> regAttributeList = new List<RegisterAttribute>();
            var stateAttributes = regState.AttributeList.Select(x => x.RegisterAttribute).ToList();
            foreach (RegisterAttribute ra in stateAttributes)
            {
                if (allAttributes.Contains(ra))
                {
                    regAttributeList.Add(ra);
                }
            }
            return regAttributeList;
          
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


                var record = regService.SaveRegisterRecordModel(attrValuesMdl);
                registerId = (int)attrValuesMdl.RegisterId;

                if (attrValuesMdl.StateId != 0)
                {
                    return Json(new { status = "Success", errorMessage = "", redirectUrl = this.Url.Action("Edit", "RegisterRecords", new { id = record.Id }) }, JsonRequestBehavior.AllowGet);
                }
                else {
                    return Json(new { status = "Success", errorMessage = "", redirectUrl = this.Url.Action("Index", "RegisterRecords", new { id = registerId }) }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                SmartContext.DbContext.RollbackTransaction();
                return Json(new { status = "Error", errorMessage = ex.Message, redirectUrl = string.Empty }, JsonRequestBehavior.AllowGet);

            }
         
        }

    }
}