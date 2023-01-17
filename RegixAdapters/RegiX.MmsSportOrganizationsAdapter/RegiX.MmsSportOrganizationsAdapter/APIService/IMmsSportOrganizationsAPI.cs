using System.ServiceModel;
using System.ComponentModel;
using TechnoLogica.RegiX.Common;
using TechnoLogica.RegiX.Common.TransportObjects;
using TechnoLogica.RegiX.Adapters.Common.Attributes;

namespace RegiX.MmsSportOrganizationsAdapter.APIService
{
    public interface IMmsSportOrganizationsAPI
    {
        [OperationContract]
        [Description("Справка за спортните обединения")]
        [Info(requestXSD: "SportOrganizationsReportRequest.xsd",
            responseXSD: "SportOrganizationsReportResponse.xsd",
            requestXSLT: "SportOrganizationsReportRequest.xslt",
            responseXSLT: "SportOrganizationsReportResponse.xslt")]
        ServiceResultDataSigned<SportOrganizationsRequestType, SportOrganizationsReportResponse> GetChildStudentStatus(ServiceRequestData<SportOrganizationsRequestType> argument);
    }
}
