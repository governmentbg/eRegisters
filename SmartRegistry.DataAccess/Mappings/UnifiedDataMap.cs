using FluentNHibernate.Mapping;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess.Mappings
{
    public class UnifiedDataMap : ClassMap<UnifiedData>
    {
        public UnifiedDataMap()
        {
            Table("UNIFIED_DATA");
            Cache.ReadWrite();
            Id(x => x.Id).GeneratedBy.Native();
            Map(x => x.Name).Column("NAME");
            Map(x => x.Description).Column("DESCRIPTION");
            Map(x => x.URI).Column("URI");
            Map(x => x.HasMultipleValues).Column("HAS_MULTIPLE_VALUES");
            Map(x => x.DataType).Column("DATA_TYPE").CustomType<UnifiedDataTypeEnum>();
            Map(x => x.IsActive).Column("IS_ACTIVE");
            References(x => x.ModifiedBy).Column("MODIFIED_BY");
            HasMany(x => x.CompositeList).KeyColumn("UNIFIED_DATA_ID").Inverse().Cascade.All().Cache.ReadWrite();
            Map(x => x.NamespaceApi).Column("NAMESPACE_API");
            References(x => x.ReferentialRegister).Column("REF_REGISTER_ID");
            References(x => x.ReferentialAttribute).Column("REF_ATTRIBUTE_ID");
        }
    }
}
