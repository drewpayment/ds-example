using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Core
{
    public class SystemFeedbackDto
    {
        public int SystemFeedbackId { get; set; }
        public SystemFeedbackType SystemFeedbackType { get; set; }
        public int RemarkId { get; set; }
        public int ClientId { get; set; }
        public RemarkDto Remark { get; set; }
    }
}
