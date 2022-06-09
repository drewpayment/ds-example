using System;

namespace Dominion.Core.Dto.Labor
{
    public class ClockTimeCardDto
    {
        public int RowTypeId { get; set; }
        public int dayAmount { get; set; }
        public string Day { get; set; }
        public string Date { get; set; }
        public string In { get; set; }
        public string InRaw { get; set; }
        public DateTime InDateTime { get; set; }
        public string InEmployeePunchId { get; set; }
        public string InClockClientLunchId { get; set; }
        public string InTimeZoneId { get; set; }
        public string Out { get; set; }
        public string OutRaw { get; set; }
        public DateTime OutDateTime { get; set; }
        public string OutEmployeePunchId { get; set; }
        public string OutClockClientLunchId { get; set; }
        public string OutTimeZoneId { get; set; }
        public string In2 { get; set; }
        public string In2Raw { get; set; }
        public DateTime In2DateTime { get; set; }
        public string In2EmployeePunchId { get; set; }
        public string In2ClockClientLunchId { get; set; }
        public string In2TimeZoneId { get; set; }
        public string Out2 { get; set; }
        public string Out2Raw { get; set; }
        public DateTime Out2DateTime { get; set; }
        public string Out2EmployeePunchId { get; set; }
        public string Out2ClockClientLunchId { get; set; }
        public string Out2TimeZoneId { get; set; }
        public string Hours { get; set; }
        public double HoursDouble { get; set; }
        public Boolean HasRequestedPunch { get; set; }
        public string Exceptions { get; set; }
        public string Notes { get; set; }
        public DayScheduleDto DayScheduleDto { get; set; }

        public bool InHasException { get; set; }
        public bool In2HasException { get; set; }
        public bool OutHasException { get; set; }
        public bool Out2HasException { get; set; }


        public bool InIsMissing { get; set; }
        public bool In2IsMissing { get; set; }
        public bool OutIsMissing { get; set; }
        public bool Out2IsMissing { get; set; }

        public bool OutIsPending { get; set; }
        public bool Out2IsPending { get; set; }

        
        public string OutUrl     { get; set; }
        public string Out2Url    { get; set; }
        public bool   OutHasUrl  { get; set; }
        public bool   Out2HasUrl { get; set; }

        public bool IsWorkedHours { get; set; }


    }
}
