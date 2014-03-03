using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Semantika.EuropeanaInside.Eck.Validation
{
    /// <summary>
    /// Represents validation schema type
    /// </summary>
    [DataContract]
    public enum ValidationProfile
    {
        [EnumMember]
        Edm,

        [EnumMember]
        Lido
    }
}
