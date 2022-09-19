using Microsoft.AspNet.Identity;
using SmartRegistry.CommonWeb;
using SmartRegistry.DataAccess;
using SmartRegistry.Domain.Common;
using SmartRegistry.Domain.Entities;
using SmartRegistry.Domain.Interfaces;
using SmartRegistry.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace SmartRegistry.Web.AuthHelp
{
    public class CustomAuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {


        private IDbContext _dbContext;
        public IDbContext DbContext
        {
            get
            {
                if (_dbContext == null)
                {
                    _dbContext = (NHibernateDbContext)HttpContext.Current.Items[SmartRegistryConstants.DbContextRequestKey];
                }
                if (_dbContext == null)
                {
                    _dbContext = new NHibernateDbContext();
                    HttpContext.Current.Items[SmartRegistryConstants.DbContextRequestKey] = _dbContext;
                }
                return _dbContext;
            }
        }

        ISmartRegistryContext context;

        private readonly PermissionEnum[] allowedroles;

        public CustomAuthorizeAttribute(params PermissionEnum[] roles)
        {
            this.allowedroles = roles;
            context = CreateSmartContext();
          
        }

        private ISmartRegistryContext CreateSmartContext()
        {
            long currentAdminBodyId = 0;
            int currUserId = 0;

            var user = System.Web.HttpContext.Current.User;

            if (user != null) {
                if (user.Identity.IsAuthenticated)
                {
                    var identityId = user.Identity.GetUserId();
                    if (!int.TryParse(identityId, out currUserId)) currUserId = 0;

                    long tmpAdmBodyId = 0;
                    var cookie = HttpContext.Current.Request.Cookies[SmartRegistryConstants.ADMIN_BODY_USER_COOKIE];
                    if (cookie != null)
                    {
                        string cookieVal = cookie.Value;
                        if (!long.TryParse(cookieVal, out tmpAdmBodyId)) tmpAdmBodyId = 0;
                    }

                    currentAdminBodyId = GetValidatedCurrentUserId(currUserId, tmpAdmBodyId);
                }

                context = new SmartRegistryWebContext(DbContext, currUserId, currentAdminBodyId);

                HttpContext.Current.Items[SmartRegistryConstants.SmartContextRequestKey] = context;
               
                return context;
            }

            return null;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;            
            foreach (var role in allowedroles)
            {
                bool hasPermit = context.CurrentUser.HasPermission(role);
                authorize = hasPermit;
            }
            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpStatusCodeResult(403);
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
    }
}