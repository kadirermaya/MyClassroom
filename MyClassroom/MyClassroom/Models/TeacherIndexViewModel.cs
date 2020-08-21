using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyClassroom.Models
{
    public class TeacherIndexViewModel
    {
        public List<Classroom> MyClassrooms { get; set; }
        public Teacher Teacher { get; set; }
        public Classroom Classroom { get; set; }
    }
}
