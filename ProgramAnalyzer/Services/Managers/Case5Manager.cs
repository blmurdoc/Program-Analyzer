using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Managers
{
    public class Case5Manager
    {
        public List<CaseObject> Case5Objects = new List<CaseObject>();
        /// <summary>
        /// SO1 Method1 Calls SO1 Method2 that directly affects SA
        /// </summary>
        public void InitializeCase5Objects(Global Global)
        {
            // Get all security objects
            var securityObject = Global.UnevaluatedObjects.Where(i => i.IsSecurityObject);

            // Go through all of the security onjects
            foreach (UnevaluatedObject uo in securityObject)
            {
                // Get all methods that affect security attributes
                var methodDirectlyAffectsAttributes = uo.Methods.Where(i => i.DirectlyAffectSecurityAttribute);

                // Go through all the methods 
                foreach (OwnedMethod om in methodDirectlyAffectsAttributes)
                {
                    // Go through all methods that call the above methods
                    foreach(CalledByMethod cm in om.CalledByMethods)
                    {
                        // Check if parent is security object
                        var obj = securityObject.Where(i => i.Name == cm.ParentObjectName).SingleOrDefault();

                        // Check is Object is null
                        if (obj != null)
                        {
                            // Add new case 5 objects name and method names
                            var case5 = new CaseObject()
                            {
                              Name = obj.Name, 
                              MethodNames = new List<string>()

                          };
                          // Check is object already belongs to the case 5 objects
                          var objectExists = Case5Objects.Where(i => i.Name == case5.Name).SingleOrDefault();

                          // If doesn't exist add new object
                          if (objectExists == null)
                          {
                              case5.MethodNames.Add(cm.Name);
                              Case5Objects.Add(case5);
                          }
                          // Add method name
                          else
                          {
                              objectExists.MethodNames.Add(cm.Name);
                          }
                          // Mark object as semi security
                          Global.UnevaluatedObjects.Where(i => i.Name == case5.Name).Single().IsSemiSecurityObject = true;
                       }
                   }
                }
            }
        }
    }
}
