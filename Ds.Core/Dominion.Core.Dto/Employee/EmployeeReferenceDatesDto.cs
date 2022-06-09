using Dominion.Core.Dto.Accruals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Employee
{
    public interface IEmployeeReferenceDatesDto
    {
        DateTime? BirthDate { get; set; }
        DateTime? HireDate { get; set; }
        DateTime? SeparationDate { get; set; }
        DateTime? AnniversaryDate { get; set; }
        DateTime? RehireDate { get; set; }
        DateTime? EligibilityDate { get; set; }
    }

    public class EmployeeReferenceDatesDto : IEmployeeReferenceDatesDto
    {
        public DateTime? BirthDate { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? SeparationDate { get; set; }
        public DateTime? AnniversaryDate { get; set; }
        public DateTime? RehireDate { get; set; }
        public DateTime? EligibilityDate { get; set; }
    }
}
