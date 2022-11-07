using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using SmartRegistry.Web.AuthHelp;
using SmartRegistry.CommonWeb.JsonHelpers;
using Newtonsoft.Json;
using SmartRegistry.Domain.Entities;
using SmartRegistry.Domain.QueryFilters;
using System.Threading.Tasks;
using SmartRegistry.CommonWeb.JsonControls;

namespace SmartRegistry.Web.Controllers
{
    public class ImportController : BaseController
    {
        public static class GroupTypes
        {
            public const string GenCSV = "Генерирай CSV";
            public const string GenCSVData = "Генерирай CSV с данни";
            public const string UploadCSV = "Качи CSV";
            public const string Validate = "Валидирай";
            public const string Reupload = "Качи файл отново";
            public const string Import = "Импортирай";
            public const string ClearAndImport = "Импортирай със зануляване";
        }


        public ActionResult Index()
        {

            ViewBag.Title = "Импортване на данни ";
            ViewBag.ListAction = "GetImportList";

            return View();
        }



        [HttpGet]
        public ActionResult Create()
        {
           
            ViewBag.PageName = "Импортиране на нов документ ";

            var registerFilter = new RegisterFilter();
            registerFilter.AdminBody = SmartContext.CurrentAdminBody;
            registerFilter.PageSize = 999;
            var registers = SmartContext.DbContext.GetRegistersDao().GetAll(registerFilter);
            ViewBag.Registers = registers.Results;

            return View("Create");
        }

    

        [HttpPost]
        public ActionResult CreateData(string RegisterList,string submitButton, HttpPostedFileBase file)
        {

            if ((submitButton == GroupTypes.GenCSV)
              || (submitButton == GroupTypes.GenCSVData)) {
                var regId = Int32.Parse(RegisterList);
                string csv = string.Empty;
                if (submitButton == GroupTypes.GenCSV)
                {
                    csv = SmartContext.ImportService.GetCSVStructure(regId);
                }
                else
                {
                    csv = SmartContext.ImportService.GetCSVStructureAndData(regId);
                }
                if (string.IsNullOrEmpty(csv))
                {
                    File(Encoding.UTF8.GetBytes(string.Empty), "text/csv", "NoData.csv");
                }

                var selectedRegister = SmartContext.DbContext.GetRegistersDao().GetById(regId);
                var fileName = string.IsNullOrEmpty(selectedRegister.NamespaceApi) ? selectedRegister.Name : selectedRegister.NamespaceApi;
                fileName = "Structure_" + fileName;
                return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", fileName + ".csv");
            }

            if (submitButton == GroupTypes.UploadCSV)
            {
                try
                {
                    if (file.ContentLength > 0)
                    {

                        var regId = Int32.Parse(RegisterList);
                        var registersDao = SmartContext.DbContext.GetRegistersDao();
                        var registerHeadDao = SmartContext.DbContext.GetRegisterAttributeHeadDao();
                        var selectedRegister = registersDao.GetById(regId);
                        var regHead = registerHeadDao.GetCurrentHeadForRegister(selectedRegister.Id);
                        var guid = Guid.NewGuid().ToString() + ".csv";

                        string _FileName = Path.GetFileName(file.FileName);
                        string _path = Path.Combine(Server.MapPath("~/ImportFiles"), guid);
                        file.SaveAs(_path);

                        ImportHead importHead = new ImportHead();
                        importHead.CreatedBy = SmartContext.CurrentUser;
                        importHead.FileName = guid;
                        importHead.InsDateTime = DateTime.Now;
                        importHead.UserFileName = file.FileName;
                        importHead.AttributeHead = regHead;
                        importHead.Register = selectedRegister;
                        importHead.Status = ImportHeadStatus.UploadedFile;

                        SmartContext.DbContext.ImportHeadDao.Save(importHead);


                    }
                    ViewBag.Message = "Файлът е качен успешно!";
                    ViewBag.PageName = "Импортиране на нов документ ";

                    var registerFilter = new RegisterFilter();
                    registerFilter.AdminBody = SmartContext.CurrentAdminBody;
                    registerFilter.PageSize = 999;
                    var registers = SmartContext.DbContext.GetRegistersDao().GetAll(registerFilter);               
                    ViewBag.Registers = registers.Results;


                    return View("Create");
                }
                catch
                {
                    ViewBag.Message = "Възникна грешка с качването на файла!";
                    ViewBag.PageName = "Импортиране на нов документ ";

                    var registerFilter = new RegisterFilter();
                    registerFilter.AdminBody = SmartContext.CurrentAdminBody;
                    registerFilter.PageSize = 999;
                    var registers = SmartContext.DbContext.GetRegistersDao().GetAll(registerFilter);
                    ViewBag.Registers = registers.Results;


                    return View("Create");
                }

            }


           return View("Create");
        }

