using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Semantika.EuropeanaInside.Eck.Validation
{
    [DataContract(Namespace = "")]
    public class ValidationRequest
    {
        [DataMember]
        public string Name {get; set;}

        [DataMember]
        public string XmlDocument { get; set; }
    }
}
