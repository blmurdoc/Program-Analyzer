using Services.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Services.Testing
{
    public class Case6Tests
    {
        UnevaluatedObjectManager UnevaluatedObjectManager = new UnevaluatedObjectManager();

        [Fact]
        public void InitializeCase6Objects_GivenCase6ObjectForCriteria1_ObjectAddedToList()
        {
            /// Initialize
            // Create the attribute list
            var attributeList = "A.b";

            // Create the program text
            var programText = "class A { public bool b; public void method1 () { b = false; } } class B { public void method2 ( A testA ) { testA.method1 (); } public void method3 () { method2 (); } }";

            // Initialize the objects
            UnevaluatedObjectManager.SecurityAttributeManager.InitializeSecurityAttributes(attributeList);
            UnevaluatedObjectManager.InitializeUnevaluatedObjects(programText);
            UnevaluatedObjectManager.Case1Manager.InitializeCase1Objects(UnevaluatedObjectManager.Global);

            /// Test
            // Call the MUT
            UnevaluatedObjectManager.Case6Manager.InitializeCase6Objects(UnevaluatedObjectManager.Global);

            /// Assert
            // Ensure that the object was added to the list
            Assert.Equal(1, UnevaluatedObjectManager.Case6Manager.Case6Objects.Count);
        }

        [Fact]
        public void InitializeCase6Objects_GivenCase6ObjectForCriteria1AlreadyInList_ObjectNotDuplicated()
        {
            /// Initialize
            // Create the attribute list
            var attributeList = "A.b";

            // Create the program text
            var programText = "class A { public bool b; public void method1 () { b = false; } } class B { public void method2 ( A testA ) { testA.method1 (); } public void method3 () { method2 (); } public void method4 () { method2 (); } }";

            // Initialize the objects
            UnevaluatedObjectManager.SecurityAttributeManager.InitializeSecurityAttributes(attributeList);
            UnevaluatedObjectManager.InitializeUnevaluatedObjects(programText);
            UnevaluatedObjectManager.Case1Manager.InitializeCase1Objects(UnevaluatedObjectManager.Global);

            /// Test
            // Call the MUT
            UnevaluatedObjectManager.Case6Manager.InitializeCase6Objects(UnevaluatedObjectManager.Global);

            /// Assert
            // Ensure that the object was not duplicated
            Assert.Equal(2, UnevaluatedObjectManager.Case6Manager.Case6Objects.Single().MethodNames.Count);
        }

        [Fact]
        public void InitializeCase6Objects_GivenCase6ObjectForCriteria1_ObjectMarkedSemiSecurity()
        {
            /// Initialize
            // Create the attribute list
            var attributeList = "A.b";

            // Create the program text
            var programText = "class A { public bool b; public void method1 () { b = false; } } class B { public void method2 ( A testA ) { testA.method1 (); } public void method3 () { method2 (); } }";

            // Initialize the objects
            UnevaluatedObjectManager.SecurityAttributeManager.InitializeSecurityAttributes(attributeList);
            UnevaluatedObjectManager.InitializeUnevaluatedObjects(programText);
            UnevaluatedObjectManager.Case1Manager.InitializeCase1Objects(UnevaluatedObjectManager.Global);

            /// Test
            // Call the MUT
            UnevaluatedObjectManager.Case6Manager.InitializeCase6Objects(UnevaluatedObjectManager.Global);

            /// Assert
            // Ensure that the object was marked semi security
            Assert.True(UnevaluatedObjectManager.Global.UnevaluatedObjects.Where(i => i.Name == "B").Single().IsSemiSecurityObject);
        }
    }
}
