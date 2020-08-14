using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MyClassroom.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("IdentityUser")]
        public string IdentityUserId { get; set; }
        public IdentityUser IdentityUser { get; set; }
        [ForeignKey("ClassId")]
        public int ClassId { get; set; }
        [ForeignKey("TeacherId")]
        public int ParentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Attendance> Attendances { get; set; }
        public List<Homework> Homeworks { get; set; }
        public List<DailyNote> DailyNotes { get; set; }

    }
}
