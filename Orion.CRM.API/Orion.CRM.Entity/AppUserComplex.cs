﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Orion.CRM.Entity
{
    public class AppUserComplex
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string RealName { get; set; }
        public int ProjectId { get; set; }
    }
}