        [HttpPost]
        public ActionResult ReviewData(string submitButton, HttpPostedFileBase file, ImportHead model, int Id)
        {
            var importHead = SmartContext.DbContext.ImportHeadDao.GetById(Id);

            if (submitButton == GroupTypes.Validate)
            {
                string errMsg = string.Empty;
                try
                {
                    string path = Server.MapPath("~/ImportFiles/");
                    errMsg = SmartContext.ImportService.ProcessFileImportHead(Id, path);
                }
                catch (Exception ex)
                {
                    errMsg = ex.Message;
                }
                ViewBag.ValidationErrors = errMsg;
            }

            if (submitButton == GroupTypes.Reupload)
            {
                ViewBag.ShowFile = "show";
            }

            if (submitButton == GroupTypes.UploadCSV)
            {
                ViewBag.ShowFile = "show";
                try
                {
                    if (file.ContentLength > 0)
                    {
                        var guid = Guid.NewGuid().ToString() + ".csv";

                        string _FileName = Path.GetFileName(file.FileName);
                        string _path = Path.Combine(Server.MapPath("~/ImportFiles"), guid);
                        file.SaveAs(_path);
                      
                        importHead.CreatedBy = SmartContext.CurrentUser;
                        importHead.FileName = guid;
                        importHead.InsDateTime = DateTime.Now;
                        importHead.UserFileName = file.FileName;
                        importHead.Status = ImportHeadStatus.UploadedFile;
                        SmartContext.DbContext.ImportHeadDao.Update(importHead);


                    }
                    ViewBag.Message = "Файлът е качен успешно!";
                 
                }
                catch
                {
                    ViewBag.Message = "Възникна грешка с качването на файла!";                 
                }
            }

            if (submitButton == GroupTypes.Import)
            {

                var importResult = SmartContext.ImportService.ImportData(importHead);
                ViewBag.InsertResult = importResult;

            }

            if (submitButton == GroupTypes.ClearAndImport)
            {
                SmartContext.DbContext.RegisterRecordsDao.ClearRegisterRecords(importHead.Register);               
                var importResult = SmartContext.ImportService.ImportData(importHead);
                ViewBag.InsertResult = importResult;
            }

            ViewBag.ImportHead = importHead;

            return View("Review", importHead);
        }


        [HttpGet]
        public ActionResult Review(int id)
        {

            var importHead = SmartContext.DbContext.ImportHeadDao.GetById(id);
            ViewBag.ImportHead = importHead;

            return View(importHead);
        }

        public ActionResult GetFilterControls()
        {
            var jsonHelper = new ImportHeadJsonHelper(SmartContext);
            var importHeadFilter = jsonHelper.GetFilters();

            var jsonData = JsonConvert.SerializeObject(importHeadFilter);

            return Content(jsonData, "application/json");
        }

        public ActionResult GetImportList()
        {
            string jsonFilters = GetRequestData();
            try
            {
                var jsonHelper = new ImportHeadJsonHelper(SmartContext);
                var logFilter = jsonHelper.DeserializeFilters(jsonFilters);

                var impHeadService = SmartContext.ImportService;
                var list = impHeadService.GetAll(logFilter);

                var areaListTable = jsonHelper.GetImportTable(GetApplicationPath(), list, logFilter);

                var jsonData = JsonConvert.SerializeObject(areaListTable);


                return Content(jsonData, "application/json");
            }
            catch (Exception exx)
            {
                return Content(exx.Message, "application/json");
            }
        }

    }
}