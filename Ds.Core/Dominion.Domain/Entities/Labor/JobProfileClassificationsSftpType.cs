using Dominion.Core.Dto.SftpConfiguration;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Labor
{
    public class JobProfileClassificationsSftpType : Entity<JobProfileClassificationsSftpType>
    {
        public int JobProfileId { get; set; }
        public SftpType SftpTypeId { get; set; }
    }
}
