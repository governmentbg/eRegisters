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
    public class ImportRowDao : BaseDao<ImportRow, int>, IImportRowDao
    {
        public ImportRowDao(NHibernateDbContext dbContext)
            : base(dbContext)
        {

        }

        public PagedResult<ImportRow> GetAll(ImportRowFilter filter)
        {
            var detachedCrit = DetachedCriteria.For<ImportRow>();

            var criteria = Session.CreateCriteria<ImportRow>("ImpRows");
            if (filter != null)
            {
                criteria = ApplyImportRowFilterToCriteria(criteria, filter);
                detachedCrit = ApplyImportRowFilterToDetachedCriteria(detachedCrit, filter);
            }

            ApplyOrderToCriteria(criteria, filter);
            ApplyFilterToDetachedCriteria(detachedCrit, filter);

            var result = GetPagedResultForCriteria(criteria, filter);

            // Eager load ImportData for returned rows
            detachedCrit.SetProjection(Projections.Property("Id"));

            Session.CreateCriteria<ImportData>()
                .Add(Subqueries.PropertyIn("Row", detachedCrit))
                .List<ImportData>();

            return result;
        }

        private DetachedCriteria ApplyImportRowFilterToDetachedCriteria(DetachedCriteria criteria, ImportRowFilter filter)
        {
            if (filter.Head != null)
            {
                criteria.Add(Restrictions.Eq("Head", filter.Head));
            }

            return criteria;
        }

        private ICriteria ApplyImportRowFilterToCriteria(ICriteria criteria, ImportRowFilter filter)
        {
            if (filter.Head != null)
            {
                criteria.Add(Restrictions.Eq("Head", filter.Head));
            }

            return criteria;
        }

    }
}
