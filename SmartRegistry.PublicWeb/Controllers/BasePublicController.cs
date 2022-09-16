using SmartRegistry.CommonWeb;
using SmartRegistry.DataAccess;
using SmartRegistry.Domain.Common;
using SmartRegistry.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

    }
}