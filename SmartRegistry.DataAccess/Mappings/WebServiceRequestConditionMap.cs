using FluentNHibernate.Mapping;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess.Mappings
{
    public class WebServiceRequestConditionMap : ClassMap<WebServiceRequestCondition>
    {
        public WebServiceRequestConditionMap()
        {
            Table("WEB_SERVICES_REQUEST_CONDITIONS");
            Cache.ReadWrite();
            Id(x => x.Id).GeneratedBy.Native();
            References(x => x.WebService).Column("WEB_SERVICE_ID");
            References(x => x.Attribute).Column("REGISTER_ATTRIBUTE_ID");
            Map(x => x.IsRequired).Column("IS_REQUIRED");
            Map(x => x.IsHidden).Column("IS_HIDDEN");
            Map(x => x.ConditionOperator).Column("CONDITION_OPERATOR").CustomType<ConditionOperator>();
            Map(x => x.DefaultValue).Column("DEFAULT_VALUE");
        }
    }
}
