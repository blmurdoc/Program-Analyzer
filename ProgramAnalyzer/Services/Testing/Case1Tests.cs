using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Services.DTOs;

namespace Services
{
    public class Case1Tests
    {
        ObjectManager ObjectManager = new ObjectManager();

        [Fact]
        public void InitializeUnevaluatedCaseOneObjects_UnevaluatedClass_AddedToUnevaluatedObjectsList()
        {
            /// Initialize
            // Create the program text of a class that isn't secure.
            var programText = "class A { }";

            /// Test
            // Call the method 
            ObjectManager.InitializeUnevaluatedCaseOneObjects(programText);

            // Get the Object count
            var unevaluatedObjectCount = ObjectManager.UnevaluatedObjects.Count;

            /// Assert
            // Ensure that an object is added to the UnevaluatedObjectList
            Assert.Equal(1, unevaluatedObjectCount);
        }

        [Fact]
        public void initializeUnevaluatedCaseOneObjects_UnevaluatedObject_UniqueUnevaluatedObjectinList()
        {
            /// Initialize
            // create the object
            var objectToAdd = new UnevaluatedObject()
            {
                Name = "A"
            };
            ObjectManager.UnevaluatedObjects.Add(objectToAdd);

            // Create the program text
            var programText = "class A { }";

            /// Test
            // Call the method with the program text
            ObjectManager.InitializeUnevaluatedCaseOneObjects(programText);

            /// Assert
            // Ensure that the count is only one of the objects
            Assert.Equal(1, ObjectManager.UnevaluatedObjects.Count);
        }

        [Fact]
        public void InitializeUnevaluatedCaseOneObjects_UnevaluatedObjectContainsMethod_MethodAddedToUnevaluatedObjectsAccessedMethodsList()
        {
            /// Initialize
            // Create a program text that has a class with a method
            var programText = "class A { public void B () { } }";

            /// Test
            // Call the initialize method
            ObjectManager.InitializeUnevaluatedCaseOneObjects(programText);

            /// Assert
            // Ensure that the unevaluated object has the method
            Assert.Equal(1, ObjectManager.UnevaluatedObjects.Single().AccessedMethods.Count);
        }

        [Fact]
        public void InitializeUnevaluatedCaseOneObjects_UnevaluatedObjectContainsMethodThatAccessesSecureObjectsMethodThatAccessesSecureAttribute_ObjectAddedToSecureList()
        {
            /// Initialize
            // Call initialize secutity objects with secure object 'item'
            var item = "A.b";
            ObjectManager.InitializeSecurityObjects(item);

            var classADeclaration = "class A { private boolean b; public void Method_b () { b = false; } }"; 
            var classXDeclaration = "class X { A someA = new A(); public void Method_y () { someA.Method_b () } }";
            var programText = String.Format("{0} {1}", classADeclaration, classXDeclaration);
            ObjectManager.InitializeSecurityObjectMethods(programText);

            /// Test
            // Call case1 with the program text
            ObjectManager.InitializeUnevaluatedCaseOneObjects(programText);

            /// Assert
            // Ensure that the method Y is now in A's affected methods.
            Assert.Equal(2, ObjectManager.SecurityObjects.Single().AffectSecureAttributesMethods.Count);
        }
    }
}