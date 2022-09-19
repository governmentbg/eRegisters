using Newtonsoft.Json;
using SmartRegistry.CommonWeb.JsonHelpers;
using SmartRegistry.CommonWeb.JsonControls;
using SmartRegistry.Domain.Entities;
using SmartRegistry.Domain.QueryFilters;
using SmartRegistry.Domain.Services;
using SmartRegistry.Web.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using SmartRegistry.CommonWeb;

namespace SmartRegistry.Web.Controllers
{
    [Authorize]
    public class RegistersController : BaseController
    {
        public RegistersController()
            : base()
        {
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Регистър на регистрите";
            ViewBag.Type = "list";
            ViewBag.ListAction = "GetRegistersList";

            return View();
        }

        public ActionResult Structure()
        {
            ViewBag.Title = "Структура на регистър";
            ViewBag.Type = "structure";
            ViewBag.ListAction = "GetRegistersListStructure";

            return View("Index");
        }

        [HttpGet]
        public ActionResult GetFilterControls()
        {
            var jsonHelper = new RegisterListJsonHelper(SmartContext);
            var registerFilters = jsonHelper.GetFilters(RegisterListType.List);

            var jsonData = JsonConvert.SerializeObject(registerFilters);

            return Content(jsonData, "application/json");
        }

        public ActionResult GetRegistersListStructure()
        {
            return GetRegistersListResult("structure");
        }

        public ActionResult GetRegistersList()
        {
            return GetRegistersListResult("list");
        }

        private ActionResult GetRegistersListResult(string type)
        {
            string jsonFilters = GetRequestData();
            try
            {
                var jsonHelper = new RegisterListJsonHelper(SmartContext);
                var regFilter = jsonHelper.DeserializeFilters(jsonFilters);

                var regService = SmartContext.RegistersService;

                regFilter.Type = (int)RegisterListType.List;
                var listType = RegisterListType.List;
                if ("structure".Equals(type)) listType = RegisterListType.Structure;

                var registersResult = regService.GetRegisters(regFilter, listType);
                var registerTable = jsonHelper.GetRegistersTable(GetApplicationPath(), registersResult, listType, regFilter);
            

                var jsonData = JsonConvert.SerializeObject(registerTable);
                return Content(jsonData, "application/json");
            }
            catch (Exception exx)
            {
                return Content(exx.Message, "application/json");
            }
        }

        [HttpGet]
        public ActionResult RegisterRights(int id)
        {
            ViewBag.GetControlsUrl = Url.Action("GetRegisterRightsControls", "Registers", new { registerId = id });
            ViewBag.PageName = "Права за регистър";

            if ((SmartContext.CurrentUser == null)
                || (!SmartContext.CurrentUser.HasRegisterRight(id, RegisterPermissionEnum.ManageRegisterRights)))
            {
                return new HttpStatusCodeResult(403);
            }

            return View("RegisterRights");
        }


        [HttpGet]
        public ActionResult GetRegisterColsAjax(string filterValue, string search)
        {
            int regId;
            if (!int.TryParse(filterValue, out regId)) return null;

            var regService = SmartContext.RegistersService;
            var regAttributes = regService.GetAttributesForReferenceLink(regId,search);

            var jsonHelper = new RegisterJsonHelper(SmartContext);
            var resultList = jsonHelper.GetAjaxRegisterColList(regAttributes);

            var jsonData = JsonConvert.SerializeObject(resultList);


            return Content(jsonData, "application/json");
        }


