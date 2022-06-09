using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.Dto;

namespace Dominion.Core.Dto.Employee
{
    public class EmployeeTerminationReasonDto : DtoObject
    {
        public int? Id { get; set; }
        public int? TypeId { get; set; }
        public string Description { get; set; }
    }
}
