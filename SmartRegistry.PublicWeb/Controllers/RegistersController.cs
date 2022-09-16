using Newtonsoft.Json;
using SmartRegistry.CommonWeb.JsonHelpers;
using SmartRegistry.Domain.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using SmartRegistry.Domain.Entities;

namespace SmartRegistry.PublicWeb.Controllers
{
    public class RegistersController : BasePublicController
    {
        // GET: Registers
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetFilterControls(int id)
        {
            var regService = SmartContext.RegistersService;
            var filterAttributes = regService.GetAttributesForFilter(id);

            var jsonHelper = new RegisterRecordListJsonHelper(SmartContext);
            var recordFilters = jsonHelper.GetFilters(filterAttributes, id,RegisterListType.Public);

            var jsonData = JsonConvert.SerializeObject(recordFilters);

            return Content(jsonData, "application/json");
        }

        [HttpGet]
        public ActionResult GetFilterShortControls()
        {
            var jsonHelper = new RegisterListJsonHelper(SmartContext);
            var registerFilters = jsonHelper.GetFiltersShort();
            var jsonData = JsonConvert.SerializeObject(registerFilters);
            return Content(jsonData, "application/json");
        }

        public ActionResult GetRegistersList()
        {
            string jsonFilters = GetRequestData();
            try
            {
                var jsonHelper = new RegisterListJsonHelper(SmartContext);
                var regFilter = jsonHelper.DeserializeFilters(jsonFilters);

                var regService = SmartContext.RegistersService;
                var registersResult = regService.GetRegisters(regFilter, RegisterListType.Public);
                var registerTable = jsonHelper.GetRegistersTable(GetApplicationPath(), registersResult, RegisterListType.Public, regFilter);
                var jsonData = JsonConvert.SerializeObject(registerTable);
                return Content(jsonData, "application/json");
            }
            catch (Exception exx)
            {
                return Content(exx.Message, "application/json");
            }
        }

  

        public ActionResult Show(int? id,string SearchName)
        {
            ViewBag.Id = id;
            if (id != null) {
                Area area = DbContext.GetAreasDao().GetById((int)id);
                ViewBag.AreaName = area.Name;
                ViewBag.GroupName = area.AreaGroup.Name;
            }          
            //  ViewBag.GetControlsUrl = Url.Action("ShowRegister", "Registers", new { themeId = id });  
            string jsonFilters = string.Empty;
            var jsonHelper = new RegisterListJsonHelper(SmartContext);
            var regFilter = jsonHelper.DeserializeFilters(jsonFilters);
            regFilter.PageSize = 0;
            regFilter.Area = id;
            if (SearchName != null && SearchName != string.Empty) {
                regFilter.Name = "%"+SearchName+ "%";
            }
        
            var registerList = DbContext.GetRegistersDao().GetAll(regFilter);

            return View("Show", registerList);
        }

        public ActionResult ShowData(int id)
        {

            ViewBag.RegisterId = id;

            var register = DbContext.GetRegistersDao().GetById(id);
            var regName = (register != null) ? register.Name : string.Empty;
            ViewBag.Title =  regName;
            
            return View("ShowData");
        }

        public ActionResult ShowDataText(int id)
        {
            ViewBag.RegisterId = id;
           
            var registerRecord = DbContext.RegisterRecordsDao.GetById(id);
            ViewBag.Title = "Запис " + registerRecord.URI + " - "+ registerRecord.Register.Name;

            ViewBag.GroupName =registerRecord.Register.Name;
            var registerService = SmartContext.RegistersService;
           
            var attributesToList = registerService.GetAttributesToShow(registerRecord.Register.Id);

            List<TempData> templist = new List<TempData>();
            foreach (var recAttr in attributesToList)
            {
                TempData tdata = new TempData();
                tdata.Title = recAttr.Name;
                var regRecVal = registerRecord.GetValue(recAttr);
                if (regRecVal != null)
                {
                    if (recAttr.UnifiedData.DataType == UnifiedDataTypeEnum.Referential)
                    {
                        tdata.Value = registerRecord.GetReferentialTextForAttribute(recAttr, SmartContext);
                    }
                    else
                    {
                        tdata.Value = regRecVal.ToString();
                    }
                }
                templist.Add(tdata);
            }

            return View("ShowDataText", templist);
        }

        public ActionResult GetRegisterRecords()
        {
            int registerId = 0;
            try
            {
                registerId = int.Parse(Request.Params["registerId"]);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            string jsonFilters = GetRequestData();
            try
            {
                var regRecordsService = SmartContext.RegisterRecordsService;
                var registerService = SmartContext.RegistersService;

                var jsonHelper = new RegisterRecordListJsonHelper(SmartContext);
                var regFilter = jsonHelper.DeserializeFilters(jsonFilters, registerId);

                if (regFilter.Register == null) throw new Exception("Не е зададен регистър във филтъра");

                var attributesToList = registerService.GetAttributesToShow(registerId);

                var registerRecordResult = regRecordsService.GetRegisterRecords(regFilter);
                var registerRecordsTable = jsonHelper.GetRegisterRecordsTable(GetApplicationPath(), registerRecordResult, registerId, attributesToList,RegisterListType.Public);

                var jsonData = JsonConvert.SerializeObject(registerRecordsTable);
                return Content(jsonData, "application/json");
            }
            catch (Exception exx)
            {
                return Content(exx.Message, "application/json");
            }
        }

    

        public ActionResult ShowRegister(int? themeId)
        {
            string jsonFilters = GetRequestData();

            try
            {
                var jsonHelper = new RegisterListJsonHelper(SmartContext);
                var regFilter = jsonHelper.DeserializeFilters(jsonFilters);
              
                regFilter.Area = themeId;
                var registerList = DbContext.GetRegistersDao().GetAll(regFilter);

                var registerTable = jsonHelper.GetRegistersTablePublic(GetApplicationPath(), registerList);
                var jsonData = JsonConvert.SerializeObject(registerTable);
                return Content(jsonData, "application/json");
            }
            catch (Exception exx)
            {
                return Content(exx.Message, "application/json");
            }

        }

    }
}