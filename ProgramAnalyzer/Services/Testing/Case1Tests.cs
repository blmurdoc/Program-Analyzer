using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Services
{
    public class Case1Tests
    {
        ObjectManager ObjectManager = new ObjectManager();

        [Fact]
        public void InitializeUnevaluatedCaseOneObjects_UnevaluatedClass_AddedToUnevaluatedObjectsList()
        {
            // Give the program two classes where the second one modifies the secure object's attribute through a method call.
            var programText = "class A { }";

            /// Test
            // Call the method 
            ObjectManager.InitializeUnevaluatedCaseOneObjects(programText);

            // Get the Object count
            var unevaluatedObjectCount = ObjectManager.UnevaluatedObjects.Count;

            /// Assert
            // Ensure that an object is added to the UnevaluatedObjectList
            Assert.Equal(1, unevaluatedObjectCount);
        }
    }
}