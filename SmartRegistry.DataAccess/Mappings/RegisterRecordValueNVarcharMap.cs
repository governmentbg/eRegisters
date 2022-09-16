using FluentNHibernate.Mapping;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess.Mappings
{
    public class RegisterRecordValueNVarcharMap : ClassMap<RegisterRecordValueNVarchar>
    {
        public RegisterRecordValueNVarcharMap()
        {
            Table("REGISTER_RECORD_VALUES_NVARCHAR");

            #region Base
            Cache.ReadWrite();
            Id(x => x.Id).GeneratedBy.Native();

            References(x => x.RegisterRecord).Column("REGISTER_RECORD_ID");
            References(x => x.Attribute).Column("REGISTER_ATTRIBUTE_ID");
            Map(x => x.Version).Column("VERSION");
            References(x => x.ModifiedBy).Column("MODIFIED_BY");
            #endregion

            Map(x => x.Value).Column("DATA_VALUE");
        }
    }
}
