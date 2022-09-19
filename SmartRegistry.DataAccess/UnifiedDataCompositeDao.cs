using NHibernate;
using NHibernate.Criterion;
using SmartRegistry.Domain.Common;
using SmartRegistry.Domain.Entities;
using SmartRegistry.Domain.Interfaces;
using SmartRegistry.Domain.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess
{
    public class UnifiedDataCompositeDao : BaseDao<UnifiedDataComposite, int>, IUnifiedDataCompositeDao
    {
        public UnifiedDataCompositeDao(NHibernateDbContext dbContext)
            : base(dbContext)
        {

        }

    }
}
