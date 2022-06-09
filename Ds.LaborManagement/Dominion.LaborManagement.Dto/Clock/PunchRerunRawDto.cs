namespace Dominion.LaborManagement.Dto.Clock
{
    public class PunchRerunRawDto : PunchRerunInfoDto
    {
        public string    FirstName            { get; set; }
        public string    LastName             { get; set; }
        public string    EmployeeNumber       { get; set; }
        public string    ClientName           { get; set; }
        public string    ClientCode           { get; set; }
    }
}