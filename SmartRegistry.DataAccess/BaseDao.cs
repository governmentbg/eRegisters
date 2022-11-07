using NHibernate;
using NHibernate.Criterion;
using Orak.Utils.Data;
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
    public class BaseDao<EntityType, IdType>
    {
        protected ISqlManager SqlMan { get; private set; }

        protected NHibernateDbContext DbContext { get; private set; }

        protected ISession Session
        {
            get
            {
                return DbContext.Session;
            }
        }
        protected PagedResult<EntityType> GetPagedResultForCriteria(ICriteria criteria,
        QueryFilterBase filter)
        {
            int? pageSize = null;
            int? currentPage = null;

            if ((filter.PageNumber != null) && (filter.PageSize != null))
            {
                pageSize = filter.PageSize;
                currentPage = filter.PageNumber;               
            }

            return GetPagedResultForCriteria(criteria, currentPage, pageSize);
        }

        protected PagedResult<EntityType> GetPagedResultForCriteria(ICriteria criteria,
        int? page, int? pageSize)
        {
            var result = new PagedResult<EntityType>();

            var countCriteria = CriteriaTransformer.TransformToRowCount(criteria);
            if ((pageSize != null) && (pageSize != 0))
            {
               criteria.SetMaxResults((int)pageSize)
                        .SetFirstResult(((int)page - 1) * (int)pageSize);               
            }

            var cntFuture = countCriteria.FutureValue<int>();
            var listFuture = criteria.Future<EntityType>();
          
            result.CurrentPage = (page == null) ? 1 : (int)page;
            result.PageSize = (pageSize == null) ? 0 : (int)pageSize;
            result.RowCount = cntFuture.Value;
            var pageCount = (double)result.RowCount / result.PageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);
            result.Results = listFuture.ToList<EntityType>();
            return result;
        }

        protected virtual IQuery ApplyFilterToQuery(IQuery query, QueryFilterBase fitler)
        {
            // TODO apply paging
            return query;
        }

        public BaseDao(NHibernateDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public virtual EntityType GetById(IdType id)
        {
            return Session.Load<EntityType>(id);
        }

        public virtual void Save(EntityType entity)
        {
            Session.SaveOrUpdate(entity);
        }

        public virtual void Update(EntityType entity)
        {
            Session.Update(entity);
        }

        public virtual void Delete(EntityType entity)
        {
            Session.Delete(entity);
            Session.Flush();
        }

        protected void ApplyOrderToCriteria(ICriteria criteria, QueryFilterBase filter)
        {
            if (!string.IsNullOrEmpty(filter.OrderByColumn))
            {
                Order order;
                if (filter.OrderByDirection == OrderDbEnum.Asc)
                {
                    order = Order.Asc(filter.OrderByColumn);
                }
                else
                {
                    order = Order.Desc(filter.OrderByColumn);
                }
                criteria.AddOrder(order);
            }
        }
        protected void ApplyFilterToDetachedCriteria(DetachedCriteria criteria, QueryFilterBase filter)
        {
            if (!string.IsNullOrEmpty(filter.OrderByColumn))
            {
                Order order;
                if (filter.OrderByDirection == OrderDbEnum.Asc)
                {
                    order = Order.Asc(filter.OrderByColumn);
                }
                else
                {
                    order = Order.Desc(filter.OrderByColumn);
                }
                criteria.AddOrder(order);
            }

            if ((filter.PageSize != null) && (filter.PageSize != 0))
            {
                criteria.SetMaxResults((int)filter.PageSize)
                         .SetFirstResult(((int)filter.PageNumber - 1) * (int)filter.PageSize);
            }

        }

        protected void ApplyStringFilterToDetachedCriteria(DetachedCriteria criteria, string filterName, string filterValue)
        {
            if (!string.IsNullOrEmpty(filterValue))
            {
                if (filterValue.StartsWith("%") || filterValue.EndsWith("%"))
                {
                    criteria.Add(Restrictions.Like(filterName, filterValue));
                }
                else
                {
                    criteria.Add(Restrictions.Eq(filterName, filterValue));
                }
            }
        }

        protected void ApplyStringFilterToCriteria(ICriteria criteria, string filterName, string filterValue)
        {
            if (!string.IsNullOrEmpty(filterValue))
            {
                if (filterValue.StartsWith("%") || filterValue.EndsWith("%"))
                {
                    criteria.Add(Restrictions.Like(filterName, filterValue));
                }
                else
                {
                    criteria.Add(Restrictions.Eq(filterName, filterValue));
                }
            }
        }
    }
}
