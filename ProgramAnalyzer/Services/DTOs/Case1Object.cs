using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    [DataContract]
    public class Case1Object
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public List<string> MethodNames { get; set; }
    }
}
