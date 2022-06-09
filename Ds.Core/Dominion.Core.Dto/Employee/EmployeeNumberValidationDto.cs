namespace Dominion.Core.Dto.Employee
{
    public class EmployeeNumberValidationDto
    {
        public string EmployeeNumber { get; set; }
        public int?   EmployeeId     { get; set; }
        public int    ClientId       { get; set; }
        public bool?  IsValid        { get; set; }
    }
}