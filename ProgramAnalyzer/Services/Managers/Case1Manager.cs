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
        public List<CaseObject> Case1Objects = new List<CaseObject>();
        /// <summary>
        /// UO methods call SO methods that affect SA
        /// </summary>
        public void InitializeCase1Objects(Global Global)
        {
            // Capture all Unevaluated Objects that are Secure.
            var securityObjects = Global.UnevaluatedObjects.Where(i => i.IsSecurityObject);

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
                        var case1object = new CaseObject()
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

                        // Make the object semi-secure
                        Global.UnevaluatedObjects.Where(i => i.Name == case1object.Name).Single().IsSemiSecurityObject = true;
                    }
                }
            }
        }
    }
}
