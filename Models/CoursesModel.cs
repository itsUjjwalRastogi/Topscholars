using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Topscholars.Models
{
    public class CoursesModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Programme { get; set; }
        public int Credit { get; set; }
        public int FacultyID { get; set; }
        public string FacultyName { get; set; }
    }
}