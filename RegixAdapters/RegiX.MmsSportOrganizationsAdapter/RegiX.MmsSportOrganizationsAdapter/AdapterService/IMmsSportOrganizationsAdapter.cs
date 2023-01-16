using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TechnoLogica.RegiX.Common;
using TechnoLogica.RegiX.Common.DataContracts;
using TechnoLogica.RegiX.Common.ObjectMapping;

namespace RegiX.MmsSportOrganizationsAdapter.AdapterService
{
    [ServiceContract]
    [Description("Адаптер за комуникация с ММС - Регистър на спортните обединения")]
    public interface IMmsSportOrganizationsAdapter : IAdapterServiceWCF
    {
        [OperationContract]
        [Description("Справка за спортните обединения")]
        CommonSignedResponse<SportOrganizationsRequestType, SportOrganizationsReportResponse> GetSportOrganizations(SportOrganizationsRequestType argument, AccessMatrix accessMatrix, AdapterAdditionalParameters aditionalParameters);
    }
}
