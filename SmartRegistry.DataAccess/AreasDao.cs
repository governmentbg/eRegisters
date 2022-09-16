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
    public class AreasDao : BaseDao<Area, int> , IAreasDao
    {
        public AreasDao(NHibernateDbContext dbContext)
            : base(dbContext)
        {

        }

        public IList<Area> GetAll()
        {
            var result = Session.Query<Area>().ToList();
            return result;
        }

        public PagedResult<Area> GetAll(AreaGroupFilter filter)
        {
            var criteria = Session.CreateCriteria<Area>();
            if (filter != null)
            {
                criteria = ApplyFilterToCriteria(criteria, filter);
            }
            var result = GetPagedResultForCriteria(criteria, filter);

            return result;
        }

        protected ICriteria ApplyFilterToCriteria(ICriteria criteria, AreaGroupFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Name))
            {
                if (filter.Name.StartsWith("%") || filter.Name.EndsWith("%"))
                {
                     criteria.Add(Restrictions.Like("Name", filter.Name));
                  
                }
                else
                {
                      criteria.Add(Restrictions.Eq("Name", filter.Name));                 
                }
            }

            if (!string.IsNullOrEmpty(filter.DescFilter))
            {
                if (filter.DescFilter.StartsWith("%") || filter.DescFilter.EndsWith("%"))
                {
                    criteria.Add(Restrictions.Like("Description", filter.DescFilter));

                }
                else
                {
                    criteria.Add(Restrictions.Eq("Description", filter.DescFilter));
                }
            }

            return criteria;
        }
    }
}
