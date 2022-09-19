using SmartRegistry.Domain.Entities;
using SmartRegistry.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess
{
    public class RegisterPermissionsDao : BaseDao<RegisterPermission, RegisterPermissionEnum>, IRegisterPermissionsDao
    {
        public RegisterPermissionsDao(NHibernateDbContext dbContext)
            : base(dbContext)
        {

        }

        public IList<RegisterPermission> GetAll()
        {
            var result = Session.Query<RegisterPermission>()
                .OrderBy(x => x.Id)
                .ToList<RegisterPermission>();

            return result;
        }
    }
}
