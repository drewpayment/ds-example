using System;
using Dominion.Core.Dto.Labor;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;
using Dominion.LaborManagement.Dto.Clock;

namespace Dominion.Domain.Entities.TimeClock
{
    public class ClockClientExceptionDetail : Entity<ClockClientExceptionDetail>, IHasModifiedData
    {
        public int                ClockClientExceptionDetailId { get; set; }
        public int                ClockClientExceptionId       { get; set; }
        public ClockExceptionType ClockExceptionId         { get; set; }
        public double?            Amount                       { get; set; }
        public bool?              IsSelected                   { get; set; }
        public int                ModifiedBy                   { get; set; }
        public DateTime           Modified                     { get; set; }
        public int?               ClockClientLunchId           { get; set; }
        public PunchType?         PunchTimeOption              { get; set; }

        //refernce entities
        public virtual ClockExceptionTypeInfo Exception                { get; set; }
        public virtual ClockClientException   ClientException          { get; set; }
        public virtual ClockClientLunch       Lunch                    { get; set; }
    }
}