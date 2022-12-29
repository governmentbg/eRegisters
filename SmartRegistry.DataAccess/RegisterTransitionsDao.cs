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
    public class RegisterTransitionsDao : BaseDao<RegisterTransition, int> , IRegisterTransitionsDao
    {
        public RegisterTransitionsDao(NHibernateDbContext dbContext)
            : base(dbContext)
        {

        }

        public Boolean CheckForExistingTransition(int start, int end, int? id)
        {
            var result = Session.Query<RegisterTransition>()
                .Where(x => x.StartState.Id == start)
                .Where(x => x.EndState.Id == end)
                .Where(x => x.Id != id )
                .FirstOrDefault();
            return (result != null);

        }

           

        public PagedResult<RegisterTransition> GetAll(RegisterTransitionFilter filter)
        {
            var criteria = Session.CreateCriteria<RegisterTransition>();
            if (filter != null)
            {
                criteria = ApplyFilterToCriteria(criteria, filter);
            }
            var result = GetPagedResultForCriteria(criteria, filter);

            return result;
        }

        protected ICriteria ApplyFilterToCriteria(ICriteria criteria, RegisterTransitionFilter filter)
        {
            if (filter.RegisterAttributesHead != null) {
                criteria.Add(Restrictions.Eq("Register", filter.RegisterAttributesHead.Register));
            }


            if (!string.IsNullOrEmpty(filter.StartState))
            {
                if (filter.StartState.StartsWith("%") || filter.StartState.EndsWith("%"))
                {
                   var critAdminBodies = criteria.CreateCriteria("StartState");
                   critAdminBodies.Add(Restrictions.Like("Name", filter.StartState));                   
                }
                else
                {
                    var critAdminBodies = criteria.CreateCriteria("StartState");
                    critAdminBodies.Add(Restrictions.Eq("Name", filter.StartState));            
                }
            }

            if (!string.IsNullOrEmpty(filter.EndState))
            {
                if (filter.EndState.StartsWith("%") || filter.EndState.EndsWith("%"))
                {
                    var critAdminBodies = criteria.CreateCriteria("EndState");
                    critAdminBodies.Add(Restrictions.Like("Name", filter.EndState));
                }
                else
                {
                    var critAdminBodies = criteria.CreateCriteria("EndState");
                    critAdminBodies.Add(Restrictions.Eq("Name", filter.EndState));
                }
            }

            return criteria;
        }

        public IList<RegisterTransition> GetEndTransactions(RegisterState regState)
        {
            var result = Session.Query<RegisterTransition>()
                .Where(x=>x.StartState==regState).ToList();
            return result;
        }
    }
}
