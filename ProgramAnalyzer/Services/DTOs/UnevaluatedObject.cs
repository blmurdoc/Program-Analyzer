using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    [DataContract]
    public class UnevaluatedObject
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public List<OwnedMethod> Methods { get; set; }
        [DataMember]
        public bool IsSecurityObject { get; set; }
        [DataMember]
        public bool IsSemiSecurityObject { get; set; }
    }
}
