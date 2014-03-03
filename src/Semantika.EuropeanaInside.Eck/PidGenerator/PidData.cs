using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Semantika.EuropeanaInside.Eck.PidGenerator
{
    [DataContract]
    public class PidData
    {
        public PidData()
        {
            InstitutionUrl = "";
            RecordType = "";
            AccessionNumber = "";
        }

        public string Pid
        {
            get
            {
                return InstitutionUrl + "_" + RecordType + "_" + AccessionNumber;
            }
        }

        [DataMember]
        public string InstitutionUrl { get; set; }
        [DataMember]
        public string RecordType{ get; set; }
        [DataMember]
        public string AccessionNumber { get; set; }
    }
}
