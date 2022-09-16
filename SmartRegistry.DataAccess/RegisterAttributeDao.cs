using SmartRegistry.Domain.Entities;
using SmartRegistry.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess
{
    public class RegisterAttributeDao : BaseDao<RegisterAttribute, int>, IRegisterAttributeDao
    {
        public RegisterAttributeDao(NHibernateDbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
