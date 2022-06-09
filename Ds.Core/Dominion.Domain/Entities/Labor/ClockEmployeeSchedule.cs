using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Labor
{
    public class ClockEmployeeSchedule : Entity<ClockEmployeeSchedule>, IHasModifiedOptionalData
    {
        public virtual int          ClockEmployeeScheduleId                      { get; set; } 
        public virtual DateTime     EventDate                                    { get; set; } 
        public virtual int          EmployeeId                                   { get; set; } 
        public virtual int          ClientId                                     { get; set; } 
        public virtual int?         ClockClientTimePolicyId                      { get; set; } 
        public virtual int?         ClockClientScheduleChangeHistoryChangeId     { get; set; } 

        public virtual DateTime?    StartTime1                                   { get; set; } 
        public virtual DateTime?    EndTime1                                     { get; set; } 
        public virtual int?         GroupScheduleShiftId1                        { get; set; } 
        public virtual int?         ClientDepartmentId1                          { get; set; } 
        public virtual int?         ClientCostCenterId1                          { get; set; } 

        public virtual DateTime?    StartTime2                                   { get; set; } 
        public virtual DateTime?    EndTime2                                     { get; set; } 
        public virtual int?         GroupScheduleShiftId2                        { get; set; } 
        public virtual int?         ClientDepartmentId2                          { get; set; } 
        public virtual int?         ClientCostCenterId2                          { get; set; } 

        public virtual DateTime?    StartTime3                                   { get; set; } 
        public virtual DateTime?    EndTime3                                     { get; set; } 
        public virtual int?         GroupScheduleShiftId3                        { get; set; } 
        public virtual int?         ClientDepartmentId3                          { get; set; } 
        public virtual int?         ClientCostCenterId3                          { get; set; } 

        public virtual DateTime?    Modified                                     { get; set; } 
        public virtual int?         ModifiedBy                                   { get; set; } 

        //Entity References
        public virtual Employee.Employee  Employee                       { get; set; } 
        public virtual ClientCostCenter ClientCostCenter1 { get; set; }
        public virtual ClientCostCenter ClientCostCenter2 { get; set; }
        public virtual ClientCostCenter ClientCostCenter3 { get; set; }
        public virtual ClientDepartment ClientDepartment1 { get; set; }
        public virtual ClientDepartment ClientDepartment2 { get; set; }
        public virtual ClientDepartment ClientDepartment3 { get; set; }
        public virtual GroupScheduleShift GroupScheduleShift1           { get; set; } 
        public virtual GroupScheduleShift GroupScheduleShift2           { get; set; } 
        public virtual GroupScheduleShift GroupScheduleShift3           { get; set; }

        public void SetSchedulePropertiesToNull()
        {
            StartTime1 = null;
            StartTime2 = null;
            StartTime3 = null;
            
            EndTime1 = null;
            EndTime2 = null;
            EndTime3 = null;
            
            GroupScheduleShiftId1 = null;
            GroupScheduleShiftId2 = null;
            GroupScheduleShiftId3 = null;

            ClientDepartmentId1 = null;
            ClientDepartmentId2 = null;
            ClientDepartmentId3 = null;

            ClientCostCenterId1 = null;
            ClientCostCenterId2 = null;
            ClientCostCenterId3 = null;
        }
    }
}
