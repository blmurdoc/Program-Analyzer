using Services.DTOs;
using Services.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Services.Testing
{
    public class Case1Tests
    {
        UnevaluatedObjectManager UnevaluatedObjectManager = new UnevaluatedObjectManager();

        [Fact]
        public void InitializeCase1Objects_SingleCase1Object_SuccessfullyAddedToList()
        {
            /// Initialize
            // Create the list of unevaluated objects
            var obj1 = new UnevaluatedObject()
            {
                IsSecurityObject = true,
                Methods = new List<OwnedMethod>(),
                Name = "FirstObject"
            };
            obj1.Methods.Add(new OwnedMethod()
                {
                    DirectlyAffectSecurityAttribute = true,
                    Name = "MethodName",
                    CalledByMethods = new List<CalledByMethod>()
                });
            obj1.Methods.Single().CalledByMethods.Add(new CalledByMethod()
                {
                    Name = "CalledByMethod",
                    ParentObjectName = "SecondObject"
                });
            var obj2 = new UnevaluatedObject()
            {
                Name = "SecondObject",
                Methods = new List<OwnedMethod>()
            };
            obj2.Methods.Add(new OwnedMethod()
                {
                    Name = "CalledByMethod"
                });

            // Add the objects to the unevaluated list
            UnevaluatedObjectManager.Global.UnevaluatedObjects.Add(obj1);
            UnevaluatedObjectManager.Global.UnevaluatedObjects.Add(obj2);

            /// Test
            // Call the MUT
            UnevaluatedObjectManager.Case1Manager.InitializeCase1Objects(UnevaluatedObjectManager.Global);

            /// Assert
            // Ensure that the obj2 is now in the Case1 Objects
            Assert.Equal(1, UnevaluatedObjectManager.Case1Manager.Case1Objects.Count);
        }
        
        [Fact]
        public void InitializeCase1Objects_GivenCase1ObjectWithTwoCase1Methods_SingleCaseOneObjectWithTwoMethodsIsInList()
        {
            /// Initialize
            // Create the attribute list
            var attributeList = "A.b";
            var programText = "class A { public bool b; public void Method1 () { b = false; } } class C { public void Method2 ( A testA ) { testA.Method1 (); } public void Method3 ( A testB ) { testB.Method1 (); } }";

            // Set up the initializer
            UnevaluatedObjectManager.SecurityAttributeManager.InitializeSecurityAttributes(attributeList);
            UnevaluatedObjectManager.InitializeUnevaluatedObjects(programText);

            /// Test
            // Call the MUT
            UnevaluatedObjectManager.Case1Manager.InitializeCase1Objects(UnevaluatedObjectManager.Global);

            /// Assert
            // Ensure that the obj2 is now in the Case1 Objects
            Assert.Equal(1, UnevaluatedObjectManager.Case1Manager.Case1Objects.Count);
            Assert.Equal(2, UnevaluatedObjectManager.Case1Manager.Case1Objects.Single().MethodNames.Count);
        }

        [Fact]
        public void InitializeCase1Objects_GivenCase1Object_ObjectMarkedSemiSecurity()
        {
            /// Initialize
            // Create the list of unevaluated objects
            var obj1 = new UnevaluatedObject()
            {
                IsSecurityObject = true,
                Methods = new List<OwnedMethod>(),
                Name = "FirstObject"
            };
            obj1.Methods.Add(new OwnedMethod()
            {
                DirectlyAffectSecurityAttribute = true,
                Name = "MethodName",
                CalledByMethods = new List<CalledByMethod>()
            });
            obj1.Methods.Single().CalledByMethods.Add(new CalledByMethod()
            {
                Name = "CalledByMethod",
                ParentObjectName = "SecondObject"
            });
            var obj2 = new UnevaluatedObject()
            {
                Name = "SecondObject",
                Methods = new List<OwnedMethod>()
            };
            obj2.Methods.Add(new OwnedMethod()
            {
                Name = "CalledByMethod"
            });

            // Add the objects to the unevaluated list
            UnevaluatedObjectManager.Global.UnevaluatedObjects.Add(obj1);
            UnevaluatedObjectManager.Global.UnevaluatedObjects.Add(obj2);

            /// Test
            // Call the MUT
            UnevaluatedObjectManager.Case1Manager.InitializeCase1Objects(UnevaluatedObjectManager.Global);

            /// Assert
            // Ensure that the object is marked semi-security
            Assert.True(UnevaluatedObjectManager.Global.UnevaluatedObjects.Where(i => i.Name == "SecondObject").Single().IsSemiSecurityObject);
        }
    }
}
