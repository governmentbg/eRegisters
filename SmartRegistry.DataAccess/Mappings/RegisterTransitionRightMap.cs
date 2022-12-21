using FluentNHibernate.Mapping;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess.Mappings
{
    public class RegisterTransitionRightMap : ClassMap<RegisterTransitionRight>
    {
        public RegisterTransitionRightMap()
        {
            Table("REGISTER_TRANSITIONS_RIGHTS");
            Cache.ReadWrite();
            Id(x => x.Id).GeneratedBy.Native();         

            References(x => x.RegisterTransition).Column("REGISTER_TRANS_ID");
            References(x => x.UserGroup).Column("USER_GROUP_ID");
            
        }
    }
}
