using Services.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Services.Testing
{
    public class Case3Tests
    {
        UnevaluatedObjectManager UnevaluatedObjectManager = new UnevaluatedObjectManager();

        [Fact]
        public void InitializeCase3Objects_GivenCase3Object_ObjectAddedToList()
        {
            /// Initialize
            // Create the attribute list
            var attributeList = "A.b";

            // Create the program text - Needs to have a semi-security object. Will use
            // a case 2 object.
            var programText = "class A { public bool b; } class C { public void Method2 ( A testA ) { testA.b = false; } } class D { public void Method3 ( C testC ) { testC.Method2 (); } } }";

            // Initialize the objects
            UnevaluatedObjectManager.SecurityAttributeManager.InitializeSecurityAttributes(attributeList);
            UnevaluatedObjectManager.InitializeUnevaluatedObjects(programText);
            UnevaluatedObjectManager.Case2Manager.InitializeCase2Objects(UnevaluatedObjectManager.Global);

            /// Test
            // Call the MUT
            UnevaluatedObjectManager.Case3Manager.InitializeCase3Objects(UnevaluatedObjectManager.Global);

            /// Assert
            // Ensure that the class D is in the case 3 objects.
            Assert.Equal(1, UnevaluatedObjectManager.Case3Manager.Case3Objects.Count);
        }

        [Fact]
        public void InitializeCase3Objects_GivenCase3ObjectThatAlreadyExists_ObjectNotDuplicated()
        {
            /// Initialize
            // Create the attribute list
            var attributeList = "A.b";

            // Create the program text - Needs to have a semi-security object. Will use
            // a case 2 object.
            var programText = "class A { public bool b; } class C { public void Method2 ( A testA ) { testA.b = false; } } class D { public void Method3 ( C testC ) { testC.Method2 (); } } }";

            // Initialize the objects
            UnevaluatedObjectManager.SecurityAttributeManager.InitializeSecurityAttributes(attributeList);
            UnevaluatedObjectManager.InitializeUnevaluatedObjects(programText);
            UnevaluatedObjectManager.Case2Manager.InitializeCase2Objects(UnevaluatedObjectManager.Global);

            // Directly add the object before evaluation with another method
            UnevaluatedObjectManager.Case3Manager.Case3Objects.Add(new DTOs.CaseObject()
                {
                    Name = "D",
                    MethodNames = new List<string>()
                });
            UnevaluatedObjectManager.Case3Manager.Case3Objects.Where(i => i.Name == "D").Single().MethodNames.Add("someOtherMethodName");

            /// Test
            // Call the MUT
            UnevaluatedObjectManager.Case3Manager.InitializeCase3Objects(UnevaluatedObjectManager.Global);

            /// Assert
            // Ensure that the object has two methods and there is only one object
            Assert.Equal(1, UnevaluatedObjectManager.Case3Manager.Case3Objects.Count);
            Assert.Equal(2, UnevaluatedObjectManager.Case3Manager.Case3Objects.Single().MethodNames.Count);
        }
        
        [Fact]
        public void InitializeCase3Objects_GivenCase3Object_ObjectMarkedSemiSecurity()
        {
            /// Initialize
            // Create the attribute list
            var attributeList = "A.b";

            // Create the program text - Needs to have a semi-security object. Will use
            // a case 2 object.
            var programText = "class A { public bool b; } class C { public void Method2 ( A testA ) { testA.b = false; } } class D { public void Method3 ( C testC ) { testC.Method2 (); } } }";

            // Initialize the objects
            UnevaluatedObjectManager.SecurityAttributeManager.InitializeSecurityAttributes(attributeList);
            UnevaluatedObjectManager.InitializeUnevaluatedObjects(programText);
            UnevaluatedObjectManager.Case2Manager.InitializeCase2Objects(UnevaluatedObjectManager.Global);

            /// Test
            // Call the MUT
            UnevaluatedObjectManager.Case3Manager.InitializeCase3Objects(UnevaluatedObjectManager.Global);

            /// Assert
            // Ensure that the object is marked semi security.
            Assert.True(UnevaluatedObjectManager.Global.UnevaluatedObjects.Where(i => i.Name == "D").Single().IsSemiSecurityObject);
        }
    }
}
