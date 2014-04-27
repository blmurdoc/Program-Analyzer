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
        public void InitializeCase3Objects(Global Global)
        {
            // Get all the semi-security objects
            var semiSecurityObjects = Global.UnevaluatedObjects.Where(i => i.IsSemiSecurityObject);

            // Go through the semi-security objects
            foreach(UnevaluatedObject uo in semiSecurityObjects)
            {
                // Get the methods that directly affect SO attributes
                var directlyAffectMethods = uo.Methods.Where(i => i.DirectlyAffectSecurityAttribute);

                // Go through each method
                foreach(OwnedMethod om in directlyAffectMethods)
                {
                    // Place every object that calls these methods into the case 3 objects list
                    foreach(CalledByMethod cm in om.CalledByMethods)
                    {
                        var case3Object = new CaseObject()
                        {
                            Name = cm.ParentObjectName,
                            MethodNames = new List<string>()
                        };

                        // Check for duplicates
                        var objectExists = Case3Objects.Where(i => i.Name == cm.ParentObjectName).SingleOrDefault();
                        if(objectExists == null)
                        {
                            if (case3Object.MethodNames.Where(i => i == cm.Name).SingleOrDefault() == null)
                                case3Object.MethodNames.Add(cm.Name);
                            Case3Objects.Add(case3Object);
                        }
                        else
                        {
                            if (case3Object.MethodNames.Where(i => i == cm.Name).SingleOrDefault() == null)
                                Case3Objects.Where(i => i.Name == cm.ParentObjectName).Single().MethodNames.Add(cm.Name);
                        }

                        // Make the object semi-security
                        Global.UnevaluatedObjects.Where(i => i.Name == case3Object.Name).Single().IsSemiSecurityObject = true;
                    }
                }
            }
        }
    }
}
