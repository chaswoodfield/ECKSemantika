using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Semantika.EuropeanaInside.Eck.Validation
{
    /// <summary>
    /// Broken rule severity types
    /// </summary>
    [DataContract]
    public enum SeverityType
    {
        [EnumMember]
        Error,
        [EnumMember]
        Warning
    }
}
