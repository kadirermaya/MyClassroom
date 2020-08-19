using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyClassroom.Models
{
    public class TeacherStudenViewModel
    {
        public Student Student { get; set; }
        public Teacher Teacher { get; set; }
        public Parent  Parent { get; set; }
        public Classroom Classroom { get; set; }
        public List<string> Positive { get; set; }
        public List<string> NeedsWork { get; set; }
             

    }
}
