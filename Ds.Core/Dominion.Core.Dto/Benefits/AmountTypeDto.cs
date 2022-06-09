using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Benefits
{
    public class AmountTypeDto
    {
        public string Description { get; set; }
        public int EmployeeDeductionAmountTypeID { get; set; }
        public int DisplayOrder { get; set; }
        public string NumberPrefix { get; set; }
    }
}
