﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyClassroom.Models
{
    public class Skill
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public int ClassId { get; set; }
        public int Point { get; set; }
    }
}
