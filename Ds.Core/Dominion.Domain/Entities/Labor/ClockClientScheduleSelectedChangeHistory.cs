using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Common;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Labor
{
    public partial class ClockClientScheduleSelectedChangeHistory : Entity<ClockClientScheduleSelectedChangeHistory>,
        IHasChangeHistoryDataWithEnum,
        IClockClientScheduleSelectedEntity,
        IHasModifiedData
    {
        private string _changeMode;
        public virtual int EmployeeId { get; set; }
        public virtual int ClockClientScheduleId { get; set; }
        public virtual int ModifiedBy { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual int ClientId { get; set; }
        public int ChangeId { get; set; }
        public ChangeModeType ChangeMode { get; set; }
        public DateTime ChangeDate { get; set; }
    }
}
