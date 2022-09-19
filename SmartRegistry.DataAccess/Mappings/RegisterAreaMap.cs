using FluentNHibernate.Mapping;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess.Mappings
{
    public class RegisterAreaMap : ClassMap<RegisterArea>
    {
        public RegisterAreaMap()
        {
            Table("REGISTER_AREAS");
            Id(x => x.Id).GeneratedBy.Native();
            References(x => x.Register).Column("REGISTER_ID");
            References(x => x.Area).Column("AREA_ID");
            Map(x => x.InsDateTime).Column("INS_DATE_TIME");
            References(x => x.CreatedBy).Column("CREATED_BY");
            References(x => x.ChangedBy).Column("CHANGED_BY");
        }
    }
}
