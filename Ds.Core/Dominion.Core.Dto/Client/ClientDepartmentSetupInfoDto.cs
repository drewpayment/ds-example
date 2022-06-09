using System.Collections.Generic;

namespace Dominion.Core.Dto.Client
{
    public class ClientDepartmentSetupInfoDto
    {
        public bool UseDepartmentsAcrossDivisions { get; set; }
        public Dictionary<int, string> DepartmentCodeLookup { get; set;}
    }
}