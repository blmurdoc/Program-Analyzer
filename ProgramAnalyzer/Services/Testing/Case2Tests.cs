using Services.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Services.Testing
{
    public class Case2Tests
    {
        UnevaluatedObjectManager UnevaluatedObjectManager = new UnevaluatedObjectManager();

        [Fact]
        public void InitializeCase2Objects_Given1Case2Object_SuccessfullyAddedToList()
        {
            /// Initialize
            // Create the attributeList
            var attributeList = "A.b";

            // Create the programText
            var programText = "class A { public bool b; } class C { public void Method ( A securityObject ) { securityObject.b = false; } }";

            // Initialize the objects
            UnevaluatedObjectManager.SecurityAttributeManager.InitializeSecurityAttributes(attributeList);
            UnevaluatedObjectManager.InitializeUnevaluatedObjects(programText);

            /// Test
            // Call the MUT
            UnevaluatedObjectManager.Case2Manager.InitializeCase2Objects(UnevaluatedObjectManager.Global);

            /// Assert
            // Ensure that the object is in the list
            Assert.Equal(1, UnevaluatedObjectManager.Case2Manager.Case2Objects.Count);
        }

        [Fact]
        public void InitializeCase2Objects_Given1Case2ObjectWith2Methods_SuccessfullyAddedToList()
        {
            /// Initialize
            // Create the attributeList
            var attributeList = "A.b";

            // Create the programText
            var programText = "class A { public bool b; } class C { public void Method ( A securityObject ) { securityObject.b = false; } public void Method2 (A securityObject ) { securityObject.b = true; } }";

            // Initialize the objects
            UnevaluatedObjectManager.SecurityAttributeManager.InitializeSecurityAttributes(attributeList);
            UnevaluatedObjectManager.InitializeUnevaluatedObjects(programText);

            /// Test
            // Call the MUT
            UnevaluatedObjectManager.Case2Manager.InitializeCase2Objects(UnevaluatedObjectManager.Global);

            /// Assert
            // Ensure that the object has both methods.
            Assert.Equal(2, UnevaluatedObjectManager.Case2Manager.Case2Objects.Single().MethodNames.Count);
        }
    }
}