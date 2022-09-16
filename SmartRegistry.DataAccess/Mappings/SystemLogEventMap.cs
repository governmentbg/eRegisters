using FluentNHibernate.Mapping;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess.Mappings
{
    class SystemLogEventMap : ClassMap<SystemLogEvent>
    {
        public SystemLogEventMap()
        {
            Table("SYSTEM_LOG");
            Cache.ReadWrite();
            Id(x => x.Id).GeneratedBy.Native();       
            References(x => x.CurrentUser).Column("USER_ID");
            Map(x => x.EventTime).Column("EVENT_TIME");
            Map(x => x.ObjectId).Column("OBJECT_ID");
            Map(x => x.InfoMessage).Column("INFO_MESSAGE");
            Map(x => x.Type).Column("EVENT_TYPE").CustomType<SystemLogEventTypeEnum>();
            References(x => x.AdministrativeBody).Column("ADMINISTRATIVE_BODY_ID");

        }


    }
}
