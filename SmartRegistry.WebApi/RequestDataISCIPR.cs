using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Xml;

namespace SmartRegistry.WebApi
{
    [DataContract(Namespace = "http://iscipr.egov.bg")]
    public class RequestDataISCIPR
    {
        [DataMember]
        public string Operation { get; set; }

        [DataMember]
        //  public XmlElement Argument { get; set; }
        public string Argument { get; set; }

        [DataMember]
        public CallContext CallContext { get; set; }

        [DataMember]
        public string EmployeeEGN { get; set; }

        [DataMember]
        public string CitizenEGN { get; set; }
    }
}