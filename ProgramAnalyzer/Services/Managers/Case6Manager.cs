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
        public void InitializeCase6Objects(Global Global)
        {
            var securityObjects = Global.UnevaluatedObjects.Where(i => i.IsSecurityObject);

            foreach(UnevaluatedObject uo in securityObjects)
            {
                var directlyAffectMethods = uo.Methods.Where(i => i.DirectlyAffectSecurityAttribute);

                foreach(OwnedMethod om in directlyAffectMethods)
                {
                    foreach(CalledByMethod cm in om.CalledByMethods)
                    {
                        var objectIsSemiSecurity = Global.UnevaluatedObjects.Where(i => i.Name == cm.ParentObjectName && i.IsSemiSecurityObject).SingleOrDefault();

                        if(objectIsSemiSecurity != null)
                        {
                            foreach(OwnedMethod omm in objectIsSemiSecurity.Methods)
                            {
                                foreach (CalledByMethod cmm in omm.CalledByMethods)
                                {
                                    var objectCalledIsSemiSecurity = Global.UnevaluatedObjects.Where(i => i.Name == cmm.ParentObjectName && i.IsSemiSecurityObject).SingleOrDefault();

                                    if(objectCalledIsSemiSecurity != null)
                                    {
                                        var case6Object = new CaseObject()
                                        {
                                            Name = objectCalledIsSemiSecurity.Name,
                                            MethodNames = new List<string>()
                                        };

                                        var objectExists = Case6Objects.Where(i => i.Name == case6Object.Name).SingleOrDefault();

                                        if (objectExists == null)
                                        {
                                            case6Object.MethodNames.Add(cmm.Name);

                                            Case6Objects.Add(case6Object);
                                        }
                                        else
                                        {
                                            objectExists.MethodNames.Add(cmm.Name);
                                        }
                                        Global.UnevaluatedObjects.Where(i => i.Name == case6Object.Name).Single().IsSemiSecurityObject = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
