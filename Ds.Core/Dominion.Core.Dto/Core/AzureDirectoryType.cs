using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Core
{
    public enum AzureDirectoryType : byte
    {
        CompanyFile            = 1,
        EmployeeFile           = 2,
        ApplicantFile          = 3,
        EmployeeOnboardingFile = 4,
        AdminFile              = 5,
    }
}
