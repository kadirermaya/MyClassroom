﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyClassroom.Models
{
    public class DailyNote
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("StudentId")]
        public int StudentId { get; set; }
        public string? Description { get; set; } 
        public DateTime Date { get; set; }
    }
}
