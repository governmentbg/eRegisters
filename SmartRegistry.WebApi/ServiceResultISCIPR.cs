using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Xml;

namespace SmartRegistry.WebApi
{
    [DataContract]
    public class ServiceResultISCIPR
    {
        [DataMember]
        public XmlElement Data { get; set; }

        [DataMember]
        public bool HasError { get; set; }

        [DataMember]
        public string ErrorCode { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }
    }
}