using FluentNHibernate.Mapping;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess.Mappings
{
    public class RegisterPermissionMap : ClassMap<RegisterPermission>
    {
        public RegisterPermissionMap()
        {
            Table("REGISTER_PERMISSIONS");
            Id(x => x.Id).CustomType<RegisterPermissionEnum>();
            Map(x => x.Name).Column("NAME");
        }
    }
}
