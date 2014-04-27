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
        /// </summary>
        public void InitializeCase6Criteria1Objects(Global Global)
        {
            // Get all the security objects
            var securityObjects = Global.UnevaluatedObjects.Where(i => i.IsSecurityObject);

            // Go through all the security objects
            foreach(UnevaluatedObject uo in securityObjects)
            {
                // Get all methods of each security objects that directly affect security attributes
                var directlyAffectMethods = uo.Methods.Where(i => i.DirectlyAffectSecurityAttribute);

                // Go through each method
                foreach(OwnedMethod om in directlyAffectMethods)
                {
                    // Go through every method that calls the methods that directly affect security attributes
                    foreach(CalledByMethod cm in om.CalledByMethods)
                    {
                        // Check if the parent object of the called by method is a semi security
                        var objectIsSemiSecurity = Global.UnevaluatedObjects.Where(i => i.Name == cm.ParentObjectName && i.IsSemiSecurityObject).SingleOrDefault();

                        // The parent object is semi security
                        if(objectIsSemiSecurity != null)
                        {
                            // Go through every method of the parent object
                            foreach(OwnedMethod omm in objectIsSemiSecurity.Methods)
                            {
                                // Go through every method called by the methods of the parent object
                                foreach (CalledByMethod cmm in omm.CalledByMethods)
                                {
                                    // Check if the new parent object is semi security
                                    var objectCalledIsSemiSecurity = Global.UnevaluatedObjects.Where(i => i.Name == cmm.ParentObjectName && i.IsSemiSecurityObject).SingleOrDefault();

                                    // The parent object is semi security
                                    if(objectCalledIsSemiSecurity != null)
                                    {
                                        // We have found a case 6 criteria 1 object
                                        var case6Object = new CaseObject()
                                        {
                                            Name = objectCalledIsSemiSecurity.Name,
                                            MethodNames = new List<string>()
                                        };

                                        // Check for duplicates
                                        var objectExists = Case6Objects.Where(i => i.Name == case6Object.Name).SingleOrDefault();

                                        // No duplicate exists, create a new object
                                        if (objectExists == null)
                                        {
                                            if(case6Object.MethodNames.Where(i => i == cmm.Name).SingleOrDefault() == null)
                                                case6Object.MethodNames.Add(cmm.Name);
                                            Case6Objects.Add(case6Object);
                                        }
                                        // Object already exists, add the method name
                                        else
                                        {
                                            if (case6Object.MethodNames.Where(i => i == cmm.Name).SingleOrDefault() == null)
                                                objectExists.MethodNames.Add(cmm.Name);
                                        }

                                        // Mark the object semi security in the master list
                                        Global.UnevaluatedObjects.Where(i => i.Name == case6Object.Name).Single().IsSemiSecurityObject = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// SSO1 Method1 Calls SSO1 Method2 that directly affects SA
        /// </summary>
        public void InitializeCase6Criteria2Objects(Global Global)
        {
            // Check if object is semi security
            var semiSecurityObjects = Global.UnevaluatedObjects.Where(i => i.IsSemiSecurityObject);

            // Go through all unevaulated objects
            foreach (UnevaluatedObject uo in semiSecurityObjects)
            {
                // Get all methods that directly affect security attributes
                var directlyAffectedMethods = uo.Methods.Where(i => i.DirectlyAffectSecurityAttribute);

                // Go through ever method of the parent object
                foreach (OwnedMethod om in directlyAffectedMethods)
                {
                    // Go through every method called my the methods of the parent object
                    foreach (CalledByMethod cbm in om.CalledByMethods)
                    {
                        // Check if the objects parent is semi secuity
                        var checkObjectForSemiSecurity = semiSecurityObjects.Where(i => i.Name == cbm.ParentObjectName).SingleOrDefault();
                        
                        // Exists a semi security object
                        if (checkObjectForSemiSecurity != null)
                        {
                            // case 6 criteria 2 has been found
                            var case6Object = new CaseObject()
                            {
                                Name = checkObjectForSemiSecurity.Name,
                                MethodNames = new List<string>()
                            };
                            // Check if exists a case6 object 
                            var checkExists = Case6Objects.Where(i => i.Name == case6Object.Name).SingleOrDefault();

                            // Doesnt exist a case 6 object
                            if (checkExists == null)
                            {
                                if (case6Object.MethodNames.Where(i => i == cbm.Name).SingleOrDefault() == null)
                                    case6Object.MethodNames.Add(cbm.Name);
                                Case6Objects.Add(case6Object);
                            }
                            // Object already exists, add the method name
                            else
                            {
                                if (case6Object.MethodNames.Where(i => i == cbm.Name).SingleOrDefault() == null)
                                    checkExists.MethodNames.Add(cbm.Name);
                            }

                            // Mark the object semi security in the master list
                            Global.UnevaluatedObjects.Where(i => i.Name == case6Object.Name).Single().IsSemiSecurityObject = true;
                        }
                    } 
                }
            }
        }
    }
}
