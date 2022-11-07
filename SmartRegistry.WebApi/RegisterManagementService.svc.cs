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
using SmartRegistry.CommonWeb;
using System.Xml.Linq;
using SmartRegistry.WebApi.Helper;
using System.Web;
using log4net;
using System.Security;
using System.IdentityModel.Claims;
using System.ServiceModel.Channels;

namespace SmartRegistry.WebApi
{
    [ServiceBehavior(Namespace = ServiceConstants.ServiceNamespace)]
    public class RegisterManagementService : IRegisterManagementService
    {
        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        DataHelper dataHelper = new DataHelper();


        public ServiceResultISCIPR RegisterRecordEntry(RequestDataISCIPR requestData)
        {         
           

            if (requestData == null)
            {
                return null;
            }

            AuthenticateConnection(requestData);

            ServiceResultISCIPR result = dataHelper.ProccessData(requestData, WebServiceType.CreateRecord);

            _logger.Debug("RegisterRecordEntry RESULT");

            return result;
        }


        public ServiceResultISCIPR RegisterRecordChange(RequestDataISCIPR requestData)
        {
           
            if (requestData == null)
            {
                return null;
            }
            AuthenticateConnection(requestData);

            ServiceResultISCIPR result = dataHelper.ProccessData(requestData, WebServiceType.ChangeRecord);

            return result;
        }

        public ServiceResultISCIPR RegisterRecordRemove(RequestDataISCIPR requestData)
        {
            
            if (requestData == null)
            {
                return null;
            }
            AuthenticateConnection(requestData);

            ServiceResultISCIPR result = dataHelper.DeleteData(requestData, WebServiceType.RemoveRecord);
            return result;
        }

        public void AuthenticateConnection(RequestDataISCIPR requestData) {
            _logger.Debug("AuthenticateConnection");

            if (OperationContext.Current.ServiceSecurityContext.AuthorizationContext.ClaimSets == null) {
                _logger.Error("No claimset service configured wrong");
                throw new SecurityException("No claimset service configured wrong");
            }

            if (OperationContext.Current.ServiceSecurityContext.AuthorizationContext.ClaimSets.Count <= 0) {
                _logger.Error("No claimset service configured wrong");
                throw new SecurityException("No claimset service configured wrong");
            }

            var cert = ((X509CertificateClaimSet)OperationContext.Current.ServiceSecurityContext.
                        AuthorizationContext.ClaimSets[0]).X509Certificate;
            if (cert != null)
            {
                var pkey = cert.PublicKey;
                var subject = cert.Subject;    
                var thumbprint = cert.Thumbprint;

                var webClient = dataHelper.DbContext.WebServicesClientsDao().GetByURIandThumbprint(requestData.CallContext.ServiceURI,thumbprint);
                if (webClient == null) {
                    throw new SecurityException("Wrong certificate");
                }

            }


           
        }

    }
}
