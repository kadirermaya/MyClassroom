namespace MyClassroom.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class StudentSkill
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("StudentId")]
        public int StudentId { get; set; }
        [ForeignKey("SkillId")]
        public int SkillId { get; set; }
        [ForeignKey("ClassId")]
        public int ClassId { get; set; }
        public string Description { get; set; }
        public int Point { get; set; }
        public DateTime Date { get; set; }
    }
}
