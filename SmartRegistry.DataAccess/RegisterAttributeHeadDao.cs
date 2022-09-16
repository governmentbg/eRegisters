using SmartRegistry.Domain.Entities;
using SmartRegistry.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess
{
    public class RegisterAttributeHeadDao : BaseDao<RegisterAttributesHead, int>, IRegisterAttributeHeadDao
    {
        public RegisterAttributeHeadDao(NHibernateDbContext dbContext)
            : base(dbContext)
        {

        }

        public RegisterAttributesHead GetCurrentHeadForRegister(int registerId)
        {
            var result = Session.Query<RegisterAttributesHead>()
                .Where(x => (x.Register.Id == registerId) && (x.Status == RegisterAttributeHeadStatusEnum.ActiveVersion))
                .FirstOrDefault();

            return result;
        }
    }
}