        [HttpGet]
        public ActionResult GetAllRegistersAjax(string search)
        {
            var regService = SmartContext.RegistersService;

            var regFilter = new RegisterFilter()
            {
                IsActive = true,
                PageNumber = 1,
                PageSize = 100,
                OrderByColumn = "Name"
            };
            if (!string.IsNullOrEmpty(search))
            {
                regFilter.Name = $"%{search}%";
            }
            var regResult = regService.GetRegisters(regFilter);

            var jsonHelper = new RegisterJsonHelper(SmartContext);
            var resultList = jsonHelper.GetAjaxRegisterList(regResult.Results);

            var jsonData = JsonConvert.SerializeObject(resultList);


            return Content(jsonData, "application/json");
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetTestRepeater()
        {
            var transformer = new TestControlsTransformer(SmartContext);
            var controls = transformer.GetRepeaterControls(GetFullApplicationPath());
            var jsonData = JsonConvert.SerializeObject(controls);


            return Content(jsonData, "application/json");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetTestFieldset()
        {
            var transformer = new TestControlsTransformer(SmartContext);
            var controls = transformer.GetFieldsetControl();
            var jsonData = JsonConvert.SerializeObject(controls);


            return Content(jsonData, "application/json");
        }

        [AllowAnonymous]
        public ActionResult TestControls()
        {
            ViewBag.GetControlsUrl = Url.Action("GetTestRepeater", "Registers");
            ViewBag.PageName = "Тестови контроли";

            return View();
        }

        [AllowAnonymous]
        public ActionResult TestFieldset()
        {
            ViewBag.GetControlsUrl = Url.Action("GetTestFieldset", "Registers");
            ViewBag.PageName = "Тестови fieldset контроли";

            return View();
        }



        [AllowAnonymous]
        public ActionResult SaveTestControls()
        {
            var jsonData = GetRequestData();
            try
            {
                if (!string.IsNullOrEmpty(jsonData))
                {
                    dynamic controls = JsonConvert.DeserializeObject(jsonData);

                    string commonError = null;
                    List<object> ctrlErr = null;

                    foreach(var control in controls)
                    {
                        if (control.name == "HasCommonError")
                        {
                            bool hasCommonError = control.value;
                            if (hasCommonError)
                            {
                                commonError = "Тестово съобщение за Обща грешка";
                            }
                        }
                        if (control.name == "HasControlErrors")
                        {
                            bool hasControlErrors = control.value;
                            if (hasControlErrors)
                            {
                                ctrlErr = new List<object>()
                                {
                                    new { controlMessage = "Грешка в паролата", controlName = "Password"},
                                    new { controlMessage = "Грешка в нецялото число", controlName = "NumericField"}
                                };
                            }
                        }
                    }

                    return Json(new
                    {
                        status = "Error",
                        errorMessage = commonError,
                        redirectUrl = string.Empty,
                        controlErrors = ctrlErr
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { status = "Error", errorMessage = "Няма данни", redirectUrl = string.Empty });
            }
            catch (Exception ex)
            {
                return Json(new { status = "Error", errorMessage = ex.Message, redirectUrl = string.Empty });
            }

        }


        [AllowAnonymous]
        public ActionResult SaveFieldSetControls()
        {
            var jsonData = GetRequestData();
            try
            {
                if (!string.IsNullOrEmpty(jsonData))
                {
                    dynamic controls = JsonConvert.DeserializeObject(jsonData);

                    string commonError = null;
                    List<object> ctrlErr = null;
                  
                    return Json(new
                    {
                        status = "Error",
                        errorMessage = commonError,
                        redirectUrl = string.Empty,
                        controlErrors = ctrlErr
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { status = "Error", errorMessage = "Няма данни", redirectUrl = string.Empty });
            }
            catch (Exception ex)
            {
                return Json(new { status = "Error", errorMessage = ex.Message, redirectUrl = string.Empty });
            }

        }


        [HttpGet]
        public ActionResult GetRegisterRightsControls(int registerId)
        {
            var registersService = SmartContext.RegistersService;
            var register = registersService.GetRegister(registerId);

            var allPermissions = SmartContext.DbContext.GetRegisterPermissionsDao().GetAll();
            var allowedUserGroups = registersService.GetPossibleUserGroupsForRegister(registerId);

            var jsonHelper = new RegisterRightsJsonHelper(SmartContext);
            var registerData = jsonHelper.GetRegisterRightsControls(register, allPermissions, allowedUserGroups);

            var jsonData = JsonConvert.SerializeObject(registerData);

            return Content(jsonData, "application/json");
        }

        [HttpPost]
        public ActionResult SaveRegisterRights()
        {
            string requestData = GetRequestData();
            try
            {
                var jsonHelper = new RegisterRightsJsonHelper(SmartContext);
                var rightsMdlList = jsonHelper.ParseRightsFromJson(requestData);


                var regService = SmartContext.RegistersService;
                var validationRes = regService.ValidateRegisterRightsModel(rightsMdlList);
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



           
                regService.SaveRegisterRightsModel(rightsMdlList);
            }
            catch (Exception ex)
            {
                SmartContext.DbContext.RollbackTransaction();
                return Json(new { status = "Error", errorMessage = ex.Message, redirectUrl = string.Empty }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { status = "Success", errorMessage = "", redirectUrl = this.Url.Action("Index", "Registers") }, JsonRequestBehavior.AllowGet);

            //string requestData = GetRequestData();
            //try
            //{
            //    var registersService = SmartContext.RegistersService;
            //    registersService.SaveRightsFromJson(requestData);

            //}
            //catch (Exception ex)
            //{
            //    SmartContext.DbContext.RollbackTransaction();
            //    return Json(new { status = "Error", errorMessage = ex.Message, redirectUrl = string.Empty }, JsonRequestBehavior.AllowGet);

            //}
            //return Json(new { status = "Success", errorMessage = "", redirectUrl = this.Url.Action("Index", "Registers") }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.GetControlsUrl = Url.Action("RegisterData", "Registers");
            ViewBag.SaveControlsUrl = Url.Action("SaveRegisterData", "Registers");
            ViewBag.PageName = "Вписване на регистър";
            return View("Edit");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            ViewBag.GetControlsUrl = Url.Action("RegisterData", "Registers", new { registerId = id });
            ViewBag.SaveControlsUrl = Url.Action("SaveRegisterData", "Registers");
            ViewBag.PageName = "Редакция на регистър";
            ViewBag.Type = "Edit";

            return View("Edit");
        }

        [HttpGet]
        public ActionResult EditStructure(int id)
        {

            ViewBag.GetControlsUrl = Url.Action("RegisterStructureData", "Registers", new { registerId = id });
            ViewBag.SaveControlsUrl = Url.Action("SaveRegisterStructureData", "Registers");
                        
            var registersService = SmartContext.RegistersService;
            var register = registersService.GetRegister(id);

            ViewBag.PageName = "Редакция на структура регистър - " + register.Name;
            ViewBag.Type = "EditStructure";

            if ((SmartContext.CurrentUser == null)
                || (!SmartContext.CurrentUser.HasRegisterRight(id, RegisterPermissionEnum.ManageRegistryStructure)))
            {
                return new HttpStatusCodeResult(403);
            }

            return View("Edit");
        }

        public ActionResult RegisterData(int? registerId)
        {
            var registersService = SmartContext.RegistersService;

            Register register = null;
            if (registerId != null)
            {
                register = registersService.GetRegister((int)registerId);
            }
            var jsonHelper = new RegisterJsonHelper(SmartContext);

            var registerData = jsonHelper.GetRegisterControls(register);
            var jsonData = JsonConvert.SerializeObject(registerData);

            return Content(jsonData, "application/json");
        }

        public ActionResult RegisterStructureData(int? registerId)
        {
            var registersService = SmartContext.RegistersService;
            Register register = null;
            if(registerId != null)
            {
                register = registersService.GetRegister((int)registerId);
            }

            var jsonHelper = new RegisterStructureJsonHelper(SmartContext);
            var registerData = jsonHelper.GetEditRegisterStructureControls(register);
            var jsonData = JsonConvert.SerializeObject(registerData);

            return Content(jsonData, "application/json");
        }

        [HttpPost]
        public ActionResult SaveRegisterStructureData()
        {
            string requestData = GetRequestData();
            try
            {
                var jsonHelper = new RegisterStructureJsonHelper(SmartContext);
                var structureMdl = jsonHelper.ParseRegisterAttributesFromJson(requestData);

                var registersService = SmartContext.RegistersService;

                var validationRes = registersService.ValidateRegisterStructureModel(structureMdl);
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


                registersService.SaveRegisterAttributesModel(structureMdl);
            }
            catch (Exception ex)
            {
                SmartContext.DbContext.RollbackTransaction();
                return Json(new { status = "Error", errorMessage = ex.Message, redirectUrl = string.Empty }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { status = "Success", errorMessage = "", redirectUrl = this.Url.Action("Structure", "Registers") }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveRegisterData()
        {
            string requestData = GetRequestData();
            try
            {
                var jsonHelper = new RegisterJsonHelper(SmartContext);
                var registerMdl = jsonHelper.ParseRegisterFromJson(requestData);

                var regService = SmartContext.RegistersService;


                var validationRes = regService.ValidateRegisterModel(registerMdl);
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

                regService.SaveRegisterModel(registerMdl);
            }
            catch (Exception ex)
            {
                SmartContext.DbContext.RollbackTransaction();
                return Json(new { status = "Error", errorMessage = ex.Message, redirectUrl = string.Empty }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { status = "Success", errorMessage = "", redirectUrl = this.Url.Action("Index", "Registers") }, JsonRequestBehavior.AllowGet);
        }

    }
}