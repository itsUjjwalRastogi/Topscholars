using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Topscholars.Models
{
    public class StudentModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Programme is required")]
        public int ProgrammeID { get; set; }
        public string Programme { get; set; }
        public decimal RollNumber { get; set;}
        [Required(ErrorMessage = "Contact Number is required")]
        public decimal ContactNumber { get; set; }
        [Required(ErrorMessage = "Date of Birth is required")]
        public DateTime DOB { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string Address {  get; set; }
        public DateTime AdmissionDate { get; set; }
    }
}