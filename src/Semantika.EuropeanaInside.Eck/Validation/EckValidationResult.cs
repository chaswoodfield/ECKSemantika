using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Semantika.EuropeanaInside.Eck.Validation
{
    /// <summary>
    /// Represents service validation result
    /// </summary>
    [DataContract]
    public class EckValidationResult
    {
        /// <summary>
        /// Represents document validation status (is document valid or not)
        /// </summary>
        [DataMember]
        public bool IsValid { get; set; }
       

        /// <summary>
        /// Validation global message
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// List of broken rules
        /// </summary>
        [DataMember]
        public List<BrokenRule> BrokenRules { get; set; }
    }
}
