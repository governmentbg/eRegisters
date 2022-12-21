using FluentNHibernate.Mapping;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess.Mappings
{
    public class RegisterStateAttributeMap : ClassMap<RegisterStateAttribute>
    {
        public RegisterStateAttributeMap()
        {
            Table("REGISTER_STATES_ATTRIBUTES");
            Cache.ReadWrite();
            Id(x => x.Id).GeneratedBy.Native();
            References(x => x.RegisterAttribute).Column("ATTRIBUTE_ID");
            References(x => x.RegisterState).Column("REGISTER_STATE_ID");
        }
    }
}
