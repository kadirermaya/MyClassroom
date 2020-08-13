using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MyClassroom.Models
{
    public class Classroom
    {
        [Key]
        public int ClassroomId { get; set; }
        public int ClassName { get; set; }
        [ForeignKey("teacherId")]
        public int TeacherID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }


    }
}
