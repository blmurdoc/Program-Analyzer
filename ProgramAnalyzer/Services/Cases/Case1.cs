using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    [DataContract]
    public class Case1
    {
        [DataMember]
        public List<UnevaluatedObject> ContainedSemiSecurityObjects { get; set; }
    }
}
