using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    [DataContract]
    public class Method
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<Method> AccessedMethods { get; set; }

        [DataMember]
        public List<Attribute> AccessedAttributes { get; set; }
    }
}
