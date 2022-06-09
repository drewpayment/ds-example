using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Labor
{
    public class ClockEmployee : Entity<ClockEmployee>, IHasModifiedOptionalData
    {
        public virtual int EmployeeId { get; set; }
        public virtual string BadgeNumber { get; set; }
        public virtual string EmployeePin { get; set; }
        public virtual double? AverageWeeklyHours { get; set; }
        public virtual int? TimeZoneId { get; set; }
        public virtual int? ClockClientTimePolicyId { get; set; }
        public virtual bool? IsDayLightSavingsObserved { get; set; }
        public virtual int? ModifiedBy { get; set; }
        public virtual DateTime? Modified { get; set; }
        public virtual int ClientId { get; set; }
        public virtual bool GeofenceEnabled { get; set; }

        //Entity References
        public virtual Client Client { get; set; }
        public virtual Employee.Employee Employee { get; set; }
        public virtual ClockClientTimePolicy TimePolicy { get; set; }
        public virtual ICollection<ClockEmployeePunch> Punches { get; set; }
        public virtual ICollection<ClockEmployeeCostCenter> ClockEmployeeCostCenters { get; set; }

        public ClockEmployee()
        {
        }

        #region Filters

        /// <summary>
        /// Expression that selects the entities which have the specified employee ID.
        /// </summary>
        /// <param name="employeeId">ID of employee to filter by.</param>
        /// <returns></returns>
        public static Expression<Func<ClockEmployee, bool>> ForEmployee(int employeeId)
        {
            return x => x.EmployeeId == employeeId;
        }

        #endregion
    }
}