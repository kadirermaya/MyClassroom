using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyClassroom.Models
{
    public class Classroom
    {
        [Key]
        public int ClassroomId { get; set; }
    }
}
