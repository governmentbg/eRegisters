using FluentNHibernate.Mapping;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess.Mappings
{
    public class UserMap : ClassMap<User>
    {
        public UserMap() {
            Table("USERS");
            Cache.ReadWrite();
            Id(x => x.Id).GeneratedBy.Native();
            Map(x => x.Name);
            Map(x => x.Email);
            Map(x => x.Phone);
            Map(x => x.ResetCode).Column("RESET_CODE");
            Map(x => x.IsActive).Column("IS_ACTIVE");
            HasMany(x => x.Identificators).KeyColumn("USER_ID").Inverse().Cascade.All().Cache.ReadWrite();
            HasMany(x => x.UserAdministrativeBodies).KeyColumn("USER_ID").Inverse().Cascade.All().Cache.ReadWrite();
            References(x => x.ModifiedBy).Column("MODIFIED_BY");
            HasManyToMany(x => x.UserGroups)
                .Table("USERS_IN_GROUPS")
                .ParentKeyColumn("USER_ID")
                .ChildKeyColumn("USER_GROUP_ID")
                .Cascade.SaveUpdate()
                .Cache.ReadWrite();
        }
    }
}
