using FluentNHibernate.Mapping;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess.Mappings
{
    public class UserAdministrativeBodyMap : ClassMap<UserAdministrativeBody>
    {
        public UserAdministrativeBodyMap()
        {
            Table("USER_ADMINISTRATIVE_BODIES");
            Cache.ReadWrite();
            Id(x => x.Id).GeneratedBy.Native();
            References(x => x.User).Column("USER_ID");
            References(x => x.AdministrativeBody).Column("ADMINISTRATIVE_BODY_ID");
        }
    }
}
