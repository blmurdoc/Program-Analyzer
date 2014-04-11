using Services.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Services.Testing
{
    public class UnevaluatedObjectTests
    {
        UnevaluatedObjectManager UnevaluatedObjectManager = new UnevaluatedObjectManager();

        [Fact]
        public void InitializeUnevaluatedObjects_GivenSingleClass_AddedToUnevaluatedObjectList()
        {
            /// Initialize
            // Create the program text
            var programText = "class A { }";

            /// Test
            // Call the MUT
            UnevaluatedObjectManager.InitializeUnevaluatedObjects(programText);

            /// Assert
            // Ensure that the object was added to the list
            Assert.Equal(1, UnevaluatedObjectManager.UnevaluatedObjects.Count);
        }

        [Fact]
        public void InitializeUnevaluatedObjects_GivenSingleClassWithMethod_MethodAddedToObject()
        {
            /// Initialize
            // Create the program text
            var programText = "class A { public void Method1 () { } }";

            /// Test
            // Call the MUT
            UnevaluatedObjectManager.InitializeUnevaluatedObjects(programText);

            /// Assert
            // Ensure that the method was added to the object
            Assert.Equal(1, UnevaluatedObjectManager.UnevaluatedObjects.Single().Methods.Count);
        }

        [Fact]
        public void InitializeUnevaluatedObjects_GivenSingleClassThatIsASecurityObject_ObjectMarkedAsSecurity()
        {
            /// Initialize
            // Create the attribute list
            var attributeList = "A.b";

            // Call the attribute initializer
            UnevaluatedObjectManager.SecurityAttributeManager.InitializeSecurityAttributes(attributeList);

            // Create the program text
            var programText = "class A { }";

            /// Test
            // Call the MUT
            UnevaluatedObjectManager.InitializeUnevaluatedObjects(programText);

            /// Assert
            // Ensure that the object was marked as a security object
            Assert.True(UnevaluatedObjectManager.UnevaluatedObjects.Single().IsSecurityObject);
        }

        [Fact]
        public void InitializeUnevaluatedObjects_GivenClassWithMethodThatCallsAnotherMethod_CorrectlyPlacedInCalledMethods()
        {
            /// Initialize
            // Create the program text
            var programText = "class A { public void MethodName1 () { } } class B { public A testA; public void MethodName2 () { testA.MethodName1 () } }";

            /// Test
            // Call the MUT
            UnevaluatedObjectManager.InitializeUnevaluatedObjects(programText);

            /// Assert
            // Ensure that the MethodName1 has Methodname2 as a called by method
            Assert.Equal(1, UnevaluatedObjectManager.UnevaluatedObjects.Where(i => i.Name == "A").Single().Methods.Single().CalledByMethods.Count);
        }

        [Fact]
        public void InitializeUnevaluatedObjects_GivenClassWithMethodThatCallsAnotherMethod_CalledByMethodHasCorrectValues()
        {
            /// Initialize
            // Create the program text
            var programText = "class A { public void MethodName1 () { } } class B { public A testA; public void MethodName2 () { testA.MethodName1 () } }";
            var calledByMethodName = "MethodName2";
            var calledByMethodParentObjectName = "B";

            /// Test
            // Call the MUT
            UnevaluatedObjectManager.InitializeUnevaluatedObjects(programText);

            // Get the first object back
            var objectAfter = UnevaluatedObjectManager.UnevaluatedObjects.Where(i => i.Name == "A").Single();

            /// Assert
            // Ensure that the called by method's values are correct.
            Assert.Equal(calledByMethodName, objectAfter.Methods.Single().CalledByMethods.Single().Name);
            Assert.Equal(calledByMethodParentObjectName, objectAfter.Methods.Single().CalledByMethods.Single().ParentObjectName);
        }

        [Fact]
        public void InitializeUnevaluatedObjects_ClassHasMethodDirectlyAffectingSecurityAttribute_MethodIsLabeledCorrectly()
        {
            /// Initialize
            // create the attribute list
            var attributeList = "A.b";
            UnevaluatedObjectManager.SecurityAttributeManager.InitializeSecurityAttributes(attributeList);

            // Create the program text
            var programText = "class A { public bool b; public void Method1 () { b = false; } }";

            /// Test
            // Call the MUT
            UnevaluatedObjectManager.InitializeUnevaluatedObjects(programText);

            /// Assert
            // Ensure that Method1 is marked as directly affects security attribute
            Assert.True(UnevaluatedObjectManager.UnevaluatedObjects.Single().Methods.Single().DirectlyAffectSecurityAttribute);
        }

        [Fact]
        public void InitializeUnevaluatedObjects_ProgramTextContainsMain_MainNotAddedToClassesFunctions()
        {
            /// Initialize
            // Create the program Text
            var programText = "class A { } int main () { }";

            /// Test
            // Call the MUT
            UnevaluatedObjectManager.InitializeUnevaluatedObjects(programText);

            /// Assert
            // Ensure that the main is not added to the class A
            Assert.Equal(0, UnevaluatedObjectManager.UnevaluatedObjects.Single().Methods.Count);
        }
    }
}