﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.In
{
    public class AddHomeMemberDto
    {
        public string UserEmail { get; set; }
        public List<string> Permissions { get; set; } = new List<string>();
    }
}
