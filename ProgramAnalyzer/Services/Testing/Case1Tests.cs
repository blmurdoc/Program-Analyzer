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
        Case1Manager Case1Manager = new Case1Manager();

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
            UnevaluatedObjectManager.UnevaluatedObjects.Add(obj1);
            UnevaluatedObjectManager.UnevaluatedObjects.Add(obj2);

            /// Test
            // Call the MUT
            Case1Manager.InitializeCase1Objects(UnevaluatedObjectManager.UnevaluatedObjects);

            /// Assert
            // Ensure that the obj2 is now in the Case1 Objects
            Assert.Equal(1, Case1Manager.Case1Objects.Count);
        }
        
        [Fact]
        public void InitializeCase1Objects_GivenCase1ObjectWithTwoCase1Methods_SingleCaseOneObjectWithTwoMethodsIsInList()
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
                Name = "CalledByMethod1",
                ParentObjectName = "SecondObject"
            });
            obj1.Methods.Single().CalledByMethods.Add(new CalledByMethod()
            {
                Name = "CalledByMethod2",
                ParentObjectName = "SecondObject"
            });
            var obj2 = new UnevaluatedObject()
            {
                Name = "SecondObject",
                Methods = new List<OwnedMethod>()
            };
            obj2.Methods.Add(new OwnedMethod()
            {
                Name = "CalledByMethod1"
            });
            obj2.Methods.Add(new OwnedMethod()
            {
                Name = "CalledByMethod2"
            });

            // Add the objects to the unevaluated list
            UnevaluatedObjectManager.UnevaluatedObjects.Add(obj1);
            UnevaluatedObjectManager.UnevaluatedObjects.Add(obj2);

            /// Test
            // Call the MUT
            Case1Manager.InitializeCase1Objects(UnevaluatedObjectManager.UnevaluatedObjects);

            /// Assert
            // Ensure that the obj2 is now in the Case1 Objects
            Assert.Equal(1, Case1Manager.Case1Objects.Count);
            Assert.Equal(2, Case1Manager.Case1Objects.Single().MethodNames.Count);
        }
    }
}
