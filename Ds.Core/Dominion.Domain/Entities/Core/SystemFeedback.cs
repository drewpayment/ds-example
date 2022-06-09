using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Core.Dto.Core;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Entities.Core
{
    public class SystemFeedback : Entity<SystemFeedback>
    {
        public int SystemFeedbackId { get; set; }
        public int RemarkId { get; set; }
        public SystemFeedbackType SystemFeedbackTypeID { get; set; }
        public int ClientId { get; set; }

        // RELATIONSHIPS
        public virtual Remark Remark { get; set; }
        public virtual Client Client { get; set; }
    }
}
