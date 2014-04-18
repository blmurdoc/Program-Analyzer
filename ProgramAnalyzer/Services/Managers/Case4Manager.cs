using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Managers
{
    public class Case4Manager
    {
        public List<CaseObject> Case4Objects = new List<CaseObject>();
        /// <summary>
        /// UO Method affects SSO Attribute which affects SO Attribute
        /// 
        /// -- Case 3 is superset (I think) -- DO NOT IMPLEMENT FOR NOW
        /// </summary>
        public void InitializeCase4Objects(List<UnevaluatedObject> unevaluatedObject)
        {
            throw new NotImplementedException();
        }
    }
}
