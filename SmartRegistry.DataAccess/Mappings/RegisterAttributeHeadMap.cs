using FluentNHibernate.Mapping;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess.Mappings
{
    public class RegisterAttributeHeadMap : ClassMap<RegisterAttributesHead>
    {
        public RegisterAttributeHeadMap()
        {
            Table("REGISTER_ATTRIBUTES_HEAD");
            Cache.ReadWrite();
            Id(x => x.Id).GeneratedBy.Native();
            References(x => x.Register).Column("REGISTER_ID");
            Map(x => x.VersionNumber).Column("VERSION_NUM");
            Map(x => x.Status).Column("STATUS").CustomType<RegisterAttributeHeadStatusEnum>();
            Map(x => x.ActivatedOn).Column("ACTIVATED_ON");
            Map(x => x.DeactivatedOn).Column("DEACTIVATED_ON");
            References(x => x.ActivatedBy).Column("ACTIVATED_BY");
            References(x => x.DeactivatedBy).Column("DEACTIVATED_BY");
            HasManyToMany(x => x.Attributes)
                .Table("REGISTER_ATTRIBUTES_ATTR_HEAD")
                .ParentKeyColumn("HEAD_ID")
                .ChildKeyColumn("ATTRIBUTE_ID")
                .Cascade.SaveUpdate();

            HasManyToMany(x => x.RegisterStatesList)
              .Table("REGISTER_STATES_HEAD")
              .ParentKeyColumn("REGISTER_HEAD_ID")
              .ChildKeyColumn("STATE_ID")
              .Cascade.SaveUpdate();

            HasManyToMany(x => x.RegisterTransitions)
          .Table("REGISTER_STATES_TRANSITIONS_HEAD")
          .ParentKeyColumn("REGISTER_HEAD_ID")
          .ChildKeyColumn("TRANSITION_ID")
          .Cascade.SaveUpdate();
        }
    }
}
