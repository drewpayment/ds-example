using Dominion.Core.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Performance
{
    public class ApprovalProcessHistoryDto
    {
        public int             ApprovalProcessHistoryId { get; set; }
        public int             ToUserId            { get; set; } 
        public int?            FromUserId          { get; set; }
        public int             EvaluationId        { get; set; }
        public ApprovalProcess Action              { get; set; }


        public UserDto ToUser { get; set; }
        public UserDto FromUser { get; set; }
        public EvaluationDto Evaluation { get; set; }
    }
}
