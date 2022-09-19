using FluentNHibernate.Mapping;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess.Mappings
{
    public class UserIdentificatorTypeMap : ClassMap<UserIdentificatorType>
    {
        public UserIdentificatorTypeMap() {
            Table("USER_IDENTIFICATOR_TYPES");
            Cache.ReadWrite();
            Id(x=>x.Id).GeneratedBy.Native();
            Map(x => x.IdentificatorType).Column("IDENTIFICATOR_TYPE");
            Map(x => x.Name);
        }
    }
}
