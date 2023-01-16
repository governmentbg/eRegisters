using RegiX.MmsSportOrganizationsAdapter.RegisterManagementServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnoLogica.RegiX.Common.DataContracts;
using TechnoLogica.RegiX.Common.DataContracts.Health;
using TechnoLogica.RegiX.Common.DataContracts.Parameter;
using TechnoLogica.RegiX.Common.ObjectMapping;
using TechnoLogica.RegiX.Common.TransportObjects;
using TechnoLogica.RegiX.Common.Utils;
using TechnoLogica.RegiX.WebServiceAdapterService;

namespace RegiX.MmsSportOrganizationsAdapter.AdapterService
{
    public class MmsSportOrganizationsAdapter : SoapServiceBaseAdapterService, IMmsSportOrganizationsAdapter
    {

        public CommonSignedResponse<SportOrganizationsRequestType, SportOrganizationsReportResponse> GetSportOrganizations(SportOrganizationsRequestType argument, AccessMatrix accessMatrix, AdapterAdditionalParameters aditionalParameters)
        {
            Guid id = new Guid();
            try
            {
                RegisterManagementServiceClient serviceClient = new RegisterManagementServiceClient();
                var isciprRequest = new RequestDataISCIPR();
                // TODO convert argument to XML
                isciprRequest.Argument = argument.ToString();
                /*
                 * TODO execute request and map results to SportOrganizationsReportResponse
                var serviceResult = serviceClient.Execute(isciprRequest);
                map results to SportOrganizationsData

                */
                SportOrganizationsReportResponse searchResults = null;
                return
                     SigningUtils.CreateAndSign(
                         argument,
                         searchResults,
                         accessMatrix,
                         aditionalParameters
                     );
            }
            catch (Exception ex)
            {
                LogError(aditionalParameters, ex, new { Guid = id });
                throw ex;
            }
        }
    }
}
