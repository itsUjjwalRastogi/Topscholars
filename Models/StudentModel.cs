﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Topscholars.Models
{
    public class StudentModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Programme { get; set; }
        public int RollNumber { get; set;}
        public int ContantNumber { get; set; }
        public DateTime DOB { get; set; }
        public string Address {  get; set; }
        public DateTime AddmissionDate { get; set; }
    }
}