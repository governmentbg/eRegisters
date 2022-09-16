using NHibernate;
using NHibernate.Criterion;
using Orak.Utils;
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
    public class UnifiedDataDao : BaseDao<UnifiedData, int>, IUnifiedDataDao
    {
        public UnifiedDataDao(NHibernateDbContext dbContext)
            : base(dbContext)
        {

        }

        protected ICriteria ApplyUnifiedDataFilterToCriteria(ICriteria criteria, UnifiedDataFilter filter)
        {
            ApplyStringFilterToCriteria(criteria, "Name", filter.Name);
            ApplyStringFilterToCriteria(criteria, "URI", filter.URI);

            return criteria;
        }

        private ICriteria CreateUniDataCriteria(UnifiedDataFilter filter)
        {
            var criteria = Session.CreateCriteria<UnifiedData>();
            if (filter != null)
            {
                criteria = ApplyUnifiedDataFilterToCriteria(criteria, filter);
            }

            if (filter.IsActive != null)
            {
                criteria.Add(Restrictions.Eq("IsActive", filter.IsActive));
            }
            if (filter.NamespaceApi != null) {
                criteria.Add(Restrictions.Eq("NamespaceApi", filter.NamespaceApi));
            }


            if (filter.OrderByColumn == "DataTypeName")
            {
                var proj = Projections.Constant(string.Empty);
                var dataTypesOrdDict = EnumUtils.GetValues<UnifiedDataTypeEnum>();

                foreach (var dt in dataTypesOrdDict)
                {
                    proj = Projections.Conditional(
                        Restrictions.Eq("DataType", dt),
                        Projections.Constant(EnumUtils.GetDisplayName(dt)),
                        proj
                        );
                }

                if (filter.OrderByDirection == OrderDbEnum.Asc)
                {
                    criteria.AddOrder(Order.Asc(proj));
                }
                else
                {
                    criteria.AddOrder(Order.Desc(proj));
                }
            }
            else
            {
                ApplyOrderToCriteria(criteria, filter);
            }

            return criteria;
        }

        public PagedResult<UnifiedData> GetAll(UnifiedDataFilter filter)
        {
            var criteria = CreateUniDataCriteria(filter);

            var result = GetPagedResultForCriteria(criteria, filter);

            return result;
        }

        public IList<UnifiedData> GetAllList(UnifiedDataFilter filter)
        {
            var criteria = CreateUniDataCriteria(filter);

            return criteria.List<UnifiedData>();
        }

        public int GetCount()
        {
            var criteria = Session.CreateCriteria<UnifiedData>();
            var countCriteria = CriteriaTransformer.TransformToRowCount(criteria);
            var count = countCriteria.UniqueResult<int>();
            return count;
        }
    }
}
