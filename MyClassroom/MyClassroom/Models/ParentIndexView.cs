using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyClassroom.Models
{
    public class ParentIndexView
    {
        public Student Student { get; set; }
        public Parent Parent { get; set; }
        public Teacher Teacher { get; set; }
        public List<Attendance>? Attendances { get; set; }
        public Homework Homeworks { get; set; }
        public List<StudentSkill>? StudentSkills { get; set; }
        public DailyNote? DailyNote { get; set; }


    }
}
