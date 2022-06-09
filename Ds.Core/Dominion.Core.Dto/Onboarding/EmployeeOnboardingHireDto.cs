using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Onboarding
{
    [Serializable]
    public class EmployeeOnboardingHireDto
    {
        public int EmployeeId { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeName { get; set; }
        public int ClientId { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? RehireDate { get; set; }
        public string ClientName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int UserId { get; set; }
        public bool? UserAddedDuringOnboarding { get; set; }
    }
}
