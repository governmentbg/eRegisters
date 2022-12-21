using Newtonsoft.Json;
using SmartRegistry.CommonWeb.JsonHelpers;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SmartRegistry.Web.Controllers
{
    public class WebServicesController : BaseController
    {
        public ActionResult List(int id)
        {
            var regServ = SmartContext.RegistersService;
            var register = regServ.GetRegister(id);
            ViewBag.Title = register.Name;
            ViewBag.RegisterId = register.Id;
            return View();
        }

        public ActionResult GetFilterControls()
        {
            var jsonHelper = new WebServiceListJsonHelper(SmartContext);

            var usersFilters = jsonHelper.GetWebServicesFilterControls();
            var jsonData = JsonConvert.SerializeObject(usersFilters);

            return Content(jsonData, "application/json");
        }

        public ActionResult GetWebServicesList()
        {
            int? registerId = GetIntQueryParam("registerId");
            if (registerId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string jsonFilters = GetRequestData();
            try
            {
                var jsonHelper = new WebServiceListJsonHelper(SmartContext);
                var webServiceFilter = jsonHelper.DeserializeFilters(jsonFilters);
                webServiceFilter.RegisterId = registerId;

                var webServService = SmartContext.WebServicesService;
                var webServResult = webServService.GetWebServices(webServiceFilter);
                var webServTable = jsonHelper.GetWebServicesTable(GetApplicationPath(), webServResult, webServiceFilter);

                var jsonData = JsonConvert.SerializeObject(webServTable);
                return Content(jsonData, "application/json");
            }
            catch (Exception exx)
            {
                return Content(exx.Message, "application/json");
            }
        }

        public ActionResult CreateManagementService(int id)
        {
            ViewBag.RegisterId = id;
            ViewBag.ServiceType = "management";
            ViewBag.GetControlsUrl = Url.Action("WebServiceData", "WebServices", new { serviceType = "management", registerId = id });
            // ViewBag.PageName = "Създаване на уеб услуга за вписване/промяна/заличаване";
            ViewBag.PageName = Resources.Content.webservices_edit_managment_header;

            return View("Edit");
        }

        public ActionResult CreateReportService(int id)
        {
            ViewBag.RegisterId = id;
            ViewBag.ServiceType = "report";
            ViewBag.GetControlsUrl = Url.Action("WebServiceData", "WebServices", new { serviceType = "report", registerId = id });
            //  ViewBag.PageName = "Създаване на уеб услуга за удостоверяване";
            ViewBag.PageName = Resources.Content.webservices_edit_report_header;

            return View("Edit");
        }

        public ActionResult Edit(int id)
        {
            var webServService = SmartContext.WebServicesService;
            var webServ = webServService.GetWebServiceById(id);
            if (webServ == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.RegisterId = webServ.Register.Id;
            if (webServ.ServiceType == WebServiceType.ReportService)
            {
                ViewBag.ServiceType = "report";
            }
            else
            {
                ViewBag.ServiceType = "management";
            }
            ViewBag.GetControlsUrl = Url.Action("WebServiceData", "WebServices", new { serviceType = ViewBag.ServiceType, registerId = webServ.Register.Id, webServiceId = id });
           // ViewBag.PageName = "Редактиране на уеб услуга";
            ViewBag.PageName = Resources.Content.webservices_edit_header;

            return View("Edit");
        }

        public ActionResult WebServiceData(int registerId, string serviceType, int? webServiceId)
        {
            var webServService = SmartContext.WebServicesService;
            WebServiceISCIPR webServ = null;
            if (webServiceId != null)
            {
                webServ = webServService.GetWebServiceById((int)webServiceId);
            }

            var jsonHelper = new WebServiceJsonHelper(SmartContext);

            var webServData = jsonHelper.GetEditControls(webServ, registerId, serviceType);
            var jsonData = JsonConvert.SerializeObject(webServData);

            return Content(jsonData, "application/json");
        }

        [HttpPost]
        public ActionResult Save()
        {
            int? registerId = GetIntQueryParam("registerId");
            if (registerId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string requestData = GetRequestData();
            try
            {
                var jsonHelper = new WebServiceJsonHelper(SmartContext);
                var webServMdl = jsonHelper.ParseWebServiceFromJson(requestData, (int)registerId);

                var webServService = SmartContext.WebServicesService;
                var validationRes = webServService.ValidateWebServiceModel(webServMdl);
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
                webServService.SaveWebServiceModel(webServMdl);
            }
            catch (Exception ex)
            {
                SmartContext.DbContext.RollbackTransaction();
                return Json(new { status = "Error", errorMessage = ex.Message, redirectUrl = string.Empty }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { status = "Success", errorMessage = "", redirectUrl = this.Url.Action("List", "WebServices", new { id = registerId }) }, JsonRequestBehavior.AllowGet);
        }

        public FileResult DownloadXSD(int id)
        {
            var webServService = SmartContext.WebServicesService;
            var requestXSD = webServService.GetRequestXSD(id);

            using (var memStream = new MemoryStream())
            {
                //requestXSD.Save(memStream);
                requestXSD.Write(memStream);
                return File(memStream.ToArray(), "application/xml", "RegServiceRequest.xsd");
            }
            //var responseXSD = webServService.GetResponseXSD(id);


        }

    }
}