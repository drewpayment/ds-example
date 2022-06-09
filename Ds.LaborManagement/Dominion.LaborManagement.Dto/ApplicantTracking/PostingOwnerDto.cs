using Dominion.Core.Dto.Contact.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    [Serializable]
    public partial class PostingOwnerDto : ContactSearchDto
    {
        public int PostingOwnerId { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        public string FullName { get; set; }
        public bool IsEnabled { get; set; }
    }
}