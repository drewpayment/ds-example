using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Employee
{
    public class DominionShortcutDto
    {
        public bool ShowPayCheck { get; set; }
        public bool ShowW2 { get; set; }
        public bool ShowACA { get; set; }
        public bool ShowLeaveManagement { get; set; }
        public bool ShowCheckList { get; set; }
        public bool ShowMyEss { get; set; }
        public string LeaveManagementToolTip { get; set; }
        public string LeaveManagementUrl { get; set; }
        public string CheckListToolTip { get; set; }
        public string AttachmentsUrl { get; set; }
        public string AttachmentsTitle { get; set; }
        public string MyEssUrl { get; set; }
        public int GenPaycheckHistoryId { get; set; }
        public string ToDoUrl { get; set; }
    }
}
