using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Activation;
using Semantika.EuropeanaInside.Eck.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.ServiceModel.Web;

namespace Semantika.EuropeanaInside.Eck
{
    
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ValidationService:IValidationService
    {
        public ValidationService()
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Logger(true, "Validation.txt"));
        }

        public EckValidationResult Validate(ValidationRequest request)
        {
            try
            {
                try
                {
                    request.XmlDocument = UTF8Encoding.UTF8.GetString(Convert.FromBase64String(request.XmlDocument));
                }
                catch
                {
                }

               

                Trace.WriteLine(DateTime.Now + "-> START Generate request = " +request.Name);
              
              

                var validationResult = new EckValidationResult();
                var validator = new XmlValidator();
                validationResult.BrokenRules = validator.ValidateXml(request.XmlDocument,
                                                                     (ValidationProfile)
                                                                     Enum.Parse(typeof (ValidationProfile), request.Name,
                                                                                true));

                if (validationResult.BrokenRules.Count > 0)
                {
                    validationResult.IsValid = false;
                    validationResult.Message = "Correct errors and then try again.";
                }
                else
                {
                    validationResult.IsValid = true;
                    validationResult.Message = "Your document is VALID!";
                }

            
               

                Trace.WriteLine(DateTime.Now + "-> RETURN validationResult = " + validationResult.IsValid);
                Trace.WriteLine(DateTime.Now + "-> END Generate");

                return validationResult;
            }
            catch (Exception err)
            {
                Trace.WriteLine(DateTime.Now + "-> ERROR: " + err.Message);
                Trace.WriteLine(DateTime.Now + "-> END Validate");
                return new EckValidationResult
                    {
                        BrokenRules = new List<BrokenRule>(),
                        IsValid = false,
                        Message = "Server internal error!"
                    };
            }
        }

        public List<string> Profiles()
        {
            string [] profiles = {"lido", "edm"};
            return profiles.ToList();            
        }


        public string Profile(string name)
        {
            if(string.IsNullOrEmpty(name))
                name="";

            switch (name.ToLower())
            {
                case "lido":
                    {
                        return XsdSchemas.Lido;
                    }

                case "edm":
                    {
                        return XsdSchemas.Edm;
                    }
            }

            return string.Empty;
        }


        public List<string> ProfilesXml()
        {
            return Profiles();
        }

        public string ProfileXml(string name)
        {
            return Profile(name);
        }

        public EckValidationResult ValidateXml(ValidationRequest request)
        {
            return Validate(request);
        }
    }
}
