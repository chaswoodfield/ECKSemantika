using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Semantika.EuropeanaInside.Eck.Validation
{
    public class XmlValidator
    {
        private readonly List<BrokenRule> _brokenRules = new List<BrokenRule>();

        public List<BrokenRule> ValidateXml(string xml, ValidationProfile validationProfile)
        {
            try
            {

                var xmlReader = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(xml)));
                var xdoc = XDocument.Load(xmlReader);
                var nmspc = xdoc.Root.Name.Namespace;

                var schemas = CachedSchemas.Get(validationProfile); // new XmlSchemaSet();
                //schemas.Add("http://www.lido-schema.org", "http://www.lido-schema.org/schema/v1.0/lido-v1.0.xsd");
                xdoc.Validate(schemas, ValidationEventHandler);

                //dodatna preverjanja
                if (validationProfile == ValidationProfile.Lido)
                {
                    var additionalBrokenRule = CheckResourceSetRightsType(xdoc, nmspc);
                    if (additionalBrokenRule != null)
                        _brokenRules.Add(additionalBrokenRule);
                }

            }
            catch (Exception error)
            {
                _brokenRules.Add(new BrokenRule
                    {
                        BrokenRuleCode = "#",
                        Message = "Error validating document. " + Environment.NewLine + error.Message,
                        Severity = SeverityType.Error
                    });
            }

            return _brokenRules;
        }



        void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            _brokenRules.Add(new BrokenRule
                {
                    BrokenRuleCode = "*",
                    Message = e.Message,
                    Severity = e.Severity == XmlSeverityType.Error ? SeverityType.Error : SeverityType.Warning
                });
        }

        #region Private methods

        private BrokenRule CheckResourceSetRightsType(XDocument p_Xdoc, XNamespace p_Xns)
        {
            if (p_Xdoc != null)
            {
                try
                {
                    var administrativeMetadata = p_Xdoc.Descendants(p_Xns + "administrativeMetadata").FirstOrDefault();
                    var conceptID = administrativeMetadata.Element(p_Xns + "resourceWrap").Element(p_Xns + "resourceSet").Element(p_Xns + "rightsResource").Element(p_Xns + "rightsType").Element(p_Xns + "conceptID");
                    
                    
                    if (conceptID.Value != null && conceptID.Value.StartsWith("http://") && (conceptID.Value.IndexOf("creativecommons.org/") > 0 || conceptID.Value.IndexOf("europeana.eu/rights") > 0 || conceptID.Value.IndexOf("europeana.eu/portal/rights") > 0))
                        return null;
                    else
                    {
                        //the lido is not valid
                        return new BrokenRule
                        {
                            BrokenRuleCode = "*",
                            Message = "rightsType in lido:administrativeMetadata/lido:resourceWrap/lido:resourceSet/lido:rightsResource/lido:rightsType/conceptID does not containt proper value",
                            Severity = SeverityType.Error
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new BrokenRule
                    {
                        BrokenRuleCode = "*",
                        Message = "Error validating document. " + Environment.NewLine + ex.Message,
                        Severity = SeverityType.Error
                    };
                }

            }

            return null;
        }

        #endregion
    }
}
