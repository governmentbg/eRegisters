using FluentNHibernate;
using FluentNHibernate.Mapping;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess.Mappings
{
    public class RegistrySettingMap : ClassMap<RegistrySetting>
    {
        public RegistrySettingMap()
        {
            Table("REGISTRY_SETTINGS");
            CompositeId(x => x.Id)
                .KeyProperty(x => x.Name)
                .KeyProperty(x => x.Owner);
            Map(Reveal.Member<RegistrySetting>("DataValue")).Column("DATA_VALUE");
            Map(Reveal.Member<RegistrySetting>("DataType")).Column("DATA_TYPE");

        }
    }
}
