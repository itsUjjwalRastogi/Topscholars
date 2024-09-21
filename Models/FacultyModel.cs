using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Topscholars.Models
{
    public class FacultyModel
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public DateTime JoiningDate { get; set; }
    }
}