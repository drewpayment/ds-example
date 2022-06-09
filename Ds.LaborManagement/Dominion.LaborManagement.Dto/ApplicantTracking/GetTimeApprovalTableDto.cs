using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class GetTimeApprovalTableDto
    {
        public DataTable Table { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
