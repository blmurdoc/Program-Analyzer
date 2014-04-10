using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Services.Testing
{
    public class SecurityAttributeTests
    {
        SecurityAttributeManager SecurityAttributeManager = new SecurityAttributeManager();

        [Fact]
        public void InitializeSecurityAttributes_AttributeGiven_CorrectlyAddedToList()
        {
            /// Initialize
            // Create the attribute list
            var attributeList = "A.b";

            /// Test
            // Call the MUT
            SecurityAttributeManager.InitializeSecurityAttributes(attributeList);

            /// Assert
            // Ensure that the attribute was added
            Assert.Equal(1, SecurityAttributeManager.SecurityAttributes.Count);
        }

        [Fact]
        public void InitializeSecurityAttributes_AttributeGiven_CoreValuesAreEqual()
        {
            /// Intialize
            // Create the attribute list
            var attributeList = "A.b";
            var objectName = "A";
            var attributeName = "b";

            /// Test
            // Call the MUT
            SecurityAttributeManager.InitializeSecurityAttributes(attributeList);

            /// Assert
            // Ensure that the attribute values were propagated correctly
            Assert.Equal(objectName, SecurityAttributeManager.SecurityAttributes.Single().ParentObjectName);
            Assert.Equal(attributeName, SecurityAttributeManager.SecurityAttributes.Single().Name);
        }

        [Fact]
        public void InitializeSecurityAttributes_MultipleAttributesGiven_BothAddedToList()
        {
            /// Initialize
            // Create the attribute list
            var attributeList = "A.b, C.d";

            /// Test
            // Call the MUT
            SecurityAttributeManager.InitializeSecurityAttributes(attributeList);

            /// Assert
            // Ensure that both attributes are in the list
            Assert.Equal(2, SecurityAttributeManager.SecurityAttributes.Count);
        }

        [Fact]
        public void InitializeSecurityAttributes_RepeatedEntry_NotAddedToList()
        {
            /// Initialize
            // Create the attribute list
            var attributeList = "A.b, A.b";

            /// Test
            // Call the MUT
            SecurityAttributeManager.InitializeSecurityAttributes(attributeList);

            /// Assert
            // Ensure that only one attribute/object is added
            Assert.Equal(1, SecurityAttributeManager.SecurityAttributes.Count);
        }
    }
}
