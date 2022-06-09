using System;
using Dominion.Core.Dto.Labor;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Entities.TimeClock;

namespace Dominion.Domain.Entities.Labor
{
    public partial class ClockEmployeeExceptionHistory : Entity<ClockEmployeeExceptionHistory>
    {
        public virtual int                 ClockEmployeeExceptionHistoryId { get; set; } 
        public virtual int?                EmployeeId                      { get; set; } 
        public virtual ClockExceptionType? ClockExceptionTypeId            { get; set; } 
        public virtual double?             Hours                           { get; set; } 
        public virtual DateTime?           EventDate                       { get; set; } 
        public virtual int?                ClockClientExceptionDetailId    { get; set; } 
        public virtual int?                ClockEmployeePunchId            { get; set; } 
        public virtual int?                ClockClientLunchId              { get; set; } 
        public virtual int                 ClientId                        { get; set; }
        public virtual int?                ClockEmployeeBenefitID          { get; set; }

        public Employee.Employee          Employee              { get; set; }
        public ClockExceptionTypeInfo     ExceptionTypeInfo     { get; set; }
        public ClockClientExceptionDetail ExceptionRuleDetail   { get; set; }
        public ClockEmployeePunch         EmployeePunch         { get; set; }
        public Client                     Client                { get; set; }
        public ClockClientLunch           LunchRule             { get; set; }
    }
}
