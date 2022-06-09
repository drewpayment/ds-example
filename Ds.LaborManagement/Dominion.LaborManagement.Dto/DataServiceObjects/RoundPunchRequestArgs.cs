using System;
using System.Data;

namespace Dominion.LaborManagement.Dto.DataServiceObjects
{
    /// <summary>
    /// Args classed used for rounding punches using the 
    /// <see cref="IDsDataServicesClockService"/>
    /// </summary>
    public class RoundPunchRequestArgs
    {
        public bool ShouldCheckGraceValues { get; set; } = true;
        public int ClientId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime PunchTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime StopTime { get; set; }
        public DataSet RulesDataSet { get; set; }
    }
}
