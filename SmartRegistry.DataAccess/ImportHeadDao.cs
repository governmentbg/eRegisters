using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
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
    public class ImportHeadDao : BaseDao<ImportHead, int>, IImportHeadDao
    {
        public ImportHeadDao(NHibernateDbContext dbContext)
            : base(dbContext)
        {

        }

        public void ClearHeadData(ImportHead importHead)
        {
            Session.Query<ImportData>()
                .Where(x => x.Head == importHead)
                .Delete();

            Session.Query<ImportRow>()
                .Where(x => x.Head == importHead)
                .Delete();

            Session.Query<ImportColumn>()
                .Where(x => x.Head == importHead)
                .Delete();
        }

        public PagedResult<ImportHead> GetAll(ImportHeadFilter filter)
        {
            var criteria = Session.CreateCriteria<ImportHead>();
            if (filter != null)
            {
                criteria = ApplyImportHeadFilterToCriteria(criteria, filter);
            }

            ApplyOrderToCriteria(criteria, filter);

            var result = GetPagedResultForCriteria(criteria, filter);

            return result;
        }

        private ICriteria ApplyImportHeadFilterToCriteria(ICriteria criteria, ImportHeadFilter filter)
        {
            if (filter.Register != null)
            {
                criteria.Add(Restrictions.Eq("Register", filter.Register));
            }

            if (filter.AdminBody != null)
            {
                criteria.CreateAlias("Register", "register")
                    .Add(Restrictions.Eq("register.AdministrativeBody", filter.AdminBody));
            }

            if (!filter.ShowProcessed)
            {
                criteria.Add(Restrictions.Not(Restrictions.Eq("Status", ImportHeadStatus.Processed)));
            }

            if (filter.Status != null)
            {
                criteria.Add(Restrictions.Eq("Status",filter.Status));
            }

            return criteria;
        }
    }
}
