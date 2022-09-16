using FluentNHibernate.Mapping;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess.Mappings
{
    public class WebServiceMap : ClassMap<WebServiceISCIPR>
    {
        public WebServiceMap()
        {
            Table("WEB_SERVICES");
            Cache.ReadWrite();
            Id(x => x.Id).GeneratedBy.Native();
            References(x => x.Register).Column("REGISTER_ID");
            References(x => x.AttributeHead).Column("REGISTER_ATTR_HEAD_ID");
            Map(x => x.ServiceKey).Column("SERVICE_KEY");
            Map(x => x.Name).Column("NAME");
            Map(x => x.Description).Column("DESCRIPTION");
            Map(x => x.ServiceType).Column("SERVICE_TYPE").CustomType<WebServiceType>();
            HasMany(x => x.RequestConditions).KeyColumn("WEB_SERVICE_ID").Inverse().Cascade.All().Cache.ReadWrite();
            HasMany(x => x.ResponseAttributes).KeyColumn("WEB_SERVICE_ID").Inverse().Cascade.All().Cache.ReadWrite();
        }
    }
}
