using Orak.Utils.Data;
using SmartRegistry.Domain.Entities;
using SmartRegistry.Domain.Common;
using SmartRegistry.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using NHibernate.Criterion;
using SmartRegistry.Domain.QueryFilters;
using NHibernate;

namespace SmartRegistry.DataAccess
{
    public class UsersDao : BaseDao<User,int>, IUsersDao
    {

        public UsersDao(NHibernateDbContext dbContext) 
            : base(dbContext)
        {

        }

        public void DeleteById(int id)
        {
            var result = Session.Query<User>().Where(x=>x.Id==id).FirstOrDefault();
            Session.Delete(result);            
        }

        protected ICriteria ApplyUserFilterToCriteria(ICriteria criteria, UserFilter filter)
        {
            ApplyStringFilterToCriteria(criteria, "Name", filter.Name);

            if (!string.IsNullOrEmpty(filter.Email))
            {
                criteria.Add(Restrictions.Eq("Email", filter.Email));
            }
            if (filter.IsActive != null)
            {
                criteria = criteria.Add(Restrictions.Eq("IsActive", filter.IsActive));
            }
            if (filter.AdminBody != null)
            {
                var critAdminBodies = criteria.CreateCriteria("UserAdministrativeBodies");
                critAdminBodies.Add(Restrictions.Eq("AdministrativeBody", filter.AdminBody));
            }
            if (filter.Identificator != null)
            {
                var critAdminBodies = criteria.CreateCriteria("Identificators");
                critAdminBodies.Add(Restrictions.Eq("Identificator", filter.Identificator));
            }
            if (filter.GroupName != null)
            {
                criteria.CreateAlias("UserGroups", "UsrGrp");               
                criteria.Add(Restrictions.Like("UsrGrp.Name", filter.GroupName));
            }
            return criteria;
        }

        public PagedResult<User> GetAllUsers(UserFilter userFilter)
        {
            var criteria = Session.CreateCriteria<User>();
            if (userFilter != null)
            {
                criteria = ApplyUserFilterToCriteria(criteria, userFilter);
            }

            var result = GetPagedResultForCriteria(criteria, userFilter);

            return result;
        }

        public User CheckForUserByPersonalIdentificator(UserIdentificatorType type,string identificationNumber)
        {
            var result = Session.Query<User>().Where(x => x.Identificators.Any(z => (z.Identificator == identificationNumber) && (z.IdentificatorType == type))).FirstOrDefault();
            return result;
        }
        public User CheckForUserByEGN(string identificationNumber)
        {
            var result = Session.Query<User>().Where(x => x.Identificators.Any(z => (z.Identificator == identificationNumber))).Where(x=>x.IsActive==true).FirstOrDefault();
            return result;
        }

        public User GetByResetCode(string resetCode)
        {
            var result = Session.Query<User>().Where(x=>x.ResetCode.Equals(resetCode)).FirstOrDefault();
            return result;
        }
    }
}
