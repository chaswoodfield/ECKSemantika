using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Semantika.EuropeanaInside.Eck.Validation
{
    /// <summary>
    /// Represents broken rule model
    /// </summary>
    [DataContract]
    public class BrokenRule
    {
        /// <summary>
        /// Gets or sets broken rule code
        /// </summary>
        [DataMember]
        public string BrokenRuleCode { get; set; }

        /// <summary>
        /// Gets or sets broken rule message
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets severity of broken rule
        /// </summary>
        [DataMember]
        public SeverityType Severity { get; set; }
    }
}
