using FluentNHibernate.Mapping;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess.Mappings
{
    public class RegisterMap : ClassMap<Register>
    {
        public RegisterMap()
        {
            Table("REGISTERS");
            Cache.ReadWrite();
            Id(x => x.Id).GeneratedBy.Native();
            Map(x => x.Name).Column("NAME");
            References(x => x.AdministrativeBody).Column("ADMINISTRATIVE_BODY_ID");
            Map(x => x.IsActive).Column("IS_ACTIVE");
            References(x => x.ModifiedBy).Column("MODIFIED_BY");
            Map(x => x.URI).Column("URI");
            Map(x => x.AdministrativeAct).Column("ADMINISTRATIVE_ACT");
            Map(x => x.LegalBasis).Column("LEGAL_BASIS");
            Map(x => x.UrlAddress).Column("URL_ADDRESS");
            Map(x => x.Description).Column("DESCRIPTION");
            Map(x => x.NamespaceApi).Column("NAMESPACE_API");
            HasMany(x => x.RegisterAreas).KeyColumn("REGISTER_ID").Inverse().Cascade.All().Cache.ReadWrite();
        }
    }
}
