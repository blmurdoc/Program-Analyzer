using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Services;

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

        [Fact(Skip="Not implemented")]
        public void InitializeSecurityObjects_PassedMultipleValuesWithSameSecurityObject_ResultsShownInSecurityObjectsList()
        {
            /// Initialize
            // 

            /// Test
            //

            /// Assert
            //

        }
    }
}