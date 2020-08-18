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
        public Student Student { get; set; }
        public Classroom Classroom { get; set; }
        public List<Points> Points { get; set; }
    }
}
