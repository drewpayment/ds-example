using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Contact;

namespace Dominion.Core.Dto.Employee
{
    [Serializable]

    /// <summary>
    /// Represents a DTO that contains basic information about an employee.
    /// </summary>
    /// <remarks>
    /// This DTO contains mostly foreign keys for the employee and was created for 
    /// use with the time clock app 
    /// </remarks>
    public class EmployeeWorkDto
    {
        public int EmployeeId { get; set; }
        public int? DivisionId { get; set; }
        public string DivisionName { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? CostCenterId { get; set; }
        public string CostCenterName { get; set; }
        public string JobTitle { get; set; }
    }
}