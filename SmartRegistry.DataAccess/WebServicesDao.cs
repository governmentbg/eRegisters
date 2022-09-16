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
    public class WebServicesDao : BaseDao<WebServiceISCIPR,int>, IWebServicesDao
    {
        public WebServicesDao(NHibernateDbContext dbContext)
            : base(dbContext)
        {

        }

        public PagedResult<WebServiceISCIPR> GetAll(WebServiceFilter filter)
        {
            var criteria = Session.CreateCriteria<WebServiceISCIPR>();
            if (filter != null)
            {
                criteria = ApplyWebServiceFilterToCriteria(criteria, filter);
            }

            var result = GetPagedResultForCriteria(criteria, filter);

            return result;
        }

        public WebServiceISCIPR GetByServiceKey(string namespaceService)
        {
            WebServiceFilter filter = new WebServiceFilter();
            filter.ServiceKey = namespaceService;
            var result = GetAll(filter).Results.FirstOrDefault();
            return result;

        }

        private ICriteria ApplyWebServiceFilterToCriteria(ICriteria criteria, WebServiceFilter filter)
        {
            ApplyStringFilterToCriteria(criteria, "Name", filter.Name);
            ApplyStringFilterToCriteria(criteria, "ServiceKey", filter.ServiceKey);

            if (filter.ServiceType != null)
            {
                criteria = criteria.Add(Restrictions.Eq("ServiceType", filter.ServiceType));
            }

            if (filter.RegisterId != null)
            {
                criteria = criteria.Add(Restrictions.Eq("Register.Id", filter.RegisterId));
            }
            if (filter.AttributeId != null)
            {
                criteria = criteria.Add(Restrictions.Eq("AttributeHead.Id", filter.AttributeId));
            }

            return criteria;
        }
    }
}
