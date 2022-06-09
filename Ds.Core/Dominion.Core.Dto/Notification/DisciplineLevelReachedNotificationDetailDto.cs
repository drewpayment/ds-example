using System;

namespace Dominion.Core.Dto.Notification
{
    [Serializable]
    public class DisciplineLevelReachedNotificationDetailDto
    {
        public int     ClientId            { get; set; }
        public int     EmployeeId          { get; set; }
        public string  EmployeeName        { get; set; }
        public int     DisciplineLevelId   { get; set; }
        public double  PointLevel          { get; set; }
        public string  DisciplineLevelName { get; set; }
    }
}
