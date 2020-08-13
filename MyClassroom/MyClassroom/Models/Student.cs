using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyClassroom.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        [ForeignKey("ClassId")]
        public int ClassId { get; set; }
        [ForeignKey("TeacherId")]
        public int TeacherId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Attendance { get; set; }
        public string HomeWork { get; set; }
        public string DailyNotes { get; set; }

    }
}
