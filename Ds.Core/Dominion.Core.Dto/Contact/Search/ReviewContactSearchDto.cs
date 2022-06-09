using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.Performance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Contact.Search
{
    public class ReviewContactSearchDto : ContactSearchDto
    {
        public ClientDivisionDto Division { get; set; }
        public CoreClientDepartmentDto Department { get; set; }
        public string JobTitle { get; set; }
        public User.UserInfoDto Supervisor { get; set; }
        public DateTime? HireDate { get; set; }
        public string PayType { get; set; }
        public CompetencyModelBasicDto CompetencyModel { get; set; }
    }
}
