using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace Semantika.EuropeanaInside.Eck.PidGenerator
{
    [ServiceContract]
    public interface IPidGenerationService
    {
        [OperationContract] ///{InstitutionUrl}/{RecordType}/{AccessionNumber}
        [WebInvoke(UriTemplate = "Generate", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle=WebMessageBodyStyle.Bare)]
        string Generate(PidData pd);

        [OperationContract] // institutionUrl=<the url>&recordType=<the type of record>&accessionNumber
        [WebInvoke(UriTemplate = "Generate?institutionUrl={institutionUrl}&recordType={recordType}&accessionNumber={accessionNumber}", Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        string Generate2(string institutionUrl, string recordType, string accessionNumber);
 
        [OperationContract]
        [WebInvoke(UriTemplate = "Lookup/{pid}", Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        PidData Lookup(string pid);
    }
}
