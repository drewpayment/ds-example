using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Core
{
    public class RemarkChangeHistoryDto
    {
        public int      RemarkId            { get; set; }
        public DateTime Change_Date         { get; set; }
        public Byte     Change_Mode         { get; set; }
        public string   FirstName           { get; set; }
        public string   LastName            { get; set; }
        public bool     IsArchived          { get; set; }
    }
}
