namespace MyClassroom.Models
{
    using System.Collections.Generic;

    public class TeacherStudenViewModel
    {
        public Student Student { get; set; }

        public Teacher Teacher { get; set; }

        public Parent Parent { get; set; }

        public Classroom Classroom { get; set; }
        public DailyNote DailyNote { get; set; }

        public List<StudentSkill> StudentSkills { get; set; }
        public List<Skill> Skills { get; set; }
    }
}
