using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Services.Managers
{
    [DataContract]
    public class Global
    {
        [DataMember]
        public List<UnevaluatedObject> UnevaluatedObjects
        {
            get
            {
                if (unevaluatedObjects == null)
                    unevaluatedObjects = new List<UnevaluatedObject>();
                return unevaluatedObjects;
            }
            set
            {
                unevaluatedObjects = value;
            }
        }
        private List<UnevaluatedObject> unevaluatedObjects;
    }
}
