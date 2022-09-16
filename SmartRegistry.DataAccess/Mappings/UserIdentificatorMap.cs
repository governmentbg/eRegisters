using FluentNHibernate.Mapping;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess.Mappings
{
    public class UserIdentificatorMap : ClassMap<UserIdentificator>
    {
        public UserIdentificatorMap() {
            Table("USER_IDENTIFICATORS");
            Cache.ReadWrite();
            Id(x=>x.Id).GeneratedBy.Native();            
            References(x => x.User).Column("USER_ID").Cascade.All();
            Map(x => x.Identificator).Column("IDENTIFICATOR");
            References(x => x.IdentificatorType).Column("IDENTIFICATOR_TYPE_ID").Cascade.All();
        }
    }
}
