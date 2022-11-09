using FluentNHibernate.Mapping;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess.Mappings
{
    public class WebServiceClientMap : ClassMap<WebServiceClient>
    {
        public WebServiceClientMap()
        {
            Table("WEB_SERVICE_CLIENTS");
            Cache.ReadWrite();
            Id(x => x.Id).GeneratedBy.Native();
            Map(x => x.ClientUrl).Column("CLIENT_URI");
            Map(x => x.Name).Column("NAME");
            Map(x => x.CertThumbprint).Column("CERT_THUMBPRINT");
        }
    }
}
