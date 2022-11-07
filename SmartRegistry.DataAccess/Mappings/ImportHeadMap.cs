using FluentNHibernate.Mapping;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess.Mappings
{
    public class ImportHeadMap : ClassMap<ImportHead>
    {
        public ImportHeadMap()
        {
            Table("IMPORT_HEAD");
            Cache.ReadWrite();
            Id(x => x.Id).GeneratedBy.Native();
            References(x => x.Register).Column("REGISTER_ID");
            References(x => x.AttributeHead).Column("ATTRIBUTE_HEAD_ID");
            Map(x => x.UserFileName).Column("USER_FILE_NAME");
            Map(x => x.FileName).Column("FILE_NAME");
            Map(x => x.InsDateTime).Column("INS_DATE_TIME");
            Map(x => x.Status).Column("STATUS").CustomType<ImportHeadStatus>();
            References(x => x.CreatedBy).Column("CREATED_BY");
        }

    }
}
