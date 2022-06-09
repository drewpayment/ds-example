using Dominion.Core.Dto.SftpConfiguration;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Labor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Misc
{
    public class SftpTypeEntity : Entity<SftpTypeEntity>
    {
        public SftpType SftpTypeId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<JobProfileClassifications> JobProfileClassificationsCollection { get; set; }
    }
}
