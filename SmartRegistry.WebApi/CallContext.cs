using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SmartRegistry.WebApi
{
    [DataContract]
    public class CallContext
    {
        [DataMember]
        public string ServiceURI { get; set; }

        [DataMember]
        public string ServiceType { get; set; }

        [DataMember]
        public string EmployeeIdentifier { get; set; }

        [DataMember]
        public string EmployeeNames { get; set; }

        [DataMember]
        public string EmployeeAdditionalIdentifier { get; set; }

        [DataMember]
        public string EmployeePosition { get; set; }

        public string ResponsiblePersonIdentifier { get; set; }

        [DataMember]
        public string LawReason { get; set; }

        [DataMember]
        public string Remark { get; set; }

        [DataMember]
        public string AdministrationOId { get; set; }

        [DataMember]
        public string AdministrationName { get; set; }

    }
}