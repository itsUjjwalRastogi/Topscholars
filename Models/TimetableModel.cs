using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Topscholars.Models
{
    public class TimetableModel
    {
        public int ID { get; set; }
        public int FacultyID { get; set; }
        public string FacultyName { get; set; }
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public string Day {  get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}