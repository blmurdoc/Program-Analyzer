using Services.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Services.Testing
{
    public class Case5Tests
    {
        public UnevaluatedObjectManager UnevaluatedObjectManager = new UnevaluatedObjectManager();

        [Fact]
        public void InitializeCase5Objects_GivenCase5Object_ObjectIsInList()
        {
            /// Initialize
            // Create an attribute list
            var securityAttribute = "A.b";

            // A case 5 object
            var programText = "class A { public bool b; public void method1 () { b = false; } public void method2 () { method1 (); } }";

            // Initialize the objects 
            UnevaluatedObjectManager.SecurityAttributeManager.InitializeSecurityAttributes(securityAttribute);
            UnevaluatedObjectManager.InitializeUnevaluatedObjects(programText);

            /// Test
            // Call the Method Under Test (MUT)
            UnevaluatedObjectManager.Case5Manager.InitializeCase5Objects(UnevaluatedObjectManager.Global);

            /// Assert
            // Ensure that object is added to the list
            Assert.Equal(1, UnevaluatedObjectManager.Case5Manager.Case5Objects.Count);
        }

        [Fact]
        public void InitializeCase5Objects_GivenCase5ObjectThatAlreadyExists_ObjectNotDuplicated()
        {
            /// Initialize
            // Create an attribute list
            var securityAttribute = "A.b";

            // A case 5 object
            var programText = "class A { public bool b; public void method1 () { b = false; } public void method2 () { method1 (); } public void method3 () { method1 (); } }";

            // Initialize the objects 
            UnevaluatedObjectManager.SecurityAttributeManager.InitializeSecurityAttributes(securityAttribute);
            UnevaluatedObjectManager.InitializeUnevaluatedObjects(programText);

            /// Test
            // Call the Method Under Test (MUT)
            UnevaluatedObjectManager.Case5Manager.InitializeCase5Objects(UnevaluatedObjectManager.Global);

            /// Assert
            // Ensure that case5 object isnt duplicated
            Assert.Equal(2, UnevaluatedObjectManager.Case5Manager.Case5Objects.Single().MethodNames.Count);
        }
    [Fact]
        public void InitialzeCase5Object_GivenCase5Object_MarkedAsSemiSecurity()
        {
            /// Initialize
            // Create an attribute list
            var securityAttribute = "A.b";

            // A case 5 object
            var programText = "class A { public bool b; public void method1 () { b = false; } public void method2 () { method1 (); } }";

            // Initialize the objects 
            UnevaluatedObjectManager.SecurityAttributeManager.InitializeSecurityAttributes(securityAttribute);
            UnevaluatedObjectManager.InitializeUnevaluatedObjects(programText);

            /// Test
            // Call the Method Under Test (MUT)
            UnevaluatedObjectManager.Case5Manager.InitializeCase5Objects(UnevaluatedObjectManager.Global);

            /// Assert
            // Ensure that case5 object is marked as semiSecurity
            Assert.True(UnevaluatedObjectManager.Global.UnevaluatedObjects.Where(i => i.Name == "A").Single().IsSemiSecurityObject);
        }
    }
}