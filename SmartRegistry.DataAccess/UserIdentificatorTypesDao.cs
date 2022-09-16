using NHibernate.Linq;
using Orak.Utils.Data;
using SmartRegistry.Domain.Entities;
using SmartRegistry.Domain.Common;
using SmartRegistry.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

namespace SmartRegistry.DataAccess
{
    public class UserIdentificatorTypesDao : BaseDao<UserIdentificatorType,long>, IUserIdentificatorTypesDao
    {

        public UserIdentificatorTypesDao(NHibernateDbContext dbContext) 
            : base(dbContext)
        {

        }

        public UserIdentificatorType GetByIdentificatorType(string idType)
        {
            var result = Session.Query<UserIdentificatorType>()
                .WithOptions(o => o.SetCacheable(true).SetCacheRegion(ConstantsDAO.CacheRegion_CONST))
                .Where(x => x.IdentificatorType == idType).FirstOrDefault();
            return result;
        }
    }
}
