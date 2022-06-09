using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Employee;
using Dominion.LaborManagement.Dto.Scheduling;
using Dominion.Utility.Mapping;

namespace Dominion.LaborManagement.Service.Mapping
{
    public class ClockEmployeePunchMaps
    {
        public class ClockEmployeePunchMap : 
            ExpressionMapper<ClockEmployeePunch, ClockEmployeePunchDto>
        {
            public override Expression<Func<ClockEmployeePunch, ClockEmployeePunchDto>> MapExpression => p => new ClockEmployeePunchDto
            {
                EmployeeId = p.EmployeeId,
                EmployeeFirstName = p.Employee.FirstName,
                EmployeeLastName = p.Employee.LastName,
                ModifiedPunch = p.ModifiedPunch,
                RawPunch = p.RawPunch,
                ClientCostCenterId = p.ClientCostCenterId,
                ClientDivisionId = p.ClientDivisionId,
                ClientDepartmentId = p.ClientDepartmentId,
                ClientShiftId = p.ClientShiftId,
                RawPunchBy = p.RawPunchBy,
                ClockEmployeePunchTypeId = p.ClockEmployeePunchTypeId,
                ClockClientLunchId = p.ClockClientLunchId,
                IsPaid = p.IsPaid,
                Comment = p.Comment,
                ShiftDate = p.ShiftDate,
                IsManualShiftOverride = p.IsManualShiftOverride,
                TimeZoneId = p.TimeZoneId,
                ClockName = p.ClockName,
                TransferOption = p.TransferOption,
                IsStopAutoLunch = p.IsStopAutoLunch,
                ClientId = p.ClientId,
                ClockEmployeeScheduleId = p.ClockEmployeeScheduleId,
                ScheduleNumber = p.ScheduleNumber,
                ClockEmployeePunchId = p.ClockEmployeePunchId,
                EmployeeComment = p.EmployeeComment,
                ClockEmployeePunchLocationId = p.ClockEmployeePunchLocationID,
                ClockEmployeePunchLocationLat = p.ClockEmployeePunchLocation.Latitude,
                ClockEmployeePunchLocationLng = p.ClockEmployeePunchLocation.Longitude
            };
        }
    }
}
