using System.Diagnostics;
using Semantika.EuropeanaInside.Eck.PidGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.IO;


namespace Semantika.EuropeanaInside.Eck
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class PidGenerationService:IPidGenerationService
    {
        public PidGenerationService()
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new Logger(true,"Pid.txt"));
        }

        public string Generate(PidData pd)
        {
            try
            {
                var ser = new DataContractJsonSerializer(typeof (PidData));
                var ms = new MemoryStream();
                ser.WriteObject(ms, pd);

                ms.Position = 0;
                var sr = new StreamReader(ms);

                Trace.WriteLine(DateTime.Now + "-> START Generate pidData = " + sr.ReadToEnd());
                Trace.WriteLine(DateTime.Now + "-> RETURN pid = " + pd.Pid);
                Trace.WriteLine(DateTime.Now + "-> END Generate");
                return pd.Pid;
            }
            catch (Exception err)
            {
                Trace.WriteLine(DateTime.Now + "-> ERROR: " + err.Message);
                Trace.WriteLine(DateTime.Now + "-> END Generate");
                return null;
            }
        }

        public PidData Lookup(string pid)
        {
            try
            {
                Trace.WriteLine(DateTime.Now + "-> START Lookup pid="+pid);
                if (string.IsNullOrEmpty(pid))
                    return null;

                var parts = pid.Split('_');


                PidData pData = null;
                if (parts.Count() > 2)
                {
                    pData = new PidData
                        {
                            InstitutionUrl = parts[0],
                            RecordType = parts[1],
                            AccessionNumber = parts[2]
                        };
                }
                Trace.WriteLine(DateTime.Now + "-> END Lookup");
                return pData;
            }
            catch (Exception err)
            {
                Trace.WriteLine(DateTime.Now +"-> ERROR: "+err.Message);
                Trace.WriteLine(DateTime.Now + "-> END Lookup");
                return null;
            }
        }



        public string Generate2(string institutionUrl, string recordType, string accessionNumber)
        {
            try
            {
                var pidData =  new PidData
                    {
                        InstitutionUrl = institutionUrl,
                        RecordType = recordType,
                        AccessionNumber = accessionNumber
                    };

                var ser = new DataContractJsonSerializer(typeof(PidData));
                var ms = new MemoryStream();
                ser.WriteObject(ms, pidData);

                ms.Position = 0;
                var sr = new StreamReader(ms);

                Trace.WriteLine(DateTime.Now + "-> START Generate2 pidData = " + sr.ReadToEnd());
                Trace.WriteLine(DateTime.Now + "-> RETURN pid = " + pidData.Pid);
                Trace.WriteLine(DateTime.Now + "-> END Generate2");

                return pidData.Pid;
            }
            catch (Exception err)
            {
                Trace.WriteLine(DateTime.Now + "-> ERROR: " + err.Message);
                Trace.WriteLine(DateTime.Now + "-> END Generate2");
                return null;
            }
        }
    }
}
