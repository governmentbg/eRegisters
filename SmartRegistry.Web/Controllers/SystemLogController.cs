using Newtonsoft.Json;
using Orak.Utils;
using SmartRegistry.CommonWeb.JsonHelpers;
using SmartRegistry.Domain.Common;
using SmartRegistry.Domain.Entities;
using SmartRegistry.Domain.QueryFilters;
using SmartRegistry.Web.AuthHelp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartRegistry.Web.Controllers
{
    public class SystemLogController : BaseController
    {

        [CustomAuthorize(PermissionEnum.AccessJournalLog)]
        public ActionResult Index()
        {
            ViewBag.Title = "Системен Журнал Лог";          
            ViewBag.ListAction = "GetSystemLogList";

            return View();
        }

        [CustomAuthorize(PermissionEnum.AccessJournalLog)]
        public ActionResult GetFilterControls()
        {
            var jsonHelper = new SystemLogListJsonHelper(SmartContext);
            var systemLogFilters = jsonHelper.GetFilters();

            var jsonData = JsonConvert.SerializeObject(systemLogFilters);

            return Content(jsonData, "application/json");
        }

        [CustomAuthorize(PermissionEnum.AccessJournalLog)]
        public ActionResult GetSystemLogList()
        {
            string jsonFilters = GetRequestData();
            try
            {
                var jsonHelper = new SystemLogListJsonHelper(SmartContext);
                var logFilter = jsonHelper.DeserializeFilters(jsonFilters);
                TempData["lastFilter"] = logFilter;

                if (!SmartContext.CurrentUser.IsGlobalAdmin) {
                    logFilter.AdminBodyName = SmartContext.CurrentAdminBody.Name;
                }


                var list = SmartContext.SystemLog.GetAll(logFilter);
                var systemLogTable = jsonHelper.GetSystemLogTable(GetApplicationPath(), list, logFilter);


                var jsonData = JsonConvert.SerializeObject(systemLogTable);


                return Content(jsonData, "application/json");
            }
            catch (Exception exx)
            {
                return Content(exx.Message, "application/json");
            }
        }

        [CustomAuthorize(PermissionEnum.AccessJournalLog)]
        public ActionResult Export()
        {
            ViewBag.Title = "Системен лог";
            ViewBag.ListAction = "GetSystemLogList";
            var lastfilter = (SystemLogFilter)TempData["lastFilter"];
            lastfilter.PageSize = 0;
            var list = SmartContext.SystemLog.GetAll(lastfilter);
            exportData(list);

            return RedirectToAction("index");
        }


        [CustomAuthorize(PermissionEnum.AccessJournalLog)]
        private void exportData(PagedResult<SystemLogEvent> list)
        {
            //CSV export
            //StringBuilder sb = new StringBuilder();
            //foreach (var item in list.Results)
            //{
            //    SystemLogEvent logevent = item;
            //    sb.Append(logevent.InfoMessage + ',');
            //    sb.Append(logevent.Type + ',');
            //    sb.Append(logevent.EventTime.ToString() + ',');
            //    sb.Append(logevent.ObjectId + ',');
            //    sb.Append("\r\n");
            //}

            //Encoding altEnc = Encoding.GetEncoding("windows-1251");
            //byte[] bytes2 = altEnc.GetBytes(sb.ToString());
            //return File(bytes2, "text/csv", "Grid.csv");


            //Exel table export
            var expTable = new System.Data.DataTable("exporttable");
            expTable.Columns.Add("Инфо", typeof(string));
            expTable.Columns.Add("Тип", typeof(string));
            expTable.Columns.Add("Дата", typeof(string));
            expTable.Columns.Add("Административна област", typeof(string));
            expTable.Columns.Add("Потребител", typeof(string));

            foreach (var item in list.Results)
            {
                SystemLogEvent logevent = item;
                expTable.Rows.Add(logevent.InfoMessage, EnumUtils.GetDisplayName(logevent.Type), logevent.EventTime.ToString(), (logevent.AdministrativeBody != null) ? logevent.AdministrativeBody.Name : string.Empty , (logevent.CurrentUser != null) ? logevent.CurrentUser.Name : null);
            }

            var grid = new GridView();
            grid.DataSource = expTable;
            grid.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=ExportData.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

        }
    }
}