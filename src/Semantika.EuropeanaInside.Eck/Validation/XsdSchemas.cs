using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Semantika.EuropeanaInside.Eck.Validation
{
    public static class XsdSchemas
    {
        private static string DownloadSchemaFromUrl(string pUrl)
        {
            var client = new WebClient();
            return client.DownloadString(pUrl);
        }

        public static string Edm
        {
            get { return Xsds.Edm; }
        }

        public static string Lido
        {
            get { return Xsds.Lido; }
        }
    }
}
