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
    public class UserGroupsDao : BaseDao<UserGroup, long>, IUserGroupsDao
    {

        public UserGroupsDao(NHibernateDbContext dbContext)
            : base(dbContext)
        {

        }

        public PagedResult<UserGroup> GetPagedUserGroups(UserGroupFilter userGroupFilter)
        {
            var criteria = CreateUserGroupsCriteria(userGroupFilter);

            var result = GetPagedResultForCriteria(criteria, userGroupFilter);
            return result;
        }

        public IList<UserGroup> GetAllUserGroups(UserGroupFilter userGroupFilter)
        {
            var criteria = CreateUserGroupsCriteria(userGroupFilter);

            var result = criteria
                .List<UserGroup>();
            return result;
        }

        private ICriteria CreateUserGroupsCriteria(UserGroupFilter userGroupFilter)
        {
            var criteria = Session.CreateCriteria<UserGroup>();
            if (!string.IsNullOrEmpty(userGroupFilter.Name))
            {
                ApplyStringFilterToCriteria(criteria, "Name", userGroupFilter.Name);
            }
            if (userGroupFilter.AdminBody != null)
            {
                criteria = criteria.Add(Restrictions.Eq("AdministrativeBody", userGroupFilter.AdminBody));
            }
            if (userGroupFilter.IsActive != null)
            {
                criteria = criteria.Add(Restrictions.Eq("IsActive", userGroupFilter.IsActive));
            }
            if (userGroupFilter.Role != null)
            {
                criteria = criteria.Add(Restrictions.Eq("Role", userGroupFilter.Role));
            }

            return criteria;
        }

    }
}
