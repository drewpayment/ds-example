﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.Dto;

namespace Dominion.Core.Dto.Payroll
{
    public class PayGradeDto : DtoObject
    {
        public virtual int PayGradeID { get; set; } //pk
        public virtual string Name { get; set; }
        public virtual string Code { get; set; }
        public virtual decimal Minimum { get; set; }
        public virtual decimal Middle { get; set; }
        public virtual decimal Maximum { get; set; }
        public virtual PayGradeType Type { get; set; }
        public virtual int ClientID { get; set; } //fk
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime Modified { get; set; }
    }
}