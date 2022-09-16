using FluentNHibernate.Mapping;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess.Mappings
{
    public class UserGroupRightMap : ClassMap<UserGroupRight>
    {
        public UserGroupRightMap()
        {
            Table("USER_GROUP_RIGHTS");
            Cache.ReadWrite();
            Id(x => x.Id).GeneratedBy.Native();
            References(x => x.UserGroup).Column("USER_GROUP_ID");
            Map(x => x.Permission).Column("PERMISSION_ID").CustomType<PermissionEnum>();
           
        }
    }
}
