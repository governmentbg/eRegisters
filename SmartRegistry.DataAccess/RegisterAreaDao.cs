using SmartRegistry.Domain.Entities;
using SmartRegistry.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess
{
    public class RegisterAreaDao : BaseDao<RegisterArea, int>, IRegisterAreaDao
    {
        public RegisterAreaDao(NHibernateDbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
