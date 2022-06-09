using Dominion.Core.Dto.Employee.Search;
using System;

namespace Dominion.Core.Dto.Performance
{
    public class ReviewSearchOptionsDto : EmployeeSearchQueryOptions
    {
        public int? ReviewTemplateId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ReferenceDate? ReferenceDate { get; set; }
    }
}