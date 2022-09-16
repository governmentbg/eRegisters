using FluentNHibernate.Mapping;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess.Mappings
{
    public class AreaGroupMap : ClassMap<AreaGroup>
    {
        public AreaGroupMap()
        {
            Table("AREA_GROUPS");
            Cache.ReadWrite();
            Id(x => x.Id).GeneratedBy.Native();
            Map(x => x.Name).Column("NAME");
            Map(x => x.Description).Column("DESCRIPTION");
            Map(x => x.InsDateTime).Column("INSDATETIME");
            References(x => x.CreatedBy).Column("CREATEDBY");
            References(x => x.ChangedBy).Column("CHANGEDBY");
            HasMany(x => x.AreaList).KeyColumn("AREA_GROUP_ID").Inverse().Cascade.All().Cache.ReadWrite();
        }
    }
}
