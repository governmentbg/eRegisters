using FluentNHibernate.Mapping;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess.Mappings
{
    public class UserGroupMap : ClassMap<UserGroup>
    {
        public UserGroupMap()
        {
            Table("USER_GROUPS");
            Cache.ReadWrite();
            Id(x => x.Id).GeneratedBy.Native();
            Map(x => x.Name).Column("NAME");
            Map(x => x.IsActive).Column("IS_ACTIVE");
            Map(x => x.Role).Column("ROLE").CustomType<UserGroupRole>();
            References(x => x.AdministrativeBody).Column("ADMINISTRATIVE_BODY_ID");
            HasMany(x => x.Rights).KeyColumn("USER_GROUP_ID").Inverse().Cascade.All().Cache.ReadWrite();
            HasMany(x => x.RegisterRights).KeyColumn("USER_GROUP_ID").Inverse().Cascade.All().Cache.ReadWrite();
        }
    }
}
