using FluentNHibernate.Mapping;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess.Mappings
{
    public class RegisterRightMap : ClassMap<RegisterRight>
    {
        public RegisterRightMap()
        {
            Table("REGISTER_RIGHTS");
            Cache.ReadWrite();
            Id(x => x.Id).GeneratedBy.Native();
            References(x => x.Register).Column("REGISTER_ID");
            References(x => x.UserGroup).Column("USER_GROUP_ID");
            Map(x => x.Permission).Column("PERMISSION_ID").CustomType<RegisterPermissionEnum>();
            Map(x => x.HasRight).Column("HAS_RIGHT");
        }
    }
}
