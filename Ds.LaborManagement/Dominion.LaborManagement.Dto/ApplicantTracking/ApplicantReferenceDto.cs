using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    [Serializable]
    public partial class ApplicantReferenceDto
    {
        public int ApplicantReferenceId { get; set; }
        public int ApplicantId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Relationship { get; set; }
        public int YearsKnown { get; set; }
        public bool IsEnabled { get; set; }
        public string Name
        {
            get
            {
                return this.FirstName + " " + this.LastName;

            }

            private set
            {

            }
        }
    }
}
