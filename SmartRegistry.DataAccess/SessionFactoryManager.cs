using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Dialect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess
{
    public class SessionFactoryManager
    {
        public static SessionFactoryManager Instance = new SessionFactoryManager();

        // SessionFactory should be used only in DAL Project
        internal ISessionFactory SessionFactory { get; private set; }

        public static void Initialize(string connectionString)
        {
            var configuration = new NHibernate.Cfg.Configuration();
            configuration.DataBaseIntegration(db =>
               {
                   db.Dialect<HanaColumnStoreDialect>();
                   db.ConnectionString = connectionString;
               });

            Instance.SessionFactory = Fluently
              .Configure(configuration)
              .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UsersDao>())
              .Cache(x => x.UseSecondLevelCache()
                    .ProviderClass<NHibernate.Caches.SysCache2.SysCacheProvider>()
                    .UseQueryCache()
              )
              .BuildSessionFactory();

        }
    }
}
