﻿using Dominion.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Labor
{
    public class ServiceRenewFrequency : Entity<ServiceRenewFrequency>
    {
        public virtual int    ServiceRenewFrequencyId   { get; set; }
        public virtual string Description               { get; set; }
    }
}