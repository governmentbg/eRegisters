using FluentNHibernate.Mapping;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess.Mappings
{
    public class RegisterAttributeMap : ClassMap<RegisterAttribute>
    {
        public RegisterAttributeMap()
        {
            Table("REGISTER_ATTRIBUTES");
            Id(x => x.Id).GeneratedBy.Native();
            References(x => x.Register).Column("REGISTER_ID");
            Map(x => x.Name).Column("NAME");
            References(x => x.UnifiedData).Column("UNIFIED_DATA_ID");
            Map(x => x.IsPublic).Column("IS_PUBLIC");
            Map(x => x.ExportOpenData).Column("EXPORT_OPEN_DATA");
            Map(x => x.ExportWebServices).Column("EXPORT_WEB_SERVICES");
            Map(x => x.CanFilter).Column("CAN_FILTER");
            References(x => x.ParentAttribute).Column("COMPOSITE_PARENT_ID");
            Map(x => x.ApiName).Column("API_NAME");
            Map(x=>x.IsEncrypted).Column("IS_ENCRYPTED");
        }
    }
}
