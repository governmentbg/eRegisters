using FluentNHibernate;
using FluentNHibernate.Mapping;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess.Mappings
{
    public class ImportDataMap : ClassMap<ImportData>
    {
        public ImportDataMap()
        {
            Table("IMPORT_DATA");
            Cache.ReadWrite();
            Id(x => x.Id).GeneratedBy.Native();
            References(x => x.Head).Column("HEAD_ID");
            References(x => x.Row).Column("ROW_ID");
            References(x => x.Column).Column("COLUMN_ID");
            Map(x => x.DataType).Column("DATA_TYPE").CustomType<ImportDataType>();
            Map(Reveal.Member<ImportData>("ValueNVarchar")).Column("VALUE_NVARCHAR");
            Map(Reveal.Member<ImportData>("ValueText")).Column("VALUE_TEXT");
            Map(x => x.Status).Column("STATUS").CustomType<ImportStatus>();
            Map(x => x.ErrorMessage).Column("ERROR_MESSAGE");
            Map(x => x.InsDateTime).Column("INS_DATE_TIME");
            References(x => x.CreatedBy).Column("CREATED_BY");
        }
    }
}
