using Owin;
using Microsoft.Owin;
using System;
using SmartRegistry.DataAccess;
using System.Configuration;

[assembly:OwinStartupAttribute(typeof(SmartRegistry.Web.Startup))]
namespace SmartRegistry.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SessionFactoryManager.Initialize(connectionString);
        }
    }
}
