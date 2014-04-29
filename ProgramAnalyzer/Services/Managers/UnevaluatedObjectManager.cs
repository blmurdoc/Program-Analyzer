using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Services.Managers;

namespace Services.Managers
{
    public class UnevaluatedObjectManager
    {
        public Global Global = new Global();
        public SecurityAttributeManager SecurityAttributeManager = new SecurityAttributeManager();
        public Case1Manager Case1Manager = new Case1Manager();
        public Case2Manager Case2Manager = new Case2Manager();
        public Case3Manager Case3Manager = new Case3Manager();
        public Case5Manager Case5Manager = new Case5Manager();
        public Case6Manager Case6Manager = new Case6Manager();

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
            string[] delimiters2 = new string[4];
            delimiters2[0] = " ";
            delimiters2[1] = "\n";
            delimiters2[2] = "\t";
            delimiters2[3] = "\r";
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

                while (currentIndex < brokenUpClass.Count() && (!encounteredOpenBrace || bracesCount != 0))
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
                            // Read the parameters
                            var startingParameterIndex = currentIndex + 3;
                            var endingParameterIndex = startingParameterIndex;
                            while (!brokenUpClass[endingParameterIndex].Contains(')'))
                                endingParameterIndex++;
                            var startingParameterEntry = brokenUpClass[startingParameterIndex];
                            var endingParameterEntry = brokenUpClass[endingParameterIndex];

                            var parameters = "";
                            for(int i = startingParameterIndex; i <= endingParameterIndex; i++)
                            {
                                if (i != endingParameterIndex)
                                    parameters += String.Format("{0} ", brokenUpClass[i]);
                                else
                                    parameters += brokenUpClass[i];
                            }

                            // Set the translation table for the method
                            string[] delimitersParameters = new string[4];
                            delimitersParameters[0] = " ";
                            delimitersParameters[1] = ",";
                            delimitersParameters[2] = "(";
                            delimitersParameters[3] = ")";
                            var splitParamters = parameters.Split(delimitersParameters, StringSplitOptions.RemoveEmptyEntries);
                            var methodDictionary = new Dictionary<string, string>();
                            for(int i = 0; i < splitParamters.Count() - 1; i = i + 2)
                                methodDictionary.Add(splitParamters[i], splitParamters[i + 1]);

                            var method = new OwnedMethod() { Name = brokenUpClass[currentIndex + 2], CalledByMethods = new List<CalledByMethod>() };
                            var tempMethodEntry = brokenUpClass[currentIndex + 1];
                            var tempMethodIndex = currentIndex + 1;
                            var tempMethodBracesCount = 0;
                            bool tempMethodEncounteredOpenBrace = false;
                            // Go through the method
                            while(!tempMethodEncounteredOpenBrace || tempMethodBracesCount != 0)
                            {
                                // Reset the object references for the new method
                                ObjectReferences = new Dictionary<string, string>();

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
                                    
                                    // Check if the object is a reference to a parameter
                                    var paramKey = "";
                                    foreach (string dictEntry in methodDictionary.Values)
                                    {
                                        if (dictEntry == methodSplit[0])
                                            paramKey = methodDictionary.Where(i => i.Value == dictEntry).Single().Key;
                                    }
                                    if (paramKey != "")
                                    {
                                        if (ObjectReferences.Where(i => i.Key == paramKey && i.Value == methodSplit[0]).Count() == 0)
                                            ObjectReferences.Add(paramKey, methodSplit[0]);
                                    }

                                    if (ObjectReferences.Count > 0)
                                    {
                                        var dictionaryEntry = ObjectReferences.Where(i => i.Value == methodSplit[0]).Single();

                                        // Add this method to the original object's called by methods
                                        var calledByMethod = new CalledByMethod() { Name = method.Name, ParentObjectName = className };
                                        var originalObject = Global.UnevaluatedObjects.Where(i => i.Name == dictionaryEntry.Key).Single();
                                        originalObject.Methods.Where(i => i.Name == methodSplit[1]).Single().CalledByMethods.Add(calledByMethod);
                                    }
                                }
                                // Could be directly affecting a security attribute
                                else if(brokenUpClass[tempMethodIndex + 1] == "=")
                                {
                                    // Check if this attribute is a security attribute
                                    bool checkSecurityAttribute = false;
                                    foreach (SecurityAttribute sa in SecurityAttributeManager.SecurityAttributes)
                                    {
                                        if (sa.Name == tempMethodEntry)
                                            checkSecurityAttribute = true;

                                        // Check the method translation table
                                        foreach(string de in methodDictionary.Values)
                                        {
                                            // If the attribute is being referenced directly
                                            var splitEntry = tempMethodEntry.Split('.');
                                            for(int i = 0; i < splitEntry.Count(); i++)
                                            {
                                                if (splitEntry[i] == sa.Name && i > 0 && splitEntry[i - 1] == de)
                                                    checkSecurityAttribute = true;
                                            }
                                        }
                                    }

                                    // Set the affect field
                                    method.DirectlyAffectSecurityAttribute = checkSecurityAttribute;
                                }
                                else
                                {
                                    var parenCheck = brokenUpClass[tempMethodIndex + 1].First();

                                    // Could be a method call on the same object
                                    if(parenCheck == '(')
                                    {
                                        var methodExists = Methods.Where(i => i.Name == tempMethodEntry).SingleOrDefault();
                                        if (methodExists != null)
                                        {
                                            methodExists.CalledByMethods.Add(new CalledByMethod()
                                                {
                                                    Name = method.Name,
                                                    ParentObjectName = className
                                                });
                                        }
                                    }

                                
                                }
                                // Set the translation dictionary.
                                method.ParameterTranslations = methodDictionary;
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

                Global.UnevaluatedObjects.Add(unevaluatedObject);
            }
        }
    }
}