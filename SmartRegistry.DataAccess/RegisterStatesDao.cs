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
    public class RegisterStatesDao : BaseDao<RegisterState, int> , IRegisterStatesDao
    {
        public RegisterStatesDao(NHibernateDbContext dbContext)
            : base(dbContext)
        {

        }

        public PagedResult<RegisterState> GetAll(RegisterStatesFilter filter)
        {
            var criteria = Session.CreateCriteria<RegisterState>();
            if (filter != null)
            {
                criteria = ApplyFilterToCriteria(criteria, filter);
            }
            var result = GetPagedResultForCriteria(criteria, filter);

            return result;
        }

        protected ICriteria ApplyFilterToCriteria(ICriteria criteria, RegisterStatesFilter filter)
        {
            if (filter.RegisterAttributesHead != null) {
                criteria.Add(Restrictions.Eq("Register", filter.RegisterAttributesHead.Register));
            }


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

            return criteria;
        }


        public IList<RegisterState> GetInitialStatesForRegister(RegisterAttributesHead regHead)
        {
            var result = Session.Query<RegisterState>()
             //   .Where(x=>x.RegisterHead == regHead)
                .Where(x=>x.InitialState==1).ToList();
            return result;
        }

     

    }
}
