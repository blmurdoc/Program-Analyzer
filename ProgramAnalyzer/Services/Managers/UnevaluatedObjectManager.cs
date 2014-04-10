﻿using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Services.Managers
{
    public class UnevaluatedObjectManager
    {
        public List<UnevaluatedObject> UnevaluatedObjects = new List<UnevaluatedObject>();
        public SecurityAttributeManager SecurityAttributeManager = new SecurityAttributeManager();

        /// <summary>
        /// Goes through the program code and creates classes for all given objects.
        /// ProgramText must be in the following input
        /// 
        /// class <ClassName>
        /// {
        ///     public <type> <AttributeName>;
        ///     public <type> <MethodName> ()
        ///     {
        ///         ...
        ///     }
        /// }
        /// </summary>
        public void InitializeUnevaluatedObjects(string programText)
        {
            // Split the program text by classes
            string[] delimiters1 = new string[1];
            delimiters1[0] = "class";
            string[] delimiters2 = new string[1];
            delimiters2[0] = " ";
            var classes = programText.Split(delimiters1, StringSplitOptions.RemoveEmptyEntries);

            // Go through each class and add it to the list
            foreach(string s in classes)
            {
                // Now split by spaces
                var brokenUpClass = s.Split(delimiters2, StringSplitOptions.RemoveEmptyEntries);

                // Go through the individual class
                var currentIndex = 0;
                var currentEntry = brokenUpClass[currentIndex];
                var className = "";
                var bracesCount = 0;
                bool encounteredOpenBrace = false;
                List<OwnedMethod> Methods = new List<OwnedMethod>();

                // Used to hold the methods names for the original attributes
                Dictionary<string, string> ObjectReferences = new Dictionary<string, string>();

                while (!encounteredOpenBrace || bracesCount != 0)
                {
                    currentEntry = brokenUpClass[currentIndex];

                    // Keep track of the open braces
                    if (currentEntry == "{")
                    {
                        bracesCount++;
                        encounteredOpenBrace = true;
                    }
                    // Keep track of the close braces
                    else if (currentEntry == "}")
                        bracesCount--;
                    // Encountered a method/attribute
                    else if(currentEntry == "public")
                    {
                        // Method
                        if(brokenUpClass[currentIndex + 3].First() == '(')
                        {
                            var method = new OwnedMethod() { Name = brokenUpClass[currentIndex + 2], CalledByMethods = new List<CalledByMethod>() };
                            var tempMethodEntry = brokenUpClass[currentIndex + 1];
                            var tempMethodIndex = currentIndex + 1;
                            var tempMethodBracesCount = 0;
                            bool tempMethodEncounteredOpenBrace = false;
                            // Go through the method
                            while(!tempMethodEncounteredOpenBrace || tempMethodBracesCount != 0)
                            {
                                tempMethodEntry = brokenUpClass[tempMethodIndex];
                                // Keep track of the open braces
                                if (tempMethodEntry == "{")
                                {
                                    tempMethodBracesCount++;
                                    tempMethodEncounteredOpenBrace = true;
                                }
                                // Keep track of the close braces
                                else if (tempMethodEntry == "}")
                                    tempMethodBracesCount--;
                                // Current Word has a '.' and a '(' so it is a method call
                                else if(tempMethodEntry.Contains(".") && brokenUpClass[tempMethodIndex + 1].First() == '(')
                                {
                                    string[] methodDelimiter = new string[1];
                                    methodDelimiter[0] = ".";
                                    var methodSplit = tempMethodEntry.Split(methodDelimiter, StringSplitOptions.RemoveEmptyEntries);
                                    var dictionaryEntry = ObjectReferences.Where(i => i.Value == methodSplit[0]).Single();

                                    // Add this method to the original object's called by methods
                                    var calledByMethod = new CalledByMethod() { Name = method.Name, ParentObjectName = className };
                                    var originalObject = UnevaluatedObjects.Where(i => i.Name == dictionaryEntry.Key).Single();
                                    originalObject.Methods.Where(i => i.Name == methodSplit[1]).Single().CalledByMethods.Add(calledByMethod);
                                }
                                // Could be directly affecting a security attribute
                                else if(brokenUpClass[tempMethodIndex + 1] == "=")
                                {
                                    // Check if this attribute is a security attribute
                                    bool checkSecurityAttribute = false;
                                    foreach (SecurityAttribute sa in SecurityAttributeManager.SecurityAttributes)
                                        if (sa.Name == tempMethodEntry)
                                            checkSecurityAttribute = true;

                                    // Set the affect field
                                    method.DirectlyAffectSecurityAttribute = checkSecurityAttribute;
                                }
                                

                                tempMethodIndex++;
                            }
                            Methods.Add(method);
                        }
                        // Attribute
                        else
                        {
                            // Get the method's reference to the object and place it in the dictionary.
                            // Regex for only alphanumeric characters
                            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
                            var originalObjectName = rgx.Replace(brokenUpClass[currentIndex + 1], "");
                            var methodSpecificName = rgx.Replace(brokenUpClass[currentIndex + 2], "");

                            // Check if the methodSpecific name is a security attribute
                            // If it is then this class is a security class
                            bool isSecurityAttribute = false;
                            foreach (SecurityAttribute sa in SecurityAttributeManager.SecurityAttributes)
                                if (sa.Name == methodSpecificName && sa.ParentObjectName == className)
                                    isSecurityAttribute = true;

                            // Only add if the attribute isn't a security attribute of this object
                            if (!isSecurityAttribute)
                                ObjectReferences.Add(originalObjectName, methodSpecificName);
                        }
                    }

                    // Get the class Name
                    if (currentIndex == 0)
                        className = currentEntry;

                    // Increment the loop counter
                    currentIndex++;
                }

                // We should have all the info for the class now
                var unevaluatedObject = new UnevaluatedObject()
                {
                    Name = className,
                    Methods = Methods
                };

                // Check if the object is a security object
                foreach (SecurityAttribute sa in SecurityAttributeManager.SecurityAttributes)
                    if (sa.ParentObjectName == unevaluatedObject.Name)
                        unevaluatedObject.IsSecurityObject = true;

                UnevaluatedObjects.Add(unevaluatedObject);
            }
        }
    }
}
