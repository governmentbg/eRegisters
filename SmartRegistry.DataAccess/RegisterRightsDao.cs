using SmartRegistry.Domain.Entities;
using SmartRegistry.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess
{
    public class RegisterRightsDao : BaseDao<RegisterRight, int>, IRegisterRightsDao
    {
        public RegisterRightsDao(NHibernateDbContext dbContext)
            : base(dbContext)
        {

        }

        public IList<RegisterRight> GetAll()
        {
            var result = Session.Query<RegisterRight>().ToList();
            return result;
        }
    }
}
