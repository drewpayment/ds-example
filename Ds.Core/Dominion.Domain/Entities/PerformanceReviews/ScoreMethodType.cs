﻿using Dominion.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    public class ScoreMethodType : IHasModifiedData
    {
        public byte ScoreMethodTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
        public User.User ModifiedByUser { get; set; }
    }
}