using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Managers
{
    public class Case1Manager
    {
        public List<Case1Object> Case1Objects = new List<Case1Object>();
        /// <summary>
        /// UO methods call SO methods that affect SA
        /// </summary>
        public void InitializeCase1Objects(List<UnevaluatedObject> unevaluatedObjects)
        {
            // Capture all Unevaluated Objects that are Secure.
            var securityObjects = unevaluatedObjects.Where(i => i.IsSecurityObject);

            // Go through all security objects.
            foreach (UnevaluatedObject uo in securityObjects) 
            {
                // Capture all methods that touch security objects.
                var securityMethods = uo.Methods.Where(i => i.DirectlyAffectSecurityAttribute);

                // Go through all Security Methods.
                foreach (OwnedMethod om in securityMethods)
                {
                    foreach (CalledByMethod cm in om.CalledByMethods)
                    {
                        // Add the case one object to the list.
                        var case1object = new Case1Object()
                        {
                            Name = cm.ParentObjectName,
                            MethodNames = new List<string>()
                        };
                        case1object.MethodNames.Add(cm.Name);
                        var obj = Case1Objects.Where(i => i.Name == case1object.Name).SingleOrDefault();
                        if (obj == null)
                            Case1Objects.Add(case1object);
                        else 
                            obj.MethodNames.Add(cm.Name);
                    }
                }
            }
        }
    }
}
