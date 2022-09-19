using FluentNHibernate.Mapping;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess.Mappings
{
    public class AdministrativeBodyMap : ClassMap<AdministrativeBody>
    {
        public AdministrativeBodyMap()
        {
            Table("ADMINISTRATIVE_BODIES");
            Cache.ReadWrite();
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Kind);
            Map(x => x.Name);
            Map(x => x.UIC);
            Map(x => x.IsActive).Column("IS_ACTIVE");
            Map(x => x.InsDateTime).Column("INS_DATE_TIME");
            Map(x => x.LastChange).Column("LAST_CHANGED");
            Map(x => x.NamespaceApi).Column("NAMESPACE_API");
        }
    }
}
