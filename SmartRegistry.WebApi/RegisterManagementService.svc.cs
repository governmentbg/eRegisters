using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml;
using SmartRegistry.DataAccess;
using SmartRegistry.Domain;
using SmartRegistry.Domain.Entities;
using SmartRegistry.Domain.Interfaces;
using System.Xml.Linq;
using SmartRegistry.WebApi.Helper;
using System.Web;

namespace SmartRegistry.WebApi
{
    [ServiceBehavior(Namespace = ServiceConstants.ServiceNamespace)]
    public class RegisterManagementService : IRegisterManagementService
    {

        DataHelper dataHelper = new DataHelper();
 

        public ServiceResultISCIPR RegisterRecordEntry(RequestDataISCIPR requestData)
        {
            if (requestData == null)
            {
                return null;
            }

            ServiceResultISCIPR result = dataHelper.ProccessData(requestData, WebServiceType.CreateRecord);

            
            return result;
        }


        public ServiceResultISCIPR RegisterRecordChange(RequestDataISCIPR requestData)
        {
            if (requestData == null)
            {
                return null;
            }

            ServiceResultISCIPR result = dataHelper.ProccessData(requestData,WebServiceType.ChangeRecord);

            return result;
        }

        public ServiceResultISCIPR RegisterRecordRemove(RequestDataISCIPR requestData)
        {
            if (requestData == null)
            {
                return null;
            }

            ServiceResultISCIPR result = dataHelper.DeleteData(requestData, WebServiceType.RemoveRecord);
            return result;
        }

    }
}
