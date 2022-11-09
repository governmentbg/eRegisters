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
    public class WebServicesClientsDao : BaseDao<WebServiceClient,int>, IWebServicesClientsDao
    {
        public WebServicesClientsDao(NHibernateDbContext dbContext)
            : base(dbContext)
        {

        }

        public PagedResult<WebServiceClient> GetAll(WebServiceClientFilter filter)
        {
            var criteria = Session.CreateCriteria<WebServiceClient>();
            if (filter != null)
            {
                criteria = ApplyWebServiceFilterToCriteria(criteria, filter);
            }

            var result = GetPagedResultForCriteria(criteria, filter);

            return result;
        }

        public WebServiceClient GetByURIandThumbprint(string uri,string thumbprint)
        {
            WebServiceClientFilter filter = new WebServiceClientFilter();
            filter.ClientUrl = uri;
            filter.CertThumbprint = thumbprint;
            var result = GetAll(filter).Results.FirstOrDefault();
            return result;

        }

        private ICriteria ApplyWebServiceFilterToCriteria(ICriteria criteria, WebServiceClientFilter filter)
        {
            ApplyStringFilterToCriteria(criteria, "Name", filter.Name);
            ApplyStringFilterToCriteria(criteria, "ClientUrl", filter.ClientUrl);
            ApplyStringFilterToCriteria(criteria, "CertThumbprint", filter.CertThumbprint);



            return criteria;
        }
    }
}
