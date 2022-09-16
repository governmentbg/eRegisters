using SmartRegistry.Domain.Common;
using SmartRegistry.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartRegistry.CommonWeb
{
    public class SmartDbContextHttpModule : IHttpModule
    {
        private HttpApplication app;

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            app = context;

            app.EndRequest += ContextEndRequest;
            app.Error += ContextError;
        }

        private void ContextEndRequest(object sender, EventArgs e)
        {
            var dbContext = (IDbContext)HttpContext.Current.Items[SmartRegistryConstants.DbContextRequestKey];
            if (dbContext != null)
            {
                HttpContext.Current.Items.Remove(SmartRegistryConstants.DbContextRequestKey);               
                dbContext.CommitTransaction();
                dbContext.Dispose();
            }
        }

        private void ContextError(object sender, EventArgs e)
        {
            var dbContext = (IDbContext)HttpContext.Current.Items[SmartRegistryConstants.DbContextRequestKey];
            if (dbContext != null)
            {
                HttpContext.Current.Items.Remove(SmartRegistryConstants.DbContextRequestKey);
                dbContext.RollbackTransaction();
                dbContext.Dispose();
            }
        }
    }
}