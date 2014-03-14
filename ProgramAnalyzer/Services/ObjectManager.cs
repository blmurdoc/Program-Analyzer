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
        public void InitializeSecurityObjects(string items)
        {
            // Split the items on the comma.
            var splitItems = items.Split(',');

            // Go through the items and create security objects.
            foreach(string s in splitItems)
            {
                // Get the name of the security object and attribute.
                var objects = s.Split('.');
                var securityObjectName = objects[0].Replace(" ", "");
                var attributeName = objects[1].Replace(" ", "");

                // Check if the security object already exists in our list.
                if(CheckIfSecurityObjectExists(securityObjectName))
                {
                    // Find the security object and add the new secure attribute.
                    var obj = SecurityObjects.Where(i => i.Name == securityObjectName).Single();
                    obj.SecureAttributes.Add(new DTOs.Attribute
                        {
                            Name = attributeName,
                            IsSecure = true
                        });
                }
                else
                {
                    // Add the new security object with its object.
                    SecurityObjects.Add(new SecurityObject
                        {
                            Name = securityObjectName,
                            SecureAttributes = new List<DTOs.Attribute>()
                        });
                    var obj = SecurityObjects.Where(i => i.Name == securityObjectName).Single();
                    obj.SecureAttributes.Add(new DTOs.Attribute
                        {
                            Name = attributeName,
                            IsSecure = true
                        });
                }
            }
         }

        /// <summary>
        /// Checks the list of security objects and sees if the given security
        /// object's name exists.
        /// </summary>
        private bool CheckIfSecurityObjectExists(string name)
        {
            foreach(SecurityObject s in SecurityObjects)
                if (s.Name == name)
                    return true;
            return false;
        }
    }
}
