﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class topscholarsEntities : DbContext
    {
        public topscholarsEntities()
            : base("name=topscholarsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Assignments> Assignments { get; set; }
        public virtual DbSet<Courses> Courses { get; set; }
        public virtual DbSet<Documents> Documents { get; set; }
        public virtual DbSet<Faculty> Faculty { get; set; }
        public virtual DbSet<Feedback> Feedback { get; set; }
        public virtual DbSet<Fees> Fees { get; set; }
        public virtual DbSet<Results> Results { get; set; }
        public virtual DbSet<Students> Students { get; set; }
        public virtual DbSet<Timetable> Timetable { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        public System.Data.Entity.DbSet<Topscholars.Models.TimetableModel> TimetableModels { get; set; }
    }
}
