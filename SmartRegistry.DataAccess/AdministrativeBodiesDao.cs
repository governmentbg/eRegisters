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
    public class AdministrativeBodiesDao : BaseDao<AdministrativeBody,long>, IAdministrativeBodiesDao
    {

        public AdministrativeBodiesDao(NHibernateDbContext dbContext) 
            : base(dbContext)
        {

        }


        public IList<AdministrativeBody> GetAdministrativeBodies()
        {
            var result = Session.Query<AdministrativeBody>().ToList(); 
            return result;
        }

    

    }
}
