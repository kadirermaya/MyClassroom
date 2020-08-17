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
        public int Id { get; set; }
        [ForeignKey("teacherId")]
        public int TeacherID { get; set; }
        public string Name { get; set; }
        public List<Student>? Students {get; set;}
        public List<Homework>? Homeworks { get; set; }

        
        Teacher teacher = new Teacher();
        


    }
}
