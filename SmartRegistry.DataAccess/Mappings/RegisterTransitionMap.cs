using FluentNHibernate.Mapping;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess.Mappings
{
    public class RegisterTransitionMap : ClassMap<RegisterTransition>
    {
        public RegisterTransitionMap()
        {
            Table("REGISTER_STATES_TRANSITIONS");
            Cache.ReadWrite();
            Id(x => x.Id).GeneratedBy.Native();         

            References(x => x.StartState).Column("START_STATE_ID");
            References(x => x.EndState).Column("END_STATE_ID");
            References(x => x.Register).Column("REGISTER_ID");
            Map(x => x.InsDateTime).Column("INSDATETIME");
            References(x => x.CreatedBy).Column("CREATED_BY");
            HasMany(x => x.RightsList).KeyColumn("REGISTER_TRANS_ID").Inverse().Cascade.All().Cache.ReadWrite();

            HasManyToMany(x => x.HeadList)
            .Table("REGISTER_STATES_TRANSITIONS_HEAD")
            .ParentKeyColumn("TRANSITION_ID")
            .ChildKeyColumn("REGISTER_HEAD_ID")
            .Cascade.SaveUpdate();

        }
    }
}
