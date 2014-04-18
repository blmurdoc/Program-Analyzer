﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    [DataContract]
    public class OwnedMethod
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public bool DirectlyAffectSecurityAttribute { get; set; }
        [DataMember]
        public List<CalledByMethod> CalledByMethods { get; set; }
        [DataMember]
        public Dictionary<string, string> ParameterTranslations { get; set; }
    }
}