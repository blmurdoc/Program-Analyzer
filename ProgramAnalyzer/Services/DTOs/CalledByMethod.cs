using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    [DataContract]
    public class CalledByMethod
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string ParentObjectName { get; set; }
    }
}