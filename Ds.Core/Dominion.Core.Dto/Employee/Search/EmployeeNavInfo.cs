namespace Dominion.Core.Dto.Employee.Search
{
    public class EmployeeNavInfo
    {
        public EmployeeSearchDto Current { get; set; }
        public EmployeeSearchDto Next    { get; set; }
        public EmployeeSearchDto Prev    { get; set; }
        public EmployeeSearchDto First   { get; set; }
        public EmployeeSearchDto Last    { get; set; }
    }
}