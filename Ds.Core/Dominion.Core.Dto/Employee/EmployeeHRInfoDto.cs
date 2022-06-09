using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.Contact;
using Dominion.Core.Dto.Core;
using Dominion.Core.Dto.Payroll;
using Dominion.Taxes.Dto;
using Dominion.Utility.OpResult;

namespace Dominion.Core.Dto.Employee
{
    public class EmployeeHRInfoDto
    {
        public int EmployeeHRInfoId { get; set; }
        public int ClientHRInfoId { get; set; }
        public int EmployeeId { get; set; }
        public String Value { get; set; }
        public virtual DateTime? Modified { get; set; }
        public virtual int? ModifiedBy { get; set; }
        public virtual int ClientId { get; set; }
        public virtual bool? InsertApproved { get; set; }
        public ClientHRInfoDto ClientHRInfo { get; set; }
    }
}