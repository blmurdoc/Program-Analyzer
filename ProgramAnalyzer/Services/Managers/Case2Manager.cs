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
            var case2Objects = new List<CaseObject>();
            foreach(UnevaluatedObject uo in unevaluatedObjects)
            {
                if (uo.Methods.Where(i => i.DirectlyAffectSecurityAttribute).Count() > 0 && !uo.IsSecurityObject)
                {
                    var methods = uo.Methods.Where(i => i.DirectlyAffectSecurityAttribute);
                    var methodNames = new List<string>();
                    foreach (OwnedMethod om in methods)
                        methodNames.Add(om.Name);

                    Case2Objects.Add(new CaseObject()
                        {
                            Name = uo.Name,
                            MethodNames = methodNames
                        });
                }
            }
        }
    }
}