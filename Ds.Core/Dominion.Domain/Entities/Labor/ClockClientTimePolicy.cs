using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Misc;
using Dominion.Domain.Entities.TimeClock;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Labor
{
    public class ClockClientTimePolicy : Entity<ClockClientTimePolicy>, IHasModifiedData
    {
        public int      ClockClientTimePolicyId { get; set; } 
        public string   Name                    { get; set; } 
        public int      ClientId                { get; set; } 
        public byte     ClientStatusId          { get; set; } 
        public byte     PayType                 { get; set; } 
        public int?     ClockClientRulesId      { get; set; } 
        public int?     ClockClientExceptionId  { get; set; }
        public int?     ClockClientHolidayId    { get; set; } 
        public int      ModifiedBy              { get; set; } 
        public DateTime Modified                { get; set; } 
        public int?     ClientDepartmentId      { get; set; } 
        public int?     ClientShiftId           { get; set; } 
        public int?     TimeZoneId              { get; set; } 
        public bool     IsAddToOtherPolicy      { get; set; }
        public bool     HasCombinedOtFrequencies { get; set; }
        public bool     AutoPointsEnabled       { get; set; }
        public bool     ShowTCARatesEnabled     { get; set; }
        public bool     GeofenceEnabled         { get; set; }

        //Entity References
        public virtual ClockClientHoliday            Holidays   { get; set; }
        public virtual ClientDepartment              Department { get; set; }
        public virtual ClockClientRules              Rules      { get; set; }
        public virtual ClockClientException          Exceptions { get; set; }
        public virtual ClientShift                   Shift      { get; set; }
        public virtual Misc.TimeZone                 TimeZone   { get; set; }

        public virtual ICollection<ClockClientLunchSelected>            LunchSelected    { get; set; }

        [Obsolete("Use LunchSelected, this can be removed if not used in DominionSource projects.")]
        public virtual ICollection<ClockClientLunch>                    Lunches          { get; set; }
        public virtual ICollection<ClockClientAddHoursSelected>         AddHoursSelected { get; set; }
        public virtual ICollection<ClockClientAddHours>                 AddHours         { get; set; }
        public virtual ICollection<ClockClientOvertimeSelected>         OvertimeSelected { get; set; }
        public virtual ICollection<ClockClientOvertime>                 Overtimes        { get; set; }
        public virtual ICollection<ClientShiftSelected>                 ShiftSelected    { get; set; }
        public virtual ICollection<ClientShift>                         Shifts           { get; set; }
        

        //enum
        public TimeZones TimeZoneName => (TimeZones)(TimeZoneId ?? 1);

        


        #region Filters

        /// <summary>
        /// Expression that selects the entities which have the specified client ID.
        /// </summary>
        /// <param name="clientId">ID of client to filter by.</param>
        /// <returns></returns>
        public static Expression<Func<ClockClientTimePolicy, bool>> ForClient(int clientId)
        {
            return x => x.ClientId == clientId;
        }

        #endregion
    }
}