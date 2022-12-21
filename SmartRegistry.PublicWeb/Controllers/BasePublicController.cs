using SmartRegistry.CommonWeb;
using SmartRegistry.DataAccess;
using SmartRegistry.Domain.Common;
using SmartRegistry.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace SmartRegistry.PublicWeb.Controllers
{
    public class BasePublicController : Controller
    {

        private IDbContext _dbContext;
        public IDbContext DbContext
        {
            get
            {
                if (_dbContext == null)
                {
                    _dbContext = (NHibernateDbContext)HttpContext.Items[SmartRegistryConstants.DbContextRequestKey];
                }
                if (_dbContext == null)
                {
                    _dbContext = new NHibernateDbContext();
                    HttpContext.Items[SmartRegistryConstants.DbContextRequestKey] = _dbContext;
                }
                return _dbContext;
            }
        }

        private ISmartRegistryContext _smartContext;
        protected ISmartRegistryContext SmartContext
        {
            get
            {
                if (_smartContext == null)
                {
                    long currentAdminBodyId = 0;
                    int currUserId = 0;

                    _smartContext = new SmartRegistryWebContext(DbContext, currUserId, currentAdminBodyId);
                }
                return _smartContext;
            }
        }

        protected string GetRequestData()
        {
            string result = string.Empty;
            using (Stream iStream = Request.InputStream)
            {
                using (StreamReader reader = new StreamReader(iStream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
            }

            return result;
        }
        protected string GetApplicationPath()
        {
            var appPath = Request.ApplicationPath;
            if (!appPath.EndsWith("/")) appPath += "/";

            return appPath;
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {


            HttpCookie languageCookie = System.Web.HttpContext.Current.Request.Cookies["Language"];
            if (languageCookie != null)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(languageCookie.Value);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageCookie.Value);
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("bg-BG");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("bg-BG");
            }


            base.Initialize(requestContext);
        }


        public ActionResult SelectLng(int id)
        {
            CultureInfo uiCultureInfo = Thread.CurrentThread.CurrentUICulture;
            CultureInfo ucultureInfo = Thread.CurrentThread.CurrentCulture;

            if (id == 1)
            {
                Response.Cookies.Remove("Language");
                HttpCookie languageCookie = System.Web.HttpContext.Current.Request.Cookies["Language"];

                var cultureInfo = new CultureInfo("bg-BG");
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);

                if (languageCookie == null) languageCookie = new HttpCookie("Language");
                languageCookie.Value = "bg-BG";
                languageCookie.Expires = DateTime.Now.AddDays(10);
                Response.SetCookie(languageCookie);


            }
            else
            {
                Response.Cookies.Remove("Language");
                HttpCookie languageCookie = System.Web.HttpContext.Current.Request.Cookies["Language"];

                var cultureInfo = new CultureInfo("en-GB");
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);

                if (languageCookie == null) languageCookie = new HttpCookie("Language");
                languageCookie.Value = "en-GB";
                languageCookie.Expires = DateTime.Now.AddDays(10);
                Response.SetCookie(languageCookie);

            }


            return RedirectToAction("Index", "Home");
        }

    }
}