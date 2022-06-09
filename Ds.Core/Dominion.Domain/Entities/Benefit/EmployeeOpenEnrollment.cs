using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dominion.Benefits.Dto.Employee;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Benefit
{
    /// <summary>
    /// Entity representing a single employee's open enrollment period. Maps to [dbo].[bpEmployeeOpenEnrollment] table.
    /// </summary>
    public class EmployeeOpenEnrollment : Entity<EmployeeOpenEnrollment>, IHasModifiedData
    {
        public virtual int               EmployeeOpenEnrollmentId { get; set; }
        public virtual int               OpenEnrollmentId         { get; set; }
        public virtual int               ClientId                 { get; set; }
        public virtual int               EmployeeId               { get; set; }
        public virtual decimal?          AnnualSalary             { get; set; }
        public virtual SalaryMethodType? SalaryMethodType         { get; set; }
        public virtual bool              IsOpen                   { get; set; }
        public virtual bool              IsSigned                 { get; set; }
        public virtual DateTime?         DateSigned               { get; set; }
        public virtual int               ModifiedBy               { get; set; }
        public virtual DateTime          Modified                 { get; set; }

        public virtual OpenEnrollment    OpenEnrollment { get; set; }
        public virtual Employee.Employee Employee       { get; set; }

        public virtual User.User         ModifiedByUser { get; set; }

        public virtual ICollection<EmployeeOpenEnrollmentSelection> EmployeeSelections { get; set; }

        #region Filters

        /// <summary>
        /// Predicate definition used to limit based on a specific employee open enrollment.
        /// </summary>
        /// <param name="employeeId">The employee id.</param>
        /// <param name="employeeOpenEnrollmentId">The employe open enrollment selelction id.</param>
        /// <returns>Predicate.</returns>
        public static Expression<Func<EmployeeOpenEnrollment, bool>> IsEmployeeOpenEnrollment(int employeeId, 
            int employeeOpenEnrollmentId)
        {
            return x => x.EmployeeId == employeeId &&
                        x.EmployeeOpenEnrollmentId == employeeOpenEnrollmentId;
        }

        #endregion
    }
}