using FluentNHibernate.Mapping;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess.Mappings
{
    public class ImportColumnMap : ClassMap<ImportColumn>
    {
        public ImportColumnMap()
        {
            Table("IMPORT_COLUMNS");
            Cache.ReadWrite();
            Id(x => x.Id).GeneratedBy.Native();
            References(x => x.Head).Column("HEAD_ID");
            Map(x => x.ColumnName).Column("COLUMN_NAME");
            References(x => x.Attribute).Column("ATTRIBUTE_ID");
            Map(x => x.InsDateTime).Column("INS_DATE_TIME");
            References(x => x.CreatedBy).Column("CREATED_BY");
        }
    }
}
