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
    
    public partial class Students
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Students()
        {
            this.Fees = new HashSet<Fees>();
            this.Results = new HashSet<Results>();
        }
    
        public int StudentId { get; set; }
        public int UserId { get; set; }
        public decimal RollNumber { get; set; }
        public decimal ContactNumber { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public System.DateTime AdmissionDate { get; set; }
        public int ProgrammeId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Fees> Fees { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Results> Results { get; set; }
        public virtual Users Users { get; set; }
        public virtual Programme Programme { get; set; }
    }
}
