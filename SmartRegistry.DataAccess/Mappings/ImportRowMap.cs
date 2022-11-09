using FluentNHibernate.Mapping;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess.Mappings
{
    public class ImportRowMap : ClassMap<ImportRow>
    {
        public ImportRowMap()
        {
            Table("IMPORT_ROWS");
            Cache.ReadWrite();
            Id(x => x.Id).GeneratedBy.Native();
            References(x => x.Head).Column("HEAD_ID");
            Map(x => x.RowNumber).Column("ROW_NUMBER");
            Map(x => x.Uri).Column("URI");
            Map(x => x.ExternalId).Column("EXTERNAL_ID");
            Map(x => x.InsDateTime).Column("INS_DATE_TIME");
            References(x => x.CreatedBy).Column("CREATED_BY");
            Map(x => x.Status).Column("STATUS").CustomType<ImportStatus>();

            HasMany(x => x.Values).KeyColumn("ROW_ID").Inverse().Cascade.All();
        }
    }
}
