﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Misc
{
    public class EffectiveDateDto
    {
        public int EffectiveId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string Table { get; set; }
        public string Column { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string AppliedBy { get; set; }
        public DateTime? AppliedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int Type { get; set; }
        public int TablePkId { get; set; }
        public string FriendlyView { get; set; }
        public int? Accepted { get; set; }
        public int EmployeeId { get; set; }
        public string Datatype { get; set; }
    }
}