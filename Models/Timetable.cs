//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Topscholars.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Timetable
    {
        public int TimetableId { get; set; }
        public Nullable<int> CourseId { get; set; }
        public Nullable<int> FacultyId { get; set; }
        public string DayOfWeek { get; set; }
        public Nullable<System.TimeSpan> StartTime { get; set; }
        public Nullable<System.TimeSpan> EndTime { get; set; }
    
        public virtual Cours Cours { get; set; }
        public virtual Faculty Faculty { get; set; }
    }
}
