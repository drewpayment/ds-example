using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Labor
{
    public enum DayShiftIndex : int
    {
        Schedule1 = 0,
        Schedule2 = 1,
        Schedule3 = 2

    }

    public class DayShiftDto
    {
        public DayShiftIndex DayShiftIndex { get; set; }
        public DateTime? ShiftStart { get; set; }
        public DateTime? ShiftEnd { get; set; }
        public int? ClientDepartmentId { get; set; }
        public string ClientDepartmentName { get; set; }
        public int? ClientCostCenterId { get; set; }
        public string ClientCostCenterName { get; set; }
        public bool HasClientDepartment { get { return (ClientDepartmentId.HasValue && ClientDepartmentId > 0); } }
        public bool HasClientCostCenter { get { return (ClientCostCenterId.HasValue && ClientCostCenterId > 0); } }
        public bool IsValid { get { return (ShiftStart.HasValue && ShiftEnd.HasValue); } }
        public string ShiftTimeDIsplayText
        {
            get
            {

                if (ShiftStart.HasValue && ShiftEnd.HasValue)
                    return (ShiftStart.Value.ToShortDateString().ToLower() + "-" + ShiftEnd.Value.ToShortDateString().ToLower());
                return "";
            }
        }


        public DayShiftDto ConstructShift(DataRowView row, DayShiftIndex index)
        {
            var shift = new DayShiftDto();

            shift.DayShiftIndex = index;

            if (!DBNull.Value.Equals(row[IndexedColumnName("StartTime", index)]))
                shift.ShiftStart = DateTime.Parse(row[IndexedColumnName("StartTime", index)].ToString());
            else
                shift.ShiftStart = null;

            if (!DBNull.Value.Equals(row[IndexedColumnName("StopTime", index)]))
                shift.ShiftEnd = DateTime.Parse(row[IndexedColumnName("StopTime", index)].ToString());
            else
                shift.ShiftEnd = null;

            if (!DBNull.Value.Equals(row[IndexedColumnName("ClientDepartmentID", index)]))
                shift.ClientDepartmentId = Int32.Parse(row[IndexedColumnName("ClientDepartmentID", index)].ToString());
            else
                shift.ClientDepartmentId = null;

            if (!DBNull.Value.Equals(row[IndexedColumnName("DepartmentName", index)]))
                shift.ClientDepartmentName = row[IndexedColumnName("DepartmentName", index)].ToString();
            else
                shift.ClientDepartmentName = "";

            if (!DBNull.Value.Equals(row[IndexedColumnName("ClientCostCenterID", index)]))
                shift.ClientCostCenterId = Int32.Parse(row[IndexedColumnName("ClientCostCenterID", index)].ToString());
            else
                shift.ClientCostCenterId = null;

            if (!DBNull.Value.Equals(row[IndexedColumnName("CostCenterName", index)]))
                shift.ClientCostCenterName = row[IndexedColumnName("CostCenterName", index)].ToString();
            else
                shift.ClientCostCenterName = "";

            return shift;
        }

        private string IndexedColumnName(string columnName, DayShiftIndex index)
        {
            if (index == DayShiftIndex.Schedule1)
            {
                return columnName;
            }
            else
            {
                return (columnName + "_Schedule" + ((int)index + 1));
            }
        }
    }

    public class DayScheduleDto
    {
        public int ClockEmployeeScheduleId { get; set; }
        public DateTime EventDate { get; set; }
        public int ClientId { get; set; }
        public int EmployeeId { get; set; }
        public DayShiftDto Shift1 { get; set; }
        public DayShiftDto Shift2 { get; set; }
        public DayShiftDto Shift3 { get; set; }
        public DateTime? StartTime_Schedule1 { get; set; }
        public DateTime? StopTime_Schedule1 { get; set; }
        public DateTime? StartTime_Schedule2 { get; set; }
        public DateTime? StopTime_Schedule2 { get; set; }
        public DateTime? StartTime_Schedule3 { get; set; }
        public DateTime? StopTime_Schedule3 { get; set; }

        public Double Hours { get; set; }


        public IEnumerable<DayShiftDto> ValidShifts
        {
            get
            {
                var tShifts = new List<DayShiftDto>();

                if ((Shift1 != null) && Shift1.IsValid)
                    tShifts.Add(Shift1);
                if ((Shift2 != null) && Shift2.IsValid)
                    tShifts.Add(Shift2);
                if ((Shift3 != null) && Shift3.IsValid)
                    tShifts.Add(Shift3);

                return tShifts;
            }
        }


        public DayScheduleDto ConstructSchedule(DataRowView row)
        {
            DateTime? startTime_Schedule1= parseDaySchedule(row["StartTime"]);
            DateTime? stopTime_Schedule1 = parseDaySchedule(row["StopTime"]);
            DateTime? startTime_Schedule2 = parseDaySchedule(row["StartTime_Schedule2"]);
            DateTime? stopTime_Schedule2 = parseDaySchedule(row["StopTime_Schedule2"]);
            DateTime? startTime_Schedule3 = parseDaySchedule(row["StartTime_Schedule3"]);
            DateTime? stopTime_Schedule3 = parseDaySchedule(row["StopTime_Schedule3"]);

            var schedule = new DayScheduleDto
            {

                ClockEmployeeScheduleId = Int32.Parse(row["ClockEmployeeScheduleID"].ToString()),
                EventDate               = DateTime.Parse(row["EventDate"].ToString()),
                EmployeeId              = Int32.Parse(row["EmployeeID"].ToString()),
                ClientId                = Int32.Parse(row["ClientID"].ToString()),
                Shift1 = new DayShiftDto().ConstructShift(row, DayShiftIndex.Schedule1),
                Shift2 = new DayShiftDto().ConstructShift(row, DayShiftIndex.Schedule2),
                Shift3 = new DayShiftDto().ConstructShift(row, DayShiftIndex.Schedule3),
                StartTime_Schedule1     = startTime_Schedule1,
                StopTime_Schedule1      = stopTime_Schedule1,
                StartTime_Schedule2     = startTime_Schedule2,
                StopTime_Schedule2      = stopTime_Schedule2,
                StartTime_Schedule3     = startTime_Schedule3,
                StopTime_Schedule3      = stopTime_Schedule3,
                Hours                   = getHours((startTime_Schedule1 == null) ? default(DateTime) : (DateTime)startTime_Schedule1, (stopTime_Schedule1 == null) ? default(DateTime) : (DateTime)stopTime_Schedule1) +
                                          getHours((startTime_Schedule2 == null) ? default(DateTime) : (DateTime)startTime_Schedule2, (stopTime_Schedule2 == null) ? default(DateTime) : (DateTime)stopTime_Schedule2) +
                                          getHours((startTime_Schedule3 == null) ? default(DateTime) : (DateTime)startTime_Schedule3, (stopTime_Schedule3 == null) ? default(DateTime) : (DateTime)stopTime_Schedule3),
            }; 

            return schedule;
        }

        public double getHours(DateTime startTime , DateTime stopTime) 
        {
            DateTime stop = stopTime;
            //If stopTime is less than startTime from faulty input, add a day to stopTime to get correct calculation of total hours
            if(DateTime.Compare(startTime, stopTime) > 0)
            {
                stop = stopTime.AddDays(1.0);
            }
            TimeSpan hoursInSchedule = stop - startTime;
            return hoursInSchedule.TotalHours;
        }

        public DateTime? parseDaySchedule(object daySchedule)
        {
            if (DBNull.Value.Equals(daySchedule))
                return null;
            return DateTime.Parse(daySchedule.ToString());
        }

    }
}
