using Orak.Utils;
using Orak.Utils.Data;
using SmartRegistry.Domain.Entities;
using SmartRegistry.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess
{
    public class RegistrySettingsDao : BaseDao<RegistrySetting,RegistrySettingCompositeId>, IRegistrySettingsDao
    {
        public RegistrySettingsDao(NHibernateDbContext dbContext) 
            : base(dbContext)
        {

        }

        public RegistrySetting Get(string name, string owner)
        {
            //Session.Get<RegistrySetting>()

            return null;
        }
    }
}
