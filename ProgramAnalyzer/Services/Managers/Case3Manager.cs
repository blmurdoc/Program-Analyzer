using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Managers
{
    public class Case3Manager
    {
        public List<CaseObject> Case3Objects = new List<CaseObject>();
        /// <summary>
        /// UO Method calls SSO Method that affects SA directly.
        /// </summary>
        public void InitializeCase3Objects(List<UnevaluatedObject> unevaluatedObject)
        {
            throw new NotImplementedException();
        }
    }
}
