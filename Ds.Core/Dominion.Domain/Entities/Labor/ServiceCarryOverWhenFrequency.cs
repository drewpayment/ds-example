﻿using Dominion.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Labor
{
    public class ServiceCarryOverWhenFrequency : Entity<ServiceCarryOverWhenFrequency>
    {
        public virtual int    ServiceCarryOverWhenFrequencyId   { get; set; }
        public virtual string Description                       { get; set; }
    }
}