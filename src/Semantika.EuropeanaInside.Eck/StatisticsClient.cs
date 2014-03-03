using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Semantika.EuropeanaInside.Eck
{
    public class StatisticsClient
    {
        public void PostStatistics(string pDateTime, int pDuration, int pItemsProcessed, int pNumberFailed, int pNumberSuccessful)
        {
            //dateTime=2013-09-02T09:30:12.000&duration=1&itemsProcessed=1&numberFailed=0&numberSuccessful=1
        }

        private static TransformedResult GetEDMRequestID(XDocument p_Document, int p_DocumentID, string p_Provider, string p_Batch)
        {
            string xml = p_Document.Declaration.ToString() + p_Document.ToString();
            string fileName = p_DocumentID + ".xml";
            
            var client = new WebClient();
            //lets generate http body
            string boundary = "--bnd";
            string endBoundary = "--bnd--";
            string newLine = Environment.NewLine;
            client.Headers.Add("Content-Type", string.Format("multipart/form-data; boundary={0}", "bnd"));

            string contentDispositionFile = string.Format("Content-Disposition: form-data; name=\"record\"; filename=\"{0}\"", fileName);
            string contentTypeFile = "Content-Type: application/xml";

            string contentDespositionSourceFormat = "Content-Disposition: form-data; name=\"sourceFormat\"";
            string contentDespositionTargetFormat = "Content-Disposition: form-data; name=\"targetFormat\"";
            string contentTypeFormat = "Content-Type: text/plain; charset=UTF8";
            //  string body = boundary + newLine + contentDispositionFile + newLine + contentTypeFile + newLine + newLine + newLine + boundary + newLine + contentDespositionSourceFormat + newLine +
            //  contentTypeFormat + newLine + newLine + "LIDO" + newLine + boundary + newLine + contentDespositionTargetFormat + newLine + contentTypeFormat + newLine + newLine + "EDM" + newLine + xml +
            //  newLine + endBoundary + newLine;

            string body = 
                boundary + newLine + 
                contentDispositionFile + newLine + 
                contentTypeFile + newLine + newLine + 
                xml + newLine + 
                boundary + newLine + 
                contentDespositionSourceFormat + newLine +
                contentTypeFormat + newLine + newLine + 
                "LIDO" + newLine + 
                boundary + newLine + 
                contentDespositionTargetFormat + newLine + 
                contentTypeFormat + newLine + newLine + 
                "EDM" + newLine +
                endBoundary + newLine;


            string address = string.Format("http://services.libis.be/euInside/dmt.php/DataMapping/{0}/{1}/Transform", p_Provider, p_Batch);
            byte[] data = Encoding.UTF8.GetBytes(body);

            var response = client.UploadData(address, data);

            var t = Encoding.UTF8.GetString(response);

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(TransformedResult));
            MemoryStream ms = new MemoryStream(response);
            ms.Position = 0;
            TransformedResult result = (TransformedResult)serializer.ReadObject(ms);

            return result;
        }

    }
}
