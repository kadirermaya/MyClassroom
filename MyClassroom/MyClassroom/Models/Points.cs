using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyClassroom.Models
{
    public class Points
    {
        [Key]
        public int PoindId { get; set; }
        [ForeignKey("StudentId")]
        public int StudentId { get; set; }
        [ForeignKey("TeacherId")]
        public int TeacherId { get; set; }
        public string Behavior { get; set; }
        public DateTime Time { get; set; }
    }
}
