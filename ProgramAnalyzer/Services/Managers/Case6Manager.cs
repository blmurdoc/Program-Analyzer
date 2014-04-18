using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Managers
{
    public class Case6Manager
    {
        public List<CaseObject> Case6Objects = new List<CaseObject>();
        /// <summary>
        /// SSO1 Method1 Calls SSO1 Method2 Calls SO Method that directly affects SA
        ///     or
        /// SSO1 Method1 Calls SSO1 Method2 that directly affects SA
        /// </summary>
        public void InitializeCase6Objects(List<UnevaluatedObject> unevaluatedObjects)
        {
            throw new NotImplementedException();
        }
    }
}
