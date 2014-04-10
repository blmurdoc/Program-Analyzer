using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class SecurityAttributeManager
    {
        public List<SecurityAttribute> SecurityAttributes = new List<SecurityAttribute>();

        /// <summary>
        /// Sets up the list of attributes as objects.
        /// Assumes input is in the form:
        ///     A.b, C.d
        /// Where A and C are classes and b and d are the security attributes
        /// </summary>
        public void InitializeSecurityAttributes(string attributes)
        {
            // Split the attributes on commas to separate each one.
            string[] delimiters1 = new string[2];
            delimiters1[0] = ",";
            delimiters1[1] = " ";
            string[] delimiters2 = new string[2];
            delimiters2[0] = ".";
            delimiters2[1] = " ";
            var separateAttributes = attributes.Split(delimiters1, StringSplitOptions.RemoveEmptyEntries);

            // Go through each attribute and create the class
            foreach(string s in separateAttributes)
            {
                // Split the entry on the '.' to separate object and attribute
                var objectAttributeSplit = s.Split(delimiters2, StringSplitOptions.RemoveEmptyEntries);

                // Create the attribute object
                var attribute = new SecurityAttribute()
                {
                    Name = objectAttributeSplit[1],
                    ParentObjectName = objectAttributeSplit[0]
                };
                
                // Make sure this isn't a duplicate
                bool isDuplicate = false;
                foreach(SecurityAttribute sa in SecurityAttributes)
                    if (sa.Name == attribute.Name && sa.ParentObjectName == attribute.ParentObjectName)
                        isDuplicate = true;

                // Insert the attribute into the list
                if(!isDuplicate)
                    SecurityAttributes.Add(attribute);
            }
        }
    }
}
