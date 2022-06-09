using System.Collections.Generic;

namespace Dominion.LaborManagement.Dto.Clock
{
    public class PunchRerunClientDto
    {
        public int    ClientId   { get; set; }
        public string ClientName { get; set; }
        public string ClientCode { get; set; }
        public IEnumerable<PunchRerunEmployeeDto> Employees { get; set; }
    }
}