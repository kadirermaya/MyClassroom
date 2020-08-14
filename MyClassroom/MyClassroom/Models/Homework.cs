using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyClassroom.Models
{
    public class Homework
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("ClassId")]
        public int ClassId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

    }
}
