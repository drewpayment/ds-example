namespace Dominion.Core.Dto.Employee
{
    public class EmployeePaySearchResultDto
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeNumber { get; set; }
        public string EmailAddress { get; set; }
        public int? DepartmentId { get; set; }
        public int? DivisionId { get; set; }
        public int? CostCenterId { get; set; }
        public string EmploymentStatus { get; set; }
        public bool IsActive { get; set; }
    }
}
