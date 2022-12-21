using FluentNHibernate.Mapping;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess.Mappings
{
    public class RegisterRecordMap : ClassMap<RegisterRecord>
    {
        public RegisterRecordMap()
        {
            Table("REGISTER_RECORDS");
            Cache.ReadWrite();
            Id(x => x.Id).GeneratedBy.Native();
            References(x => x.Register).Column("REGISTER_ID");
            References(x => x.AttributeHead).Column("ATTRIBUTE_HEAD_ID");
            Map(x => x.Version).Column("VERSION");
            Map(x => x.InsDateTime).Column("INS_DATE_TIME");
            Map(x => x.IsActive).Column("IS_ACTIVE");
            Map(x => x.URI).Column("URI");
            References(x => x.CreatedBy).Column("CREATED_BY");
            References(x => x.ModifiedBy).Column("MODIFIED_BY");
            Map(x => x.ExternalId).Column("EXTERNAL_ID");
            References(x => x.RegisterState).Column("STATE_ID");
            Map(x => x.HashTimestamp).Column("HASH_TIMESTAMP");

            HasMany(x => x.VarcharValues).KeyColumn("REGISTER_RECORD_ID").Inverse().Cascade.All();
            HasMany(x => x.TextValues).KeyColumn("REGISTER_RECORD_ID").Inverse().Cascade.All();
            HasMany(x => x.DateTimeValues).KeyColumn("REGISTER_RECORD_ID").Inverse().Cascade.All();
            HasMany(x => x.IntValues).KeyColumn("REGISTER_RECORD_ID").Inverse().Cascade.All();
            HasMany(x => x.DecimalValues).KeyColumn("REGISTER_RECORD_ID").Inverse().Cascade.All();
        }
    }
}
