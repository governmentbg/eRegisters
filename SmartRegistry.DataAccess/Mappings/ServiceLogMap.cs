using FluentNHibernate.Mapping;
using SmartRegistry.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRegistry.DataAccess.Mappings
{
    class ServiceLogMap : ClassMap<ServiceLog>
    {
        public ServiceLogMap()
        {
            Table("SERVICE_LOG");
            Cache.ReadWrite();
            Id(x => x.Id).GeneratedBy.Native(); 
            Map(x => x.ServiceUri).Column("SERVICE_URI");
            Map(x => x.ServiceType).Column("SERVICE_TYPE");
            Map(x => x.InsDateTime).Column("INS_DATE_TIME");
            Map(x => x.EmployeeNames).Column("EMPLOYEE_NAMES");
            Map(x => x.EmployeeIdentificator).Column("EMPLOYEE_IDENTIFIER");
           
        }


    }
}
