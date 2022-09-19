using FluentNHibernate.Mapping;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess.Mappings
{
    public class WebServiceResponseAttributeMap : ClassMap<WebServiceResponseAttribute>
    {
        public WebServiceResponseAttributeMap()
        {
            Table("WEB_SERVICES_RESPONSE_ATTRIBUTES");
            Cache.ReadWrite();
            Id(x => x.Id).GeneratedBy.Native();
            References(x => x.WebService).Column("WEB_SERVICE_ID");
            References(x => x.Attribute).Column("REGISTER_ATTRIBUTE_ID");
        }
    }
}
