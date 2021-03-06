﻿using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ObjectManager
    {
        // Initialize the lists.
        public List<SecurityObject> SecurityObjects = new List<SecurityObject>();
        public List<UnevaluatedObject> UnevaluatedObjects = new List<UnevaluatedObject>();
        public Case1 Case1Objects = new Case1();

        /// <summary>
        /// The attributes list will be in the form:
        /// 
        ///     A.b, C.d
        ///     
        /// where A and C are security objects and b and d are the attributes
        /// that contains secure information.
        /// 
        /// This will create the Security Objects with the attributes
        /// in place.
        /// </summary>
        public void InitializeSecurityObjects(string items)
        {
            // Split the items on the comma.
            var splitItems = items.Split(',');

            // Go through the items and create security objects.
            foreach(string s in splitItems)
            {
                // Get the name of the security object and attribute.
                var objects = s.Split('.');
                var securityObjectName = objects[0].Replace(" ", "");
                var attributeName = objects[1].Replace(" ", "");

                // Check if the security object already exists in our list.
                if(CheckIfSecurityObjectExists(securityObjectName))
                {
                    // Find the security object and add the new secure attribute.
                    var obj = SecurityObjects.Where(i => i.Name == securityObjectName).Single();
                    obj.SecureAttributes.Add(new DTOs.Attribute
                        {
                            Name = attributeName,
                            IsSecure = true
                        });
                }
                else
                {
                    // Add the new security object with its object.
                    SecurityObjects.Add(new SecurityObject
                        {
                            Name = securityObjectName,
                            SecureAttributes = new List<DTOs.Attribute>(),
                            AffectSecureAttributesMethods = new List<Method>()
                        });
                    var obj = SecurityObjects.Where(i => i.Name == securityObjectName).Single();
                    obj.SecureAttributes.Add(new DTOs.Attribute
                        {
                            Name = attributeName,
                            IsSecure = true
                        });
                }
            }
         }

        /// <summary>
        /// Called after initializing the security object attributes. 
        /// This will go through the given program and set the methods that
        /// interact with the security object's attributes.
        /// </summary>
        public void InitializeSecurityObjectMethods(string programText)
        {
            foreach(SecurityObject s in SecurityObjects)
            {
                // Search the program for the class declaration.
                var startingIndex = programText.IndexOf(String.Format("class {0}", s.Name));
                
                // Found the start of the class declaration.
                if(startingIndex != -1)
                {
                    var classDeclaration = programText.Substring(startingIndex).Split(' ');
                    for(int i = 0; i < classDeclaration.Count(); i++)
                    {
                        // Check the public methods.
                        PublicMethodCheck(classDeclaration, i, s);
                    }
                }
            }
        }

        /// <summary>
        /// Takes in the program and places objects into the unevaluated objects if they are 
        /// not secure.
        /// </summary>
        public void InitializeUnevaluatedCaseOneObjects(string programText)
        {
            string[] delimiters = new string[1];
            delimiters[0] = "class";
            var classes = programText.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            // Go through each class declaration
            foreach(string ss in classes)
            {
                char[] splitting = new char[1];
                splitting[0] = ' ';
                var classDeclaration = ss.Split(splitting, StringSplitOptions.RemoveEmptyEntries);
                var className = classDeclaration[0];
                bool isInSecurityObjects = false;
                foreach(SecurityObject s in SecurityObjects)
                {
                    if (s.Name == className)
                        isInSecurityObjects = true;
                }
                if(!isInSecurityObjects && UnevaluatedObjects.Where(i => i.Name == className).SingleOrDefault() == null)
                {
                    // Create the unevaluated object
                    UnevaluatedObjects.Add(new UnevaluatedObject()
                        {
                            Name = className,
                            IsSemiSecurity = false,
                            AccessedMethods = new List<Method>()
                        });

                    // Add the methods from the unevaluated class to its accessed methods.
                    for (int i = 0; i < classDeclaration.Length; i++ )
                    {
                        if (i < classDeclaration.Length - 3 && classDeclaration[i] == "public" && classDeclaration[i + 3].First() == '(')
                        {
                            var methodToAdd = new Method() { Name = classDeclaration[1] };
                            var obj = UnevaluatedObjects.Where(j => j.Name == className).Single();
                            obj.AccessedMethods.Add(methodToAdd);

                            foreach(SecurityObject so in SecurityObjects)
                            {
                                // Check if any security object's accessed methods are called from this method.
                                // Similar to the way the test is structured.
                                foreach (Method m in so.AffectSecureAttributesMethods)
                                {
                                    // Check if the name is called.
                                }
                            }
                        }
                    }
                }

            }
        }

        /// <summary>
        /// Checks for public methods.
        /// </summary>
        private void PublicMethodCheck(string[] classDeclaration, int i, SecurityObject s)
        {
            if (classDeclaration[i] == "public" && i < classDeclaration.Count() - 4 && classDeclaration[i + 3].First() == '(')
            {
                // Next word will be the return type so we need i + 2 for the name of the method.
                var name = classDeclaration[i + 2];

                // Should be the parameters of the function
                var loopCount = 0;

                // Go throught the public method to see if it alters any attributes.
                while (classDeclaration[i + loopCount] != "}")
                {
                    CheckMethodAltersAttributeDirectly(s, classDeclaration, loopCount, name, i);
                    loopCount++;
                }
            }
        }

        /// <summary>
        ///  Check if the method alters any secure attributes directly.
        /// </summary>
        private void CheckMethodAltersAttributeDirectly(SecurityObject s, string[] classDeclaration, int loopCount, string name, int i)
        {
            foreach (DTOs.Attribute a in s.SecureAttributes)
            {
                // Found a affected method. Add it to the security object's methods.
                if (classDeclaration[i + loopCount] == a.Name)
                {
                    var methodExists = s.AffectSecureAttributesMethods.Where(j => j.Name == name).SingleOrDefault();
                    // Check if method already exists.
                    if (methodExists != null)
                        s.AffectSecureAttributesMethods.Where(k => k.Name == name).Single().AccessedAttributes.Add(a);
                    // Method doesn't exist.
                    else
                    {
                        var method = new DTOs.Method()
                        {
                            Name = name,
                            AccessedAttributes = new List<DTOs.Attribute>()
                        };
                        method.AccessedAttributes.Add(a);
                        s.AffectSecureAttributesMethods.Add(method);
                    }
                }
            }
        }

        /// <summary>
        /// Checks the list of security objects and sees if the given security
        /// object's name exists.
        /// </summary>
        private bool CheckIfSecurityObjectExists(string name)
        {
            foreach(SecurityObject s in SecurityObjects)
                if (s.Name == name)
                    return true;
            return false;
        }
    }
}
