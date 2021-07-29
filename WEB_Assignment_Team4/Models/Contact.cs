using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB_Assignment_Team4.Models
{

    public class Rootobject
    {
        public Contact[] Property1 { get; set; }
    }

    public class Contact
    {
        public string _id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public string PhoneNumber { get; set; }
        public string SubjectTitle { get; set; }
    }
}
