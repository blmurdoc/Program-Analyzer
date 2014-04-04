using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Services;
using Services.DTOs;

namespace Services.Testing
{
    public class ObjectManagerTests
    {
        ObjectManager ObjectManager = new ObjectManager();

        [Fact]
        public void InitializeSecurityObjects_Passed1SecurityObjectAndAttribute_ResultsShownInSecurityObjectsList()
        {
            /// Initialize
            // Create a list containing one item.
            var singleItem = "A.b";

            /// Test
            // Initialize the security objects.
            ObjectManager.InitializeSecurityObjects(singleItem);

            /// Assert
            // Ensure that the SecurityObject list contains a single element.
            Assert.Equal(1, ObjectManager.SecurityObjects.Count);
        }

        [Fact]
        public void InitializeSecurityObjects_Passed1SecurityObjectAndAttribute_SecurityObjectValuesPersist()
        {
            /// Initialize
            // Create a list containing one item.
            var singleItem = "A.b";

            // Keep the security Object name.
            var securityObjectName = "A";

            /// Test
            // Initialize the security objects.
            ObjectManager.InitializeSecurityObjects(singleItem);

            /// Assert
            // Ensure that the security object values persist.
            Assert.Equal(securityObjectName, ObjectManager.SecurityObjects.Single().Name);
        }

        [Fact]
        public void InitializeSecurityObjects_Passed1SecurityObjectAndAttribute_AttributeValuesPersist()
        {
            /// Initialize
            // Create a list containing one item and keep the item name.
            var singleItem = "A.b";
            var attribute = "b";

            /// Test
            // Initialize the security objects.
            ObjectManager.InitializeSecurityObjects(singleItem);

            /// Assert
            // Ensure that the attribute vaules persist.
            Assert.Equal(attribute, ObjectManager.SecurityObjects.Single().SecureAttributes.Single().Name);
            Assert.True(ObjectManager.SecurityObjects.First().SecureAttributes.Single().IsSecure);
        }

        [Fact]
        public void InitializeSecurityObjects_PassedMultipleObjects_ResultsShownInSecurityObjectsList()
        {
            /// Initialize
            // Create a list containing multiple items.
            var items = "A.b, C.d";
      
            /// Test
            // Initialize the security objects.
            ObjectManager.InitializeSecurityObjects(items);

            /// Assert
            // Ensure that the SecurityObject list contains the same number of elements.
            Assert.Equal(items.Split(',').Count(), ObjectManager.SecurityObjects.Count);
        }

        [Fact]
        public void InitializeSecurityObjects_PassedMultipleValuesWithSameSecurityObject_ResultsShownInSecurityObjectsList()
        {
            /// Initialize
            // Create a list containing one security object with multiple secure attributes.
            var items = "A.b, A.c";

            /// Test
            // Initialize the security objects.
            ObjectManager.InitializeSecurityObjects(items);

            /// Assert
            // Ensure that one security object exists.
            Assert.Equal(1, ObjectManager.SecurityObjects.Count);
        }

        [Fact]
        public void InitializeSecurityObjectsMethods_ProgramContainsMethodThatAffectsAttribute_MethodIsAddedToSecurityObjectsMethods()
        {
            /// Initialize
            // Set the security object to one object with one attribute.
            var item = "A.b";
            ObjectManager.InitializeSecurityObjects(item);

            // Create a method that will belong to A's affected methods.
            var methodToTest = new Method()
                {
                    Name = "Changeb"
                };

            // Give a program that contains the security object with a method that 
            // changes the method's attribute.
            var programText = "class A { private boolean b; public void Changeb () { b = false; } }";

            /// Test
            // Call the method initializer.
            ObjectManager.InitializeSecurityObjectMethods(programText);

            /// Assert
            // Ensure that the method is now in A's affected methods.
            Assert.Equal(methodToTest.Name, ObjectManager.SecurityObjects.Single().AffectSecureAttributesMethods.Single().Name);
        }

        [Fact]
        public void InitializeSecurityObjectsMethods_ProgramContainsOneMethodThatAffects2Attributes_MethodHasBothAttributes()
        {
            /// Initialize
            // Set the security object to one object with 2 attributes.
            var items = "A.b, A.c";
            ObjectManager.InitializeSecurityObjects(items);

            // Give the program a method that changes both attributes.
            var programText = "class A { private boolean b; private boolean c; public void ChangeAttr () { b = false; c = false; } }";

            /// Test
            // Call the method initializer.
            ObjectManager.InitializeSecurityObjectMethods(programText);

            // Get the attributes
            var attr1AfterTest = ObjectManager.SecurityObjects.Single().AffectSecureAttributesMethods.Single().AccessedAttributes.Where(i => i.Name == "b").Count();
            var attr2AfterTest = ObjectManager.SecurityObjects.Single().AffectSecureAttributesMethods.Single().AccessedAttributes.Where(i => i.Name == "c").Count();

            /// Assert
            // Ensure that both attributes belong to the method.
            Assert.Equal(1, attr1AfterTest);
            Assert.Equal(1, attr2AfterTest);
        }

    }
}