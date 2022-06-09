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
    public class FieldTypeDto
    {
        public int FieldTypeId { get; set; }
        public String FieldType { get; set; }    }
}