using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ObjectManager
    {
        // Initialize the lists.
        public List<SecurityObject> SecurityObjects = new List<SecurityObject>();
        
        /// <summary>
        /// The attributes list will be in the form:
        /// 
        ///     A.b, C.d
        ///     
        /// where A and C are security objects and b and d are the attributes
        /// that contains secure information.
        /// 
        /// This will create the Security Objects with the attributes
        /// in place.
        /// </summary>
        public void InitializeSecurityObjects(string attributes)
        {
            throw new NotImplementedException();
            //// Create a list of security objects where each entry is of the form:
            //// A.c
            //// where A is a security object and c is the secure attribute.
            //List<string> objectsAndAttributes = attributes.Replace(" ", "").Split(',').ToList();

            //// Go through the list and add to the SecurityObjects.
            //for (int i = 0; i < objectsAndAttributes.Count; i++)
            //{
            //    // Set objects
            //}
        }
    }
}
