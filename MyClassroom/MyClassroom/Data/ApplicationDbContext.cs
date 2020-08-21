using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyClassroom.Models;

namespace MyClassroom.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
         
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityRole>()
            .HasData(
            new IdentityRole
            {
            Name = "Admin",
            NormalizedName = "ADMIN"
            },
            new IdentityRole
            {
                Name = "Teacher",
                NormalizedName = "TEACHER"
            },
            new IdentityRole
            {
                Name = "Parent",
                NormalizedName = "PARENT"
            },
            new IdentityRole
            {
                Name = "Student",
                NormalizedName = "STUDENT"
            }
            ) ;
        }

        public DbSet<Models.Teacher> Teachers { get; set; }
        public DbSet<Models.Parent> Parents { get; set; }
        public DbSet<Models.Student> Students { get; set; }
        public DbSet<Models.Homework> Homeworks { get; set; }
        public DbSet<Models.DailyNote> DailyNotes { get; set; }
        public DbSet<Models.Classroom> Classroom { get; set; }
        public DbSet<Models.StudentSkill> StudentSkill { get; set; }
        public DbSet<Models.Skill> Skill { get; set; }




    }
}
