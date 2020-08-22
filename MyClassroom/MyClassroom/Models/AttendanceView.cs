using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyClassroom.Models
{
    public class AttendanceView
    {
        public List<Attendance> Attendances { get; set; }
        public Student Student { get; set; }
        public Parent Parent { get; set; }
    }
}
