using RegiX.MmsSportOrganizationsAdapter.AdapterService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnoLogica.RegiX.Adapters.Common;
using TechnoLogica.RegiX.Common.TransportObjects;

namespace RegiX.MmsSportOrganizationsAdapter.APIService
{
    public class MmsSportOrganizationsAPI : BaseAPIService<MmsSportOrganizationsAPI>, IMmsSportOrganizationsAPI
    {
        public ServiceResultDataSigned<SportOrganizationsRequestType, SportOrganizationsReportResponse> GetChildStudentStatus(ServiceRequestData<SportOrganizationsRequestType> argument)
        {
            return AdapterClient.Execute<IMmsSportOrganizationsAdapter, SportOrganizationsRequestType, SportOrganizationsReportResponse>(
                (i, r, a, o) => i.GetSportOrganizations(r, a, o),
                 argument);
        }
    }
}
