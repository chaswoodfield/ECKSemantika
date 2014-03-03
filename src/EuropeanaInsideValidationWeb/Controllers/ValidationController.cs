using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace EuropeanaInsideValidationWeb.Controllers
{
    public class ValidationController : ApiController
    {
        private const string ProfileUploadLocation = "~/UploadedProfiles/";

        public HttpResponseMessage Post(string provider, string name)
        {

            var validationResult = new ValidationResult {Provider = provider, Set = "0", Record = new List<Record>()};
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                var profileLocation = HttpContext.Current.Server.MapPath(ProfileUploadLocation + "/" + provider + "/" + name +"/profile.xsd");

                var ms = new MemoryStream(File.ReadAllBytes(profileLocation));
                var schema = XmlSchema.Read(ms, null);
                var xmlSchemaSet = new XmlSchemaSet();
                xmlSchemaSet.Add(schema);
                xmlSchemaSet.Compile();


                var validator = new XmlValidator();
                
                var postedFile = httpRequest.Files[0];
                var stream = new StreamReader(postedFile.InputStream);
                string x = stream.ReadToEnd();
                string xml = HttpUtility.UrlDecode(x);

                var record = validator.ValidateXml(xml, xmlSchemaSet, "0", "0", provider, name);
                validationResult.Record.Add(record);

                var serializer = new XmlSerializer(typeof(ValidationResult));
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                var sms = new MemoryStream();
                serializer.Serialize(sms, validationResult, ns);
                sms.Position = 0;
                var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StreamContent(sms) };
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/xml");
                return response;

            }

            if (httpRequest.InputStream.Length>0)
            {
                var profileLocation = HttpContext.Current.Server.MapPath(ProfileUploadLocation + "/" + provider + "/" + name + "/profile.xsd");

                var ms = new MemoryStream(File.ReadAllBytes(profileLocation));
                var schema = XmlSchema.Read(ms, null);
                var xmlSchemaSet = new XmlSchemaSet();
                xmlSchemaSet.Add(schema);
                xmlSchemaSet.Compile();


                var validator = new XmlValidator();

               
                var stream = new StreamReader(httpRequest.InputStream);
                string x = stream.ReadToEnd();
                string xml = HttpUtility.UrlDecode(x);

                var record = validator.ValidateXml(xml, xmlSchemaSet, "0", "0", provider, name);
                validationResult.Record.Add(record);

                var serializer = new XmlSerializer(typeof(ValidationResult));
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                var sms = new MemoryStream();
                serializer.Serialize(sms, validationResult, ns);
                sms.Position = 0;
                var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StreamContent(sms) };
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/xml");
                return response;
            }


            return new HttpResponseMessage(HttpStatusCode.PreconditionFailed);
           
        }
    }

    public class XmlValidator
    {
        private readonly Record _recordValidation = new  Record{Error = new List<Error>()};
        private readonly List<Error> _errors = new List<Error>();

        public Record ValidateXml(string xml, XmlSchemaSet schemas, string recordId, string set, string provider, string profile)
        {
            try
            {
                _recordValidation.Id = recordId;
                
                var xmlReader = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(xml)));
                var xdoc = XDocument.Load(xmlReader);
                xdoc.Validate(schemas, ValidationEventHandler);
            }
            catch (Exception error)
            {
                _errors.Add(new Error
                {
                    Plugin = "xsdValidator",
                    Value = error.Message
                });
            }

            if (_errors.Count > 0)
            {
                _recordValidation.Error = _errors;
                _recordValidation.Result = ResultType.Error;
            }
            else
            {
                _recordValidation.Result= ResultType.Success;
            }

            return _recordValidation;
        }

        void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            _errors.Add(new Error
            {
                Plugin = "xsdValidator",
                Value = e.Message
            });
        }
    }
 
    public class ValidationResult
    {
        [XmlAttribute]
        public string Set { get; set; }

        [XmlAttribute]
        public string Provider { get; set; }

        [XmlElement]
        public List<Record> Record { get; set; }

    }

    public class Record
    {
        [XmlAttribute]
        public string Id { get; set; }

        [XmlAttribute]
        public ResultType Result { get; set; }

        [XmlElement]
        public List<Error> Error { get; set; }
    }

    public class Error
    {
        [XmlAttribute]
        public string Plugin { get; set; }

        [XmlText]
        public string Value { get; set; }
    }

    public enum ResultType
    {
        Error,
        Success
    }
}
