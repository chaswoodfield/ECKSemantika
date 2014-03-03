using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Semantika.EuropeanaInside.Eck.Validation
{
    public class MediaReferenceValidator
    {
        public bool ValidateMediaReference(string pUrl)
        {
            HttpWebResponse response = null;
           

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(pUrl);
                request.Method = "HEAD";
                response = (HttpWebResponse) request.GetResponse();
                return true;
            }
            catch 
            {
                return false;
            }
            finally
            {
                // Don't forget to close your response.
                if (response != null)
                {
                    response.Close();
                }
            }
        }
    }
}
