using FluentNHibernate.Mapping;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess.Mappings
{
    public class UnifiedDataCompositeMap : ClassMap<UnifiedDataComposite>
    {
        public UnifiedDataCompositeMap()
        {
            Table("UNIFIED_DATA_COMPOSITION_CONTENTS");
            Cache.ReadWrite();
            Id(x => x.Id).GeneratedBy.Native();
            References(x => x.MainUnifiedData).Column("UNIFIED_DATA_ID");
            References(x => x.ContainedUnifiedData).Column("CONTAINS_DATA_ID");
            Map(x => x.OrderCol).Column("ORD_COL");
        }
    }
}
