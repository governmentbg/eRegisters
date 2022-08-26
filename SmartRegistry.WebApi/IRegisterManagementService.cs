using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SmartRegistry.WebApi
{
    [ServiceContract(Namespace = ServiceConstants.ServiceNamespace)]
    public interface IRegisterManagementService
    {
        [OperationContract]
        ServiceResultISCIPR RegisterRecordEntry(RequestDataISCIPR requestData);

        [OperationContract]
        ServiceResultISCIPR RegisterRecordChange(RequestDataISCIPR requestData);

        [OperationContract]
        ServiceResultISCIPR RegisterRecordRemove(RequestDataISCIPR requestData);
    }
}
