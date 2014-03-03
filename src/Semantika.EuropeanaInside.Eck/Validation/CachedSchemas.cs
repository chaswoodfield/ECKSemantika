using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Schema;

namespace Semantika.EuropeanaInside.Eck.Validation
{
    public static class CachedSchemas
    {
        private static readonly System.Web.Caching.Cache _Cache = HttpRuntime.Cache;

        public static void Add(ValidationProfile schemaName, XmlSchemaSet xmlSchemaSet)
        {
            _Cache.Insert("xmlschema_"+schemaName.ToString(),xmlSchemaSet, null, DateTime.Now.AddDays(30), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
        }

        public static XmlSchemaSet Get(ValidationProfile schemaName)
        {
            var ccsch = _Cache["xmlschema_"+schemaName.ToString()] as XmlSchemaSet;

            if (ccsch == null)
            {
                using (
                    var ms =
                        new MemoryStream(
                            Encoding.UTF8.GetBytes(schemaName == ValidationProfile.Lido
                                                       ? XsdSchemas.Lido
                                                       : XsdSchemas.Edm)))
                {
                    var schema = XmlSchema.Read(ms, null);
                    var xmlSchemaSet = new XmlSchemaSet();
                    xmlSchemaSet.Add(schema);
                    xmlSchemaSet.Compile();

                    

                    ccsch = xmlSchemaSet;

                    Add(schemaName, xmlSchemaSet);
                }
            
            }

            return ccsch;
        }
    }
}
