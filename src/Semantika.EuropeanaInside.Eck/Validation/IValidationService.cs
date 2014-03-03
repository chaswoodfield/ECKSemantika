using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace Semantika.EuropeanaInside.Eck.Validation
{
    /// <summary>
    /// Validation service inteface
    /// </summary>
    [ServiceContract]
    public interface IValidationService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "profiles", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<string> Profiles();

        [OperationContract]
        [WebInvoke(UriTemplate = "xml/profiles", Method = "GET", RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml)]
        List<string> ProfilesXml();

        [OperationContract]
        [WebInvoke(UriTemplate = "profiles/{name}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string Profile(string name);

        [OperationContract]
        [WebInvoke(UriTemplate = "xml/profiles/{name}", Method = "GET", RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml)]
        string ProfileXml(string name);


        /// <summary>
        /// Validate single xml document
        /// </summary>
        /// <param name="xmldocument">Xml as string</param>
        /// <returns>Validation result</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "validate", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EckValidationResult Validate(ValidationRequest request);

        [OperationContract]
        [WebInvoke(UriTemplate = "xml/validate", Method = "POST", RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml)]
        EckValidationResult ValidateXml(ValidationRequest request);

    }
}
