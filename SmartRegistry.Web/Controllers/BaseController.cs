using SmartRegistry.Domain.Common;
using SmartRegistry.Domain.Interfaces;
using SmartRegistry.Domain.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartRegistry.DataAccess;
using System.IO;
using System.Text;
using SmartRegistry.CommonWeb;
using SmartRegistry.Domain.Entities;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Security;

namespace SmartRegistry.Web.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        private ISmartRegistryContext _smartContext;

        protected string GetApplicationPath()
        {
            var appPath = Request.ApplicationPath;
            if (!appPath.EndsWith("/")) appPath += "/";

            return appPath;
        }

        protected string GetFullApplicationPath()
        {
            var res = Request.Url.Scheme + System.Uri.SchemeDelimiter + 
                Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

            res = res + GetApplicationPath();

            return res;
        }

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


        protected ISmartRegistryContext SmartContext
        {
            get
            {
                if (_smartContext == null)
                {
                    long currentAdminBodyId = 0;
                    int currUserId = 0;
                    if (User.Identity.IsAuthenticated)
                    {
                        var identityId = User.Identity.GetUserId();
                        if (!int.TryParse(identityId, out currUserId)) currUserId = 0;

                        long tmpAdmBodyId = 0;
                        var cookie = Request.Cookies[SmartRegistryConstants.ADMIN_BODY_USER_COOKIE];
                        if (cookie != null)
                        {
                            string cookieVal = cookie.Value;
                            if (!long.TryParse(cookieVal, out tmpAdmBodyId)) tmpAdmBodyId = 0;
                        }

                        currentAdminBodyId = GetValidatedCurrentUserId(currUserId, tmpAdmBodyId);
                    }
                    _smartContext = new SmartRegistryWebContext(DbContext, currUserId, currentAdminBodyId);
                }             
                Session["sm"] = _smartContext;
                TempData["sm"] = _smartContext;

                checkUserAuth();
                

                return _smartContext;
            }
        }

        private void checkUserAuth()
        {           
            int currUserId = 0;
            if (User.Identity.IsAuthenticated)
            {
                var identityId = User.Identity.GetUserId();
                if (!int.TryParse(identityId, out currUserId)) currUserId = 0;
                var userDao = DbContext.GetUsersDao();
                var user = userDao.GetById(currUserId);
                if (user.IsActive == false) {
                    HttpContext.GetOwinContext().Authentication.SignOut();
                }
            }           
        }

        public long GetValidatedCurrentUserId(int currentUserId, long currentAdminBodyId)
        {
            long result = 0;

            var userDao = DbContext.GetUsersDao();
            var user = userDao.GetById(currentUserId);

            if (user != null)
            {
                if (user.IsGlobalAdmin && currentAdminBodyId != 0)
                {
                    return currentAdminBodyId;
                }
                var curUsrAdmBody = user.UserAdministrativeBodies.Where(x => x.AdministrativeBody.Id == currentAdminBodyId).FirstOrDefault();
                if (curUsrAdmBody == null)
                {
                    curUsrAdmBody = user.UserAdministrativeBodies.FirstOrDefault();
                }

                if (curUsrAdmBody != null)
                {
                    result = curUsrAdmBody.AdministrativeBody.Id;
                }
            }

            return result;
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


        protected int? GetIntQueryParam(string paramName)
        {
            int result = 0;
            try
            {
                result = int.Parse(Request.Params["registerId"]);
            }
            catch (Exception ex)
            {
                return null;
            }
            return result;
        }

    }

}
