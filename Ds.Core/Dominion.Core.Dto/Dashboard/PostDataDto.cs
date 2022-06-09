using Dominion.Core.Dto.TimeCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Dominion.Core.Dto.Dashboard
{
    public class PostDataDto
    {
        public int ClientId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<int> EmployeeIds { get; set; }
        public int CurrentPayrollID { get; set; }
        public int userId { get; set; }
        public int userTypeId { get; set; }
    }
}
