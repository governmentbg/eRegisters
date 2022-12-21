using FluentNHibernate.Mapping;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess.Mappings
{
    public class RegisterStateMap : ClassMap<RegisterState>
    {
        public RegisterStateMap()
        {
            Table("REGISTER_STATES");
            Cache.ReadWrite();
            Id(x => x.Id).GeneratedBy.Native();
            Map(x => x.Name).Column("NAME");        
            Map(x => x.InsDateTime).Column("INSDATETIME");
            Map(x => x.InitialState).Column("IS_INITIAL_STATE");
            
            References(x => x.CreatedBy).Column("CREATED_BY");
            References(x => x.Register).Column("REGISTER_ID");
            
            HasMany(x => x.AttributeList).KeyColumn("REGISTER_STATE_ID").Inverse().Cascade.All().Cache.ReadWrite();

            HasManyToMany(x => x.HeadList)
            .Table("REGISTER_STATES_HEAD")
            .ParentKeyColumn("STATE_ID")
            .ChildKeyColumn("REGISTER_HEAD_ID")
            .Cascade.SaveUpdate();

        }
    }
}
