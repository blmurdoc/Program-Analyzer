using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Managers
{
    public class Case2Manager
    {
        public List<CaseObject> Case2Objects = new List<CaseObject>();
        /// <summary>
        /// UO Method affects SA directly.
        /// </summary>
        public void InitializeCase2Objects(List<UnevaluatedObject> unevaluatedObjects)
        {
            throw new NotImplementedException();
        }
    }
}