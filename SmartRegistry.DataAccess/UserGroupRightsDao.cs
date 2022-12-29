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

namespace SmartRegistry.DataAccess
{
    public class UserGroupRightsDao : BaseDao<UserGroupRight, long>, IUserGroupRightsDao
    {

        public UserGroupRightsDao(NHibernateDbContext dbContext) 
            : base(dbContext)
        {

        }

    }
}
