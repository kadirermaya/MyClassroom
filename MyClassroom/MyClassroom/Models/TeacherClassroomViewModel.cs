using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyClassroom.Models
{
    public class TeacherClassroomViewModel
    {
       
        public List<Student> Students { get; set; }
        public List<Student> AllStudents { get; set; }
        public Teacher Teacher { get; set; }
        public Classroom Classroom { get; set; }
        public List<StudentSkill> StudentSkills { get; set; }
        public Homework Homework { get; set; }
    }
}
